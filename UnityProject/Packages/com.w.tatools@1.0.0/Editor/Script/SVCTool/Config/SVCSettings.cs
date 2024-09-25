namespace jj.TATools.Editor
{
    using System.Reflection;
    using System.IO;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [CreateAssetMenu(fileName = TOOL_SETTING_NAME, menuName = TOOL_MENU_1)]
    internal class SVCSettings : ScriptableObject
    {
        #region Fields

        // Unity defined keywords //////////////////////////////////////////
        internal const string UNITY_KW_SHADOWS_DEPTH = "SHADOWS_DEPTH";
        internal const string UNITY_KW_DIRECTIONAL = "DIRECTIONAL";
        internal const string UNITY_KW_LIGHTPROBE_SH = "LIGHTPROBE_SH";
        internal const string UNITY_KW_DIRLIGHTMAP_COMBINED = "DIRLIGHTMAP_COMBINED";
        internal const string UNITY_KW_DYNAMICLIGHTMAP_ON = "DYNAMICLIGHTMAP_ON";
        internal const string UNITY_KW_LIGHTMAP_ON = "LIGHTMAP_ON";
        internal const string UNITY_KW_LIGHTMAP_SHADOW_MIXING = "LIGHTMAP_SHADOW_MIXING";
        internal const string UNITY_KW_SHADOWS_SHADOWMASK = "SHADOWS_SHADOWMASK";
        internal const string UNITY_KW_INSTANCING_ON = "INSTANCING_ON";
        internal const string UNITY_KW_FOG_LINEAR = "FOG_LINEAR";
        internal const string UNITY_KW_FOG_EXP = "FOG_EXP";
        internal const string UNITY_KW_FOG_EXP2 = "FOG_EXP2";

        // URP defined keywords //////////////////////////////////////////
        // Main Light - Shadow:禁用Screen Space Shadow
        internal const string URP_MAIN_LIGHT_SHADOWS = "_MAIN_LIGHT_SHADOWS";
        internal const string URP_MAIN_LIGHT_SHADOWS_CASCADE = "_MAIN_LIGHT_SHADOWS_CASCADE";
        internal const string URP_MAIN_LIGHT_SHADOWS_SCREEN = "_MAIN_LIGHT_SHADOWS_SCREEN";
        // Additional Light
        internal const string URP_ADDITIONAL_LIGHTS = "_ADDITIONAL_LIGHTS";
        internal const string URP_ADDITIONAL_LIGHTS_VERTEX = "_ADDITIONAL_LIGHTS_VERTEX";
        internal const string URP_ADDITIONAL_LIGHT_SHADOWS = "_ADDITIONAL_LIGHT_SHADOWS";
        // SH
        internal const string URP_EVALUATE_SH_MIXED = "EVALUATE_SH_MIXED";
        internal const string URP_EVALUATE_SH_VERTEX = "EVALUATE_SH_VERTEX";
        // Shadow
        internal const string URP_SHADOWS_SOFT = "_SHADOWS_SOFT";
        // SSAO
        internal const string URP_SCREEN_SPACE_OCCLUSION = "_SCREEN_SPACE_OCCLUSION";
        // Decal
        internal const string URP_DBUFFER_MRT1 = "_DBUFFER_MRT1";
        internal const string URP_DBUFFER_MRT2 = "_DBUFFER_MRT2";
        internal const string URP_DBUFFER_MRT3 = "_DBUFFER_MRT3";
        // Reflection Probe
        internal const string URP_REFLECTION_PROBE_BLENDING = "_REFLECTION_PROBE_BLENDING";
        internal const string URP_REFLECTION_PROBE_BOX_PROJECTION = "_REFLECTION_PROBE_BOX_PROJECTION";


        // URP defined keywords //////////////////////////////////////////
        internal const string SVC_EXTENSION = ".shadervariants";
        internal const string TOOL_MENU_1 = "Tools/TATools/SVC/Collect Shader Variants";
        internal const string TOOL_MENU_2 = "Tools/TATools/SVC/Check Shader Standards";
        internal const string TOOL_MENU_3 = "Tools/TATools/SVC/Build Bundles(Debug)";
        const string TOOL_SETTING_NAME = "SVCSettings";

        // Config  //////////////////////////////////////////////////////
        public List<string> m_SearchInFolders = new List<string>() { "Assets" };
        [SerializeField]
        List<string> m_DynamicKeywords = new List<string>();

        [SerializeField]
        List<string> m_CollectedSVCFiles = new List<string>();

        [SerializeField]
        string m_URPAssetParameterConfigAssembly = "jj.Bifrost.Runtime";
        [SerializeField]
        string m_URPAssetParameterConfigTypeName = "jj.Core.Runtime.URPAssetParameterConfig";
        [SerializeField]
        string m_URPAssetParameterConfigUrpAssetsPropName = "allUrpAssets";


        public List<string> DynamicKeywords
        {
            get
            {
                // 校验配置中动态关键字是否重复，剔除重复关键字
                // var dynamicFinalKeywords = m_DynamicKeywords.Distinct().ToList<string>();// m_DynamicKeywords.Where((x, i) => dynamicKeywords.FindIndex(z => z == x) == i).ToList();
                return m_DynamicKeywords;
            }
        }

        public List<ShaderVariantCollection> CollectedSVC 
        {
            get
            {
                if (m_CollectedSVCFiles == null || m_CollectedSVCFiles.Count == 0)
                    return null;

                List<ShaderVariantCollection> svcAssetList = new List<ShaderVariantCollection>();
                foreach (var svcFile in m_CollectedSVCFiles)
                {
                    if (string.IsNullOrEmpty(svcFile)) continue;

                    var svcAsset = AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(svcFile);
                    if (svcAsset != null)
                        svcAssetList.Add(svcAsset);
                }

                return svcAssetList;
            }
        }

        static SVCSettings _instance;
        internal static SVCSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    MonoScript ms = MonoScript.FromScriptableObject(new SVCSettings());
                    string scriptFilePath = AssetDatabase.GetAssetPath(ms);
                    string scriptDirectoryPath = Path.GetDirectoryName(scriptFilePath);
                    string[] findResultGUID = AssetDatabase.FindAssets("t:" + typeof(SVCSettings), new string[] { scriptDirectoryPath });
                    if (findResultGUID.Length == 0)
                    {
                        _instance = ScriptableObject.CreateInstance<SVCSettings>();

                        AssetDatabase.CreateAsset(_instance, scriptDirectoryPath + "\\" + TOOL_SETTING_NAME + ".asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        foreach (var guid in findResultGUID)
                        {
                            _instance = AssetDatabase.LoadAssetAtPath<SVCSettings>(AssetDatabase.GUIDToAssetPath(guid));
                            break;
                        }
                    }
                }

                return _instance;
            }
        }

