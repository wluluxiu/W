namespace jj.TATools.Editor
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using UnityEngineInternal;

    [System.Serializable]
    internal class AudioClipSetting
    {
        #region Fields

        public ESpecialRuleType m_RuleType = ESpecialRuleType._0_Default;
        public List<string> m_AssetParentFolderListForPartFolderRule = new List<string>();
        public List<string> m_AssetParentFolderListForFullFolderRule = new List<string>();
        public List<string> m_AssetNameListForAssetNameRule = new List<string>();
        public List<string> m_AssetPathListForAssetPathRule = new List<string>();

        // Default
        public PropertySetMode m_ForceToMonoSetMode = PropertySetMode.Ignore;
        public bool m_ForceToMono = true;
        public PropertySetMode m_LoadInBackgroundSetMode = PropertySetMode.Ignore;
        public bool m_LoadInBackground = true;
        // Android
        public PropertySetMode m_LoadTypeForAndroidSetMode = PropertySetMode.Ignore;
        public AudioClipLoadType m_LoadTypeForAndroid = AudioClipLoadType.CompressedInMemory;
        public PropertySetMode m_PreloadAudioDataForAndroidSetMode = PropertySetMode.Ignore;
        public bool m_PreloadAudioDataForAndroid = true;
        public PropertySetMode m_CompressionFormatForAndroidSetMode = PropertySetMode.Ignore;
        public AudioCompressionFormat m_CompressionFormatForAndroid = AudioCompressionFormat.Vorbis;
        public PropertySetMode m_QualityForAndroidSetMode = PropertySetMode.Ignore;
        [Range(0, 100)]
        public int m_QualityForAndroid = 100;
        public PropertySetMode m_SampleRateSettingForAndroidSetMode = PropertySetMode.Ignore;
        public AudioSampleRateSetting m_SampleRateSettingForAndroid = AudioSampleRateSetting.PreserveSampleRate;
        // IOS
        public PropertySetMode m_LoadTypeForIosSetMode = PropertySetMode.Ignore;
        public AudioClipLoadType m_LoadTypeForIos = AudioClipLoadType.CompressedInMemory;
        public PropertySetMode m_PreloadAudioDataForIosSetMode = PropertySetMode.Ignore;
        public bool m_PreloadAudioDataForIos = true;
        public PropertySetMode m_CompressionFormatForIosSetMode = PropertySetMode.Ignore;
        public AudioCompressionFormat m_CompressionFormatForIos = AudioCompressionFormat.Vorbis;
        public PropertySetMode m_QualityForIosSetMode = PropertySetMode.Ignore;
        public int m_QualityForIos = 100;
        public PropertySetMode m_SampleRateSettingForIosSetMode = PropertySetMode.Ignore;
        public AudioSampleRateSetting m_SampleRateSettingForIos = AudioSampleRateSetting.PreserveSampleRate;

        #endregion

        #region Internal Methods

        internal static AudioClipSetting GetDefaultSetting()
        {
            var defaultSetting = new AudioClipSetting();

            defaultSetting.m_RuleType = ESpecialRuleType._0_Default;
            defaultSetting.m_AssetParentFolderListForPartFolderRule = new List<string>();
            defaultSetting.m_AssetParentFolderListForFullFolderRule = new List<string>();
            defaultSetting.m_AssetNameListForAssetNameRule = new List<string>();
            defaultSetting.m_AssetPathListForAssetPathRule = new List<string>();

            // Default
            defaultSetting.m_ForceToMonoSetMode = PropertySetMode.Force;
            defaultSetting.m_ForceToMono = true;
            defaultSetting.m_LoadInBackgroundSetMode = PropertySetMode.Force;
            defaultSetting.m_LoadInBackground = true;
            // Android
            defaultSetting.m_LoadTypeForAndroidSetMode = PropertySetMode.Force;
            defaultSetting.m_LoadTypeForAndroid = AudioClipLoadType.CompressedInMemory;
            defaultSetting.m_PreloadAudioDataForAndroidSetMode = PropertySetMode.Force;
            defaultSetting.m_PreloadAudioDataForAndroid = true;
            defaultSetting.m_CompressionFormatForAndroidSetMode = PropertySetMode.Force;
            defaultSetting.m_CompressionFormatForAndroid = AudioCompressionFormat.Vorbis;
            defaultSetting.m_QualityForAndroidSetMode = PropertySetMode.Once;
            defaultSetting.m_QualityForAndroid = 100;
            defaultSetting.m_SampleRateSettingForAndroidSetMode = PropertySetMode.Once;
            defaultSetting.m_SampleRateSettingForAndroid = AudioSampleRateSetting.PreserveSampleRate;
            // IOS
            defaultSetting.m_LoadTypeForIosSetMode = PropertySetMode.Force;
            defaultSetting.m_LoadTypeForIos = AudioClipLoadType.CompressedInMemory;
            defaultSetting.m_PreloadAudioDataForIosSetMode = PropertySetMode.Force;
            defaultSetting.m_PreloadAudioDataForIos = true;
            defaultSetting.m_CompressionFormatForIosSetMode = PropertySetMode.Force;
            defaultSetting.m_CompressionFormatForIos = AudioCompressionFormat.Vorbis;
            defaultSetting.m_QualityForIosSetMode = PropertySetMode.Once;
            defaultSetting.m_QualityForIos = 100;
            defaultSetting.m_SampleRateSettingForIosSetMode = PropertySetMode.Once;
            defaultSetting.m_SampleRateSettingForIos = AudioSampleRateSetting.PreserveSampleRate;

            return defaultSetting;
        }

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

        internal void DoPreImport(AudioImporter aImporter)
        {
            // First Imported //////////////////
            bool isFirstImported = aImporter.userData != AssetImportPipelineUtility.ASSET_FIRST_IMPORTED;
            if (isFirstImported)
                aImporter.userData = AssetImportPipelineUtility.ASSET_FIRST_IMPORTED;

            // Default /////////////////////////
            // forceToMono
            if (m_ForceToMonoSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) aImporter.forceToMono = m_ForceToMono;
            }
            else if (m_ForceToMonoSetMode == PropertySetMode.Force)
                aImporter.forceToMono = m_ForceToMono;
            // Load In Background
            if (m_LoadInBackgroundSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) aImporter.loadInBackground = m_LoadInBackground;
            }
            else if (m_LoadInBackgroundSetMode == PropertySetMode.Force)
                aImporter.loadInBackground = m_LoadInBackground;
            // Load In Background
            if (m_LoadInBackgroundSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) aImporter.loadInBackground = m_LoadInBackground;
            }
            else if (m_LoadInBackgroundSetMode == PropertySetMode.Force)
                aImporter.loadInBackground = m_LoadInBackground;

            // Android /////////////////////////
            var androidSettings = aImporter.GetOverrideSampleSettings(BuildTargetGroup.Android);

            // Load Type
            if (m_LoadTypeForAndroidSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) androidSettings.loadType = m_LoadTypeForAndroid;
            }
            else if (m_LoadTypeForAndroidSetMode == PropertySetMode.Force)
                androidSettings.loadType = m_LoadTypeForAndroid;
            // m_PreloadAudioData
            if (androidSettings.loadType == AudioClipLoadType.Streaming)
                androidSettings.preloadAudioData = false;
            else
            {
                if (m_PreloadAudioDataForAndroidSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported) androidSettings.preloadAudioData = m_PreloadAudioDataForAndroid;
                }
                else if (m_PreloadAudioDataForAndroidSetMode == PropertySetMode.Force)
                    androidSettings.preloadAudioData = m_PreloadAudioDataForAndroid;
            }
            // Compression Format
            if (m_CompressionFormatForAndroidSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) androidSettings.compressionFormat = m_CompressionFormatForAndroid;
            }
            else if (m_CompressionFormatForAndroidSetMode == PropertySetMode.Force)
                androidSettings.compressionFormat = m_CompressionFormatForAndroid;
            // Quality
            var qualityValue = m_QualityForAndroid / 100.0f;
            if (m_QualityForAndroidSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) androidSettings.quality = qualityValue;
            }
            else if (m_QualityForAndroidSetMode == PropertySetMode.Force)
                androidSettings.quality = qualityValue;
            // Sample Rate Setting 
            if (m_SampleRateSettingForAndroidSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) androidSettings.sampleRateSetting = m_SampleRateSettingForAndroid;
            }
            else if (m_SampleRateSettingForAndroidSetMode == PropertySetMode.Force)
                androidSettings.sampleRateSetting = m_SampleRateSettingForAndroid;

            aImporter.SetOverrideSampleSettings(BuildTargetGroup.Android, androidSettings);
            // IOS //////////////////////////////
            var iosSettings = aImporter.GetOverrideSampleSettings(BuildTargetGroup.iOS);

            // Load Type
            if (m_LoadTypeForIosSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) iosSettings.loadType = m_LoadTypeForIos;
            }
            else if (m_LoadTypeForIosSetMode == PropertySetMode.Force)
                iosSettings.loadType = m_LoadTypeForIos;
            // m_PreloadAudioData
            if (iosSettings.loadType == AudioClipLoadType.Streaming)
                iosSettings.preloadAudioData = false;
            else
            {
                if (m_PreloadAudioDataForIosSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported) iosSettings.preloadAudioData = m_PreloadAudioDataForIos;
                }
                else if (m_PreloadAudioDataForIosSetMode == PropertySetMode.Force)
                    iosSettings.preloadAudioData = m_PreloadAudioDataForIos;
            }
            // Compression Format
            if (m_CompressionFormatForIosSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) iosSettings.compressionFormat = m_CompressionFormatForIos;
            }
            else if (m_CompressionFormatForIosSetMode == PropertySetMode.Force)
                iosSettings.compressionFormat = m_CompressionFormatForIos;
            // Quality
            qualityValue = m_QualityForIos / 100.0f;
            if (m_QualityForIosSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) iosSettings.quality = qualityValue;
            }
            else if (m_QualityForIosSetMode == PropertySetMode.Force)
                iosSettings.quality = qualityValue;
            // Sample Rate Setting 
            if (m_SampleRateSettingForIosSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) iosSettings.sampleRateSetting = m_SampleRateSettingForIos;
            }
            else if (m_SampleRateSettingForIosSetMode == PropertySetMode.Force)
                iosSettings.sampleRateSetting = m_SampleRateSettingForIos;

            aImporter.SetOverrideSampleSettings(BuildTargetGroup.iOS, iosSettings);
        }

        #endregion
    }

    internal class AudioClipImportPipeline : ScriptableObject
    {
        #region Fields

        [SerializeField]
        List<AudioClipSetting> m_SpecialSettings;

        List<AudioClipSetting> m_SortedSpecialSettings;

        #endregion

        #region Internal Methods

        internal void AutoSortSettings()
        {
            m_SortedSpecialSettings = new List<AudioClipSetting>(m_SpecialSettings);
            m_SortedSpecialSettings.Sort((x, y) => string.CompareOrdinal(((int)x.m_RuleType).ToString(), ((int)y.m_RuleType).ToString()));
        }

        internal void DoPreImport(AudioImporter aImporter)
        {
            if (m_SortedSpecialSettings != null && m_SortedSpecialSettings.Count > 0)
            {
                foreach (var setting in m_SortedSpecialSettings)
                {
                    if (setting.CheckRule(aImporter.assetPath))
                        setting.DoPreImport(aImporter);
                }
            }
        }

        #endregion
    }

    [CustomEditor(typeof(AudioClipImportPipeline))]
    internal class AudioClipImportPipelineInspector : Editor
    {
        #region Fields

        class Styles
        {
            internal static readonly GUIContent SetMode = new GUIContent("", "Once-资源首次导入工程修改，Force资源每次重新导入时修改。");
            internal static readonly GUIContent[] Platforms = new GUIContent[] {
                new GUIContent("Android",""),
                new GUIContent("IOS","")
            };

            internal static readonly GUIContent SpecialSettings = new GUIContent("自定义配置:");

            internal static readonly GUIContent RuleType = new GUIContent("规则类型", "1.None:通用规则;\n2.AssetPath:资源路径;\n3.AssetName:资源名;\n4.AssetParentFullFolder:全目录;\n5.AssetParentPartFolder:部分目录;");
            internal static readonly GUIContent AssetPath = new GUIContent("资源路径", "格式：Assets/XXX/YYY/test.png");
            internal static readonly GUIContent AssetName = new GUIContent("资源名", "格式: _ASTC_4x4 或 _RW");
            internal static readonly GUIContent AssetParentFullFolder = new GUIContent("全目录", "格式：Assets/XXX/YYY/");
            internal static readonly GUIContent AssetParentPartFolder = new GUIContent("部分目录", "格式：/XXX/YYY/ ");

            public static readonly GUIContent ForceToMono = EditorGUIUtility.TrTextContent("Force To Mono");
            public static readonly GUIContent LoadInBackground = EditorGUIUtility.TrTextContent("Load In Background");
            public static readonly GUIContent LoadType = EditorGUIUtility.TrTextContent("Load Type");
            public static readonly GUIContent PreloadAudioData = EditorGUIUtility.TrTextContent("Preload Audio Data*");
            public static readonly GUIContent CompressionFormat = EditorGUIUtility.TrTextContent("Compression Format");
            public static readonly GUIContent Quality = EditorGUIUtility.TrTextContent("Quality");
            public static readonly GUIContent SampleRateSetting = EditorGUIUtility.TrTextContent("Sample Rate Setting");
        }

        // Base Property
        private SerializedProperty m_SpecialSettings;

        // Children Property
        private SerializedProperty m_RuleType;

        private SerializedProperty m_AssetParentFolderListForPartFolderRule;
        private SerializedProperty m_AssetParentFolderListForFullFolderRule;
        private SerializedProperty m_AssetNameListForAssetNameRule;
        private SerializedProperty m_AssetPathListForAssetPathRule;

        // Default
        private SerializedProperty m_ForceToMonoSetMode;
        private SerializedProperty m_ForceToMono;
        private SerializedProperty m_LoadInBackgroundSetMode;
        private SerializedProperty m_LoadInBackground;
        // Android
        private SerializedProperty m_LoadTypeForAndroidSetMode;
        private SerializedProperty m_LoadTypeForAndroid;
        private SerializedProperty m_PreloadAudioDataForAndroidSetMode;
        private SerializedProperty m_PreloadAudioDataForAndroid;
        private SerializedProperty m_CompressionFormatForAndroidSetMode;
        private SerializedProperty m_CompressionFormatForAndroid;
        private SerializedProperty m_QualityForAndroidSetMode;
        private SerializedProperty m_QualityForAndroid;
        private SerializedProperty m_SampleRateSettingForAndroidSetMode;
        private SerializedProperty m_SampleRateSettingForAndroid;
        // IOS
        private SerializedProperty m_LoadTypeForIosSetMode;
        private SerializedProperty m_LoadTypeForIos;
        private SerializedProperty m_PreloadAudioDataForIosSetMode;
        private SerializedProperty m_PreloadAudioDataForIos;
        private SerializedProperty m_CompressionFormatForIosSetMode;
        private SerializedProperty m_CompressionFormatForIos;
        private SerializedProperty m_QualityForIosSetMode;
        private SerializedProperty m_QualityForIos;
        private SerializedProperty m_SampleRateSettingForIosSetMode;
        private SerializedProperty m_SampleRateSettingForIos;

        private int m_PlatformIndex = 0;
        private List<bool> m_SpecialChildSettingFoldouts = new List<bool>();

        private static Dictionary<int, string> m_RuleTypeValueToDisplayMapping = null;

        #endregion

        #region Editor Methods

        void OnEnable()
        {
            m_SpecialSettings = serializedObject.FindProperty("m_SpecialSettings");

            BuildRuleTypeValueToDisplayMapping();
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
                var ruleTypeIntValue = setting.FindPropertyRelative("m_RuleType").enumValueFlag;
                var ruleTypeDisplayName = m_RuleTypeValueToDisplayMapping[ruleTypeIntValue];
                string title = "";
                if (ruleTypeIntValue == (int)ESpecialRuleType._1_AssetParentPartFolder)
                {
                    var assetParentFolderListForPartFolderRule = setting.FindPropertyRelative("m_AssetParentFolderListForPartFolderRule");
                    if (assetParentFolderListForPartFolderRule.arraySize > 0)
                    {
                        title = assetParentFolderListForPartFolderRule.GetArrayElementAtIndex(0).stringValue;
                    }
                }
                if (ruleTypeIntValue == (int)ESpecialRuleType._2_AssetParentFullFolder)
                {
                    var assetParentFolderListForFullFolderRule = setting.FindPropertyRelative("m_AssetParentFolderListForFullFolderRule");
                    if (assetParentFolderListForFullFolderRule.arraySize > 0)
                    {
                        title = assetParentFolderListForFullFolderRule.GetArrayElementAtIndex(0).stringValue;
                    }
                }
                if (ruleTypeIntValue == (int)ESpecialRuleType._3_AssetName)
                {
                    var assetNameListForAssetNameRule = setting.FindPropertyRelative("m_AssetNameListForAssetNameRule");
                    if (assetNameListForAssetNameRule.arraySize > 0)
                    {
                        title = assetNameListForAssetNameRule.GetArrayElementAtIndex(0).stringValue;
                    }
                }
                if (ruleTypeIntValue == (int)ESpecialRuleType._4_AssetPath)
                {
                    var assetPathListForAssetPathRule = setting.FindPropertyRelative("m_AssetPathListForAssetPathRule");
                    if (assetPathListForAssetPathRule.arraySize > 0)
                    {
                        title = Path.GetFileName(assetPathListForAssetPathRule.GetArrayElementAtIndex(0).stringValue);
                    }
                }

                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();

                m_SpecialChildSettingFoldouts[i] = EditorGUILayout.Foldout(m_SpecialChildSettingFoldouts[i], ruleTypeDisplayName + (string.IsNullOrEmpty(title) ? "" : ("[" + title + "]")));

                GUI.color = Color.yellow;

                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    deleteIndex = i;
                }

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                if (m_SpecialChildSettingFoldouts[i])
                {
                    DoTextureSettingUILayout(setting);
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
            ownerProperty.arraySize++;
            serializedObject.ApplyModifiedProperties();
            var newItemProperty = ownerProperty.GetArrayElementAtIndex(ownerProperty.arraySize - 1);
            var newValue = AudioClipSetting.GetDefaultSetting();
            newItemProperty.boxedValue = newValue;
            serializedObject.ApplyModifiedProperties();
        }

        void BuildRuleTypeValueToDisplayMapping()
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
        }

        void DoTexturePropertyGroupUILayout(SerializedProperty data, GUIContent dataGUIContent, SerializedProperty setMode)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(data, dataGUIContent);

            EditorGUILayout.PropertyField(setMode, Styles.SetMode, GUILayout.Width(100));

            EditorGUILayout.EndHorizontal();
        }

        void DoTextureSettingUILayout(SerializedProperty parent)
        {
            // Get Child Properties
            m_RuleType = parent.FindPropertyRelative("m_RuleType");
            m_AssetParentFolderListForPartFolderRule = parent.FindPropertyRelative("m_AssetParentFolderListForPartFolderRule");
            m_AssetParentFolderListForFullFolderRule = parent.FindPropertyRelative("m_AssetParentFolderListForFullFolderRule");
            m_AssetNameListForAssetNameRule = parent.FindPropertyRelative("m_AssetNameListForAssetNameRule");
            m_AssetPathListForAssetPathRule = parent.FindPropertyRelative("m_AssetPathListForAssetPathRule");

            // Default
            m_ForceToMonoSetMode = parent.FindPropertyRelative("m_ForceToMonoSetMode");
            m_ForceToMono = parent.FindPropertyRelative("m_ForceToMono");
            m_LoadInBackgroundSetMode = parent.FindPropertyRelative("m_LoadInBackgroundSetMode");
            m_LoadInBackground = parent.FindPropertyRelative("m_LoadInBackground");
            // Android
            m_LoadTypeForAndroidSetMode = parent.FindPropertyRelative("m_LoadTypeForAndroidSetMode");
            m_LoadTypeForAndroid = parent.FindPropertyRelative("m_LoadTypeForAndroid");
            m_PreloadAudioDataForAndroidSetMode = parent.FindPropertyRelative("m_PreloadAudioDataForAndroidSetMode");
            m_PreloadAudioDataForAndroid = parent.FindPropertyRelative("m_PreloadAudioDataForAndroid");
            m_CompressionFormatForAndroidSetMode = parent.FindPropertyRelative("m_CompressionFormatForAndroidSetMode");
            m_CompressionFormatForAndroid = parent.FindPropertyRelative("m_CompressionFormatForAndroid");
            m_QualityForAndroidSetMode = parent.FindPropertyRelative("m_QualityForAndroidSetMode");
            m_QualityForAndroid = parent.FindPropertyRelative("m_QualityForAndroid");
            m_SampleRateSettingForAndroidSetMode = parent.FindPropertyRelative("m_SampleRateSettingForAndroidSetMode");
            m_SampleRateSettingForAndroid = parent.FindPropertyRelative("m_SampleRateSettingForAndroid");
            // Ios
            m_LoadTypeForIosSetMode = parent.FindPropertyRelative("m_LoadTypeForIosSetMode");
            m_LoadTypeForIos = parent.FindPropertyRelative("m_LoadTypeForIos");
            m_PreloadAudioDataForIosSetMode = parent.FindPropertyRelative("m_PreloadAudioDataForIosSetMode");
            m_PreloadAudioDataForIos = parent.FindPropertyRelative("m_PreloadAudioDataForIos");
            m_CompressionFormatForIosSetMode = parent.FindPropertyRelative("m_CompressionFormatForIosSetMode");
            m_CompressionFormatForIos = parent.FindPropertyRelative("m_CompressionFormatForIos");
            m_QualityForIosSetMode = parent.FindPropertyRelative("m_QualityForIosSetMode");
            m_QualityForIos = parent.FindPropertyRelative("m_QualityForIos");
            m_SampleRateSettingForIosSetMode = parent.FindPropertyRelative("m_SampleRateSettingForIosSetMode");
            m_SampleRateSettingForIos = parent.FindPropertyRelative("m_SampleRateSettingForIos");


            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical("Box");

            float spaceHeight = 4;

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

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_ForceToMono, Styles.ForceToMono, m_ForceToMonoSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_LoadInBackground, Styles.LoadInBackground, m_LoadInBackgroundSetMode);

            GUILayout.Space(spaceHeight);

            m_PlatformIndex = GUILayout.Toolbar(m_PlatformIndex, Styles.Platforms);

            EditorGUI.indentLevel++;

            switch (m_PlatformIndex)
            {
                case 0:

                    DoTexturePropertyGroupUILayout(m_LoadTypeForAndroid, Styles.LoadType, m_LoadTypeForAndroidSetMode);

                    DoTexturePropertyGroupUILayout(m_PreloadAudioDataForAndroid, Styles.PreloadAudioData, m_PreloadAudioDataForAndroidSetMode);

                    DoTexturePropertyGroupUILayout(m_CompressionFormatForAndroid, Styles.CompressionFormat, m_CompressionFormatForAndroidSetMode);

                    DoTexturePropertyGroupUILayout(m_QualityForAndroid, Styles.Quality, m_QualityForAndroidSetMode);

                    DoTexturePropertyGroupUILayout(m_SampleRateSettingForAndroid, Styles.SampleRateSetting, m_SampleRateSettingForAndroidSetMode);

                    break;
                case 1:

                    DoTexturePropertyGroupUILayout(m_LoadTypeForIos, Styles.LoadType, m_LoadTypeForIosSetMode);

                    DoTexturePropertyGroupUILayout(m_PreloadAudioDataForIos, Styles.PreloadAudioData, m_PreloadAudioDataForIosSetMode);

                    DoTexturePropertyGroupUILayout(m_CompressionFormatForIos, Styles.CompressionFormat, m_CompressionFormatForIosSetMode);

                    DoTexturePropertyGroupUILayout(m_QualityForIos, Styles.Quality, m_QualityForIosSetMode);

                    DoTexturePropertyGroupUILayout(m_SampleRateSettingForIos, Styles.SampleRateSetting, m_SampleRateSettingForIosSetMode);

                    break;
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }

        #endregion
    }
}