namespace jj.TATools.Editor
{
    using System;
    using System.Drawing;
    using System.Collections.Generic;

    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using OfficeOpenXml.Drawing;
    using OfficeOpenXml.Drawing.Chart;


    internal class ExcelHelper
    {
        const float DEFAULT_DPI = 96;

        internal static readonly System.Drawing.Color REFERENCED_IN_CODE_COLOR = System.Drawing.Color.FromArgb(255, 192, 0);

        #region internal Methods

        /// <summary>
        ///  EPPlus 从 5.0.0 版本开始，它要求在使用之前设置该属性，以便进行许可证验证.
        /// </summary>
        internal static void SetExcelPackageLicenseContextProperty(LicenseContext licenseContext)
        {
            ExcelPackage.LicenseContext = licenseContext;
        }

        /// <summary>
        /// MeasureString
        /// </summary>
        /// <param name="s"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        internal static float MeasureString(string s, Font font)
        {
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                return g.MeasureString(s, font, int.MaxValue, StringFormat.GenericTypographic).Width;
            }
        }

        /// <summary>
        /// 获取单元格的宽度(像素)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        internal static int GetWidthInPixels(ExcelRange cell)
        {
            double columnWidth = cell.Worksheet.Column(cell.Start.Column).Width;
            Font font = new Font(cell.Style.Font.Name, cell.Style.Font.Size, FontStyle.Regular);
            double pxBaseline = Math.Round(MeasureString("1234567890", font) / 10);
            return (int)(columnWidth * pxBaseline);
        }

