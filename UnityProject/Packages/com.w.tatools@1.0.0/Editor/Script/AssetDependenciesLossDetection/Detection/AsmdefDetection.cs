namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;

    internal class AsmdefDetection : BaseDetection
    {
        #region Fields

        const string GUID_START_WITH = "        \"GUID:";
        const char SPLIT_CHAR = '"';

        #endregion

        #region Override Methods

        internal override void DoDetection(string assetPath)
        {
            base.DoDetection(assetPath);

            StreamReader sr = new StreamReader(assetPath);
            var lineStr = sr.ReadLine();
            while (lineStr != null)
            {
                if (lineStr.StartsWith(GUID_START_WITH))
                {
                    lineStr = lineStr.Replace(GUID_START_WITH, "").Replace(" ", "");
                    var arr = lineStr.Split(SPLIT_CHAR);
                    var guid = arr[0];
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