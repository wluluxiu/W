using System.Linq;
using System.Collections.Generic;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace jj.TATools.Editor
{
    /// <summary>
    /// 当前版本Texture透明度占比详情报告
    /// </summary>
    internal class TextureTransparentReporter
    {
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color SPECIAL_DATA_COLOR_1 = System.Drawing.Color.FromArgb(255, 255, 0);

            ExcelWorksheet currentTransparentWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_6);
            currentTransparentWS.View.ShowGridLines = false;
            currentTransparentWS.View.FreezePanes(6, 1);
            currentTransparentWS.Column(1).Width = 5;
            currentTransparentWS.Column(2).Width = 9;
            currentTransparentWS.Column(3).Width = 18;
            currentTransparentWS.Column(4).Width = 18;
            currentTransparentWS.Column(5).Width = 18;
            currentTransparentWS.Column(6).Width = 18;

            // 图例 ///////////////////////////////////////////////////////////
            ExcelRange tempExcelRange = currentTransparentWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSET_TEXTURE_COMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            // 
            tempExcelRange = currentTransparentWS.Cells[1, 2,1,3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 System.Drawing.Color.FromArgb(255, 255, 255));
            tempExcelRange = currentTransparentWS.Cells[2, 2, 2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_CURRENT, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_1);
            tempExcelRange = currentTransparentWS.Cells[3, 2, 3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_LEGEND_LAST, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);


            // Title ///////////////////////////////////////////////////////////
            tempExcelRange = currentTransparentWS.Cells[5, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_11, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = currentTransparentWS.Cells[5, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = currentTransparentWS.Cells[5, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = currentTransparentWS.Cells[5, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_9, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = currentTransparentWS.Cells[5, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_10, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = currentTransparentWS.Cells[5, 7, 5, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_8, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Details ///////////////////////////////////////////////////////////
            int currentRow = 6;
            var transparentExceptionMapping = TextureDataParser.CurrentVersionTransparentExceptiongMapping;
            var sortedDataMapping = transparentExceptionMapping.OrderByDescending(o => o.m_TransparentPercentage).ToList();
            foreach (var asset in sortedDataMapping)
            {
                string typeName = asset.m_TextureType;
                Dictionary<string, TextureData> tempAssetAddedDic = null;
                if (TextureDataParser.CurrentVersionAddedMapping != null)
                {
                    TextureDataParser.CurrentVersionAddedMapping.TryGetValue(typeName, out tempAssetAddedDic);
                }
                Dictionary<string, TextureData> tempAssetModifiedDic = null;
                if (TextureDataParser.CurrentVersionModifiedMapping != null)
                {
                    TextureDataParser.CurrentVersionModifiedMapping.TryGetValue(typeName, out tempAssetModifiedDic);
                }

                System.Drawing.Color color = CONTENT_DATA_COLOR_1;
                if (tempAssetAddedDic != null && tempAssetAddedDic.ContainsKey(asset.m_TexturePath))
                {
                    color = SPECIAL_DATA_COLOR_1;
                }
                if (tempAssetModifiedDic != null && tempAssetModifiedDic.ContainsKey(asset.m_TexturePath))
                {
                    color = SPECIAL_DATA_COLOR_1;
                }

                tempExcelRange = currentTransparentWS.Cells[currentRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, currentRow - 5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     color);
                tempExcelRange = currentTransparentWS.Cells[currentRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(asset.m_DiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     color);
                tempExcelRange = currentTransparentWS.Cells[currentRow, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(asset.m_GpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     color);
                tempExcelRange = currentTransparentWS.Cells[currentRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.GetResolutionStr(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     color);
                tempExcelRange = currentTransparentWS.Cells[currentRow, 6];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.GetTransparentPertanageStr(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     color);
                tempExcelRange = currentTransparentWS.Cells[currentRow, 7, currentRow, 21];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_TexturePath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     color);

                currentRow++;
            }
        }
    }
}
