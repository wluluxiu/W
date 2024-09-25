using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace jj.TATools.Editor
{
    /// <summary>
    ///  脚本异常引用详情报告
    /// </summary>
    internal class ScriptDependenciesReporter
    {
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color UPDATE_DATA_COLOR = System.Drawing.Color.FromArgb(255, 255, 0);

            ExcelWorksheet scriptExceptionWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_11);
            scriptExceptionWS.View.ShowGridLines = false;
            scriptExceptionWS.View.FreezePanes(10, 1);
            scriptExceptionWS.Column(1).Width = 5;
            scriptExceptionWS.Column(2).Width = 10;
            scriptExceptionWS.Column(3).Width = 18;
            scriptExceptionWS.Column(4).Width = 18;
            scriptExceptionWS.Column(5).Width = 18;
            scriptExceptionWS.Column(6).Width = 18;
            scriptExceptionWS.Column(7).Width = 18;
            scriptExceptionWS.Column(8).Width = 18;
            scriptExceptionWS.Column(9).Width = 18;
            scriptExceptionWS.Column(10).Width = 18;
            scriptExceptionWS.Column(11).Width = 18;
            scriptExceptionWS.Column(12).Width = 18;
            scriptExceptionWS.Column(13).Width = 18;

            // 脚本异常引用统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = scriptExceptionWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = scriptExceptionWS.Cells[1, 2, 1, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SCRIPT_DEPENDENDENCIES_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = scriptExceptionWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SCRIPT_EXCEPTION_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = scriptExceptionWS.Cells[2, 3,2,4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            // 图例
            tempExcelRange = scriptExceptionWS.Cells[5, 2, 5, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 System.Drawing.Color.FromArgb(255, 255, 255));
            tempExcelRange = scriptExceptionWS.Cells[6, 2, 6, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_CURRENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR);
            tempExcelRange = scriptExceptionWS.Cells[7, 2, 7, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_LAST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            // Title /////////////////////////////
            int row = 9;
            tempExcelRange = scriptExceptionWS.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SCRIPT_EXCEPTION_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = scriptExceptionWS.Cells[row, 3, row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = scriptExceptionWS.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = scriptExceptionWS.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = scriptExceptionWS.Cells[row, 9,row,13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_7, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // 脚本异常引用详情 ///////////////////////////////////////////////////////////
            row++;
            var currentTotalFilesMapping = AssetRecordDataParser.CurrentVerisonFilesMapping;
            int totalFileAmount = 0;
            long currentTotalFileSize = 0;
            Dictionary<string, BaseRecorder> scriptsDataMapping = null;
            if (currentTotalFilesMapping.TryGetValue(EAssetType.Script, out scriptsDataMapping))
            {
                if (scriptsDataMapping.Count > 0)
                {
                    // Details /////////////////////////////
                    Dictionary<string, BaseRecorder> tempAddedDic = null;
                    AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(EAssetType.Texture, out tempAddedDic);
                    Dictionary<string, BaseRecorder> tempModifiedDic = null;
                    AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(EAssetType.Texture, out tempModifiedDic);
                    System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                    foreach (var baseRecorder in scriptsDataMapping.Values)
                    {
                        if (!baseRecorder.DependenciesInvalid()) continue;

                        totalFileAmount++;
                        currentTotalFileSize += baseRecorder.m_FileDiskSize;

                        bool updateAsset = false;
                        if (tempAddedDic != null && tempAddedDic.ContainsKey(baseRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }
                        if (tempModifiedDic != null && tempModifiedDic.ContainsKey(baseRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }

                        if (updateAsset)
                        {
                            itemColor = UPDATE_DATA_COLOR;
                        }

                        int addRow = baseRecorder.m_DirectDepenpendices.Count - 1;
                        tempExcelRange = scriptExceptionWS.Cells[row, 2,row + addRow, 2];
                        ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = scriptExceptionWS.Cells[row, 3, row + addRow, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, baseRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = scriptExceptionWS.Cells[row, 7, row + addRow, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(baseRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = scriptExceptionWS.Cells[row, 8, row + addRow, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, baseRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = scriptExceptionWS.Cells[row, 9, row + addRow, 9];
                        ExcelHelper.SetExcelRange(tempExcelRange, baseRecorder.m_DirectDepenpendices.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        for (int k = 0; k < baseRecorder.m_DirectDepenpendices.Count; k++)
                        {
                            tempExcelRange = scriptExceptionWS.Cells[row + k, 10, row + k, 13];
                            ExcelHelper.SetExcelRange(tempExcelRange, baseRecorder.m_DirectDepenpendices[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                 itemColor);
                        }
   
                        row+= baseRecorder.m_DirectDepenpendices.Count;
                    }
                }
            }

            // 数据：脚本异常引用统计 /////////////////////////////////////
            tempExcelRange = scriptExceptionWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = scriptExceptionWS.Cells[3, 3,3,4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
        }
    }
}
