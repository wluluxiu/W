//using System.Collections.Generic;

//using OfficeOpenXml;
//using OfficeOpenXml.Style;

//namespace jj.TATools.Editor
//{
//    /// <summary>
//    /// 定制文件夹详情报告
//    /// </summary>
//    internal class AssetCustomDirRuleReporter
//    {
//        static int GenerateCustomRuleDefaultReport(ExcelWorksheet worksheet, int row, string typeShowName, string typeName, Dictionary<string, AssetData> assetDataMapping,
//          System.Drawing.Color CONTENT_DATA_COLOR_0,
//          System.Drawing.Color CONTENT_DATA_COLOR_1,
//          System.Drawing.Color CONTENT_DATA_COLOR_2,
//          Dictionary<string, long> filesSizeMapping,
//          Dictionary<string, long> filesBuildSizeMapping,
//          bool needShowCurrentVersion = false)
//        {
//            // Title /////////////////////////////
//            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "  " + typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_0);

//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 2);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 5];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 6];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 7, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_0);

//            // Total /////////////////////////////
//            var currentFilesSizeData = filesSizeMapping;
//            var currentFilesBuildSizeData = filesBuildSizeMapping;
//            long currentFileSize = 0;
//            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
//            long currentFileBuildSize = 0;
//            if (!currentFilesBuildSizeData.TryGetValue(typeName, out currentFileBuildSize)) currentFileBuildSize = 0;
//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 2);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, "(" + assetDataMapping.Count + " 个)", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileBuildSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 5, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_2);

//            // Details /////////////////////////////
//            row++;
//            tempExcelRange = worksheet.Cells[row, 2, row + assetDataMapping.Count - 1, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
//                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                    CONTENT_DATA_COLOR_1);
//            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
//            foreach (var asset in assetDataMapping.Values)
//            {
//                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
//                if (needShowCurrentVersion)
//                {
//                    Dictionary<string, AssetData> tempAddedDic = null;
//                    if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
//                    {
//                        if (tempAddedDic.ContainsKey(asset.m_AssetPath))
//                            itemColor = currentVersionColor;
//                    }
//                    Dictionary<string, AssetData> tempModifiedDic = null;
//                    if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
//                    {
//                        if (tempModifiedDic.ContainsKey(asset.m_AssetPath))
//                            itemColor = currentVersionColor;
//                    }
//                }

//                ExcelHelper.SetExcelRow(worksheet, row, 2);
//                tempExcelRange = worksheet.Cells[row, 3];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_DiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                        ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                        itemColor);
//                tempExcelRange = worksheet.Cells[row, 4];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_BuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                        ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                        itemColor);
//                tempExcelRange = worksheet.Cells[row, 5];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                        ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                        itemColor);
//                tempExcelRange = worksheet.Cells[row, 6];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_ReferenceCount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                        ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                        itemColor);
//                tempExcelRange = worksheet.Cells[row, 7, row, 15];
//                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
//                        ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                        itemColor);

//                row++;
//            }

//            return row;
//        }

//        static int GenerateCustomRuleModelReport(ExcelWorksheet worksheet, int row, string typeShowName, string typeName, Dictionary<string, AssetData> assetDataMapping,
//           System.Drawing.Color CONTENT_DATA_COLOR_0,
//           System.Drawing.Color CONTENT_DATA_COLOR_1,
//           System.Drawing.Color CONTENT_DATA_COLOR_2,
//            Dictionary<string, long> filesSizeMapping,
//            Dictionary<string, long> filesBuildSizeMapping,
//            bool needShowCurrentVersion = false)
//        {
//            // Title /////////////////////////////
//            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "  " + typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);

