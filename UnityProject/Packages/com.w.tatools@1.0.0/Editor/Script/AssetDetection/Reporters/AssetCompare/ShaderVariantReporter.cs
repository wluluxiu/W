using System.Linq;
using System.Collections.Generic;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace jj.TATools.Editor
{
    /// <summary>
    /// Shader变体数量详情报告
    /// </summary>
    internal class ShaderVariantReporter
    {
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color UPDATE_DATA_COLOR = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color EXCEPTION_DATA_COLOR = System.Drawing.Color.FromArgb(255, 0, 1);

            ExcelWorksheet shaderVariantWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_10);
            shaderVariantWS.View.ShowGridLines = false;
            shaderVariantWS.View.FreezePanes(7, 1);
            shaderVariantWS.Column(1).Width = 5;
            shaderVariantWS.Column(2).Width = 10;
            shaderVariantWS.Column(3).Width = 14;
            shaderVariantWS.Column(4).Width = 14;
            shaderVariantWS.Column(5).Width = 14;
            shaderVariantWS.Column(6).Width = 14;
            shaderVariantWS.Column(7).Width = 14;
            shaderVariantWS.Column(8).Width = 14;
            shaderVariantWS.Column(9).Width = 14;
            shaderVariantWS.Column(10).Width = 14;
            shaderVariantWS.Column(11).Width = 18;

            // Return
            ExcelRange tempExcelRange = shaderVariantWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);

            // 图例
            tempExcelRange = shaderVariantWS.Cells[1, 2, 1, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 System.Drawing.Color.FromArgb(255, 255, 255));
            tempExcelRange = shaderVariantWS.Cells[2, 2, 2, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_CURRENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR);
            tempExcelRange = shaderVariantWS.Cells[3, 2, 3, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_LAST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            tempExcelRange = shaderVariantWS.Cells[4, 2, 4, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_EXCEPTION, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 EXCEPTION_DATA_COLOR);

            // Title
            int row = 6;
            tempExcelRange = shaderVariantWS.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_EXCEPTION_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         CONTENT_DATA_COLOR_0);
            tempExcelRange = shaderVariantWS.Cells[row, 3, row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = shaderVariantWS.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = shaderVariantWS.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = shaderVariantWS.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SHADER_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = shaderVariantWS.Cells[row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SHADER_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);


            //  Shader变体数量详情 ///////////////////////////////////////////////////////////
            var currentTotalFilesMapping = AssetRecordDataParser.CurrentVerisonFilesMapping;
            Dictionary<string, BaseRecorder> shaderDataMapping = null;
            if (currentTotalFilesMapping.TryGetValue(EAssetType.Shader, out shaderDataMapping))
            {
                if (shaderDataMapping.Count > 0)
                {
                    Dictionary<string, BaseRecorder> tempAddedDic = null;
                    AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(EAssetType.Shader, out tempAddedDic);
                    Dictionary<string, BaseRecorder> tempModifiedDic = null;
                    AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(EAssetType.Shader, out tempModifiedDic);
                    System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                    row ++;
                    int index = 0;
                    var sortDataMapping = shaderDataMapping.OrderByDescending(o => (o.Value as ShaderRecorder).m_KeywordsList.Count).ToDictionary(o => o.Key, p => p.Value);
                    foreach (var baseRecorder in sortDataMapping.Values)
                    {
                        ShaderRecorder shaderRecorder = baseRecorder as ShaderRecorder;

                        index++;

                        bool updateAsset = false;
                        if (tempAddedDic != null && tempAddedDic.ContainsKey(shaderRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }
                        if (tempModifiedDic != null && tempModifiedDic.ContainsKey(shaderRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }

                        if (updateAsset)
                        {
                            itemColor = UPDATE_DATA_COLOR;
                        }

                        tempExcelRange = shaderVariantWS.Cells[row, 2];
                        ExcelHelper.SetExcelRange(tempExcelRange, index, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = shaderVariantWS.Cells[row, 3, row, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = shaderVariantWS.Cells[row, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(shaderRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        System.Drawing.Color singleColColor = shaderRecorder.ReferenciesInvalid() ? EXCEPTION_DATA_COLOR : itemColor;
                        tempExcelRange = shaderVariantWS.Cells[row, 9];
                        ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             singleColColor);
                        singleColColor = shaderRecorder.DependenciesInvalid() ? EXCEPTION_DATA_COLOR : itemColor;
                        tempExcelRange = shaderVariantWS.Cells[row, 10];
                        ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_DirectDepenpendices.Count, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             singleColColor);
                        tempExcelRange = shaderVariantWS.Cells[row, 11];
                        ExcelHelper.SetExcelRange(tempExcelRange, shaderRecorder.m_KeywordsList.Count, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);


                        row++;
                    }
                }
            }
        }
    }
}
