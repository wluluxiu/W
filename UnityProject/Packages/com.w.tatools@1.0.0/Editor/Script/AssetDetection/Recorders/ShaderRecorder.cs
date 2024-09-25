using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    /// <summary>
    /// 检测项目：
    /// 1.Keyword
    /// </summary>
    internal class ShaderRecorder : BaseRecorder
    {
        #region Fields

        internal List<string> m_KeywordsList = new List<string>();

        #endregion

        #region Override Methods

        internal override void Record(string assetPath, EAssetType assetType)
        {
            base.Record(assetPath, assetType);

            var shader = AssetDatabase.LoadAssetAtPath<Shader>(assetPath);

            var globalKeywords = AssetDetectionUtility.GetShaderGlobalKeywords(shader);
            if (globalKeywords != null)
            {
                foreach (var kw in globalKeywords)
                {
                    if (!this.m_KeywordsList.Contains(kw))
                        this.m_KeywordsList.Add(kw);
                }
            }
            var localKeywords = AssetDetectionUtility.GetShaderLocalKeywords(shader);
            if (localKeywords != null)
            {
                foreach (var kw in localKeywords)
                {
                    if (!this.m_KeywordsList.Contains(kw))
                        this.m_KeywordsList.Add(kw);
                }
            }

            shader = null;
        }

        /// <summary>
        /// BaseFormat|Keyword1#Keyword2#Keyword3#...
        /// </summary>
        /// <returns></returns>
        internal override string GetOutputStr()
        {
            var baseOutputStr = base.GetOutputStr();

            string spiltStr = CHAR_SPLIT_FIRST_FLAG.ToString();

            // Keywords
            string keywordsStr = "";
            foreach (var kw in this.m_KeywordsList)
                keywordsStr += kw + CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(keywordsStr))
                keywordsStr = keywordsStr.Substring(0, keywordsStr.Length - 1);

            return baseOutputStr + spiltStr + keywordsStr;
        }

        internal override void ParseStrLine(string stringLine)
        {
            base.ParseStrLine(stringLine);

            if (base.m_SourceDataArr.Length > 7 && !string.IsNullOrEmpty(base.m_SourceDataArr[7]))
            {
                string[] arr = base.m_SourceDataArr[7].Split(CHAR_SPLIT_SECOND_FLAG);
                this.m_KeywordsList = new List<string>(arr);
            }
        }

        internal override void Release()
        {
            base.Release();

            this.m_KeywordsList.Clear();
            this.m_KeywordsList = null;
        }

        #endregion
    }
}
