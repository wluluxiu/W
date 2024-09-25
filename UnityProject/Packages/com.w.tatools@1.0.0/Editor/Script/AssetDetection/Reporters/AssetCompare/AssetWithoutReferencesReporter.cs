using System.Collections.Generic;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace jj.TATools.Editor
{
    /// <summary>
    /// 废弃资源详情报告
    /// </summary>
    internal class AssetWithoutReferencesReporter
    {
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color WHITE_LIST_DATA_COLOR = System.Drawing.Color.FromArgb(191, 191, 191);

            ExcelWorksheet noReferencesExceptWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_6);
            noReferencesExceptWS.View.ShowGridLines = false;
            noReferencesExceptWS.Column(1).Width = 5;
            noReferencesExceptWS.Column(2).Width = 30;
            noReferencesExceptWS.Column(3).Width = 18;
            noReferencesExceptWS.Column(4).Width = 18;
            noReferencesExceptWS.Column(5).Width = 18;
            noReferencesExceptWS.Column(6).Width = 14;
            noReferencesExceptWS.Column(7).Width = 18;
            noReferencesExceptWS.Column(8).Width = 14;
            noReferencesExceptWS.Column(9).Width = 14;
            noReferencesExceptWS.Column(10).Width = 14;
            noReferencesExceptWS.Column(11).Width = 14;
            noReferencesExceptWS.Column(12).Width = 14;
            noReferencesExceptWS.Column(13).Width = 14;
            noReferencesExceptWS.Column(14).Width = 14;
            noReferencesExceptWS.Column(15).Width = 14;
            noReferencesExceptWS.Column(16).Width = 14;
            noReferencesExceptWS.Column(17).Width = 14;
            noReferencesExceptWS.Column(18).Width = 14;
            noReferencesExceptWS.Column(19).Width = 14;
            noReferencesExceptWS.Column(20).Width = 14;
            noReferencesExceptWS.Column(21).Width = 14;

            // 冗余资源统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = noReferencesExceptWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = noReferencesExceptWS.Cells[1, 2, 1, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_NO_REFERENCES_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = noReferencesExceptWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = noReferencesExceptWS.Cells[2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_7, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = noReferencesExceptWS.Cells[2, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            // 超链接：当前版本资源新增统计
            int currentRow = 4;
            var currentFilesSizeData = AssetRecordDataParser.CurrentNoRefFilesSizeMapping;
            var allAssetTypeMapping = AssetRecordDataParser.AssetTypeMapping;
            Dictionary<EAssetType, string> hyperLinkToTotalMapping = new Dictionary<EAssetType, string>();
            foreach (var assetType in allAssetTypeMapping)
            {
                var typeShowName = assetType.ToString();
                long currentFileSize = 0;
                if (!currentFilesSizeData.TryGetValue(assetType, out currentFileSize)) currentFileSize = 0;

                hyperLinkToTotalMapping[assetType] = "B" + currentRow;

                if (currentFileSize == 0) continue;

                currentRow++;
            }

            // 图例
            currentRow++;
            tempExcelRange = noReferencesExceptWS.Cells[currentRow, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 System.Drawing.Color.FromArgb(255, 255, 255));
            currentRow++;
            tempExcelRange = noReferencesExceptWS.Cells[currentRow, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_CURRENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 System.Drawing.Color.FromArgb(255, 255, 0));
            currentRow++;
            tempExcelRange = noReferencesExceptWS.Cells[currentRow, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_LAST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            currentRow++;
            tempExcelRange = noReferencesExceptWS.Cells[currentRow, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_REFERENCED_IN_CODE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 ExcelHelper.REFERENCED_IN_CODE_COLOR);
            currentRow++;
            tempExcelRange = noReferencesExceptWS.Cells[currentRow, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_WHITELIST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 WHITE_LIST_DATA_COLOR);

            // 冗余资源详情 ///////////////////////////////////////////////////////////
            currentRow += 2;
            Dictionary<EAssetType, string> hyperLinkToDetailsMapping = new Dictionary<EAssetType, string>();
            Dictionary<EAssetType, int> typeAmountMapping = new Dictionary<EAssetType, int>();
            var currentTotalFilesMapping = AssetRecordDataParser.CurrentNoRefMapping;
            var dynamicLoadedWhiteList = AssetRecordDataParser.CurrentWhiteListDynamicLoaded;
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

                if (assetType == EAssetType.Texture)
                {
                    currentRow = AssetReportUtils.GenerateCommonTextureReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList,true);
                }
                else if (assetType == EAssetType.RenderTexture)
                {
                    currentRow = AssetReportUtils.GenerateCommonRenderTextureReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList, true);
                }
                else if (assetType == EAssetType.SpriteAtlas)
                {
                    currentRow = AssetReportUtils.GenerateCommonSpriteAtlasReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList, true);
                }
                else if (assetType == EAssetType.Model)
                {
                    currentRow = AssetReportUtils.GenerateCommonModelReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList, true);
                }
                else if (assetType == EAssetType.Mesh)
                {
                    currentRow = AssetReportUtils.GenerateCommonMeshReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList, true);
                }
                else if (assetType == EAssetType.Prefab)
                {
                    currentRow = AssetReportUtils.GenerateCommonPrefabReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList, true);
                }
                else if (assetType == EAssetType.Scene)
                {
                    currentRow = AssetReportUtils.GenerateCommonSceneReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList,true);
                }
                else if (assetType == EAssetType.Material)
                {
                    currentRow = AssetReportUtils.GenerateCommonMaterialReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList,true);
                }
                else if (assetType == EAssetType.Shader)
                {
                    currentRow = AssetReportUtils.GenerateCommonShaderReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList, true);
                }
                else if (assetType == EAssetType.AnimationClip)
                {
                    currentRow = AssetReportUtils.GenerateCommonClipReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList,true);
                }
                else
                {
                    currentRow = AssetReportUtils.GenerateCommonDefaultReport(noReferencesExceptWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, true, dynamicLoadedWhiteList, true);
                }

                currentRow++;
            }

            // 数据：冗余资源统计 /////////////////////////////////////
            int detailsRow = 4;
            long currentTotalFileSize = 0;
            foreach (var assetType in allAssetTypeMapping)
            {
                var typeShowName = assetType.ToString();
                long currentFileSize = 0;
                if (!currentFilesSizeData.TryGetValue(assetType, out currentFileSize)) currentFileSize = 0;

                currentTotalFileSize += currentFileSize;

                if (currentFileSize == 0) continue;

                tempExcelRange = noReferencesExceptWS.Cells[detailsRow, 2];
                ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_6, hyperLinkToDetailsMapping[assetType], typeShowName);
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = noReferencesExceptWS.Cells[detailsRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, typeAmountMapping[assetType], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = noReferencesExceptWS.Cells[detailsRow, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);

                detailsRow++;
            }

            tempExcelRange = noReferencesExceptWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TITLE_0, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = noReferencesExceptWS.Cells[3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = noReferencesExceptWS.Cells[3, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);

        }
    }
}
