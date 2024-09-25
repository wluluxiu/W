using System.IO;
using System.Collections.Generic;

namespace jj.TATools.Editor
{
    internal enum ExceptionType
    {
        NONE = -1,
        // Texture
        TEXTURE_RW = 0,
        TEXTURE_FORMAT,
        TEXTURE_RESOLUTION_NPOT,
        TEXTURE_RESOLUTION_SIZE,
        TEXTURE_FILTERMODE,
        TEXTURE_ANISOLEVEL,
        TEXTURE_NOUSED,
        // Model & Mesh
        MODEL_RW,
        MODEL_MESH_TRIANGLE_LIMIT,
        MODEL_MESH_VERTEX_LIMIT,
        MODEL_MESH_BLENDSHAPE_LIMIT,
        MODEL_MESH_VC_LIMIT,
        MODEL_MESH_UV_NOUSED,
        MODEL_MESH_UV2_AUTO,
        MODEL_BONE_LIMIT,
        MODEL_ANIM_COMPRESSION,
        MODEL_NOUSED,
        // Material
        MATERIAL_MISS_SHADER,
        MATERIAL_BUILTIN_REF,
        MATERIAL_NO_USED_DATA,
        MATERIAL_NO_USED,
        // Shader
        SHADER_DEP,
        SHADER_NO_USED,
        // Prefab
        PREFAB_MISS_SCRIPT,
        PREFAB_COLLIDER,
        PREFAB_BUILTIN_REF,
        PREFAB_MISS_MAT,
        // Scene
        SCENE_MISS_SCRIPT,
        SCENE_COLLIDER,
        SCENE_BUILTIN_REF,
        // Script
        SCRIPT_DEP,
        // AnimatorController & AnimationClip
        ANIMATORCONTROLLER_NOUSED,
        // AuidoClip & VedioClip
        AUDIO_VIDEO_NOUSED,
        // Repeat Files
        REPEAT_FILE,
        // Resources Folder
        RESOURCES_FOLDER,
        // Assetbundle Strategy
        ASSETBUNDLE_STRATEGY,
        // Assetbunle Builtin Res
        ASSETBUNDLE_BUILTIN
    }

    internal class AssetRecordDataParser
    {
        #region Fields

        internal static List<EAssetType> AssetTypeMapping { get; set; }

        internal static Dictionary<EAssetType, Dictionary<string, BaseRecorder>> LastVerisonFilesMapping { get; set; }
        internal static Dictionary<EAssetType, Dictionary<string, BaseRecorder>> CurrentVerisonFilesMapping { get; set; }

        internal static Dictionary<EAssetType, Dictionary<string, BaseRecorder>> CurrentVersionDeletedMapping { get; set; }
        internal static Dictionary<EAssetType, Dictionary<string, BaseRecorder>> CurrentVersionAddedMapping { get; set; }
        internal static Dictionary<EAssetType, Dictionary<string, BaseRecorder>> CurrentVersionModifiedMapping { get; set; }

        internal static Dictionary<EAssetType, Dictionary<string, Dictionary<string, BaseRecorder>>> CurrentRepeatFilesExceptionMapping { get; set; }

        internal static Dictionary<EAssetType, Dictionary<string, BaseRecorder>> CurrentNoRefMapping { get; set; }

        internal static Dictionary<string, Dictionary<EAssetType, Dictionary<string, BaseRecorder>>> CurrentCustomDirFilesMapping { get; set; }

        internal static int LastTotalFileAmount { get; set; }
        internal static int CurrentTotalFileAmount { get; set; }

        internal static Dictionary<EAssetType, long> LastFilesSizeMapping { get; set; }
        internal static Dictionary<EAssetType, long> CurrentFilesSizeMapping { get; set; }
        internal static Dictionary<EAssetType, long> CurrentAddedFilesSizeMapping { get; set; }
        internal static Dictionary<EAssetType, long> CurrentBeforeModifiedMemorySizeMapping { get; set; }
        internal static Dictionary<EAssetType, long> CurrentBeforeModifiedFilesSizeMapping { get; set; }
        internal static Dictionary<EAssetType, long> CurrentAfterModifiedMemorySizeMapping { get; set; }
        internal static Dictionary<EAssetType, long> CurrentAfterModifiedFilesSizeMapping { get; set; }
        internal static Dictionary<EAssetType, long> CurrentDeletedFilesSizeMapping { get; set; }
        internal static Dictionary<EAssetType, long> CurrentNoRefFilesSizeMapping { get; set; }
        internal static Dictionary<EAssetType, long> CurrentRepeatFilesSizeMapping { get; set; }

        internal static Dictionary<string, Dictionary<EAssetType, long>> CurrentCustomDirFilesSizeMapping { get; set; }
        internal static Dictionary<string, int> CurrentCustomDirFilesAmountMapping { get; set; }

        internal static List<string> CurrentWhiteListDynamicLoaded { get; set; }
        // internal static Dictionary<string, string> CurrentCustomDirRuleMapping { get; set; }

        internal static Dictionary<EAssetType, Dictionary<string, BaseRecorder>> FileInResourcesFolderMapping { get; set; }
        internal static Dictionary<EAssetType, long> FilesInResourcesFolderSizeMapping { get; set; }

        internal static Dictionary<EAssetType, Dictionary<string, BaseRecorder>> BuiltinDependicesFileMapping { get; set; }
        internal static Dictionary<EAssetType, long> BuiltinDependicesFileSizeMapping { get; set; }

        internal static Dictionary<EAssetType, Dictionary<ExceptionType, int>> ExceptionMapping { get; set; }

