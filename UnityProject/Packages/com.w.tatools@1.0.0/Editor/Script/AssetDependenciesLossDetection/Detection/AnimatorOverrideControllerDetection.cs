namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;

    internal class AnimatorOverrideControllerDetection : BaseDetection
    {
        #region Fields

        const string CONTROLLER_PROP = "  m_Controller:";
        const string OVERRIDE_CLIP_PROP = "    m_OverrideClip: ";
        const string GUID_PROP = "guid:";
        const char SPLIT_CHAR = ',';

        internal string m_LossControllerGuid;

        #endregion

        #region Methods

        internal override void DoDetection(string assetPath)
        {
            base.DoDetection(assetPath);

            StreamReader sr = new StreamReader(base.m_AssetPath);
            var lineStr = sr.ReadLine();
            while (lineStr != null)
            {
                // Controller
                if (lineStr.StartsWith(CONTROLLER_PROP))
                {
                    lineStr = lineStr.Replace(CONTROLLER_PROP, "").Replace(" ","");
                    var arr = lineStr.Split(SPLIT_CHAR);
                    if (arr.Length == 3)
                    {
                        lineStr = arr[1];
                        if (lineStr.StartsWith(GUID_PROP))
                        {
                            var guid = lineStr.Replace(GUID_PROP, "");
                            if (!string.IsNullOrEmpty(guid))
                            {
                                var filePath = AssetDatabase.GUIDToAssetPath(guid);
                                if (!File.Exists(filePath) && !DependenciesLossDetectionSettings.Instance.IsUnityBuiltinAsset(filePath))
                                {
                                    m_LossControllerGuid = guid;
                                }
                            }
                        }
                    }
                }
                else if (lineStr.StartsWith(OVERRIDE_CLIP_PROP))
                {
                    lineStr = lineStr.Replace(OVERRIDE_CLIP_PROP, "").Replace(" ", "");
                    var arr = lineStr.Split(SPLIT_CHAR);
                    if (arr.Length == 3)
                    {
                        lineStr = arr[1];
                        if (lineStr.StartsWith(GUID_PROP))
                        {
                            var guid = lineStr.Replace(GUID_PROP, "");
                            if (!string.IsNullOrEmpty(guid))
                            {
                                var filePath = AssetDatabase.GUIDToAssetPath(guid);
                                if (!File.Exists(filePath) && !DependenciesLossDetectionSettings.Instance.IsUnityBuiltinAsset(filePath))
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
            return !string.IsNullOrEmpty(this.m_LossControllerGuid) || (base.m_LossGuids != null && base.m_LossGuids.Count > 0);
        }

        #endregion
    }
}