//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 2);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 5];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 6];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 7];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 8];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 9];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 10];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 11];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 12];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 13];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 14];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_8, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            // Total /////////////////////////////
//            var currentFilesSizeData = filesSizeMapping;
//            var currentFilesBuildSizeData = filesBuildSizeMapping;
//            long currentFileSize = 0;
//            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
//            long currentFileBuildSize = 0;
//            if (!currentFilesBuildSizeData.TryGetValue(typeName, out currentFileBuildSize)) currentFileBuildSize = 0;
//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 2);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, "(" + assetDataMapping.Count + " 个)", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileBuildSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 5, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);

//            // Details /////////////////////////////
//            row++;
//            tempExcelRange = worksheet.Cells[row, 2, row + assetDataMapping.Count - 1, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_1);
//            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
//            foreach (var asset in assetDataMapping.Values)
//            {
//                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
//                if (needShowCurrentVersion)
//                {
//                    Dictionary<string, AssetData> tempAddedDic = null;
//                    if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
//                    {
//                        if (tempAddedDic.ContainsKey(asset.m_AssetPath))
//                            itemColor = currentVersionColor;
//                    }
//                    Dictionary<string, AssetData> tempModifiedDic = null;
//                    if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
//                    {
//                        if (tempModifiedDic.ContainsKey(asset.m_AssetPath))
//                            itemColor = currentVersionColor;
//                    }
//                }
//                ExcelHelper.SetExcelRow(worksheet, row, 2);
//                tempExcelRange = worksheet.Cells[row, 3];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_DiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 4];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_BuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 5];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 6];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_ReferenceCount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 7];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_MeshRW ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 8];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_Triangle, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 9];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_VertexCount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 10];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_BoneCount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 11];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_HasNormals ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 12];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_HasTangents ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 13];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_HasColors ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 14];
//                string uvInfoStr = asset.GetUVInfo();
//                ExcelHelper.SetExcelRange(tempExcelRange, uvInfoStr == null ? "无" : uvInfoStr, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 15];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.GetModelClipInfo(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);


//                row++;
//            }

//            return row;
//        }

//        static int GenerateCustomRuleTextureReport(ExcelWorksheet worksheet, int row, string typeShowName, string typeName, Dictionary<string, AssetData> assetDataMapping,
//           System.Drawing.Color CONTENT_DATA_COLOR_0,
//           System.Drawing.Color CONTENT_DATA_COLOR_1,
//           System.Drawing.Color CONTENT_DATA_COLOR_2,
//            Dictionary<string, long> filesSizeMapping,
//            Dictionary<string, long> filesBuildSizeMapping,
//            bool needShowCurrentVersion = false)
//        {
//            // Title /////////////////////////////
//            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "  " + typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);

//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 2);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 5];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 6];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 7];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 8];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 9];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 10];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 11];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 12];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 13];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 14, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            // Total /////////////////////////////
//            var currentFilesSizeData = filesSizeMapping;
//            var currentFilesBuildSizeData = filesBuildSizeMapping;
//            long currentFileSize = 0;
//            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
//            long currentFileBuildSize = 0;
//            if (!currentFilesBuildSizeData.TryGetValue(typeName, out currentFileBuildSize)) currentFileBuildSize = 0;
//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 2);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, "(" + assetDataMapping.Count + " 个)", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileBuildSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 5, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);

//            // Details /////////////////////////////
//            row++;
//            tempExcelRange = worksheet.Cells[row, 2, row + assetDataMapping.Count - 1, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_1);
//            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
//            foreach (var asset in assetDataMapping.Values)
//            {
//                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
//                if (needShowCurrentVersion)
//                {
//                    Dictionary<string, AssetData> tempAddedDic = null;
//                    if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
//                    {
//                        if (tempAddedDic.ContainsKey(asset.m_AssetPath))
//                            itemColor = currentVersionColor;
//                    }
//                    Dictionary<string, AssetData> tempModifiedDic = null;
//                    if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
//                    {
//                        if (tempModifiedDic.ContainsKey(asset.m_AssetPath))
//                            itemColor = currentVersionColor;
//                    }
//                }
//                ExcelHelper.SetExcelRow(worksheet, row, 2);
//                tempExcelRange = worksheet.Cells[row, 3];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_DiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 4];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_BuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 5];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 6];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_ReferenceCount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 7];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_TextureRW ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 8];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_Width + "x" + asset.m_Height, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 9];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_Format, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 10];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_Mipmap ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 11];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_Anisolevel, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 12];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_FilterMode, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 13];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_GpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 14, row, 15];
//                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);