        /// <summary>
        /// 获取单元格的高度(像素)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        internal static int GetHeightInPixels(ExcelRange cell)
        {
            double rowHeight = cell.Worksheet.Row(cell.Start.Row).Height;
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiY = graphics.DpiY;
                return (int)(rowHeight * (1.0 / DEFAULT_DPI) * dpiY);
            }
        }

        /// <summary>
        /// 获取自适应调整后的图片尺寸
        /// </summary>
        /// <param name="image"></param>
        /// <param name="cellColumnWidthInPix"></param>
        /// <param name="cellRowHeightInPix"></param>
        /// <returns>item1:调整后的图片宽度; item2:调整后的图片高度</returns>
        internal static Tuple<int, int> GetAdjustImageSize(Image image, int cellColumnWidthInPix, int cellRowHeightInPix)
        {
            int imageWidthInPix = image.Width;
            int imageHeightInPix = image.Height;
            //调整图片尺寸,适应单元格
            int adjustImageWidthInPix;
            int adjustImageHeightInPix;
            if (imageHeightInPix * cellColumnWidthInPix > imageWidthInPix * cellRowHeightInPix)
            {
                //图片高度固定,宽度自适应
                adjustImageHeightInPix = cellRowHeightInPix;
                double ratio = (1.0) * adjustImageHeightInPix / imageHeightInPix;
                adjustImageWidthInPix = (int)(imageWidthInPix * ratio);
            }
            else
            {
                //图片宽度固定,高度自适应
                adjustImageWidthInPix = cellColumnWidthInPix;
                double ratio = (1.0) * adjustImageWidthInPix / imageWidthInPix;
                adjustImageHeightInPix = (int)(imageHeightInPix * ratio);
            }
            return new Tuple<int, int>(adjustImageWidthInPix, adjustImageHeightInPix);
        }

        internal static void SetExcelRange(ExcelRange excelRange, object value,
          ExcelHorizontalAlignment hAlignment, ExcelVerticalAlignment vAlignment,
          int fontSize, bool bold, bool merge,
          ExcelBorderStyle borderLeft, ExcelBorderStyle borderRight, ExcelBorderStyle borderTop, ExcelBorderStyle borderBottom,
          System.Drawing.Color color)
        {
            excelRange.Merge = merge;
            excelRange.Value = value;
            excelRange.Style.HorizontalAlignment = hAlignment;
            excelRange.Style.VerticalAlignment = vAlignment;
            excelRange.Style.Font.Bold = bold;
            excelRange.Style.Font.Size = fontSize;
            excelRange.Style.Font.Name = "宋体";
            excelRange.Style.Border.Left.Style = borderLeft;
            excelRange.Style.Border.Right.Style = borderRight;
            excelRange.Style.Border.Top.Style = borderTop;
            excelRange.Style.Border.Bottom.Style = borderBottom;
            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelRange.Style.Fill.BackgroundColor.SetColor(color);
        }

        /// <summary>
        /// 像单个单元格插入图片
        /// </summary>
        /// <param name="excelRange"></param>
        /// <param name="imagePath"></param>
        /// <param name="autofit"></param>
        internal static void InsertImage(ExcelWorksheet worksheet, int rowNum, int colNum, string imagePath, int thumbNailIamgeHeight, bool autofit, ExcelBorderStyle borderLeft, ExcelBorderStyle borderRight, ExcelBorderStyle borderTop, ExcelBorderStyle borderBottom)
        {
            var excelRange = worksheet.Cells[rowNum, colNum];
            excelRange.Style.Border.Left.Style = borderLeft;
            excelRange.Style.Border.Right.Style = borderRight;
            excelRange.Style.Border.Top.Style = borderTop;
            excelRange.Style.Border.Bottom.Style = borderBottom;

            var pictureName = System.IO.Path.GetFileNameWithoutExtension(imagePath);
            var mainIamge = Image.FromFile(imagePath);
            var oriWidth = mainIamge.Width;
            var oriHeight = mainIamge.Height;
            Image thumbnailImage = null;
            if (thumbNailIamgeHeight >= oriHeight) thumbnailImage = mainIamge;
            else
            {
                float scale = thumbNailIamgeHeight * 1.0f / oriHeight;
                var thumbNailIamgeWidth = (int)(oriWidth * scale);
                thumbnailImage = mainIamge.GetThumbnailImage(thumbNailIamgeWidth, thumbNailIamgeHeight, () => false, IntPtr.Zero);
            }
            var picture = excelRange.Worksheet.Drawings.AddPicture(pictureName, thumbnailImage);
            int cellColumnWidthInPix = GetWidthInPixels(excelRange);
            int cellRowHeightInPix = GetHeightInPixels(excelRange);
            int adjustImageWidthInPix = cellColumnWidthInPix;
            int adjustImageHeightInPix = cellRowHeightInPix;
            if (autofit)
            {
                //图片尺寸适应单元格
                var adjustImageSize = GetAdjustImageSize(thumbnailImage, cellColumnWidthInPix, cellRowHeightInPix);
                adjustImageWidthInPix = adjustImageSize.Item1;
                adjustImageHeightInPix = adjustImageSize.Item2;
            }
            //设置为居中显示
            int columnOffsetPixels = (int)((cellColumnWidthInPix - adjustImageWidthInPix) / 2.0);
            int rowOffsetPixels = (int)((cellRowHeightInPix - adjustImageHeightInPix) / 2.0);
            picture.SetSize(adjustImageWidthInPix, adjustImageHeightInPix);
            picture.SetPosition(rowNum - 1, rowOffsetPixels, colNum - 1, columnOffsetPixels);

            mainIamge.Dispose();
            thumbnailImage.Dispose();
        }

        internal static ExcelChart AddChart(ExcelWorksheet workSheet, eChartType chartType,
            string chartName, string title, int titleFontSize, Color titleFontColor, bool titleFontBold,
            int startLeft, int startTop, int width, int height,
            eChartStyle mainStyle,
            eLineStyle legendLlineStyle, Color legendFillColor,
            bool useSecondaryAxis)
        {
            ExcelChart chart = workSheet.Drawings.AddChart(chartName, chartType);
            chart.SetPosition(startTop, startLeft);
            chart.SetSize(width, height);
            chart.Title.Text = title;
            chart.Title.Font.Color = titleFontColor;
            chart.Title.Font.Size = titleFontSize;
            chart.Title.Font.Bold = titleFontBold;
            chart.Style = mainStyle;
            chart.Legend.Border.LineStyle = legendLlineStyle;
            chart.Legend.Border.Fill.Color = legendFillColor;

            var newChartType = chart.PlotArea.ChartTypes.Add(chartType);
            newChartType.UseSecondaryAxis = useSecondaryAxis;

            return newChartType;
        }

        internal static void SetExcelChartData(ExcelChart excelChart, ExcelRangeBase serie, ExcelRangeBase xserie, ExcelAddressBase headerAddress)
        {
            var newSerie = (excelChart.Series.Add(serie, xserie) as ExcelChartSerie);
            newSerie.HeaderAddress = headerAddress;
        }

        internal static void SetExcelRow(ExcelWorksheet worksheet, int row, int outlineLevel, bool collapsed = true)
        {
            ExcelRow tempExcelRow = worksheet.Row(row);
            tempExcelRow.OutlineLevel = outlineLevel;
            tempExcelRow.Collapsed = collapsed;
        }

        internal static void SetExcelRow(ExcelWorksheet worksheet, int rowFrom, int rowTo, int outlineLevel, bool collapsed = true)
        {
            for (int i = rowFrom; i <= rowTo; i++)
            {
                ExcelRow tempExcelRow = worksheet.Row(i);
                tempExcelRow.OutlineLevel = outlineLevel;
                tempExcelRow.Collapsed = collapsed;
            }
        }

        internal static void SetHyperLink(ExcelRange rng, string sheetName, string cellCoords, string displayName)
        {
            bool bold = rng.Style.Font.Bold;
            float fontSize = rng.Style.Font.Size;

            rng.Hyperlink = new ExcelHyperLink((char)39 + sheetName + (char)39 + "!" + cellCoords, displayName);
            rng.Style.Font.UnderLine = true;
            rng.Style.Font.Bold = bold;
            rng.Style.Font.Size = fontSize;
        }

        internal static void SetExcelPieChart(ExcelWorksheet ownerSheet, string chartName,
           ExcelRange title, System.Drawing.Color titleFontColor, int titleFontSize, bool titleFontBold,
           ExcelRange serieX, ExcelRange serieY,
           int pixelTop, int pixelLeft, int pixelWidth, int pixelHeight,
           int dataLabelFontSize, bool dataLabelFontBold,
           bool showLegend, eLegendPosition legendPosiiton,
           bool showValue, bool showCategory, bool showSeriesName, bool showPercent, bool showLeaderLines, bool showBubbleSize, bool showLegendKey)
        {
            ExcelPieChart chart = ownerSheet.Drawings.AddChart(chartName, eChartType.Pie) as ExcelPieChart;
            chart.SetPosition(pixelTop, pixelLeft);
            chart.SetSize(pixelWidth, pixelHeight);
            chart.Title.Text = (string)title.Value;
            chart.Title.Font.Color = titleFontColor;
            chart.Title.Font.Size = titleFontSize;
            chart.Title.Font.Bold = titleFontBold;
            if (!showLegend)
                chart.Legend.Remove();
            else
                chart.Legend.Position = legendPosiiton;
            chart.DataLabel.Font.Size = dataLabelFontSize;
            chart.DataLabel.Font.Bold = dataLabelFontBold;
            chart.DataLabel.ShowValue = showValue;
            chart.DataLabel.ShowCategory = showCategory;
            chart.DataLabel.ShowSeriesName = showSeriesName;
            chart.DataLabel.ShowPercent = showPercent;
            chart.DataLabel.ShowLeaderLines = showLeaderLines;
            chart.DataLabel.ShowBubbleSize = showBubbleSize;
            chart.DataLabel.ShowLegendKey = showLegendKey;

            var serie = (chart.Series.Add(serieX, serieY) as ExcelPieChartSerie);
            serie.HeaderAddress = title;
        }

        internal static void SetExcelPieChart(ExcelWorksheet ownerSheet, string chartName,
           string title, System.Drawing.Color titleFontColor, int titleFontSize, bool titleFontBold,
           ExcelRange serieX, ExcelRange serieY,
           int pixelTop, int pixelLeft, int pixelWidth, int pixelHeight,
           int dataLabelFontSize, bool dataLabelFontBold,
           bool showLegend, eLegendPosition legendPosiiton,
           bool showValue, bool showCategory, bool showSeriesName, bool showPercent, bool showLeaderLines, bool showBubbleSize, bool showLegendKey)
        {
            ExcelPieChart chart = ownerSheet.Drawings.AddChart(chartName, eChartType.Pie) as ExcelPieChart;
            chart.SetPosition(pixelTop, pixelLeft);
            chart.SetSize(pixelWidth, pixelHeight);
            chart.Title.Text = title;
            chart.Title.Font.Color = titleFontColor;
            chart.Title.Font.Size = titleFontSize;
            chart.Title.Font.Bold = titleFontBold;
            if (!showLegend)
                chart.Legend.Remove();
            else
                chart.Legend.Position = legendPosiiton;
            chart.DataLabel.Font.Size = dataLabelFontSize;
            chart.DataLabel.Font.Bold = dataLabelFontBold;
            chart.DataLabel.ShowValue = showValue;
            chart.DataLabel.ShowCategory = showCategory;
            chart.DataLabel.ShowSeriesName = showSeriesName;
            chart.DataLabel.ShowPercent = showPercent;
            chart.DataLabel.ShowLeaderLines = showLeaderLines;
            chart.DataLabel.ShowBubbleSize = showBubbleSize;
            chart.DataLabel.ShowLegendKey = showLegendKey;
            chart.DataLabel.Separator = ":";


            var serie = (chart.Series.Add(serieX, serieY) as ExcelPieChartSerie);
            //serie.HeaderAddress = title;
        }

        internal static void SetExcelRangeDataListValidation(ExcelWorksheet worksheet, string address, List<string> formulaList)
        {
            if (formulaList == null || formulaList.Count == 0) return;

            var dataValidations = worksheet.DataValidations;
            var validation = dataValidations.AddListValidation(address);
            validation.ShowInputMessage = true;
            foreach (var formula in formulaList)
                validation.Formula.Values.Add(formula);
        }

        #endregion
    }
}

