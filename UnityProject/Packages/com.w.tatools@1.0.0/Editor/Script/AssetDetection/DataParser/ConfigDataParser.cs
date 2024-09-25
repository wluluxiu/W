using System.IO;
using System.Collections.Generic;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace jj.TATools.Editor
{
    internal class ConfigDataParser
    {
        #region Fields

        internal static readonly System.Drawing.Color TITLE_COLOR = System.Drawing.Color.FromArgb(155, 194, 230);
        internal static readonly System.Drawing.Color ITEM_COLOR = System.Drawing.Color.FromArgb(255, 255, 255);

        #endregion

        #region Internal Methods

        internal static List<string> GetDynamicLoadedWhiteListData(out bool error)
        {
            error = false;
            string whiteListFilePath = AssetDetectionUtility.PathCombine(AppConfigHelper.ASSET_WHITELIST_FLODER, ConstDefine.CONFIG_WHITELIST_FILE_NAME);
            if (!File.Exists(whiteListFilePath)) return null;

            List<string> dynamicLoadedWhiteList = new List<string>();
            FileInfo sourFile = null;
            try
            {
                sourFile = new FileInfo(whiteListFilePath);
                
            }
            catch(System.Exception e)
            {
                error = true;
                AssetDetectionUtility.MessageBox("[" + Path.GetFileName(whiteListFilePath) + "]文件已经被打开，请关闭后重新生成报告！", "生成资源检测报告异常");
                return dynamicLoadedWhiteList;
            }

            using (ExcelPackage package = new ExcelPackage(sourFile))
            {
                var worksheets = package.Workbook.Worksheets;
                int sheetCount = worksheets.Count;
                for (int i = 0; i <= sheetCount; i++)
                {
                    var worksheet = worksheets[i];

                    if (worksheet.Name == ConstDefine.CONFIG_WHITELIST_DYNAMIC_LOADED)
                    {
                        var start = worksheet.Dimension.Start;
                        var end = worksheet.Dimension.End;

                        for (int k = start.Row + 1; k <= end.Row; k++)
                        {
                            var value = worksheet.Cells[k, 3].Value;
                            if (value == null) continue;

                            string assetPath = value.ToString();
                            if (!dynamicLoadedWhiteList.Contains(assetPath))
                                dynamicLoadedWhiteList.Add(assetPath);
                        }

                        break;
                    }

                }
            }
            return dynamicLoadedWhiteList;
        }

        internal static void GenerateWhiteListConfigFile(string whiteListConfigFilePath)
        {
            FileInfo newfile = new FileInfo(whiteListConfigFilePath);
            if (newfile.Exists)
            {
                newfile.Delete();
                newfile = new FileInfo(whiteListConfigFilePath);
            }

            using (ExcelPackage package = new ExcelPackage(newfile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(ConstDefine.CONFIG_WHITELIST_DYNAMIC_LOADED);
                worksheet.View.ShowGridLines = false;
                worksheet.View.FreezePanes(2, 1);
                worksheet.Column(1).Width = 5;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 150;

                System.Drawing.Color Blue_1 = System.Drawing.Color.FromArgb(155, 194, 230);

                // Title
                int row = 1;
                ExcelRange tempExcelRange = worksheet.Cells[row, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.CONFIG_WHITELIST_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     Blue_1);
                tempExcelRange = worksheet.Cells[row, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.CONFIG_WHITELIST_TITLE_2, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     Blue_1);

                int demoTotalRow = 100;
                ExcelHelper.SetExcelRangeDataListValidation(worksheet, worksheet.Cells[2, 2, demoTotalRow, 2].Address, new List<string>() {
                    "纹理","材质","Shader","Prefab","模型","动画","动画控制器",
                });

                for (row = 2; row < demoTotalRow; row++)
                {
                    tempExcelRange = worksheet.Cells[row, 2];
                    ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         ITEM_COLOR);
                    tempExcelRange = worksheet.Cells[row, 3];
                    ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         ITEM_COLOR);
                }


                package.Save();
            }
        }

        internal static Dictionary<string,string> GetCustomPathRuleDataMapping(out bool error)
        {
            error = false;
            string pathRuleFilePath = AssetDetectionUtility.PathCombine(AppConfigHelper.ASSET_CUSTOM_RULE_FOLDER, ConstDefine.CONFIG_CUSTOM_RULE_FILE_NAME);
            if (!File.Exists(pathRuleFilePath)) return null;

            Dictionary<string, string> pathRuleDic = new Dictionary<string, string>();
            FileInfo sourFile = null;
            try
            {
                sourFile = new FileInfo(pathRuleFilePath);
                
            }
            catch (System.Exception e)
            {
                error = true;
                AssetDetectionUtility.MessageBox("[" + Path.GetFileName(pathRuleFilePath) + "]文件已经被打开，请关闭后重新生成报告！", "生成资源检测报告异常");
                return pathRuleDic;
            }

            using (ExcelPackage package = new ExcelPackage(sourFile))
            {
                var worksheets = package.Workbook.Worksheets;
                int sheetCount = worksheets.Count;
                for (int i = 0; i <= sheetCount; i++)
                {
                    var worksheet = worksheets[i];

                    if (worksheet.Name == ConstDefine.CONFIG_CUSTOM_RULE_0_FOLDER_PATH)
                    {
                        var start = worksheet.Dimension.Start;
                        var end = worksheet.Dimension.End;

                        for (int k = start.Row + 1; k <= end.Row; k++)
                        {
                            var dirPathValue = worksheet.Cells[k, 3].Value;
                            if (dirPathValue == null) continue;

                            var dirNameValue = worksheet.Cells[k, 2].Value;

                            string dirPath = dirPathValue.ToString();
                            string dirName = dirNameValue == null ? dirPath : dirNameValue.ToString();
                            if (!pathRuleDic.ContainsKey(dirPath))
                                pathRuleDic.Add(dirPath, dirName);
                        }

                        break;
                    }

                }
            }
            return pathRuleDic;
        }

        internal static void GenerateCustomRuleConfigFile(string customRuleConfigFilePath)
        {
            FileInfo newfile = new FileInfo(customRuleConfigFilePath);
            if (newfile.Exists)
            {
                newfile.Delete();
                newfile = new FileInfo(customRuleConfigFilePath);
            }

            using (ExcelPackage package = new ExcelPackage(newfile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(ConstDefine.CONFIG_CUSTOM_RULE_0_FOLDER_PATH);
                worksheet.View.ShowGridLines = false;
                worksheet.View.FreezePanes(2, 1);
                worksheet.Column(1).Width = 5;
                worksheet.Column(2).Width = 40;
                worksheet.Column(3).Width = 150;

                // Title
                int row = 1;
                ExcelRange tempExcelRange = worksheet.Cells[row, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.CONFIG_PATH_RULE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     TITLE_COLOR);
                tempExcelRange = worksheet.Cells[row, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.CONFIG_PATH_RULE_TITLE_2, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     TITLE_COLOR);

                int demoTotalRow = 100;
                for (row = 2; row < demoTotalRow; row++)
                {
                    tempExcelRange = worksheet.Cells[row, 2];
                    ExcelHelper.SetExcelRange(tempExcelRange, null, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         ITEM_COLOR);
                    tempExcelRange = worksheet.Cells[row, 3];
                    ExcelHelper.SetExcelRange(tempExcelRange, null, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         ITEM_COLOR);
                }


                package.Save();
            }
        }

        #endregion
    }
}