//                row++;
//            }

//            return row;
//        }

//        static int GenerateCustomRuleClipReport(ExcelWorksheet worksheet, int row, string typeShowName, string typeName, Dictionary<string, AssetData> assetDataMapping,
//          System.Drawing.Color CONTENT_DATA_COLOR_0,
//          System.Drawing.Color CONTENT_DATA_COLOR_1,
//          System.Drawing.Color CONTENT_DATA_COLOR_2,
//            Dictionary<string, long> filesSizeMapping,
//            Dictionary<string, long> filesBuildSizeMapping,
//            bool needShowCurrentVersion = false)
//        {
//            // Title /////////////////////////////
//            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "  " + typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);

//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 2);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 5];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 6];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 7];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_CLIP_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 8];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_CLIP_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 9, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);

//            // Total /////////////////////////////
//            var currentFilesSizeData = filesSizeMapping;
//            var currentFilesBuildSizeData = filesBuildSizeMapping;
//            long currentFileSize = 0;
//            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
//            long currentFileBuildSize = 0;
//            if (!currentFilesBuildSizeData.TryGetValue(typeName, out currentFileBuildSize)) currentFileBuildSize = 0;
//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 2);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, "(" + assetDataMapping.Count + " 个)", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileBuildSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 5, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);

//            // Details /////////////////////////////
//            row++;
//            tempExcelRange = worksheet.Cells[row, 2, row + assetDataMapping.Count - 1, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_1);
//            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
//            foreach (var asset in assetDataMapping.Values)
//            {
//                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
//                if (needShowCurrentVersion)
//                {
//                    Dictionary<string, AssetData> tempAddedDic = null;
//                    if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
//                    {
//                        if (tempAddedDic.ContainsKey(asset.m_AssetPath))
//                            itemColor = currentVersionColor;
//                    }
//                    Dictionary<string, AssetData> tempModifiedDic = null;
//                    if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
//                    {
//                        if (tempModifiedDic.ContainsKey(asset.m_AssetPath))
//                            itemColor = currentVersionColor;
//                    }
//                }
//                ExcelHelper.SetExcelRow(worksheet, row, 2);
//                tempExcelRange = worksheet.Cells[row, 3];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_DiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 4];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_BuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 5];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 6];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_ReferenceCount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 7];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_ClipLoop ? "是" : "否", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 8];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_ClipLength, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 9, row, 15];
//                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);

//                row++;
//            }

//            return row;
//        }

//        static int GenerateCustomRuleShaderReport(ExcelWorksheet worksheet, int row, string typeShowName, string typeName, Dictionary<string, AssetData> assetDataMapping,
//          System.Drawing.Color CONTENT_DATA_COLOR_0,
//          System.Drawing.Color CONTENT_DATA_COLOR_1,
//          System.Drawing.Color CONTENT_DATA_COLOR_2,
//          Dictionary<string, long> filesSizeMapping,
//          Dictionary<string, long> filesBuildSizeMapping,
//          bool needShowCurrentVersion = false)
//        {
//            // Title /////////////////////////////
//            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "  " + typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);

//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 2);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 5];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 6];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 7];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SHADER_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);
//            tempExcelRange = worksheet.Cells[row, 8, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_0);

