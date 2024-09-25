using System.Linq;
using System.Collections.Generic;

using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace jj.TATools.Editor
{
    /// <summary>
    /// Texture迭代新增详情报告
    /// </summary>
    internal class TextureNewerReporter
    {
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);

            ExcelWorksheet currentAddedWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_2);
            currentAddedWS.View.ShowGridLines = false;
            currentAddedWS.Column(1).Width = 5;
            currentAddedWS.Column(2).Width = 20;
            currentAddedWS.Column(3).Width = 18;
            currentAddedWS.Column(4).Width = 18;
            currentAddedWS.Column(5).Width = 18;
            currentAddedWS.Column(6).Width = 18;
            currentAddedWS.Column(7).Width = 18;

            // 当前版本Texture新增统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = currentAddedWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = currentAddedWS.Cells[1, 2, 1, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_ADDED_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = currentAddedWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = currentAddedWS.Cells[2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_7, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = currentAddedWS.Cells[2, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = currentAddedWS.Cells[2, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_5, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            // 超链接：当前版本Texture新增统计
            int currentRow = 4;
            var currentFilesSizeData = TextureDataParser.CurrentAddedFilesSizeMapping;
            var currentFilesBuildSizeData = TextureDataParser.CurrentAddedFilesGpuSizeMapping;
            var allAssetTypeMapping = TextureDataParser.TextureTypeNameMapping;
            Dictionary<string, string> hyperLinkToTotalMapping = new Dictionary<string, string>();
            foreach (var data in allAssetTypeMapping)
            {
                var typeName = data.Key;
                var typeShowName = data.Value;
                long currentFileSize = 0;
                if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
                long currentFileBuildSize = 0;
                if (!currentFilesBuildSizeData.TryGetValue(typeName, out currentFileBuildSize)) currentFileBuildSize = 0;

                hyperLinkToTotalMapping[typeName] = "B" + currentRow;

                if (currentFileSize == 0) continue;

                currentRow++;
            }
            // 当前版本Texture新增详情 ///////////////////////////////////////////////////////////
            currentRow++;
            Dictionary<string, string> hyperLinkToDetailsMapping = new Dictionary<string, string>();
            Dictionary<string, int> typeAmountMapping = new Dictionary<string, int>();
            var currentTotalFilesMapping = TextureDataParser.CurrentVersionAddedMapping;
            int totalFileAmount = 0;
            foreach (var data in allAssetTypeMapping)
            {
                var typeName = data.Key;
                var typeShowName = data.Value;
                Dictionary<string, TextureData> assetDataMapping = null;
                if (!currentTotalFilesMapping.TryGetValue(typeName, out assetDataMapping)) continue;

                if (assetDataMapping.Count == 0) continue;

                typeAmountMapping[typeName] = assetDataMapping.Count;
                totalFileAmount += assetDataMapping.Count;

                hyperLinkToDetailsMapping[typeName] = "B" + currentRow;

                assetDataMapping = assetDataMapping.OrderByDescending(o => o.Value.m_GpuSize).ToDictionary(o => o.Key, p => p.Value);

                currentRow = Asset2DReportUtils.GenerateCommon2DReportList(currentAddedWS, currentRow, typeShowName, typeName, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, currentFilesBuildSizeData);

                currentRow++;
            }

            // 数据：当前版本Texture新增统计 /////////////////////////////////////
            int detailsRow = 4;
            long currentTotalFileSize = 0;
            long currentTotalFileBuildSize = 0;
            foreach (var data in allAssetTypeMapping)
            {
                var typeName = data.Key;
                var typeShowName = data.Value;
                long currentFileSize = 0;
                if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
                long currentFileBuildSize = 0;
                if (!currentFilesBuildSizeData.TryGetValue(typeName, out currentFileBuildSize)) currentFileBuildSize = 0;

                currentTotalFileSize += currentFileSize;
                currentTotalFileBuildSize += currentFileBuildSize;

                if (currentFileSize == 0) continue;

                tempExcelRange = currentAddedWS.Cells[detailsRow, 2];
                ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_2, hyperLinkToDetailsMapping[typeName], typeShowName);
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = currentAddedWS.Cells[detailsRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, typeAmountMapping[typeName], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = currentAddedWS.Cells[detailsRow, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = currentAddedWS.Cells[detailsRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileBuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);

                detailsRow++;
            }

            tempExcelRange = currentAddedWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_DETAIL_TITLE_0, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentAddedWS.Cells[3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentAddedWS.Cells[3, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentAddedWS.Cells[3, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileBuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
        }
    }
}
