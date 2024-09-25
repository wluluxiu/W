using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace jj.TATools.Editor
{
    partial class MainView
    {
        #region Fields

        private bool m_ToolConfigReady = false;
        private string[] m_TotalTabControls = new string[] { "资源检测工具", "图集检测工具"};//new string[] { "资源检测工具", "图集检测工具", "纹理检测工具" };
        private int m_TotalTabControlIndex = 0;
        private Vector2 m_ScrollBar;

        // Asset Compare Tool
       // private string m_SelectLastVersionAssetFileTextBox;
        private bool m_GenerateCurrentFullReporterCheckBox;
        private bool m_GenerateCurrentNewerReporterCheckBox;
        private bool m_GenerateCurrentDeletedReporterCheckBox;
        private bool m_GenerateCurrentModifiedReporterCheckBox;
        private bool m_GenerateCurrentRepeatedReporterCheckBox;
        private bool m_GenerateCurrentNoUsedReporterCheckBox;
        private bool m_GenerateCurrentTextureExceptionReporterCheckBox;
        private bool m_GenerateCurrentModelExceptionReporterCheckBox;
        private bool m_GenerateCurrentMaterialExceptionReporterCheckBox;
        private bool m_GenerateCurrentShaderExceptionReporterCheckBox;
        private bool m_GenerateCurrentScriptExceptionReporterCheckBox;
        private bool m_GenerateCurrentDepBuiltinReporterCheckBox;
        private bool m_GenerateCurrentPrefabExceptionReporterCheckBox;
        private bool m_GenerateCurrentSceneExceptionReporterCheckBox;
        private bool m_GenerateCurrentABStrategyReporterCheckBox;
        private bool m_GenerateCurrentABContainBuiltinReporterCheckBox;
        private bool m_GenerateCurrentResorucesFolderReporterCheckBox;

        // Texture Compare Tool
        private string m_TextureSelectLastVersionFolderTextBox;
        private string m_TextureSelectCurrentVersionFolderTextBox;
        private string m_TextureExtensionTextBox;

        // Module List
        private bool m_ModuleFolderFoldoutFlag = false;
        private List<string> m_ModuleFolderList = new List<string>();
        private List<bool> m_ModuleFolderStateList = new List<bool>();

        #endregion

        #region Local Methods

        private void InitializeTools()
        {
            this.m_GenerateCurrentFullReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_FULL_REPORTER_CHECKBOX;
            this.m_GenerateCurrentNewerReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_NEWER_REPORTER_CHECKBOX;
            this.m_GenerateCurrentDeletedReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_DELETED_REPORTER_CHECKBOX;
            this.m_GenerateCurrentModifiedReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_MODIFIED_REPORTER_CHECKBOX;
            this.m_GenerateCurrentRepeatedReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_REPEATED_REPORTER_CHECKBOX;
            this.m_GenerateCurrentNoUsedReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_NOUSED_REPORTER_CHECKBOX;
            this.m_GenerateCurrentTextureExceptionReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_TEXTURE_REPORTER_CHECKBOX;
            this.m_GenerateCurrentModelExceptionReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_MODEL_REPORTER_CHECKBOX;
            this.m_GenerateCurrentMaterialExceptionReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_MATERIAL_REPORTER_CHECKBOX;
            this.m_GenerateCurrentShaderExceptionReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_SHADER_REPORTER_CHECKBOX;
            this.m_GenerateCurrentScriptExceptionReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_SCRIPT_REPORTER_CHECKBOX;
            this.m_GenerateCurrentDepBuiltinReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_DEP_BUILTIN_REPORTER_CHECKBOX;
            this.m_GenerateCurrentPrefabExceptionReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_PREFAB_REPORTER_CHECKBOX;
            this.m_GenerateCurrentSceneExceptionReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_SCENE_REPORTER_CHECKBOX;
            this.m_GenerateCurrentABStrategyReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_AB_STRATEGY_REPORTER_CHECKBOX;
            this.m_GenerateCurrentABContainBuiltinReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_AB_BUILTIN_REPORTER_CHECKBOX;
            this.m_GenerateCurrentResorucesFolderReporterCheckBox = AppConfigHelper.CONFIG_GENERATE_RESOURCES_FOLDER_REPORTER_CHECKBOX;

          //  this.m_SelectLastVersionAssetFileTextBox = AppConfigHelper.LAST_VERSION_ASSET_FILE;

            this.m_TextureSelectLastVersionFolderTextBox = AppConfigHelper.LAST_VERSION_TEXTURE_FLODER;
            this.m_TextureSelectCurrentVersionFolderTextBox = AppConfigHelper.CURRENT_VERSION_TEXTURE_FLODER;
            this.m_TextureExtensionTextBox = AppConfigHelper.TEXTURE_FILE_EXTENSION;

            GetConfigFromLocalEditorPrefs();
        }

        private void Reset()
        {
            this.m_ScrollBar = Vector2.zero;
        }

        private void AssetCompareToolUILayout()
        {
            EditorGUILayout.BeginVertical("Box");

            // Config Split Flag Label //////////////////////////////////////
            EditorGUILayout.LabelField("************************************** 配置文件相关 ***********************************");

            // 生成白名单配置按钮 & 生成自定以路径配置按钮
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("生成白名单配置", GUILayout.Width(250), GUILayout.Height(50)))
            {
                this.GenerateWhiteListBtnCallback();
            }

            if (GUILayout.Button("生成自定义规则配置", GUILayout.Width(250), GUILayout.Height(50)))
            {
                this.GenerateCustomFolderBtnCallback();
            }

            EditorGUILayout.EndHorizontal();

            //// Asset File Split Flag Label //////////////////////////////////////
            //EditorGUILayout.LabelField("************************************** 资源文件相关 ***********************************");

            //// LastVersion Asset File Btn & TextBox
            //EditorGUILayout.BeginHorizontal();

            //if (GUILayout.Button("对比版本资源Cache文件:", GUILayout.Width(145), GUILayout.Height(20)))
            //{
            //    this.SelectLastVersionAssetFileBtnCallback();
            //}

            //this.m_SelectLastVersionAssetFileTextBox = EditorGUILayout.TextField("", this.m_SelectLastVersionAssetFileTextBox, GUILayout.Height(20));

            //EditorGUILayout.EndHorizontal();

            //// Save Asset File Config Btn
            //if (GUILayout.Button("保存配置", GUILayout.Height(30)))
            //{
            //    this.SaveBuildAssetFileConfig();
            //}

            // Excute Operation Split Flag Label //////////////////////////////////////
            EditorGUILayout.LabelField("************************************** 执行相关 ***************************************");

            // 
            // Generate Asset Report & Clear All Config DataBtn
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("生成资源检测报告", GUILayout.Width(250), GUILayout.Height(50)))
            {
                this.GenerateAssetReportBtnCallback();
            }

            if (GUILayout.Button("清空所有数据", GUILayout.Width(250), GUILayout.Height(50)))
            {
                this.ClearAllConfigDataBtnCallback();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            this.m_GenerateCurrentFullReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_1, this.m_GenerateCurrentFullReporterCheckBox,GUILayout.Height(20));

           // GUILayout.FlexibleSpace();

            this.m_GenerateCurrentNewerReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_2, this.m_GenerateCurrentNewerReporterCheckBox, GUILayout.Height(20));
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            this.m_GenerateCurrentDeletedReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_3, this.m_GenerateCurrentDeletedReporterCheckBox, GUILayout.Height(20));

           // GUILayout.FlexibleSpace();

            this.m_GenerateCurrentModifiedReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_4, this.m_GenerateCurrentModifiedReporterCheckBox, GUILayout.Height(20));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            this.m_GenerateCurrentRepeatedReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_5, this.m_GenerateCurrentRepeatedReporterCheckBox, GUILayout.Height(20));

           // GUILayout.FlexibleSpace();

            this.m_GenerateCurrentNoUsedReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_6, this.m_GenerateCurrentNoUsedReporterCheckBox, GUILayout.Height(20));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            this.m_GenerateCurrentTextureExceptionReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_7, this.m_GenerateCurrentTextureExceptionReporterCheckBox, GUILayout.Height(20));

           // GUILayout.FlexibleSpace();

            this.m_GenerateCurrentModelExceptionReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_8, this.m_GenerateCurrentModelExceptionReporterCheckBox, GUILayout.Height(20));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            this.m_GenerateCurrentMaterialExceptionReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_9, this.m_GenerateCurrentMaterialExceptionReporterCheckBox, GUILayout.Height(20));

           // GUILayout.FlexibleSpace();

            this.m_GenerateCurrentShaderExceptionReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_10, this.m_GenerateCurrentShaderExceptionReporterCheckBox, GUILayout.Height(20));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            this.m_GenerateCurrentScriptExceptionReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_11, this.m_GenerateCurrentScriptExceptionReporterCheckBox, GUILayout.Height(20));

           // GUILayout.FlexibleSpace();

            this.m_GenerateCurrentDepBuiltinReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_12, this.m_GenerateCurrentDepBuiltinReporterCheckBox, GUILayout.Height(20));

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();

            this.m_GenerateCurrentPrefabExceptionReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_13, this.m_GenerateCurrentPrefabExceptionReporterCheckBox, GUILayout.Height(20));

           // GUILayout.FlexibleSpace();

            this.m_GenerateCurrentSceneExceptionReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_14, this.m_GenerateCurrentSceneExceptionReporterCheckBox, GUILayout.Height(20));

            EditorGUILayout.EndHorizontal();

            // 打包方式不同，需要重构
            EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.BeginHorizontal();

            this.m_GenerateCurrentABStrategyReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_15, this.m_GenerateCurrentABStrategyReporterCheckBox, GUILayout.Height(20));

           // GUILayout.FlexibleSpace();

            this.m_GenerateCurrentABContainBuiltinReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_16, this.m_GenerateCurrentABContainBuiltinReporterCheckBox, GUILayout.Height(20));

            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginHorizontal();

            this.m_GenerateCurrentResorucesFolderReporterCheckBox = EditorGUILayout.ToggleLeft(ConstDefine.REPORT_ASSETCOMPARE_SHEET_17, this.m_GenerateCurrentResorucesFolderReporterCheckBox, GUILayout.Height(20));

           // GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void TextureCompareToolUILayout()
        {
            EditorGUILayout.BeginVertical("Box");

            // Texture Config Split Flag Label
            EditorGUILayout.LabelField("************************************** 配置相关 ***************************************");

            // LastVersion Texture Folder Btn & TextBox
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("对比版本Texture根路径", GUILayout.Width(140), GUILayout.Height(20)))
            {
                this.SelectLastVersionTextureFolderBtnCallback();
            }

            this.m_TextureSelectLastVersionFolderTextBox = EditorGUILayout.TextField("", this.m_TextureSelectLastVersionFolderTextBox, GUILayout.Height(20));

            EditorGUILayout.EndHorizontal();

            // CurrentVersion Texture Folder Btn & TextBox
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("当前版本Texture根路径", GUILayout.Width(140), GUILayout.Height(20)))
            {
                this.SelectCurrentVersionTextureFolderBtnCallback();
            }

            this.m_TextureSelectCurrentVersionFolderTextBox = EditorGUILayout.TextField("", this.m_TextureSelectCurrentVersionFolderTextBox, GUILayout.Height(20));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Texture文件后缀配置", GUILayout.Width(140), GUILayout.Height(20)))
            {

            }

            this.m_TextureExtensionTextBox = EditorGUILayout.TextField("", this.m_TextureExtensionTextBox, GUILayout.Height(20));

            EditorGUILayout.EndHorizontal();

            // Save Texture Config Btn
            if (GUILayout.Button("保存配置", GUILayout.Height(30)))
            {
                this.SaveTextureConfig();
            }

            // Texture Excute Operation Split Flag Label
            EditorGUILayout.LabelField("************************************** 执行相关 ***************************************");

            EditorGUILayout.BeginHorizontal();

            // Generate Texture Report Btn
            if (GUILayout.Button("生成Texture检测报告", GUILayout.Height(50)))
            {
                this.GenerateTextureReportBtnCallback();
            }
            // Generate Texture Cache Report Btn
            if (GUILayout.Button("纹理文件扩展名收集", GUILayout.Height(50)))
            {
                this.CollectTextureFileExtension();
            }

            EditorGUILayout.EndHorizontal();
            

            EditorGUILayout.EndVertical();
        }

        private void SpriteAtlasUILayout()
        {
            EditorGUILayout.BeginVertical("Box");

            if (GUILayout.Button("生成图集检测报告", GUILayout.Height(50)))
            {
                this.GenerateSpriteAtlasReportBtnCallback();
            }

            EditorGUILayout.EndVertical();
        }

        private void UILayout()
        {

            // Module Folder List////////////////////////////////////
            GUI.color = Color.cyan;
            m_ModuleFolderFoldoutFlag = EditorGUILayout.Foldout(m_ModuleFolderFoldoutFlag, "模块目录列表");
            if (m_ModuleFolderFoldoutFlag)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginVertical("Box");

                int deleteModuleIndex = -1;
                for (int i = 0; i < this.m_ModuleFolderList.Count; i++)
                {
                    var moduleFolder = this.m_ModuleFolderList[i];
                    var moduleState = this.m_ModuleFolderStateList[i];

                    Color oriColor = GUI.color;

                    if (!Directory.Exists(moduleFolder))
                        GUI.color = Color.red;
                    else
                    {
                        GUI.color = moduleState ? oriColor : Color.grey;
                    }

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField("No." + (i + 1), GUILayout.Width(60));

                    this.m_ModuleFolderStateList[i] = EditorGUILayout.ToggleLeft("", moduleState, GUILayout.Width(30));

                    EditorGUILayout.LabelField(moduleFolder);

                    if (GUILayout.Button("-", GUILayout.Width(30)))
                    {
                        deleteModuleIndex = i;
                    }

                    EditorGUILayout.EndHorizontal();

                    GUI.color = oriColor;
                }

                EditorGUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    string tempModuleFolder = EditorUtility.OpenFolderPanel("添加模块路径", Application.dataPath, "");
                    if (!string.IsNullOrEmpty(tempModuleFolder))
                    {
                        tempModuleFolder = tempModuleFolder.Replace(Application.dataPath, "Assets").Replace("\\", "/");
                        this.m_ModuleFolderList.Add(tempModuleFolder);
                        this.m_ModuleFolderStateList.Add(true);

                        SaveConfigToLocalEditorPrefs();
                    }
                }

                EditorGUILayout.EndHorizontal();

                if (deleteModuleIndex >= 0 && deleteModuleIndex < this.m_ModuleFolderList.Count)
                {
                    this.m_ModuleFolderList.RemoveAt(deleteModuleIndex);
                    this.m_ModuleFolderStateList.RemoveAt(deleteModuleIndex);

                    SaveConfigToLocalEditorPrefs();
                }

                EditorGUILayout.EndVertical();

                EditorGUI.indentLevel--;
            }

            GUILayout.Space(10);

            GUI.color = Color.yellow;

            // Tool Menu ////////////////////////////////////
            int tempIndex = GUILayout.Toolbar(this.m_TotalTabControlIndex, this.m_TotalTabControls);
            if (tempIndex != this.m_TotalTabControlIndex)
            {
                this.m_TotalTabControlIndex = tempIndex;
                Reset();
            }

            GUI.color = Color.white;

            m_ScrollBar = EditorGUILayout.BeginScrollView(m_ScrollBar, false, false);

            switch (this.m_TotalTabControlIndex)
            {
                case 0:
                    AssetCompareToolUILayout();
                    break;
                case 1:
                    SpriteAtlasUILayout();
                    break;
                //case 2:
                //    TextureCompareToolUILayout();
                //    break;
            }

            EditorGUILayout.EndScrollView();
        }

        private void GetConfigFromLocalEditorPrefs()
        {
            this.m_ModuleFolderList.Clear();
            this.m_ModuleFolderStateList.Clear();
            int moduleFolderAmount = EditorPrefs.GetInt(ConstDefine.MODULE_FOLDER_AMOUNT);
            bool needUpdate = false;
            if (moduleFolderAmount > 0)
            {
                for (int i = 0; i < moduleFolderAmount; i++)
                {
                    var moduleFolder = EditorPrefs.GetString(ConstDefine.MODULE_FOLDER_ITEM_DIR + i);
                    var moduleFolderState = EditorPrefs.GetBool(ConstDefine.MODULE_FOLDER_ITEM_STATE + i);
                    if (string.IsNullOrEmpty(moduleFolder) || !Directory.Exists(moduleFolder))
                    {
                        needUpdate = true;
                        continue;
                    }

                    if (!this.m_ModuleFolderList.Contains(moduleFolder))
                    {
                        this.m_ModuleFolderList.Add(moduleFolder);
                        this.m_ModuleFolderStateList.Add(moduleFolderState);
                    }
                    else
                        needUpdate = true;
                }
            }
            else
            {
                needUpdate = true;

                var targetModulePackageFileName = "package.json";
                var packageFiles = Directory.GetFiles(Application.dataPath, "*.json", SearchOption.AllDirectories);
                if (packageFiles != null && packageFiles.Length > 0)
                {
                    foreach (var packageFile in packageFiles)
                    {
                        var fileName = Path.GetFileName(packageFile);
                        if (fileName != targetModulePackageFileName) continue;
                        var parentInfo = Directory.GetParent(packageFile);
                        var moduleFolder = parentInfo.FullName;
                        moduleFolder = moduleFolder.Replace("\\", "/").Replace(Application.dataPath.Replace("\\", "/"), "Assets");
                        if (!this.m_ModuleFolderList.Contains(moduleFolder))
                        {
                            this.m_ModuleFolderList.Add(moduleFolder);
                            this.m_ModuleFolderStateList.Add(true);
                        }
                    }
                }
                
            }

            if (needUpdate)
                SaveConfigToLocalEditorPrefs();
        }

        private void SaveConfigToLocalEditorPrefs()
        {
            EditorPrefs.SetInt(ConstDefine.MODULE_FOLDER_AMOUNT, this.m_ModuleFolderList.Count);
            for (int i = 0; i < this.m_ModuleFolderList.Count; i++)
            {
                EditorPrefs.SetString(ConstDefine.MODULE_FOLDER_ITEM_DIR + i, this.m_ModuleFolderList[i]);
                EditorPrefs.SetBool(ConstDefine.MODULE_FOLDER_ITEM_STATE + i, this.m_ModuleFolderStateList[i]);
            }
        }

        #endregion
    }
}