//            // Total /////////////////////////////
//            var currentFilesSizeData = filesSizeMapping;
//            var currentFilesBuildSizeData = filesBuildSizeMapping;
//            long currentFileSize = 0;
//            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
//            long currentFileBuildSize = 0;
//            if (!currentFilesBuildSizeData.TryGetValue(typeName, out currentFileBuildSize)) currentFileBuildSize = 0;
//            row++;
//            ExcelHelper.SetExcelRow(worksheet, row, 2);
//            tempExcelRange = worksheet.Cells[row, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, "(" + assetDataMapping.Count + " 个)", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(currentFileBuildSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);
//            tempExcelRange = worksheet.Cells[row, 5, row, 15];
//            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_2);

//            // Details /////////////////////////////
//            row++;
//            tempExcelRange = worksheet.Cells[row, 2, row + assetDataMapping.Count - 1, 2];
//            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_1);
//            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
//            foreach (var asset in assetDataMapping.Values)
//            {
//                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
//                if (needShowCurrentVersion)
//                {
//                    Dictionary<string, AssetData> tempAddedDic = null;
//                    if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
//                    {
//                        if (tempAddedDic.ContainsKey(asset.m_AssetPath))
//                            itemColor = currentVersionColor;
//                    }
//                    Dictionary<string, AssetData> tempModifiedDic = null;
//                    if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
//                    {
//                        if (tempModifiedDic.ContainsKey(asset.m_AssetPath))
//                            itemColor = currentVersionColor;
//                    }
//                }
//                ExcelHelper.SetExcelRow(worksheet, row, 2);
//                tempExcelRange = worksheet.Cells[row, 3];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_DiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 4];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(asset.m_BuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 5];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 6];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_ReferenceCount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 7];
//                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_ShaderVariantCnt, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);
//                tempExcelRange = worksheet.Cells[row, 8, row, 15];
//                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     itemColor);

//                row++;
//            }

//            return row;
//        }

//        internal static void GenerateReport(ExcelPackage package)
//        {
//            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
//            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
//            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);
//            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
//            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
//            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);

//            ExcelWorksheet customRuleWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_9);
//            customRuleWS.View.ShowGridLines = false;
//            customRuleWS.Column(1).Width = 5;
//            customRuleWS.Column(2).Width = 15;
//            customRuleWS.Column(3).Width = 15;
//            customRuleWS.Column(4).Width = 15;
//            customRuleWS.Column(5).Width = 15;
//            customRuleWS.Column(6).Width = 15;
//            customRuleWS.Column(7).Width = 15;
//            customRuleWS.Column(8).Width = 15;
//            customRuleWS.Column(9).Width = 15;
//            customRuleWS.Column(10).Width = 15;
//            customRuleWS.Column(11).Width = 15;
//            customRuleWS.Column(12).Width = 15;
//            customRuleWS.Column(13).Width = 15;
//            customRuleWS.Column(14).Width = 15;
//            customRuleWS.Column(15).Width = 15;

//            //  Data Mapping  ///////////////////////////////////////////////////////////
//            var filesMapping = AssetRecordDataParser.CurrentCustomDirFilesMapping;
//            var customDirRuleMapping = AssetRecordDataParser.CurrentCustomDirRuleMapping;
//            var filesSizeMapping = AssetRecordDataParser.CurrentCustomDirFilesSizeMapping;
//            var filesBuildSizeMapping = AssetRecordDataParser.CurrentCustomDirFilesBuildSizeMapping;
//            var dirFilesAmountMappaing = AssetRecordDataParser.CurrentCustomDirFilesAmountMapping;
//            if (filesMapping == null || filesMapping.Count == 0) return;

//            Dictionary<string, long> filesTotalSizeMapping = new Dictionary<string, long>();
//            Dictionary<string, long> filesTotalBuildSizeMapping = new Dictionary<string, long>();
//            foreach (var data in filesSizeMapping)
//            {
//                var dir = data.Key;
//                var dirFilesSizeMapping = data.Value;
//                var dirFilesBuildSizeMapping = filesBuildSizeMapping[dir];

