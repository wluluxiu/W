namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    internal class ShaderParseData
    {
        #region Fields

        const string SHADER_SPACE_STR = " ";
        const string SHADER_TAB_STR = "\t";
        const string SHADER_COMMENT_STR = "//";
        const string SHADER_SUBSHADER_STR = "subshader";
        const string SHADER_PASS_STR_0 = "pass";
        const string SHADER_PASS_STR_1 = "pass{";
        const string SHADER_PASS_LIGHTMODE_START_STR = "\"lightmode\"=\"";
        const string SHADER_PASS_LIGHTMODE_END_STR = "\"";
        const string SHADER_PASS_NAME_START_STR = "name\"";
        const string SHADER_PASS_NAME_END_STR = "\"";

        private Shader m_Shader;
        private int  m_RealSubShaderAmount = 0;
        internal Dictionary<int, SubShader> m_DataMapping = new Dictionary<int, SubShader>();

        #endregion

        internal ShaderParseData() { }

        internal ShaderParseData(Shader shader)
        {
            this.m_Shader = shader;
            this.m_DataMapping.Clear();

            GetFallbackSubShaderInfo();

            var dataMapping = SVCUtility.GetShaderKeywordsPerPassPerSubShader(shader,m_RealSubShaderAmount);
            foreach (var data in dataMapping)
            {
                var subshaderIndex = data.Key;
                var subShaderPassList = data.Value;
                if (subShaderPassList.Count == 0) continue;

                SubShader subShader = new SubShader(subshaderIndex);
                m_DataMapping[subshaderIndex] = subShader;

                foreach (var passData in subShaderPassList)
                {
                    Pass pass = new Pass(passData.Key);
                    subShader.AddPass(passData.Key, pass);

                    foreach (var kw in passData.Value)
                    {
                        pass.AddKeyword(kw.name);
                    }
                }
            }

            ParsePassLightModeInfo();
        }

        #region Internal Methods

        internal void SetRenderPipeline(UnityEngine.Object renderPipelineObj)
        {
            foreach (var data in this.m_DataMapping)
            {
                data.Value.SetRenderPipeline(renderPipelineObj);
            }
        }

        internal ShaderParseData Clone(UnityEngine.Object renderPipelineObj)
        {
            ShaderParseData cloneData = new ShaderParseData();
            cloneData.m_Shader = this.m_Shader;
            cloneData.m_RealSubShaderAmount = this.m_RealSubShaderAmount;
            cloneData.m_DataMapping = new Dictionary<int, SubShader>();
            foreach (var data in this.m_DataMapping)
            {
                cloneData.m_DataMapping[data.Key] = data.Value.Clone(renderPipelineObj);
            }

            return cloneData;
        }

        internal void AddMaterialVariantKeywordGroup(string keywordGroup,Material mat)
        {
            foreach (var subshader in m_DataMapping.Values)
            {
                subshader.AddMaterialVariantKeywordGroup(keywordGroup,mat);
            }
        }

        internal void AddDynamicVariantKeywordGroup(string[] keywordGroup)
        {
            foreach (var subshader in m_DataMapping.Values)
            {
                subshader.AddDynamicVariantKeywordGroup(keywordGroup);
            }
        }

        internal void CombineMaterialAndDynamicVariant()
        {
            foreach (var subshader in m_DataMapping.Values)
            {
                subshader.CombineMaterialAndDynamicVariant();
            }
        }

        internal void CombineBuiltinKWVariant(Dictionary<Material, List<SceneInfo>> sceneInfoDic)
        {
            foreach (var subShader in m_DataMapping.Values)
            {
                subShader.CombineBuiltinKWVariant(sceneInfoDic);
            }
        }

        #endregion

        #region Internal Static Methods

        public static ShaderParseData CreateData(Shader shader)
        {
            return new ShaderParseData(shader);
        }

        #endregion

        #region Lcoal Methods

        void GetFallbackSubShaderInfo()
        {
            string path = AssetDatabase.GetAssetPath(m_Shader);
            if (!File.Exists(path)) return;

            StreamReader sr = new StreamReader(path);
            string lineStr = sr.ReadLine();
            while (lineStr != null)
            {
                string noSpaceTabLineStr = lineStr.Replace(SHADER_SPACE_STR, "").Replace(SHADER_TAB_STR, "").ToLower();
                if (noSpaceTabLineStr.StartsWith(SHADER_COMMENT_STR))
                {
                    lineStr = sr.ReadLine();
                    continue;
                }

                if (noSpaceTabLineStr.StartsWith(SHADER_SUBSHADER_STR))
                    m_RealSubShaderAmount ++;

                lineStr = sr.ReadLine();
            }

            sr.Close();
            sr.Dispose();
        }

        void ParsePassLightModeInfo()
        {
            string path = AssetDatabase.GetAssetPath(m_Shader);
            if (!File.Exists(path)) return;

            StreamReader sr = new StreamReader(path);
            string lineStr = sr.ReadLine();
            int subshaderIndex = 0;
            int passIndex = 0;
            SubShader tempSubShader = null;
            Pass tempPass = null;
            while (lineStr != null)
            {
                string noSpaceTabLineStr = lineStr.Replace(SHADER_SPACE_STR, "").Replace(SHADER_TAB_STR, "").ToLower();
                if (noSpaceTabLineStr.StartsWith(SHADER_COMMENT_STR))
                {
                    lineStr = sr.ReadLine();
                    continue;
                }

                if (tempPass != null)
                {
                    // Pass Name
                    if (noSpaceTabLineStr.StartsWith(SHADER_PASS_NAME_START_STR))
                    {
                        var tempStr = noSpaceTabLineStr.Replace(SHADER_PASS_NAME_START_STR, "");
                        int index = tempStr.IndexOf(SHADER_PASS_NAME_END_STR);
                        tempPass.m_PassName = tempStr.Substring(0, index);
                    }
                    // Pass Type
                    else if (noSpaceTabLineStr.Contains(SHADER_PASS_LIGHTMODE_START_STR))
                    {
                        var arr = noSpaceTabLineStr.Split(new string[] { SHADER_PASS_LIGHTMODE_START_STR }, System.StringSplitOptions.None);
                        if (arr.Length == 2)
                        {
                            int index = arr[1].IndexOf(SHADER_PASS_LIGHTMODE_END_STR);
                            var lightModeStr = arr[1].Substring(0, index);
                            tempPass.m_PassType = PassTypeDefine.GetPassTypeByLightMode(lightModeStr);
                        }
                        else
                        {
                            var error = "SVC-Collect Shader Type-Invalid LightMode Format:" + path + "\n" +
                                        "\t\t[" + tempSubShader.m_SubShaderIndex + "][" + tempPass.m_PassIndex + "][" + tempPass.m_PassName + "]";
                            Debug.LogError(error);
                        }
                    }
                }

                if (tempSubShader != null)
                {
                    // Pass Start
                    if (noSpaceTabLineStr == SHADER_PASS_STR_0 || noSpaceTabLineStr == SHADER_PASS_STR_1)
                    {
                        if (tempSubShader.m_DataMapping.TryGetValue(passIndex, out tempPass))
                        {
                            passIndex++;
                        }
                        else
                            tempPass = null;
                    }
                }

                // SubShader Start
                if (noSpaceTabLineStr.StartsWith(SHADER_SUBSHADER_STR))
                {
                    if (!m_DataMapping.TryGetValue(subshaderIndex, out tempSubShader))
                        tempSubShader = null;
                    else
                        subshaderIndex++;

                    tempPass = null;
                    passIndex = 0;
                }

                lineStr = sr.ReadLine();
            }

            sr.Close();
            sr.Dispose();
        }

        #endregion
    }
}