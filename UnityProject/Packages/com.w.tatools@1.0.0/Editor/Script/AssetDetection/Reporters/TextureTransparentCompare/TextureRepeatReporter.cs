//using System.IO;
//using System.Collections.Generic;

//using OfficeOpenXml;
//using OfficeOpenXml.Style;

//namespace jj.TATools.Editor
//{
//    /// <summary>
//    /// 重复纹理详情报告
//    /// </summary>
//    internal class TextureRepeatReporter
//    {
//        static int GenerateRepeatReportList(ExcelWorksheet worksheet, int row, string typeShowName, string typeName, Dictionary<string, Dictionary<string, TextureData>> assetDataMapping, Dictionary<string, string> hyperLinkToTotalMapping,
//           System.Drawing.Color CONTENT_DATA_COLOR_0,
//           System.Drawing.Color CONTENT_DATA_COLOR_1,
//           System.Drawing.Color CONTENT_DATA_COLOR_2,
//           System.Drawing.Color CONTENT_DATA_COLOR_3)
//        {
//            // Title /////////////////////////////
//            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
//            ExcelHelper.SetHyperLink(tempExcelRange, worksheet.Name, hyperLinkToTotalMapping[typeName], typeShowName);
//            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);

//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 1);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 3, row + 1, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_REPEAT_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 5];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 6];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 7];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_9, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 8];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_10, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 9,row,21];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_8, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
           
//            // Total /////////////////////////////
//            var currentFilesSizeData = TextureDataParser.CurrentRepeatFilesSizeMapping;
//            var currentFilesGpuSizeData = TextureDataParser.CurrentRepeatFilesGpuSizeMapping;
//            long currentFileSize = 0;
//            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
//            long currentFileGpuSize = 0;
//            if (!currentFilesGpuSizeData.TryGetValue(typeName, out currentFileGpuSize)) currentFileGpuSize = 0;
//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 1);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, "(" + assetDataMapping.Count + " 个)", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 5];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 6];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileGpuSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 7, row, 21];
//            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);

//            // Details /////////////////////////////
//            Dictionary<string, TextureData> tempAssetAddedDic = null;
//            if (TextureDataParser.CurrentVersionAddedMapping != null)
//            {
//                TextureDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAssetAddedDic);
//            }
//            Dictionary<string, TextureData> tempAssetModifiedDic = null;
//            if (TextureDataParser.CurrentVersionModifiedMapping != null)
//            {
//                TextureDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempAssetModifiedDic);
//            }
//            row++;
//            int mergeTotalRow = row;
//            int totalAmount = 0;
//            foreach (var data in assetDataMapping)
//            {
//                int mergeChildRow = row;
//                int childAmount = 0;

//                if (data.Value.Count == 0) continue;

//                foreach (var childData in data.Value)
//                {
//                    var asset = childData.Value;

//                    System.Drawing.Color color = CONTENT_DATA_COLOR_1;
//                    if (tempAssetAddedDic != null && tempAssetAddedDic.ContainsKey(asset.m_TexturePath))
//                    {
//                        color = CONTENT_DATA_COLOR_3;
//                    }
//                    if (tempAssetModifiedDic != null && tempAssetModifiedDic.ContainsKey(asset.m_TexturePath))
//                    {
//                        color = CONTENT_DATA_COLOR_3;
//                    }

//                    ExcelHelper.SetExcelRow(worksheet, row, 1);
//                    tempExcelRange = worksheet.Cells[row, 5];
//                    ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_DiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                         color);
//                    tempExcelRange = worksheet.Cells[row, 6];
//                    ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_GpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                         color);
//                    tempExcelRange = worksheet.Cells[row, 7];
//                    ExcelHelper.SetExcelRange(tempExcelRange, asset.GetResolutionStr(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                         color);
//                    tempExcelRange = worksheet.Cells[row, 8];
//                    ExcelHelper.SetExcelRange(tempExcelRange, asset.GetTransparentPertanageStr(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                         color);
//                    tempExcelRange = worksheet.Cells[row, 9, row, 21];
//                    ExcelHelper.SetExcelRange(tempExcelRange, asset.m_TexturePath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
//                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                         color);
                  
//                    row++;
//                    totalAmount++;
//                    childAmount++;
//                }
//                tempExcelRange = worksheet.Cells[mergeChildRow, 3, mergeChildRow + childAmount - 1, 4];
//                ExcelHelper.SetExcelRange(tempExcelRange, Path.GetFileName(data.Key) + "|" + childAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CONTENT_DATA_COLOR_1);
//            }

//            if (totalAmount >= 1)
//            {
//                tempExcelRange = worksheet.Cells[mergeTotalRow, 2, mergeTotalRow + totalAmount - 1, 2];
//                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CONTENT_DATA_COLOR_1);
//            }

//            return row;
//        }

//        internal static void GenerateReport(ExcelPackage package)
//        {
//            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
//            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
//            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);
//            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
//            System.Drawing.Color SPECIAL_DATA_COLOR_1 = System.Drawing.Color.FromArgb(255, 255, 0);
//            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
//            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);

//            ExcelWorksheet repeatWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_5);
//            repeatWS.View.ShowGridLines = false;
//            repeatWS.Column(1).Width = 5;
//            repeatWS.Column(2).Width = 20;
//            repeatWS.Column(3).Width = 18;
//            repeatWS.Column(4).Width = 18;
//            repeatWS.Column(5).Width = 18;
//            repeatWS.Column(6).Width = 18;
//            repeatWS.Column(7).Width = 18;
//            repeatWS.Column(8).Width = 18;

