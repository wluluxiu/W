namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;

    internal class AnimatiorControllerData
    {
        internal string m_StateName;
        internal string m_MoitonLossGuid;
        internal List<string> m_BehaivourFileIDS;
        internal List<string> m_BehaivourLossGuids;
    }

    internal class AnimatorControllerDetection : BaseDetection
    {
        #region Fields

        const string ANIMATOR_STATE_PROP = "AnimatorState:";
        const string MOTION_PROP = "  m_Motion:";
        const string SCRIPT_PROP = "  m_Script:";
        const string NAME_PROP = "  m_Name: ";
        const string GUID_PROP = "guid:";
        const string FILE_ID_START = "--- !u!";
        const string BEHAVIOURS_PROP = "  m_StateMachineBehaviours:";
        const string BEHAVIOURS_FILEID_PROP_START = "  - {fileID: ";
        const string BEHAVIOURS_FILEID_PROP_END = "}";
        const char SPILT_CHAR_0 = '&';
        const char SPLIT_CHAR_1 = ',';

        internal List<AnimatiorControllerData> m_LossDataList;

        #endregion

        #region Methods

        internal override void DoDetection(string assetPath)
        {
            base.DoDetection(assetPath);

            // Parse All //////////////////////////
            List<AnimatiorControllerData> totalDataList = new List<AnimatiorControllerData>();
            AnimatiorControllerData tempData = null;
            string fileID = null;
            bool stateCollectFlag = false;
            bool behaivourCollectFlag = false;
            Dictionary<string, string> fileID2GuidMapping = new Dictionary<string, string>();
            StreamReader sr = new StreamReader(base.m_AssetPath);
            var lineStr = sr.ReadLine();
            while (lineStr != null)
            {
                // Behaviour
                if (lineStr == BEHAVIOURS_PROP)
                {
                    behaivourCollectFlag = true;
                    tempData.m_BehaivourFileIDS = new List<string>();
                }
                else if (lineStr.StartsWith(BEHAVIOURS_FILEID_PROP_START) && behaivourCollectFlag)
                {
                    var tempBehaviourFileID = lineStr.Replace(BEHAVIOURS_FILEID_PROP_START, "").Replace(BEHAVIOURS_FILEID_PROP_END, "");
                    if (!string.IsNullOrEmpty(tempBehaviourFileID))
                    {
                        tempData.m_BehaivourFileIDS.Add(tempBehaviourFileID);
                    }
                }
                else
                {
                    behaivourCollectFlag = false;

                    // AniamtorState
                    if (lineStr == ANIMATOR_STATE_PROP)
                    {
                        tempData = new AnimatiorControllerData();
                        totalDataList.Add(tempData);
                        stateCollectFlag = true;
                    }
                    else
                    {
                        // File ID
                        if (lineStr.StartsWith(FILE_ID_START))
                        {
                            fileID = lineStr.Replace(FILE_ID_START, "").Replace(" ", "");
                            var arr = fileID.Split(SPILT_CHAR_0);
                            if (arr.Length == 2)
                                fileID = arr[1];
                            else
                                fileID = null;

                            stateCollectFlag = false;
                        }
                        // Script Guid
                        else if (lineStr.StartsWith(SCRIPT_PROP))
                        {
                            var tempScriptStr = lineStr.Replace(SCRIPT_PROP, "").Replace(" ", "");
                            var arr = tempScriptStr.Split(SPLIT_CHAR_1);
                            if (arr.Length == 3)
                            {
                                tempScriptStr = arr[1];
                                if (tempScriptStr.StartsWith(GUID_PROP))
                                {
                                    var guid = tempScriptStr.Replace(GUID_PROP, "");
                                    if (!string.IsNullOrEmpty(guid))
                                    {
                                        fileID2GuidMapping[fileID] = guid;
                                    }
                                }
                            }
                        }
                        // Motion Name
                        else if (lineStr.StartsWith(NAME_PROP) && stateCollectFlag)
                        {
                            tempData.m_StateName = lineStr.Replace(NAME_PROP, "");
                        }
                        // Motion Guid
                        else if (lineStr.StartsWith(MOTION_PROP))
                        {
                            var tempScriptStr = lineStr.Replace(MOTION_PROP, "").Replace(" ", "");
                            var arr = tempScriptStr.Split(SPLIT_CHAR_1);
                            if (arr.Length == 3)
                            {
                                tempScriptStr = arr[1];
                                if (tempScriptStr.StartsWith(GUID_PROP))
                                {
                                    var guid = tempScriptStr.Replace(GUID_PROP, "");
                                    if (!string.IsNullOrEmpty(guid))
                                    {
                                        tempData.m_MoitonLossGuid = guid;
                                    }
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

            // Collect Loss Data /////////////////////
            this.m_LossDataList = new List<AnimatiorControllerData>();
            foreach (var data in totalDataList)
            {
                bool isLossDataFlag = false;

                // Collect Loss Behavior Guid
                if (data.m_BehaivourFileIDS != null)
                {
                    data.m_BehaivourLossGuids = new List<string>();
                    foreach (var behaviourFileID in data.m_BehaivourFileIDS)
                    {
                        string behaviourGuid = null;
                        if (fileID2GuidMapping.TryGetValue(behaviourFileID, out behaviourGuid))
                        {
                            var behaviourAssetPath = AssetDatabase.GUIDToAssetPath(behaviourGuid);
                            if (!File.Exists(behaviourAssetPath) && !DependenciesLossDetectionSettings.Instance.IsUnityBuiltinAsset(behaviourAssetPath))
                            {
                                data.m_BehaivourLossGuids.Add(behaviourGuid);
                                isLossDataFlag = true;
                            }
                        }
                    }
                }
                //  Collect Loss Motion Data
                if (!string.IsNullOrEmpty(data.m_MoitonLossGuid))
                {
                    var motionRefAssetPath = AssetDatabase.GUIDToAssetPath(data.m_MoitonLossGuid);
                    if ((!File.Exists(motionRefAssetPath) && !DependenciesLossDetectionSettings.Instance.IsUnityBuiltinAsset(motionRefAssetPath)))
                    {
                        isLossDataFlag = true;
                    }
                    else
                        data.m_MoitonLossGuid = null;
                }

                if(isLossDataFlag)
                    this.m_LossDataList.Add(data);
            }
        }

        internal override bool IsInvalid()
        {
            return m_LossDataList != null && m_LossDataList.Count > 0;
        }

        #endregion
    }
}