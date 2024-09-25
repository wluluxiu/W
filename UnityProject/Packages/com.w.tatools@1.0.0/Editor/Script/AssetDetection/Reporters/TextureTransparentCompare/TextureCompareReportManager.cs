using System.IO;
using OfficeOpenXml;

namespace jj.TATools.Editor
{
    internal class TextureCompareReportManager
    {
        internal static void GenerateTextureCompareReport(string lastVersionTextureFolder, string currentVersionTextureFolder,string findFilefilters,bool onlyCache)
        {
            ExcelHelper.SetExcelPackageLicenseContextProperty(LicenseContext.NonCommercial);

            string textureCompareReportPath = AssetDetectionUtility.SaveFilePanel(ConstDefine.REPORT_ASSET_TEXTURE_FILE_NAME, ConstDefine.REPORT_ASSET_TEXTURE_FILE_NAME, ConstDefine.REPORT_FILE_EXTENSION, AppConfigHelper.TEXTURE_COMPARE_REPORT_FOLDER);
            if (string.IsNullOrEmpty(textureCompareReportPath)) return;

            if (TextureDataParser.ParseData(lastVersionTextureFolder, currentVersionTextureFolder,findFilefilters, onlyCache))
            {
                FileInfo sourFile = null;
                try
                {
                    sourFile = new FileInfo(textureCompareReportPath);
                    if (sourFile.Exists)
                    {
                        sourFile.Delete();
                        sourFile = new FileInfo(textureCompareReportPath);
                    }
                }
                catch (System.Exception e)
                {
                    AssetDetectionUtility.MessageBox("[" + Path.GetFileName(textureCompareReportPath) + "]文件已经被打开，请关闭后重新生成报告！", "生成资源检测报告异常");
                    return;
                }

                using (ExcelPackage package = new ExcelPackage(sourFile))
                {
                    // 2D整体统计以及报告目录
                    TextureTotalContentReporter.GenerateReport(package);

                    // 当前版本资源详情报告
                    TextureCurrentTotalReporter.GenerateReport(package);

                    // 资源迭代新增详情报告
                    TextureNewerReporter.GenerateReport(package);

                    // 资源迭代删除详情报告
                    TextureDeletedReporter.GenerateReport(package);

                    // 资源迭代修改详情报告
                    TextureModifiedReporter.GenerateReport(package);

                    //// 重复资源详情报告
                    //if (progressFunc != null) progressFunc(ConstDefine.REPORT_TEXTURE_TIPS_6, 80);
                    //TextureRepeatReporter.GenerateReport(package);

                    // 透明度占比详情报告
                    TextureTransparentReporter.GenerateReport(package);

                    package.Save();
                }

                AssetDetectionUtility.OpenFile(textureCompareReportPath);
            }
        }

    }
}