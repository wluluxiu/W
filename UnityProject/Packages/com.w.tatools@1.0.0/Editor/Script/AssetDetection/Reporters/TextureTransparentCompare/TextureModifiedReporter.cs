using System.Linq;
using System.Collections.Generic;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace jj.TATools.Editor
{
    /// <summary>
    /// 2D资源迭代修改详情报告
    /// </summary>
    internal class TextureModifiedReporter
    {
        static int GenerateModifiedReportList(ExcelWorksheet worksheet, int row, string typeShowName, string typeName, Dictionary<string, TextureData> assetDataMapping, Dictionary<string, string> hyperLinkToTotalMapping,
          System.Drawing.Color CONTENT_DATA_COLOR_0,
          System.Drawing.Color CONTENT_DATA_COLOR_1,
          System.Drawing.Color CONTENT_DATA_COLOR_2)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 19];
            ExcelHelper.SetHyperLink(tempExcelRange, worksheet.Name, hyperLinkToTotalMapping[typeName], typeShowName);
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            row++;
            ExcelHelper.SetExcelRow(worksheet, row, 1);
            tempExcelRange = worksheet.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_9, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_8, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_10, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 11, row, 19];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_8, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Total /////////////////////////////
            var currentFilesSizeData = TextureDataParser.CurrentAfterModifiedFilesSizeMapping;
            var currentFilesGpuSizeData = TextureDataParser.CurrentAfterModifiedFilesGpuSizeMapping;
            var lastFilesSizeData = TextureDataParser.CurrentBeforeModifiedFilesSizeMapping;
            var lastFilesGpuSizeData = TextureDataParser.CurrentBeforeModifiedFilesGpuSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
            long currentFileGpuSize = 0;
            if (!currentFilesGpuSizeData.TryGetValue(typeName, out currentFileGpuSize)) currentFileGpuSize = 0;
            long lastFileSize = 0;
            if (lastFilesSizeData != null && !lastFilesSizeData.TryGetValue(typeName, out lastFileSize)) lastFileSize = 0;
            long lastFileGpuSize = 0;
            if (lastFilesGpuSizeData != null && !lastFilesGpuSizeData.TryGetValue(typeName, out lastFileGpuSize)) lastFileGpuSize = 0;
            row++;
            ExcelHelper.SetExcelRow(worksheet, row, 1);
            tempExcelRange = worksheet.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, "(" + assetDataMapping.Count + " 个)", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);
            tempExcelRange = worksheet.Cells[row, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileGpuSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);
            tempExcelRange = worksheet.Cells[row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastFileSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastFileGpuSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);
            tempExcelRange = worksheet.Cells[row, 9, row, 19];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            tempExcelRange = worksheet.Cells[row, 2, row + assetDataMapping.Count - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            var sortedDataMapping = assetDataMapping.OrderByDescending(o => o.Value.m_GpuSize).ToDictionary(o => o.Key, p => p.Value);
            foreach (var asset in sortedDataMapping.Values)
            {
                ExcelHelper.SetExcelRow(worksheet, row, 1);
                tempExcelRange = worksheet.Cells[row, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(asset.m_DiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = worksheet.Cells[row, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(asset.m_GpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = worksheet.Cells[row, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.GetResolutionStr(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = worksheet.Cells[row, 6];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.GetTransparentPertanageStr(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = worksheet.Cells[row, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(asset.m_DiskOldSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = worksheet.Cells[row, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(asset.m_GpuOldSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = worksheet.Cells[row, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.GetOldResolutionStr(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = worksheet.Cells[row, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.GetTransparentOldPertanageStr(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = worksheet.Cells[row, 11,row,19];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_TexturePath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
              
                row++;
            }

            return row;
        }

        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color TITLE_COLOR = System.Drawing.Color.FromArgb(255, 217, 102);
            System.Drawing.Color UPDATE_DATA_COLOR_0 = System.Drawing.Color.FromArgb(244, 176, 132);
            System.Drawing.Color UPDATE_DATA_COLOR_1 = System.Drawing.Color.FromArgb(252, 228, 214);
            System.Drawing.Color LAST_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color LAST_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);

            ExcelWorksheet currentModifiedWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_4);
            currentModifiedWS.View.ShowGridLines = false;
            currentModifiedWS.Column(1).Width = 5;
            currentModifiedWS.Column(2).Width = 20;
            currentModifiedWS.Column(3).Width = 18;
            currentModifiedWS.Column(4).Width = 18;
            currentModifiedWS.Column(5).Width = 18;
            currentModifiedWS.Column(6).Width = 18;
            currentModifiedWS.Column(7).Width = 18;
            currentModifiedWS.Column(8).Width = 18;
            currentModifiedWS.Column(9).Width = 18;
            currentModifiedWS.Column(10).Width = 18;

            // 当前版本资源删除统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = currentModifiedWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = currentModifiedWS.Cells[1, 2, 1, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TITLE_COLOR);
            tempExcelRange = currentModifiedWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TITLE_COLOR);
            tempExcelRange = currentModifiedWS.Cells[2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_7, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TITLE_COLOR);
            tempExcelRange = currentModifiedWS.Cells[2, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_1, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[2, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_2, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[2, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 LAST_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[2, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 LAST_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[2, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_5, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[2, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_6, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR_0);

            // 超链接：当前版本资源修改统计
            int currentRow = 4;
            var currentFilesSizeData = TextureDataParser.CurrentAfterModifiedFilesSizeMapping;
            var currentFilesBuildSizeData = TextureDataParser.CurrentAfterModifiedFilesGpuSizeMapping;
            var lastFilesSizeData = TextureDataParser.CurrentBeforeModifiedFilesSizeMapping;
            var lastFilesBuildSizeData = TextureDataParser.CurrentBeforeModifiedFilesGpuSizeMapping;
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
            // 当前版本资源修改详情 ///////////////////////////////////////////////////////////
            currentRow++;
            Dictionary<string, string> hyperLinkToDetailsMapping = new Dictionary<string, string>();
            Dictionary<string, int> typeAmountMapping = new Dictionary<string, int>();
            var currentTotalFilesMapping = TextureDataParser.CurrentVersionModifiedMapping;
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

                currentRow = GenerateModifiedReportList(currentModifiedWS, currentRow, typeShowName, typeName, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0);

                currentRow++;
            }

            // 数据：当前版本资源修改统计 /////////////////////////////////////
            int detailsRow = 4;
            long currentTotalFileSize = 0;
            long currentTotalFileGpuSize = 0;
            long lastTotalFileSize = 0;
            long lastTotalFileGpuSize = 0;
            foreach (var data in allAssetTypeMapping)
            {
                var typeName = data.Key;
                var typeShowName = data.Value;
                long currentFileSize = 0;
                if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
                long currentFileGpuSize = 0;
                if (!currentFilesBuildSizeData.TryGetValue(typeName, out currentFileGpuSize)) currentFileGpuSize = 0;
                long lastFileSize = 0;
                if (!lastFilesSizeData.TryGetValue(typeName, out lastFileSize)) lastFileSize = 0;
                long lastFileGpuSize = 0;
                if (!lastFilesBuildSizeData.TryGetValue(typeName, out lastFileGpuSize)) lastFileGpuSize = 0;

                currentTotalFileSize += currentFileSize;
                currentTotalFileGpuSize += currentFileGpuSize;
                lastTotalFileSize += lastFileSize;
                lastTotalFileGpuSize += lastFileGpuSize;

                if (currentFileSize == 0) continue;

                tempExcelRange = currentModifiedWS.Cells[detailsRow, 2];
                ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_4, hyperLinkToDetailsMapping[typeName], typeShowName);
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     TITLE_COLOR);
                tempExcelRange = currentModifiedWS.Cells[detailsRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, typeAmountMapping[typeName], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     TITLE_COLOR);
                tempExcelRange = currentModifiedWS.Cells[detailsRow, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = currentModifiedWS.Cells[detailsRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileGpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = currentModifiedWS.Cells[detailsRow, 6];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     LAST_DATA_COLOR_1);
                tempExcelRange = currentModifiedWS.Cells[detailsRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastFileGpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     LAST_DATA_COLOR_1);
                tempExcelRange = currentModifiedWS.Cells[detailsRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileSize - lastFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     UPDATE_DATA_COLOR_1);
                tempExcelRange = currentModifiedWS.Cells[detailsRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileGpuSize - lastFileGpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     UPDATE_DATA_COLOR_1);

                detailsRow++;
            }

            tempExcelRange = currentModifiedWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_DETAIL_TITLE_0, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[3, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[3, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileGpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[3, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[3, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastTotalFileGpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[3, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize - lastTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentModifiedWS.Cells[3, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileGpuSize - lastTotalFileGpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
        }
    }
}