        static Dictionary<EAssetType, VoidTwoParamsCallback<BaseRecorder, Dictionary<ExceptionType, int>>> ExceptionCollecionActions;
        internal static Dictionary<ExceptionType, string[]> ExceptionTips = new Dictionary<ExceptionType, string[]> {
            // Texture
            { ExceptionType.TEXTURE_RW,AppConfigHelper.TIPS_TEXTURE_RW},
            { ExceptionType.TEXTURE_FORMAT,AppConfigHelper.TIPS_TEXTURE_FORMAT},
            { ExceptionType.TEXTURE_RESOLUTION_NPOT,AppConfigHelper.TIPS_TEXTURE_RESOLUTION_NPOT},
            { ExceptionType.TEXTURE_RESOLUTION_SIZE,AppConfigHelper.TIPS_TEXTURE_RESOLUTION_SIZE},
            { ExceptionType.TEXTURE_FILTERMODE,AppConfigHelper.TIPS_TEXTURE_FILTERMODE},
            { ExceptionType.TEXTURE_ANISOLEVEL,AppConfigHelper.TIPS_TEXTURE_ANISOLEVEL},
            { ExceptionType.TEXTURE_NOUSED,AppConfigHelper.TIPS_TEXTURE_NOUSED},
            // Model & Mesh
            { ExceptionType.MODEL_RW,AppConfigHelper.TIPS_MODEL_RW},
            { ExceptionType.MODEL_MESH_TRIANGLE_LIMIT,AppConfigHelper.TIPS_MODEL_MESH_TRIANGLE_LIMIT},
            { ExceptionType.MODEL_MESH_VERTEX_LIMIT,AppConfigHelper.TIPS_MODEL_MESH_VERTEX_LIMIT},
            { ExceptionType.MODEL_MESH_BLENDSHAPE_LIMIT,AppConfigHelper.TIPS_MODEL_MESH_BLENDSHAPE_LIMIT},
            { ExceptionType.MODEL_MESH_VC_LIMIT,AppConfigHelper.TIPS_MODEL_MESH_VC_LIMIT},
            { ExceptionType.MODEL_MESH_UV_NOUSED,AppConfigHelper.TIPS_MODEL_MESH_UV_NOUSED},
            { ExceptionType.MODEL_MESH_UV2_AUTO,AppConfigHelper.TIPS_MODEL_MESH_UV2_AUTO},
            { ExceptionType.MODEL_BONE_LIMIT,AppConfigHelper.TIPS_MODEL_BONE_LIMIT},
            { ExceptionType.MODEL_ANIM_COMPRESSION,AppConfigHelper.TIPS_MODEL_ANIM_COMPRESSION},
            { ExceptionType.MODEL_NOUSED,AppConfigHelper.TIPS_MODEL_NOUSED},
             // Material
            { ExceptionType.MATERIAL_MISS_SHADER,AppConfigHelper.TIPS_MATERIAL_MISS_SHADER},
            { ExceptionType.MATERIAL_BUILTIN_REF,AppConfigHelper.TIPS_MATERIAL_BUILTIN_REF},
            { ExceptionType.MATERIAL_NO_USED_DATA,AppConfigHelper.TIPS_MATERIAL_NO_USED_DATA},
            { ExceptionType.MATERIAL_NO_USED,AppConfigHelper.TIPS_MATERIAL_NO_USED},
            // Shader
            { ExceptionType.SHADER_DEP,AppConfigHelper.TIPS_SHADER_DEP},
            { ExceptionType.SHADER_NO_USED,AppConfigHelper.TIPS_SHADER_NO_USED},
            // Prefab
            { ExceptionType.PREFAB_MISS_SCRIPT,AppConfigHelper.TIPS_PREFAB_MISS_SCRIPT},
            { ExceptionType.PREFAB_COLLIDER,AppConfigHelper.TIPS_PREFAB_COLLIDER},
            { ExceptionType.PREFAB_BUILTIN_REF,AppConfigHelper.TIPS_PREFAB_BUILTIN_REF},
            { ExceptionType.PREFAB_MISS_MAT,AppConfigHelper.TIPS_PREFAB_MISS_MAT},
            // Scene
            { ExceptionType.SCENE_MISS_SCRIPT,AppConfigHelper.TIPS_SCENE_MISS_SCRIPT},
            { ExceptionType.SCENE_COLLIDER,AppConfigHelper.TIPS_SCENE_COLLIDER},
            { ExceptionType.SCENE_BUILTIN_REF,AppConfigHelper.TIPS_SCENE_BUILTIN_REF},
            // Script
            { ExceptionType.SCRIPT_DEP,AppConfigHelper.TIPS_SCRIPT_DEP},
            // AnimatorConroller & AnimationClip
            { ExceptionType.ANIMATORCONTROLLER_NOUSED,AppConfigHelper.TIPS_ANIMATORCONTROLLER_NOUSED},
            // Audio & Video
            { ExceptionType.AUDIO_VIDEO_NOUSED,AppConfigHelper.TIPS_AUDIO_VIDEO_NOUSED},
            // Repeat Files
            { ExceptionType.REPEAT_FILE,AppConfigHelper.TIPS_REPEAT_FILES},
            // Resources Folder Files
            { ExceptionType.RESOURCES_FOLDER,AppConfigHelper.TIPS_RESOURCES_FOLDER},
            // Assetbundle Strategy Exception Files
            { ExceptionType.ASSETBUNDLE_STRATEGY,AppConfigHelper.TIPS_ASSETBUNDLE_STRATEGY},
             // Assetbundle Builtin Exception
            { ExceptionType.ASSETBUNDLE_BUILTIN,AppConfigHelper.TIPS_ASSETBUNDLE_BUILTIN},
        };

        internal static Dictionary<EAssetType, Dictionary<string, BaseRecorder>> AssetbundleStrategyExceptionMapping { get; set; }
        internal static int AssetbunleStrategyExceptionAmount = 0;