//                long dirFilesTotalSize = 0;
//                if (!filesTotalSizeMapping.TryGetValue(dir, out dirFilesTotalSize)) dirFilesTotalSize = 0;
//                long dirFilesTotalBuildSize = 0;
//                if (!filesTotalBuildSizeMapping.TryGetValue(dir, out dirFilesTotalBuildSize)) dirFilesTotalBuildSize = 0;

//                foreach (var typeName in dirFilesSizeMapping.Keys)
//                {
//                    var fileSize = dirFilesSizeMapping[typeName];
//                    var fileBuildSize = dirFilesBuildSizeMapping[typeName];

//                    dirFilesTotalSize += fileSize;
//                    dirFilesTotalBuildSize += fileBuildSize;
//                }

//                filesTotalSizeMapping[dir] = dirFilesTotalSize;
//                filesTotalBuildSizeMapping[dir] = dirFilesTotalBuildSize;
//            }

//            // 当前版本定制文件资源统计 ///////////////////////////////////////////////////////////
//            ExcelRange tempExcelRange = customRuleWS.Cells[1, 1];
//            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 RETURN_COLOR);
//            tempExcelRange = customRuleWS.Cells[1, 2, 1, 9];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_CUSTOM_DIR_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);
//            tempExcelRange = customRuleWS.Cells[2, 2, 2, 3];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_CUSTOM_DIR_CONTENT_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);
//            tempExcelRange = customRuleWS.Cells[2, 4, 2, 6];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_CUSTOM_DIR_CONTENT_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);
//            tempExcelRange = customRuleWS.Cells[2, 7];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_CUSTOM_DIR_CONTENT_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);
//            tempExcelRange = customRuleWS.Cells[2, 8];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_CUSTOM_DIR_CONTENT_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);
//            tempExcelRange = customRuleWS.Cells[2, 9];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_CUSTOM_DIR_CONTENT_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CURRENT_DATA_COLOR_0);

//            // 超链接：当前版本定制文件资源统计
//            int currentRow = 3;
//            Dictionary<string, string> hyperLinkToTotalMapping = new Dictionary<string, string>();
//            foreach (var dir in filesSizeMapping.Keys)
//            {
//                hyperLinkToTotalMapping[dir] = "B" + currentRow;

//                currentRow++;
//            }

//            // 图例
//            currentRow++;
//            tempExcelRange = customRuleWS.Cells[currentRow, 2, currentRow, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 System.Drawing.Color.FromArgb(255, 255, 255));
//            currentRow++;
//            tempExcelRange = customRuleWS.Cells[currentRow, 2, currentRow, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_CURRENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 System.Drawing.Color.FromArgb(255, 255, 0));
//            currentRow++;
//            tempExcelRange = customRuleWS.Cells[currentRow, 2, currentRow, 4];
//            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_LAST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
//                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                 CONTENT_DATA_COLOR_1);

//            // 当前版本定制文件详情 ///////////////////////////////////////////////////////////
//            currentRow+=2;
//            Dictionary<string, string> hyperLinkToDetailsMapping = new Dictionary<string, string>();
//            var textureTypeName = AssetType.IMAGE.ToString();
//            var fbxTypeName = AssetType.FBX.ToString();
//            var modTypeName = AssetType.MOD.ToString();
//            var meshTypeName = AssetType.MESH.ToString();
//            var shaderTypeName = AssetType.SHADER.ToString();
//            var clipTypeName = AssetType.CLIP.ToString();
//            var animatorClipTypeName = AssetType.ANIMATORCLIP.ToString();
//            var allAssetTypeMapping = AssetRecordDataParser.ASSETTYPE_NAME_MAPPING;
//            foreach (var dirData in filesMapping)
//            {
//                var dir = dirData.Key;
//                var dirShowName = customDirRuleMapping[dir];
//                var dirChildDataMapping = dirData.Value;

