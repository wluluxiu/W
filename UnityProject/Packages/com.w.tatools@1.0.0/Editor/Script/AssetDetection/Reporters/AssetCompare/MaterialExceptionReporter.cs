using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace jj.TATools.Editor
{
    /// <summary>
    ///  材质异常详情报告
    /// </summary>
    internal class MaterialExceptionReporter
    {
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color EXCEPTION_DATA_COLOR = System.Drawing.Color.FromArgb(255, 0, 1);
            System.Drawing.Color UPDATE_DATA_COLOR = System.Drawing.Color.FromArgb(255, 255, 0);

            ExcelWorksheet materialExceptionWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_9);
            materialExceptionWS.View.ShowGridLines = false;
            materialExceptionWS.View.FreezePanes(11, 1);
            materialExceptionWS.Column(1).Width = 5;
            materialExceptionWS.Column(2).Width = 10;
            materialExceptionWS.Column(3).Width = 18;
            materialExceptionWS.Column(4).Width = 18;
            materialExceptionWS.Column(5).Width = 18;
            materialExceptionWS.Column(6).Width = 18;
            materialExceptionWS.Column(7).Width = 18;
            materialExceptionWS.Column(8).Width = 18;
            materialExceptionWS.Column(9).Width = 18;
            materialExceptionWS.Column(10).Width = 18;
            materialExceptionWS.Column(11).Width = 18;
            materialExceptionWS.Column(12).Width = 18;
            materialExceptionWS.Column(13).Width = 18;
            materialExceptionWS.Column(14).Width = 18;
 

            // 当前版本异常Material统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = materialExceptionWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = materialExceptionWS.Cells[1, 2, 1, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_MATERIAL_EXCEPTION_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_MATERIAL_EXCEPTION_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            // 图例
            tempExcelRange = materialExceptionWS.Cells[5, 2, 5, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 System.Drawing.Color.FromArgb(255, 255, 255));
            tempExcelRange = materialExceptionWS.Cells[6, 2, 6, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_CURRENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR);
            tempExcelRange = materialExceptionWS.Cells[7, 2, 7, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_LAST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            tempExcelRange = materialExceptionWS.Cells[8, 2, 8, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_EXCEPTION, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 EXCEPTION_DATA_COLOR);

            // Title /////////////////////////////
            int row = 10;
            tempExcelRange = materialExceptionWS.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_MATERIAL_EXCEPTION_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[row, 3, row, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MATERIAL_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MATERIAL_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MATERIAL_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MATERIAL_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[row, 11,row, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MATERIAL_TITLE_4, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
           
            // 当前版本异常Material详情 ///////////////////////////////////////////////////////////
            row++;
            var currentTotalFilesMapping = AssetRecordDataParser.CurrentVerisonFilesMapping;
            int totalFileAmount = 0;
            long currentTotalFileSize = 0;
            Dictionary<string, BaseRecorder> materialDataMapping = null;
            if (currentTotalFilesMapping.TryGetValue(EAssetType.Material, out materialDataMapping))
            {
                if (materialDataMapping.Count > 0)
                {
                    // Details /////////////////////////////
                    Dictionary<string, BaseRecorder> tempAddedDic = null;
                    AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(EAssetType.Material, out tempAddedDic);
                    Dictionary<string, BaseRecorder> tempModifiedDic = null;
                    AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(EAssetType.Material, out tempModifiedDic);
                    System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                    foreach (var baseRecorder in materialDataMapping.Values)
                    {
                        MaterialRecorder materialRecorder = baseRecorder as MaterialRecorder;

                        bool refEx = materialRecorder.ReferenciesInvalid();
                        bool refShaderEx = materialRecorder.RefShaderValid();
                        bool existNoUsedTexPropsEx = materialRecorder.ExistNoUsedTexProps();
                        bool existNoUsedFloatAndIntPropsEx = materialRecorder.ExistNoUsedFloatAndIntProps();
                        bool existNoUsedColorAndVectorPropsEx = materialRecorder.ExistNoUsedColorAndVectorProps();
                        bool depBuiltinEx = materialRecorder.InvalidBuiltinDependencies();

                        if (!refEx && !refShaderEx && !existNoUsedTexPropsEx && !existNoUsedFloatAndIntPropsEx && !existNoUsedColorAndVectorPropsEx && !depBuiltinEx) continue;

                        totalFileAmount++;
                        currentTotalFileSize += materialRecorder.m_FileDiskSize;

                        bool updateAsset = false;
                        if (tempAddedDic != null && tempAddedDic.ContainsKey(materialRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }
                        if (tempModifiedDic != null && tempModifiedDic.ContainsKey(materialRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }

                        if (updateAsset)
                        {
                            itemColor = UPDATE_DATA_COLOR;
                        }

                        int addRow = materialRecorder.m_BuiltinDependencies.Count > 0 ? (materialRecorder.m_BuiltinDependencies.Count - 1) : 0;
                        tempExcelRange = materialExceptionWS.Cells[row, 2, row + addRow, 2];
                        ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = materialExceptionWS.Cells[row, 3,row + addRow, 4];
                        ExcelHelper.SetExcelRange(tempExcelRange, materialRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = materialExceptionWS.Cells[row, 5, row + addRow, 5];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(materialRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = materialExceptionWS.Cells[row, 6, row + addRow, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, materialRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             refEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = materialExceptionWS.Cells[row, 7, row + addRow, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, refShaderEx ? "Miss|" + materialRecorder.m_MissShaderGuid: "-", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             refShaderEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = materialExceptionWS.Cells[row, 8, row + addRow, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, materialRecorder.m_NoUsedTexEnvsProps.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             existNoUsedTexPropsEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = materialExceptionWS.Cells[row, 9, row + addRow, 9];
                        ExcelHelper.SetExcelRange(tempExcelRange, materialRecorder.m_NoUsedFloatAndIntProps.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             existNoUsedFloatAndIntPropsEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = materialExceptionWS.Cells[row, 10, row + addRow, 10];
                        ExcelHelper.SetExcelRange(tempExcelRange, materialRecorder.m_NoUsedColorAndVectorProps.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             existNoUsedColorAndVectorPropsEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = materialExceptionWS.Cells[row, 11,row + addRow,11];
                        ExcelHelper.SetExcelRange(tempExcelRange, materialRecorder.m_BuiltinDependencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             depBuiltinEx ? EXCEPTION_DATA_COLOR : itemColor);
                        if (materialRecorder.m_BuiltinDependencies.Count == 0)
                        {
                            tempExcelRange = materialExceptionWS.Cells[row, 12, row, 14];
                            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                 itemColor);
                        }
                        else
                        {
                            for (int k = 0; k < materialRecorder.m_BuiltinDependencies.Count; k++)
                            {
                                tempExcelRange = materialExceptionWS.Cells[row + k, 12, row + k, 14];
                                ExcelHelper.SetExcelRange(tempExcelRange, materialRecorder.m_BuiltinDependencies[k], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     itemColor);
                            }
                        }

                        row += (materialRecorder.m_BuiltinDependencies.Count == 0 ? 1 : materialRecorder.m_BuiltinDependencies.Count);
                    }
                }
            }

            // 数据：当前版本异常Material统计 /////////////////////////////////////
            tempExcelRange = materialExceptionWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = materialExceptionWS.Cells[3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
        }
    }
}