        internal static Dictionary<string,List<string>> AssetbunleBuiltinExceptionMapping { get; set; }

        internal static bool GenerateCurrentFullReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentNewerReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentDeletedReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentModifiedReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentRepeatedReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentNoUsedReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentTextureExceptionReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentModelExceptionReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentMaterialExceptionReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentShaderExceptionReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentScriptExceptionReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentDepBuiltinReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentPrefabExceptionReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentSceneExceptionReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentABStrategyReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentABContainBuiltinReporterCheckBox{ get; set; }
        internal static bool GenerateCurrentResorucesFolderReporterCheckBox{ get; set; }
     

        #endregion

        #region Local Methods

        static Dictionary<EAssetType, Dictionary<string, BaseRecorder>> GetAssetsMapping(string filePath,out int fileAmount, out Dictionary<string, List<BaseRecorder>> repeatFilesMapping)
        {
            fileAmount = 0;
            repeatFilesMapping = null;

            if (!File.Exists(filePath)) return null;

            Dictionary<string, List<BaseRecorder>> temprepeatFilesMapping = new Dictionary<string, List<BaseRecorder>>();
            Dictionary<EAssetType, Dictionary<string, BaseRecorder>> mapping = new Dictionary<EAssetType, Dictionary<string, BaseRecorder>>();
            Dictionary<string, BaseRecorder> tempDic = null;
            List<BaseRecorder> tempList = null;
            int localFileAmount = 0;
            BaseRecorder.GetAllRecorders(filePath, (BaseRecorder recorder) =>
            {
                if (!mapping.TryGetValue(recorder.m_AssetType, out tempDic))
                {
                    tempDic = new Dictionary<string, BaseRecorder>();
                    mapping[recorder.m_AssetType] = tempDic;
                }

                tempDic[recorder.m_AssetPath] = recorder;
                localFileAmount++;

                if (!temprepeatFilesMapping.TryGetValue(recorder.m_AssetMD5, out tempList))
                {
                    tempList = new List<BaseRecorder>();
                    temprepeatFilesMapping[recorder.m_AssetMD5] = tempList;
                }

                tempList.Add(recorder);
            });

            fileAmount = localFileAmount;
            repeatFilesMapping = temprepeatFilesMapping;

            return mapping;
        }
        static Dictionary<EAssetType, Dictionary<string, Dictionary<string, BaseRecorder>>> getRepeatFilesExceptionData(Dictionary<string, List<BaseRecorder>> fileMD5Mapping)
        {
            CurrentRepeatFilesSizeMapping = new Dictionary<EAssetType, long>();

            if (fileMD5Mapping == null || fileMD5Mapping.Count == 0) return new Dictionary<EAssetType, Dictionary<string, Dictionary<string, BaseRecorder>>>();
     
            Dictionary<EAssetType, Dictionary<string, Dictionary<string, BaseRecorder>>> mapping = new Dictionary<EAssetType, Dictionary<string, Dictionary<string, BaseRecorder>>>();
            Dictionary<string, Dictionary<string, BaseRecorder>> tempDic = null;
            Dictionary<string, BaseRecorder> tempChildDic = null;
            foreach (var data in fileMD5Mapping)
            {
                var recorders = data.Value;
                if (recorders.Count <= 1) continue;

                var firstRecorder = recorders[0];
                var assetType = firstRecorder.m_AssetType;
                if (!mapping.TryGetValue(assetType, out tempDic))
                {
                    tempDic = new Dictionary<string, Dictionary<string, BaseRecorder>>();
                    mapping[assetType] = tempDic;
                }

                long currentRepeatFileSize = 0;
                if (!CurrentRepeatFilesSizeMapping.TryGetValue(assetType, out currentRepeatFileSize)) currentRepeatFileSize = 0;

                foreach (var recorder in recorders)
                {
                    if (!tempDic.TryGetValue(firstRecorder.m_AssetPath, out tempChildDic))
                    {
                        tempChildDic = new Dictionary<string, BaseRecorder>();
                        tempDic[firstRecorder.m_AssetPath] = tempChildDic;
                    }

                    if (!tempChildDic.ContainsKey(recorder.m_AssetPath))
                    {
                        tempChildDic.Add(recorder.m_AssetPath, recorder);
                        currentRepeatFileSize += recorder.m_FileDiskSize;
                    }
                }

                CurrentRepeatFilesSizeMapping[assetType] = currentRepeatFileSize;
            }

            return mapping;
        }

        //static void getCustomDirFilesMapping(Dictionary<EAssetType, Dictionary<string, BaseRecorder>> currentVerisonFilesMapping)
        //{
        //    CurrentCustomDirFilesMapping = new Dictionary<string, Dictionary<EAssetType, Dictionary<string, BaseRecorder>>>();
        //    CurrentCustomDirFilesSizeMapping = new Dictionary<string, Dictionary<EAssetType, long>>();
        //    CurrentCustomDirFilesAmountMapping = new Dictionary<string, int>();

        //    Dictionary<EAssetType, Dictionary<string, BaseRecorder>> tempFirstDic = null;
        //    Dictionary<string, BaseRecorder> tempSecondDic = null;
        //    Dictionary<EAssetType, long> tempThirdDic = null;

        //    if (CurrentCustomDirRuleMapping == null || CurrentCustomDirRuleMapping.Count == 0) return;

        //    foreach (var dir in CurrentCustomDirRuleMapping.Keys)
        //    {
        //        int totalAmount = 0;
        //        if (!CurrentCustomDirFilesAmountMapping.TryGetValue(dir, out totalAmount)) totalAmount = 0;

        //        foreach (var data in currentVerisonFilesMapping)
        //        {
        //            var assetType = data.Key;
        //            foreach (var childData in data.Value)
        //            {
        //                var assetPath = childData.Key;
        //                if (assetPath.StartsWith(dir))
        //                {
        //                    totalAmount++;

