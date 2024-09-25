using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    /// <summary>
    /// 检测项目：
    /// 1.内置Dep
    /// </summary>
    internal class ScriptRecorder : BaseRecorder
    {

        #region Override Methods

        internal override void Record(string assetPath, EAssetType assetType)
        {
            base.Record(assetPath, assetType);

            var script = AssetDatabase.LoadAssetAtPath<UnityEditor.MonoScript>(assetPath);
            base.m_BuiltinDependencies = AssetDetectionUtility.GetBuiltinDenpendencies(script);
            script = null;
        }

        /// <summary>
        /// BaseFormat|BuiltinDep1#BuiltinDep2#BuiltinDep3#...
        /// </summary>
        /// <returns></returns>
        internal override string GetOutputStr()
        {
            var baseOutputStr = base.GetOutputStr();

            string spiltStr = CHAR_SPLIT_FIRST_FLAG.ToString();

            // Builtin Dep
            string builtinDepStr = "";
            foreach (var nodePath in base.m_BuiltinDependencies)
                builtinDepStr += nodePath + CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(builtinDepStr))
                builtinDepStr = builtinDepStr.Substring(0, builtinDepStr.Length - 1);

            return baseOutputStr + spiltStr + builtinDepStr;
        }

        internal override void ParseStrLine(string stringLine)
        {
            base.ParseStrLine(stringLine);

            if (base.m_SourceDataArr.Length > 7 && !string.IsNullOrEmpty(base.m_SourceDataArr[7]))
            {
                string[] arr = base.m_SourceDataArr[7].Split(CHAR_SPLIT_SECOND_FLAG);
                base.m_BuiltinDependencies = new List<string>(arr);
            }
        }

        #endregion
    }
}
