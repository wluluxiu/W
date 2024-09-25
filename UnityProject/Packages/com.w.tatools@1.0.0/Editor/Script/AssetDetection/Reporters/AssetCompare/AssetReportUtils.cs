using System.Collections.Generic;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace jj.TATools.Editor
{
    internal class AssetReportUtils
    {
        internal static int GenerateCommonDefaultShowBuiltinReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
          System.Drawing.Color CONTENT_DATA_COLOR_0,
          System.Drawing.Color CONTENT_DATA_COLOR_1,
          System.Drawing.Color CONTENT_DATA_COLOR_2,
          Dictionary<EAssetType, long> filesSizeMapping,
          bool needShowCurrentVersion, List<string> whiteList = null)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 13];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4, row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10, row, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_6, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int baseRow = row;
            int totalAddRow = 0;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var asset in assetDataMapping.Values)
            {
                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(asset.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(asset.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(asset.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }

                for (int k = 0; k < asset.m_BuiltinDependencies.Count; k++)
                {
                    ExcelHelper.SetExcelRow(worksheet, row + k, 1);
                }
                int addRow = asset.m_BuiltinDependencies.Count - 1;
                totalAddRow += addRow;
                tempExcelRange = worksheet.Cells[row, 3,row + addRow,3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(asset.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[row, 4,row + addRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[row, 8, row + addRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[row, 9, row + addRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_DirectDepenpendices.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[row, 10, row + addRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_BuiltinDependencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                for (int k = 0; k < asset.m_BuiltinDependencies.Count; k++)
                {
                    tempExcelRange = worksheet.Cells[row + k, 11, row + k, 13];
                    ExcelHelper.SetExcelRange(tempExcelRange, asset.m_BuiltinDependencies[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         itemColor);
                }

                row+= asset.m_BuiltinDependencies.Count;
            }

            tempExcelRange = worksheet.Cells[baseRow, 2, baseRow + assetDataMapping.Count - 1 + totalAddRow, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonDefaultReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
           System.Drawing.Color CONTENT_DATA_COLOR_0,
           System.Drawing.Color CONTENT_DATA_COLOR_1,
           System.Drawing.Color CONTENT_DATA_COLOR_2,
           Dictionary<EAssetType, long> filesSizeMapping,
           bool needShowCurrentVersion, List<string> whiteList = null,
           bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var asset in assetDataMapping.Values)
            {
                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(asset.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(asset.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(asset.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }

                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (asset.m_ReferencedInCodePathList != null && asset.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + asset.m_ReferencedInCodePathList.Count - 1;
                    }
                }
                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(asset.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (asset.m_ReferencedInCodePathList == null || asset.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, asset.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < asset.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, asset.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, asset.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_BuiltinDependencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9, endRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_DirectDepenpendices.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 10, endRow, 21];
                tempExcelRange = worksheet.Cells[startRow, 10, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonTextureReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
           System.Drawing.Color CONTENT_DATA_COLOR_0,
           System.Drawing.Color CONTENT_DATA_COLOR_1,
           System.Drawing.Color CONTENT_DATA_COLOR_2,
            Dictionary<EAssetType, long> filesSizeMapping,
            bool needShowCurrentVersion, List<string> whiteList = null,
            bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4,row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10,row,11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 15];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 16, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var baseRecorder in assetDataMapping.Values)
            {
                TextureRecorder meshRecorder = baseRecorder as TextureRecorder;

                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(meshRecorder.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(meshRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(meshRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }

                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (meshRecorder.m_ReferencedInCodePathList != null && meshRecorder.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + meshRecorder.m_ReferencedInCodePathList.Count - 1;
                    }
                }
                tempExcelRange = worksheet.Cells[startRow, 3,endRow,3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(meshRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4,endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (meshRecorder.m_ReferencedInCodePathList == null || meshRecorder.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < meshRecorder.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
               
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Readable == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9, endRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Width + "x" + meshRecorder.m_Height, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 10, endRow, 11];
                ExcelHelper.SetExcelRange(tempExcelRange, ((ETextureImporterFormat)meshRecorder.m_FormatAndroid).ToString() + "|" + ((ETextureImporterFormat)meshRecorder.m_FormatIOS).ToString(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 12, endRow, 12];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Mipmap == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 13, endRow, 13];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_AnisoLevel, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 14, endRow, 14];
                ExcelHelper.SetExcelRange(tempExcelRange, ((EFilterMode)meshRecorder.m_FilterMode).ToString(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 15, endRow, 15];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(meshRecorder.m_MemorySize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 16, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonSpriteAtlasReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
           System.Drawing.Color CONTENT_DATA_COLOR_0,
           System.Drawing.Color CONTENT_DATA_COLOR_1,
           System.Drawing.Color CONTENT_DATA_COLOR_2,
            Dictionary<EAssetType, long> filesSizeMapping,
            bool needShowCurrentVersion, List<string> whiteList = null,
            bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ATLAS_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ATLAS_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10, row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ATLAS_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ATLAS_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ATLAS_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ATLAS_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 15];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ATLAS_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 16, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var baseRecorder in assetDataMapping.Values)
            {
                SpriteAtlasRecorder atlasRecorder = baseRecorder as SpriteAtlasRecorder;

                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(atlasRecorder.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(atlasRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(atlasRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }
                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (atlasRecorder.m_ReferencedInCodePathList != null && atlasRecorder.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + atlasRecorder.m_ReferencedInCodePathList.Count - 1;
                    }
                }
                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(atlasRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (atlasRecorder.m_ReferencedInCodePathList == null || atlasRecorder.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, atlasRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < atlasRecorder.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, atlasRecorder.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, atlasRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasRecorder.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasRecorder.m_RW == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9, endRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasRecorder.m_MaxSizeAndroid + "|" + atlasRecorder.m_MaxSizeIOS, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 10,endRow, 11];
                ExcelHelper.SetExcelRange(tempExcelRange, ((ETextureImporterFormat)atlasRecorder.m_FormatAndroid).ToString() + "|" + ((ETextureImporterFormat)atlasRecorder.m_FormatIOS).ToString(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 12, endRow, 12];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasRecorder.m_GenerateMipmaps == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 13, endRow, 13];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasRecorder.m_AnisoLevel, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 14, endRow, 14];
                ExcelHelper.SetExcelRange(tempExcelRange, ((EFilterMode)atlasRecorder.m_FilterMode).ToString(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 15, endRow, 15];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasRecorder.m_IncludeInBuild == 1 ? "开" : "关", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 16, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonRenderTextureReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
           System.Drawing.Color CONTENT_DATA_COLOR_0,
           System.Drawing.Color CONTENT_DATA_COLOR_1,
           System.Drawing.Color CONTENT_DATA_COLOR_2,
            Dictionary<EAssetType, long> filesSizeMapping,
            bool needShowCurrentVersion, List<string> whiteList = null,
            bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_RT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_RT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_RT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_RT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_RT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_RT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_RT_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 15];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_RT_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 16, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var baseRecorder in assetDataMapping.Values)
            {
                RenderTextureRecorder rtRecorder = baseRecorder as RenderTextureRecorder;

                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(rtRecorder.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(rtRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(rtRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }
                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (rtRecorder.m_ReferencedInCodePathList != null && rtRecorder.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + rtRecorder.m_ReferencedInCodePathList.Count - 1;
                    }
                }

                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(rtRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (rtRecorder.m_ReferencedInCodePathList == null || rtRecorder.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < rtRecorder.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_RandomWrite == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9, endRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_Width + "x" + rtRecorder.m_Height, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 10, endRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, ((EGraphicsFormat)rtRecorder.m_ColorFormat).ToString(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 11, endRow, 11];
                ExcelHelper.SetExcelRange(tempExcelRange, ((EGraphicsFormat)rtRecorder.m_DepthStencilFormat).ToString(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 12, endRow, 12];
                ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_EnableMipmaps == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 13, endRow, 13];
                ExcelHelper.SetExcelRange(tempExcelRange, ((ERTAntiAliasing)rtRecorder.m_AntiAliasing).ToString(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 14, endRow, 14];
                ExcelHelper.SetExcelRange(tempExcelRange, ((EFilterMode)rtRecorder.m_FilterMode).ToString(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 15, endRow, 15];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(rtRecorder.m_MemorySize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 16, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonClipReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
          System.Drawing.Color CONTENT_DATA_COLOR_0,
          System.Drawing.Color CONTENT_DATA_COLOR_1,
          System.Drawing.Color CONTENT_DATA_COLOR_2,
            Dictionary<EAssetType, long> filesSizeMapping,
            bool needShowCurrentVersion, List<string> whiteList = null,
             bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_CLIP_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_CLIP_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_CLIP_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_CLIP_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 12, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var baseRecorder in assetDataMapping.Values)
            {
                AnimationClipRecorder animRecorder = baseRecorder as AnimationClipRecorder;

                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(animRecorder.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(animRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(animRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }

                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (animRecorder.m_ReferencedInCodePathList != null && animRecorder.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + animRecorder.m_ReferencedInCodePathList.Count - 1;
                    }
                }
              
                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(animRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4,endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, animRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (animRecorder.m_ReferencedInCodePathList == null || animRecorder.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, animRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < animRecorder.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, animRecorder.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, animRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, animRecorder.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, animRecorder.m_Legacy == 1 ? "旧" : "新", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9, endRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, animRecorder.m_Length, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 10, endRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, animRecorder.m_Legacy == 1 ? ((EAnimWrapMode)animRecorder.m_WrapMode).ToString() : "-", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 11, endRow, 11];
                ExcelHelper.SetExcelRange(tempExcelRange, animRecorder.m_Legacy == 1 ? "-" : (animRecorder.m_LoopTime == 1 ? "是" : "否"), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 12, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row  - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonModelReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
           System.Drawing.Color CONTENT_DATA_COLOR_0,
           System.Drawing.Color CONTENT_DATA_COLOR_1,
           System.Drawing.Color CONTENT_DATA_COLOR_2,
            Dictionary<EAssetType, long> filesSizeMapping,
            bool needShowCurrentVersion, List<string> whiteList = null,
            bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4,row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 15];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 16];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_8, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 17];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_9, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 18];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_10, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 19, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var baseRecorder in assetDataMapping.Values)
            {
                ModelRecorder modelRecorder = baseRecorder as ModelRecorder;

                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(modelRecorder.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(modelRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(modelRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }
                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (modelRecorder.m_ReferencedInCodePathList != null && modelRecorder.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + modelRecorder.m_ReferencedInCodePathList.Count - 1;
                    }
                }
                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(modelRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4,endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (modelRecorder.m_ReferencedInCodePathList == null || modelRecorder.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < modelRecorder.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_RW == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9, endRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Triangle, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 10, endRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_VertexCount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 11, endRow, 11];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Bones, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 12, endRow, 12];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Normal == 1 ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 13, endRow, 13];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Tangent == 1 ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 14, endRow, 14];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Color == 1 ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 15, endRow, 15];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.GetUVShowStr(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 16, endRow, 16];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_BlendShapeCount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 17, endRow, 17];
                ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_GenerateLightmapUVs == 1 ? "开启" : "关闭", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 18, endRow, 18];
                ExcelHelper.SetExcelRange(tempExcelRange, ((EModelImporterAnimationCompression)modelRecorder.m_AnimCompression).ToString(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 19, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonMeshReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
            System.Drawing.Color CONTENT_DATA_COLOR_0,
            System.Drawing.Color CONTENT_DATA_COLOR_1,
            System.Drawing.Color CONTENT_DATA_COLOR_2,
              Dictionary<EAssetType, long> filesSizeMapping,
              bool needShowCurrentVersion, List<string> whiteList = null,
               bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MESH_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MESH_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MESH_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MESH_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MESH_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MESH_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MESH_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 15];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MESH_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 16, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var baseRecorder in assetDataMapping.Values)
            {
                MeshRecorder meshRecorder = baseRecorder as MeshRecorder;

                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(meshRecorder.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(meshRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(meshRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }
                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (meshRecorder.m_ReferencedInCodePathList != null && meshRecorder.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + meshRecorder.m_ReferencedInCodePathList.Count - 1;
                    }
                }
                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(meshRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (meshRecorder.m_ReferencedInCodePathList == null || meshRecorder.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < meshRecorder.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Readable == 1 ? "开启":"关闭", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9, endRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Triangle, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 10, endRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_VertexCount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 11, endRow, 11];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Normal == 1 ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 12, endRow, 12];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Tangent == 1 ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 13, endRow, 13];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_Color == 1 ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 14, endRow, 14];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.GetUVShowStr(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 15, endRow, 15];
                ExcelHelper.SetExcelRange(tempExcelRange, meshRecorder.m_BlendShapeCount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 16, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonPrefabReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
          System.Drawing.Color CONTENT_DATA_COLOR_0,
          System.Drawing.Color CONTENT_DATA_COLOR_1,
          System.Drawing.Color CONTENT_DATA_COLOR_2,
            Dictionary<EAssetType, long> filesSizeMapping,
            bool needShowCurrentVersion, List<string> whiteList = null,
            bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_PREFAB_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_PREFAB_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_PREFAB_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_PREFAB_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 12, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var baseRecorder in assetDataMapping.Values)
            {
                PrefabRecorder prefabRecorder = baseRecorder as PrefabRecorder;

                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(prefabRecorder.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(prefabRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(prefabRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }
                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (prefabRecorder.m_ReferencedInCodePathList != null && prefabRecorder.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + prefabRecorder.m_ReferencedInCodePathList.Count - 1;
                    }
                }
                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(prefabRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, prefabRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (prefabRecorder.m_ReferencedInCodePathList == null || prefabRecorder.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, prefabRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < prefabRecorder.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, prefabRecorder.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, prefabRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, prefabRecorder.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, prefabRecorder.m_MissingCompList.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9, endRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, prefabRecorder.m_CollidersMapping.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 10, endRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, prefabRecorder.m_BuiltinDependencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 11, endRow, 11];
                ExcelHelper.SetExcelRange(tempExcelRange, prefabRecorder.m_MissingMaterialsList.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 12, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonSceneReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
          System.Drawing.Color CONTENT_DATA_COLOR_0,
          System.Drawing.Color CONTENT_DATA_COLOR_1,
          System.Drawing.Color CONTENT_DATA_COLOR_2,
            Dictionary<EAssetType, long> filesSizeMapping,
            bool needShowCurrentVersion, List<string> whiteList = null,
             bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SCENE_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SCENE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SCENE_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 11, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var baseRecorder in assetDataMapping.Values)
            {
                SceneRecorder sceneRecorder = baseRecorder as SceneRecorder;

                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(sceneRecorder.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(sceneRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(sceneRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }
                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (sceneRecorder.m_ReferencedInCodePathList != null && sceneRecorder.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + sceneRecorder.m_ReferencedInCodePathList.Count - 1;
                    }
                }
                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(sceneRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (sceneRecorder.m_ReferencedInCodePathList == null || sceneRecorder.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < sceneRecorder.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_MissingCompList.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9, endRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_CollidersMapping.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 10, endRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_BuiltinDependencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 11, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonMaterialReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
          System.Drawing.Color CONTENT_DATA_COLOR_0,
          System.Drawing.Color CONTENT_DATA_COLOR_1,
          System.Drawing.Color CONTENT_DATA_COLOR_2,
            Dictionary<EAssetType, long> filesSizeMapping,
            bool needShowCurrentVersion, List<string> whiteList = null,
            bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MATERIAL_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MATERIAL_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MATERIAL_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MATERIAL_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MATERIAL_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 13, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var baseRecorder in assetDataMapping.Values)
            {
                MaterialRecorder matRecorder = baseRecorder as MaterialRecorder;

                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(matRecorder.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(matRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(matRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }
                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (matRecorder.m_ReferencedInCodePathList != null && matRecorder.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + matRecorder.m_ReferencedInCodePathList.Count - 1;
                    }
                }

                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(matRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, matRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (matRecorder.m_ReferencedInCodePathList == null || matRecorder.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, matRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < matRecorder.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, matRecorder.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, matRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, matRecorder.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, matRecorder.m_ShaderRefState == 0 ? "Miss":"-", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9, endRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, matRecorder.m_NoUsedTexEnvsProps.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 10, endRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, matRecorder.m_NoUsedFloatAndIntProps.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 11, endRow, 11];
                ExcelHelper.SetExcelRange(tempExcelRange, matRecorder.m_NoUsedColorAndVectorProps.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 12, endRow, 12];
                ExcelHelper.SetExcelRange(tempExcelRange, matRecorder.m_BuiltinDependencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 13, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row  - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }

        internal static int GenerateCommonShaderReport(ExcelWorksheet worksheet, int row, string typeShowName, EAssetType typeName, Dictionary<string, BaseRecorder> assetDataMapping, Dictionary<EAssetType, string> hyperLinkToTotalMapping,
          System.Drawing.Color CONTENT_DATA_COLOR_0,
          System.Drawing.Color CONTENT_DATA_COLOR_1,
          System.Drawing.Color CONTENT_DATA_COLOR_2,
          Dictionary<EAssetType, long> filesSizeMapping,
          bool needShowCurrentVersion, List<string> whiteList = null,
          bool showReferencedInCode = false)
        {
            // Title /////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[row, 2, row, 21];
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
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4,row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SHADER_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 9, row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SHADER_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 11, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            int totalStartRow = row;
            System.Drawing.Color currentVersionColor = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color whiteListColor = System.Drawing.Color.FromArgb(191, 191, 191);
            foreach (var baseRecorder in assetDataMapping.Values)
            {
                ShaderRecorder shaderRecorder = baseRecorder as ShaderRecorder;

                System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                if (whiteList != null && whiteList.Contains(shaderRecorder.m_AssetPath))
                {
                    itemColor = whiteListColor;
                }
                else
                {
                    if (needShowCurrentVersion)
                    {
                        Dictionary<string, BaseRecorder> tempAddedDic = null;
                        if (AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAddedDic))
                        {
                            if (tempAddedDic.ContainsKey(shaderRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                        Dictionary<string, BaseRecorder> tempModifiedDic = null;
                        if (AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempModifiedDic))
                        {
                            if (tempModifiedDic.ContainsKey(shaderRecorder.m_AssetPath))
                                itemColor = currentVersionColor;
                        }
                    }
                }
                int startRow = row;
                int endRow = row;
                if (showReferencedInCode)
                {
                    if (shaderRecorder.m_ReferencedInCodePathList != null && shaderRecorder.m_ReferencedInCodePathList.Count > 0)
                    {
                        endRow = row + shaderRecorder.m_ReferencedInCodePathList.Count - 1;
                    }
                }
                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(shaderRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 4,endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                if (showReferencedInCode)
                {
                    if (shaderRecorder.m_ReferencedInCodePathList == null || shaderRecorder.m_ReferencedInCodePathList.Count == 0)
                    {
                        ExcelHelper.SetExcelRow(worksheet, row, 1);
                        tempExcelRange = worksheet.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);
                    }
                    else
                    {
                        for (int k = 0; k < shaderRecorder.m_ReferencedInCodePathList.Count; k++)
                        {
                            var itemRow = row + k;
                            ExcelHelper.SetExcelRow(worksheet, itemRow, 1);
                            tempExcelRange = worksheet.Cells[itemRow, 6];
                            ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_ReferencedInCodePathList[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                                    ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     ExcelHelper.REFERENCED_IN_CODE_COLOR);
                        }
                    }
                }
                else
                {
                    ExcelHelper.SetExcelRow(worksheet, row, 1);
                    tempExcelRange = worksheet.Cells[row, 6];
                    ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                                ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                itemColor);
                }
                tempExcelRange = worksheet.Cells[startRow, 7, endRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_BundleNamesStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 8, endRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_DirectDepenpendices.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 9,endRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_KeywordsList.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[startRow, 11, endRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);

                row = endRow + 1;
            }

            tempExcelRange = worksheet.Cells[totalStartRow, 2, row - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            return row;
        }
    }
}
