namespace jj.TATools.Editor
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    internal class Pass
    {
        #region Fields

        internal const char KW_SPLIT_CHAR = '|';

        internal string m_PassName;
        internal PassType m_PassType;
        internal int m_PassIndex;

        internal List<string> m_Keywords = new List<string>();
        internal Dictionary<Material, string> m_KWGroupFromMaterials = new Dictionary<Material, string>(); // Value: 以"|"分隔
        internal List<string[]> m_DynamicKeywords = new List<string[]>(); // Item：A B C ...
        internal Dictionary<Material, List<string[]>> m_CombineKeywords = new Dictionary<Material, List<string[]>>(); //Value-Item：A B C ...
        private UnityEngine.Object m_RenderPipelineObj;

        #endregion

        internal Pass(int passIndex)
        {
            this.m_PassIndex = passIndex;
        }

        #region Internal Methods

        internal void SetRenderPipeline(UnityEngine.Object renderPipelineObj)
        {
            m_RenderPipelineObj = renderPipelineObj;
        }

        internal Pass Clone(UnityEngine.Object renderPipelineObj)
        {
            Pass clonePass = new Pass(this.m_PassIndex);

            clonePass.m_RenderPipelineObj = renderPipelineObj;
            clonePass.m_PassName = string.IsNullOrEmpty(this.m_PassName) ? this.m_PassName : (string)this.m_PassName.Clone();
            clonePass.m_PassType = this.m_PassType;
            clonePass.m_PassIndex = this.m_PassIndex;
            clonePass.m_Keywords = new List<string>();
            foreach (var kw in this.m_Keywords)
            {
                clonePass.m_Keywords.Add((string)kw.Clone());
            }
            clonePass.m_KWGroupFromMaterials = new Dictionary<Material, string>();
            foreach (var data in this.m_KWGroupFromMaterials)
            {
                clonePass.m_KWGroupFromMaterials[data.Key] = (string)data.Value.Clone();
            }
            clonePass.m_CombineKeywords = new Dictionary<Material, List<string[]>>();
            foreach (var data in this.m_CombineKeywords)
            {
                List<string[]> cloneList = new List<string[]>();
                clonePass.m_CombineKeywords[data.Key] = cloneList;
                foreach (var list in data.Value)
                {
                    cloneList.Add((string[])list.Clone());
                }
            }
            clonePass.m_DynamicKeywords = new List<string[]>();
            foreach (var arr in this.m_DynamicKeywords)
            {
                clonePass.m_DynamicKeywords.Add((string[])arr.Clone());
            }

            return clonePass;
        }

        internal void AddKeyword(string keyword)
        {
            m_Keywords.Add(keyword);
        }

        internal void AddMaterialVariantKeywordGroup(string keywordGroup, Material mat)
        {
            m_KWGroupFromMaterials[mat] = keywordGroup;
        }

        internal void AddDynamicVariantKeywordGroup(string[] keywordGroup)
        {
            List<string> validKWS = new List<string>();
            if (keywordGroup.Length == 0)
                validKWS.Add("");
            else
            {
                foreach (var kw in keywordGroup)
                {
                    if (m_Keywords.Contains(kw))
                        validKWS.Add(kw);
                }
                if (validKWS.Count == 0)
                    return;
            }

            validKWS.Sort((x, y) => string.CompareOrdinal(x, y));

            var temp = validKWS.ToArray();
            if (!m_DynamicKeywords.Contains(temp))
            {
                m_DynamicKeywords.Add(temp);
            }
        }

        internal void CombineMaterialAndDynamicVariant()
        {
            Dictionary<Material, List<string[]>> materialValidKWSMapping = new Dictionary<Material, List<string[]>>();
            List<string[]> tempList = null;
            foreach (var data in m_KWGroupFromMaterials)
            {
                var mat = data.Key;
                var keywordGroup = data.Value;

                string[] splitedArr = keywordGroup.Split(KW_SPLIT_CHAR);
                List<string> validKWS = new List<string>();
                foreach (var kw in splitedArr)
                {
                    if (m_Keywords.Contains(kw))
                        validKWS.Add(kw);
                }

                if (!materialValidKWSMapping.TryGetValue(mat, out tempList))
                {
                    tempList = new List<string[]>();
                    materialValidKWSMapping[mat] = tempList;
                }

                tempList.Add(validKWS.ToArray());
            }

            if (materialValidKWSMapping.Count > 0)
            {
                if (m_DynamicKeywords.Count > 0)
                {
                    foreach (var data in materialValidKWSMapping)
                    {
                        var mat = data.Key;
                        var mKWs = data.Value;

                        if (!m_CombineKeywords.TryGetValue(mat, out tempList))
                        {
                            tempList = new List<string[]>();
                            m_CombineKeywords[mat] = tempList;
                        }
                        foreach (var dKW in m_DynamicKeywords)
                        {
                            foreach (var mKW in mKWs)
                            {
                                List<string> kwList = new List<string>(mKW);
                                kwList.AddRange(dKW);
                                kwList.Sort((x, y) => string.CompareOrdinal(x, y));
                                tempList.Add(kwList.ToArray());
                            }

                        }
                    }
                }
                else
                {
                    foreach (var data in materialValidKWSMapping)
                    {
                        var mat = data.Key;
                        var mKWs = data.Value;

                        if (!m_CombineKeywords.TryGetValue(mat, out tempList))
                        {
                            tempList = new List<string[]>();
                            m_CombineKeywords[mat] = tempList;
                        }

                        foreach (var mKW in mKWs)
                        {
                            tempList.Add(mKW);
                        }
                    }
                }
            }
            else
            {
                foreach (var data in m_KWGroupFromMaterials)
                {
                    var mat = data.Key;

                    if (!m_CombineKeywords.TryGetValue(mat, out tempList))
                    {
                        tempList = new List<string[]>();
                        m_CombineKeywords[mat] = tempList;
                    }

                    if (m_DynamicKeywords.Count > 0)
                    {
                        foreach (var dKW in m_DynamicKeywords)
                        {
                            tempList.Add(dKW);
                        }
                    }
                    else
                    {
                        tempList.Add(new string[0]);
                    }
                }
            }
        }

        internal void CombineBuiltinKWVariant(Dictionary<Material, List<SceneInfo>> sceneInfoDic)
        {
            Dictionary<Material, List<string>> buildinKWGroupDic = new Dictionary<Material, List<string>>();
            List<string> buildinKWGroup = null;
            var pipelineType = SVCUtility.GetProjectRenderPipelineType();
            if (pipelineType == ERenderPipelineType.BIRP)
            {
                // TODO
            }
            else
            {
                if (sceneInfoDic != null && sceneInfoDic.Count > 0)
                {
                    foreach (var data in sceneInfoDic)
                    {
                        var mat = data.Key;
                        var sceneInfoList = data.Value;

                        if (!buildinKWGroupDic.TryGetValue(mat, out buildinKWGroup))
                        {
                            buildinKWGroup = new List<string>();
                            buildinKWGroupDic[mat] = buildinKWGroup;
                        }

                        var oldBuiltinTransferShader = BuiltinTransferedShader();
                        foreach (var sceneInfo in sceneInfoList)
                        {
                            var dirMode = sceneInfo.m_LightingDirectionalMode;
                            var urpCombinedKeywords = GenerateURPCombinedKeywords(sceneInfo.m_MainDirLight && sceneInfo.m_MainDirLightWithShadow, sceneInfo.m_MainDirLight && sceneInfo.m_MainDirLightWithSoftShadow);

                            // Fog /////////////////////////////////////////
                            var fogKW = GetFogKW(sceneInfo);
                            // GI //////////////////////////////////////////
                            // Realtime Scene
                            if (sceneInfo.m_IsLightingDataAssetNull)
                            {
                                if (m_PassType == PassType.ShadowCaster)
                                {
                                    // 兼容Builtin转过来的Shader
                                    if (sceneInfo.m_MainDirLight && sceneInfo.m_MainDirLightWithShadow)
                                    {
                                        // 兼容Builtin转过来的Shader
                                        if (oldBuiltinTransferShader)
                                        {
                                            var kwGroup = SVCSettings.UNITY_KW_SHADOWS_DEPTH;
                                            if (!buildinKWGroup.Contains(kwGroup))
                                                buildinKWGroup.Add(kwGroup);
                                        }
                                        // URP
                                        else
                                        {
                                            buildinKWGroup.Add("");
                                        }
                                    }
                                }
                                else if (m_PassType == PassType.ScriptableRenderPipeline)
                                {
                                    // 兼容Builtin转过来的Shader
                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRECTIONAL + KW_SPLIT_CHAR + SVCSettings.UNITY_KW_LIGHTPROBE_SH;
                                    if (!buildinKWGroup.Contains(kwGroup))
                                        buildinKWGroup.Add(kwGroup);
                                }
                            }
                            // Realtime GI Scene
                            else if (sceneInfo.m_EnabledRealtimeGI && !sceneInfo.m_EnabledBakedGI)
                            {
                                if (m_PassType == PassType.ShadowCaster)
                                {
                                    // 兼容Builtin转过来的Shader
                                    if (sceneInfo.m_MainDirLight && sceneInfo.m_MainDirLightWithShadow)
                                    {
                                        // 兼容Builtin转过来的Shader
                                        if (oldBuiltinTransferShader)
                                        {
                                            var kwGroup = SVCSettings.UNITY_KW_SHADOWS_DEPTH;
                                            if (!buildinKWGroup.Contains(kwGroup))
                                                buildinKWGroup.Add(kwGroup);
                                        }
                                        // URP
                                        else
                                        {
                                            buildinKWGroup.Add("");
                                        }
                                    }
                                }
                                else if (m_PassType == PassType.ScriptableRenderPipeline)
                                {
                                    if (sceneInfo.m_ExistContributeGITrue)
                                    {
                                        // 兼容Builtin转过来的Shader
                                        if (oldBuiltinTransferShader)
                                        {
                                            var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRECTIONAL + KW_SPLIT_CHAR
                                                            + SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR +
                                                            SVCSettings.UNITY_KW_DYNAMICLIGHTMAP_ON;
                                            if (dirMode == UnityEngine.LightmapsMode.NonDirectional)
                                                kwGroup = kwGroup.Replace(SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR, "");
                                            if (!buildinKWGroup.Contains(kwGroup))
                                                buildinKWGroup.Add(kwGroup);
                                        }
                                        // URP
                                        else
                                        {
                                            var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR + SVCSettings.UNITY_KW_DYNAMICLIGHTMAP_ON;
                                            if (dirMode == UnityEngine.LightmapsMode.NonDirectional)
                                                kwGroup = kwGroup.Replace(SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR, "");
                                            if (!buildinKWGroup.Contains(kwGroup))
                                                buildinKWGroup.Add(kwGroup);
                                        }
                                    }

                                    if (sceneInfo.m_ExistContributeGIFalse)
                                    {
                                        // 兼容Builtin转过来的Shader
                                        var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRECTIONAL + KW_SPLIT_CHAR + SVCSettings.UNITY_KW_LIGHTPROBE_SH;
                                        if (!buildinKWGroup.Contains(kwGroup))
                                            buildinKWGroup.Add(kwGroup);
                                        // URP -NULL
                                    }
                                }
                            }
                            else if (sceneInfo.m_EnabledBakedGI)
                            {
                                var mixedBakeMode = sceneInfo.m_MixedBakeMode;
                                switch (mixedBakeMode)
                                {
                                    // Baked Indirect Scene
                                    case UnityEngine.MixedLightingMode.IndirectOnly:
                                        if (m_PassType == PassType.ShadowCaster)
                                        {
                                            // 兼容Builtin转过来的Shader
                                            if (sceneInfo.m_MainDirLight && sceneInfo.m_MainDirLightWithShadow)
                                            {
                                                // 兼容Builtin转过来的Shader
                                                if (oldBuiltinTransferShader)
                                                {
                                                    var kwGroup = SVCSettings.UNITY_KW_SHADOWS_DEPTH;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                                // URP
                                                else
                                                {
                                                    buildinKWGroup.Add("");
                                                }
                                            }
                                        }
                                        else if (m_PassType == PassType.ScriptableRenderPipeline)
                                        {
                                            if (sceneInfo.m_ExistContributeGITrue)
                                            {
                                                // 兼容Builtin转过来的Shader
                                                if (oldBuiltinTransferShader)
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRECTIONAL + KW_SPLIT_CHAR
                                                  + SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR +
                                                  SVCSettings.UNITY_KW_LIGHTMAP_ON;

                                                    if (dirMode == UnityEngine.LightmapsMode.NonDirectional)
                                                        kwGroup = kwGroup.Replace(SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR, "");
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                                // URP
                                                else
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR + SVCSettings.UNITY_KW_LIGHTMAP_ON;
                                                    if (dirMode == UnityEngine.LightmapsMode.NonDirectional)
                                                        kwGroup = kwGroup.Replace(SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR, "");
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                            }

                                            if (sceneInfo.m_ExistContributeGIFalse)
                                            {
                                                // 兼容Builtin转过来的Shader
                                                if (oldBuiltinTransferShader)
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRECTIONAL + KW_SPLIT_CHAR + SVCSettings.UNITY_KW_LIGHTPROBE_SH;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                                // URP
                                                else
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                            }
                                        }
                                        break;
                                    // Baked Shadowmask Scene
                                    case UnityEngine.MixedLightingMode.Shadowmask:
                                        var shadowMaskMode = SVCUtility.GetShadowmaskMode();
                                        if (m_PassType == PassType.ShadowCaster)
                                        {
                                            if (sceneInfo.m_ExistContributeGITrue && shadowMaskMode == UnityEngine.ShadowmaskMode.DistanceShadowmask)
                                            {
                                                // 兼容Builtin转过来的Shader
                                                if (sceneInfo.m_MainDirLight && sceneInfo.m_MainDirLightWithShadow)
                                                {
                                                    var kwGroup = SVCSettings.UNITY_KW_SHADOWS_DEPTH;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                            }

                                            if (sceneInfo.m_ExistContributeGIFalse)
                                            {
                                                // 兼容Builtin转过来的Shader
                                                if (sceneInfo.m_MainDirLight && sceneInfo.m_MainDirLightWithShadow)
                                                {
                                                    // 兼容Builtin转过来的Shader
                                                    if (oldBuiltinTransferShader)
                                                    {
                                                        var kwGroup = SVCSettings.UNITY_KW_SHADOWS_DEPTH;
                                                        if (!buildinKWGroup.Contains(kwGroup))
                                                            buildinKWGroup.Add(kwGroup);
                                                    }
                                                    // URP
                                                    else
                                                    {
                                                        buildinKWGroup.Add("");
                                                    }
                                                }
                                            }
                                        }
                                        else if (m_PassType == PassType.ScriptableRenderPipeline)
                                        {
                                            if (sceneInfo.m_ExistContributeGITrue)
                                            {
                                                // 兼容Builtin转过来的Shader
                                                if (oldBuiltinTransferShader)
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRECTIONAL + KW_SPLIT_CHAR
                                                        + SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR +
                                                        SVCSettings.UNITY_KW_LIGHTMAP_ON + KW_SPLIT_CHAR +
                                                        SVCSettings.UNITY_KW_LIGHTMAP_SHADOW_MIXING + KW_SPLIT_CHAR +
                                                        SVCSettings.UNITY_KW_SHADOWS_SHADOWMASK;
                                                    if (dirMode == UnityEngine.LightmapsMode.NonDirectional)
                                                        kwGroup = kwGroup.Replace(SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR, "");
                                                    if (shadowMaskMode == UnityEngine.ShadowmaskMode.DistanceShadowmask)
                                                        kwGroup = kwGroup.Replace(SVCSettings.UNITY_KW_LIGHTMAP_SHADOW_MIXING + KW_SPLIT_CHAR, "");
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                                // URP
                                                else
                                                {
                                                    var commonKWGroup = fogKW + SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR +
                                                                   SVCSettings.UNITY_KW_LIGHTMAP_ON + KW_SPLIT_CHAR + SVCSettings.UNITY_KW_SHADOWS_SHADOWMASK;
                                                    if (dirMode == UnityEngine.LightmapsMode.NonDirectional)
                                                        commonKWGroup = commonKWGroup.Replace(SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR, "");

                                                    var urpCombinedKeywords1 = GenerateURPShadowMaskCombinedKeywords(sceneInfo.m_MainDirLight && sceneInfo.m_MainDirLightWithShadow, sceneInfo.m_MainDirLight && sceneInfo.m_MainDirLightWithSoftShadow);
                                                    // 1
                                                    var kwGroup = urpCombinedKeywords + commonKWGroup;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                    // 2
                                                    kwGroup = urpCombinedKeywords1 + commonKWGroup;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);

                                                    // 3
                                                    commonKWGroup += KW_SPLIT_CHAR + SVCSettings.UNITY_KW_LIGHTMAP_SHADOW_MIXING;
                                                    kwGroup = urpCombinedKeywords + commonKWGroup;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                    // 4
                                                    kwGroup = urpCombinedKeywords1 + commonKWGroup;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);

                                                }
                                            }

                                            if (sceneInfo.m_ExistContributeGIFalse)
                                            {
                                                // 兼容Builtin转过来的Shader
                                                if (oldBuiltinTransferShader)
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRECTIONAL + KW_SPLIT_CHAR + SVCSettings.UNITY_KW_LIGHTPROBE_SH + KW_SPLIT_CHAR + SVCSettings.UNITY_KW_SHADOWS_SHADOWMASK;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                                // URP
                                                else
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_LIGHTMAP_SHADOW_MIXING + KW_SPLIT_CHAR + SVCSettings.UNITY_KW_SHADOWS_SHADOWMASK;
                                                    if (shadowMaskMode == UnityEngine.ShadowmaskMode.DistanceShadowmask)
                                                        kwGroup = kwGroup.Replace(SVCSettings.UNITY_KW_LIGHTMAP_SHADOW_MIXING + KW_SPLIT_CHAR, "");
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                            }
                                        }
                                        break;
                                    // Baked Subtractive Scene
                                    case UnityEngine.MixedLightingMode.Subtractive:
                                        if (m_PassType == PassType.ShadowCaster)
                                        {
                                            if (sceneInfo.m_ExistContributeGIFalse && sceneInfo.m_MainDirLight && sceneInfo.m_MainDirLightWithShadow)
                                            {
                                                // 兼容Builtin转过来的Shader
                                                if (oldBuiltinTransferShader)
                                                {
                                                    var kwGroup = SVCSettings.UNITY_KW_SHADOWS_DEPTH;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                                // URP
                                                else
                                                {
                                                    buildinKWGroup.Add("");
                                                }
                                            }
                                        }
                                        else if (m_PassType == PassType.ScriptableRenderPipeline)
                                        {
                                            if (sceneInfo.m_ExistContributeGITrue)
                                            {
                                                // 兼容Builtin转过来的Shader
                                                if (oldBuiltinTransferShader)
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRECTIONAL + KW_SPLIT_CHAR
                                                                    + SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR +
                                                                    SVCSettings.UNITY_KW_LIGHTMAP_ON + KW_SPLIT_CHAR +
                                                                    SVCSettings.UNITY_KW_LIGHTMAP_SHADOW_MIXING;
                                                    if (dirMode == UnityEngine.LightmapsMode.NonDirectional)
                                                        kwGroup = kwGroup.Replace(SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR, "");
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                                // URP
                                                else
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR +
                                                                   SVCSettings.UNITY_KW_LIGHTMAP_ON + KW_SPLIT_CHAR +
                                                                   SVCSettings.UNITY_KW_LIGHTMAP_SHADOW_MIXING;
                                                    if (dirMode == UnityEngine.LightmapsMode.NonDirectional)
                                                        kwGroup = kwGroup.Replace(SVCSettings.UNITY_KW_DIRLIGHTMAP_COMBINED + KW_SPLIT_CHAR, "");
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                            }

                                            if (sceneInfo.m_ExistContributeGIFalse)
                                            {
                                                // 兼容Builtin转过来的Shader
                                                if (oldBuiltinTransferShader)
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_DIRECTIONAL + KW_SPLIT_CHAR + SVCSettings.UNITY_KW_LIGHTPROBE_SH;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                                // URP
                                                else
                                                {
                                                    var kwGroup = urpCombinedKeywords + fogKW + SVCSettings.UNITY_KW_LIGHTMAP_SHADOW_MIXING;
                                                    if (!buildinKWGroup.Contains(kwGroup))
                                                        buildinKWGroup.Add(kwGroup);
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }

                    }
                }

            }

            CombineBuiltinKWVariantInternal(buildinKWGroupDic);
        }

        #endregion

        #region Private Methods

        private string GetFogKW(SceneInfo sceneInfo)
        {
            string fogKW = "";
            if (sceneInfo.m_EnableFog)
            {
                switch (sceneInfo.m_FogMode)
                {
                    // Linear
                    case UnityEngine.FogMode.Linear:
                        fogKW = SVCSettings.UNITY_KW_FOG_LINEAR + KW_SPLIT_CHAR;
                        break;
                    // Exponential
                    case UnityEngine.FogMode.Exponential:
                        fogKW = SVCSettings.UNITY_KW_FOG_EXP + KW_SPLIT_CHAR;
                        break;
                    // ExponentialSquared
                    case UnityEngine.FogMode.ExponentialSquared:
                        fogKW = SVCSettings.UNITY_KW_FOG_EXP2 + KW_SPLIT_CHAR;
                        break;
                }
            }

            return fogKW;
        }

        private bool BuiltinTransferedShader()
        {
            if (m_PassType == PassType.ScriptableRenderPipeline)
                return m_Keywords.Contains(SVCSettings.UNITY_KW_DIRECTIONAL);
            else if (m_PassType == PassType.ShadowCaster)
                return m_Keywords.Contains(SVCSettings.UNITY_KW_SHADOWS_DEPTH);

            return false;
        }

        private string GenerateURPCombinedKeywords(bool mainLightHasShadow, bool mainLightHasSoftShadow)
        {
            string urpCombinedKeywords = "";

            if (m_PassType == PassType.ScriptableRenderPipeline)
            {
                // Main Light - GI
                var mainLightDisabled = URPSetting.MainLightIsDisabledFromURPSetting(m_RenderPipelineObj);
                var castShadow = URPSetting.MainLightShadowsSupportedFromURPSetting(m_RenderPipelineObj);
                if (!mainLightDisabled && castShadow)
                {
                    if (mainLightHasShadow)
                    {
                        var cascadeCount = URPSetting.GetCascadeCountFromURPSetting(m_RenderPipelineObj);
                        if (cascadeCount == 1)
                        {
                            if (m_Keywords.Contains(SVCSettings.URP_MAIN_LIGHT_SHADOWS))
                                urpCombinedKeywords += SVCSettings.URP_MAIN_LIGHT_SHADOWS + KW_SPLIT_CHAR;

                        }
                        else if (cascadeCount > 1)
                        {
                            if (m_Keywords.Contains(SVCSettings.URP_MAIN_LIGHT_SHADOWS_CASCADE))
                                urpCombinedKeywords += SVCSettings.URP_MAIN_LIGHT_SHADOWS_CASCADE + KW_SPLIT_CHAR;
                        }
                    }
                    else
                    {
                        if (m_Keywords.Contains(SVCSettings.URP_MAIN_LIGHT_SHADOWS))
                            urpCombinedKeywords += SVCSettings.URP_MAIN_LIGHT_SHADOWS + KW_SPLIT_CHAR;
                    }
                }

                // Additional Light
                var additionalLightMode = URPSetting.GetAdditionalLightModeFromURPSetting(m_RenderPipelineObj);
                var additionalLightShadowsSupported = URPSetting.AdditionalLightShadowsSupportedFromURPSetting(m_RenderPipelineObj);
                if (additionalLightMode != 0)
                {
                    // PerVertex
                    if (additionalLightMode == 2)
                    {
                        if (m_Keywords.Contains(SVCSettings.URP_ADDITIONAL_LIGHTS_VERTEX))
                            urpCombinedKeywords += SVCSettings.URP_ADDITIONAL_LIGHTS_VERTEX + KW_SPLIT_CHAR;
                    }
                    // PerPixel
                    else if (additionalLightMode == 1)
                    {
                        if (additionalLightShadowsSupported && m_Keywords.Contains(SVCSettings.URP_ADDITIONAL_LIGHT_SHADOWS))
                            urpCombinedKeywords += SVCSettings.URP_ADDITIONAL_LIGHT_SHADOWS + KW_SPLIT_CHAR;
                        if (m_Keywords.Contains(SVCSettings.URP_ADDITIONAL_LIGHTS))
                            urpCombinedKeywords += SVCSettings.URP_ADDITIONAL_LIGHTS + KW_SPLIT_CHAR;
                    }
                }

                // Soft Shadow
                var shadowDistance = URPSetting.GetShadowDistanceFromURPSetting(m_RenderPipelineObj);
                var softShadowSupported = URPSetting.SoftShadowSupportedFromURPSetting(m_RenderPipelineObj);
                if (shadowDistance > 0.0f && softShadowSupported && mainLightHasShadow)
                {
                    if (m_Keywords.Contains(SVCSettings.URP_SHADOWS_SOFT))
                        urpCombinedKeywords += SVCSettings.URP_SHADOWS_SOFT + KW_SPLIT_CHAR;
                }

                // SH
                var shEvaluationMode = URPSetting.GetSHEvaluationModeFromURPSetting(m_RenderPipelineObj);
                // SH Auto 
                if (shEvaluationMode == 0)
                {
#if UNITY_ANDROID || UNITY_IOS
                    if (m_Keywords.Contains(SVCSettings.URP_EVALUATE_SH_VERTEX))
                        urpCombinedKeywords += SVCSettings.URP_EVALUATE_SH_VERTEX + KW_SPLIT_CHAR;
#else
                     // SH PerPixel Null
#endif
                }
                // SH PerVertex
                else if (shEvaluationMode == 1)
                {
                    if (m_Keywords.Contains(SVCSettings.URP_EVALUATE_SH_VERTEX))
                        urpCombinedKeywords += SVCSettings.URP_EVALUATE_SH_VERTEX + KW_SPLIT_CHAR;
                }
                // SH PerMixed
                else if (shEvaluationMode == 2)
                {
                    if (m_Keywords.Contains(SVCSettings.URP_EVALUATE_SH_MIXED))
                        urpCombinedKeywords += SVCSettings.URP_EVALUATE_SH_MIXED + KW_SPLIT_CHAR;
                }
                // SH PerPixel
                else if (shEvaluationMode == 3)
                {
                    // Null
                }

                // Reflection Probe
                if (URPSetting.EnabledReflectionProbeBlendingFromURPSetting(m_RenderPipelineObj) && m_Keywords.Contains(SVCSettings.URP_REFLECTION_PROBE_BLENDING))
                    urpCombinedKeywords += SVCSettings.URP_REFLECTION_PROBE_BLENDING + KW_SPLIT_CHAR;
                if (URPSetting.EnabledReflectionProbeBoxProjectionFromURPSetting(m_RenderPipelineObj) && m_Keywords.Contains(SVCSettings.URP_REFLECTION_PROBE_BOX_PROJECTION))
                    urpCombinedKeywords += SVCSettings.URP_REFLECTION_PROBE_BOX_PROJECTION + KW_SPLIT_CHAR;
            }

            return urpCombinedKeywords;
        }

        private string GenerateURPShadowMaskCombinedKeywords(bool mainLightHasShadow, bool mainLightHasSoftShadow)
        {
            string urpCombinedKeywords = "";

            if (m_PassType == PassType.ScriptableRenderPipeline)
            {
                // Main Light - TODO - GI
                var mainLightDisabled = URPSetting.MainLightIsDisabledFromURPSetting(m_RenderPipelineObj);
                var castShadow = URPSetting.MainLightShadowsSupportedFromURPSetting(m_RenderPipelineObj);
                if (!mainLightDisabled && castShadow)
                {
                    if (m_Keywords.Contains(SVCSettings.URP_MAIN_LIGHT_SHADOWS))
                        urpCombinedKeywords += SVCSettings.URP_MAIN_LIGHT_SHADOWS + KW_SPLIT_CHAR;
                }

                // Additional Light
                var additionalLightMode = URPSetting.GetAdditionalLightModeFromURPSetting(m_RenderPipelineObj);
                var additionalLightShadowsSupported = URPSetting.AdditionalLightShadowsSupportedFromURPSetting(m_RenderPipelineObj);
                if (additionalLightMode != 0)
                {
                    // PerVertex
                    if (additionalLightMode == 2)
                    {
                        if (m_Keywords.Contains(SVCSettings.URP_ADDITIONAL_LIGHTS_VERTEX))
                            urpCombinedKeywords += SVCSettings.URP_ADDITIONAL_LIGHTS_VERTEX + KW_SPLIT_CHAR;
                    }
                    // PerPixel
                    else if (additionalLightMode == 1)
                    {
                        if (additionalLightShadowsSupported && m_Keywords.Contains(SVCSettings.URP_ADDITIONAL_LIGHT_SHADOWS))
                            urpCombinedKeywords += SVCSettings.URP_ADDITIONAL_LIGHT_SHADOWS + KW_SPLIT_CHAR;
                        if (m_Keywords.Contains(SVCSettings.URP_ADDITIONAL_LIGHTS))
                            urpCombinedKeywords += SVCSettings.URP_ADDITIONAL_LIGHTS + KW_SPLIT_CHAR;
                    }
                }

                // Soft Shadow - No Soft

                // SH
                var shEvaluationMode = URPSetting.GetSHEvaluationModeFromURPSetting(m_RenderPipelineObj);
                // SH Auto 
                if (shEvaluationMode == 0)
                {
#if UNITY_ANDROID || UNITY_IOS
                    if (m_Keywords.Contains(SVCSettings.URP_EVALUATE_SH_VERTEX))
                        urpCombinedKeywords += SVCSettings.URP_EVALUATE_SH_VERTEX + KW_SPLIT_CHAR;
#else
                    // SH PerPixel Null
#endif
                }
                // SH PerVertex
                else if (shEvaluationMode == 1)
                {
                    if (m_Keywords.Contains(SVCSettings.URP_EVALUATE_SH_VERTEX))
                        urpCombinedKeywords += SVCSettings.URP_EVALUATE_SH_VERTEX + KW_SPLIT_CHAR;
                }
                // SH PerMixed
                else if (shEvaluationMode == 2)
                {
                    if (m_Keywords.Contains(SVCSettings.URP_EVALUATE_SH_MIXED))
                        urpCombinedKeywords += SVCSettings.URP_EVALUATE_SH_MIXED + KW_SPLIT_CHAR;
                }
                // SH PerPixel
                else if (shEvaluationMode == 3)
                {
                    // Null
                }

                // Reflection Probe
                if (URPSetting.EnabledReflectionProbeBlendingFromURPSetting(m_RenderPipelineObj) && m_Keywords.Contains(SVCSettings.URP_REFLECTION_PROBE_BLENDING))
                    urpCombinedKeywords += SVCSettings.URP_REFLECTION_PROBE_BLENDING + KW_SPLIT_CHAR;
                if (URPSetting.EnabledReflectionProbeBoxProjectionFromURPSetting(m_RenderPipelineObj) && m_Keywords.Contains(SVCSettings.URP_REFLECTION_PROBE_BOX_PROJECTION))
                    urpCombinedKeywords += SVCSettings.URP_REFLECTION_PROBE_BOX_PROJECTION + KW_SPLIT_CHAR;
            }

            return urpCombinedKeywords;
        }

        private void CombineBuiltinKWVariantInternal(Dictionary<Material, List<string>> buildinKWGroupDic)
        {
            List<Material> noSceneRefMats = new List<Material>();
            if (buildinKWGroupDic.Count == 0)
            {
                foreach (var mat in m_CombineKeywords.Keys)
                    noSceneRefMats.Add(mat);
            }
            else
            {
                List<Material> sceneRefMats = new List<Material>();
                foreach (var data in buildinKWGroupDic)
                {
                    var mat = data.Key;
                    var builtinKWGroups = data.Value;
                    if (builtinKWGroups == null) continue;

                    List<string[]> cKWs = null;
                    if (m_CombineKeywords.TryGetValue(mat, out cKWs))
                    {
                        List<string[]> finalGroup = new List<string[]>();
                        foreach (var builtinKWGroup in builtinKWGroups)
                        {
                            var builtinKWs = new List<string>(builtinKWGroup.Split(KW_SPLIT_CHAR));
                            for (int i = builtinKWs.Count - 1; i >= 0; i--)
                            {
                                if (!m_Keywords.Contains(builtinKWs[i]))
                                    builtinKWs.RemoveAt(i);
                            }

                            foreach (var cKW in cKWs)
                            {
                                List<string> kwList = new List<string>(cKW);
                                kwList.AddRange(builtinKWs);
                                kwList.Sort((x, y) => string.CompareOrdinal(x, y));
                                finalGroup.Add(kwList.ToArray());
                            }
                        }

                        m_CombineKeywords[mat] = finalGroup;
                        sceneRefMats.Add(mat);
                    }
                }

                foreach (var mat in m_CombineKeywords.Keys)
                {
                    if (!sceneRefMats.Contains(mat))
                        noSceneRefMats.Add(mat);
                }
            }

            if (noSceneRefMats.Count > 0)
            {
                List<string> builtinKWs = new List<string>();
                if (m_PassType == PassType.ShadowCaster)
                {
                    if (m_Keywords.Contains(SVCSettings.UNITY_KW_SHADOWS_DEPTH))
                        builtinKWs.Add(SVCSettings.UNITY_KW_SHADOWS_DEPTH);
                }
                else if (m_PassType == PassType.ScriptableRenderPipeline)
                {
                    var urpCombinedKeywords = GenerateURPCombinedKeywords(true, true);
                    var arr = urpCombinedKeywords.Split(KW_SPLIT_CHAR);
                    foreach (var kw in arr)
                    {
                        builtinKWs.Add(kw);
                    }

                    if (m_Keywords.Contains(SVCSettings.UNITY_KW_DIRECTIONAL))
                        builtinKWs.Add(SVCSettings.UNITY_KW_DIRECTIONAL);
                    if (m_Keywords.Contains(SVCSettings.UNITY_KW_LIGHTPROBE_SH))
                        builtinKWs.Add(SVCSettings.UNITY_KW_LIGHTPROBE_SH);

                }

                if (builtinKWs.Count > 0)
                {
                    foreach (var mat in noSceneRefMats)
                    {
                        List<string[]> cKWs = m_CombineKeywords[mat];
                        List<string[]> finalGroup = new List<string[]>();
                        foreach (var cKW in cKWs)
                        {
                            List<string> kwList = new List<string>(cKW);
                            kwList.AddRange(builtinKWs);
                            kwList.Sort((x, y) => string.CompareOrdinal(x, y));
                            finalGroup.Add(kwList.ToArray());
                        }

                        m_CombineKeywords[mat] = finalGroup;
                    }
                }
            }
        }

        #endregion
    }

}