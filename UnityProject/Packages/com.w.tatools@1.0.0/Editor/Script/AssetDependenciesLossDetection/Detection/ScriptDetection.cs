namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;

    internal class ScriptDetection : BaseDetection
    {
        #region Fields

        const string START_COLLECT_FLAG = "  defaultReferences:";
        const string GUID_START_WITH = "  - ";
        const string GUID_PROP = "guid:";
        const char SPLIT_CHAR_0 = ',';
        const char SPLIT_CHAR_1 = ':';

        #endregion

        #region Override Methods

        internal override void DoDetection(string assetPath)
        {
            base.DoDetection(assetPath);

            var metaFilePath = assetPath + ".meta";
            bool collectGuidFlag = false;
            StreamReader sr = new StreamReader(metaFilePath);
            var lineStr = sr.ReadLine();
            while (lineStr != null)
            {
                if (lineStr == START_COLLECT_FLAG)
                {
                    collectGuidFlag = true;
                }
                else if (lineStr.StartsWith(GUID_START_WITH) && collectGuidFlag)
                {
                    lineStr = lineStr.Replace(GUID_START_WITH, "").Replace(" ", "");
                    var arr = lineStr.Split(SPLIT_CHAR_0);
                    if (arr.Length == 3)
                    {
                        lineStr = arr[1];
                        if (lineStr.StartsWith(GUID_PROP))
                        {
                            var guid = lineStr.Replace(GUID_PROP, "");
                            if (!string.IsNullOrEmpty(guid))
                            {
                                var refAssetPath = AssetDatabase.GUIDToAssetPath(guid);
                                if (!File.Exists(refAssetPath) && !DependenciesLossDetectionSettings.Instance.IsUnityBuiltinAsset(refAssetPath))
                                {
                                    if (base.m_LossGuids == null) base.m_LossGuids = new List<string>();
                                    base.m_LossGuids.Add(guid);

                                    var propName = arr[0].Split(SPLIT_CHAR_1)[0];
                                    if (base.m_LossGuidPropNames == null) base.m_LossGuidPropNames = new List<string>();
                                    base.m_LossGuidPropNames.Add(propName);
                                }
                            }
                        }
                    }
                }

                lineStr = sr.ReadLine();
            }

            sr.Close();
            sr.Dispose();
            sr = null;
        }

        internal override bool IsInvalid()
        {
            return base.m_LossGuids != null && base.m_LossGuids.Count > 0;
        }

        #endregion 
    }
}