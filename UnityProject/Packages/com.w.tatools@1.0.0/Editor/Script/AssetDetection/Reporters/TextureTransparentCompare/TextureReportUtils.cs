using System.Collections.Generic;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace jj.TATools.Editor
{
    internal class Asset2DReportUtils
    {
        internal static int GenerateCommon2DReportList(ExcelWorksheet worksheet, int row, string typeShowName, string typeName, Dictionary<string, TextureData> assetDataMapping, Dictionary<string, string> hyperLinkToTotalMapping,
           System.Drawing.Color CONTENT_DATA_COLOR_0,
           System.Drawing.Color CONTENT_DATA_COLOR_1,
           System.Drawing.Color CONTENT_DATA_COLOR_2,
           Dictionary<string, long> filesSizeMapping,
           Dictionary<string, long> filesBuildSizeMapping)
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
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_9, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_10, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = worksheet.Cells[row, 7, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_TEXTURE_TOTAL_CONTENT_TITLE_8, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
           
            // Total /////////////////////////////
            var currentFilesSizeData = filesSizeMapping;
            var currentFilesBuildSizeData = filesBuildSizeMapping;
            long currentFileSize = 0;
            if (!currentFilesSizeData.TryGetValue(typeName, out currentFileSize)) currentFileSize = 0;
            long currentFileBuildSize = 0;
            if (!currentFilesBuildSizeData.TryGetValue(typeName, out currentFileBuildSize)) currentFileBuildSize = 0;
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
            tempExcelRange = worksheet.Cells[row, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(currentFileBuildSize), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);
            tempExcelRange = worksheet.Cells[row, 5, row, 21];
            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_2);

            // Details /////////////////////////////
            row++;
            tempExcelRange = worksheet.Cells[row, 2, row + assetDataMapping.Count - 1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, typeShowName, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_1);

            System.Drawing.Color invalidDataColor = System.Drawing.Color.FromArgb(255,0,0);
            foreach (var asset in assetDataMapping.Values)
            {
                var resolutionStr = asset.GetResolutionStr();
                System.Drawing.Color itemColor = string.IsNullOrEmpty(resolutionStr) ? invalidDataColor : CONTENT_DATA_COLOR_1;

                ExcelHelper.SetExcelRow(worksheet, row, 1);
                tempExcelRange = worksheet.Cells[row, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(asset.m_DiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[row, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(asset.m_GpuSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[row, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, resolutionStr, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[row, 6];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.GetTransparentPertanageStr(), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, false,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = worksheet.Cells[row, 7,row,21];
                ExcelHelper.SetExcelRange(tempExcelRange, asset.m_TexturePath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
               
                row++;
            }

            return row;
        }
    }
}
