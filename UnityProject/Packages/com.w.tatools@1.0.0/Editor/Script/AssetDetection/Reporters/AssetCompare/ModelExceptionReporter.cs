using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace jj.TATools.Editor
{
    /// <summary>
    ///  Model异常详情报告
    /// </summary>
    internal class ModelExceptionReporter
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

            ExcelWorksheet modelExceptionWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_8);
            modelExceptionWS.View.ShowGridLines = false;
            modelExceptionWS.View.FreezePanes(11, 1);
            modelExceptionWS.Column(1).Width = 5;
            modelExceptionWS.Column(2).Width = 10;
            modelExceptionWS.Column(3).Width = 18;
            modelExceptionWS.Column(4).Width = 18;
            modelExceptionWS.Column(5).Width = 18;
            modelExceptionWS.Column(6).Width = 18;
            modelExceptionWS.Column(7).Width = 14;
            modelExceptionWS.Column(8).Width = 14;
            modelExceptionWS.Column(9).Width = 14;
            modelExceptionWS.Column(10).Width = 14;
            modelExceptionWS.Column(11).Width = 14;
            modelExceptionWS.Column(12).Width = 14;
            modelExceptionWS.Column(13).Width = 14;
            modelExceptionWS.Column(14).Width = 14;
            modelExceptionWS.Column(15).Width = 14;
            modelExceptionWS.Column(16).Width = 14;
            modelExceptionWS.Column(17).Width = 14;


            // 当前版本异常模型统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = modelExceptionWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = modelExceptionWS.Cells[1, 2, 1, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_MODEL_EXCEPTION_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_MODEL_EXCEPTION_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            // 图例
            tempExcelRange = modelExceptionWS.Cells[5, 2, 5, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 System.Drawing.Color.FromArgb(255, 255, 255));
            tempExcelRange = modelExceptionWS.Cells[6, 2, 6, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_CURRENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR);
            tempExcelRange = modelExceptionWS.Cells[7, 2, 7, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_LAST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            tempExcelRange = modelExceptionWS.Cells[8, 2, 8, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_EXCEPTION, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 EXCEPTION_DATA_COLOR);

            // Title /////////////////////////////
            int row = 10;
            tempExcelRange = modelExceptionWS.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_MODEL_EXCEPTION_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 3, row, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 15];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_9, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 16];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_8, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[row, 17];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_MODEL_TITLE_10, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // 当前版本异常模型详情 ///////////////////////////////////////////////////////////
            row++;
            var currentTotalFilesMapping = AssetRecordDataParser.CurrentVerisonFilesMapping;
            int totalFileAmount = 0;
            long currentTotalFileSize = 0;
            // Model
            Dictionary<string, BaseRecorder> modelDataMapping = null;
            if (currentTotalFilesMapping.TryGetValue(EAssetType.Model, out modelDataMapping))
            {
                if (modelDataMapping.Count > 0)
                {
                    // Details /////////////////////////////
                    Dictionary<string, BaseRecorder> tempAddedDic = null;
                    AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(EAssetType.Model, out tempAddedDic);
                    Dictionary<string, BaseRecorder> tempModifiedDic = null;
                    AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(EAssetType.Model, out tempModifiedDic);
                    System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                    foreach (var baseRecorder in modelDataMapping.Values)
                    {
                        ModelRecorder modelRecorder = baseRecorder as ModelRecorder;

                        bool boneEx = modelRecorder.BoneCountInvalid();
                        bool uvEx = modelRecorder.UVChannalInvalid();
                        bool blendShapeEx = modelRecorder.BlendShapeInvalid();
                        bool generateUV2Ex = modelRecorder.GenerateUV2Invalid();
                        bool animCompressionEx = modelRecorder.AnimCompressionInvalid();
                        bool triangleEx = modelRecorder.TriangleInvalid();
                        bool vertexCountEx = modelRecorder.VertexCountInvalid();
                        bool vertexColorEx = modelRecorder.VertexColorInvalid();
                        bool rwEx = modelRecorder.RWInvalid();
                        bool refEx = modelRecorder.ReferenciesInvalid();

                        if (!boneEx && !rwEx && !uvEx && !blendShapeEx &&
                            !generateUV2Ex && !animCompressionEx && !refEx &&
                            !triangleEx && !vertexCountEx && !vertexColorEx) continue;

                        totalFileAmount++;
                        currentTotalFileSize += modelRecorder.m_FileDiskSize;

                        bool updateAsset = false;
                        if (tempAddedDic != null && tempAddedDic.ContainsKey(modelRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }
                        if (tempModifiedDic != null && tempModifiedDic.ContainsKey(modelRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }

                        if (updateAsset)
                        {
                            itemColor = UPDATE_DATA_COLOR;
                        }

                        tempExcelRange = modelExceptionWS.Cells[row, 2];
                        ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 3, row, 4];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 5];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(modelRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             refEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, rwEx ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             rwEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Triangle, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             triangleEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 9];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_VertexCount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             vertexCountEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 10];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Bones, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             boneEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 11];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Normal == 1 ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 12];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Tangent == 1 ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 13];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_Color == 1 ? "有" : "无", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             vertexColorEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 14];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.GetUVShowStr(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             uvEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 15];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_GenerateLightmapUVs == 1 ? "开启":"关闭", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             generateUV2Ex ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 16];
                        ExcelHelper.SetExcelRange(tempExcelRange, modelRecorder.m_BlendShapeCount, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             blendShapeEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = modelExceptionWS.Cells[row, 17];
                        ExcelHelper.SetExcelRange(tempExcelRange, ((EModelImporterAnimationCompression)modelRecorder.m_AnimCompression).ToString(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             animCompressionEx ? EXCEPTION_DATA_COLOR : itemColor);

                        row++;
                    }
                }
            }

            // 数据：当前版本异常模型统计 /////////////////////////////////////
            tempExcelRange = modelExceptionWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = modelExceptionWS.Cells[3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
        }
    }
}