//                hyperLinkToDetailsMapping[dir] = "B" + currentRow;

//                tempExcelRange = customRuleWS.Cells[currentRow, 2, currentRow, 15];
//                string finalShowName = dirShowName + "[" + dir + "]";
//                ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_9, hyperLinkToTotalMapping[dir], finalShowName);
//                ExcelHelper.SetExcelRange(tempExcelRange, finalShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 16, true, true,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CURRENT_DATA_COLOR_1);

//                currentRow++;
//                foreach (var dirChildData in dirChildDataMapping)
//                {
//                    var typeName = dirChildData.Key;
//                    var typeShowName = allAssetTypeMapping[typeName];
//                    var assetDataMapping = dirChildData.Value;

//                    ExcelHelper.SetExcelRow(customRuleWS, currentRow, 1);

//                    if (typeName == textureTypeName)
//                        currentRow = GenerateCustomRuleTextureReport(customRuleWS, currentRow, typeShowName, typeName, assetDataMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, filesSizeMapping[dir], filesBuildSizeMapping[dir], true);
//                    else if (typeName == shaderTypeName)
//                        currentRow = GenerateCustomRuleShaderReport(customRuleWS, currentRow, typeShowName, typeName, assetDataMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, filesSizeMapping[dir], filesBuildSizeMapping[dir], true);
//                    else if (typeName == fbxTypeName || typeName == meshTypeName || typeName == modTypeName)
//                        currentRow = GenerateCustomRuleModelReport(customRuleWS, currentRow, typeShowName, typeName, assetDataMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, filesSizeMapping[dir], filesBuildSizeMapping[dir], true);
//                    else if (typeName == clipTypeName || typeName == animatorClipTypeName)
//                        currentRow = GenerateCustomRuleClipReport(customRuleWS, currentRow, typeShowName, typeName, assetDataMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, filesSizeMapping[dir], filesBuildSizeMapping[dir], true);
//                    else
//                        currentRow = GenerateCustomRuleDefaultReport(customRuleWS, currentRow, typeShowName, typeName, assetDataMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, filesSizeMapping[dir], filesBuildSizeMapping[dir], true);

//                    ExcelHelper.SetExcelRow(customRuleWS, currentRow, 1);

//                    currentRow++;
//                }

//                currentRow++;
//            }

//            // 数据：当前版本定制文件资源统计 /////////////////////////////////////
//            int detailsRow = 3;
//            foreach (var data in filesTotalSizeMapping)
//            {
//                var dir = data.Key;
//                var dirShowName = customDirRuleMapping[dir];
//                long fileSize = data.Value;
//                long fileBuildSize = filesTotalSizeMapping[dir];
//                var fileAmount = dirFilesAmountMappaing[dir];

//                tempExcelRange = customRuleWS.Cells[detailsRow, 2, detailsRow, 3];
//                ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_9, hyperLinkToDetailsMapping[dir], dirShowName);
//                ExcelHelper.SetExcelRange(tempExcelRange, dirShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CURRENT_DATA_COLOR_1);
//                tempExcelRange = customRuleWS.Cells[detailsRow, 4, detailsRow, 6];
//                ExcelHelper.SetExcelRange(tempExcelRange, dir, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CURRENT_DATA_COLOR_1);
//                tempExcelRange = customRuleWS.Cells[detailsRow, 7];
//                ExcelHelper.SetExcelRange(tempExcelRange, fileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CURRENT_DATA_COLOR_1);
//                tempExcelRange = customRuleWS.Cells[detailsRow, 8];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(fileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CURRENT_DATA_COLOR_1);
//                tempExcelRange = customRuleWS.Cells[detailsRow, 9];
//                ExcelHelper.SetExcelRange(tempExcelRange, Utility.GetFileSize(fileBuildSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
//                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
//                     CURRENT_DATA_COLOR_1);

//                detailsRow++;
//            }
//        }
//    }
//}
