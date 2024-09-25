namespace jj.TATools.Editor
{
    using UnityEngine;
    using UnityEditor;

    internal class GuidConversionWindow : EditorWindow
    {
        #region Fields

        private string m_GuidStr = "";
        private string m_AssetPath = "";

        #endregion

        [MenuItem(DependenciesLossDetectionSettings.TOOL_MENU_BASE + DependenciesLossDetectionSettings.TOOL_MENU_WINDOW)]
        static void OpenWindow()
        {
            GuidConversionWindow window = EditorWindow.GetWindow<GuidConversionWindow>(DependenciesLossDetectionSettings.TOOL_MENU_WINDOW);
            window.Show();
        }

        #region EditorWindow Methods

        void OnEnable()
        {
            m_GuidStr = "";
            m_AssetPath = "";
        }

        void OnGUI()
        {
            if (EditorApplication.isCompiling) this.ShowNotification(new GUIContent("Code is compiling ..."));
            else UILayout();
        }

        #endregion

        #region Local Methods

        private void UILayout()
        {
            EditorGUILayout.BeginVertical("Box");

            m_GuidStr = EditorGUILayout.TextField("GUID:", m_GuidStr);

            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(m_GuidStr));

            if (GUILayout.Button("Get AssetPath From GUID"))
            {
                m_AssetPath = AssetDatabase.GUIDToAssetPath(m_GuidStr);
            }

            m_AssetPath = EditorGUILayout.TextField("AssetPath:", m_AssetPath);

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}