using System.Collections.Generic;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace jj.TATools.Editor
{
    /// <summary>
    /// 当前版本资源详情报告
    /// </summary>
    internal class AssetCurrentTotalReporter
    {
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);

            ExcelWorksheet currentTotalWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_1);
            currentTotalWS.View.ShowGridLines = false;
            currentTotalWS.Column(1).Width = 5;
            currentTotalWS.Column(2).Width = 30;
            currentTotalWS.Column(3).Width = 18;
            currentTotalWS.Column(4).Width = 18;
            currentTotalWS.Column(5).Width = 35;
            currentTotalWS.Column(6).Width = 14;
            currentTotalWS.Column(7).Width = 18;
            currentTotalWS.Column(8).Width = 14;
            currentTotalWS.Column(9).Width = 14;
            currentTotalWS.Column(10).Width = 14;
            currentTotalWS.Column(11).Width = 14;
            currentTotalWS.Column(12).Width = 14;
            currentTotalWS.Column(13).Width = 14;
            currentTotalWS.Column(14).Width = 14;
            currentTotalWS.Column(15).Width = 14;
            currentTotalWS.Column(16).Width = 14;
            currentTotalWS.Column(17).Width = 14;
            currentTotalWS.Column(18).Width = 14;
            currentTotalWS.Column(19).Width = 14;
            currentTotalWS.Column(20).Width = 14;
            currentTotalWS.Column(21).Width = 14;

            // 当前版本资源统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = currentTotalWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = currentTotalWS.Cells[1, 2, 1, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = currentTotalWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = currentTotalWS.Cells[2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_7, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = currentTotalWS.Cells[2, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            // 超链接：当前版本资源统计
            int currentRow = 4;
            var currentFilesSizeData = AssetRecordDataParser.CurrentFilesSizeMapping;
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
            // 当前版本资源详情 ///////////////////////////////////////////////////////////
            currentRow++;
            Dictionary<EAssetType, string> hyperLinkToDetailsMapping = new Dictionary<EAssetType, string>();
            Dictionary<EAssetType, int> typeAmountMapping = new Dictionary<EAssetType, int>();
            var currentTotalFilesMapping = AssetRecordDataParser.CurrentVerisonFilesMapping;
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
                    currentRow = AssetReportUtils.GenerateCommonTextureReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }
                else if (assetType == EAssetType.RenderTexture)
                {
                    currentRow = AssetReportUtils.GenerateCommonRenderTextureReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }
                else if (assetType == EAssetType.SpriteAtlas)
                {
                    currentRow = AssetReportUtils.GenerateCommonSpriteAtlasReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }
                else if (assetType == EAssetType.Model)
                {
                    currentRow = AssetReportUtils.GenerateCommonModelReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }
                else if (assetType == EAssetType.Mesh)
                {
                    currentRow = AssetReportUtils.GenerateCommonMeshReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }
                else if (assetType == EAssetType.Prefab)
                {
                     currentRow = AssetReportUtils.GenerateCommonPrefabReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }
                else if (assetType == EAssetType.Scene)
                {
                     currentRow = AssetReportUtils.GenerateCommonSceneReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }
                else if (assetType == EAssetType.Material)
                {
                     currentRow = AssetReportUtils.GenerateCommonMaterialReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }
                else if (assetType == EAssetType.Shader)
                {
                    currentRow = AssetReportUtils.GenerateCommonShaderReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }
                else if (assetType == EAssetType.AnimationClip)
                {
                    currentRow = AssetReportUtils.GenerateCommonClipReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }
                else
                {
                    currentRow = AssetReportUtils.GenerateCommonDefaultReport(currentTotalWS, currentRow, typeShowName, assetType, assetDataMapping, hyperLinkToTotalMapping, CONTENT_DATA_COLOR_0, CONTENT_DATA_COLOR_1, SPECIAL_DATA_COLOR_0, currentFilesSizeData, false);
                }


                currentRow++;
            }

            // 数据：当前版本资源统计 /////////////////////////////////////
            int detailsRow = 4;
            long currentTotalFileSize = 0;
            foreach (var assetType in allAssetTypeMapping)
            {
                var typeShowName = assetType.ToString();
                long currentFileSize = 0;
                if (!currentFilesSizeData.TryGetValue(assetType, out currentFileSize)) currentFileSize = 0;

                currentTotalFileSize += currentFileSize;

                if (currentFileSize == 0) continue;

                tempExcelRange = currentTotalWS.Cells[detailsRow, 2];
                ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_1, hyperLinkToDetailsMapping[assetType], typeShowName);
                ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = currentTotalWS.Cells[detailsRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, typeAmountMapping[assetType], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);
                tempExcelRange = currentTotalWS.Cells[detailsRow, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);

                detailsRow++;
            }

            tempExcelRange = currentTotalWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TITLE_0, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentTotalWS.Cells[3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = currentTotalWS.Cells[3, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
        }
    }
}
