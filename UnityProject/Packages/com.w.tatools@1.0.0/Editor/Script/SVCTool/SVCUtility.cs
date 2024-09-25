namespace jj.TATools.Editor
{
    using System.Reflection;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Rendering;

    internal enum ERenderPipelineType
    { 
        URP = 0,//Universal Render Pipeline
        BIRP //Built-In Render Pipeline
    }

    internal class SVCUtility
    {
        const char COMBINE_KW_SPLIT_CHAR = '|';

        internal static ERenderPipelineType GetProjectRenderPipelineType()
        {
            ERenderPipelineType pipeType = ERenderPipelineType.URP;

            if (GraphicsSettings.currentRenderPipeline != null)
                pipeType = ERenderPipelineType.URP;
            else if(QualitySettings.renderPipeline != null)
                pipeType = ERenderPipelineType.URP;
            else
                pipeType = ERenderPipelineType.BIRP;

            return pipeType;
        }

        internal static bool HasStaticEditorFlags(GameObject go, StaticEditorFlags flag)
        {
            return (GameObjectUtility.GetStaticEditorFlags(go) & StaticEditorFlags.ContributeGI) != 0;
        }

        internal static ShadowmaskMode GetShadowmaskMode()
        {
            return QualitySettings.shadowmaskMode;
        }

        internal static List<string> GetShaderAllKeywords(Shader shader)
        {
            List<LocalKeyword> allKeywords = new List<LocalKeyword>(shader.keywordSpace.keywords);
            allKeywords.Sort((x, y) => string.CompareOrdinal(x.name, y.name));

            List<string> finalAllKeywords = new List<string>();
            foreach (var kw in allKeywords)
                finalAllKeywords.Add(kw.name);

            return finalAllKeywords;
        }

        internal static List<LocalKeyword> GetShaderGlobalKeywords(Shader shader)
        {
            List<LocalKeyword> globalKeywords = new List<LocalKeyword>();
            var keywordSpace = shader.keywordSpace;
            var keywords = keywordSpace.keywords;

            foreach (var k in keywords)
            {
                if (k.isOverridable)
                    globalKeywords.Add(k);
            }

            globalKeywords.Sort((x, y) => string.CompareOrdinal(x.name, y.name));

            return globalKeywords;
        }

        internal static List<LocalKeyword> GetShaderLocalKeywords(Shader shader)
        {
            List<LocalKeyword> localKeywords = new List<LocalKeyword>();
            var keywordSpace = shader.keywordSpace;
            var keywords = keywordSpace.keywords;

            foreach (var k in keywords)
            {
                if (!k.isOverridable)
                    localKeywords.Add(k);
            }

            localKeywords.Sort((x, y) => string.CompareOrdinal(x.name, y.name));

            return localKeywords;
        }

        internal static Dictionary<int, Dictionary<int, List<LocalKeyword>>> GetShaderKeywordsPerPassPerSubShader(Shader shader,int realSubShaderAmount)
        {
            Dictionary<int, Dictionary<int, List<LocalKeyword>>> mapping = new Dictionary<int, Dictionary<int, List<LocalKeyword>>>();
            Dictionary<int, List<LocalKeyword>> tempDic = null;
            var shaderData = ShaderUtil.GetShaderData(shader);
            for (int i = 0; i < shaderData.SubshaderCount; i++)
            {
                if (!mapping.TryGetValue(i, out tempDic))
                {
                    tempDic = new Dictionary<int, List<LocalKeyword>>();
                    mapping[i] = tempDic;
                }

                if (i >= realSubShaderAmount)
                    continue;

                var subShader = shaderData.GetSubshader(i);
                for (int k = 0; k < subShader.PassCount; k++)
                {
                    var pass = subShader.GetPass(k);
                    if (pass.IsGrabPass) continue;

                    PassIdentifier passIdentifier = new PassIdentifier((uint)i, (uint)k);
                    var kwArr = ShaderUtil.GetPassKeywords(shader, in passIdentifier);
                    if (kwArr != null && kwArr.Length > 0)
                    {
                        string kwStr = "";
                        foreach (var kw in kwArr)
                        {
                            kwStr += kw.name + " ";
                        }
                        tempDic[k] = new List<LocalKeyword>(kwArr);
                    }
                }
            }

            return mapping;
        }

        internal static MethodInfo GetMethodInfo<T>(string methodName)
        {
            MethodInfo mInfo = typeof(T).GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

            return mInfo;
        }

        internal static void GetShaderVariantEntriesFiltered(Shader shader, string[] filterKeywords, ShaderVariantCollection excludeCollection, out int[] passTypes, out string[] keywordLists, out string[] remainingKeywords, int maxEntries = 100000000)
        {
            passTypes = null;
            keywordLists = null;
            remainingKeywords = null;
            MethodInfo mInfo = GetMethodInfo<ShaderUtil>("GetShaderVariantEntriesFiltered");
            object[] paramsArray = new object[] { shader, maxEntries, filterKeywords, excludeCollection, passTypes, keywordLists, remainingKeywords };
            mInfo.Invoke(null, paramsArray);

            passTypes = (int[])paramsArray[4];
            keywordLists = (string[])paramsArray[5];
            remainingKeywords = (string[])paramsArray[6];
        }

        internal static Dictionary<Shader, Dictionary<PassType, List<string>>> GetShaderVariantCollectionData(List<ShaderVariantCollection> svcList)
        {
            Dictionary<Shader, Dictionary<PassType, List<string>>> mapping = new Dictionary<Shader, Dictionary<PassType, List<string>>>();
            Dictionary<PassType, List<string>> tempDic = null;
            List<string> tempList = null;
            string[] splitArr = new string[] { " " };
            if (svcList != null && svcList.Count > 0)
            {
                foreach (var svc in svcList)
                {
                    SerializedObject so = new SerializedObject(svc);
                    SerializedProperty childCollectionSP = so.FindProperty("m_Shaders");
                    if (childCollectionSP != null)
                    {
                        for (int k = 0; k < childCollectionSP.arraySize; k++)
                        {
                            SerializedProperty sp = childCollectionSP.GetArrayElementAtIndex(k);

                            SerializedProperty shaderSP = sp.FindPropertyRelative("first");

                            Shader shader = (Shader)shaderSP.objectReferenceValue;
                            if (shader == null) continue;

                            string shaderPath = AssetDatabase.GetAssetPath(shader);
                            if (!System.IO.File.Exists(shaderPath)) continue;

                            if (!mapping.TryGetValue(shader, out tempDic))
                            {
                                tempDic = new Dictionary<PassType, List<string>>();
                                mapping[shader] = tempDic;
                            }

                            SerializedProperty shaderVariantSP = sp.FindPropertyRelative("second.variants");
                            for (int j = 0; j < shaderVariantSP.arraySize; j++)
                            {
                                SerializedProperty itemSP = shaderVariantSP.GetArrayElementAtIndex(j);
                                SerializedProperty keywordsSP = itemSP.FindPropertyRelative("keywords");
                                SerializedProperty passTypeSP = itemSP.FindPropertyRelative("passType");

                                string keywordsStr = keywordsSP.stringValue;
                                List<string> keywords = new List<string>(keywordsStr.Split(splitArr, System.StringSplitOptions.RemoveEmptyEntries));
                                keywords.Sort((x, y) => string.CompareOrdinal(x, y));
                                string combineStr = "";
                                foreach (var kw in keywords)
                                {
                                    combineStr += kw + "|";
                                }
                                if (!string.IsNullOrEmpty(combineStr))
                                    combineStr = combineStr.Substring(0, combineStr.Length - 1);

                                PassType passType = (PassType)passTypeSP.intValue;
                                if (!tempDic.TryGetValue(passType, out tempList))
                                {
                                    tempList = new List<string>();
                                    tempDic[passType] = tempList;
                                }

                                tempList.Add(combineStr);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError(svc + ":Not find " + "m_Shaders");
                    }
                }
            }

            return mapping;
        }

        internal static bool HasSurfaceShaders(Shader s)
        {
            MethodInfo mInfo = GetMethodInfo<ShaderUtil>("HasSurfaceShaders");
            object[] paramsArray = new object[] { s};
            return (bool)(mInfo.Invoke(null, paramsArray));
        }

        static void GetCombination(ref List<string[]> list, string[] source, int totalCount, int requiredCount, int[] indexList, int M)
        {
            for (int i = totalCount; i >= requiredCount; i--)
            {
                indexList[requiredCount - 1] = i - 1;
                if (requiredCount > 1)
                {
                    GetCombination(ref list, source, i - 1, requiredCount - 1, indexList, M);
                }
                else
                {
                    if (list == null)
                    {
                        list = new List<string[]>();
                    }
                    string[] temp = new string[M];
                    for (int j = 0; j < indexList.Length; j++)
                    {
                        temp[j] = source[indexList[j]];
                    }
                    list.Add(temp);
                }
            }
        }

        internal static List<string> GetAllDynamicKeywords(List<string> dynamicKeywords)
        {
            List<string> allCombineKWs = new List<string>();
            foreach (var item in dynamicKeywords)
            {
                var itemSplitList = new List<string>(item.Split(COMBINE_KW_SPLIT_CHAR));
                itemSplitList = itemSplitList.Distinct().ToList<string>();
                foreach (var kw in itemSplitList)
                {
                    if (!string.IsNullOrEmpty(kw) && !allCombineKWs.Contains(kw))
                        allCombineKWs.Add(kw);
                }
            }

            return allCombineKWs;
        }

        internal static List<string[]> GetCombination(string[] targetCombineArr)
        {
            // Parse
            List<string> totalCombineKWs = new List<string>();
            List<List<string>> alwaysKeepKWsMapping = new List<List<string>>();
            foreach (var item in targetCombineArr)
            {
                var itemSplitList = new List<string>(item.Split(COMBINE_KW_SPLIT_CHAR));
                itemSplitList = itemSplitList.Distinct().ToList<string>();
                if (itemSplitList.Count > 1)
                {
                    alwaysKeepKWsMapping.Add(itemSplitList);
                }

                foreach (var kw in itemSplitList)
                {
                    if(!string.IsNullOrEmpty(kw) && !totalCombineKWs.Contains(kw))
                        totalCombineKWs.Add(kw);
                }
            }

            List<string[]> totalFullCombineList = new List<string[]>();
            string[] t = totalCombineKWs.ToArray();
            for (int i = 0; i < t.Length; i++)
            {
                int combineCount = i + 1;
                int[] temp = new int[combineCount];
                GetCombination(ref totalFullCombineList, t, t.Length, combineCount, temp, combineCount);
            }

            List<string[]> finalCombineList = new List<string[]>();
            if (alwaysKeepKWsMapping.Count == 0 || (alwaysKeepKWsMapping.Count == 1 && targetCombineArr.Length == 1 && alwaysKeepKWsMapping[0][0] == ""))
                finalCombineList.Add(new string[0]);

            foreach (var combineGroup in totalFullCombineList)
            {
                bool invalidGroup = false;
                foreach (var keepKWGroup in alwaysKeepKWsMapping)
                {
                    bool keepIndexCanBeZero = (keepKWGroup[0] == "");
                    int invalidIndex = 0;
                    foreach (var kw in combineGroup)
                    {
                        if (keepKWGroup.Contains(kw))
                            invalidIndex++;
                    }

                    if ((invalidIndex == 0 && !keepIndexCanBeZero) || invalidIndex > 1)
                    {
                        invalidGroup = true;
                        break;
                    }
                }

                if (!invalidGroup)
                {
                    finalCombineList.Add(combineGroup);
                }
            }

            return finalCombineList;
        }

        internal static bool CheckMaterialMissShader(Material mat)
        {
            SerializedObject materialProp = new SerializedObject(mat);
            SerializedProperty shaderRefProp = materialProp.FindProperty("m_Shader");
            var shader = shaderRefProp.objectReferenceValue;

            return shader == null;
        }

        //internal static List<T> GetAllDenpendenciesWithoutBuiltin<T>(UnityEngine.Object asset) where T : UnityEngine.Object
        //{
        //    if (asset == null) return null;

        //    List<T> dependencies = new List<T>();
        //    var allDependenices = EditorUtility.CollectDependencies(new UnityEngine.Object[] { asset });
        //    foreach (var depObj in allDependenices)
        //    {
        //        if (depObj == asset) continue;

        //        T realObj = depObj as T;
        //        if (realObj != null)
        //        {
        //            var assetPath = AssetDatabase.GetAssetPath(realObj);
        //            if (File.Exists(assetPath))
        //            {
        //                if (!dependencies.Contains(realObj))
        //                    dependencies.Add(realObj);
        //            }
        //        }
        //    }

        //    return dependencies;
        //}

        internal static List<T> GetDependencies<T>(string assetPath, bool recursive) where T : UnityEngine.Object
        {
            List<T> dependencies = new List<T>();
            var dependenciePathArray = AssetDatabase.GetDependencies(assetPath, recursive);
            foreach (var depPath in dependenciePathArray)
            {
                var depObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(depPath);
                T realObj = depObj as T;
                if (realObj != null)
                {
                    if (!dependencies.Contains(realObj))
                        dependencies.Add(realObj);
                }
            }
            return dependencies;
        }

        internal static List<string> GetDependencies(System.Type type, string assetPath, bool recursive)
        {
            List<string> dependencies = new List<string>();
            var dependenciePathArray = AssetDatabase.GetDependencies(assetPath, recursive);
            foreach (var depPath in dependenciePathArray)
            {
                var depObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(depPath);
                if (depObj.GetType() == type)
                {
                    if (!dependencies.Contains(depPath))
                        dependencies.Add(depPath);
                }
            }
            return dependencies;
        }

    }
}