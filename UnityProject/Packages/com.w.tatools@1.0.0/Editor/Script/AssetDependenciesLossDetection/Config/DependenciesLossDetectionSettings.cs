namespace jj.TATools.Editor
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

  //  [CreateAssetMenu(fileName = DependenciesLossDetectionSettings.TOOL_SETTING_NAME, menuName = DependenciesLossDetectionSettings.TOOL_MENU_BASE + DependenciesLossDetectionSettings.TOOL_MENU_SETTING)]
    internal class DependenciesLossDetectionSettings : ScriptableObject
    {
        #region Fields

        internal const string TOOL_MENU_BASE = "Tools/TATools/ADLD/";
        internal const string TOOL_MENU_DETECTION = "Detection";
        internal const string TOOL_MENU_WINDOW = "Guid Conversion";
        internal const string TOOL_MENU_SETTING = "Settings";

        internal const string TOOL_SETTING_NAME = "DependenciesLossDetectionSettings";

        public string m_ReportOutputRelativeFolder = "./JJTATools/ADLD";
        public List<string> m_SearchFolders = new List<string>() { "Assets" };
        public List<string> m_IgnoreFolders = new List<string>() { "Editor" };
        public List<string> m_IgnoreTypeNames = new List<string>() { 
            "UnityEngine.Texture2D",
            "UnityEngine.Cubemap",
            "UnityEngine.RenderTexture",
            "UnityEngine.AnimationClip",
            "UnityEditor.DefaultAsset",
            "UnityEngine.TextAsset",
            "UnityEngine.Mesh",
            "TMPro.TMP_StyleSheet",
            "DG.Tweening.Core.DOTweenSettings",
            "UnityEditor.ShaderInclude",
            "UnityEngine.AudioClip",
            "UnityEngine.Video.VideoClip",
            "UnityEngine.AssetGraph.AssetReferenceDatabase",
            "UnityEngine.AssetGraph.BatchBuildConfig",
            "UnityEngine.AssetGraph.DataModel.Version2.ConfigGraph",
            "AssetBridgeScript",
            "jj.Core.Runtime.MockConfig",};
        public List<string> m_IgnoreTypeAssemblyNames = new List<string>() {
            "UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "UnityEngine.AnimationModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "Unity.TextMeshPro, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "DOTween, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            "UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "UnityEngine.VideoModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "Unity.AssetGraph.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "Unity.AssetGraph.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "Unity.AssetGraph.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "jj.Core.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "jj.Core.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",};
        public List<string> m_BuiltinAssetPaths = new List<string>() { "Library/unity default resources", "Resources/unity_builtin_extra" };

        private List<Type> m_IgnoreTypes = new List<Type>();


        static DependenciesLossDetectionSettings _instance;
        internal static DependenciesLossDetectionSettings Instance
        {
            get 
            {
                if (_instance == null)
                {
                    MonoScript ms = MonoScript.FromScriptableObject(new DependenciesLossDetectionSettings());
                    string scriptFilePath = AssetDatabase.GetAssetPath(ms);
                    string scriptDirectoryPath = Path.GetDirectoryName(scriptFilePath);
                    string[] findResultGUID = AssetDatabase.FindAssets("t:" + typeof(DependenciesLossDetectionSettings), new string[] { scriptDirectoryPath });
                    if (findResultGUID.Length == 0)
                    {
                        _instance = ScriptableObject.CreateInstance<DependenciesLossDetectionSettings>();

                        AssetDatabase.CreateAsset(_instance, scriptDirectoryPath + "\\" + TOOL_SETTING_NAME + ".asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        foreach (var guid in findResultGUID)
                        {
                            _instance = AssetDatabase.LoadAssetAtPath<DependenciesLossDetectionSettings>(AssetDatabase.GUIDToAssetPath(guid));
                            break;
                        }
                    }

                    _instance.m_IgnoreTypes.Clear();
                    for (int i = 0; i < _instance.m_IgnoreTypeNames.Count; i++)
                    {
                        var typeName = _instance.m_IgnoreTypeNames[i];
                        var assemblyName = _instance.m_IgnoreTypeAssemblyNames[i];
                        if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(assemblyName)) continue;

                        System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(assemblyName);
                        if (assembly != null)
                        {
                            Type type = assembly.GetType(typeName);
                            if (type != null)
                                _instance.m_IgnoreTypes.Add(type);
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region Internal Methods

        internal bool IgnoreFolderAsset(string assetPath)
        {
            foreach (var folder in m_IgnoreFolders)
            {
                if (string.IsNullOrEmpty(folder)) continue;

                var ignoreFolder = "/" + folder + "/";
                if (assetPath.Contains(ignoreFolder))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool IsUnityBuiltinAsset(string assetPath)
        {
            foreach (var builtinPath in m_BuiltinAssetPaths)
            {
                if (string.IsNullOrEmpty(builtinPath)) continue;

                if (assetPath == builtinPath)
                {
                    return true;
                }
            }

            return false;
        }

        internal bool IsIgnoreType(Type targetType)
        {
            return m_IgnoreTypes.Contains(targetType);
        }

        internal string GetReportOutPutFolder()
        {
            DirectoryInfo dInfo = new DirectoryInfo(m_ReportOutputRelativeFolder);
            if (!dInfo.Exists) dInfo.Create();

            return dInfo.FullName;
        }

        #endregion

    }

    [CustomEditor(typeof(DependenciesLossDetectionSettings))]
    internal class DependenciesLossDetectionSettingsInspector : Editor
    {
        #region Fields

        class Styles
        {
            internal static GUIContent ReportOutputRelativeFolder = new GUIContent("报告输出相对路径","路径格式:‘./AAA/BBB’,则生成在Assets同级目录下/AAA/BBB/目录.");
            internal static GUIContent SearchFolders = new GUIContent("检测范围列表", "路径格式:\n 1.以‘/’分割;\n 2.结尾不要有‘/’或 '\\\\'.");
            internal static GUIContent IgnoreFolders = new GUIContent("跳过检测目录列表", "如果资源路径中包含列表中配置的文件夹目录则该资源跳过检测.");
            internal static GUIContent IgnoreTypes = new GUIContent("跳过检测类型列表", "跳过配置列表中类型资源检测，提高工具执行效率.");
            internal static GUIContent BuiltinAssetPaths = new GUIContent("Unity内置资源路径列表", "Unity引擎内置资源依赖引用跳过检测.");
        }

        private SerializedProperty m_ReportOutputRelativeFolder;
        private SerializedProperty m_SearchFolders;
        private SerializedProperty m_IgnoreFolders;
        private SerializedProperty m_IgnoreTypeNames;
        private SerializedProperty m_IgnoreTypeAssemblyNames;
        private SerializedProperty m_BuiltinAssetPaths;

        private bool m_IgnoreTypesFoldout = false;

        #endregion

        #region Editor Methods

        private void OnEnable()
        {
            m_ReportOutputRelativeFolder = serializedObject.FindProperty("m_ReportOutputRelativeFolder");
            m_SearchFolders = serializedObject.FindProperty("m_SearchFolders");
            m_IgnoreFolders = serializedObject.FindProperty("m_IgnoreFolders");
            m_IgnoreTypeNames = serializedObject.FindProperty("m_IgnoreTypeNames");
            m_IgnoreTypeAssemblyNames = serializedObject.FindProperty("m_IgnoreTypeAssemblyNames");
            m_BuiltinAssetPaths = serializedObject.FindProperty("m_BuiltinAssetPaths");
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            serializedObject.Update();

            // Report Output Folder
            EditorGUILayout.PropertyField(m_ReportOutputRelativeFolder, Styles.ReportOutputRelativeFolder);

            // Search Folders
            EditorGUILayout.PropertyField(m_SearchFolders,Styles.SearchFolders);

            // Ignore Folders
            EditorGUILayout.PropertyField(m_IgnoreFolders, Styles.IgnoreFolders);

            // Ignore Types
            m_IgnoreTypesFoldout = EditorGUILayout.Foldout(m_IgnoreTypesFoldout, Styles.IgnoreTypes);
            if (m_IgnoreTypesFoldout)
            {
                EditorGUILayout.BeginVertical("Box");

                int deleteIndex = -1;
                for (int i = 0; i < m_IgnoreTypeNames.arraySize; i++)
                {
                    var typeElement = m_IgnoreTypeNames.GetArrayElementAtIndex(i);
                    var assemblyElement = m_IgnoreTypeAssemblyNames.GetArrayElementAtIndex(i);

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("-", GUILayout.Width(25)))
                    {
                        deleteIndex = i;
                    }

                    typeElement.stringValue = EditorGUILayout.TextField("", typeElement.stringValue);

                    assemblyElement.stringValue = EditorGUILayout.TextField("", assemblyElement.stringValue);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("+", GUILayout.Width(25)))
                {
                    m_IgnoreTypeNames.arraySize++;
                    m_IgnoreTypeAssemblyNames.arraySize++;
                }

                GUILayout.FlexibleSpace();

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                if (deleteIndex >= 0 && deleteIndex < m_IgnoreTypeNames.arraySize)
                {
                    m_IgnoreTypeNames.DeleteArrayElementAtIndex(deleteIndex);
                    m_IgnoreTypeAssemblyNames.DeleteArrayElementAtIndex(deleteIndex);
                }
            }

            // Unity Builtin AssetPath
            EditorGUILayout.PropertyField(m_BuiltinAssetPaths, Styles.BuiltinAssetPaths);


            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}