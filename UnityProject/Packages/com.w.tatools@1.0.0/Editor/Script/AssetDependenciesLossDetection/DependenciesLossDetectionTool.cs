namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;

    internal class DependenciesLossDetectionTool
    {
        [MenuItem(DependenciesLossDetectionSettings.TOOL_MENU_BASE + DependenciesLossDetectionSettings.TOOL_MENU_DETECTION)]
        static void DoReferenceLossDetection()
        {
            var guids = AssetDatabase.FindAssets("", DependenciesLossDetectionSettings.Instance.m_SearchFolders.ToArray());
            if (guids == null || guids.Length == 0) return;

            // All Assets Detection ////////////////////////////////////////////////////////
            Dictionary<EDetectionType, List<BaseDetection>> detectionsMapping = new Dictionary<EDetectionType, List<BaseDetection>>();
            List<BaseDetection> tempList = null;
            List<string> pathList = new List<string>();
            int len = guids.Length;
            for (int i = 0; i < len; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                if (DependenciesLossDetectionSettings.Instance.IgnoreFolderAsset(assetPath)) continue;

                bool result = EditorUtility.DisplayCancelableProgressBar("Dependences Loss Detection[" + i + "|" + len + "]", "Detect..." + assetPath, (i + 1) * 1.0f / len);
                if (result)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }

                var assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                if (DependenciesLossDetectionSettings.Instance.IsIgnoreType(assetType)) continue;
                if (pathList.Contains(assetPath)) continue;
                pathList.Add(assetPath);

                var baseDetection = RegisterAssetDetection(assetPath, assetType);
                if (baseDetection != null && baseDetection.IsInvalid())
                {
                    if (!detectionsMapping.TryGetValue(baseDetection.m_Type, out tempList))
                    {
                        tempList = new List<BaseDetection>();
                        detectionsMapping[baseDetection.m_Type] = tempList;
                    }
                    tempList.Add(baseDetection);
                }
              
            }
            EditorUtility.ClearProgressBar();

            // Genereate Detection Report ////////////////////////////////////////////////////////
            if (detectionsMapping.Count > 0)
            {
                var now = System.DateTime.Now;
                var fileName = string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}{7}", DepLossUtility.REPORT_FILE_NAME,
                    now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DepLossUtility.REPORT_FILE_EXTENSION);

                var fileFullPath = DependenciesLossDetectionSettings.Instance.GetReportOutPutFolder() + "\\" + fileName;
                if (DependenciesLossReporter.GenerateReport(detectionsMapping,fileFullPath))
                {
                    DepLossUtility.OpenFile(fileFullPath);
                }

            }
            else
            {
                DepLossUtility.MessageBox(DepLossUtility.REPORT_FILE_NAME, "未检测到丢失数据!!!", "OK");
            }
            
            DepLossUtility.FreeMemory();
        }

        static BaseDetection RegisterAssetDetection(string assetPath, System.Type type)
        {
            EDetectionType assetType = EDetectionType.Normal;
            if (DepLossUtility.IsPrefab(assetPath)) assetType = EDetectionType.Prefab;
            else
            {
                if (type == typeof(UnityEditor.SceneAsset)) assetType = EDetectionType.Scene;
                else if (type == typeof(UnityEditor.MonoScript)) assetType = EDetectionType.Script;
                else if (type == typeof(UnityEditorInternal.AssemblyDefinitionAsset)) assetType = EDetectionType.Asmdef;
                else if (type == typeof(UnityEditor.MonoScript)) assetType = EDetectionType.Script;
                else if (type == typeof(UnityEditor.Animations.AnimatorController)) assetType = EDetectionType.AnimatorController;
                else if (type == typeof(UnityEngine.AnimatorOverrideController)) assetType = EDetectionType.AnimatorOverrideController;
                else if (type == typeof(UnityEngine.Material)) assetType = EDetectionType.Material;
                else if (type == typeof(UnityEngine.Shader)) assetType = EDetectionType.Shader;
                else if (type == typeof(UnityEngine.U2D.SpriteAtlas)) assetType = EDetectionType.SpriteAtlas;
                else if (type == typeof(TMPro.TMP_FontAsset)) assetType = EDetectionType.TMP_SDF;
                else if (type == typeof(TMPro.TMP_SpriteAsset)) assetType = EDetectionType.TMP_Sprite;
                else if (type == typeof(TMPro.TMP_Settings)) assetType = EDetectionType.TMP_Setting;
                else assetType = EDetectionType.Normal;
            }

            BaseDetection baseDetection = BaseDetection.CreateDetection(assetType);
            baseDetection.DoDetection(assetPath);

            return baseDetection;
        }
    }
}