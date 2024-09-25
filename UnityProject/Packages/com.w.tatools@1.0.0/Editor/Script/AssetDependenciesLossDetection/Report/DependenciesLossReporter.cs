namespace jj.TATools.Editor
{
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    using UnityEditor;

    internal class DependenciesLossReporter
    {
        #region Local Methods

        static void GenerateContentReport(ExcelPackage package, Dictionary<EDetectionType, List<BaseDetection>> detectionsMapping)
        {
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DepLossUtility.REPORT_CONTENT);
            worksheet.View.ShowGridLines = false;
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 18;
            worksheet.Column(3).Width = 18;
            worksheet.Column(4).Width = 18;
            worksheet.Column(5).Width = 18;

            // 资源依赖丢失统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = worksheet.Cells[2, 2, 2, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_CONTENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[3, 2, 3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[3, 4, 3, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            int row = 4;
            foreach (var data in detectionsMapping)
            {
                var type = data.Key.ToString();
                var lossDataList = data.Value;

                tempExcelRange = worksheet.Cells[row, 2, row, 3];
                ExcelHelper.SetHyperLink(tempExcelRange, type, "A1", type);
                ExcelHelper.SetExcelRange(tempExcelRange, type, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);

                tempExcelRange = worksheet.Cells[row, 4, row, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, lossDataList.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CURRENT_DATA_COLOR_1);

                row++;
            }
        }

        static void GenerateCommonNoPropReportItem(ExcelWorksheet worksheet, List<BaseDetection> lossDataList, int row)
        {
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color CONTENT_DATA_COLOR_2 = System.Drawing.Color.FromArgb(255, 255, 255);

            int startRow = row;
            for (int i = 0; i < lossDataList.Count; i++)
            {
                var baseDetection = lossDataList[i];

                int addRow = baseDetection.m_LossGuids.Count - 1;
                int endRow = startRow + addRow;

                System.Drawing.Color itemColor = (i % 2 == 0) ? CONTENT_DATA_COLOR_2 : CONTENT_DATA_COLOR_1;

                ExcelRange tempExcelRange = worksheet.Cells[startRow, 2, endRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, (i + 1), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, baseDetection.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                for (int k = 0; k < baseDetection.m_LossGuids.Count; k++)
                {
                    var guid = baseDetection.m_LossGuids[k];

                    int currentRow = startRow + k;

                    tempExcelRange = worksheet.Cells[currentRow, 6, currentRow, 10];
                    ExcelHelper.SetExcelRange(tempExcelRange, guid, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                    tempExcelRange = worksheet.Cells[currentRow, 11, currentRow, 13];
                    ExcelHelper.SetExcelRange(tempExcelRange, AssetDatabase.GUIDToAssetPath(guid), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                }

                startRow += baseDetection.m_LossGuids.Count;
            }
        }

        static void GenerateAnimatorControllerReportItem(ExcelWorksheet worksheet, List<BaseDetection> lossDataList, int row)
        {
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color CONTENT_DATA_COLOR_2 = System.Drawing.Color.FromArgb(255, 255, 255);

            int startRow = row;
            for (int i = 0; i < lossDataList.Count; i++)
            {
                var animatorControllerDetection = lossDataList[i] as AnimatorControllerDetection;

                System.Drawing.Color itemColor = (i % 2 == 0) ? CONTENT_DATA_COLOR_2 : CONTENT_DATA_COLOR_1;

                ExcelRange tempExcelRange = null;

                int currentRow = startRow;
                int addRow = 0;
                for (int j = 0; j < animatorControllerDetection.m_LossDataList.Count; j++)
                {
                    var animatorData = animatorControllerDetection.m_LossDataList[j];
                    var stateName = animatorData.m_StateName;

                    int childStartRow = currentRow;

                    if (!string.IsNullOrEmpty(animatorData.m_MoitonLossGuid))
                    {
                        addRow++;
                        tempExcelRange = worksheet.Cells[currentRow, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, "[Motion]", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                               ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                               itemColor);

                        tempExcelRange = worksheet.Cells[currentRow, 9, currentRow, 10];
                        ExcelHelper.SetExcelRange(tempExcelRange, animatorData.m_MoitonLossGuid, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                               ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                               itemColor);

                        tempExcelRange = worksheet.Cells[currentRow, 11, currentRow, 13];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDatabase.GUIDToAssetPath(animatorData.m_MoitonLossGuid), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                               ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                               itemColor);

                        currentRow++;
                    }

                    if (animatorData.m_BehaivourLossGuids != null)
                    {
                        for (int k = 0; k < animatorData.m_BehaivourLossGuids.Count; k++)
                        {
                            addRow++;

                            var guid = animatorData.m_BehaivourLossGuids[k];

                            tempExcelRange = worksheet.Cells[currentRow, 8];
                            ExcelHelper.SetExcelRange(tempExcelRange, "[Script]", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);

                            tempExcelRange = worksheet.Cells[currentRow, 9, currentRow, 10];
                            ExcelHelper.SetExcelRange(tempExcelRange,guid, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);

                            tempExcelRange = worksheet.Cells[currentRow, 11, currentRow, 13];
                            ExcelHelper.SetExcelRange(tempExcelRange, AssetDatabase.GUIDToAssetPath(guid), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);

                            currentRow++;
                        }
                    }

                    tempExcelRange = worksheet.Cells[childStartRow, 6, currentRow - 1, 7];
                    ExcelHelper.SetExcelRange(tempExcelRange, stateName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);
                }

                int endRow = currentRow - 1;
                tempExcelRange = worksheet.Cells[startRow, 2, endRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, (i + 1), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, animatorControllerDetection.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                startRow = endRow + 1;
            }
        }

        static void GenerateAnimatorOverrideControllerReportItem(ExcelWorksheet worksheet, List<BaseDetection> lossDataList, int row)
        {
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color CONTENT_DATA_COLOR_2 = System.Drawing.Color.FromArgb(255, 255, 255);

            int startRow = row;
            for (int i = 0; i < lossDataList.Count; i++)
            {
                var animatorOverrideControllerDetection = lossDataList[i] as AnimatorOverrideControllerDetection;

                System.Drawing.Color itemColor = (i % 2 == 0) ? CONTENT_DATA_COLOR_2 : CONTENT_DATA_COLOR_1;

                ExcelRange tempExcelRange = null;

                int currentRow = startRow;
                if (!string.IsNullOrEmpty(animatorOverrideControllerDetection.m_LossControllerGuid))
                {
                    tempExcelRange = worksheet.Cells[currentRow, 6, currentRow, 7];
                    ExcelHelper.SetExcelRange(tempExcelRange, "[Controller]", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                    tempExcelRange = worksheet.Cells[currentRow, 8, currentRow, 10];
                    ExcelHelper.SetExcelRange(tempExcelRange, animatorOverrideControllerDetection.m_LossControllerGuid, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                    tempExcelRange = worksheet.Cells[currentRow, 11, currentRow, 13];
                    ExcelHelper.SetExcelRange(tempExcelRange, AssetDatabase.GUIDToAssetPath(animatorOverrideControllerDetection.m_LossControllerGuid), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                    currentRow++;
                }

                if (animatorOverrideControllerDetection.m_LossGuids != null)
                {
                    for (int k = 0; k < animatorOverrideControllerDetection.m_LossGuids.Count; k++)
                    {
                        var guid = animatorOverrideControllerDetection.m_LossGuids[k];

                        tempExcelRange = worksheet.Cells[currentRow, 6, currentRow, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, "[OverrideClip]", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                               ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                               itemColor);

                        tempExcelRange = worksheet.Cells[currentRow, 8, currentRow, 10];
                        ExcelHelper.SetExcelRange(tempExcelRange, guid, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                               ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                               itemColor);

                        tempExcelRange = worksheet.Cells[currentRow, 11, currentRow, 13];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDatabase.GUIDToAssetPath(guid), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                               ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                               itemColor);

                        currentRow++;
                    }
                }

                int endRow = currentRow - 1;
                tempExcelRange = worksheet.Cells[startRow, 2, endRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, (i + 1), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, animatorOverrideControllerDetection.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                startRow = currentRow;
            }
        }

        static void GenerateCommonPropReportItem(ExcelWorksheet worksheet, List<BaseDetection> lossDataList, int row)
        {
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color CONTENT_DATA_COLOR_2 = System.Drawing.Color.FromArgb(255, 255, 255);

            int startRow = row;
            for (int i = 0; i < lossDataList.Count; i++)
            {
                var baseDetection = lossDataList[i];

                int addRow = baseDetection.m_LossGuids.Count - 1;
                int endRow = startRow + addRow;

                System.Drawing.Color itemColor = (i % 2 == 0) ? CONTENT_DATA_COLOR_2 : CONTENT_DATA_COLOR_1;

                ExcelRange tempExcelRange = worksheet.Cells[startRow, 2, endRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, (i + 1), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, baseDetection.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                for (int k = 0; k < baseDetection.m_LossGuids.Count; k++)
                {
                    var guid = baseDetection.m_LossGuids[k];

                    int currentRow = startRow + k;
                    tempExcelRange = worksheet.Cells[currentRow, 6, currentRow, 7];
                    ExcelHelper.SetExcelRange(tempExcelRange, baseDetection.m_LossGuidPropNames[k], ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                    tempExcelRange = worksheet.Cells[currentRow, 8, currentRow, 10];
                    ExcelHelper.SetExcelRange(tempExcelRange, guid, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                    tempExcelRange = worksheet.Cells[currentRow, 11, currentRow, 13];
                    ExcelHelper.SetExcelRange(tempExcelRange, AssetDatabase.GUIDToAssetPath(guid), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                }

                startRow += baseDetection.m_LossGuids.Count;
            }
        }

        static void GenerateSpriteAtlasReportItem(ExcelWorksheet worksheet, List<BaseDetection> lossDataList, int row)
        {
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color CONTENT_DATA_COLOR_2 = System.Drawing.Color.FromArgb(255, 255, 255);

            int startRow = row;
            for (int i = 0; i < lossDataList.Count; i++)
            {
                var spriteAtlasDetection = lossDataList[i] as SpriteAtlasDetection;

                System.Drawing.Color itemColor = (i % 2 == 0) ? CONTENT_DATA_COLOR_2 : CONTENT_DATA_COLOR_1;

                ExcelRange tempExcelRange = null;

                int currentRow = startRow;
                if (!string.IsNullOrEmpty(spriteAtlasDetection.m_LossMasterGuid))
                {
                    tempExcelRange = worksheet.Cells[currentRow, 6, currentRow, 7];
                    ExcelHelper.SetExcelRange(tempExcelRange, "[Master Atlas]", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                    tempExcelRange = worksheet.Cells[currentRow, 8, currentRow, 10];
                    ExcelHelper.SetExcelRange(tempExcelRange, spriteAtlasDetection.m_LossMasterGuid, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                    tempExcelRange = worksheet.Cells[currentRow, 11, currentRow, 13];
                    ExcelHelper.SetExcelRange(tempExcelRange, AssetDatabase.GUIDToAssetPath(spriteAtlasDetection.m_LossMasterGuid), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);

                    currentRow++;
                }

                if (spriteAtlasDetection.m_LossGuids != null)
                {
                    for (int k = 0; k < spriteAtlasDetection.m_LossGuids.Count; k++)
                    {
                        var guid = spriteAtlasDetection.m_LossGuids[k];

                        tempExcelRange = worksheet.Cells[currentRow, 6, currentRow, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, "[Sprite]", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                               ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                               itemColor);

                        tempExcelRange = worksheet.Cells[currentRow, 8, currentRow, 10];
                        ExcelHelper.SetExcelRange(tempExcelRange, guid, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                               ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                               itemColor);

                        tempExcelRange = worksheet.Cells[currentRow, 11, currentRow, 13];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDatabase.GUIDToAssetPath(guid), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                               ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                               itemColor);

                        currentRow++;
                    }
                }

                int endRow = currentRow - 1;
                tempExcelRange = worksheet.Cells[startRow, 2, endRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, (i + 1), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, spriteAtlasDetection.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                startRow = currentRow;
            }
        }

        static void GenerateSceneReportItem(ExcelWorksheet worksheet, List<BaseDetection> lossDataList, int row)
        {
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color CONTENT_DATA_COLOR_2 = System.Drawing.Color.FromArgb(255, 255, 255);

            int startRow = row;
            for (int i = 0; i < lossDataList.Count; i++)
            {
                var sceneDetection = lossDataList[i] as SceneDetection;

                System.Drawing.Color itemColor = (i % 2 == 0) ? CONTENT_DATA_COLOR_2 : CONTENT_DATA_COLOR_1;

                ExcelRange tempExcelRange = null;

                int currentRow = startRow;
                int addRow = 0;
                for (int j = 0; j < sceneDetection.m_LossDataList.Count; j++)
                {
                    var sceneNodeData = sceneDetection.m_LossDataList[j];
                    var nodeRelativePath = sceneNodeData.RelativeNodePath;

                    int childStartRow = currentRow;

                    if (sceneNodeData.m_LossGuids != null)
                    {
                        for (int k = 0; k < sceneNodeData.m_LossGuids.Count; k++)
                        {
                            addRow++;

                            var guid = sceneNodeData.m_LossGuids[k];
                            var guidProp = sceneNodeData.m_LossGuidPropNames[k];

                            tempExcelRange = worksheet.Cells[currentRow, 8];
                            ExcelHelper.SetExcelRange(tempExcelRange,"[" + guidProp + "]", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);

                            tempExcelRange = worksheet.Cells[currentRow, 9, currentRow, 10];
                            ExcelHelper.SetExcelRange(tempExcelRange, guid, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);

                            tempExcelRange = worksheet.Cells[currentRow, 11, currentRow, 13];
                            ExcelHelper.SetExcelRange(tempExcelRange, AssetDatabase.GUIDToAssetPath(guid), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);

                            currentRow++;
                        }
                    }

                    tempExcelRange = worksheet.Cells[childStartRow, 6, currentRow - 1, 7];
                    ExcelHelper.SetExcelRange(tempExcelRange, nodeRelativePath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);
                }

                int endRow = currentRow - 1;
                tempExcelRange = worksheet.Cells[startRow, 2, endRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, (i + 1), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, sceneDetection.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                startRow = endRow + 1;
            }
        }

        static void GeneratePrefabReportItem(ExcelWorksheet worksheet, List<BaseDetection> lossDataList, int row)
        {
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color CONTENT_DATA_COLOR_2 = System.Drawing.Color.FromArgb(255, 255, 255);

            int startRow = row;
            for (int i = 0; i < lossDataList.Count; i++)
            {
                var prefabDetection = lossDataList[i] as PrefabDetection;

                System.Drawing.Color itemColor = (i % 2 == 0) ? CONTENT_DATA_COLOR_2 : CONTENT_DATA_COLOR_1;

                ExcelRange tempExcelRange = null;

                int currentRow = startRow;
                int addRow = 0;
                for (int j = 0; j < prefabDetection.m_LossDataList.Count; j++)
                {
                    var sceneNodeData = prefabDetection.m_LossDataList[j];
                    var nodeRelativePath = sceneNodeData.RelativeNodePath;

                    int childStartRow = currentRow;

                    if (sceneNodeData.m_LossGuids != null)
                    {
                        for (int k = 0; k < sceneNodeData.m_LossGuids.Count; k++)
                        {
                            addRow++;

                            var guid = sceneNodeData.m_LossGuids[k];
                            var guidProp = sceneNodeData.m_LossGuidPropNames[k];

                            tempExcelRange = worksheet.Cells[currentRow, 8];
                            ExcelHelper.SetExcelRange(tempExcelRange, "[" + guidProp + "]", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);

                            tempExcelRange = worksheet.Cells[currentRow, 9, currentRow, 10];
                            ExcelHelper.SetExcelRange(tempExcelRange, guid, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);

                            tempExcelRange = worksheet.Cells[currentRow, 11, currentRow, 13];
                            ExcelHelper.SetExcelRange(tempExcelRange, AssetDatabase.GUIDToAssetPath(guid), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                                   ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                   itemColor);

                            currentRow++;
                        }
                    }

                    tempExcelRange = worksheet.Cells[childStartRow, 6, currentRow - 1, 7];
                    ExcelHelper.SetExcelRange(tempExcelRange, nodeRelativePath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                           ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                           itemColor);
                }

                int endRow = currentRow - 1;
                tempExcelRange = worksheet.Cells[startRow, 2, endRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, (i + 1), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                tempExcelRange = worksheet.Cells[startRow, 3, endRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, prefabDetection.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                       ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                       itemColor);

                startRow = endRow + 1;
            }
        }

        static void GenerateDetailsReport(ExcelPackage package, EDetectionType type, List<BaseDetection> lossDataList)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(255, 217, 102);

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(type.ToString());
            worksheet.View.ShowGridLines = false;
            worksheet.View.FreezePanes(3, 1);
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 10;
            worksheet.Column(3).Width = 18;
            worksheet.Column(4).Width = 18;
            worksheet.Column(5).Width = 18;
            worksheet.Column(6).Width = 12;
            worksheet.Column(7).Width = 12;
            worksheet.Column(8).Width = 24;
            worksheet.Column(9).Width = 20;
            worksheet.Column(10).Width = 20;
            worksheet.Column(11).Width = 15;
            worksheet.Column(12).Width = 15;
            worksheet.Column(13).Width = 15;

            // Return Content
            ExcelRange tempExcelRange = worksheet.Cells[1, 1, 2, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, DepLossUtility.REPORT_CONTENT, "A1", DepLossUtility.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);

            // Title
            tempExcelRange = worksheet.Cells[1, 2, 2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[1, 3, 2, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[1, 6, 1, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         CONTENT_DATA_COLOR_0);
            if (type == EDetectionType.Prefab || type == EDetectionType.Scene)
            {
                tempExcelRange = worksheet.Cells[2, 6, 2, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             CONTENT_DATA_COLOR_0);

                tempExcelRange = worksheet.Cells[2, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             CONTENT_DATA_COLOR_0);

                tempExcelRange = worksheet.Cells[2, 9, 2, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             CONTENT_DATA_COLOR_0);
            }
            else if (type == EDetectionType.AnimatorController)
            {
                tempExcelRange = worksheet.Cells[2, 6, 2, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             CONTENT_DATA_COLOR_0);

                tempExcelRange = worksheet.Cells[2, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             CONTENT_DATA_COLOR_0);

                tempExcelRange = worksheet.Cells[2, 9, 2, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             CONTENT_DATA_COLOR_0);
            }
            else if (type == EDetectionType.Material || type == EDetectionType.AnimatorOverrideController || 
                type == EDetectionType.Script || type == EDetectionType.Shader || type == EDetectionType.SpriteAtlas ||
                type == EDetectionType.TMP_SDF || type == EDetectionType.TMP_Setting || type == EDetectionType.TMP_Sprite)
            {
                tempExcelRange = worksheet.Cells[2, 6, 2, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             CONTENT_DATA_COLOR_0);

                tempExcelRange = worksheet.Cells[2, 8, 2, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             CONTENT_DATA_COLOR_0);
            }
            else
            {
                tempExcelRange = worksheet.Cells[2, 6, 2, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             CONTENT_DATA_COLOR_0);
            }
 
            tempExcelRange = worksheet.Cells[2, 11, 2, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, DepLossUtility.REPORT_DETIAL_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         CONTENT_DATA_COLOR_0);

            // Item
            int row = 3;
            if (type == EDetectionType.Normal || type == EDetectionType.Asmdef)
                GenerateCommonNoPropReportItem(worksheet, lossDataList, row);
            else if (type == EDetectionType.AnimatorController)
                GenerateAnimatorControllerReportItem(worksheet, lossDataList, row);
            else if (type == EDetectionType.AnimatorOverrideController)
                GenerateAnimatorOverrideControllerReportItem(worksheet, lossDataList, row);
            else if (type == EDetectionType.Material || type == EDetectionType.Script || type == EDetectionType.Shader ||
                type == EDetectionType.TMP_SDF || type == EDetectionType.TMP_Setting || type == EDetectionType.TMP_Sprite)
                GenerateCommonPropReportItem(worksheet, lossDataList, row);
            else if (type == EDetectionType.Prefab)
                GeneratePrefabReportItem(worksheet, lossDataList, row);
            else if (type == EDetectionType.Scene)
                GenerateSceneReportItem(worksheet, lossDataList, row);
            else if (type == EDetectionType.SpriteAtlas)
                GenerateSpriteAtlasReportItem(worksheet, lossDataList, row);

        }

        #endregion

        #region Internal Methods

        internal static bool GenerateReport(Dictionary<EDetectionType, List<BaseDetection>> detectionsMapping,string reportFilePath)
        {
            FileInfo sourFile = null;
            try
            {
                sourFile = new FileInfo(reportFilePath);
                if (sourFile.Exists)
                {
                    sourFile.Delete();
                    sourFile = new FileInfo(reportFilePath);
                }
            }
            catch (System.Exception e)
            {
                DepLossUtility.MessageBox(DepLossUtility.REPORT_FILE_NAME, "[" + Path.GetFileName(reportFilePath) + "]文件已经被打开，请关闭后重新生成报告！", "OK");
                return false;
            }

            ExcelHelper.SetExcelPackageLicenseContextProperty(LicenseContext.NonCommercial);

            using (ExcelPackage package = new ExcelPackage(sourFile))
            {
                var sortedDataMapping = detectionsMapping.OrderByDescending(o => o.Value.Count).ToDictionary(o => o.Key, p => p.Value);

                // Content Report
                GenerateContentReport(package, sortedDataMapping);

                // Details Report
                foreach (var data in sortedDataMapping)
                {
                    GenerateDetailsReport(package, data.Key, data.Value);
                }

                package.Save();
            }
           

            return true;
        }

        #endregion
    }
}