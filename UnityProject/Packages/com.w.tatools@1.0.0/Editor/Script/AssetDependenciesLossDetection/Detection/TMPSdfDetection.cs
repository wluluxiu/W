namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;

    internal class TMPSdfDetection : BaseDetection
    {
        #region Fields

        const string GUID_PROP = "guid:";
        const char SPLIT_CHAR_0 = ',';
        const char SPLIT_CHAR_1 = ':';

        const string REFERENCED_FONT_ASSET_GUID_PROP = "    referencedFontAssetGUID: ";
        const string REFERENCED_TEXT_ASSET_GUID_PROP = "    referencedTextAssetGUID: ";

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

                                    var propName = arr[0].Split(SPLIT_CHAR_1)[0];
                                    if (base.m_LossGuidPropNames == null) base.m_LossGuidPropNames = new List<string>();
                                    base.m_LossGuidPropNames.Add(propName);
                                }
                            }
                        }
                    }
                }
                else if (lineStr.StartsWith(REFERENCED_FONT_ASSET_GUID_PROP) && lineStr != REFERENCED_FONT_ASSET_GUID_PROP)
                {
                    var guid = lineStr.Replace(REFERENCED_FONT_ASSET_GUID_PROP, "").Replace(" ", "");
                    var refAssetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (!File.Exists(refAssetPath) && !DependenciesLossDetectionSettings.Instance.IsUnityBuiltinAsset(refAssetPath))
                    {
                        if (base.m_LossGuids == null) base.m_LossGuids = new List<string>();
                        base.m_LossGuids.Add(guid);

                        var propName = REFERENCED_FONT_ASSET_GUID_PROP.Replace(" ", "").Replace(SPLIT_CHAR_1.ToString(), "");
                        if (base.m_LossGuidPropNames == null) base.m_LossGuidPropNames = new List<string>();
                        base.m_LossGuidPropNames.Add(propName);
                    }
                }
                else if (lineStr.StartsWith(REFERENCED_TEXT_ASSET_GUID_PROP) && lineStr != REFERENCED_TEXT_ASSET_GUID_PROP)
                {
                    var guid = lineStr.Replace(REFERENCED_TEXT_ASSET_GUID_PROP, "").Replace(" ", "");
                    var refAssetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (!File.Exists(refAssetPath) && !DependenciesLossDetectionSettings.Instance.IsUnityBuiltinAsset(refAssetPath))
                    {
                        if (base.m_LossGuids == null) base.m_LossGuids = new List<string>();
                        base.m_LossGuids.Add(guid);

                        var propName = REFERENCED_TEXT_ASSET_GUID_PROP.Replace(" ", "").Replace(SPLIT_CHAR_1.ToString(), ""); ;
                        if (base.m_LossGuidPropNames == null) base.m_LossGuidPropNames = new List<string>();
                        base.m_LossGuidPropNames.Add(propName);
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