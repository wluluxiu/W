namespace jj.TATools.Editor
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;

    internal class DepLossUtility
    {
        #region Fields

        internal const string REPORT_FILE_NAME = "资源依赖引用丢失检测报告";
        internal const string REPORT_FILE_EXTENSION = ".xlsx";

        internal const string REPORT_CONTENT = "资源依赖引用丢失统计";
        internal const string REPORT_CONTENT_TITLE_0 = "类 型";
        internal const string REPORT_CONTENT_TITLE_1 = "数 量";

        internal const string REPORT_RETURN = "返回";
        internal const string REPORT_DETIAL_TITLE_0 = "ID";
        internal const string REPORT_DETIAL_TITLE_1 = "资源路径";
        internal const string REPORT_DETIAL_TITLE_2 = "依赖丢失信息";
        internal const string REPORT_DETIAL_TITLE_3 = "Guid";
        internal const string REPORT_DETIAL_TITLE_4 = "路径";
        internal const string REPORT_DETIAL_TITLE_5 = "节点";
        internal const string REPORT_DETIAL_TITLE_6 = "StateName";
        internal const string REPORT_DETIAL_TITLE_7 = "属性名";

        const string PREFAB_FILE_EXTENSION = ".prefab";

        #endregion

        #region Internal Methods

        internal static bool IsPrefab(string assetPath)
        {
            return assetPath.EndsWith(PREFAB_FILE_EXTENSION);
        }

        internal static void OpenFile(string filePath)
        {
            if (File.Exists(filePath))
                System.Diagnostics.Process.Start(filePath);
        }

        internal static void MessageBox(string title,string message,string ok)
        {
            EditorUtility.DisplayDialog(title, message, ok);
        }

        internal static void FreeMemory()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.UnloadUnusedAssetsImmediate();
            System.GC.Collect();
        }

        #endregion
    }
}