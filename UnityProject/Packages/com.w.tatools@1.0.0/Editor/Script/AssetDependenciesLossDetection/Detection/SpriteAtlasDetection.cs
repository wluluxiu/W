namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;

    internal class SpriteAtlasDetection : BaseDetection
    {
        #region Fields

        const string MASTER_ATLAS_PROP_STARTWITH = "  m_MasterAtlas:";
        const string START_COLLECT_FLAG = "    packables:";
        const string GUID_START_WITH = "    - {fileID:";
        const string GUID_PROP = "guid:";
        const char SPLIT_CHAR_0 = ',';

        internal string m_LossMasterGuid;

        #endregion

        #region Override Methods

        internal override void DoDetection(string assetPath)
        {
            base.DoDetection(assetPath);

            bool collectGuidFlag = false;
            StreamReader sr = new StreamReader(assetPath);
            var lineStr = sr.ReadLine();
            while (lineStr != null)
            {
                if (lineStr.StartsWith(MASTER_ATLAS_PROP_STARTWITH) && lineStr.Contains(GUID_PROP))
                {
                    lineStr = lineStr.Replace(" ", "");
                    var arr = lineStr.Split(SPLIT_CHAR_0);
                    if (arr.Length == 3)
                    {
                        lineStr = arr[1];
                        var guid = lineStr.Replace(GUID_PROP, "");
                        if (!string.IsNullOrEmpty(guid))
                        {
                            var refAssetPath = AssetDatabase.GUIDToAssetPath(guid);
                            if (!File.Exists(refAssetPath) && !DependenciesLossDetectionSettings.Instance.IsUnityBuiltinAsset(refAssetPath))
                            {
                                m_LossMasterGuid = guid;
                            }
                        }
                    }
                }
                else if (lineStr == START_COLLECT_FLAG)
                {
                    collectGuidFlag = true;
                }
                else if (lineStr.StartsWith(GUID_START_WITH) && collectGuidFlag)
                {
                    lineStr = lineStr.Replace(" ", "");
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
                                }
                            }
                        }
                    }
                }
                else
                    collectGuidFlag = false;

                lineStr = sr.ReadLine();
            }

            sr.Close();
            sr.Dispose();
            sr = null;
        }

        internal override bool IsInvalid()
        {
            return !string.IsNullOrEmpty(this.m_LossMasterGuid) || (base.m_LossGuids != null && base.m_LossGuids.Count > 0);
        }

        #endregion 
    }
}