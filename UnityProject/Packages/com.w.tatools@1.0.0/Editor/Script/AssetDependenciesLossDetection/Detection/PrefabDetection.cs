namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;

    internal class PrefabNodeData
    {
        internal string m_NodeName;
        internal List<string> m_CompenentFileIDS;

        internal PrefabNodeData m_Father;

        internal List<string> m_LossGuids;
        internal List<string> m_LossGuidPropNames;

        internal string RelativeNodePath { get { return GetRelativeNodePath(); } }
        
        internal bool Invalid { get { return m_LossGuids != null && m_LossGuids.Count > 0; } }

        string GetRelativeNodePath(PrefabNodeData father)
        {
            string relativePath = father.GetRelativeNodePath() + "/" + m_NodeName;
            return relativePath;
        }

        string GetRelativeNodePath()
        {
            if (m_Father == null) return m_NodeName;

            return GetRelativeNodePath(m_Father);
        }
    }

    internal class PrefabDetection : BaseDetection
    {
        #region Fields

        const string GAMEOBJECT_PROP = "GameObject:";
        const string GAMEOBJECT_NAME_PROP = "  m_Name: ";
        const string GAMEOBJECT_COMP_PROP = "  m_Component:";
        const string GAMEOBJECT_COMP_ITEM_START_PROP = "  - component: {";
        const string GAMEOBJECT_COMP_ITEM_END_PROP = "}";

        const string TRANSFORM_PROP_0 = "Transform:";
        const string TRANSFORM_PROP_1 = "RectTransform:";
        const string TRANSFORM_FATHER_START_PROP = "  m_Father:";
        const string TRANSFORM_FATHER_END_PROP = "}";

        const string SPECIAL_GUID_START_PROP = "  - {fileID:";
        const string ANIMAITON_PROP = "  m_Animations:";
        const string MATERIAL_PROP = "  m_Materials:";

        const string PREFAB_LINK_PROP = "PrefabInstance:";
        const string PREFAB_LINK_FATHER_START_PROP = "    m_TransformParent: {fileID: ";
        const string PREFAB_LINK_FATHER_END_PROP = "}";
        const string PREFAB_LINK_NAME_START_PROP = "      propertyPath: m_Name";
        const string PREFAB_LINK_NAME_VALUE_PROP = "      value: ";
        const string PREFAB_LINK_ADD_OBJ_START_PROP = "      addedObject:";
        const string PREFAB_LINK_ADD_OBJ_END_PROP = "}";
        const string PREFAB_LINK_GUID_START = "  m_SourcePrefab:";
        const string PREFAB_LINK_TRANSFORM_FILEID_FLAG = "stripped";

        const string FILE_ID_PROP = "--- !u!";
        const string GUID_PROP = "guid:";
        const char SPLIT_CHAR_0 = ',';
        const char SPLIT_CHAR_1 = ':';
        const char SPLIT_CHAR_2 = '&';

        internal List<PrefabNodeData> m_LossDataList;

        #endregion

        #region Override Methods

        internal override void DoDetection(string assetPath)
        {
            base.DoDetection(assetPath);

            Dictionary<string, string> componentID2GoMapping = new Dictionary<string, string>();
            Dictionary<string, PrefabNodeData> totalNodeMapping = new Dictionary<string, PrefabNodeData>();
            PrefabNodeData prefabNodeData = null;
            Dictionary<string, List<string>> lossGuidMapping = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> lossGuidPropMapping = new Dictionary<string, List<string>>();
            Dictionary<string, string> fatherFileIDMapping = new Dictionary<string, string>();
            List<string> tempList = null;
            StreamReader sr = new StreamReader(assetPath);
            bool collectGameObjectFlag = false;
            bool collectTransformFlag = false;
            bool collectLinkPrefabName = false;
            bool collectNormalGuidFlag = false;
            bool strippedPartFlag = false;
            string fileID = "";
            string specialGuidProp = "";
            var lineStr = sr.ReadLine();
            while (lineStr != null)
            {
                // File ID
                if (lineStr.StartsWith(FILE_ID_PROP))
                {
                    if (lineStr.EndsWith(PREFAB_LINK_TRANSFORM_FILEID_FLAG))
                    {
                        strippedPartFlag = true;
                    }
                    else
                    {
                        strippedPartFlag = false;
                        collectGameObjectFlag = false;
                        collectTransformFlag = false;
                        var tempStr = lineStr.Replace(FILE_ID_PROP, "").Replace(" ", "");
                        var arr = tempStr.Split(SPLIT_CHAR_2);
                        if (arr.Length == 2)
                        {
                            fileID = arr[1];
                        }
                        else
                            fileID = "";
                    }
                }

                // Stripped
                if (strippedPartFlag)
                {
                    // GameObject //////////////////////////////////////////////
                    if (lineStr == GAMEOBJECT_PROP)
                    {
                        if (!collectNormalGuidFlag) collectNormalGuidFlag = true;
                    }
                    lineStr = sr.ReadLine();
                    continue;
                }

                // Normal Prefab //////////////////////////////////////
                // GameObject
                if (lineStr == GAMEOBJECT_PROP)
                {
                    if (!collectNormalGuidFlag) collectNormalGuidFlag = true;
                    collectGameObjectFlag = true;
                    prefabNodeData = new PrefabNodeData();
                    totalNodeMapping.Add(fileID, prefabNodeData);
                }
                // GameObject Name
                else if (collectGameObjectFlag && lineStr.StartsWith(GAMEOBJECT_NAME_PROP))
                {
                    prefabNodeData.m_NodeName = lineStr.Replace(GAMEOBJECT_NAME_PROP, "");
                }
                // GameObject Component Start
                else if (collectGameObjectFlag && lineStr == GAMEOBJECT_COMP_PROP)
                {
                    prefabNodeData.m_CompenentFileIDS = new List<string>();
                }
                // GameObject Component File ID
                else if (collectGameObjectFlag && lineStr.StartsWith(GAMEOBJECT_COMP_ITEM_START_PROP))
                {
                    var tempStr = lineStr.Replace(GAMEOBJECT_COMP_ITEM_START_PROP, "").Replace(GAMEOBJECT_COMP_ITEM_END_PROP, "").Replace(" ", "");
                    var arr = tempStr.Split(SPLIT_CHAR_1);
                    if (arr.Length == 2)
                    {
                        var componentFileID = arr[1];
                        if (componentFileID != "0")
                        {
                            prefabNodeData.m_CompenentFileIDS.Add(componentFileID);
                            componentID2GoMapping[componentFileID] = fileID;
                        }
                    }
                }
                // Transform
                else if (lineStr == TRANSFORM_PROP_0 || lineStr == TRANSFORM_PROP_1)
                {
                    collectTransformFlag = true;
                }
                // Transform Father
                else if (collectTransformFlag && lineStr.StartsWith(TRANSFORM_FATHER_START_PROP))
                {
                    var tempStr = lineStr.Replace(TRANSFORM_FATHER_START_PROP, "").Replace(TRANSFORM_FATHER_END_PROP, "").Replace(" ", "");
                    var arr = tempStr.Split(SPLIT_CHAR_1);
                    if (arr.Length == 2)
                    {
                        var fatherFileID = arr[1] != "0" ? arr[1] : "";
                        fatherFileIDMapping[fileID] = fatherFileID;
                    }
                }
                // special guid prop name
                else if (collectNormalGuidFlag && lineStr == ANIMAITON_PROP)
                {
                    specialGuidProp = "AnimationClip";
                }
                // special guid prop name
                else if (collectNormalGuidFlag && lineStr == MATERIAL_PROP)
                {
                    specialGuidProp = "Renderer-Material";
                }
                // guid
                else if (collectNormalGuidFlag && lineStr.Contains(GUID_PROP))
                {
                    var arr = lineStr.Replace(" ", "").Split(SPLIT_CHAR_0);
                    if (arr.Length == 3)
                    {
                        var guid = arr[1].Replace(GUID_PROP, "");
                        if (!string.IsNullOrEmpty(guid))
                        {
                            var refAssetPath = AssetDatabase.GUIDToAssetPath(guid);
                            if (!File.Exists(refAssetPath) && !DependenciesLossDetectionSettings.Instance.IsUnityBuiltinAsset(refAssetPath))
                            {
                                if (!lossGuidMapping.TryGetValue(fileID, out tempList))
                                {
                                    tempList = new List<string>();
                                    lossGuidMapping[fileID] = tempList;
                                }

                                tempList.Add(guid);

                                tempList = null;
                                string propName = "";
                                if (lineStr.StartsWith(SPECIAL_GUID_START_PROP))
                                    propName = specialGuidProp;
                                else
                                    propName = arr[0].Split(SPLIT_CHAR_1)[0];
                                if (!lossGuidPropMapping.TryGetValue(fileID, out tempList))
                                {
                                    tempList = new List<string>();
                                    lossGuidPropMapping[fileID] = tempList;
                                }

                                tempList.Add(propName);
                            }
                        }
                    }
                }

                // Link Prefab //////////////////////////////////////
                if (lineStr == PREFAB_LINK_PROP)
                {
                    collectNormalGuidFlag = false;
                    collectGameObjectFlag = true;
                    prefabNodeData = new PrefabNodeData();
                    totalNodeMapping.Add(fileID, prefabNodeData);
                }
                // Link Prefab Add Obj
                else if (lineStr.StartsWith(PREFAB_LINK_ADD_OBJ_START_PROP))
                {
                    var tempStr = lineStr.Replace(PREFAB_LINK_ADD_OBJ_START_PROP, "").Replace(PREFAB_LINK_ADD_OBJ_END_PROP, "").Replace(" ", "");
                    var arr = tempStr.Split(SPLIT_CHAR_1);
                    if (arr.Length == 2)
                    {
                        var componentFileID = arr[1];
                        if (!string.IsNullOrEmpty(componentFileID) && componentFileID != "0")
                        {
                            if (prefabNodeData.m_CompenentFileIDS == null) prefabNodeData.m_CompenentFileIDS = new List<string>();
                            prefabNodeData.m_CompenentFileIDS.Add(componentFileID);
                            componentID2GoMapping[componentFileID] = fileID;
                        }
                    }
                }
                else if (lineStr.StartsWith(PREFAB_LINK_GUID_START))
                {
                    lineStr = lineStr.Replace(PREFAB_LINK_GUID_START, "").Replace(" ", "").Replace(" ", "");
                    var arr = lineStr.Split(SPLIT_CHAR_0);
                    if (arr.Length == 3)
                    {
                        lineStr = arr[1];
                        var sourcePrefabGuid = "";
                        if (lineStr.StartsWith(GUID_PROP))
                        {
                            sourcePrefabGuid = lineStr.Replace(GUID_PROP, "");
                        }

                        var refAssetPath = AssetDatabase.GUIDToAssetPath(sourcePrefabGuid);
                        if (!File.Exists(refAssetPath) && !DependenciesLossDetectionSettings.Instance.IsUnityBuiltinAsset(refAssetPath))
                        {
                            if (!lossGuidMapping.TryGetValue(fileID, out tempList))
                            {
                                tempList = new List<string>();
                                lossGuidMapping[fileID] = tempList;
                            }

                            tempList.Add(sourcePrefabGuid);

                            tempList = null;
                            if (!lossGuidPropMapping.TryGetValue(fileID, out tempList))
                            {
                                tempList = new List<string>();
                                lossGuidPropMapping[fileID] = tempList;
                            }

                            tempList.Add("LinkPrefab");
                        }

                        componentID2GoMapping[fileID] = fileID;
                    }
                }
                // Link Prefab Father File ID
                else if (lineStr.StartsWith(PREFAB_LINK_FATHER_START_PROP))
                {
                    var linkPrefabFatherFileID = lineStr.Replace(PREFAB_LINK_FATHER_START_PROP, "").Replace(PREFAB_LINK_FATHER_END_PROP, "");
                    if (linkPrefabFatherFileID == "0")
                        linkPrefabFatherFileID = "";

                    fatherFileIDMapping[fileID] = linkPrefabFatherFileID;
                }
                // Link Prefab Name
                else if (lineStr.StartsWith(PREFAB_LINK_NAME_START_PROP))
                {
                    collectLinkPrefabName = true;
                }
                else if (collectLinkPrefabName && lineStr.StartsWith(PREFAB_LINK_NAME_VALUE_PROP))
                {
                    collectLinkPrefabName = false;
                    prefabNodeData.m_NodeName = lineStr.Replace(PREFAB_LINK_NAME_VALUE_PROP, "");
                }

                lineStr = sr.ReadLine();
            }

            sr.Close();
            sr.Dispose();
            sr = null;

            // Father
            foreach (var data in fatherFileIDMapping)
            {
                var childFileID = data.Key;
                string goChildFileID = null;
                if (componentID2GoMapping.TryGetValue(childFileID, out goChildFileID))
                {
                    if (!string.IsNullOrEmpty(data.Value))
                    {
                        PrefabNodeData childData = null;
                        if (totalNodeMapping.TryGetValue(goChildFileID, out childData))
                        {
                            string goFatherFileID = null;
                            if (componentID2GoMapping.TryGetValue(data.Value, out goFatherFileID))
                            {
                                childData.m_Father = totalNodeMapping[goFatherFileID];
                            }
                            else
                                childData.m_Father = null;
                        }
                    }
                }
            }
            // Guid
            foreach (var data in lossGuidMapping)
            {
                var tempFileID = data.Key;
                var list = data.Value;
                var propList = lossGuidPropMapping[tempFileID];
                string nodeFileID = null;
                if(componentID2GoMapping.TryGetValue(tempFileID,out nodeFileID))
                {
                    PrefabNodeData pnData = null;
                    if (totalNodeMapping.TryGetValue(nodeFileID, out pnData))
                    {
                        if (pnData.m_LossGuids == null) pnData.m_LossGuids = new List<string>();
                        foreach (var guid in list)
                        {
                            pnData.m_LossGuids.Add(guid);
                        }

                        if (pnData.m_LossGuidPropNames == null) pnData.m_LossGuidPropNames = new List<string>();
                        foreach (var guidProp in propList)
                        {
                            pnData.m_LossGuidPropNames.Add(guidProp);
                        }
                    }
                }
            }

            foreach (var data in totalNodeMapping.Values)
            {
                if (data.Invalid)
                {
                    if (this.m_LossDataList == null) this.m_LossDataList = new List<PrefabNodeData>();

                    this.m_LossDataList.Add(data);
                }
            }
        }

        internal override bool IsInvalid()
        {
            return this.m_LossDataList != null && this.m_LossDataList.Count > 0;
        }

        #endregion 
    }
}