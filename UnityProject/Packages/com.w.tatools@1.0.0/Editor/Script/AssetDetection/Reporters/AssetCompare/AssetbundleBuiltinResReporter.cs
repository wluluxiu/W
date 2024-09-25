using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Linq;

namespace jj.TATools.Editor
{
    /// <summary>
    ///  Assetbunle内置资源详情报告
    /// </summary>
    internal class AssetbundleBuiltinResReporter
    {
        /// <summary>
        /// 打包方式不同，需要重构
        /// </summary>
        /// <param name="package"></param>
        internal static void GenerateReport(ExcelPackage package)
        {
            System.Drawing.Color RETURN_COLOR = System.Drawing.Color.FromArgb(6, 239, 248);
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color CURRENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(226, 239, 218);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);

            ExcelWorksheet abBuiltinExceptionWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_ASSETCOMPARE_SHEET_16);
            abBuiltinExceptionWS.View.ShowGridLines = false;
            abBuiltinExceptionWS.View.FreezePanes(2, 1);
            abBuiltinExceptionWS.Column(1).Width = 5;
            abBuiltinExceptionWS.Column(2).Width = 14;
            abBuiltinExceptionWS.Column(3).Width = 40;
            abBuiltinExceptionWS.Column(4).Width = 20;
            abBuiltinExceptionWS.Column(5).Width = 100;
           

            ExcelRange tempExcelRange = abBuiltinExceptionWS.Cells[1, 1];
            ExcelHelper.SetHyperLink(tempExcelRange, ConstDefine.REPORT_ASSETCOMPARE_SHEET_0, "A1", ConstDefine.REPORT_RETURN);
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_RETURN, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 RETURN_COLOR);
            tempExcelRange = abBuiltinExceptionWS.Cells[1, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = abBuiltinExceptionWS.Cells[1, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = abBuiltinExceptionWS.Cells[1, 4,1,5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_ASSETBUNDLE_TITLE_2, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            /*
            // Details /////////////////////////////
            if (AssetRecordDataParser.AssetbunleBuiltinExceptionMapping.Count == 0) return;

            var sortDataMapping = AssetRecordDataParser.AssetbunleBuiltinExceptionMapping.OrderByDescending(o => o.Value.Count).ToDictionary(o => o.Key, p => p.Value);
            int row = 2;
            int index = 1;
            foreach (var data in sortDataMapping)
            {
                var bundleName = data.Key;
                var builtinResList = data.Value;

                int addRow = builtinResList.Count - 1;

                tempExcelRange = abBuiltinExceptionWS.Cells[row, 2, row + addRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, index, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);

                tempExcelRange = abBuiltinExceptionWS.Cells[row, 3, row + addRow, 3];
                ExcelHelper.SetExcelRange(tempExcelRange, bundleName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);

                tempExcelRange = abBuiltinExceptionWS.Cells[row, 4, row + addRow, 4];
                ExcelHelper.SetExcelRange(tempExcelRange, builtinResList.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);

                for (int i = 0; i < builtinResList.Count; i++)
                {
                    tempExcelRange = abBuiltinExceptionWS.Cells[row + i, 5];
                    ExcelHelper.SetExcelRange(tempExcelRange, builtinResList[i], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, false,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         CONTENT_DATA_COLOR_1);
                }

                index++;
                row += builtinResList.Count;
            }
            */
        }
    }
}
