namespace jj.TATools.Editor
{
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using System.Reflection;

    public class ShaderVariantCollectionTool
    {
        #region MenuItems

#if SVC_DEBUG_ON
        [MenuItem(SVCSettings.TOOL_MENU_3)]
        static void BuildBundleDebug()
        {
            ShaderVariantCulledTool.StartCulledTool();

            List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

            foreach (var folder in SVCSettings.Instance.m_SearchInFolders)
            {
                var folderName = new DirectoryInfo(folder).Name;
                var shaderGuids = AssetDatabase.FindAssets("t:Shader", new string[] { folder });
                AssetBundleBuild build = new AssetBundleBuild();

                build.assetBundleName = folderName + "_shader.bundle";
                List<string> assetNameList = new List<string>();
                foreach (var guid in shaderGuids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    assetNameList.Add(assetPath);
                }
                var svcFile = folder + "/" + folderName + ".shadervariants";
                if (File.Exists(svcFile))
                    assetNameList.Add(svcFile);

                build.assetNames = assetNameList.ToArray();
                builds.Add(build);

                var sceneGuids = AssetDatabase.FindAssets("t:Scene", new string[] { folder });
                foreach (var guid in sceneGuids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    AssetBundleBuild build1 = new AssetBundleBuild();
                    build1.assetBundleName = Path.GetFileNameWithoutExtension(assetPath) + ".bundle";
                    build1.assetNames = new string[] { assetPath };
                    builds.Add(build1);
                }
            }

            var outputPath = Application.streamingAssetsPath;
            if (Directory.Exists(outputPath))
            {
                var files = Directory.GetFiles(outputPath, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                    File.Delete(file);
            }
            else
                Directory.CreateDirectory(outputPath);

            BuildPipeline.BuildAssetBundles(outputPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();

            ShaderVariantCulledTool.FinishCulledTool();
        }
#endif
        [MenuItem(SVCSettings.TOOL_MENU_1)]
        static void OneKeyCollectionShaderVariants()
        {
            var setting = SVCSettings.Instance;
            setting.ClearSVCFileCache();
            var searchInFolders = SVCSettings.Instance.m_SearchInFolders;
            for (int i = 0; i < searchInFolders.Count; i++)
            {
                var searchInFolder = searchInFolders[i];
                if (string.IsNullOrEmpty(searchInFolder)) continue;

                var svcFile = GenSvcInFolder(searchInFolder);
                if (!string.IsNullOrEmpty(svcFile))
                    setting.AddSVCFile(svcFile);
            }
        }

        //[MenuItem(SVCSettings.TOOL_MENU_2)]
        //static void OneKeyCheckShaderStandards()
        //{
        //    var setting = SVCSettings.Instance;
        //    CheckShaderStandards(SVCSettings.Instance.m_SearchInFolders.ToArray());
        //}

        #endregion

        #region Public Methods

        /// <summary>
        /// 按模块目录收集Shader变体
        /// </summary>
        /// <param name="folder">目录路径</param>
        public static string GenSvcInFolder(string folder) { 
            var svc = CollectionShaderVariantsInFolder(folder);
            if (svc == null) return null; // 未收集到shader variant

            var relativeFolder = folder.Replace("\\", "/").Replace(Application.dataPath, "Assets");
            if (relativeFolder.EndsWith("/"))
                relativeFolder = relativeFolder.Substring(0, relativeFolder.Length - 1);

            var folderName = new DirectoryInfo(folder).Name;
            string outputPath = string.Format("{0}/{1}{2}", relativeFolder, folderName, SVCSettings.SVC_EXTENSION);
            if (File.Exists(outputPath))
                AssetDatabase.DeleteAsset(outputPath);

            AssetDatabase.CreateAsset(svc, outputPath);

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            return outputPath;
        }

        /// <summary>
        /// 按模块目录收集Shader变体
        /// </summary>
        /// <param name="folder">模块模块：执行绝对、相对路径</param>
        /// <returns>返回svc对象</returns>
        public static ShaderVariantCollection CollectionShaderVariantsInFolder(string folder)
        {
            return CollectionShaderVariantsInFolder(folder, SVCSettings.Instance.DynamicKeywords);
        }

        /// <summary>
        /// 按模块目录收集Shader变体
        /// </summary>
        /// <param name="folder">模块模块：执行绝对、相对路径</param>
        /// <param name="dynamicKeywordList">
        /// 代码中需要根据相关业务逻辑动态开关设置的Keyword List(例如引用UGUI中关键字"UNITY_UI_CLIP_RECT")
        /// 规则：
        /// 1.声明关键字'#pragma multi_compile _ A1'-> 配置'A1';
        /// 2.声明关键字'#pragma multi_compile _ B1 B2 B3'-> 配置'空字符串|B1|B2|B3';
        /// 3.声明关键字'#pragma multi_compile C1 C2 C3'-> 配置'C1|C2|C3'.
        /// 样例：List<string> dynamicKeywordList = new List<string>(){"A1","|B1|B2|B3","C1|C2|C3"}
        /// </param>
        /// <returns>返回svc对象</returns>
        public static ShaderVariantCollection CollectionShaderVariantsInFolder(string folder,List<string> dynamicKeywordList)
        {
            if (!Directory.Exists(folder)) return null;

            var relativeFolder = folder.Replace("\\", "/").Replace(Application.dataPath, "Assets");
            if (relativeFolder.EndsWith("/"))
                relativeFolder = relativeFolder.Substring(0, relativeFolder.Length - 1);


            #region ### 000: 没有被材质引用的Shader(URP管线相关Shader除外)不予收集 #############################################################
            #endregion

            #region ### 001: 收集当前模块内URP 配置 ############################################################################################

            var renderPipelineList = SVCSettings.Instance.GetUniversalRenderPipelineAssetForModule(relativeFolder);

            #endregion

            #region ### 002: 遍历材质，按Shader分类 ############################################################################################
            Dictionary<Shader, List<Material>> shaderToMaterialMapping = new Dictionary<Shader, List<Material>>();
            List<Material> tempMatList = null;
            var allMaterialGUIDs = AssetDatabase.FindAssets("t:Material", new string[] { relativeFolder });
            var allCount = allMaterialGUIDs.Length;
            for (int k = 0; k < allCount; k++)
            {
                var guid = allMaterialGUIDs[k];
                var matPath = AssetDatabase.GUIDToAssetPath(guid);

                EditorUtility.DisplayProgressBar("[" + relativeFolder + "]-Collect Shader Per Material[" + k + "|" + allCount + "]", "Collect..." + matPath, (k + 1.0f) / allCount);

                Material material = AssetDatabase.LoadAssetAtPath<Material>(matPath);
                if (!SVCUtility.CheckMaterialMissShader(material))
                {
                    var realShader = material.shader;
                    if (!shaderToMaterialMapping.TryGetValue(realShader, out tempMatList))
                    {
                        tempMatList = new List<Material>();
                        shaderToMaterialMapping[realShader] = tempMatList;
                    }
                    tempMatList.Add(material);
                }
            }
            EditorUtility.ClearProgressBar();

            #endregion

            #region ### 003: 模块内VolumeProfile，LensFlareDataSRP配置 #########################################################################
            var hasTAA = false;
            var hasFXAA = false;
            var hasSMAA = false;
            var hasDithering = false;
            var hasUseScreenCoordOverride = false;
            // VolumeProfile In Module
            var volumeProfileType = URPBuiltinUtility.GetURPVolumeProfileType();
            List<string> allVolumeProfilePathList = new List<string>();
            var volumeProfileGuids = AssetDatabase.FindAssets("t:" + volumeProfileType.Name, new string[] { relativeFolder });
            foreach (var guid in volumeProfileGuids)
                allVolumeProfilePathList.Add(AssetDatabase.GUIDToAssetPath(guid));
            // LensFlareDataSRP In Module
            var lensFlareDataSRPType = URPBuiltinUtility.GetURPLensFlareDataSRPType();
            var lensFlareDataSRPGuids = AssetDatabase.FindAssets("t:" + lensFlareDataSRPType.Name, new string[] { relativeFolder });
            List<string> allLensFlarePathList = new List<string>();
            foreach (var guid in lensFlareDataSRPGuids)
                allLensFlarePathList.Add(AssetDatabase.GUIDToAssetPath(guid));

            #endregion

            #region ### 004: 遍历Prefab引用的模块外材质，按Shader分类 ##########################################################################
            var allPrefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { relativeFolder });
            allCount = allPrefabGUIDs.Length;
            for (int k = 0; k < allCount; k++)
            {
                var guid = allPrefabGUIDs[k];
                var prefabPath = AssetDatabase.GUIDToAssetPath(guid);

                EditorUtility.DisplayProgressBar("[" + relativeFolder + "]-Collect Shader Per Prefab[" + k + "|" + allCount + "]", "Collect..." + prefabPath, (k + 1.0f) / allCount);

                // Dep Materials Out Of Module
                var matDependencies = SVCUtility.GetDependencies<Material>(prefabPath, true);
                if (matDependencies != null && matDependencies.Count > 0)
                {
                    foreach (var mat in matDependencies)
                    {
                        var curShader = mat.shader;
                        if (shaderToMaterialMapping.TryGetValue(curShader, out tempMatList))
                        {
                            if (tempMatList.Contains(mat)) continue;
                        }

                        if (!SVCUtility.CheckMaterialMissShader(mat))
                        {
                            var realShader = mat.shader;
                            if (!shaderToMaterialMapping.TryGetValue(realShader, out tempMatList))
                            {
                                tempMatList = new List<Material>();
                                shaderToMaterialMapping[realShader] = tempMatList;
                            }
                            tempMatList.Add(mat);
                        }
                    }
                }

                //  Dep Volume Profiles Out Of Module
                var prefabDepenendVolumeProfiles = SVCUtility.GetDependencies(volumeProfileType, prefabPath, true);
                foreach (var vp in prefabDepenendVolumeProfiles)
                {
                    if (!allVolumeProfilePathList.Contains(vp))
                        allVolumeProfilePathList.Add(vp);
                }
                //Dep LensFlaredDataSRP Out Of Module
                var lensFlareDataSRPs = SVCUtility.GetDependencies(lensFlareDataSRPType, prefabPath, true);
                foreach (var lf in lensFlareDataSRPs)
                {
                    if (!allLensFlarePathList.Contains(lf))
                        allLensFlarePathList.Add(lf);
                }
                // Camera
                if (!(hasFXAA && hasSMAA && hasTAA && hasDithering && hasUseScreenCoordOverride))
                {
                    var prefabGO = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                    var cameras = prefabGO.GetComponentsInChildren<Camera>(true);
                    foreach (var camera in cameras)
                    {
                        if (hasFXAA && hasSMAA && hasTAA && hasDithering && hasUseScreenCoordOverride)
                            break;

                        var comps = camera.GetComponents<MonoBehaviour>();
                        foreach (var comp in comps)
                        {
                            var compType = comp.GetType();
                            if (compType.FullName == URPBuiltinUtility.UniversalAdditionalCameraDataTypeName)
                            {
                                var cameraTypeField = compType.GetField("m_CameraType", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                var cameraTypeFieldValue = (int)cameraTypeField.GetValue(comp); // 0-Base,1-Overlay
                                if (cameraTypeFieldValue == 0)
                                {
                                    var antialiasingField = compType.GetField("m_Antialiasing", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                    var antialiasingFieldValue = (int)antialiasingField.GetValue(comp); //  0-None,1-FastApproximateAntialiasing,2-SubpixelMorphologicalAntiAliasing,3-TemporalAntiAliasing,
                                    if (antialiasingFieldValue == 1)
                                        hasFXAA = true;
                                    else if (antialiasingFieldValue == 2)
                                        hasSMAA = true;
                                    else if (antialiasingFieldValue == 3)
                                        hasTAA = true;
                                    var ditheringField = compType.GetField("m_Dithering", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                    var ditheringFieldValue = (bool)ditheringField.GetValue(comp);
                                    if (ditheringFieldValue)
                                        hasDithering = true;
                                    var useScreenCoordOverrideField = compType.GetField("m_UseScreenCoordOverride", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                    var useScreenCoordOverrideFieldValue = (bool)useScreenCoordOverrideField.GetValue(comp);
                                    if (useScreenCoordOverrideFieldValue)
                                        hasUseScreenCoordOverride = true;
                                }
                            }

                            if (hasFXAA && hasSMAA && hasTAA && hasDithering && hasUseScreenCoordOverride)
                                break;
                        }
                    }
                }

            }
            EditorUtility.ClearProgressBar();

            #endregion

            #region ### 005: 遍历场景,收集Shader相关GI、Fog信息 & 引用模块外材质 & 引用模块外URP内置资源########################################
            Dictionary<Shader, Dictionary<Material, List<SceneInfo>>> shaderToSceneInfoMapping = new Dictionary<Shader, Dictionary<Material, List<SceneInfo>>>();
            Dictionary<Material, List<SceneInfo>> tempSceneInfoDic = null;
            List<SceneInfo> tempSceneInfoList = null;
            string activeSceneBeforePath = EditorSceneManager.GetActiveScene().path;
            var allSceneGuids = AssetDatabase.FindAssets("t:Scene", new string[] { relativeFolder });
            foreach (var guid in allSceneGuids)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guid);

                // Volume Profiles Out Of Module  //////////////////////////////////
                var prefabDepenendVolumeProfiles = SVCUtility.GetDependencies(volumeProfileType, scenePath, true);
                foreach (var vp in prefabDepenendVolumeProfiles)
                {
                    if (!allVolumeProfilePathList.Contains(vp))
                        allVolumeProfilePathList.Add(vp);
                }
                // LensFlareDataSRP Out Of Module //////////////////////////////////
                var lensFlareDataSRPs = SVCUtility.GetDependencies(lensFlareDataSRPType, scenePath, true);
                foreach (var lf in lensFlareDataSRPs)
                {
                    if (!allLensFlarePathList.Contains(lf))
                        allLensFlarePathList.Add(lf);
                }

                try
                {
                    Scene currentScene = EditorSceneManager.OpenScene(scenePath);
                    // Renderer Ref Material  /////////////////////////////////////////////
                    // Directional &  Shadow
                    var lights = UnityEngine.Object.FindObjectsOfType<Light>();
                    float maxDirLightIntensity = 0;
                    Light mainDirLight = null;
                    foreach (var light in lights)
                    {
                        if (light.type == LightType.Directional && light.lightmapBakeType != LightmapBakeType.Baked)
                        {
                            if (light.intensity > maxDirLightIntensity)
                            {
                                maxDirLightIntensity = light.intensity;
                                mainDirLight = light;
                            }
                        }
                    }

                    var renderers = currentScene.GetRootGameObjects().SelectMany(gameObject => gameObject.GetComponentsInChildren<Renderer>()).Distinct().ToArray();
                    foreach (var renderer in renderers)
                    {
                        foreach (var m in renderer.sharedMaterials)
                        {
                            if (m == null || SVCUtility.CheckMaterialMissShader(m)) continue;

                            var shader = m.shader;
                            // var matPath = AssetDatabase.GetAssetPath(m);
                            //var shaderPath = AssetDatabase.GetAssetPath(shader);
                            // 收集场景引用非本模块资源
                            // if (!File.Exists(matPath) || !matPath.StartsWith(folder) || !File.Exists(shaderPath) || !shaderPath.StartsWith(folder)) continue;

                            // 收集场景Renderer引用模块外得材质
                            if (!shaderToMaterialMapping.TryGetValue(shader, out tempMatList))
                            {
                                tempMatList = new List<Material>();
                                shaderToMaterialMapping[shader] = tempMatList;
                            }
                            if (!tempMatList.Contains(m))
                                tempMatList.Add(m);

                            // 收集 GI、Fog信息
                            SceneInfo sceneInfo = new SceneInfo();
                            if (mainDirLight != null)
                            {
                                sceneInfo.m_MainDirLight = true;
                                sceneInfo.m_MainDirLightWithShadow = mainDirLight.shadows != LightShadows.None;
                                sceneInfo.m_MainDirLightWithSoftShadow = mainDirLight.shadows == LightShadows.Soft;
                            }
                            if (!shaderToSceneInfoMapping.TryGetValue(shader, out tempSceneInfoDic))
                            {
                                tempSceneInfoDic = new Dictionary<Material, List<SceneInfo>>();
                                shaderToSceneInfoMapping[shader] = tempSceneInfoDic;
                            }

                            if (!tempSceneInfoDic.TryGetValue(m, out tempSceneInfoList))
                            {
                                tempSceneInfoList = new List<SceneInfo>();
                                tempSceneInfoDic[m] = tempSceneInfoList;
                            }

                            tempSceneInfoList.Add(sceneInfo);

                            if (SVCUtility.HasStaticEditorFlags(renderer.gameObject, StaticEditorFlags.ContributeGI) && (renderer.lightmapIndex >= 0 || renderer.realtimeLightmapIndex >= 0))
                            {
                                sceneInfo.m_ExistContributeGITrue = true;
                            }
                            else
                            {
                                sceneInfo.m_ExistContributeGIFalse = true;
                            }
                        }
                    }

                    // Compenent Ref Material  Out Of Module /////////////////////////////////////////////
                    var matDependencies = SVCUtility.GetDependencies<Material>(scenePath,true);
                    foreach (var depMat in matDependencies)
                    {
                        if (SVCUtility.CheckMaterialMissShader(depMat)) continue;

                        var shader = depMat.shader;

                        // 收集场景非Renderer引用模块外得材质
                        if (!shaderToMaterialMapping.TryGetValue(shader, out tempMatList))
                        {
                            tempMatList = new List<Material>();
                            shaderToMaterialMapping[shader] = tempMatList;
                        }
                        if (!tempMatList.Contains(depMat))
                            tempMatList.Add(depMat);
                    }

                    // Camera ////////////////////////////////////////////
                    if (!(hasFXAA && hasSMAA && hasTAA && hasDithering && hasUseScreenCoordOverride))
                    {
                        var cameras = currentScene.GetRootGameObjects().SelectMany(gameObject => gameObject.GetComponentsInChildren<Camera>()).Distinct().ToArray();
                        foreach (var camera in cameras)
                        {
                            if (hasFXAA && hasSMAA && hasTAA && hasDithering && hasUseScreenCoordOverride)
                                break;

                            var comps = camera.GetComponents<MonoBehaviour>();
                            foreach (var comp in comps)
                            {
                                var compType = comp.GetType();
                                if (compType.FullName == URPBuiltinUtility.UniversalAdditionalCameraDataTypeName)
                                {
                                    var cameraTypeField = compType.GetField("m_CameraType", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                    var cameraTypeFieldValue = (int)cameraTypeField.GetValue(comp); // 0-Base,1-Overlay
                                    if (cameraTypeFieldValue == 0)
                                    {
                                        var antialiasingField = compType.GetField("m_Antialiasing", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                        var antialiasingFieldValue = (int)antialiasingField.GetValue(comp); //  0-None,1-FastApproximateAntialiasing,2-SubpixelMorphologicalAntiAliasing,3-TemporalAntiAliasing,
                                        if (antialiasingFieldValue == 1)
                                            hasFXAA = true;
                                        else if (antialiasingFieldValue == 2)
                                            hasSMAA = true;
                                        else if (antialiasingFieldValue == 3)
                                            hasTAA = true;
                                        var ditheringField = compType.GetField("m_Dithering", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                        var ditheringFieldValue = (bool)ditheringField.GetValue(comp);
                                        if (ditheringFieldValue)
                                            hasDithering = true;
                                        var useScreenCoordOverrideField = compType.GetField("m_UseScreenCoordOverride", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                                        var useScreenCoordOverrideFieldValue = (bool)useScreenCoordOverrideField.GetValue(comp);
                                        if (useScreenCoordOverrideFieldValue)
                                            hasUseScreenCoordOverride = true;
                                    }
                                }

                                if (hasFXAA && hasSMAA && hasTAA && hasDithering && hasUseScreenCoordOverride)
                                    break;
                            }
                        }
                    }
                }
                catch (System.Exception)
                {
                    Debug.LogError("SVCTools-000-Open Scene Error:" + scenePath);
                    continue;
                }
            }

            if (!string.IsNullOrEmpty(activeSceneBeforePath))
            {
                EditorSceneManager.OpenScene(activeSceneBeforePath);
            }
            #endregion

            #region ### 006: Per SubShader & Per Pass 收集材质中Keyword组合 ####################################################################
            var dynamicKeywords = dynamicKeywordList.Distinct().ToList<string>();// m_DynamicKeywords.Where((x, i) => dynamicKeywords.FindIndex(z => z == x) == i).ToList();//校验配置中动态关键字是否重复，剔除重复关键字
            var allDynamicKeywords = SVCUtility.GetAllDynamicKeywords(dynamicKeywords);
            Dictionary<Shader, ShaderParseData> sourceShaderParseDataMapping = new Dictionary<Shader, ShaderParseData>();
            List<Dictionary<Shader, ShaderParseData>> shaderParseDataMappingPerURPSetting = new List<Dictionary<Shader, ShaderParseData>>();
            shaderParseDataMappingPerURPSetting.Add(sourceShaderParseDataMapping);
            int index = 0;
            int count = shaderToMaterialMapping.Count;
            foreach (var data in shaderToMaterialMapping)
            {
                Shader s = data.Key;

                EditorUtility.DisplayProgressBar("[" + relativeFolder + "]-Collect Keyword Per Material[" + index + "|" + count + "]", "Collect..." + s, (index + 1.0f) / count);

                ShaderParseData spData = ShaderParseData.CreateData(s);
                spData.SetRenderPipeline(renderPipelineList.Count > 0 ? renderPipelineList[0] : null);
                sourceShaderParseDataMapping.Add(s, spData);

                foreach (var mat in data.Value)
                {
                    List<string> materialKWS = new List<string>(mat.shaderKeywords);
                    materialKWS = materialKWS.Distinct().ToList<string>();
                    materialKWS.Sort((x, y) => string.CompareOrdinal(x, y));
                    // 材质预制的关键字包含动态关键字需要剔除关键字
                    if (materialKWS.Count > 0 && allDynamicKeywords != null && allDynamicKeywords.Count > 0)
                    {
                        for (int i = materialKWS.Count - 1; i >= 0; i--)
                        {
                            if (dynamicKeywords.Contains(materialKWS[i]))
                            {
                                materialKWS.RemoveAt(i);
                            }
                        }
                    }

                    // Instancing
                    if (mat.enableInstancing)
                    {
                        materialKWS.Add(SVCSettings.UNITY_KW_INSTANCING_ON);
                    }

                    // Collect Material Keyword
                    materialKWS.Sort((x, y) => string.CompareOrdinal(x, y));
                    string combineStr = "";
                    foreach (var kw in materialKWS)
                    {
                        combineStr += kw + "|";
                    }
                    if (!string.IsNullOrEmpty(combineStr))
                        combineStr = combineStr.Substring(0, combineStr.Length - 1);

                    spData.AddMaterialVariantKeywordGroup(combineStr, mat);
                }
                EditorUtility.ClearProgressBar();
            }
            #endregion

            #region ### 007: Per SubShader & Per Pass 收集配置中动态Keyword组合(未被预制到材质中，运行时动态开关) ##############################
            if (dynamicKeywords != null && dynamicKeywords.Count > 0)
            {
                // 收集配置中动态Keyword
                var dynamicAllCombineKWS = SVCUtility.GetCombination(dynamicKeywords.ToArray());
                foreach (var spData in sourceShaderParseDataMapping.Values)
                {
                    foreach (var group in dynamicAllCombineKWS)
                    {
                        spData.AddDynamicVariantKeywordGroup(group);
                    }
                }
            }
            #endregion

            #region ### 008: Per SubShader & Per Pass 合并材质Keyword与动态Keyword组合 #########################################################
            foreach (var spData in sourceShaderParseDataMapping.Values)
            {
                spData.CombineMaterialAndDynamicVariant();
            }
            #endregion

            #region ### 009: Per SubShader & Per Pass 合并Unity内置GI、Fog Keyword组合 #########################################################

            // Clone
            var sourceMapping = shaderParseDataMappingPerURPSetting[0];
            for (int i = 1; i < renderPipelineList.Count; i++)
            {
                var cloneShaderParseDataMapping = new Dictionary<Shader, ShaderParseData>();
                foreach (var data in sourceMapping)
                {
                    cloneShaderParseDataMapping[data.Key] = data.Value.Clone(renderPipelineList[i]);
                }
                shaderParseDataMappingPerURPSetting.Add(cloneShaderParseDataMapping);
            }

            // Combine Keywords
            foreach (var data in shaderParseDataMappingPerURPSetting)
            {
                foreach (var childData in data)
                {
                    Dictionary<Material, List<SceneInfo>> sceneInfoDic = null;
                    shaderToSceneInfoMapping.TryGetValue(childData.Key, out sceneInfoDic);
                    childData.Value.CombineBuiltinKWVariant(sceneInfoDic);
                }
            }

            #endregion

            #region ### 010:Build URP Builtin SV ################################################################################################

            List<ShaderVariantCollection.ShaderVariant> urpSVList = null;
            if (!(allSceneGuids.Length == 0 && allVolumeProfilePathList.Count == 0 && allLensFlarePathList.Count == 0))
                urpSVList = URPBuiltinUtility.GenerateSvForModule(relativeFolder,allVolumeProfilePathList,allLensFlarePathList,hasTAA,hasFXAA,hasSMAA,hasDithering,hasUseScreenCoordOverride);

            #endregion

            #region ### 011: Build SVC ##########################################################################################################
            ShaderVariantCollection finalSVC = new ShaderVariantCollection();
            int stepIndex = 0;
            foreach (var data in shaderParseDataMappingPerURPSetting)
            {
                foreach (var childData in data)
                {
                    Shader s = childData.Key;
                    string shaderPath = AssetDatabase.GetAssetPath(s);

                    foreach (var subshader in childData.Value.m_DataMapping.Values)
                    {
                        foreach (var pass in subshader.m_DataMapping.Values)
                        {
                            if (PassTypeDefine.PassTypeInvalid(pass.m_PassType)) continue;

                            foreach (var combineData in pass.m_CombineKeywords)
                            {
                                var list = combineData.Value;
                                if (list.Count == 0)
                                {
                                    try
                                    {
                                        ShaderVariantCollection.ShaderVariant sv = new ShaderVariantCollection.ShaderVariant(s, pass.m_PassType, "");
                                        if (!finalSVC.Contains(sv))
                                            finalSVC.Add(sv);
                                    }
                                    catch (System.Exception e)
                                    {
                                        // TODO
                                    }
                                }
                                else
                                {
                                    var temp = list.Distinct().ToArray();
                                    foreach (var variant in temp)
                                    {
                                        try
                                        {
                                            ShaderVariantCollection.ShaderVariant sv = new ShaderVariantCollection.ShaderVariant(s, pass.m_PassType, variant);
                                            if (!finalSVC.Contains(sv))
                                                finalSVC.Add(sv);
                                        }
                                        catch (System.Exception e)
                                        {
                                            // TODO
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                stepIndex++;
            }

            #endregion

            #region ### 012: Add URP SV To SVC ##################################################################################################
        
            if (urpSVList != null)
            {
                foreach (var sv in urpSVList)
                {
                    if (!finalSVC.Contains(sv))
                        finalSVC.Add(sv);
                }
            }

            if (finalSVC.variantCount == 0)
                return null;

            return finalSVC;

            #endregion
        }

        /// <summary>
        /// 检查Shader书写规范
        /// </summary>
        /// <param name="folders"></param>
        /// <param name="outputFile"></param>
        public static void CheckShaderStandards(string[] folders, string outputFile = "")
        {
            if (folders == null || folders.Length == 0) return;

            var shaderGuids = AssetDatabase.FindAssets("t:Shader", folders);
            string outputStr = "";
            for (int i = 0; i < shaderGuids.Length; i++)
            {
                var shaderPath = AssetDatabase.GUIDToAssetPath(shaderGuids[i]);
                var shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);
                if (shader == null) continue;

                string errorStr = "";
                bool surface = SVCUtility.HasSurfaceShaders(shader);
                if (surface)
                {
                    outputStr += shaderPath + "\n\t不允许使用SurfaceShader,需要转成VF Shader!!!\n";
                }
            }

            var finalOutputFile = string.IsNullOrEmpty(outputFile) ? "ShaderStandards-Error.txt" : outputFile;
            StreamWriter sw = new StreamWriter(finalOutputFile, false, System.Text.Encoding.UTF8);
            sw.Write(outputStr);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        #endregion
    }

 }