#endregion

        #region Internal Methods

        internal void ClearSVCFileCache()
        {
            m_CollectedSVCFiles.Clear();
        }

        internal void AddSVCFile(string svcFile)
        {
            if (!m_CollectedSVCFiles.Contains(svcFile))
                m_CollectedSVCFiles.Add(svcFile);
        }

        internal List<UnityEngine.Object> GetUniversalRenderPipelineAssetForModule(string moduleFolder)
        {
            List<UnityEngine.Object> pipelineList = new List<Object>();
            try
            {
                Assembly assembly = Assembly.Load(m_URPAssetParameterConfigAssembly);
                var type = assembly.GetType(m_URPAssetParameterConfigTypeName);
                var guids = AssetDatabase.FindAssets("t:" + type.Name, new string[] { moduleFolder });
                if (guids != null && guids.Length > 0)
                {
                    foreach (var guid in guids)
                    {
                        var pipelineContainerObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(guid));
                        if (pipelineContainerObj != null)
                        {
                            var urpAssetsField = type.GetField(m_URPAssetParameterConfigUrpAssetsPropName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                            if (urpAssetsField != null)
                            {
                                var urpAssetsFieldValue = (UnityEngine.Object[])urpAssetsField.GetValue(pipelineContainerObj);
                                if (urpAssetsFieldValue != null)
                                {
                                    foreach (var pipelineObj in urpAssetsFieldValue)
                                    {
                                        if (!pipelineList.Contains(pipelineObj))
                                            pipelineList.Add(pipelineObj);
                                    }
                                }
                            }
                        }
                    }

                    if (pipelineList.Count == 0)
                    {
                        var defaultPipelineAsset = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline;
                        if (defaultPipelineAsset != null)
                            pipelineList.Add(defaultPipelineAsset);
                    }
                }
                else
                {
                    var defaultPipelineAsset = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline;
                    if (defaultPipelineAsset != null)
                        pipelineList.Add(defaultPipelineAsset);

                }
            }
            catch (System.Exception e)
            {
                var defaultPipelineAsset = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline;
                if (defaultPipelineAsset != null)
                    pipelineList.Add(defaultPipelineAsset);
            } 

            return pipelineList;
        }

        #endregion
    }

    [CustomEditor(typeof(SVCSettings))]
    internal class SVCSettingsInspector : Editor
    {
        #region Fields

        class Styles
        {
            internal static GUIContent SearchInFolders = new GUIContent("检测范围列表", "路径格式:\n 1.以‘/’分割;\n 2.结尾不要有‘/’或 '\\\\'.");
            internal static GUIContent DynamicKeywords = new GUIContent("动态关键字列表", "代码中需要根据相关业务逻辑动态开关的设置的关键字.\n规则:\n1.声明关键字'#pragma multi_compile _ D1'-> 配置'D1';\n2.声明关键字'#pragma multi_compile _ D1 D2 D3'-> 配置'空字符串|D1|D2|D3';\n3.声明关键字'#pragma multi_compile D1 D2 D3'-> 配置'D1|D2|D3';");
            internal static GUIContent CollectedSVCFiles = new GUIContent("已收集SVC路径列表", "该列表每次执行变体收集后自动更新。");

            internal static GUIContent URPAssetParameterConfigFoldout = new GUIContent("URPAssetParameterConfig", "");
            internal static GUIContent URPAssetParameterConfigAssembly = new GUIContent("URPAssetParameterConfig程序集", "");
            internal static GUIContent URPAssetParameterConfigTypeName = new GUIContent("URPAssetParameterConfig类型", "");
            internal static GUIContent URPAssetParameterConfigUrpAssetsPropName = new GUIContent("URPAssetParameterConfig-UrpAssets属性", "");
        }


        private SerializedProperty m_SearchInFolders;
        private SerializedProperty m_DynamicKeywords;
        private SerializedProperty m_CollectedSVCFiles;

        private bool m_URPAssetParameterConfigFoldout = false;
        private SerializedProperty m_URPAssetParameterConfigAssembly;
        private SerializedProperty m_URPAssetParameterConfigTypeName;
        private SerializedProperty m_URPAssetParameterConfigUrpAssetsPropName;

        #endregion

        #region Editor Methods

        private void OnEnable()
        {
            m_SearchInFolders = serializedObject.FindProperty("m_SearchInFolders");
            m_DynamicKeywords = serializedObject.FindProperty("m_DynamicKeywords");
            m_CollectedSVCFiles = serializedObject.FindProperty("m_CollectedSVCFiles");

            m_URPAssetParameterConfigAssembly = serializedObject.FindProperty("m_URPAssetParameterConfigAssembly");
            m_URPAssetParameterConfigTypeName = serializedObject.FindProperty("m_URPAssetParameterConfigTypeName");
            m_URPAssetParameterConfigUrpAssetsPropName = serializedObject.FindProperty("m_URPAssetParameterConfigUrpAssetsPropName");
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            serializedObject.Update();

            // Search Folders
            EditorGUILayout.PropertyField(m_SearchInFolders, Styles.SearchInFolders);

            // Dynamic Keywords
            EditorGUILayout.PropertyField(m_DynamicKeywords, Styles.DynamicKeywords);

            // Collected SVC Files
#if SVC_DEBUG_ON
               
            EditorGUILayout.PropertyField(m_CollectedSVCFiles, Styles.CollectedSVCFiles);
#endif

            m_URPAssetParameterConfigFoldout = EditorGUILayout.Foldout(m_URPAssetParameterConfigFoldout, Styles.URPAssetParameterConfigFoldout);
            if (m_URPAssetParameterConfigFoldout)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(m_URPAssetParameterConfigAssembly, Styles.URPAssetParameterConfigAssembly);
                EditorGUILayout.PropertyField(m_URPAssetParameterConfigTypeName, Styles.URPAssetParameterConfigTypeName);
                EditorGUILayout.PropertyField(m_URPAssetParameterConfigUrpAssetsPropName, Styles.URPAssetParameterConfigUrpAssetsPropName);

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}