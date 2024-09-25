using System.IO;
using OfficeOpenXml;

namespace jj.TATools.Editor
{
    internal class AssetCompareReportManager
    {
        internal static string GenerateAssetCompareReport(string currentVersionAssetFile,string moduleFolder,string tick, string lastVersionAssetFile = "")
        {
            ExcelHelper.SetExcelPackageLicenseContextProperty(LicenseContext.NonCommercial);

            var outputFolder = AssetDetectionUtility.PathCombine(AppConfigHelper.ASSET_COMPARE_REPORT_FLODER, tick);
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
            var fileName = string.Format("《{0}》{1}.{2}", new DirectoryInfo(moduleFolder).Name, ConstDefine.REPORT_FILE_NAME, ConstDefine.REPORT_FILE_EXTENSION);
            string assetCompareReportPath = AssetDetectionUtility.PathCombine(outputFolder, fileName);
            if (string.IsNullOrEmpty(assetCompareReportPath)) return null;

            if (AssetRecordDataParser.ParseData(lastVersionAssetFile, currentVersionAssetFile))
            {
                FileInfo sourFile = null;
                try
                {
                    sourFile = new FileInfo(assetCompareReportPath);
                    if (sourFile.Exists)
                    {
                        sourFile.Delete();
                        sourFile = new FileInfo(assetCompareReportPath);
                    }
                }
                catch (System.Exception e)
                {
                    AssetDetectionUtility.MessageBox("[" + Path.GetFileName(assetCompareReportPath) + "]文件已经被打开，请关闭后重新生成报告！", "生成资源检测报告异常");
                    return null;
                }

                using (ExcelPackage package = new ExcelPackage(sourFile))
                {

                    // 整体统计以及报告目录
                    AssetTotalContentReporter.GenerateReport(package);

                    // 当前版本资源详情报告
                    if (AssetRecordDataParser.GenerateCurrentFullReporterCheckBox)
                    {
                        AssetCurrentTotalReporter.GenerateReport(package);
                    }

                    // 资源迭代新增详情报告
                    if (AssetRecordDataParser.GenerateCurrentNewerReporterCheckBox)
                    {
                        AssetNewerReporter.GenerateReport(package);
                    }

                    // 资源迭代删除详情报告
                    if (AssetRecordDataParser.GenerateCurrentDeletedReporterCheckBox)
                    {
                        AssetDeletedReporter.GenerateReport(package);
                    }

                    // 资源迭代修改详情报告
                    if (AssetRecordDataParser.GenerateCurrentModifiedReporterCheckBox)
                    {
                        AssetModifiedReporter.GenerateReport(package);
                    }

                    // 重复资源详情报告
                    if (AssetRecordDataParser.GenerateCurrentRepeatedReporterCheckBox)
                    {
                        AssetRepeatReporter.GenerateReport(package);
                    }

                    // 冗余资源详情报告
                    if (AssetRecordDataParser.GenerateCurrentNoUsedReporterCheckBox)
                    {
                        AssetWithoutReferencesReporter.GenerateReport(package);
                    }

                    // 纹理异常详情报告
                    if (AssetRecordDataParser.GenerateCurrentTextureExceptionReporterCheckBox)
                    {
                        TextureExceptionReporter.GenerateReport(package);
                    }

                    // 模型异常详情报告
                    if (AssetRecordDataParser.GenerateCurrentModelExceptionReporterCheckBox)
                    {
                        ModelExceptionReporter.GenerateReport(package);
                    }

                    // 材质异常详情报告
                    if (AssetRecordDataParser.GenerateCurrentMaterialExceptionReporterCheckBox)
                    {
                        MaterialExceptionReporter.GenerateReport(package);
                    }

                    // Shader详情报告
                    if (AssetRecordDataParser.GenerateCurrentShaderExceptionReporterCheckBox)
                    {
                        ShaderVariantReporter.GenerateReport(package);
                    }

                    // 脚本异常引用详情报告
                    if (AssetRecordDataParser.GenerateCurrentScriptExceptionReporterCheckBox)
                    {
                        ScriptDependenciesReporter.GenerateReport(package);
                    }

                    // 引用内置资源详情报告
                    if (AssetRecordDataParser.GenerateCurrentDepBuiltinReporterCheckBox)
                    {
                        AssetBuiltinDependenciesReproter.GenerateReport(package);
                    }

                    // Prefab异常详情报告
                    if (AssetRecordDataParser.GenerateCurrentPrefabExceptionReporterCheckBox)
                    {
                        PrefabExceptionReporter.GenerateReport(package);
                    }

                    // Scene异常详情报告
                    if (AssetRecordDataParser.GenerateCurrentSceneExceptionReporterCheckBox)
                    {
                        SceneExceptionReporter.GenerateReport(package);
                    }

                    // 分包策略异常详情报告
                    if (AssetRecordDataParser.GenerateCurrentABStrategyReporterCheckBox)
                    {
                        AssetbundleStrategyExceptionReporter.GenerateReport(package);
                    }
       
                    // Assetbunle内置资源详情报告
                    if (AssetRecordDataParser.GenerateCurrentABContainBuiltinReporterCheckBox)
                    {
                        AssetbundleBuiltinResReporter.GenerateReport(package);
                    }

                    // Resources目录资源统计
                    if (AssetRecordDataParser.GenerateCurrentResorucesFolderReporterCheckBox)
                    {
                        AssetInResourcesFolderReporter.GenerateReport(package);
                    }

                    // 定制文件夹详情报告
                    // if (progressFunc != null) progressFunc(ConstDefine.REPORT_TIPS_10, 98);
                    // AssetCustomDirRuleReporter.GenerateReport(package);

                    package.Save();
                }
            }

            return assetCompareReportPath;
        }

    }
}
