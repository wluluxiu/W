using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace jj.TATools.Editor
{
    /// <summary>
    /// Texture统计以及报告目录
    /// </summary>
    internal class TextureTotalContentReporter
    {
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color UPDATE_DATA_COLOR_0 = System.Drawing.Color.FromArgb(244, 176, 132);
            System.Drawing.Color UPDATE_DATA_COLOR_1 = System.Drawing.Color.FromArgb(252, 228, 214);
            System.Drawing.Color LAST_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color LAST_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(255, 192, 0);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(252, 227, 140);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);

            ExcelWorksheet totalContentWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_0);
            totalContentWS.View.ShowGridLines = false;
            totalContentWS.Column(1).Width = 5;
            totalContentWS.Column(2).Width = 20;
            totalContentWS.Column(3).Width = 18;
            totalContentWS.Column(4).Width = 18;
            totalContentWS.Column(5).Width = 20;
            totalContentWS.Column(6).Width = 18;
            totalContentWS.Column(7).Width = 18;
            totalContentWS.Column(8).Width = 20;
            totalContentWS.Column(9).Width = 18;
            totalContentWS.Column(10).Width = 18;

            // 当前版本资源统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = totalContentWS.Cells[1, 2, 1, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_5, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            // 原始版本资源统计 ///////////////////////////////////////////////////////////
            tempExcelRange = totalContentWS.Cells[1, 5, 1, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 LAST_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 LAST_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 LAST_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_5, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 LAST_DATA_COLOR_0);

            // 资源增量统计 ///////////////////////////////////////////////////////////
            tempExcelRange = totalContentWS.Cells[1, 8, 1, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_5, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR_0);

            // 数据：当前版本资源统计&原始版本资源统计&资源增量统计 ///////////////////////////////////////////////////////////
            int contentRow = 4;
            var currentFilesSizeData = TextureDataParser.CurrentFilesSizeMapping;
            var currentFilesBuildSizeData = TextureDataParser.CurrentFilesGpuSizeMapping;
            var lastFilesSizeData = TextureDataParser.LastFilesSizeMapping;
            var lastFilesGpuSizeData = TextureDataParser.LastFilesGpuSizeMapping;
            var allAssetTypeMapping = TextureDataParser.TextureTypeNameMapping;
            long currentTotalFileSize = 0;
            long currentTotalFileBuildSize = 0;
            long lastTotalFileSize = 0;
            long lastTotalFileBuildSize = 0;

            foreach (var data in allAssetTypeMapping)
            {
                var typeName = data.Key;
                var typeShowName = data.Value;
                long currentFileSize = 0;
                if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
                long currentFileGpuSize = 0;
                if (!currentFilesBuildSizeData.TryGetValue(typeName, out currentFileGpuSize)) currentFileGpuSize = 0;
                long lastFileSize = 0;
                if (lastFilesSizeData != null &&!lastFilesSizeData.TryGetValue(typeName, out lastFileSize)) lastFileSize = 0;
                long lastFileGpuSize = 0;
                if (lastFilesGpuSizeData != null && !lastFilesGpuSizeData.TryGetValue(typeName, out lastFileGpuSize)) lastFileGpuSize = 0;
                long updateFileSize = currentFileSize - lastFileSize;
                long updateBuildSize = currentFileGpuSize - lastFileGpuSize;

                currentTotalFileSize += currentFileSize;
                currentTotalFileBuildSize += currentFileGpuSize;
                lastTotalFileSize += lastFileSize;
                lastTotalFileBuildSize += lastFileGpuSize;

                if (currentFileSize == 0 && lastFileSize == 0) continue;

                tempExcelRange = totalContentWS.Cells[contentRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileGpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);

                tempExcelRange = totalContentWS.Cells[contentRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     LAST_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 6];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     LAST_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastFileGpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     LAST_DATA_COLOR_1);

                tempExcelRange = totalContentWS.Cells[contentRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     UPDATE_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(updateFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     UPDATE_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(updateBuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     UPDATE_DATA_COLOR_1);

                contentRow++;
            }

            tempExcelRange = totalContentWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_DETAIL_TITLE_0 + ":" + TextureDataParser.CurrentTotalFileAmount + " 个", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[3, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileBuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);

            tempExcelRange = totalContentWS.Cells[3, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_DETAIL_TITLE_0 + ":" + TextureDataParser.LastTotalFileAmount + " 个", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[3, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[3, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastTotalFileBuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);

            tempExcelRange = totalContentWS.Cells[3, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_DETAIL_TITLE_0 + ":" + (TextureDataParser.CurrentTotalFileAmount - TextureDataParser.LastTotalFileAmount) + " 个", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[3, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize - lastTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[3, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileBuildSize - lastTotalFileBuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);

            // 透明度占比统计 1 ///////////////////////////////////////////////////////////
            contentRow += 2;
            int tpStartRow = contentRow;
            tempExcelRange = totalContentWS.Cells[contentRow, 2,contentRow, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.TRANSPARNET_PERCENTAGE_TITLE_0 + " ①", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            contentRow ++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.TRANSPARNET_PERCENTAGE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[contentRow, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.TRANSPARNET_PERCENTAGE_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[contentRow, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.TRANSPARNET_PERCENTAGE_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            var tpDataMaping = TextureDataParser.CurrentVersionTransparentCountMapping1;
            var tpShowMapping = TextureDataParser.TP_SHHOW_LIST_1;
            var totalCount = TextureDataParser.CurrentTotalFileAmount;
            foreach (var key in tpShowMapping)
            {
                int amount = 0;
                tpDataMaping.TryGetValue(key, out amount);

                contentRow++;
                tempExcelRange = totalContentWS.Cells[contentRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, key, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                      ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                      CURRENT_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, amount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                      ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                      CURRENT_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, (amount * 100.0f / totalCount).ToString("F2") + " %", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                      ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                      CURRENT_DATA_COLOR_1);
            }

            // 透明度占比统计 2 ///////////////////////////////////////////////////////////
            tempExcelRange = totalContentWS.Cells[tpStartRow, 6, tpStartRow, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.TRANSPARNET_PERCENTAGE_TITLE_0 + " ②", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tpStartRow++;
            tempExcelRange = totalContentWS.Cells[tpStartRow, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.TRANSPARNET_PERCENTAGE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[tpStartRow, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.TRANSPARNET_PERCENTAGE_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[tpStartRow, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.TRANSPARNET_PERCENTAGE_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            tpDataMaping = TextureDataParser.CurrentVersionTransparentCountMapping2;
            tpShowMapping = TextureDataParser.TP_SHHOW_LIST_2;
            foreach (var key in tpShowMapping)
            {
                int amount = 0;
                tpDataMaping.TryGetValue(key, out amount);

                tpStartRow++;
                tempExcelRange = totalContentWS.Cells[tpStartRow, 6];
                ExcelHelper.SetExcelRange(tempExcelRange, key, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                      ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                      CURRENT_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[tpStartRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, amount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                      ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                      CURRENT_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[tpStartRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, (amount * 100.0f / totalCount).ToString("F2") + " %", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                      ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                      CURRENT_DATA_COLOR_1);
            }

            // 详情报告目录 ///////////////////////////////////////////////////////////
            contentRow += 3;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            //当前版本Texture详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_1, "A1", ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_1);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_1, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            //Texture迭代新增详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_2, "A1", ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_2);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_2, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            //Texture迭代删除详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_3, "A1", ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_3);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            //Texture迭代修改详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_4, "A1", ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_4);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            //重复Texture详情报告
            //contentRow++;
            //tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            //ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_5, "A1", ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_5);
            //ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_5, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
            //     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
            //     CONTENT_DATA_COLOR_1);
            //Texture透明度占比详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_6, "A1", ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_6);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_6, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
        }
    }
}
