namespace jj.TATools.Editor
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.U2D;
    using UnityEditor.U2D;

    [System.Serializable]
    internal class SpriteAtlasSetting
    {
        #region Fields

        public ESpecialRuleType m_RuleType = ESpecialRuleType._0_Default;
        public List<string> m_AssetParentFolderListForPartFolderRule = new List<string>();
        public List<string> m_AssetParentFolderListForFullFolderRule = new List<string>();
        public List<string> m_AssetNameListForAssetNameRule = new List<string>();
        public List<string> m_AssetPathListForAssetPathRule = new List<string>();

        public PropertySetMode m_ReadableSetMode = PropertySetMode.Force;
        public bool m_Readable = false;
        public PropertySetMode m_GenerateMipmapSetMode = PropertySetMode.Once;
        public bool m_GenerateMipmap = false;
        public PropertySetMode m_SRGBSetMode = PropertySetMode.Once;
        public bool m_SRGB = true;
        public PropertySetMode m_FilterModeSetMode = PropertySetMode.Once;
        public FilterMode m_FilterMode = FilterMode.Bilinear;
        public PropertySetMode m_AnisoLevelSetMode = PropertySetMode.Once;
        public int m_AnisoLevel = 1;
        public PropertySetMode m_MaxSizeForAndroidSetMode = PropertySetMode.Once;
        public int m_MaxSizeForAndroid = 2048;
        public PropertySetMode m_FormatForAndroidSetMode = PropertySetMode.Once;
        public TextureImporterFormat m_FormatForAndroid = TextureImporterFormat.ASTC_6x6;
        public PropertySetMode m_CompressionForAndroidSetMode = PropertySetMode.Once;
        public TextureImporterCompression m_CompressionForAndroid = TextureImporterCompression.Compressed;
        public PropertySetMode m_MaxSizeForIosSetMode = PropertySetMode.Once;
        public int m_MaxSizeForIos = 2048;
        public PropertySetMode m_FormatForIosSetMode = PropertySetMode.Force;
        public TextureImporterFormat m_FormatForIos = TextureImporterFormat.ASTC_6x6;
        public PropertySetMode m_CompressionForIosSetMode = PropertySetMode.Once;
        public TextureImporterCompression m_CompressionForIos = TextureImporterCompression.Compressed;

        #endregion

        #region Internal Methods

        internal static SpriteAtlasSetting GetDefaultSetting()
        {
            var defaultSetting = new SpriteAtlasSetting();

            defaultSetting.m_RuleType = ESpecialRuleType._0_Default;
            defaultSetting.m_AssetParentFolderListForPartFolderRule = new List<string>();
            defaultSetting.m_AssetParentFolderListForFullFolderRule = new List<string>();
            defaultSetting.m_AssetNameListForAssetNameRule = new List<string>();
            defaultSetting.m_AssetPathListForAssetPathRule = new List<string>();

            defaultSetting.m_ReadableSetMode = PropertySetMode.Force;
            defaultSetting.m_Readable = false;
            defaultSetting.m_GenerateMipmapSetMode = PropertySetMode.Force;
            defaultSetting.m_GenerateMipmap = false;
            defaultSetting.m_SRGBSetMode = PropertySetMode.Once;
            defaultSetting.m_SRGB = true;
            defaultSetting.m_FilterModeSetMode = PropertySetMode.Force;
            defaultSetting.m_FilterMode = FilterMode.Bilinear;
            defaultSetting.m_AnisoLevelSetMode = PropertySetMode.Force;
            defaultSetting.m_AnisoLevel = 1;
            defaultSetting.m_MaxSizeForAndroidSetMode = PropertySetMode.Once;
            defaultSetting.m_MaxSizeForAndroid = 2048;
            defaultSetting.m_FormatForAndroidSetMode = PropertySetMode.Force;
            defaultSetting.m_FormatForAndroid = TextureImporterFormat.ASTC_4x4;
            defaultSetting.m_CompressionForAndroidSetMode = PropertySetMode.Force;
            defaultSetting.m_CompressionForAndroid = TextureImporterCompression.Compressed;
            defaultSetting.m_MaxSizeForIosSetMode = PropertySetMode.Once;
            defaultSetting.m_MaxSizeForIos = 2048;
            defaultSetting.m_FormatForIosSetMode = PropertySetMode.Force;
            defaultSetting.m_FormatForIos = TextureImporterFormat.ASTC_4x4;
            defaultSetting.m_CompressionForIosSetMode = PropertySetMode.Force;
            defaultSetting.m_CompressionForIos = TextureImporterCompression.Compressed;

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

        internal void DoPreImport(string spriteAtlasPath)
        {
            var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(spriteAtlasPath);
            var needSaveAssetIfDirty = false;

            // First Imported //////////////////
            var androidPlatformSeting = spriteAtlas.GetPlatformSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_ANDROID);
            var iosPlatformSeting = spriteAtlas.GetPlatformSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_IOS);
            bool isFirstImported = (androidPlatformSeting != null && !androidPlatformSeting.overridden) || (iosPlatformSeting != null && !iosPlatformSeting.overridden);

            var needUpdateTS = false;
            var textureSettings = spriteAtlas.GetTextureSettings();
            // isReadable
            if (m_ReadableSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    if (textureSettings.readable != m_Readable)
                    {
                        textureSettings.readable = m_Readable;
                        needSaveAssetIfDirty = true;
                        needUpdateTS = true;
                    }
                }
            }
            else if (m_ReadableSetMode == PropertySetMode.Force)
            {
                if (textureSettings.readable != m_Readable)
                {
                    textureSettings.readable = m_Readable;
                    needSaveAssetIfDirty = true;
                    needUpdateTS = true;
                }
            }

            // Generate Mipmap
            if (m_GenerateMipmapSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    if (textureSettings.generateMipMaps != m_GenerateMipmap)
                    {
                        textureSettings.generateMipMaps = m_GenerateMipmap;
                        needSaveAssetIfDirty = true;
                        needUpdateTS = true;
                    }
                }
            }
            else if (m_GenerateMipmapSetMode == PropertySetMode.Force)
            {
                if (textureSettings.generateMipMaps != m_GenerateMipmap)
                {
                    textureSettings.generateMipMaps = m_GenerateMipmap;
                    needSaveAssetIfDirty = true;
                    needUpdateTS = true;
                }
            }
            // srgb
            if (m_SRGBSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    if (textureSettings.sRGB != m_SRGB)
                    {
                        textureSettings.sRGB = m_SRGB;
                        needSaveAssetIfDirty = true;
                        needUpdateTS = true;
                    }
                }
            }
            else if (m_SRGBSetMode == PropertySetMode.Force)
            {
                if (textureSettings.sRGB != m_SRGB)
                {
                    textureSettings.sRGB = m_SRGB;
                    needSaveAssetIfDirty = true;
                    needUpdateTS = true;
                }
            }
            // filterMode
            if (m_FilterModeSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    if (textureSettings.filterMode != m_FilterMode)
                    {
                        textureSettings.filterMode = m_FilterMode;
                        needSaveAssetIfDirty = true;
                        needUpdateTS = true;
                    }
                }
            }
            else if (m_FilterModeSetMode == PropertySetMode.Force)
            {
                if (textureSettings.filterMode != m_FilterMode)
                {
                    textureSettings.filterMode = m_FilterMode;
                    needSaveAssetIfDirty = true;
                    needUpdateTS = true;
                }
            }
            // anisoLevel
            if (m_AnisoLevelSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    if (textureSettings.anisoLevel != m_AnisoLevel)
                    {
                        textureSettings.anisoLevel = m_AnisoLevel;
                        needSaveAssetIfDirty = true;
                        needUpdateTS = true;
                    }
                }
            }
            else if (m_AnisoLevelSetMode == PropertySetMode.Force)
            {
                if (textureSettings.anisoLevel != m_AnisoLevel)
                {
                    textureSettings.anisoLevel = m_AnisoLevel;
                    needSaveAssetIfDirty = true;
                    needUpdateTS = true;
                }
            }

            if(needUpdateTS)
               spriteAtlas.SetTextureSettings(textureSettings);

            // Android Platform 
            if (androidPlatformSeting != null)
            {
                var needUpdateAndroidSetting = false;

                if (androidPlatformSeting.overridden != true)
                {
                    androidPlatformSeting.overridden = true;
                    needSaveAssetIfDirty = true;
                    needUpdateAndroidSetting = true;
                }

                // Max Size
                if (m_MaxSizeForAndroidSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        if (androidPlatformSeting.maxTextureSize != m_MaxSizeForAndroid)
                        {
                            androidPlatformSeting.maxTextureSize = m_MaxSizeForAndroid;
                            needSaveAssetIfDirty = true;
                            needUpdateAndroidSetting = true;
                        }
                    }
                }
                else if (m_MaxSizeForAndroidSetMode == PropertySetMode.Force)
                {
                    if (androidPlatformSeting.maxTextureSize != m_MaxSizeForAndroid)
                    {
                        androidPlatformSeting.maxTextureSize = m_MaxSizeForAndroid;
                        needSaveAssetIfDirty = true;
                        needUpdateAndroidSetting = true;
                    }
                }

                // Format
                if (m_FormatForAndroidSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        if (androidPlatformSeting.format != m_FormatForAndroid)
                        {
                            androidPlatformSeting.format = m_FormatForAndroid;
                            needSaveAssetIfDirty = true;
                            needUpdateAndroidSetting = true;
                        }
                    }
                }
                else if (m_FormatForAndroidSetMode == PropertySetMode.Force)
                {
                    if (androidPlatformSeting.format != m_FormatForAndroid)
                    {
                        androidPlatformSeting.format = m_FormatForAndroid;
                        needSaveAssetIfDirty = true;
                        needUpdateAndroidSetting = true;
                    }
                }

                // Compressor Quality
                if (m_CompressionForAndroidSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        if (androidPlatformSeting.textureCompression != m_CompressionForAndroid)
                        {
                            androidPlatformSeting.textureCompression = m_CompressionForAndroid;
                            needSaveAssetIfDirty = true;
                            needUpdateAndroidSetting = true;
                        }
                    }
                }
                else if (m_CompressionForAndroidSetMode == PropertySetMode.Force)
                {
                    if (androidPlatformSeting.textureCompression != m_CompressionForAndroid)
                    {
                        androidPlatformSeting.textureCompression = m_CompressionForAndroid;
                        needSaveAssetIfDirty = true;
                        needUpdateAndroidSetting = true;
                    }
                }

                if (needUpdateAndroidSetting)
                    spriteAtlas.SetPlatformSettings(androidPlatformSeting);
            }

            // IOS Platform 
            if (iosPlatformSeting != null)
            {
                var needUpdateIosSetting = false;
                if (iosPlatformSeting.overridden != true)
                {
                    iosPlatformSeting.overridden = true;
                    needSaveAssetIfDirty = true;
                    needUpdateIosSetting = true;
                }

                // Max Size
                if (m_MaxSizeForIosSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        if (iosPlatformSeting.maxTextureSize != m_MaxSizeForIos)
                        {
                            iosPlatformSeting.maxTextureSize = m_MaxSizeForIos;
                            needSaveAssetIfDirty = true;
                            needUpdateIosSetting = true;
                        }
                    }
                }
                else if (m_MaxSizeForIosSetMode == PropertySetMode.Force)
                {
                    if (iosPlatformSeting.maxTextureSize != m_MaxSizeForIos)
                    {
                        iosPlatformSeting.maxTextureSize = m_MaxSizeForIos;
                        needSaveAssetIfDirty = true;
                        needUpdateIosSetting = true;
                    }
                }
                // Format
                if (m_FormatForIosSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        if (iosPlatformSeting.format != m_FormatForIos)
                        {
                            iosPlatformSeting.format = m_FormatForIos;
                            needSaveAssetIfDirty = true;
                            needUpdateIosSetting = true;
                        }
                    }
                }
                else if (m_FormatForIosSetMode == PropertySetMode.Force)
                {
                    if (iosPlatformSeting.format != m_FormatForIos)
                    {
                        iosPlatformSeting.format = m_FormatForIos;
                        needSaveAssetIfDirty = true;
                        needUpdateIosSetting = true;
                    }
                }
                // Compressor Quality
                if (m_CompressionForIosSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        if (iosPlatformSeting.textureCompression != m_CompressionForIos)
                        {
                            iosPlatformSeting.textureCompression = m_CompressionForIos;
                            needSaveAssetIfDirty = true;
                            needUpdateIosSetting = true;
                        }
                    }
                }
                else if (m_CompressionForIosSetMode == PropertySetMode.Force)
                {
                    if (iosPlatformSeting.textureCompression != m_CompressionForIos)
                    {
                        iosPlatformSeting.textureCompression = m_CompressionForIos;
                        needSaveAssetIfDirty = true;
                        needUpdateIosSetting = true;
                    }
                }

                if (needUpdateIosSetting)
                    spriteAtlas.SetPlatformSettings(iosPlatformSeting);
            }

            if (needSaveAssetIfDirty)
                AssetDatabase.SaveAssetIfDirty(spriteAtlas);
        }

        #endregion
    }

    internal class SpriteAtlasImportPipeline : ScriptableObject
    {
        #region Fields

        const string SPRITE_ATLAS_EXTENSION = ".spriteatlas";

        [SerializeField]
        List<SpriteAtlasSetting> m_SpecialSettings;

        List<SpriteAtlasSetting> m_SortedSpecialSettings;

        #endregion

        #region Internal Methods

        internal void AutoSortSettings()
        {
            m_SortedSpecialSettings = new List<SpriteAtlasSetting>(m_SpecialSettings);
            m_SortedSpecialSettings.Sort((x, y) => string.CompareOrdinal(((int)x.m_RuleType).ToString(), ((int)y.m_RuleType).ToString()));
        }

        internal void DoPreImport(string spriteAtlasPath)
        {
            if (m_SortedSpecialSettings != null && m_SortedSpecialSettings.Count > 0)
            {
                foreach (var setting in m_SortedSpecialSettings)
                {
                    if (setting.CheckRule(spriteAtlasPath))
                        setting.DoPreImport(spriteAtlasPath);
                }
            }
        }

        internal static void AutoSet()
        {
            var tempMapping = new Dictionary<string, SpriteAtlasImportPipeline>();
            var subPipelineRelativePath = AssetImportPipelineUtility.SubPipelineRelativePath;
            var guids = AssetDatabase.FindAssets("t:" + typeof(SpriteAtlasImportPipeline), new string[] { "Assets" });
            if (guids != null)
            {
                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    SpriteAtlasImportPipeline pipeline = AssetDatabase.LoadAssetAtPath<SpriteAtlasImportPipeline>(assetPath);
                    pipeline.AutoSortSettings();

                    var fileName = Path.GetFileName(assetPath);
                    tempMapping[assetPath.Replace(subPipelineRelativePath + fileName, "")] = pipeline;
                }
            }

            foreach (var data in tempMapping)
            {
                var folder = data.Key;
                var pipeline = data.Value;
                var subFiles = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
                foreach (var subFile in subFiles)
                {
                    if (!subFile.EndsWith(SPRITE_ATLAS_EXTENSION)) continue;

                    var assetPath = subFile.Replace(Application.dataPath, "Assets").Replace("\\", "/");
                    pipeline.DoPreImport(assetPath);
                }
            }
        }

        #endregion
    }

    [CustomEditor(typeof(SpriteAtlasImportPipeline))]
    internal class SpriteAtlasImportPipelineInspector : Editor
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

            internal static readonly GUIContent Readable = new GUIContent("Read/Write", "Enable to be able to access the raw pixel data from code.");
            internal static readonly GUIContent GenerateMipmap = new GUIContent("Generate Mipmaps", "");
            internal static readonly GUIContent SRGB = new GUIContent("sRGB", "");
            internal static readonly GUIContent FilterMode = new GUIContent("Filter Mode", "");
            internal static readonly GUIContent AnisoLevel = new GUIContent("Aniso Level", "");
            internal static readonly GUIContent MaxSizeForAndroid = new GUIContent("Max Size", "");
            internal static readonly GUIContent FormatForAndroid = new GUIContent("Format", "Android平台纹理压缩格式。");
            internal static readonly GUIContent CompressionForAndroid = new GUIContent("Compressor Quality", "");
            internal static readonly GUIContent MaxSizeForIos = new GUIContent("Max Size", "");
            internal static readonly GUIContent FormatForIos = new GUIContent("Format", "IOS平台纹理压缩格式。");
            internal static readonly GUIContent CompressionForIos = new GUIContent("Compressor Quality", "");
        }

        // Base Property
        private SerializedProperty m_SpecialSettings;

        // Children Property
        private SerializedProperty m_RuleType;

        private SerializedProperty m_AssetParentFolderListForPartFolderRule;
        private SerializedProperty m_AssetParentFolderListForFullFolderRule;
        private SerializedProperty m_AssetNameListForAssetNameRule;
        private SerializedProperty m_AssetPathListForAssetPathRule;
  
        private SerializedProperty m_Readable;
        private SerializedProperty m_GenerateMipmap;
        private SerializedProperty m_SRGB;
        private SerializedProperty m_FilterMode;
        private SerializedProperty m_AnisoLevel;
        private SerializedProperty m_MaxSizeForAndroid;
        private SerializedProperty m_FormatForAndroid;
        private SerializedProperty m_CompressionForAndroid;
        private SerializedProperty m_MaxSizeForIos;
        private SerializedProperty m_FormatForIos;
        private SerializedProperty m_CompressionForIos;

        private SerializedProperty m_ReadableSetMode;
        private SerializedProperty m_GenerateMipmapSetMode;
        private SerializedProperty m_SRGBSetMode;
        private SerializedProperty m_FilterModeSetMode;
        private SerializedProperty m_AnisoLevelSetMode;
        private SerializedProperty m_MaxSizeForAndroidSetMode;
        private SerializedProperty m_FormatForAndroidSetMode;
        private SerializedProperty m_CompressionForAndroidSetMode;
        private SerializedProperty m_MaxSizeForIosSetMode;
        private SerializedProperty m_FormatForIosSetMode;
        private SerializedProperty m_CompressionForIosSetMode;

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
            var newValue = SpriteAtlasSetting.GetDefaultSetting();
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
            m_SRGB = parent.FindPropertyRelative("m_SRGB");
            m_Readable = parent.FindPropertyRelative("m_Readable");
            m_GenerateMipmap = parent.FindPropertyRelative("m_GenerateMipmap");
            m_FilterMode = parent.FindPropertyRelative("m_FilterMode");
            m_AnisoLevel = parent.FindPropertyRelative("m_AnisoLevel");
            m_MaxSizeForAndroid = parent.FindPropertyRelative("m_MaxSizeForAndroid");
            m_FormatForAndroid = parent.FindPropertyRelative("m_FormatForAndroid");
            m_CompressionForAndroid = parent.FindPropertyRelative("m_CompressionForAndroid");
            m_MaxSizeForIos = parent.FindPropertyRelative("m_MaxSizeForIos");
            m_FormatForIos = parent.FindPropertyRelative("m_FormatForIos");
            m_CompressionForIos = parent.FindPropertyRelative("m_CompressionForIos");

            m_SRGBSetMode = parent.FindPropertyRelative("m_SRGBSetMode");
            m_ReadableSetMode = parent.FindPropertyRelative("m_ReadableSetMode");
            m_GenerateMipmapSetMode = parent.FindPropertyRelative("m_GenerateMipmapSetMode");
            m_FilterModeSetMode = parent.FindPropertyRelative("m_FilterModeSetMode");
            m_AnisoLevelSetMode = parent.FindPropertyRelative("m_AnisoLevelSetMode");
            m_MaxSizeForAndroidSetMode = parent.FindPropertyRelative("m_MaxSizeForAndroidSetMode");
            m_FormatForAndroidSetMode = parent.FindPropertyRelative("m_FormatForAndroidSetMode");
            m_CompressionForAndroidSetMode = parent.FindPropertyRelative("m_CompressionForAndroidSetMode");
            m_MaxSizeForIosSetMode = parent.FindPropertyRelative("m_MaxSizeForIosSetMode");
            m_FormatForIosSetMode = parent.FindPropertyRelative("m_FormatForIosSetMode");
            m_CompressionForIosSetMode = parent.FindPropertyRelative("m_CompressionForIosSetMode");

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
                    window.AddItem(new GUIContent(displayName == ruleTypeDisplayName ? (displayName + "     √") : displayName), false, (System.Object value) => {
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

            DoTexturePropertyGroupUILayout(m_Readable, Styles.Readable, m_ReadableSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_GenerateMipmap, Styles.GenerateMipmap, m_GenerateMipmapSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_SRGB, Styles.SRGB, m_SRGBSetMode);

            GUILayout.Space(spaceHeight);          

            DoTexturePropertyGroupUILayout(m_FilterMode, Styles.FilterMode, m_FilterModeSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_AnisoLevel, Styles.AnisoLevel, m_AnisoLevelSetMode);

            GUILayout.Space(spaceHeight);

            m_PlatformIndex = GUILayout.Toolbar(m_PlatformIndex, Styles.Platforms);

            EditorGUI.indentLevel++;

            switch (m_PlatformIndex)
            {
                case 0:

                    DoTexturePropertyGroupUILayout(m_MaxSizeForAndroid, Styles.MaxSizeForAndroid, m_MaxSizeForAndroidSetMode);

                    DoTexturePropertyGroupUILayout(m_FormatForAndroid, Styles.FormatForAndroid, m_FormatForAndroidSetMode);

                    DoTexturePropertyGroupUILayout(m_CompressionForAndroid, Styles.CompressionForAndroid, m_CompressionForAndroidSetMode);

                    break;
                case 1:

                    DoTexturePropertyGroupUILayout(m_MaxSizeForIos, Styles.MaxSizeForIos, m_MaxSizeForIosSetMode);

                    DoTexturePropertyGroupUILayout(m_FormatForIos, Styles.FormatForIos, m_FormatForIosSetMode);

                    DoTexturePropertyGroupUILayout(m_CompressionForIos, Styles.CompressionForIos, m_CompressionForIosSetMode);

                    break;
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }

        #endregion
    }
}