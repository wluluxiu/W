using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Linq;

namespace jj.TATools.Editor
{
    /// <summary>
    ///  分包策略异常详情报告
    /// </summary>
    internal class AssetbundleStrategyExceptionReporter
    {
        static int GenerateItemReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
           System.Drawing.Color CONTENT_DATA_COLOR_0,
           System.Drawing.Color CONTENT_DATA_COLOR_1,
           System.Drawing.Color CONTENT_DATA_COLOR_2)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 11];
            ExcelHelper.SetHyperLink(tempExcelRange, worksheet.Name, hyperLinkToTotalMapping[typeName], typeShowName);
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            row++;
            ExcelHelper.SetExcelRow(worksheet, row, 1);
            tempExcelRange = worksheet.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 3, row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9,row,11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            
            // Total /////////////////////////////
            row++;
            ExcelHelper.SetExcelRow(worksheet, row, 1);
            tempExcelRange = worksheet.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, "(" + assetDataMapping.Count + " 个)", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);
            tempExcelRange = worksheet.Cells[row, 3, row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int startTypeRow = row;
            int typeAddRow = 0;
            var sortDataMapping = assetDataMapping.OrderByDescending(o => o.Value.m_BundleNames.Count).ToDictionary(o => o.Key, p => p.Value);
            foreach (var asset in sortDataMapping.Values)
            {
                int addRow = asset.m_BundleNames.Count - 1;
                tempExcelRange = worksheet.Cells[row, 3, row + addRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = worksheet.Cells[row, 8, row + addRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = worksheet.Cells[row, 9, row + addRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_BundleNames.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                
                for (int i = 0; i < asset.m_BundleNames.Count; i++)
                {
                    ExcelHelper.SetExcelRow(worksheet, row + i, 1);
                    tempExcelRange = worksheet.Cells[row + i, 10, row + i, 11];
                    ExcelHelper.SetExcelRange(tempExcelRange, asset.m_BundleNames[i], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         CONTENT_DATA_COLOR_1);

                    typeAddRow++;
                }

                row+= asset.m_BundleNames.Count;
            }

            int endTypeRow = startTypeRow + typeAddRow - 1;
            tempExcelRange = worksheet.Cells[startTypeRow, 2, endTypeRow, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return endTypeRow + 1;
        }

        /// <summary>
        ///  打包方式不同，需要重构
        /// </summary>
        /// <param name="package"></param>
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);

            ExcelWorksheet abExceptionWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_15);
            abExceptionWS.View.ShowGridLines = false;
            abExceptionWS.Column(1).Width = 5;
            abExceptionWS.Column(2).Width = 30;
            abExceptionWS.Column(3).Width = 18;
            abExceptionWS.Column(4).Width = 18;
            abExceptionWS.Column(5).Width = 18;
            abExceptionWS.Column(6).Width = 18;
            abExceptionWS.Column(7).Width = 18;
            abExceptionWS.Column(8).Width = 14;
            abExceptionWS.Column(9).Width = 14;
            abExceptionWS.Column(10).Width = 20;
            abExceptionWS.Column(11).Width = 20;


            // 分包策略异常统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = abExceptionWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = abExceptionWS.Cells[1, 2, 1, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_ASSETBUNDLE_STRATEGY_EXCEPTION_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = abExceptionWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = abExceptionWS.Cells[2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_7, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            /*
            // 超链接：分包策略异常统计
            var currentTotalFilesMapping = AssetRecordDataParser.AssetbundleStrategyExceptionMapping;
            int currentRow = 4;
            var allAssetTypeMapping = AssetRecordDataParser.AssetTypeMapping;
            Dictionary<EAssetType, string> hyperLinkToTotalMapping = new Dictionary<EAssetType, string>();
            foreach (var assetType in allAssetTypeMapping)
            {
                var typeShowName = assetType.ToString();

                Dictionary<string, BaseRecorder> assetDataMapping = null;
                if (!currentTotalFilesMapping.TryGetValue(assetType, out assetDataMapping)) continue;

                if (assetDataMapping.Count == 0) continue;

                hyperLinkToTotalMapping[assetType] = "B" + currentRow;

                currentRow++;
            }
            // 分包策略异常详情 ///////////////////////////////////////////////////////////
            currentRow++;
            Dictionary<EAssetType, string> hyperLinkToDetailsMapping = new Dictionary<EAssetType, string>();
            Dictionary<EAssetType, int> typeAmountMapping = new Dictionary<EAssetType, int>();
            int totalFileAmount = 0;
            foreach (var assetType in allAssetTypeMapping)
            {
                var typeShowName = assetType.ToString();
                Dictionary<string, BaseRecorder> assetDataMapping = null;
                if (!currentTotalFilesMapping.TryGetValue(assetType, out assetDataMapping)) continue;

                if (assetDataMapping.Count == 0) continue;

                typeAmountMapping[assetType] = assetDataMapping.Count;
                totalFileAmount += assetDataMapping.Count;

                hyperLinkToDetailsMapping[assetType] = "B" + currentRow;

                currentRow = GenerateItemReport(abExceptionWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0);

                currentRow++;
            }

            // 数据：分包策略异常统计 /////////////////////////////////////
            int detailsRow = 4;
            foreach (var assetType in allAssetTypeMapping)
            {
                var typeShowName = assetType.ToString();

                Dictionary<string, BaseRecorder> assetDataMapping = null;
                if (!currentTotalFilesMapping.TryGetValue(assetType, out assetDataMapping)) continue;

                if (assetDataMapping.Count == 0) continue;

                tempExcelRange = abExceptionWS.Cells[detailsRow, 2];
                ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_15, hyperLinkToDetailsMapping[assetType], typeShowName);
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = abExceptionWS.Cells[detailsRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, typeAmountMapping[assetType], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);

                detailsRow++;
            }

            tempExcelRange = abExceptionWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TITLE_0, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = abExceptionWS.Cells[3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);

           */
        }
    }
}