        //                    if (!CurrentCustomDirFilesMapping.TryGetValue(dir, out tempFirstDic))
        //                    {
        //                        tempFirstDic = new Dictionary<EAssetType, Dictionary<string, BaseRecorder>>();
        //                        CurrentCustomDirFilesMapping[dir] = tempFirstDic;
        //                    }

        //                    if (!tempFirstDic.TryGetValue(assetType, out tempSecondDic))
        //                    {
        //                        tempSecondDic = new Dictionary<string, BaseRecorder>();
        //                        tempFirstDic[assetType] = tempSecondDic;
        //                    }

        //                    tempSecondDic[assetPath] = childData.Value;

        //                    // Collect File Size
        //                    if (!CurrentCustomDirFilesSizeMapping.TryGetValue(dir, out tempThirdDic))
        //                    {
        //                        tempThirdDic = new Dictionary<EAssetType, long>();
        //                        CurrentCustomDirFilesSizeMapping[dir] = tempThirdDic;
        //                    }

        //                    long fileSize = 0;
        //                    if (!tempThirdDic.TryGetValue(assetType, out fileSize)) fileSize = 0;

        //                    fileSize += childData.Value.m_FileDiskSize;
        //                    tempThirdDic[assetType] = fileSize;
        //                }
        //            }
        //        }

        //        CurrentCustomDirFilesAmountMapping[dir] = totalAmount;
        //    }

        //}

        static void GetAllAssetTypeMapping()
        {
            AssetTypeMapping = new List<EAssetType>();
            var array = System.Enum.GetValues(typeof(EAssetType));
            foreach (EAssetType type in array)
            {
                AssetTypeMapping.Add(type);
            }
        }

        static void CollectAssetsException(ExceptionType exceptionType, Dictionary<ExceptionType, int> mapping)
        {
            int amount = 0;
            if (!mapping.TryGetValue(exceptionType, out amount)) amount = 0;

            amount++;
            mapping[exceptionType] = amount;
        }

