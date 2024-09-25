using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace jj.TATools.Editor
{
    /// <summary>
    /// 整体统计以及报告目录
    /// </summary>
    internal class AssetTotalContentReporter
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
            System.Drawing.Color SPECIAL_DATA_COLOR_1 = System.Drawing.Color.FromArgb(255, 0, 0);
            System.Drawing.Color SPECIAL_DATA_COLOR_2 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color SPECIAL_DATA_COLOR_3 = System.Drawing.Color.FromArgb(255, 255, 255);

            ExcelWorksheet totalContentWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_0);
            totalContentWS.View.ShowGridLines = false;
            totalContentWS.Column(1).Width = 5;
            totalContentWS.Column(2).Width = 20;
            totalContentWS.Column(3).Width = 20;
            totalContentWS.Column(4).Width = 15;
            totalContentWS.Column(5).Width = 20;
            totalContentWS.Column(6).Width = 20;
            totalContentWS.Column(7).Width = 15;
            totalContentWS.Column(8).Width = 20;
            totalContentWS.Column(9).Width = 20;
            totalContentWS.Column(10).Width = 15;
            totalContentWS.Column(11).Width = 15;
            totalContentWS.Column(12).Width = 15;
            totalContentWS.Column(13).Width = 15;
            totalContentWS.Column(14).Width = 15;
            totalContentWS.Column(15).Width = 15;

            // 当前版本资源统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = totalContentWS.Cells[1, 2, 1, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 2,2,3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            // 原始版本资源统计 ///////////////////////////////////////////////////////////
            tempExcelRange = totalContentWS.Cells[1, 5, 1, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 LAST_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 5,2,6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 LAST_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 LAST_DATA_COLOR_0);

            // 资源增量统计 ///////////////////////////////////////////////////////////
            tempExcelRange = totalContentWS.Cells[1, 8, 1, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 8,2,9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[2, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR_0);

            // 数据：当前版本资源统计&原始版本资源统计&资源增量统计 ///////////////////////////////////////////////////////////
            int contentRow = 4;
            var currentFilesSizeData = AssetRecordDataParser.CurrentFilesSizeMapping;
            var lastFilesSizeData = AssetRecordDataParser.LastFilesSizeMapping;
            var allAssetTypeMapping = AssetRecordDataParser.AssetTypeMapping;
            long currentTotalFileSize = 0;
            long lastTotalFileSize = 0;

            foreach (var assetType in allAssetTypeMapping)
            {
                var typeShowName = assetType.ToString();
                long currentFileSize = 0;
                if (!currentFilesSizeData.TryGetValue(assetType, out currentFileSize)) currentFileSize = 0;
                long lastFileSize = 0;
                if (lastFilesSizeData!= null && !lastFilesSizeData.TryGetValue(assetType, out lastFileSize)) lastFileSize = 0;

                long updateFileSize = currentFileSize - lastFileSize;

                currentTotalFileSize += currentFileSize;
                lastTotalFileSize += lastFileSize;

                if (currentFileSize == 0 && lastFileSize == 0) continue;

                tempExcelRange = totalContentWS.Cells[contentRow, 2,contentRow,3];
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);

                tempExcelRange = totalContentWS.Cells[contentRow, 5,contentRow,6];
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     LAST_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     LAST_DATA_COLOR_1);

                tempExcelRange = totalContentWS.Cells[contentRow, 8,contentRow,9];
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     UPDATE_DATA_COLOR_1);
                tempExcelRange = totalContentWS.Cells[contentRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(updateFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     UPDATE_DATA_COLOR_1);

                contentRow++;
            }

            tempExcelRange = totalContentWS.Cells[3, 2,3,3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TITLE_0 + ":" + AssetRecordDataParser.CurrentTotalFileAmount + " 个", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[3, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);

            tempExcelRange = totalContentWS.Cells[3, 5,3,6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TITLE_0 + ":" + AssetRecordDataParser.LastTotalFileAmount + " 个", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[3, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(lastTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);

            tempExcelRange = totalContentWS.Cells[3, 8,3,9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TITLE_0 + ":" + (AssetRecordDataParser.CurrentTotalFileAmount - AssetRecordDataParser.LastTotalFileAmount) + " 个", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = totalContentWS.Cells[3, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize - lastTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);

            // 异常资源统计 ///////////////////////////////////////////////////////////
            contentRow ++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 17];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_1);
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_1);
            tempExcelRange = totalContentWS.Cells[contentRow, 4, contentRow, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_1);
            tempExcelRange = totalContentWS.Cells[contentRow, 6, contentRow, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_1);
            tempExcelRange = totalContentWS.Cells[contentRow, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_1);
            tempExcelRange = totalContentWS.Cells[contentRow, 9,contentRow,17];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_1);

            var exceptionDataMapping = AssetRecordDataParser.ExceptionMapping;
            var exceptionTips = AssetRecordDataParser.ExceptionTips;
            var repeatFileMapping = AssetRecordDataParser.CurrentRepeatFilesExceptionMapping;
            Dictionary<string, Dictionary<string, BaseRecorder>> repeatFileByTypeMapping = null;
            var assetsInResourcesFolderMapping = AssetRecordDataParser.FileInResourcesFolderMapping;
            Dictionary<string, BaseRecorder> assetsInResourcesFolderByTypeMapping = null;
            int colorIndex = 0;
            if (exceptionDataMapping != null && exceptionDataMapping.Count > 0)
            {
                contentRow++;
                foreach (var data in exceptionDataMapping)
                {
                    var assetType = data.Key.ToString();
                    var exceptionMapping = data.Value;
                    repeatFileMapping.TryGetValue(data.Key, out repeatFileByTypeMapping);
                    assetsInResourcesFolderMapping.TryGetValue(data.Key, out assetsInResourcesFolderByTypeMapping);

                    if (exceptionMapping.Count == 0 && 
                        (repeatFileByTypeMapping == null || repeatFileByTypeMapping.Count == 0) &&
                        (assetsInResourcesFolderByTypeMapping == null || assetsInResourcesFolderByTypeMapping.Count == 0)) continue;

                    System.Drawing.Color itemColor = (colorIndex % 2 == 0) ? SPECIAL_DATA_COLOR_3 : SPECIAL_DATA_COLOR_2;
                    colorIndex++;

                    int addRow = exceptionMapping.Count - 1;
                    if (repeatFileByTypeMapping != null && repeatFileByTypeMapping.Count > 0)
                        addRow++;
                    if (assetsInResourcesFolderByTypeMapping != null && assetsInResourcesFolderByTypeMapping.Count > 0)
                        addRow++;

                    tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow + addRow, 3];
                    ExcelHelper.SetExcelRange(tempExcelRange, assetType, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         itemColor);
                    foreach (var childData in exceptionMapping)
                    {
                        var exceptionType = childData.Key;
                        var amount = childData.Value;

                        string[] exceptDes = null;
                        exceptionTips.TryGetValue(exceptionType, out exceptDes);

                        tempExcelRange = totalContentWS.Cells[contentRow, 4, contentRow, 5];
                        var tempStr = (exceptDes == null || exceptDes.Length == 0) ? "" : exceptDes[0];
                        ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempStr = (exceptDes == null || exceptDes.Length < 1) ? "" : exceptDes[1];
                        tempExcelRange = totalContentWS.Cells[contentRow, 6, contentRow, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = totalContentWS.Cells[contentRow, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, amount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempStr = (exceptDes == null || exceptDes.Length < 2) ? "" : exceptDes[2];
                        tempExcelRange = totalContentWS.Cells[contentRow, 9,contentRow,17];
                        ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
 
                        contentRow++;
                    }
                    // Repeat
                    if (repeatFileByTypeMapping != null && repeatFileByTypeMapping.Count > 0)
                    {
                        string[] exceptDes = null;
                        exceptionTips.TryGetValue(ExceptionType.REPEAT_FILE, out exceptDes);

                        tempExcelRange = totalContentWS.Cells[contentRow, 4, contentRow, 5];
                        var tempStr = (exceptDes == null || exceptDes.Length == 0) ? "" : exceptDes[0];
                        ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempStr = (exceptDes == null || exceptDes.Length < 1) ? "" : exceptDes[1];
                        tempExcelRange = totalContentWS.Cells[contentRow, 6, contentRow, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = totalContentWS.Cells[contentRow, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, repeatFileByTypeMapping.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempStr = (exceptDes == null || exceptDes.Length < 2) ? "" : exceptDes[2];
                        tempExcelRange = totalContentWS.Cells[contentRow, 9, contentRow, 17];
                        ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);

                        contentRow++;
                    }
                    // Resoruces Folder
                    if (assetsInResourcesFolderByTypeMapping != null && assetsInResourcesFolderByTypeMapping.Count > 0)
                    {
                        string[] exceptDes = null;
                        exceptionTips.TryGetValue(ExceptionType.RESOURCES_FOLDER, out exceptDes);

                        tempExcelRange = totalContentWS.Cells[contentRow, 4, contentRow, 5];
                        var tempStr = (exceptDes == null || exceptDes.Length == 0) ? "" : exceptDes[0];
                        ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempStr = (exceptDes == null || exceptDes.Length < 1) ? "" : exceptDes[1];
                        tempExcelRange = totalContentWS.Cells[contentRow, 6,contentRow,7];
                        ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = totalContentWS.Cells[contentRow, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, assetsInResourcesFolderByTypeMapping.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempStr = (exceptDes == null || exceptDes.Length < 2) ? "" : exceptDes[2];
                        tempExcelRange = totalContentWS.Cells[contentRow, 9, contentRow, 17];
                        ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);

                        contentRow++;
                    }
                }
            }
            // Assetbunle Exception
            int assetbunleExceptionStartRow = contentRow;
            int abExceptionAddRow = 0;
            System.Drawing.Color assetbunleItemColor = (colorIndex % 2 == 0) ? SPECIAL_DATA_COLOR_3 : SPECIAL_DATA_COLOR_2;
            colorIndex++;
            if (AssetRecordDataParser.AssetbunleStrategyExceptionAmount > 0)
            {
                abExceptionAddRow++;
                string[] exceptDes = null;
                exceptionTips.TryGetValue(ExceptionType.ASSETBUNDLE_STRATEGY, out exceptDes);

                tempExcelRange = totalContentWS.Cells[contentRow, 4, contentRow, 5];
                var tempStr = (exceptDes == null || exceptDes.Length == 0) ? "" : exceptDes[0];
                ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     assetbunleItemColor);
                tempStr = (exceptDes == null || exceptDes.Length < 1) ? "" : exceptDes[1];
                tempExcelRange = totalContentWS.Cells[contentRow, 6, contentRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     assetbunleItemColor);
                tempExcelRange = totalContentWS.Cells[contentRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetRecordDataParser.AssetbunleStrategyExceptionAmount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     assetbunleItemColor);
                tempStr = (exceptDes == null || exceptDes.Length < 2) ? "" : exceptDes[2];
                tempExcelRange = totalContentWS.Cells[contentRow, 9, contentRow, 17];
                ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     assetbunleItemColor);

                contentRow++;
            }
            // Assetbunle Builtin
            if (AssetRecordDataParser.AssetbunleBuiltinExceptionMapping.Count > 0)
            {
                abExceptionAddRow++;
                string[] exceptDes = null;
                exceptionTips.TryGetValue(ExceptionType.ASSETBUNDLE_BUILTIN, out exceptDes);

                tempExcelRange = totalContentWS.Cells[contentRow, 4, contentRow, 5];
                var tempStr = (exceptDes == null || exceptDes.Length == 0) ? "" : exceptDes[0];
                ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     assetbunleItemColor);
                tempStr = (exceptDes == null || exceptDes.Length < 1) ? "" : exceptDes[1];
                tempExcelRange = totalContentWS.Cells[contentRow, 6, contentRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     assetbunleItemColor);
                tempExcelRange = totalContentWS.Cells[contentRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetRecordDataParser.AssetbunleBuiltinExceptionMapping.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     assetbunleItemColor);
                tempStr = (exceptDes == null || exceptDes.Length < 2) ? "" : exceptDes[2];
                tempExcelRange = totalContentWS.Cells[contentRow, 9, contentRow, 17];
                ExcelHelper.SetExcelRange(tempExcelRange, tempStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     assetbunleItemColor);

                contentRow++;
            }
            // Asestbunle Exception Type
            if (abExceptionAddRow > 0)
            {
                tempExcelRange = totalContentWS.Cells[assetbunleExceptionStartRow, 2, assetbunleExceptionStartRow + abExceptionAddRow - 1, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, "Assetbundle", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     assetbunleItemColor);
            }
            // 详情报告目录 ///////////////////////////////////////////////////////////
            contentRow ++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            //当前版本资源详情报告
            contentRow++;
            int reportIndex = 1;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_1, "A1",reportIndex +". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_1);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_1, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //资源迭代新增详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_2, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_2);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_2, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //资源迭代删除详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_3, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_3);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //资源迭代修改详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_4, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_4);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //重复资源详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_5, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_5);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_5, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //冗余资源详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_6, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_6);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_6, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //纹理异常详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_7, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_7);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_7, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //模型异常详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_8, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_8);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_8, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //材质异常详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_9, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_9);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_9, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //Shader异常详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_10, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_10);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_10, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //脚本异常引用详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_11, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_11);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_11, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //内置资源引用详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_12, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_12);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_12, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //Prefab异常详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_13, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_13);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_13, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //Scene异常详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_14, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_14);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_14, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //分包策略异常详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_15, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_15);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_15, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //Assetbunle内置资源详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_16, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_16);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_16, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            reportIndex++;
            //Resoruces目录资源详情报告
            contentRow++;
            tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_17, "A1", reportIndex + ". " + ConstDefine.REPORT_ASSETCOMPARE_SHEET_17);
            ExcelHelper.SetExcelRange(tempExcelRange, reportIndex + "." + ConstDefine.REPORT_ASSETCOMPARE_SHEET_17, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            //定制文件夹详情报告
            //contentRow++;
            //tempExcelRange = totalContentWS.Cells[contentRow, 2, contentRow, 3];
            //ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_14, "A1", ConstDefine.REPORT_ASSETCOMPARE_SHEET_14);
            //ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_14, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
            //     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
            //     CONTENT_DATA_COLOR_1);

        }
    }
}
