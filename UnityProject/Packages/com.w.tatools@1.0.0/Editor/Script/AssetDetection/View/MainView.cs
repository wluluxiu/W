using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    public partial class MainView : EditorWindow
    {
        #region Editor Window Methods

        [MenuItem("Tools/TATools/Asset Detection")]
        static void OpenWindow()
        {
            MainView mainView = EditorWindow.GetWindow<MainView>("Asset Detection Tools(" + ConstDefine.APP_VERSION + ")");
            mainView.Show();
        }

        private void OnEnable()
        {
            m_ToolConfigReady = AppConfigHelper.CreateFolders();
            if (m_ToolConfigReady)
                InitializeTools();
        }

        private void OnDisable()
        {
            if (m_ToolConfigReady)
                SaveConfigToLocalEditorPrefs();
        }

        private void OnGUI()
        {
            if (!m_ToolConfigReady) return;

            if (EditorApplication.isCompiling) this.ShowNotification(new GUIContent("Code is compiling !!!"));
            else UILayout();
        }

        #endregion

        #region Asset Compare Tool

        private void GenerateWhiteListBtnCallback()
        {
            AppConfigHelper.CreateWhileListConfigFile(true);
        }

        private void GenerateCustomFolderBtnCallback()
        {
            AppConfigHelper.CreateCustomRuleConfigFile(true);
        }

        //private void SelectLastVersionAssetFileBtnCallback()
        //{
        //    string selectedFilePath = AssetDetectionUtility.OpenFilePanel(ConstDefine.SELECT_FILE_TITLE_0, ConstDefine.SELECT_FILE_FILTER, AppConfigHelper.ASSET_COMPARE_SOURCE_FLODER);
        //    this.m_SelectLastVersionAssetFileTextBox = string.IsNullOrEmpty(selectedFilePath) ? "" : selectedFilePath;
        //}

        private void SaveBuildAssetFileConfig()
        {
            //if (!string.IsNullOrEmpty(this.m_SelectLastVersionAssetFileTextBox))
            //    AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_LAST_VERSION_ASSET_FILE, this.m_SelectLastVersionAssetFileTextBox);

            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_FULL_REPORTER_CHECKBOX, this.m_GenerateCurrentFullReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_NEWER_REPORTER_CHECKBOX, this.m_GenerateCurrentNewerReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_DELETED_REPORTER_CHECKBOX, this.m_GenerateCurrentDeletedReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_MODIFIED_REPORTER_CHECKBOX, this.m_GenerateCurrentModifiedReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_REPEATED_REPORTER_CHECKBOX, this.m_GenerateCurrentRepeatedReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_NOUSED_REPORTER_CHECKBOX, this.m_GenerateCurrentNoUsedReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_TEXTURE_REPORTER_CHECKBOX, this.m_GenerateCurrentTextureExceptionReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_MODEL_REPORTER_CHECKBOX, this.m_GenerateCurrentModelExceptionReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_MATERIAL_REPORTER_CHECKBOX, this.m_GenerateCurrentMaterialExceptionReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_SHADER_REPORTER_CHECKBOX, this.m_GenerateCurrentShaderExceptionReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_SCRIPT_REPORTER_CHECKBOX, this.m_GenerateCurrentScriptExceptionReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_DEP_BUILTIN_REPORTER_CHECKBOX, this.m_GenerateCurrentDepBuiltinReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_PREFAB_REPORTER_CHECKBOX, this.m_GenerateCurrentPrefabExceptionReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_SCENE_REPORTER_CHECKBOX, this.m_GenerateCurrentSceneExceptionReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_AB_STRATEGY_REPORTER_CHECKBOX, this.m_GenerateCurrentABStrategyReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_AB_BUILTIN_REPORTER_CHECKBOX, this.m_GenerateCurrentABContainBuiltinReporterCheckBox ? "1" : "0");
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_GENERATE_RESOURCES_FOLDER_REPORTER_CHECKBOX, this.m_GenerateCurrentResorucesFolderReporterCheckBox ? "1" : "0");
        }


        private string GenerateAssetReportForModule(string moduleFolder,string tick)
        {
            var moduleReportFilePath = "";

            var currentVersionAssetFilePath = AssetsRecorderTool.DoAssetsRecordData(moduleFolder,tick);
            if (!string.IsNullOrEmpty(currentVersionAssetFilePath))
            {
                moduleReportFilePath = AssetCompareReportManager.GenerateAssetCompareReport(currentVersionAssetFilePath, moduleFolder, tick);
            }
            else 
            {
                Debug.LogError(string.Format("资源检测报告[{0}]:当前版本资源数据路径为空!!!", moduleFolder));
            }

            return moduleReportFilePath;
        }

        private void GenerateAssetReportBtnCallback()
        {
            AssetRecordDataParser.GenerateCurrentFullReporterCheckBox = m_GenerateCurrentFullReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentNewerReporterCheckBox = m_GenerateCurrentNewerReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentDeletedReporterCheckBox = m_GenerateCurrentDeletedReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentModifiedReporterCheckBox = m_GenerateCurrentModifiedReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentRepeatedReporterCheckBox = m_GenerateCurrentRepeatedReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentNoUsedReporterCheckBox = m_GenerateCurrentNoUsedReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentTextureExceptionReporterCheckBox = m_GenerateCurrentTextureExceptionReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentModelExceptionReporterCheckBox = m_GenerateCurrentModelExceptionReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentMaterialExceptionReporterCheckBox = m_GenerateCurrentMaterialExceptionReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentShaderExceptionReporterCheckBox = m_GenerateCurrentShaderExceptionReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentScriptExceptionReporterCheckBox = m_GenerateCurrentScriptExceptionReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentDepBuiltinReporterCheckBox = m_GenerateCurrentDepBuiltinReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentPrefabExceptionReporterCheckBox = m_GenerateCurrentPrefabExceptionReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentSceneExceptionReporterCheckBox = m_GenerateCurrentSceneExceptionReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentABStrategyReporterCheckBox = m_GenerateCurrentABStrategyReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentABContainBuiltinReporterCheckBox = m_GenerateCurrentABContainBuiltinReporterCheckBox;
            AssetRecordDataParser.GenerateCurrentResorucesFolderReporterCheckBox = m_GenerateCurrentResorucesFolderReporterCheckBox;

            // Filter Module Dir
            List<string> finalModuleList = new List<string>();
            for (int i = 0; i < this.m_ModuleFolderList.Count; i++)
            {
                var moduleFolder = this.m_ModuleFolderList[i];
                var moduleState = this.m_ModuleFolderStateList[i];
                if (Directory.Exists(moduleFolder) && moduleState)
                {
                    if (!finalModuleList.Contains(moduleFolder))
                        finalModuleList.Add(moduleFolder);
                }
            }

            // Generate Module Report
            var now = System.DateTime.Now;
            var tick = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            List<string> reportFilePathList = new List<string>();
            if (finalModuleList.Count == 0)
            {
                var reportFilePath = GenerateAssetReportForModule("Assets", tick);
                if (!string.IsNullOrEmpty(reportFilePath))
                    reportFilePathList.Add(reportFilePath);
            }
            else
            {
                foreach (var module in finalModuleList)
                {
                    var reportFilePath = GenerateAssetReportForModule(module, tick);
                    if (!string.IsNullOrEmpty(reportFilePath))
                        reportFilePathList.Add(reportFilePath);
                }
            }
            // Open Module Report File
            Debug.LogError("资源检测报告数量:" + reportFilePathList.Count);
            foreach (var moduleFile in reportFilePathList)
            {
                Debug.Log("\t" + moduleFile);
                AssetDetectionUtility.OpenFile(moduleFile);
            }
        }

        private void ClearAllConfigDataBtnCallback()
        {
            // Clear App Config
            AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_LAST_VERSION_ASSET_FILE, "");

            // Clear UI Data
          //  this.m_SelectLastVersionAssetFileTextBox = "";

            // Clear Config File
            AssetDetectionUtility.DeleteFile(AppConfigHelper.CONFIG_CUSTOM_RULE_FILE_PATH);
            AssetDetectionUtility.DeleteFile(AppConfigHelper.CONFIG_WHITE_LIST_FILE_PATH);

            // Clear Compare Report
            var files = Directory.GetFiles(AppConfigHelper.ASSET_COMPARE_REPORT_FLODER, "*." + ConstDefine.REPORT_FILE_EXTENSION, SearchOption.AllDirectories);
            foreach (var file in files)
                AssetDetectionUtility.DeleteFile(file);

        }

        #endregion

        #region Texture Compare Tool

        private void SelectLastVersionTextureFolderBtnCallback()
        {
            string selectedFolderPath = AssetDetectionUtility.OpenFolderPanel(ConstDefine.SELECT_TEXTURE_FOLER_TITLE_0, AppConfigHelper.PROJECT_DATA_BASE_FOLDER);
            this.m_TextureSelectLastVersionFolderTextBox = string.IsNullOrEmpty(selectedFolderPath) ? "" : selectedFolderPath;
        }

        private void SelectCurrentVersionTextureFolderBtnCallback()
        {
            string selectedFolderPath = AssetDetectionUtility.OpenFolderPanel(ConstDefine.SELECT_TEXTURE_FOLER_TITLE_1, AppConfigHelper.PROJECT_DATA_BASE_FOLDER);
            this.m_TextureSelectCurrentVersionFolderTextBox = string.IsNullOrEmpty(selectedFolderPath) ? "" : selectedFolderPath;
        }

        private void SaveTextureConfig()
        {
            if (!string.IsNullOrEmpty(this.m_TextureSelectLastVersionFolderTextBox))
                AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_LAST_VERSION_TEXTURE_FLODER, this.m_TextureSelectLastVersionFolderTextBox);

            if (!string.IsNullOrEmpty(this.m_TextureSelectCurrentVersionFolderTextBox))
                AppConfigHelper.AddConfigToEditorPrefs(ConstDefine.CONFIG_CURRENT_VERSION_TEXTURE_FLODER, this.m_TextureSelectCurrentVersionFolderTextBox);
        }

        private void GenerateTextureReportBtnCallback()
        {
            if (string.IsNullOrEmpty(this.m_TextureSelectCurrentVersionFolderTextBox))
            {
                AssetDetectionUtility.MessageBox("[当前版本Texture根路径]为空!!!", "生成Texture检测报告");
                return;
            }

            if (string.IsNullOrEmpty(this.m_TextureExtensionTextBox))
            {
                AssetDetectionUtility.MessageBox("[Texture文件后缀配置]为空!!!", "生成Texture检测报告");
                return;
            }

            TextureCompareReportManager.GenerateTextureCompareReport(
                this.m_TextureSelectLastVersionFolderTextBox, this.m_TextureSelectCurrentVersionFolderTextBox,
                this.m_TextureExtensionTextBox, false
            );
        }

        private void CollectTextureFileExtension()
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture", new string[] { "Assets" });
            if (guids == null || guids.Length == 0) return;

            Dictionary<string, int> fileExtensionDic = new Dictionary<string, int>();
            int len = guids.Length;
            for (int i = 0; i < len; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                EditorUtility.DisplayProgressBar("Collect Texture File Extension[" + i + "|" + len + "]", "Collect..." + assetPath, (i + 1) * 1.0f / len);

                var extension = Path.GetExtension(assetPath);
                int amount = 0;
                if (!fileExtensionDic.TryGetValue(extension, out amount)) amount = 0;

                amount++;

                fileExtensionDic[extension] = amount;
            }
            EditorUtility.ClearProgressBar();

            string extensionOutputStr = "";
            foreach (var data in fileExtensionDic)
            {
                extensionOutputStr += data.Key + "|" + data.Value + "\n";
            }
            var filePath = "TextureFileExtension.txt";
            StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            sw.Write(extensionOutputStr);
            sw.Flush();
            sw.Close();
            sw.Dispose();

            System.Diagnostics.Process.Start(filePath);
        }

        #endregion

        #region SpirteAtlas Tool

        private string GenerateSpriteAtlasReportForModule(string moduleFolder,string tick)
        {
            var reportPath = "";
            var dataFilesFolder = "";
            var spriteAtlasDataList = SpirteAtlasTool.DoCheckSpriteAtlas(moduleFolder, tick,out dataFilesFolder);
            if (!string.IsNullOrEmpty(dataFilesFolder) && spriteAtlasDataList.Count > 0)
                reportPath = SpriteAtlasReportMananger.GenerateReport(spriteAtlasDataList, dataFilesFolder, moduleFolder,tick);
            else
            {
                Debug.LogError(string.Format("图集检测报告[{0}]:[图集数据文件]路径为空!!!", moduleFolder));
            }

            return reportPath;
        }

        private void GenerateSpriteAtlasReportBtnCallback()
        {
            // Filter Module Dir
            List<string> finalModuleList = new List<string>();
            for(int i = 0; i< this.m_ModuleFolderList.Count;i ++)
            {
                var moduleFolder = this.m_ModuleFolderList[i];
                var moduleState = this.m_ModuleFolderStateList[i];
                if (Directory.Exists(moduleFolder) && moduleState)
                {
                    if (!finalModuleList.Contains(moduleFolder))
                        finalModuleList.Add(moduleFolder);
                }
            }

            // Generate Module Report
            var now = System.DateTime.Now;
            var tick = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            List<string> reportFilePathList = new List<string>();
            if (finalModuleList.Count == 0)
            {
                var reportFilePath = GenerateSpriteAtlasReportForModule("Assets",tick);
                if (!string.IsNullOrEmpty(reportFilePath))
                    reportFilePathList.Add(reportFilePath);
            }
            else
            {
                foreach (var module in finalModuleList)
                {
                    var reportFilePath = GenerateSpriteAtlasReportForModule(module, tick);
                    if (!string.IsNullOrEmpty(reportFilePath))
                        reportFilePathList.Add(reportFilePath);
                }
            }
            // Open Module Report File
            Debug.LogError("图集检测报告数量:" + reportFilePathList.Count);
            foreach (var moduleFile in reportFilePathList)
            {
                Debug.Log("\t" + moduleFile);
                AssetDetectionUtility.OpenFile(moduleFile);
            }
        }

        #endregion
    }
}