        static void RegisterExceptionCollectionActions()
        {
            ExceptionCollecionActions = new Dictionary<EAssetType, VoidTwoParamsCallback<BaseRecorder, Dictionary<ExceptionType, int>>>();
            // Texture
            ExceptionCollecionActions[EAssetType.Texture] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                TextureRecorder textureRecorder = baseRecorder as TextureRecorder;
                if (textureRecorder.RWInvalid())
                    CollectAssetsException(ExceptionType.TEXTURE_RW, mapping);
                if (textureRecorder.FormatInvalid())
                    CollectAssetsException(ExceptionType.TEXTURE_FORMAT, mapping);
                if (textureRecorder.POTInvalid())
                    CollectAssetsException(ExceptionType.TEXTURE_RESOLUTION_NPOT, mapping);
                if (textureRecorder.ResolutionInvalid())
                    CollectAssetsException(ExceptionType.TEXTURE_RESOLUTION_SIZE, mapping);
                if (textureRecorder.FilterModeInvalid())
                    CollectAssetsException(ExceptionType.TEXTURE_FILTERMODE, mapping);
                if (textureRecorder.AnisoLevelInvalid())
                    CollectAssetsException(ExceptionType.TEXTURE_ANISOLEVEL, mapping);
                if (textureRecorder.ReferenciesInvalid())
                    CollectAssetsException(ExceptionType.TEXTURE_NOUSED, mapping);
            };
            // Model
            ExceptionCollecionActions[EAssetType.Model] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                ModelRecorder modelRecorder = baseRecorder as ModelRecorder;
                if (modelRecorder.RWInvalid())
                    CollectAssetsException(ExceptionType.MODEL_RW, mapping);
                if (modelRecorder.TriangleInvalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_TRIANGLE_LIMIT, mapping);
                if (modelRecorder.VertexCountInvalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_VERTEX_LIMIT, mapping);
                if (modelRecorder.BlendShapeInvalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_BLENDSHAPE_LIMIT, mapping);
                if (modelRecorder.VertexColorInvalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_VC_LIMIT, mapping);
                if (modelRecorder.UVChannalInvalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_UV_NOUSED, mapping);
                if (modelRecorder.GenerateUV2Invalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_UV2_AUTO, mapping);
                if (modelRecorder.BoneCountInvalid())
                    CollectAssetsException(ExceptionType.MODEL_BONE_LIMIT, mapping);
                if (modelRecorder.AnimCompressionInvalid())
                    CollectAssetsException(ExceptionType.MODEL_ANIM_COMPRESSION, mapping);
                if (modelRecorder.ReferenciesInvalid())
                    CollectAssetsException(ExceptionType.MODEL_NOUSED, mapping);
            };
            // Mesh
            ExceptionCollecionActions[EAssetType.Mesh] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                MeshRecorder meshRecorder = baseRecorder as MeshRecorder;
                if (meshRecorder.RWInvalid())
                    CollectAssetsException(ExceptionType.MODEL_RW, mapping);
                if (meshRecorder.TriangleInvalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_TRIANGLE_LIMIT, mapping);
                if (meshRecorder.VertexCountInvalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_VERTEX_LIMIT, mapping);
                if (meshRecorder.BlendShapeInvalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_BLENDSHAPE_LIMIT, mapping);
                if (meshRecorder.VertexColorInvalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_VC_LIMIT, mapping);
                if (meshRecorder.UVChannalInvalid())
                    CollectAssetsException(ExceptionType.MODEL_MESH_UV_NOUSED, mapping);
                if (meshRecorder.ReferenciesInvalid())
                    CollectAssetsException(ExceptionType.MODEL_NOUSED, mapping);
            };
            // Material
            ExceptionCollecionActions[EAssetType.Material] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                MaterialRecorder materialRecorder = baseRecorder as MaterialRecorder;
                if (materialRecorder.RefShaderValid())
                    CollectAssetsException(ExceptionType.MATERIAL_MISS_SHADER, mapping);
                if (materialRecorder.InvalidBuiltinDependencies())
                    CollectAssetsException(ExceptionType.MATERIAL_BUILTIN_REF, mapping);
                if (materialRecorder.ExistNoUsedTexProps() || materialRecorder.ExistNoUsedFloatAndIntProps() || materialRecorder.ExistNoUsedColorAndVectorProps())
                    CollectAssetsException(ExceptionType.MATERIAL_NO_USED_DATA, mapping);
                if (materialRecorder.ReferenciesInvalid())
                    CollectAssetsException(ExceptionType.MATERIAL_NO_USED, mapping);
            };
            // Shader
            ExceptionCollecionActions[EAssetType.Shader] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                ShaderRecorder shaderRecorder = baseRecorder as ShaderRecorder;
                if (shaderRecorder.DependenciesInvalid())
                    CollectAssetsException(ExceptionType.SHADER_DEP, mapping);
                if (shaderRecorder.ReferenciesInvalid())
                    CollectAssetsException(ExceptionType.SHADER_NO_USED, mapping);
            };
            // Prefab
            ExceptionCollecionActions[EAssetType.Prefab] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                PrefabRecorder prefabRecorder = baseRecorder as PrefabRecorder;
                if (prefabRecorder.m_MissingCompList.Count > 0)
                    CollectAssetsException(ExceptionType.PREFAB_MISS_SCRIPT, mapping);
                if (prefabRecorder.m_CollidersMapping.Count > 0)
                    CollectAssetsException(ExceptionType.PREFAB_COLLIDER, mapping);
                if (prefabRecorder.InvalidBuiltinDependencies())
                    CollectAssetsException(ExceptionType.PREFAB_BUILTIN_REF, mapping);
                if (prefabRecorder.m_MissingMaterialsList.Count > 0)
                    CollectAssetsException(ExceptionType.PREFAB_MISS_MAT, mapping);
            };
            // Scene
            ExceptionCollecionActions[EAssetType.Scene] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                SceneRecorder sceneRecorder = baseRecorder as SceneRecorder;
                if (sceneRecorder.m_MissingCompList.Count > 0)
                    CollectAssetsException(ExceptionType.SCENE_MISS_SCRIPT, mapping);
                if (sceneRecorder.m_CollidersMapping.Count > 0)
                    CollectAssetsException(ExceptionType.SCENE_COLLIDER, mapping);
                if (sceneRecorder.InvalidBuiltinDependencies())
                    CollectAssetsException(ExceptionType.SCENE_BUILTIN_REF, mapping);
            };
            // Script
            ExceptionCollecionActions[EAssetType.Script] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                ScriptRecorder scriptRecorder = baseRecorder as ScriptRecorder;
                if (scriptRecorder.DependenciesInvalid())
                    CollectAssetsException(ExceptionType.SCRIPT_DEP, mapping);
            };
            // AnimationClip
            ExceptionCollecionActions[EAssetType.AnimationClip] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                if (baseRecorder.ReferenciesInvalid())
                    CollectAssetsException(ExceptionType.ANIMATORCONTROLLER_NOUSED, mapping);
            };
            // AnimatorController
            ExceptionCollecionActions[EAssetType.AnimatorController] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                if (baseRecorder.ReferenciesInvalid())
                    CollectAssetsException(ExceptionType.ANIMATORCONTROLLER_NOUSED, mapping);
            };
            // Audio
            ExceptionCollecionActions[EAssetType.AudioClip] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                if (baseRecorder.ReferenciesInvalid())
                    CollectAssetsException(ExceptionType.AUDIO_VIDEO_NOUSED, mapping);
            };
            // Video
            ExceptionCollecionActions[EAssetType.VideoClip] = (BaseRecorder baseRecorder, Dictionary<ExceptionType, int> mapping) =>
            {
                if (baseRecorder.ReferenciesInvalid())
                    CollectAssetsException(ExceptionType.AUDIO_VIDEO_NOUSED, mapping);
            };
        }

        #endregion

        #region Internal Methods

        internal static bool ParseData(string lastVersionAssetFile, string currentVersionAssetFile)
        {
            RegisterExceptionCollectionActions();

            TextureRecorder.UpdateDataFromConfig();
            ModelRecorder.UpdateDataFromConfig();

            GetAllAssetTypeMapping();

            bool whiteListError = false;
            CurrentWhiteListDynamicLoaded = ConfigDataParser.GetDynamicLoadedWhiteListData(out whiteListError);
            bool customDirRuleError = false;
            //  CurrentCustomDirRuleMapping = ConfigDataParser.GetCustomPathRuleDataMapping(out customDirRuleError);
            if (whiteListError || customDirRuleError) return false;

            string lastVerisonAssetsPath = lastVersionAssetFile;
            string currentVerisonAssetsPath = currentVersionAssetFile;
            int _lastFileTotalAmount = 0;
            Dictionary<string, List<BaseRecorder>> lastFileMD5Mapping = null;
            LastVerisonFilesMapping = GetAssetsMapping(lastVerisonAssetsPath, out _lastFileTotalAmount,out lastFileMD5Mapping);
            LastTotalFileAmount = _lastFileTotalAmount;
            int _currentFileTotalAmount = 0;

            Dictionary<string, List<BaseRecorder>> currentFileMD5Mapping = null;
            CurrentVerisonFilesMapping = GetAssetsMapping(currentVerisonAssetsPath, out _currentFileTotalAmount, out currentFileMD5Mapping);
            CurrentTotalFileAmount = _currentFileTotalAmount;
            if (CurrentVerisonFilesMapping != null)
            {
                CurrentRepeatFilesExceptionMapping = getRepeatFilesExceptionData(currentFileMD5Mapping);
                CurrentFilesSizeMapping = new Dictionary<EAssetType, long>();
                CurrentAddedFilesSizeMapping = new Dictionary<EAssetType, long>();
                CurrentBeforeModifiedMemorySizeMapping = new Dictionary<EAssetType, long>();
                CurrentBeforeModifiedFilesSizeMapping = new Dictionary<EAssetType, long>();
                CurrentAfterModifiedMemorySizeMapping = new Dictionary<EAssetType, long>();
                CurrentAfterModifiedFilesSizeMapping = new Dictionary<EAssetType, long>();
                CurrentDeletedFilesSizeMapping = new Dictionary<EAssetType, long>();
                CurrentVersionDeletedMapping = new Dictionary<EAssetType, Dictionary<string, BaseRecorder>>();
                CurrentVersionAddedMapping = new Dictionary<EAssetType, Dictionary<string, BaseRecorder>>();
                CurrentVersionModifiedMapping = new Dictionary<EAssetType, Dictionary<string, BaseRecorder>>();
                FileInResourcesFolderMapping = new Dictionary<EAssetType, Dictionary<string, BaseRecorder>>();
                FilesInResourcesFolderSizeMapping = new Dictionary<EAssetType, long>();
                BuiltinDependicesFileMapping = new Dictionary<EAssetType, Dictionary<string, BaseRecorder>>();
                BuiltinDependicesFileSizeMapping = new Dictionary<EAssetType, long>();
                ExceptionMapping = new Dictionary<EAssetType, Dictionary<ExceptionType, int>>();
                CurrentNoRefMapping = new Dictionary<EAssetType, Dictionary<string, BaseRecorder>>();
                CurrentNoRefFilesSizeMapping = new Dictionary<EAssetType, long>();
                AssetbundleStrategyExceptionMapping = new Dictionary<EAssetType, Dictionary<string, BaseRecorder>>();
                AssetbunleStrategyExceptionAmount = 0;
                AssetbunleBuiltinExceptionMapping = new Dictionary<string, List<string>>();
                // getCustomDirFilesMapping(CurrentVerisonFilesMapping);
                Dictionary<string, BaseRecorder> tempDic = null;
                Dictionary<string, BaseRecorder> tempDic1 = null;
                Dictionary<string, BaseRecorder> tempDic2 = null;
                Dictionary<ExceptionType, int> tempDic3 = null;
                Dictionary<string, BaseRecorder> tempDic4 = null;
                Dictionary<string, BaseRecorder> tempDic5 = null;
                List<string> tempList = null;

                // New Version Deleted ///////////////////////////////////////////////////////////////////
                if (LastVerisonFilesMapping != null)
                {
                    LastFilesSizeMapping = new Dictionary<EAssetType, long>();
                    foreach (var key in LastVerisonFilesMapping.Keys)
                    {
                        // Deleted File Size Collection
                        long currentDeletedFileSize = 0;
                        if (!CurrentDeletedFilesSizeMapping.TryGetValue(key, out currentDeletedFileSize))
                        {
                            currentDeletedFileSize = 0;
                        }

                        // File Size Collection
                        var lastDataList = LastVerisonFilesMapping[key];
                        long lastFileSize = 0;
                        if (!LastFilesSizeMapping.TryGetValue(key, out lastFileSize))
                        {
                            lastFileSize = 0;
                        }

                        foreach (var lastData in lastDataList)
                        {
                            lastFileSize += lastData.Value.m_FileDiskSize;
                        }

                        // File Update Collection
                        if (!CurrentVerisonFilesMapping.ContainsKey(key))
                        {
                            CurrentVersionDeletedMapping[key] = LastVerisonFilesMapping[key];
                            foreach (var childData in LastVerisonFilesMapping[key])
                            {
                                currentDeletedFileSize += childData.Value.m_FileDiskSize;
                            }
                        }
                        else
                        {
                            Dictionary<string, BaseRecorder> lastVersionDic = LastVerisonFilesMapping[key];
                            Dictionary<string, BaseRecorder> newVersionDic = CurrentVerisonFilesMapping[key];
                            foreach (var file in lastVersionDic.Keys)
                            {
                                if (!newVersionDic.ContainsKey(file))
                                {
                                    if (!CurrentVersionDeletedMapping.TryGetValue(key, out tempDic))
                                    {
                                        tempDic = new Dictionary<string, BaseRecorder>();
                                        CurrentVersionDeletedMapping[key] = tempDic;
                                    }

                                    tempDic.Add(file, lastVersionDic[file]);

                                    currentDeletedFileSize += lastVersionDic[file].m_FileDiskSize;
                                }
                            }
                        }

                        LastFilesSizeMapping[key] = lastFileSize;

                        CurrentDeletedFilesSizeMapping[key] = currentDeletedFileSize;
                    }
                }
            
                foreach (var data in CurrentVerisonFilesMapping)
                {
                    var assetType = data.Key;
                    var currentDataMapping = data.Value;

                    // Assetbundle Exception Collection ///////////////////////////////////////////////////
                    if (!AssetbundleStrategyExceptionMapping.TryGetValue(assetType, out tempDic5))
                    {
                        tempDic5 = new Dictionary<string, BaseRecorder>();
                        AssetbundleStrategyExceptionMapping[assetType] = tempDic5;
                    }

                    // Exception Collection /////////////////////////////////////////////////////////////
                    if (!ExceptionMapping.TryGetValue(assetType, out tempDic3))
                    {
                        tempDic3 = new Dictionary<ExceptionType, int>();
                        ExceptionMapping[assetType] = tempDic3;
                    }
                    VoidTwoParamsCallback<BaseRecorder, Dictionary<ExceptionType, int>> exceptionCollectionAction = null;
                    ExceptionCollecionActions.TryGetValue(assetType, out exceptionCollectionAction);

                    // File Size Collection  ///////////////////////////////////////////////////////
                    long currentFileSize = 0;
                    if (!CurrentFilesSizeMapping.TryGetValue(assetType, out currentFileSize)) currentFileSize = 0;
                    long fileInResourcesSize = 0;
                    if (!FilesInResourcesFolderSizeMapping.TryGetValue(assetType, out fileInResourcesSize)) fileInResourcesSize = 0;
                    long builtinDependenciesFileSize = 0;
                    if (!BuiltinDependicesFileSizeMapping.TryGetValue(assetType, out builtinDependenciesFileSize)) builtinDependenciesFileSize = 0;
                    long noReferenceFileSize = 0;
                    if (!CurrentNoRefFilesSizeMapping.TryGetValue(assetType, out noReferenceFileSize)) noReferenceFileSize = 0;
                    long currentBeforeModifiedFileSize = 0;
                    long currentAfterModifiedFileSize = 0;
                    if (!CurrentBeforeModifiedFilesSizeMapping.TryGetValue(assetType, out currentBeforeModifiedFileSize)) currentBeforeModifiedFileSize = 0;
                    if (!CurrentAfterModifiedFilesSizeMapping.TryGetValue(assetType, out currentAfterModifiedFileSize)) currentAfterModifiedFileSize = 0;
                    long currentBeforeModifiedMemorySize = 0;
                    long currentAfterModifiedMemorySize = 0;
                    if (!CurrentBeforeModifiedMemorySizeMapping.TryGetValue(assetType, out currentBeforeModifiedMemorySize)) currentBeforeModifiedMemorySize = 0;
                    if (!CurrentAfterModifiedMemorySizeMapping.TryGetValue(assetType, out currentAfterModifiedMemorySize)) currentAfterModifiedMemorySize = 0;
                    
                    // File In Resources Folder Collection /////////////////////////////////////////
                    if (!FileInResourcesFolderMapping.TryGetValue(assetType, out tempDic1))
                    {
                        tempDic1 = new Dictionary<string, BaseRecorder>();
                        FileInResourcesFolderMapping[assetType] = tempDic1;
                    }

                    // File With Builtin Dependencies Collection //////////////////////////////////
                    if (!BuiltinDependicesFileMapping.TryGetValue(assetType, out tempDic2))
                    {
                        tempDic2 = new Dictionary<string, BaseRecorder>();
                        BuiltinDependicesFileMapping[assetType] = tempDic2;
                    }

                    // No References File Collection ///////////////////////////////////////////
                    if (!CurrentNoRefMapping.TryGetValue(assetType, out tempDic4))
                    {
                        tempDic4 = new Dictionary<string, BaseRecorder>();
                        CurrentNoRefMapping[assetType] = tempDic4;
                    }

                    // New Version Added & Modified //////////////////////////////////////////////////
                    long currentAddedFileSize = 0;
                    if (!CurrentAddedFilesSizeMapping.TryGetValue(assetType, out currentAddedFileSize)) currentAddedFileSize = 0;
                    bool isNewVersionType = false;
                    Dictionary<string, BaseRecorder> lastDataMapping = null;
                    if (LastVerisonFilesMapping == null || !LastVerisonFilesMapping.ContainsKey(assetType))
                    {
                        CurrentVersionAddedMapping[assetType] = currentDataMapping;
                        isNewVersionType = true;
                    }
                    else
                    {
                        lastDataMapping = LastVerisonFilesMapping[assetType];
                    }

                    foreach (var currentData in currentDataMapping)
                    {
                        var assetPath = currentData.Key;
                        var currentRecorder = currentData.Value;

                        currentFileSize += currentRecorder.m_FileDiskSize;

                        // Assetbundle Strategy Exception Collection ////////////////////////////////////
                        if (currentRecorder.AssetbundleInvalid())
                        {
                            tempDic5[assetPath] = currentRecorder;
                            AssetbunleStrategyExceptionAmount++;
                        }

                        // Assetbundle Builtin Exception Collection ////////////////////////////////////////////
                        if (currentRecorder.m_BundleNames.Count > 0 && currentRecorder.m_BuiltinDependencies.Count > 0)
                        {
                            foreach (var bundleName in currentRecorder.m_BundleNames)
                            {
                                if (!AssetbunleBuiltinExceptionMapping.TryGetValue(bundleName, out tempList))
                                {
                                    tempList = new List<string>();
                                    AssetbunleBuiltinExceptionMapping[bundleName] = tempList;
                                }

                                foreach (var builtinRes in currentRecorder.m_BuiltinDependencies)
                                {
                                    if (!tempList.Contains(builtinRes))
                                        tempList.Add(builtinRes);
                                }
                            }
                        }                    

                        // File In Resources Folder Collection /////////////////////////////////////////
                        if (currentData.Value.IsInReourcesFolder())
                        {
                            fileInResourcesSize += currentRecorder.m_FileDiskSize;
                            tempDic1[currentData.Key] = currentRecorder;
                        }

                        // File With Builtin Dependencies Collection //////////////////////////////////
                        if (currentData.Value.InvalidBuiltinDependencies())
                        {
                            builtinDependenciesFileSize += currentRecorder.m_FileDiskSize;
                            tempDic2[currentData.Key] = currentRecorder;
                        }

                        // Exception Collection ///////////////////////////////////////////////////////
                        if (exceptionCollectionAction != null && tempDic3 != null)
                            exceptionCollectionAction(currentData.Value, tempDic3);

                        // No References File Collection //////////////////////////////////////////////
                        if (currentRecorder.m_Referencies.Count == 0)
                        {
                            noReferenceFileSize += currentRecorder.m_FileDiskSize;
                            tempDic4.Add(currentRecorder.m_AssetPath, currentRecorder);
                        }

                        // New Version Added & Modified ///////////////////////////////////////////////
                        if (isNewVersionType)
                            currentAddedFileSize += currentRecorder.m_FileDiskSize;
                        else
                        {
                            if (!lastDataMapping.ContainsKey(assetPath))
                            {
                                if (!CurrentVersionAddedMapping.TryGetValue(assetType, out tempDic))
                                {
                                    tempDic = new Dictionary<string, BaseRecorder>();
                                    CurrentVersionAddedMapping[assetType] = tempDic;
                                }

                                tempDic.Add(assetPath, currentRecorder);
                                currentAddedFileSize += currentRecorder.m_FileDiskSize;
                            }
                            else
                            {
                                var lastRecorder = lastDataMapping[assetPath];
                                if (currentRecorder.m_AssetMD5 != lastRecorder.m_AssetMD5)
                                {
                                    if (!CurrentVersionModifiedMapping.TryGetValue(assetType, out tempDic))
                                    {
                                        tempDic = new Dictionary<string, BaseRecorder>();
                                        CurrentVersionModifiedMapping[assetType] = tempDic;
                                    }

                                    tempDic.Add(assetPath, currentRecorder);
                                    currentRecorder.m_FileDiskSizeOld = lastRecorder.m_FileDiskSize;
                                    if (lastRecorder.m_AssetType == EAssetType.Texture)
                                    {
                                        TextureRecorder textureLastRecorder = lastRecorder as TextureRecorder;
                                        TextureRecorder textureCurrentRecorder = currentRecorder as TextureRecorder;
                                        textureCurrentRecorder.m_MemorySizeOld = textureLastRecorder.m_MemorySize;
                                        currentBeforeModifiedMemorySize += textureLastRecorder.m_MemorySize;
                                        currentAfterModifiedMemorySize += textureCurrentRecorder.m_MemorySize;
                                    }
                                    if (lastRecorder.m_AssetType == EAssetType.RenderTexture)
                                    {
                                        RenderTextureRecorder rtLastRecorder = lastRecorder as RenderTextureRecorder;
                                        RenderTextureRecorder rtCurrentRecorder = currentRecorder as RenderTextureRecorder;
                                        rtCurrentRecorder.m_MemorySizeOld = rtLastRecorder.m_MemorySize;
                                        currentBeforeModifiedMemorySize += rtLastRecorder.m_MemorySize;
                                        currentAfterModifiedMemorySize += rtCurrentRecorder.m_MemorySize;
                                    }
                                    currentBeforeModifiedFileSize += lastRecorder.m_FileDiskSize;
                                    currentAfterModifiedFileSize += currentRecorder.m_FileDiskSize;
                                }
                                else
                                {
                                    if (lastRecorder.m_AssetMetaMD5 != currentRecorder.m_AssetMetaMD5)
                                    {
                                        if (!CurrentVersionModifiedMapping.TryGetValue(assetType, out tempDic))
                                        {
                                            tempDic = new Dictionary<string, BaseRecorder>();
                                            CurrentVersionModifiedMapping[assetType] = tempDic;
                                        }

                                        tempDic.Add(assetPath, currentDataMapping[assetPath]);
                                        currentRecorder.m_FileDiskSizeOld = lastRecorder.m_FileDiskSize;
                                        if (lastRecorder.m_AssetType == EAssetType.Texture)
                                        {
                                            TextureRecorder textureLastRecorder = lastRecorder as TextureRecorder;
                                            TextureRecorder textureCurrentRecorder = currentRecorder as TextureRecorder;
                                            textureCurrentRecorder.m_MemorySizeOld = textureLastRecorder.m_MemorySize;
                                            currentBeforeModifiedMemorySize += textureLastRecorder.m_MemorySize;
                                            currentAfterModifiedMemorySize += textureCurrentRecorder.m_MemorySize;
                                        }
                                        if (lastRecorder.m_AssetType == EAssetType.RenderTexture)
                                        {
                                            RenderTextureRecorder rtLastRecorder = lastRecorder as RenderTextureRecorder;
                                            RenderTextureRecorder rtCurrentRecorder = currentRecorder as RenderTextureRecorder;
                                            rtCurrentRecorder.m_MemorySizeOld = rtLastRecorder.m_MemorySize;
                                            currentBeforeModifiedMemorySize += rtLastRecorder.m_MemorySize;
                                            currentAfterModifiedMemorySize += rtCurrentRecorder.m_MemorySize;
                                        }
                                        currentBeforeModifiedFileSize += lastRecorder.m_FileDiskSize;
                                        currentAfterModifiedFileSize += currentRecorder.m_FileDiskSize;
                                    }
                                }
                            }
                        }
                    }

                    // File Size Collection ///////////////////////////////////////////////////////
                    CurrentFilesSizeMapping[assetType] = currentFileSize;
                    CurrentAddedFilesSizeMapping[assetType] = currentAddedFileSize;
                    CurrentBeforeModifiedFilesSizeMapping[assetType] = currentBeforeModifiedFileSize;
                    CurrentAfterModifiedFilesSizeMapping[assetType] = currentAfterModifiedFileSize;
                    CurrentBeforeModifiedMemorySizeMapping[assetType] = currentBeforeModifiedMemorySize;
                    CurrentAfterModifiedMemorySizeMapping[assetType] = currentAfterModifiedMemorySize;
                    FilesInResourcesFolderSizeMapping[assetType] = fileInResourcesSize;
                    BuiltinDependicesFileSizeMapping[assetType] = builtinDependenciesFileSize;
                    CurrentNoRefFilesSizeMapping[assetType] = noReferenceFileSize;
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
