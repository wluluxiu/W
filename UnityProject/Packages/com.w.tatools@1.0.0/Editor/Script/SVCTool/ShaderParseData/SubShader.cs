namespace jj.TATools.Editor
{
    using System.Collections.Generic;
    using UnityEngine;

    internal class SubShader
    {
        #region Fields

        internal int m_SubShaderIndex;
        internal Dictionary<int, Pass> m_DataMapping = new Dictionary<int, Pass>();

        #endregion

        internal SubShader(int subshaderIndex)
        {
            this.m_SubShaderIndex = subshaderIndex;
        }

        #region Internal Methods

        internal void SetRenderPipeline(UnityEngine.Object renderPipelineObj)
        {
            foreach (var data in this.m_DataMapping)
            {
                data.Value.SetRenderPipeline(renderPipelineObj);
            }
        }

        internal SubShader Clone(UnityEngine.Object renderPipelineObj)
        {
            SubShader cloenSubShader = new SubShader(this.m_SubShaderIndex);
            cloenSubShader.m_DataMapping = new Dictionary<int, Pass>();
            foreach(var data in this.m_DataMapping)
            {
                cloenSubShader.m_DataMapping[data.Key] = data.Value.Clone(renderPipelineObj);
            }

            return cloenSubShader;
        }

        internal void AddPass(int passID, Pass pass)
        {
            m_DataMapping[passID] = pass;
        }

        internal void AddMaterialVariantKeywordGroup(string keywordGroup,Material mat)
        {
            foreach (var pass in m_DataMapping.Values)
            {
                pass.AddMaterialVariantKeywordGroup(keywordGroup,mat);
            }
        }

        internal void AddDynamicVariantKeywordGroup(string[] keywordGroup)
        {
            foreach (var pass in m_DataMapping.Values)
            {
                pass.AddDynamicVariantKeywordGroup(keywordGroup);
            }
        }

        internal void CombineMaterialAndDynamicVariant()
        {
            foreach (var pass in m_DataMapping.Values)
            {
                pass.CombineMaterialAndDynamicVariant();
            }
        }

        internal void CombineBuiltinKWVariant(Dictionary<Material, List<SceneInfo>> sceneInfoDic)
        {
            foreach (var pass in m_DataMapping.Values)
            {
                pass.CombineBuiltinKWVariant(sceneInfoDic);
            }
        }

        #endregion
    }
}