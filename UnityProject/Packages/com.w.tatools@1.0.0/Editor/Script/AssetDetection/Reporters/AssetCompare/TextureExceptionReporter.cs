using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace jj.TATools.Editor
{
    /// <summary>
    ///  纹理异常详情报告
    /// </summary>
    internal class TextureExceptionReporter
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

            ExcelWorksheet textureExceptionWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_7);
            textureExceptionWS.View.ShowGridLines = false;
            textureExceptionWS.View.FreezePanes(11, 1);
            textureExceptionWS.Column(1).Width = 5;
            textureExceptionWS.Column(2).Width = 10;
            textureExceptionWS.Column(3).Width = 18;
            textureExceptionWS.Column(4).Width = 18;
            textureExceptionWS.Column(5).Width = 18;
            textureExceptionWS.Column(6).Width = 18;
            textureExceptionWS.Column(7).Width = 18;
            textureExceptionWS.Column(8).Width = 18;
            textureExceptionWS.Column(9).Width = 18;
            textureExceptionWS.Column(10).Width = 18;
            textureExceptionWS.Column(11).Width = 18;
            textureExceptionWS.Column(12).Width = 18;
            textureExceptionWS.Column(13).Width = 18;
            textureExceptionWS.Column(14).Width = 18;
            textureExceptionWS.Column(15).Width = 18;


            // 当前版本异常纹理统计 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = textureExceptionWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = textureExceptionWS.Cells[1, 2, 1, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_EXCEPTION_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_EXCEPTION_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[2, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_EXCEPTION_CONTENT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);

            // 图例
            tempExcelRange = textureExceptionWS.Cells[5, 2, 5, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 System.Drawing.Color.FromArgb(255, 255, 255));
            tempExcelRange = textureExceptionWS.Cells[6, 2, 6, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_CURRENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 UPDATE_DATA_COLOR);
            tempExcelRange = textureExceptionWS.Cells[7, 2, 7, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_LAST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);
            tempExcelRange = textureExceptionWS.Cells[8, 2, 8, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_EXCEPTION, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 EXCEPTION_DATA_COLOR);

            // Title /////////////////////////////
            int row = 10;
            tempExcelRange = textureExceptionWS.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_EXCEPTION_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[row, 3, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[row, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[row, 11, row, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[row, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[row, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[row, 15];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_TEXTURE_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // 当前版本异常纹理详情 ///////////////////////////////////////////////////////////
            row++;
            var currentTotalFilesMapping = AssetRecordDataParser.CurrentVerisonFilesMapping;
            int totalFileAmount = 0;
            long currentTotalFileSize = 0;
            long currentTotalGPUSize = 0;
            // Texture
            Dictionary<string, BaseRecorder> textureDataMapping = null;
            if (currentTotalFilesMapping.TryGetValue(EAssetType.Texture, out textureDataMapping))
            {
                if (textureDataMapping.Count > 0)
                {
                    // Details /////////////////////////////
                    Dictionary<string, BaseRecorder> tempAddedDic = null;
                    AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(EAssetType.Texture, out tempAddedDic);
                    Dictionary<string, BaseRecorder> tempModifiedDic = null;
                    AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(EAssetType.Texture, out tempModifiedDic);
                    System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                    foreach (var baseRecorder in textureDataMapping.Values)
                    {
                        TextureRecorder textureRecorder = baseRecorder as TextureRecorder;

                        bool anisoLevelEx = textureRecorder.AnisoLevelInvalid();
                        bool rwEx = textureRecorder.RWInvalid();
                        bool formatEx = textureRecorder.FormatInvalid();
                        bool filterModeEx = textureRecorder.FilterModeInvalid();
                        bool resolutionEx = textureRecorder.ResolutionInvalid();
                        bool potEx = textureRecorder.POTInvalid();

                        if (!anisoLevelEx && !rwEx && !formatEx && !filterModeEx && !resolutionEx) continue;

                        totalFileAmount++;
                        currentTotalFileSize += textureRecorder.m_FileDiskSize;
                        currentTotalGPUSize += textureRecorder.m_MemorySize;

                        bool updateAsset = false;
                        if (tempAddedDic != null && tempAddedDic.ContainsKey(textureRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }
                        if (tempModifiedDic != null && tempModifiedDic.ContainsKey(textureRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }

                        if (updateAsset)
                        {
                            itemColor = UPDATE_DATA_COLOR;
                        }

                        tempExcelRange = textureExceptionWS.Cells[row, 2];
                        ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 3, row, 5];
                        ExcelHelper.SetExcelRange(tempExcelRange, textureRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(textureRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(textureRecorder.m_MemorySize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, textureRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 9];
                        ExcelHelper.SetExcelRange(tempExcelRange, rwEx ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             rwEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 10];
                        ExcelHelper.SetExcelRange(tempExcelRange, textureRecorder.m_Width + "x" + textureRecorder.m_Height, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             (resolutionEx || potEx) ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 11, row, 12];
                        ExcelHelper.SetExcelRange(tempExcelRange, (ETextureImporterFormat)textureRecorder.m_FormatAndroid + "|" + (ETextureImporterFormat)textureRecorder.m_FormatIOS, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             formatEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 13];
                        ExcelHelper.SetExcelRange(tempExcelRange, textureRecorder.m_Mipmap == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 14];
                        ExcelHelper.SetExcelRange(tempExcelRange, textureRecorder.m_AnisoLevel, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             anisoLevelEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 15];
                        ExcelHelper.SetExcelRange(tempExcelRange, ((EFilterMode)textureRecorder.m_FilterMode).ToString(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             filterModeEx ? EXCEPTION_DATA_COLOR : itemColor);

                        row++;
                    }
                }
            }
            // RenderTexture
            Dictionary<string, BaseRecorder> rtDataMapping = null;
            if (currentTotalFilesMapping.TryGetValue(EAssetType.RenderTexture, out rtDataMapping))
            {
                if (rtDataMapping.Count > 0)
                {
                    // Details /////////////////////////////
                    Dictionary<string, BaseRecorder> tempAddedDic = null;
                    AssetRecordDataParser.CurrentVersionAddedMapping.TryGetValue(EAssetType.RenderTexture, out tempAddedDic);
                    Dictionary<string, BaseRecorder> tempModifiedDic = null;
                    AssetRecordDataParser.CurrentVersionModifiedMapping.TryGetValue(EAssetType.RenderTexture, out tempModifiedDic);
                    System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
                    foreach (var baseRecorder in rtDataMapping.Values)
                    {
                        RenderTextureRecorder rtRecorder = baseRecorder as RenderTextureRecorder;

                        bool rwEx = rtRecorder.RWInvalid();
                        bool fliterModeEx = rtRecorder.FilterModeInvalid();
                        bool resolutionEx = rtRecorder.ResolutionInvalid();
                        bool potEx = rtRecorder.POTInvalid();

                        if (!rwEx && !fliterModeEx && !resolutionEx) continue;

                        totalFileAmount++;
                        currentTotalFileSize += rtRecorder.m_FileDiskSize;
                        currentTotalGPUSize += rtRecorder.m_MemorySize;

                        bool updateAsset = false;
                        if (tempAddedDic != null && tempAddedDic.ContainsKey(rtRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }
                        if (tempModifiedDic != null && tempModifiedDic.ContainsKey(rtRecorder.m_AssetPath))
                        {
                            updateAsset = true;
                        }

                        if (updateAsset)
                        {
                            itemColor = UPDATE_DATA_COLOR;
                        }

                        tempExcelRange = textureExceptionWS.Cells[row, 2];
                        ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 3, row, 5];
                        ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 6];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(rtRecorder.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 7];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(rtRecorder.m_MemorySize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 8];
                        ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_Referencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 9];
                        ExcelHelper.SetExcelRange(tempExcelRange, rwEx ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             rwEx ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 10];
                        ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_Width + "x" + rtRecorder.m_Height, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             (resolutionEx || potEx) ? EXCEPTION_DATA_COLOR : itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 11, row, 12];
                        ExcelHelper.SetExcelRange(tempExcelRange, "-", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 13];
                        ExcelHelper.SetExcelRange(tempExcelRange, rtRecorder.m_EnableMipmaps == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 14];
                        ExcelHelper.SetExcelRange(tempExcelRange, "-", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = textureExceptionWS.Cells[row, 15];
                        ExcelHelper.SetExcelRange(tempExcelRange, ((EFilterMode)rtRecorder.m_FilterMode).ToString(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, false,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             fliterModeEx ? EXCEPTION_DATA_COLOR : itemColor);

                        row++;
                    }
                }
            }

            // 数据：当前版本异常纹理统计 /////////////////////////////////////
            tempExcelRange = textureExceptionWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, totalFileAmount, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalFileSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = textureExceptionWS.Cells[3, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentTotalGPUSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
        }
    }
}
