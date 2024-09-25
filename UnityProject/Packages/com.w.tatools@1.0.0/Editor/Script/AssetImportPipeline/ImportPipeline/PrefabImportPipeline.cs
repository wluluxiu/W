namespace jj.TATools.Editor
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.Linq;

    internal enum PrefabModifiedType
    {
        [PipelineEnumAttribute("组件属性值修改")]
        CompPropertyValue = 0,
        [PipelineEnumAttribute("Spine.Unity-Use Clip修改")]
        SpineUnityUseClipping = 1,
    }

    internal enum SystemType
    {
        INT = 0,
        LONG,
        FLOAT,
        DOUBLE,
        BOOL
    }

    [System.Serializable]
    internal class PrefabSetting
    {
        #region Fields

        public ESpecialRuleType m_RuleType = ESpecialRuleType._0_Default;
        public List<string> m_AssetParentFolderListForPartFolderRule = new List<string>();
        public List<string> m_AssetParentFolderListForFullFolderRule = new List<string>();
        public List<string> m_AssetNameListForAssetNameRule = new List<string>();
        public List<string> m_AssetPathListForAssetPathRule = new List<string>();

        public PrefabModifiedType m_ModifiedType = PrefabModifiedType.CompPropertyValue;

        // CompPropertyValue
        public string m_AssemblyName;
        public string m_ComponentTypeName;
        public string m_ComponentPropertyName;
        public SystemType m_ComponentPropertyValueType;
        public string m_ComponentPropertyValue;

        #endregion

        #region Internal Methods

        internal bool CheckRule(string assetPath)
        {
            var assetPathLower = assetPath.ToLower();
            bool result = false;
            switch (m_RuleType)
            {
                case ESpecialRuleType._0_Default:
                    result = true;
                    break;
                case ESpecialRuleType._1_AssetParentPartFolder:
                    foreach (var partFolder in m_AssetParentFolderListForPartFolderRule)
                    {
                        if (assetPathLower.Contains(partFolder.ToLower()))
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                case ESpecialRuleType._2_AssetParentFullFolder:
                    foreach (var fullFolder in m_AssetParentFolderListForFullFolderRule)
                    {
                        if (assetPathLower.StartsWith(fullFolder.ToLower()))
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                case ESpecialRuleType._3_AssetName:
                    var assetName = Path.GetFileNameWithoutExtension(assetPathLower);
                    foreach (var nameSuffix in m_AssetNameListForAssetNameRule)
                    {
                        if (assetName.Contains(nameSuffix.ToLower()))
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                case ESpecialRuleType._4_AssetPath:
                    result = m_AssetPathListForAssetPathRule.Contains(assetPath);
                    break;
            }

            return result;
        }

        internal void DoPreImport(GameObject prefab)
        {
            if (m_ModifiedType == PrefabModifiedType.CompPropertyValue)
                DoCompPropertyValuePreImport(prefab);
            else if (m_ModifiedType == PrefabModifiedType.SpineUnityUseClipping)
                DoSpineUnityUseClippingPreImport(prefab);
            // else if ...
        }

        #endregion

        #region Local Methods

        private System.Object ConvertPropertyValue(string valueStr)
        {
            System.Object value = null;

            switch (m_ComponentPropertyValueType)
            {
                case SystemType.INT:
                    value = Convert.ToInt32(valueStr);
                    break;
                case SystemType.LONG:
                    value = Convert.ToInt64(valueStr);
                    break;
                case SystemType.FLOAT:
                    value = Convert.ToSingle(valueStr);
                    break;
                case SystemType.DOUBLE:
                    value = Convert.ToDouble(valueStr);
                    break;
                case SystemType.BOOL:
                    value = Convert.ToBoolean(valueStr);
                    break;
            }

            return value;
        }

        private void DoCompPropertyValuePreImport(GameObject prefab)
        {
            if (string.IsNullOrEmpty(m_AssemblyName) || string.IsNullOrEmpty(m_ComponentTypeName) || string.IsNullOrEmpty(m_ComponentPropertyName) || string.IsNullOrEmpty(m_ComponentPropertyValue))
                return;

            var assembly = Assembly.Load(m_AssemblyName);
            if (assembly != null)
            {
                var componentType = assembly.GetType(m_ComponentTypeName);
                if (componentType != null)
                {
                    var components = prefab.GetComponentsInChildren(componentType, true);
                    if (components != null && components.Length > 0)
                    {
                        var propertyValue = ConvertPropertyValue(m_ComponentPropertyValue);
                        foreach (var comp in components)
                        {
                            var prop = componentType.GetProperty(m_ComponentPropertyName);
                            if (prop != null)
                            {
                                prop.SetValue(comp, propertyValue);
                            }
                            else
                            {
                                var field = componentType.GetField(m_ComponentPropertyName);
                                if (field != null)
                                {
                                    field.SetValue(comp, propertyValue);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DoSpineUnityUseClippingPreImport(GameObject prefab)
        {
            var assemblyName = "spine-unity";
            var componentTypeName = "Spine.Unity.SkeletonGraphic";
            var meshGeneratorFieldName = "meshGenerator";
            var meshGeneratorSettingFieldName = "settings";
            var settingUseClippingFieldName = "useClipping";
            Assembly assembly = Assembly.Load(assemblyName);
            if (assembly != null)
            {
                Type componentType = assembly.GetType(componentTypeName);
                if (componentType != null)
                {
                    var components = prefab.GetComponentsInChildren(componentType, true);
                    if (components != null && components.Length > 0)
                    {
                        foreach (var comp in components)
                        {
                            var meshField = componentType.GetField(meshGeneratorFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                            if (meshField != null)
                            {
                                var meshGenerator = meshField.GetValue(comp);
                                var type = meshField.FieldType;
                                var settingsField = type.GetField(meshGeneratorSettingFieldName);
                                if (settingsField != null)
                                {
                                    var settings = settingsField.GetValue(meshGenerator);
                                    type = settingsField.FieldType;
                                    var field = type.GetField(settingUseClippingFieldName);
                                    field.SetValue(settings, false);
                                    settingsField.SetValue(meshGenerator, settings);
                                }
                            }
                        }
                    }
                    EditorUtility.SetDirty(prefab);
                }
            }
        }

        #endregion
    }

    internal class PrefabImportPipeline : ScriptableObject
    {
        #region Fields

        const string PREFAB_EXTENSION = ".prefab";

        [SerializeField]
        List<PrefabSetting> m_SpecialSettings;

        List<PrefabSetting> m_SortedSpecialSettings;

        #endregion

        #region Internal Methods

        internal static bool IsPrefabAsset(string assetPath)
        {
            return assetPath.EndsWith(PREFAB_EXTENSION);
        }

        internal void AutoSortSettings()
        {
            m_SortedSpecialSettings = new List<PrefabSetting>(m_SpecialSettings);
            m_SortedSpecialSettings.Sort((x, y) => string.CompareOrdinal(((int)x.m_RuleType).ToString(), ((int)y.m_RuleType).ToString()));
        }

        internal void DoPreImport(string prefabAssetPath)
        {
            if (m_SortedSpecialSettings != null && m_SortedSpecialSettings.Count > 0)
            {
                GameObject prefab = null;
                foreach (var setting in m_SortedSpecialSettings)
                {
                    if (setting.CheckRule(prefabAssetPath))
                    {
                        if (prefab == null)
                            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);

                        setting.DoPreImport(prefab);
                    }
                }
            }
        }

        internal static void AutoSet()
        {
            var tempMapping = new Dictionary<string, PrefabImportPipeline>();
            var subPipelineRelativePath = AssetImportPipelineUtility.SubPipelineRelativePath;
            var guids = AssetDatabase.FindAssets("t:" + typeof(PrefabImportPipeline), new string[] { "Assets" });
            if (guids != null)
            {
                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    PrefabImportPipeline pipeline = AssetDatabase.LoadAssetAtPath<PrefabImportPipeline>(assetPath);
                    pipeline.AutoSortSettings();

                    var fileName = Path.GetFileName(assetPath);
                    tempMapping[assetPath.Replace(subPipelineRelativePath + fileName, "")] = pipeline;
                }
            }

            foreach (var data in tempMapping)
            {
                var folder = data.Key;
                var pipeline = data.Value;
                guids = AssetDatabase.FindAssets("t:Prefab", new string[] { folder });
                foreach (var guid in guids)
                {
                    pipeline.DoPreImport(AssetDatabase.GUIDToAssetPath(guid));
                }
            }
        }

        #endregion
    }

    [CustomEditor(typeof(PrefabImportPipeline))]
    internal class PrefabImportPipelineInspector : Editor
    {
        #region Fields

        class Styles
        {
            internal static readonly GUIContent SpecialSettings = new GUIContent("自定义配置:");

            internal static readonly GUIContent RuleType = new GUIContent("规则类型", "1.None:通用规则;\n2.AssetPath:资源路径;\n3.AssetName:资源名;\n4.AssetParentFullFolder:全目录;\n5.AssetParentPartFolder:部分目录;");
            internal static readonly GUIContent AssetPath = new GUIContent("资源路径", "格式：Assets/XXX/YYY/test.png");
            internal static readonly GUIContent AssetName = new GUIContent("资源名", "格式: _ASTC_4x4 或 _RW");
            internal static readonly GUIContent AssetParentFullFolder = new GUIContent("全目录", "格式：Assets/XXX/YYY/");
            internal static readonly GUIContent AssetParentPartFolder = new GUIContent("部分目录", "格式：/XXX/YYY/ ");

            // CompPropertyValue
            internal static readonly GUIContent AssemblyName = new GUIContent("程序集", "目标组件所在程序集全名。\n例如：BoxCollider，程序集[UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]");
            internal static readonly GUIContent ComponentTypeName = new GUIContent("组件类型名(包含命名空间)", "例如：BoxCollider，组件类型名[UnityEngine.BoxCollider]");
            internal static readonly GUIContent ComponentPropertyName = new GUIContent("属性名", "");
            internal static readonly GUIContent ComponentPropertyValueType = new GUIContent("属性值类型", "目前只支持int、long、float、double、bool");
            internal static readonly GUIContent ComponentPropertyValue = new GUIContent("属性值", "");
        }

        // Base Property
        private SerializedProperty m_SpecialSettings;

        // Children Property /////////////////////////////
        private SerializedProperty m_RuleType;

        private SerializedProperty m_AssetParentFolderListForPartFolderRule;
        private SerializedProperty m_AssetParentFolderListForFullFolderRule;
        private SerializedProperty m_AssetNameListForAssetNameRule;
        private SerializedProperty m_AssetPathListForAssetPathRule;

        private SerializedProperty m_ModifiedType;

        // CompPropertyValue
        private SerializedProperty m_AssemblyName;
        private SerializedProperty m_ComponentTypeName;
        private SerializedProperty m_ComponentPropertyName;
        private SerializedProperty m_ComponentPropertyValueType;
        private SerializedProperty m_ComponentPropertyValue;

        private List<bool> m_SpecialChildSettingFoldouts = new List<bool>();

        private static Dictionary<int, string> m_RuleTypeValueToDisplayMapping = null;
        private static Dictionary<int, string> m_ModifiedTypeValueToDisplayMapping = null;

        #endregion

        #region Editor Methods

        void OnEnable()
        {
            m_SpecialSettings = serializedObject.FindProperty("m_SpecialSettings");

            BuildEnumTypeValueToDisplayMapping();
        }

        public override void OnInspectorGUI()
        {
            if (m_SpecialChildSettingFoldouts.Count == 0)
            {
                for (int i = 0; i < m_SpecialSettings.arraySize; i++)
                {
                    m_SpecialChildSettingFoldouts.Add(false);
                }
            }
            serializedObject.Update();

            EditorGUILayout.LabelField(Styles.SpecialSettings);

            int deleteIndex = -1;
            for (int i = 0; i < m_SpecialSettings.arraySize; i++)
            {
                var setting = m_SpecialSettings.GetArrayElementAtIndex(i);

                var modifiedType = setting.FindPropertyRelative("m_ModifiedType").enumValueFlag;
                var modifiedTypeDisplayName = m_ModifiedTypeValueToDisplayMapping[modifiedType];

                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();

                m_SpecialChildSettingFoldouts[i] = EditorGUILayout.Foldout(m_SpecialChildSettingFoldouts[i], modifiedTypeDisplayName);

                GUI.color = Color.yellow;

                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    deleteIndex = i;
                }

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                if (m_SpecialChildSettingFoldouts[i])
                {
                    DoPrefabSettingUILayout(setting);
                }

                EditorGUI.indentLevel--;

                GUILayout.Space(6);
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            GUI.color = Color.red;

            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                AddNewItem(m_SpecialSettings);
                m_SpecialChildSettingFoldouts.Add(false);
            }

            GUI.color = Color.white;

            EditorGUILayout.EndHorizontal();

            if (deleteIndex >= 0 && deleteIndex < m_SpecialSettings.arraySize)
            {
                m_SpecialSettings.DeleteArrayElementAtIndex(deleteIndex);
                m_SpecialChildSettingFoldouts.RemoveAt(deleteIndex);
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Local Methods

        void AddNewItem(SerializedProperty ownerProperty)
        {
            GenericMenu window = new GenericMenu();
            foreach (var data in m_ModifiedTypeValueToDisplayMapping)
            {
                var value = data.Key;
                var displayName = data.Value;
                window.AddItem(new GUIContent(displayName), false, (System.Object value) =>
                {
                    ownerProperty.arraySize++;
                    serializedObject.ApplyModifiedProperties();
                    var newItemProperty = ownerProperty.GetArrayElementAtIndex(ownerProperty.arraySize - 1);
                    var modifiedType = newItemProperty.FindPropertyRelative("m_ModifiedType");
                    modifiedType.enumValueFlag = (int)value;

                    serializedObject.ApplyModifiedProperties();
                }, value);

                window.AddSeparator("");
            }

            window.ShowAsContext();
        }

        void BuildEnumTypeValueToDisplayMapping()
        {
            m_RuleTypeValueToDisplayMapping = new Dictionary<int, string>();
            var enumType = typeof(ESpecialRuleType);
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                PipelineEnumAttribute attribute = field.GetCustomAttributes(typeof(PipelineEnumAttribute), false).FirstOrDefault() as PipelineEnumAttribute;
                if (attribute != null)
                {
                    m_RuleTypeValueToDisplayMapping[(int)field.GetValue(null)] = attribute.DisplayName;
                }
            }

            m_ModifiedTypeValueToDisplayMapping = new Dictionary<int, string>();
            enumType = typeof(PrefabModifiedType);
            fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                PipelineEnumAttribute attribute = field.GetCustomAttributes(typeof(PipelineEnumAttribute), false).FirstOrDefault() as PipelineEnumAttribute;
                if (attribute != null)
                {
                    m_ModifiedTypeValueToDisplayMapping[(int)field.GetValue(null)] = attribute.DisplayName;
                }
            }
        }

        void ModifyCompPropertyValueUILayout(SerializedProperty parent)
        {
            m_AssemblyName = parent.FindPropertyRelative("m_AssemblyName");
            m_ComponentTypeName = parent.FindPropertyRelative("m_ComponentTypeName");
            m_ComponentPropertyName = parent.FindPropertyRelative("m_ComponentPropertyName");
            m_ComponentPropertyValueType = parent.FindPropertyRelative("m_ComponentPropertyValueType");
            m_ComponentPropertyValue = parent.FindPropertyRelative("m_ComponentPropertyValue");

            float spaceHeight = 4;

            EditorGUILayout.PropertyField(m_AssemblyName,Styles.AssemblyName);

            GUILayout.Space(spaceHeight);

            EditorGUILayout.PropertyField(m_ComponentTypeName, Styles.ComponentTypeName);

            GUILayout.Space(spaceHeight);

            EditorGUILayout.PropertyField(m_ComponentPropertyName, Styles.ComponentPropertyName);

            GUILayout.Space(spaceHeight);

            EditorGUILayout.PropertyField(m_ComponentPropertyValueType, Styles.ComponentPropertyValueType);

            GUILayout.Space(spaceHeight);

            EditorGUILayout.PropertyField(m_ComponentPropertyValue, Styles.ComponentPropertyValue);

            GUILayout.Space(spaceHeight);
        }

        void DoPrefabSettingUILayout(SerializedProperty parent)
        {
            // Rule ///////////////////
            m_RuleType = parent.FindPropertyRelative("m_RuleType");
            m_AssetParentFolderListForPartFolderRule = parent.FindPropertyRelative("m_AssetParentFolderListForPartFolderRule");
            m_AssetParentFolderListForFullFolderRule = parent.FindPropertyRelative("m_AssetParentFolderListForFullFolderRule");
            m_AssetNameListForAssetNameRule = parent.FindPropertyRelative("m_AssetNameListForAssetNameRule");
            m_AssetPathListForAssetPathRule = parent.FindPropertyRelative("m_AssetPathListForAssetPathRule");

            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical("Box");

            var ruleTypeIntValue = m_RuleType.enumValueFlag;
            var ruleTypeDisplayName = m_RuleTypeValueToDisplayMapping[ruleTypeIntValue];
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Styles.RuleType);
            if (GUILayout.Button(ruleTypeDisplayName))
            {
                GenericMenu window = new GenericMenu();
                foreach (var data in m_RuleTypeValueToDisplayMapping)
                {
                    var value = data.Key;
                    var displayName = data.Value;
                    window.AddItem(new GUIContent(displayName == ruleTypeDisplayName ? (displayName + "     √") : displayName), false, (System.Object value) =>
                    {
                        m_RuleType.enumValueFlag = (int)value;
                        serializedObject.ApplyModifiedProperties();
                    }, value);

                    window.AddSeparator("");
                }

                window.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            EditorGUI.indentLevel++;

            var ruleType = (ESpecialRuleType)m_RuleType.enumValueFlag;
            switch (ruleType)
            {
                case ESpecialRuleType._1_AssetParentPartFolder:
                    EditorGUILayout.PropertyField(m_AssetParentFolderListForPartFolderRule, Styles.AssetParentPartFolder);
                    break;
                case ESpecialRuleType._2_AssetParentFullFolder:
                    EditorGUILayout.PropertyField(m_AssetParentFolderListForFullFolderRule, Styles.AssetParentFullFolder);
                    break;
                case ESpecialRuleType._3_AssetName:
                    EditorGUILayout.PropertyField(m_AssetNameListForAssetNameRule, Styles.AssetName);
                    break;
                case ESpecialRuleType._4_AssetPath:
                    EditorGUILayout.PropertyField(m_AssetPathListForAssetPathRule, Styles.AssetPath);
                    break;
            }

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;

            GUILayout.Space(4);

            m_ModifiedType = parent.FindPropertyRelative("m_ModifiedType");
            var modifiedType = (PrefabModifiedType)m_ModifiedType.enumValueFlag;
            switch (modifiedType)
            {
                case PrefabModifiedType.CompPropertyValue:
                    ModifyCompPropertyValueUILayout(parent);
                    break;
                case PrefabModifiedType.SpineUnityUseClipping:
                    break;
                    // case ...
            }

            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }

        #endregion
    }
}

