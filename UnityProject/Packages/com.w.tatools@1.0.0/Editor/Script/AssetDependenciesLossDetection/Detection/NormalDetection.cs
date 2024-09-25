namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;

    internal class NormalDetection : BaseDetection
    {
        #region Fields

        const string GUID_PROP = "guid:";
        const char SPLIT_CHAR_0 = ',';

        #endregion

        #region Override Methods

        internal override void DoDetection(string assetPath)
        {
            base.DoDetection(assetPath);

            StreamReader sr = new StreamReader(assetPath);
            var lineStr = sr.ReadLine();
            while (lineStr != null)
            {
                if (lineStr.Contains(GUID_PROP))
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