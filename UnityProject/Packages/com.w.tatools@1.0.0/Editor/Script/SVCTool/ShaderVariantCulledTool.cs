namespace jj.TATools.Editor
{
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Rendering;

    public class ShaderVariantCulledTool : IPreprocessShaders
    {
        #region Fields

        private static bool m_EnabledCulledTool = false;

        private static Dictionary<Shader, Dictionary<PassType, List<string>>> m_SVCollectedForABMappings = new Dictionary<Shader, Dictionary<PassType, List<string>>>();
        private static Dictionary<Shader, Dictionary<PassType, List<string>>> m_SVCollectedForBuiltinMappings = new Dictionary<Shader, Dictionary<PassType, List<string>>>();
        private static bool m_IgoreCullBuiltinSV = false;

#if SVC_DEBUG_ON
        private static Dictionary<Shader, Dictionary<PassType, Dictionary<ShaderType, List<string>>>> m_DebugKeepVariablesMapping = new Dictionary<Shader, Dictionary<PassType, Dictionary<ShaderType, List<string>>>>();
        private static Dictionary<Shader, Dictionary<PassType, Dictionary<ShaderType, List<string>>>> m_DebugCulledVariablesMapping = new Dictionary<Shader, Dictionary<PassType, Dictionary<ShaderType, List<string>>>>();
        private static Dictionary<Shader, Dictionary<PassType, Dictionary<ShaderType, List<string>>>> m_DebugTotalVariablesMapping = new Dictionary<Shader, Dictionary<PassType, Dictionary<ShaderType, List<string>>>>();
#endif

        #endregion

        #region IPreprocessShaders

        public int callbackOrder { get { return 0; } }

        public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
        {
            if (!m_EnabledCulledTool && m_SVCollectedForABMappings.Count == 0)
            {
                CulledSVForBuiltin(shader, snippet, data); // Cull Shader Variant For Builtin
            }
            else
            {
                CulledSVForAssetbundles(shader, snippet, data); // Cull Shader Variant For Assetbundles
            }
        }

        #endregion

        #region Start Tools

        /// <summary>
        /// 开启SVC剔除工具
        /// 在执行Assetbundle构建逻辑之前调用
        /// </summary>
        internal static void StartCulledTool()
        {
            StartCulledTool(SVCSettings.Instance.CollectedSVC);
        }

        /// <summary>
        /// 开启SVC剔除工具
        /// 在执行Assetbundle构建逻辑之前调用
        /// </summary>
        /// </summary>
        /// <param name="collectedSVCList">已经收集完毕的svc list</param>
        public static void StartCulledTool(List<ShaderVariantCollection> collectedSVCList)
        {
            m_EnabledCulledTool = true;
            m_SVCollectedForABMappings.Clear();
            m_SVCollectedForBuiltinMappings.Clear();
            m_IgoreCullBuiltinSV = false;

#if SVC_DEBUG_ON
            m_DebugKeepVariablesMapping.Clear();
            m_DebugCulledVariablesMapping.Clear();
            m_DebugTotalVariablesMapping.Clear();
#endif
            if (collectedSVCList != null && collectedSVCList.Count > 0)
                m_SVCollectedForABMappings = SVCUtility.GetShaderVariantCollectionData(collectedSVCList);

#if SVC_DEBUG_ON
            foreach (var data in m_SVCollectedForABMappings)
            {
                var shader = data.Key;
                var output = "";
                foreach (var childData in data.Value)
                {
                    var passType = childData.Key;
                    output += passType.ToString() + "\n";
                    foreach (var kw in childData.Value)
                    {
                        output += "\t" + kw + "\n";
                    }
                }

                StreamWriter sw = new StreamWriter(shader.name.Replace("/", "-") + "_SVC.txt", false, System.Text.Encoding.UTF8);
                sw.Write(output);
                sw.Flush();
                sw.Close();
            }
#endif
        }

        /// <summary>
        /// 关闭SVC剔除工具
        /// 在Assetbundle构建逻辑执行完毕后调用
        /// </summary>
        public static void FinishCulledTool()
        {
            m_EnabledCulledTool = false;
            m_SVCollectedForABMappings.Clear();
            m_SVCollectedForBuiltinMappings.Clear();
            m_IgoreCullBuiltinSV = false;

#if SVC_DEBUG_ON
            // Culled
            foreach (var data in m_DebugCulledVariablesMapping)
            {
                var shader = data.Key;
                string outputStr = "";
                foreach (var childData in data.Value)
                {
                    var passType = childData.Key;
                    outputStr += passType.ToString() + "\n";
                    foreach (var thirdData in childData.Value)
                    {
                        var shaderType = thirdData.Key;
                        outputStr += "\t" + shaderType.ToString() + "\n";
                        foreach (var kw in thirdData.Value)
                        {
                            outputStr += "\t\t" + kw + "\n";
                        }
                    }
                }

                StreamWriter sw = new StreamWriter(shader.name.Replace("/", "-") + "_Culled.txt", false, System.Text.Encoding.UTF8);
                sw.Write(outputStr);
                sw.Flush();
                sw.Close();
            }
            // Keep
            foreach (var data in m_DebugKeepVariablesMapping)
            {
                var shader = data.Key;
                string outputStr = "";
                foreach (var childData in data.Value)
                {
                    var passType = childData.Key;
                    outputStr += passType.ToString() + "\n";
                    foreach (var thirdData in childData.Value)
                    {
                        var shaderType = thirdData.Key;
                        outputStr += "\t" + shaderType.ToString() + "\n";
                        foreach (var kw in thirdData.Value)
                        {
                            outputStr += "\t\t" + kw + "\n";
                        }
                    }
                }

                StreamWriter sw = new StreamWriter(shader.name.Replace("/", "-") + "_Keep.txt", false, System.Text.Encoding.UTF8);
                sw.Write(outputStr);
                sw.Flush();
                sw.Close();
            }
            // Total
            foreach (var data in m_DebugTotalVariablesMapping)
            {
                var shader = data.Key;
                string outputStr = "";
                foreach (var childData in data.Value)
                {
                    var passType = childData.Key;
                    outputStr += passType.ToString() + "\n";
                    foreach (var thirdData in childData.Value)
                    {
                        var shaderType = thirdData.Key;
                        outputStr += "\t" + shaderType.ToString() + "\n";
                        foreach (var kw in thirdData.Value)
                        {
                            outputStr += "\t\t" + kw + "\n";
                        }
                    }
                }

                StreamWriter sw = new StreamWriter(shader.name.Replace("/", "-") + "_Total.txt", false, System.Text.Encoding.UTF8);
                sw.Write(outputStr);
                sw.Flush();
                sw.Close();
            }
#endif
        }

        #endregion

        #region Local Methods

        void CulledSVForAssetbundles(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
        {
            Dictionary<PassType, List<string>> tempDic = null;
#if SVC_DEBUG_ON
            Dictionary<PassType, Dictionary<ShaderType, List<string>>> debugTempDic1 = null;
            Dictionary<ShaderType, List<string>> debugTempDic2 = null;
            List<string> debugTempList1 = null;
            Dictionary<PassType, Dictionary<ShaderType, List<string>>> debugTempDic3 = null;
            Dictionary<ShaderType, List<string>> debugTempDic4 = null;
            List<string> debugTempList2 = null;
            Dictionary<PassType, Dictionary<ShaderType, List<string>>> debugTempDic5 = null;
            Dictionary<ShaderType, List<string>> debugTempDic6 = null;
            List<string> debugTempList3 = null;
            // Total
            if (!m_DebugTotalVariablesMapping.TryGetValue(shader, out debugTempDic5))
            {
                debugTempDic5 = new Dictionary<PassType, Dictionary<ShaderType, List<string>>>();
                m_DebugTotalVariablesMapping[shader] = debugTempDic5;
            }

            if (!debugTempDic5.TryGetValue(snippet.passType, out debugTempDic6))
            {
                debugTempDic6 = new Dictionary<ShaderType, List<string>>();
                debugTempDic5[snippet.passType] = debugTempDic6;
            }

            if (!debugTempDic6.TryGetValue(snippet.shaderType, out debugTempList3))
            {
                debugTempList3 = new List<string>();
                debugTempDic6[snippet.shaderType] = debugTempList3;
            }

            for (int i = data.Count - 1; i >= 0; i--)
            {
                ShaderKeyword[] keywords = data[i].shaderKeywordSet.GetShaderKeywords();

                List<string> totalKWList = new List<string>();
                foreach (var kw in keywords)
                    totalKWList.Add(kw.name);

                totalKWList.Sort((x, y) => string.CompareOrdinal(x, y));
                string fullKW = "";
                foreach (var kw in totalKWList)
                    fullKW += kw + "|";
                if (!string.IsNullOrEmpty(fullKW))
                    fullKW = fullKW.Substring(0, fullKW.Length - 1);

                debugTempList3.Add(fullKW);
            }
#endif

            // 疑似unity2022.3.32 BUG：
            // 1.在pc，ios平台，剔除ShaderType.Vertex，会导致后续ShaderType.Fragment变体不编译，只能剔除ShaderType.Fragment
            // 2.在Android平台，OnProcessShader传入snippet没有ShaderType.Fragment变体，只能剔除ShaderType.Vertex
            bool shaderTypeCondition = false;
#if UNITY_ANDROID //|| UNITY_IOS
            shaderTypeCondition = snippet.shaderType == ShaderType.Vertex;
#else
           shaderTypeCondition = snippet.shaderType == ShaderType.Fragment;
#endif

            if (m_SVCollectedForABMappings.TryGetValue(shader, out tempDic) && shaderTypeCondition)
            {
#if SVC_DEBUG_ON
                // Culled
                if (!m_DebugCulledVariablesMapping.TryGetValue(shader, out debugTempDic1))
                {
                    debugTempDic1 = new Dictionary<PassType, Dictionary<ShaderType, List<string>>>();
                    m_DebugCulledVariablesMapping[shader] = debugTempDic1;
                }

                if (!debugTempDic1.TryGetValue(snippet.passType, out debugTempDic2))
                {
                    debugTempDic2 = new Dictionary<ShaderType, List<string>>();
                    debugTempDic1[snippet.passType] = debugTempDic2;
                }

                if (!debugTempDic2.TryGetValue(snippet.shaderType, out debugTempList1))
                {
                    debugTempList1 = new List<string>();
                    debugTempDic2[snippet.shaderType] = debugTempList1;
                }
                // Keep
                if (!m_DebugKeepVariablesMapping.TryGetValue(shader, out debugTempDic3))
                {
                    debugTempDic3 = new Dictionary<PassType, Dictionary<ShaderType, List<string>>>();
                    m_DebugKeepVariablesMapping[shader] = debugTempDic3;
                }

                if (!debugTempDic3.TryGetValue(snippet.passType, out debugTempDic4))
                {
                    debugTempDic4 = new Dictionary<ShaderType, List<string>>();
                    debugTempDic3[snippet.passType] = debugTempDic4;
                }

                if (!debugTempDic4.TryGetValue(snippet.shaderType, out debugTempList2))
                {
                    debugTempList2 = new List<string>();
                    debugTempDic4[snippet.shaderType] = debugTempList2;
                }
#endif

                List<string> svcKWList = null;
                tempDic.TryGetValue(snippet.passType, out svcKWList);
                if (svcKWList != null && svcKWList.Count > 0)
                {
                    for (int i = data.Count - 1; i >= 0; --i)
                    {
                        ShaderKeyword[] keywords = data[i].shaderKeywordSet.GetShaderKeywords();

                        List<string> totalKWList = new List<string>();
                        foreach (var kw in keywords)
                            totalKWList.Add(kw.name);

                        totalKWList.Sort((x, y) => string.CompareOrdinal(x, y));
                        string fullKW = "";
                        foreach (var kw in totalKWList)
                            fullKW += kw + "|";
                        if (!string.IsNullOrEmpty(fullKW))
                            fullKW = fullKW.Substring(0, fullKW.Length - 1);

                        // Culled
                        if (!svcKWList.Contains(fullKW))
                        {
                            data.RemoveAt(i);

#if SVC_DEBUG_ON
                            debugTempList1.Add(fullKW);
#endif
                        }
                        // Keep
                        else
                        {
#if SVC_DEBUG_ON
                            debugTempList2.Add(fullKW);
#endif
                        }
                    }
                }
                else
                {
#if SVC_DEBUG_ON
                    Debug.LogError("## Culled All - " + shader + "|" + snippet.passType + "|" + snippet.shaderType);
#endif
                    // Culled
                    for (int i = data.Count - 1; i >= 0; --i)
                    {
                        data.RemoveAt(i);
                    }
                }
            }
            else
            {
                // SVC中未包含的shader - 不予处理
            }
        }

        void CulledSVForBuiltin(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
        {
            if (m_SVCollectedForBuiltinMappings.Count == 0 &&!m_IgoreCullBuiltinSV)
            {
                List<ShaderVariantCollection> allSVCList = new List<ShaderVariantCollection>();
                var guids = AssetDatabase.FindAssets("t:" + typeof(ShaderVariantCollection).Name, new string[] { "Assets" });
                foreach (var guid in guids)
                {
                    var svcPath = AssetDatabase.GUIDToAssetPath(guid);
                    var svc = AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(svcPath);
                    if (svc != null)
                    {
                        allSVCList.Add(svc);
                    }
                }
                if (allSVCList != null && allSVCList.Count > 0)
                    m_SVCollectedForBuiltinMappings = SVCUtility.GetShaderVariantCollectionData(allSVCList);
                else
                {
                    if(!m_IgoreCullBuiltinSV)
                        m_IgoreCullBuiltinSV = true;
                }
            }

            if (m_IgoreCullBuiltinSV) return;

            Dictionary<PassType, List<string>> tempDic = null;
            // 1.在pc，ios平台，剔除ShaderType.Vertex，会导致后续ShaderType.Fragment变体不编译，只能剔除ShaderType.Fragment
            // 2.在Android平台，OnProcessShader传入snippet没有ShaderType.Fragment变体，只能剔除ShaderType.Vertex
            bool shaderTypeCondition = false;
#if UNITY_ANDROID
            shaderTypeCondition = snippet.shaderType == ShaderType.Vertex;
#else
           shaderTypeCondition = snippet.shaderType == ShaderType.Fragment;
#endif

            if (m_SVCollectedForBuiltinMappings.TryGetValue(shader, out tempDic) && shaderTypeCondition)
            {
                List<string> svcKWList = null;
                tempDic.TryGetValue(snippet.passType, out svcKWList);
                if (svcKWList != null && svcKWList.Count > 0)
                {
                    for (int i = data.Count - 1; i >= 0; --i)
                    {
                        ShaderKeyword[] keywords = data[i].shaderKeywordSet.GetShaderKeywords();

                        List<string> totalKWList = new List<string>();
                        foreach (var kw in keywords)
                            totalKWList.Add(kw.name);

                        totalKWList.Sort((x, y) => string.CompareOrdinal(x, y));
                        string fullKW = "";
                        foreach (var kw in totalKWList)
                            fullKW += kw + "|";
                        if (!string.IsNullOrEmpty(fullKW))
                            fullKW = fullKW.Substring(0, fullKW.Length - 1);

                        // Culled
                        if (!svcKWList.Contains(fullKW))
                        {
                            data.RemoveAt(i);
                        }
                        // Keep
                        else
                        {

                        }
                    }
                }
                else
                {
#if SVC_DEBUG_ON
                    Debug.LogError("## Culled All - " + shader + "|" + snippet.passType + "|" + snippet.shaderType);
#endif
                    // Culled
                    for (int i = data.Count - 1; i >= 0; --i)
                    {
                        data.RemoveAt(i);
                    }
                }
            }
            else
            {
                // SVC中未包含的shader - 不予处理
            }
        }

        #endregion
    }
}