//            // 当前版本重复资源统计 ///////////////////////////////////////////////////////////
//            ExcelRange tempExcelRange = repeatWS.Cells[1, 1];
//            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 RETURN_COLOR);
//            tempExcelRange = repeatWS.Cells[1, 2, 1, 5];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_REPEAT_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);
//            tempExcelRange = repeatWS.Cells[2, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);
//            tempExcelRange = repeatWS.Cells[2, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);
//            tempExcelRange = repeatWS.Cells[2, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);
//            tempExcelRange = repeatWS.Cells[2, 5];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);

//            // 超链接：当前版本重复资源统计
//            int currentRow = 4;
//            var currentFilesSizeData = TextureDataParser.CurrentRepeatFilesSizeMapping;
//            var currentFilesGpuSizeData = TextureDataParser.CurrentRepeatFilesGpuSizeMapping;
//            var allAssetTypeMapping = TextureDataParser.TextureTypeNameMapping;
//            Dictionary<string, string> hyperLinkToTotalMapping = new Dictionary<string, string>();
//            foreach (var data in allAssetTypeMapping)
//            {
//                var typeName = data.Key;
//                var typeShowName = data.Value;
//                long currentFileSize = 0;
//                if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
//                long currentFileBuildSize = 0;
//                if (!currentFilesGpuSizeData.TryGetValue(typeName, out currentFileBuildSize)) currentFileBuildSize = 0;

//                hyperLinkToTotalMapping[typeName] = "B" + currentRow;

//                if (currentFileSize == 0) continue;

//                currentRow++;
//            }

//            // 图例
//            currentRow++;
//            tempExcelRange = repeatWS.Cells[currentRow, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 System.Drawing.Color.FromArgb(255, 255, 255));
//            currentRow++;
//            tempExcelRange = repeatWS.Cells[currentRow, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_CURRENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 SPECIAL_DATA_COLOR_1);
//            currentRow++;
//            tempExcelRange = repeatWS.Cells[currentRow, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_LAST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_1);

//            // 当前版本重复资源详情 ///////////////////////////////////////////////////////////
//            currentRow += 2;
//            Dictionary<string, string> hyperLinkToDetailsMapping = new Dictionary<string, string>();
//            Dictionary<string, int> typeAmountMapping = new Dictionary<string, int>();
//            var currentTotalFilesMapping = TextureDataParser.CurrentRepeatFilesExceptionMapping;
//            int totalFileAmount = 0;
//            foreach (var data in allAssetTypeMapping)
//            {
//                var typeName = data.Key;
//                var typeShowName = data.Value;
//                Dictionary<string, Dictionary<string, TextureData>> assetDataMapping = null;
//                if (!currentTotalFilesMapping.TryGetValue(typeName, out assetDataMapping)) continue;

//                if (assetDataMapping.Count == 0) continue;

//                typeAmountMapping[typeName] = assetDataMapping.Count;
//                totalFileAmount += assetDataMapping.Count;

//                hyperLinkToDetailsMapping[typeName] = "B" + currentRow;

//                currentRow = GenerateRepeatReportList(repeatWS, currentRow, typeShowName, typeName, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, SPECIAL_DATA_COLOR_1);

//                currentRow++;
//            }
//            // 数据：当前版本重复资源统计 /////////////////////////////////////
//            int detailsRow = 4;
//            long currentTotalFileSize = 0;
//            long currentTotalFileGpuSize = 0;
//            foreach (var data in allAssetTypeMapping)
//            {
//                var typeName = data.Key;
//                var typeShowName = data.Value;
//                long currentFileSize = 0;
//                if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
//                long currentFileGpuSize = 0;
//                if (!currentFilesGpuSizeData.TryGetValue(typeName, out currentFileGpuSize)) currentFileGpuSize = 0;

//                currentTotalFileSize += currentFileSize;
//                currentTotalFileGpuSize += currentFileGpuSize;

//                if (currentFileSize == 0) continue;

//                tempExcelRange = repeatWS.Cells[detailsRow, 2];
//                ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_5, hyperLinkToDetailsMapping[typeName], typeShowName);
//                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CURRENT_DATA_COLOR_1);
//                tempExcelRange = repeatWS.Cells[detailsRow, 3];
//                ExcelHelper.SetExcelRange(tempExcelRange, typeAmountMapping[typeName], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CURRENT_DATA_COLOR_1);
//                tempExcelRange = repeatWS.Cells[detailsRow, 4];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CURRENT_DATA_COLOR_1);
//                tempExcelRange = repeatWS.Cells[detailsRow, 5];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileGpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CURRENT_DATA_COLOR_1);


//                detailsRow++;
//            }

//            tempExcelRange = repeatWS.Cells[3, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_DETAIL_TITLE_0, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 SPECIAL_DATA_COLOR_0);
//            tempExcelRange = repeatWS.Cells[3, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 SPECIAL_DATA_COLOR_0);
//            tempExcelRange = repeatWS.Cells[3, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 SPECIAL_DATA_COLOR_0);
//            tempExcelRange = repeatWS.Cells[3, 5];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentTotalFileGpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 SPECIAL_DATA_COLOR_0);
//        }
//    }
//}
