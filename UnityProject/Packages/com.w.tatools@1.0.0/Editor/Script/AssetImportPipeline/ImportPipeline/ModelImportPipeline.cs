namespace jj.TATools.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    [System.Serializable]
    internal class ModelSetting
    {
        #region Fields

        public ESpecialRuleType m_RuleType = ESpecialRuleType._0_Default;
        public List<string> m_AssetParentFolderListForPartFolderRule = new List<string>();
        public List<string> m_AssetParentFolderListForFullFolderRule = new List<string>();
        public List<string> m_AssetNameListForAssetNameRule = new List<string>();
        public List<string> m_AssetPathListForAssetPathRule = new List<string>();

        // Model //////////////////////////
        public PropertySetMode m_BakeAxisConversionSetMode = PropertySetMode.Once;
        public bool m_BakeAxisConversion = false;
        public PropertySetMode m_ImportVisibilitySetMode = PropertySetMode.Once;
        public bool m_ImportVisibility = false;
        public PropertySetMode m_ImportCamerasSetMode = PropertySetMode.Once;
        public bool m_ImportCameras = false;
        public PropertySetMode m_ImportLightsSetMode = PropertySetMode.Once;
        public bool m_ImportLights = false;
        public PropertySetMode m_MeshCompressionSetMode = PropertySetMode.Force;
        public ModelImporterMeshCompression m_MeshCompression = ModelImporterMeshCompression.Off;
        public PropertySetMode m_ReadableSetMode = PropertySetMode.Force;
        public bool m_Readable = false;
        public PropertySetMode m_OptimizeMeshSetMode = PropertySetMode.Once;
        public MeshOptimizationFlags m_OptimizeMesh = MeshOptimizationFlags.Everything;
        public PropertySetMode m_AddColliderSetMode = PropertySetMode.Once;
        public bool m_AddCollider = false;
        public PropertySetMode m_KeepQuadsSetMode = PropertySetMode.Once;
        public bool m_KeepQuads = false;
        public PropertySetMode m_WeldVerticesSetMode = PropertySetMode.Once;
        public bool m_WeldVertices = true;
        public PropertySetMode m_SwapUVChannelsSetMode = PropertySetMode.Force;
        public bool m_SwapUVChannels = false;
        public PropertySetMode m_GenerateSecondaryUVSetMode = PropertySetMode.Force;
        public bool m_GenerateSecondaryUV = false;
        // Rig //////////////////////////////
        // Animation ////////////////////////
        // Material /////////////////////////

        #endregion

        #region Internal Methods

        internal static ModelSetting GetDefaultSetting()
        {
            var defaultSetting = new ModelSetting();

            defaultSetting.m_RuleType = ESpecialRuleType._0_Default;
            defaultSetting.m_AssetParentFolderListForPartFolderRule = new List<string>();
            defaultSetting.m_AssetParentFolderListForFullFolderRule = new List<string>();
            defaultSetting.m_AssetNameListForAssetNameRule = new List<string>();
            defaultSetting.m_AssetPathListForAssetPathRule = new List<string>();

            // Model
            defaultSetting.m_BakeAxisConversionSetMode = PropertySetMode.Once;
            defaultSetting.m_BakeAxisConversion = false;
            defaultSetting.m_ImportVisibilitySetMode = PropertySetMode.Force;
            defaultSetting.m_ImportVisibility = false;
            defaultSetting.m_ImportCamerasSetMode = PropertySetMode.Force;
            defaultSetting.m_ImportCameras = false;
            defaultSetting.m_ImportLightsSetMode = PropertySetMode.Force;
            defaultSetting.m_ImportLights = false;
            defaultSetting.m_MeshCompressionSetMode = PropertySetMode.Force;
            defaultSetting.m_MeshCompression = ModelImporterMeshCompression.Off;
            defaultSetting.m_ReadableSetMode = PropertySetMode.Force;
            defaultSetting.m_Readable = false;
            defaultSetting.m_OptimizeMeshSetMode = PropertySetMode.Once;
            defaultSetting.m_OptimizeMesh = MeshOptimizationFlags.Everything;
            defaultSetting.m_AddColliderSetMode = PropertySetMode.Force;
            defaultSetting.m_AddCollider = false;
            defaultSetting.m_KeepQuadsSetMode = PropertySetMode.Once;
            defaultSetting.m_KeepQuads = false;
            defaultSetting.m_WeldVerticesSetMode = PropertySetMode.Force;
            defaultSetting.m_WeldVertices = true;
            defaultSetting.m_SwapUVChannelsSetMode = PropertySetMode.Force;
            defaultSetting.m_SwapUVChannels = false;
            defaultSetting.m_GenerateSecondaryUVSetMode = PropertySetMode.Force;
            defaultSetting.m_GenerateSecondaryUV = false;
            // Rig
            // Animation
            // Material

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

        internal void DoPreImport(ModelImporter mImporter)
        {
            // First Imported //////////////////
            bool isFirstImported = mImporter.userData != AssetImportPipelineUtility.ASSET_FIRST_IMPORTED;
            if (isFirstImported)
                mImporter.userData = AssetImportPipelineUtility.ASSET_FIRST_IMPORTED;

            // Model  //////////////////////////
            // Bake Axis Conversion
            if (m_BakeAxisConversionSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.bakeAxisConversion = m_BakeAxisConversion;
            }
            else if(m_BakeAxisConversionSetMode == PropertySetMode.Force)
                mImporter.bakeAxisConversion = m_BakeAxisConversion;
            // Import Visibility
            if (m_ImportVisibilitySetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.importVisibility = m_ImportVisibility;
            }
            else if(m_ImportVisibilitySetMode == PropertySetMode.Force)
                mImporter.importVisibility = m_ImportVisibility;
            // Import Cameras
            if (m_ImportCamerasSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.importCameras = m_ImportCameras;
            }
            else if (m_ImportCamerasSetMode == PropertySetMode.Force)
                mImporter.importCameras = m_ImportCameras;
            // Import Lights
            if (m_ImportLightsSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.importLights = m_ImportLights;
            }
            else if (m_ImportLightsSetMode == PropertySetMode.Force)
                mImporter.importLights = m_ImportLights;
            // Mesh Compression
            if (m_MeshCompressionSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.meshCompression = m_MeshCompression;
            }
            else if (m_MeshCompressionSetMode == PropertySetMode.Force)
                mImporter.meshCompression = m_MeshCompression;
            // Read/Write
            if (m_ReadableSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.isReadable = m_Readable;
            }
            else if (m_ReadableSetMode == PropertySetMode.Force)
                mImporter.isReadable = m_Readable;
            // Optimize Mesh
            if (m_OptimizeMeshSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.meshOptimizationFlags = m_OptimizeMesh;
            }
            else if (m_OptimizeMeshSetMode == PropertySetMode.Force)
                mImporter.meshOptimizationFlags = m_OptimizeMesh;
            // Generate Colliders
            if (m_AddColliderSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.addCollider = m_AddCollider;
            }
            else if (m_AddColliderSetMode == PropertySetMode.Force)
                mImporter.addCollider = m_AddCollider;
            // Keep Quads
            if (m_KeepQuadsSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.keepQuads = m_KeepQuads;
            }
            else if (m_KeepQuadsSetMode == PropertySetMode.Force)
                mImporter.keepQuads = m_KeepQuads;
            // Weld Vertices
            if (m_WeldVerticesSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.weldVertices = m_WeldVertices;
            }
            else if (m_WeldVerticesSetMode == PropertySetMode.Force)
                mImporter.weldVertices = m_WeldVertices;
            // Swap UVs
            if (m_SwapUVChannelsSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.swapUVChannels = m_SwapUVChannels;
            }
            else if (m_SwapUVChannelsSetMode == PropertySetMode.Force)
                mImporter.swapUVChannels = m_SwapUVChannels;
            // Generate Secondary UV
            if (m_GenerateSecondaryUVSetMode == PropertySetMode.Once)
            {
                if (isFirstImported) mImporter.generateSecondaryUV = m_GenerateSecondaryUV;
            }
            else if (m_GenerateSecondaryUVSetMode == PropertySetMode.Force)
                mImporter.generateSecondaryUV = m_GenerateSecondaryUV;
            // Rig //////////////////////////////
            // Animation ////////////////////////
            // Material /////////////////////////
        }

        #endregion
    }

    //[CreateAssetMenu(menuName = AssetImportPipelineUtility.TOOL_MENU_MODEL_SETTING, fileName = "ModelImportPipeline")]
    internal class ModelImportPipeline : ScriptableObject
    {
        #region Fields

        [SerializeField]
        List<ModelSetting> m_SpecialSettings;

        List<ModelSetting> m_SortedSpecialSettings;

        #endregion

        #region Internal Methods

        internal void AutoSortSettings()
        {
            m_SortedSpecialSettings = new List<ModelSetting>(m_SpecialSettings);
            m_SortedSpecialSettings.Sort((x, y) => string.CompareOrdinal(((int)x.m_RuleType).ToString(), ((int)y.m_RuleType).ToString()));
        }

        internal void DoPreImport(ModelImporter mImporter)
        {
            if (m_SortedSpecialSettings != null && m_SortedSpecialSettings.Count > 0)
            {
                foreach (var setting in m_SortedSpecialSettings)
                {
                    if (setting.CheckRule(mImporter.assetPath))
                        setting.DoPreImport(mImporter);
                }
            }
        }

        #endregion
    }

    [CustomEditor(typeof(ModelImportPipeline))]
    internal class ModelImportPipelineInspector : Editor
    {
        #region Fields

        class Styles
        {
            #region Menu //////////////////////////////////
            internal static readonly GUIContent[] ModelMenus = new GUIContent[] {
                new GUIContent("Model",""),
                new GUIContent("Rig",""),
                new GUIContent("Animation",""),
                new GUIContent("Materials",""),
            };

            internal static readonly GUIContent SetMode = new GUIContent("", "Once-资源首次导入工程修改，Force资源每次重新导入时修改。");
            internal static readonly GUIContent SpecialSettings = new GUIContent("自定义配置:");
            internal static readonly GUIContent RuleType = new GUIContent("规则类型", "1.None:通用规则;\n2.AssetPath:资源路径;\n3.AssetName:资源名;\n4.AssetParentFullFolder:全目录;\n5.AssetParentPartFolder:部分目录;");
            internal static readonly GUIContent AssetPath = new GUIContent("资源路径", "格式：Assets/XXX/YYY/test.png");
            internal static readonly GUIContent AssetName = new GUIContent("资源名", "格式: _ASTC_4x4 或 _RW");
            internal static readonly GUIContent AssetParentFullFolder = new GUIContent("全目录", "格式：Assets/XXX/YYY/");
            internal static readonly GUIContent AssetParentPartFolder = new GUIContent("部分目录", "格式：/XXX/YYY/ ");

            #endregion

            #region Model /////////////////////////////////

            internal static readonly GUIContent BakeAxisConversion = new GUIContent("Bake Axis Conversion", "Perform axis conversion on all content for models defined in an axis system that differs from Unity's (left handed, Z forward, Y-up).");
            internal static readonly GUIContent ImportVisibility = new GUIContent("Import Visibility", "Use visibility properties to enable or disable MeshRenderer components.");
            internal static readonly GUIContent ImportCameras = new GUIContent("Import Cameras", "");
            internal static readonly GUIContent ImportLights = new GUIContent("Import Lights", "");
            internal static readonly GUIContent MeshCompression = new GUIContent("Mesh Compression", "Higher compression ratio means lower mesh precision. If enabled, the mesh bounds and a lower bit depth per component are used to compress the mesh data.");
            internal static readonly GUIContent Readable = new GUIContent("Read/Write", "Allow vertices and indices to be accessed from script.");
            internal static readonly GUIContent OptimizeMesh = new GUIContent("Optimize Mesh", "Reorder vertices and/or polygons for better GPU performance.");
            internal static readonly GUIContent AddCollider = new GUIContent("Generate Colliders", "Should Unity generate mesh colliders for all meshes.");
            internal static readonly GUIContent KeepQuads = new GUIContent("Keep Quads", "If model contains quad faces, they are kept for DX11 tessellation.");
            internal static readonly GUIContent WeldVertices = new GUIContent("Weld Vertices", "Combine vertices that share the same position in space.");
            internal static readonly GUIContent SwapUVChannels = new GUIContent("Swap UVs", "Swaps the 2 UV channels in meshes. Use if your diffuse texture uses UVs from the lightmap.");
            internal static readonly GUIContent GenerateSecondaryUV = new GUIContent("Generate Lightmap UVs", "Generate lightmap UVs into UV2.");

            #endregion

            #region Rig ///////////////////////////////////

            #endregion

            #region Animation /////////////////////////////

            #endregion

            #region Materials /////////////////////////////

            #endregion
        }

        // Base Property
        private SerializedProperty m_SpecialSettings;

        // Children Property
        private SerializedProperty m_RuleType;
        private SerializedProperty m_AssetParentFolderListForPartFolderRule;
        private SerializedProperty m_AssetParentFolderListForFullFolderRule;
        private SerializedProperty m_AssetNameListForAssetNameRule;
        private SerializedProperty m_AssetPathListForAssetPathRule;

        #region Children Property-Model /////////////////////////////////

        private SerializedProperty m_BakeAxisConversion;
        private SerializedProperty m_ImportVisibility;
        private SerializedProperty m_ImportCameras;
        private SerializedProperty m_ImportLights;
        private SerializedProperty m_MeshCompression;
        private SerializedProperty m_Readable;
        private SerializedProperty m_OptimizeMesh;
        private SerializedProperty m_AddCollider;
        private SerializedProperty m_KeepQuads;
        private SerializedProperty m_WeldVertices;
        private SerializedProperty m_SwapUVChannels;
        private SerializedProperty m_GenerateSecondaryUV;

        private SerializedProperty m_BakeAxisConversionSetMode;
        private SerializedProperty m_ImportVisibilitySetMode;
        private SerializedProperty m_ImportCamerasSetMode;
        private SerializedProperty m_ImportLightsSetMode;
        private SerializedProperty m_MeshCompressionSetMode;
        private SerializedProperty m_ReadableSetMode;
        private SerializedProperty m_OptimizeMeshSetMode;
        private SerializedProperty m_AddColliderSetMode;
        private SerializedProperty m_KeepQuadsSetMode;
        private SerializedProperty m_WeldVerticesSetMode;
        private SerializedProperty m_SwapUVChannelsSetMode;
        private SerializedProperty m_GenerateSecondaryUVSetMode;

        #endregion

        #region Children Property-Rig ///////////////////////////////////

        #endregion

        #region Children Property-Animation /////////////////////////////

        #endregion

        #region Children Property-Materials /////////////////////////////

        #endregion

        private int m_MenuIndex = 0;

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
                    DoModelSettingUILayout(setting);
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
            var newValue = ModelSetting.GetDefaultSetting();
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

        void DoModelPropertyGroupUILayout(SerializedProperty data, GUIContent dataGUIContent, SerializedProperty setMode)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(data, dataGUIContent);

            EditorGUILayout.PropertyField(setMode, Styles.SetMode, GUILayout.Width(100));

            EditorGUILayout.EndHorizontal();
        }

        // Model UI
        void DoModelUILayout(SerializedProperty parent)
        {
            m_BakeAxisConversion = parent.FindPropertyRelative("m_BakeAxisConversion");
            m_ImportVisibility = parent.FindPropertyRelative("m_ImportVisibility");
            m_ImportCameras = parent.FindPropertyRelative("m_ImportCameras");
            m_ImportLights = parent.FindPropertyRelative("m_ImportLights");
            m_MeshCompression = parent.FindPropertyRelative("m_MeshCompression");
            m_Readable = parent.FindPropertyRelative("m_Readable");
            m_OptimizeMesh = parent.FindPropertyRelative("m_OptimizeMesh");
            m_AddCollider = parent.FindPropertyRelative("m_AddCollider");
            m_KeepQuads = parent.FindPropertyRelative("m_KeepQuads");
            m_WeldVertices = parent.FindPropertyRelative("m_WeldVertices");
            m_SwapUVChannels = parent.FindPropertyRelative("m_SwapUVChannels");
            m_GenerateSecondaryUV = parent.FindPropertyRelative("m_GenerateSecondaryUV");

            m_BakeAxisConversionSetMode = parent.FindPropertyRelative("m_BakeAxisConversionSetMode");
            m_ImportVisibilitySetMode = parent.FindPropertyRelative("m_ImportVisibilitySetMode");
            m_ImportCamerasSetMode = parent.FindPropertyRelative("m_ImportCamerasSetMode");
            m_ImportLightsSetMode = parent.FindPropertyRelative("m_ImportLightsSetMode");
            m_MeshCompressionSetMode = parent.FindPropertyRelative("m_MeshCompressionSetMode");
            m_ReadableSetMode = parent.FindPropertyRelative("m_ReadableSetMode");
            m_OptimizeMeshSetMode = parent.FindPropertyRelative("m_OptimizeMeshSetMode");
            m_AddColliderSetMode = parent.FindPropertyRelative("m_AddColliderSetMode");
            m_KeepQuadsSetMode = parent.FindPropertyRelative("m_KeepQuadsSetMode");
            m_WeldVerticesSetMode = parent.FindPropertyRelative("m_WeldVerticesSetMode");
            m_SwapUVChannelsSetMode = parent.FindPropertyRelative("m_SwapUVChannelsSetMode");
            m_GenerateSecondaryUVSetMode = parent.FindPropertyRelative("m_GenerateSecondaryUVSetMode");

            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical("Box");

            float spaceHeight = 4;

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_BakeAxisConversion, Styles.BakeAxisConversion, m_BakeAxisConversionSetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_ImportVisibility, Styles.ImportVisibility, m_ImportVisibilitySetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_ImportCameras, Styles.ImportCameras, m_ImportCamerasSetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_ImportLights, Styles.ImportLights, m_ImportLightsSetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_MeshCompression, Styles.MeshCompression, m_MeshCompressionSetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_Readable, Styles.Readable, m_ReadableSetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_OptimizeMesh, Styles.OptimizeMesh, m_OptimizeMeshSetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_AddCollider, Styles.AddCollider, m_AddColliderSetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_KeepQuads, Styles.KeepQuads, m_KeepQuadsSetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_WeldVertices, Styles.WeldVertices, m_WeldVerticesSetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_SwapUVChannels, Styles.SwapUVChannels, m_SwapUVChannelsSetMode);

            GUILayout.Space(spaceHeight);

            DoModelPropertyGroupUILayout(m_GenerateSecondaryUV, Styles.GenerateSecondaryUV, m_GenerateSecondaryUVSetMode);

            GUILayout.Space(spaceHeight);

            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }

        // Rig UI
        void DoRigUILayout(SerializedProperty parent)
        {

        }

        // Animation UI
        void DoAnimationUILayout(SerializedProperty parent)
        {

        }

        // Materials UI
        void DoMaterialsUILayout(SerializedProperty parent)
        {

        }

        void DoModelSettingUILayout(SerializedProperty parent)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical("Box");

            m_RuleType = parent.FindPropertyRelative("m_RuleType");
            m_AssetParentFolderListForPartFolderRule = parent.FindPropertyRelative("m_AssetParentFolderListForPartFolderRule");
            m_AssetParentFolderListForFullFolderRule = parent.FindPropertyRelative("m_AssetParentFolderListForFullFolderRule");
            m_AssetNameListForAssetNameRule = parent.FindPropertyRelative("m_AssetNameListForAssetNameRule");
            m_AssetPathListForAssetPathRule = parent.FindPropertyRelative("m_AssetPathListForAssetPathRule");

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

            m_MenuIndex = GUILayout.Toolbar(m_MenuIndex, Styles.ModelMenus);
            switch (m_MenuIndex)
            {
                // Model
                case 0:
                    DoModelUILayout(parent);
                    break;
                // Rig
                case 1:
                    DoRigUILayout(parent);
                    break;
                // Animation
                case 2:
                    DoAnimationUILayout(parent);
                    break;
                // Materials
                case 3:
                    DoMaterialsUILayout(parent);
                    break;
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}

