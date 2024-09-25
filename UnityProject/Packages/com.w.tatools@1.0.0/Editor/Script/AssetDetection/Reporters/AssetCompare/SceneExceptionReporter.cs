using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace jj.TATools.Editor
{
    /// <summary>
    ///  场景异常详情报告
    /// </summary>
    internal class SceneExceptionReporter
    {
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color UPDATE_DATA_COLOR = System.Drawing.Color.FromArgb(255, 255, 0);
            System.Drawing.Color EXCEPTION_DATA_COLOR = System.Drawing.Color.FromArgb(255, 0, 1);

            ExcelWorksheet prefabExceptionWs = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_14);
            prefabExceptionWs.View.ShowGridLines = false;
            prefabExceptionWs.View.FreezePanes(11, 1);
            prefabExceptionWs.Column(1).Width = 5;
            prefabExceptionWs.Column(2).Width = 10;
            prefabExceptionWs.Column(3).Width = 14;
            prefabExceptionWs.Column(4).Width = 14;
            prefabExceptionWs.Column(5).Width = 14;
            prefabExceptionWs.Column(6).Width = 14;
            prefabExceptionWs.Column(7).Width = 18;
            prefabExceptionWs.Column(8).Width = 18;
            prefabExceptionWs.Column(9).Width = 12;
            prefabExceptionWs.Column(10).Width = 18;
            prefabExceptionWs.Column(11).Width = 18;
            prefabExceptionWs.Column(12).Width = 12;
            prefabExceptionWs.Column(13).Width = 18;
            prefabExceptionWs.Column(14).Width = 18;
            prefabExceptionWs.Column(15).Width = 12;
            prefabExceptionWs.Column(16).Width = 18;
            prefabExceptionWs.Column(17).Width = 18;

            //Scene异常引用统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = prefabExceptionWs.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = prefabExceptionWs.Cells[1, 2, 1, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SCENE_EXCEPTION_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = prefabExceptionWs.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SCENE_EXCEPTION_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = prefabExceptionWs.Cells[2, 3, 2, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            // 图例
            tempExcelRange = prefabExceptionWs.Cells[5, 2, 5, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 System.Drawing.Color.FromArgb(255, 255, 255));
            tempExcelRange = prefabExceptionWs.Cells[6, 2, 6, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_CURRENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR);
            tempExcelRange = prefabExceptionWs.Cells[7, 2, 7, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_LAST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            tempExcelRange = prefabExceptionWs.Cells[8, 2, 8, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_EXCEPTION, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 EXCEPTION_DATA_COLOR);

            // Title /////////////////////////////
            int row = 10;
            tempExcelRange = prefabExceptionWs.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SCENE_EXCEPTION_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = prefabExceptionWs.Cells[row, 3, row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = prefabExceptionWs.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = prefabExceptionWs.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = prefabExceptionWs.Cells[row, 9, row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SCENE_TITLE_0, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = prefabExceptionWs.Cells[row, 12, row, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SCENE_TITLE_1, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = prefabExceptionWs.Cells[row, 15, row, 17];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SCENE_TITLE_2, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            //Scene异常引用详情 ///////////////////////////////////////////////////////////
            row++;
            var currentTotalFilesMapping = AssetRecordDataParser.CurrentVerisonFilesMapping;
            int totalFileAmount = 0;
            long currentTotalFileSize = 0;
            Dictionary<string, BaseRecorder> sceneDataMapping = null;
            if (currentTotalFilesMapping.TryGetValue(EAssetType.Scene, out sceneDataMapping))
            {
                if (sceneDataMapping.Count > 0)
                {
                    // Details /////////////////////////////
                    Dictionary<string, BaseRecorder> tempAddedDic = null;
                    AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(EAssetType.Scene, out tempAddedDic);
                    Dictionary<string, BaseRecorder> tempModifiedDic = null;
                    AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(EAssetType.Scene, out tempModifiedDic);
                    System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                    foreach (var baseRecorder in sceneDataMapping.Values)
                    {
                        SceneRecorder sceneRecorder = baseRecorder as SceneRecorder;

                        bool scriptEx = sceneRecorder.m_MissingCompList.Count > 0;
                        bool colliderEx = sceneRecorder.m_CollidersMapping.Count > 0;
                        bool builtinDepEx = sceneRecorder.m_BuiltinDependencies.Count > 0;
  
                        if (!scriptEx && !colliderEx && !builtinDepEx) continue;

                        totalFileAmount++;
                        currentTotalFileSize += baseRecorder.m_FileDiskSize;

                        bool updateAsset = false;
                        if (tempAddedDic != null && tempAddedDic.ContainsKey(baseRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }
                        if (tempModifiedDic != null && tempModifiedDic.ContainsKey(baseRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }

                        if (updateAsset)
                        {
                            itemColor = UPDATE_DATA_COLOR;
                        }

                        int scriptAddRow = sceneRecorder.m_MissingCompList.Count - 1;
                        int colliderAddRow = sceneRecorder.m_CollidersMapping.Count - 1;
                        int builtinDepAddRow = sceneRecorder.m_BuiltinDependencies.Count - 1;
                        int finalAddRow = System.Math.Max(scriptAddRow, System.Math.Max(colliderAddRow, builtinDepAddRow));
                        int finalAddAmount = finalAddRow + 1;
                        tempExcelRange = prefabExceptionWs.Cells[row, 2, row + finalAddRow, 2];
                        ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = prefabExceptionWs.Cells[row, 3, row + finalAddRow, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = prefabExceptionWs.Cells[row, 7, row + finalAddRow, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(sceneRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = prefabExceptionWs.Cells[row, 8, row + finalAddRow, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        // Miss Script
                        tempExcelRange = prefabExceptionWs.Cells[row, 9, row + finalAddRow, 9];
                        ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_MissingCompList.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             scriptEx ? EXCEPTION_DATA_COLOR : itemColor);
                        for (int k = 0; k < finalAddAmount; k++)
                        {
                            tempExcelRange = prefabExceptionWs.Cells[row + k, 10, row + k, 11];
                            string itemStr = (k <= scriptAddRow && scriptAddRow >= 0) ? sceneRecorder.m_MissingCompList[k] : "";
                            ExcelHelper.SetExcelRange(tempExcelRange, itemStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                 itemColor);
                        }
                        // Colliders
                        tempExcelRange = prefabExceptionWs.Cells[row, 12, row + finalAddRow, 12];
                        ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_CollidersMapping.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             colliderEx ? EXCEPTION_DATA_COLOR : itemColor);
                        if (sceneRecorder.m_CollidersMapping.Count == 0)
                        {
                            for (int m = 0; m <= finalAddRow; m++)
                            {
                                tempExcelRange = prefabExceptionWs.Cells[row + m, 13, row + m, 14];
                                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     itemColor);
                            }
                        }
                        else
                        {
                            int k = 0;
                            foreach (var nodePath in sceneRecorder.m_CollidersMapping.Keys)
                            {
                                tempExcelRange = prefabExceptionWs.Cells[row + k, 13, row + k, 14];
                                ExcelHelper.SetExcelRange(tempExcelRange, nodePath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     itemColor);
                                k++;
                            }
                            for (int m = k; m < finalAddAmount; m++)
                            {
                                tempExcelRange = prefabExceptionWs.Cells[row + m, 13, row + m, 14];
                                ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                     itemColor);
                            }
                        }
                        // Builtin Dep
                        tempExcelRange = prefabExceptionWs.Cells[row, 15, row + finalAddRow, 15];
                        ExcelHelper.SetExcelRange(tempExcelRange, sceneRecorder.m_BuiltinDependencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             builtinDepEx ? EXCEPTION_DATA_COLOR : itemColor);
                        for (int k = 0; k < finalAddAmount; k++)
                        {
                            tempExcelRange = prefabExceptionWs.Cells[row + k, 16, row + k, 17];
                            string itemStr = (k <= builtinDepAddRow && builtinDepAddRow >= 0) ? sceneRecorder.m_BuiltinDependencies[k] : "";
                            ExcelHelper.SetExcelRange(tempExcelRange, itemStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                 itemColor);
                        }

                        row += finalAddAmount;
                    }
                }
            }

            // 数据：脚本异常引用统计 /////////////////////////////////////
            tempExcelRange = prefabExceptionWs.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = prefabExceptionWs.Cells[3, 3, 3, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
        }
    }
}
