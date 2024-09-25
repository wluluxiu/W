using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    /// <summary>
    /// 检测项目：
    /// 1.Missing Material
    /// 2.Missing Script
    /// 3.Colliders
    /// 4.内置Dep
    /// </summary>
    internal class PrefabRecorder : BaseRecorder
    {
        #region Fields

        internal List<string> m_MissingCompList = new List<string>();
        internal Dictionary<string, int> m_CollidersMapping = new Dictionary<string, int>();
        internal List<string> m_MissingMaterialsList = new List<string>();

        #endregion

        #region Override Methods

        internal override void Record(string assetPath, EAssetType assetType)
        {
            base.Record(assetPath, assetType);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            Transform[] allNodes = prefab.transform.GetComponentsInChildren<Transform>(true);
            foreach (var node in allNodes)
            {
                string nodePath = null;
                // Missing Component
                bool existMissingComp = false;
                var comps = node.GetComponents<Component>();
                foreach (var comp in comps)
                {
                    if (comp == null)
                    {
                        existMissingComp = true;
                        break;
                    }
                }

                if (existMissingComp)
                {
                    nodePath = AssetDetectionUtility.GetTransformNodePath(node);
                    if (!m_MissingCompList.Contains(nodePath))
                        m_MissingCompList.Add(nodePath);
                }

                // Collider
                var colliders = node.GetComponents<Collider>();
                if (colliders != null && colliders.Length > 0)
                {
                    if (nodePath == null) nodePath = AssetDetectionUtility.GetTransformNodePath(node);
                    m_CollidersMapping[nodePath] = colliders.Length;
                }

                // Missing Mat
                var r = node.GetComponent<Renderer>();
                if (r != null)
                {
                    bool missingMatFlag = false;
                    if(!r.enabled) continue;

                    if (r.sharedMaterials == null || r.sharedMaterials.Length == 0)
                    {
                        missingMatFlag = true;
                    }
                    else
                    {
                        foreach (var mat in r.sharedMaterials)
                        {
                            if (mat == null)
                            {
                                missingMatFlag = true;
                                break;
                            }
                        }
                    }

                    if (missingMatFlag)
                    {
                        if (nodePath == null) nodePath = AssetDetectionUtility.GetTransformNodePath(node);
                        if (!m_MissingMaterialsList.Contains(nodePath))
                            m_MissingMaterialsList.Add(nodePath);
                    }
                }
            }

            base.m_BuiltinDependencies = AssetDetectionUtility.GetBuiltinDenpendencies(prefab);

            prefab = null;
        }

        /// <summary>
        /// BaseFormat|
        /// MissingCompNodePath1#MissingCompNodePath2#MissingCompNodePath3#...|
        /// ColliderNodePath1$Amount1#ColliderNodePath2$Amount2|ColliderNodePath3$Amount3...|
        /// MissingMatNode1#MissingMatNode2#MissingMatNode3#...|
        /// BuiltinDep1#BuiltinDep2#BuiltinDep3#...
        /// </summary>
        /// <returns></returns>
        internal override string GetOutputStr()
        {
            var baseOutputStr = base.GetOutputStr();

            string spiltStr = CHAR_SPLIT_FIRST_FLAG.ToString();

            // Missing Component
            string missingCompOutputStr = "";
            foreach (var nodePath in m_MissingCompList)
                missingCompOutputStr += nodePath + CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(missingCompOutputStr))
                missingCompOutputStr = missingCompOutputStr.Substring(0, missingCompOutputStr.Length - 1);

            // Collider
            string collidersOutputStr = "";
            foreach (var data in m_CollidersMapping)
                collidersOutputStr += data.Key + CHAR_SPLIT_THIRD_FLAG + data.Value.ToString() + CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(collidersOutputStr))
                collidersOutputStr = collidersOutputStr.Substring(0, collidersOutputStr.Length - 1);

            // Missing Material
            string missingMaterialStr = "";
            foreach (var nodePath in m_MissingMaterialsList)
                missingMaterialStr += nodePath + CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(missingMaterialStr))
                missingMaterialStr = missingMaterialStr.Substring(0, missingMaterialStr.Length - 1);

            // Builtin Dep
            string builtinDepStr = "";
            foreach (var nodePath in base.m_BuiltinDependencies)
                builtinDepStr += nodePath + CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(builtinDepStr))
                builtinDepStr = builtinDepStr.Substring(0, builtinDepStr.Length - 1);

            return baseOutputStr + spiltStr + missingCompOutputStr + spiltStr + collidersOutputStr + spiltStr + missingMaterialStr + spiltStr + builtinDepStr;
        }

        internal override void ParseStrLine(string stringLine)
        {
            base.ParseStrLine(stringLine);

            if (base.m_SourceDataArr.Length > 7 && !string.IsNullOrEmpty(base.m_SourceDataArr[7]))
            {
                string[] arr = base.m_SourceDataArr[7].Split(CHAR_SPLIT_SECOND_FLAG);
                this.m_MissingCompList = new List<string>(arr);
            }

            if (base.m_SourceDataArr.Length > 8 && !string.IsNullOrEmpty(base.m_SourceDataArr[8]))
            {
                string[] arr = base.m_SourceDataArr[8].Split(CHAR_SPLIT_SECOND_FLAG);
                if (arr != null)
                {
                    foreach (var childData in arr)
                    {
                        var childArr = childData.Split(CHAR_SPLIT_THIRD_FLAG);
                        if (childArr.Length != 2) continue;

                        m_CollidersMapping[childArr[0]] = !string.IsNullOrEmpty(childArr[1]) ? int.Parse(childArr[1]) : -1;
                    }
                }
            }

            if (base.m_SourceDataArr.Length > 9 && !string.IsNullOrEmpty(base.m_SourceDataArr[9]))
            {
                string[] arr = base.m_SourceDataArr[9].Split(CHAR_SPLIT_SECOND_FLAG);
                this.m_MissingMaterialsList = new List<string>(arr);
            }

            if (base.m_SourceDataArr.Length > 10 && !string.IsNullOrEmpty(base.m_SourceDataArr[10]))
            {
                string[] arr = base.m_SourceDataArr[10].Split(CHAR_SPLIT_SECOND_FLAG);
                base.m_BuiltinDependencies = new List<string>(arr);
            }
        }

        internal override void Release()
        {
            base.Release();

            this.m_MissingCompList.Clear();
            this.m_MissingCompList = null;
            this.m_CollidersMapping.Clear();
            this.m_CollidersMapping = null;
            this.m_MissingMaterialsList.Clear();
            this.m_MissingMaterialsList = null;
        }

        #endregion
    }
}
