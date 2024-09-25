using System.IO;
using System.Linq;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using UnityEditor;

namespace jj.TATools.Editor
{
    internal class SpriteAtlasReportMananger
    {
        #region Fields

        internal static Dictionary<string, SpriteAtlasData> SpriteAtlasMapping = new Dictionary<string, SpriteAtlasData>();
        internal static Dictionary<string, List<string>> SpriteToAtlasMapping = new Dictionary<string, List<string>>();

        internal static long FileDiskSizeTotal = 0;
        internal static long FileMemorySizeTotal = 0;
        internal static long SpriteMemorySizeTotal = 0;

        #endregion

        #region Local Methods

        static bool ParseData(List<SpriteAtlasData> spriteAtlasDataList, string sourceDataFolder)
        {
            if (!Directory.Exists(sourceDataFolder)) return false;

            SpriteAtlasData.UpdateDataFromConfig();

            SpriteAtlasMapping.Clear();
            SpriteToAtlasMapping.Clear();
            List<string> tempList = null;

            FileDiskSizeTotal = 0;
            FileMemorySizeTotal = 0;
            SpriteMemorySizeTotal = 0;
            
            foreach(var saData in spriteAtlasDataList)
            {
                FileDiskSizeTotal += saData.m_FileDiskSize;
                FileMemorySizeTotal += saData.m_TotalMemorySize;
                SpriteMemorySizeTotal += saData.m_TotalSpriteMemorySize;

                SpriteAtlasMapping[saData.m_AssetPath] = saData;

                saData.GetPageImageFiles(sourceDataFolder);

                foreach (var dep in saData.m_Dependencies)
                {
                    if (!SpriteToAtlasMapping.TryGetValue(dep, out tempList))
                    {
                        tempList = new List<string>();
                        SpriteToAtlasMapping[dep] = tempList;
                    }

                    tempList.Add(saData.m_AssetPath);
                }
            }
           
            return true;
        }

        static void GenerateDetailsReport(ExcelPackage package)
        {
            System.Drawing.Color CURRENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(169, 208, 142);
            System.Drawing.Color SPECIAL_DATA_COLOR_0 = System.Drawing.Color.FromArgb(129, 149, 234);
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
            System.Drawing.Color EXCEPTION_DATA_COLOR = System.Drawing.Color.FromArgb(255, 0, 1);

            ExcelWorksheet spriteAtlasWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_SPRITEATLAS_SHEET_0);
            spriteAtlasWS.View.ShowGridLines = false;
            spriteAtlasWS.View.FreezePanes(8, 1);
            spriteAtlasWS.Column(1).Width = 5;
            spriteAtlasWS.Column(2).Width = 15;
            spriteAtlasWS.Column(3).Width = 15;
            spriteAtlasWS.Column(4).Width = 18;
            spriteAtlasWS.Column(5).Width = 18;
            spriteAtlasWS.Column(6).Width = 15;
            spriteAtlasWS.Column(7).Width = 15;
            spriteAtlasWS.Column(8).Width = 15;
            spriteAtlasWS.Column(9).Width = 15;
            spriteAtlasWS.Column(10).Width = 15;
            spriteAtlasWS.Column(11).Width = 15;
            spriteAtlasWS.Column(12).Width = 15;
            spriteAtlasWS.Column(13).Width = 15;
            spriteAtlasWS.Column(14).Width = 15;
            spriteAtlasWS.Column(15).Width = 18;
            spriteAtlasWS.Column(16).Width = 18;
            spriteAtlasWS.Column(17).Width = 18;
            spriteAtlasWS.Column(18).Width = 18;
            spriteAtlasWS.Column(19).Width = 18;

            // Contents
            ExcelRange tempExcelRange = spriteAtlasWS.Cells[1, 2, 1, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITEATLAS_DETAIL_TITLE, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 16, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[2, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITEATLAS_CONTENT_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[2, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITEATLAS_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[2, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITEATLAS_CONTENT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[2, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITEATLAS_CONTENT_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[2, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITEATLAS_CONTENT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 14, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CURRENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[3, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, SpriteAtlasMapping.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[3, 3];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(FileDiskSizeTotal), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[3, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(FileMemorySizeTotal), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[3, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(SpriteMemorySizeTotal), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[3, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ((FileMemorySizeTotal - SpriteMemorySizeTotal) * 100.0f / SpriteMemorySizeTotal).ToString("F2") + " %", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, false, false,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 SPECIAL_DATA_COLOR_0);

            // Title /////////////////////////////
            int row = 6;
            tempExcelRange = spriteAtlasWS.Cells[row, 2,row + 1,2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 3, row + 1, 4];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_DEFAULT_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 5, row + 1, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITEATLAS_CONTENT_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 6, row + 1, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITEATLAS_CONTENT_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 7, row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_9, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row + 1, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_9_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row + 1, 8];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_9_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row + 1, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_9_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 10, row + 1, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 11, row + 1, 11];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_7, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 12, row + 1, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 13, row + 1, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 14, row + 1, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 15, row + 1, 15];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 16,row,20];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_8, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row + 1, 16];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_8_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row + 1, 17];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_8_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row + 1, 18];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_8_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row + 1, 19];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_8_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row + 1, 20];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_DETAIL_SPRITEATLAS_TITLE_8_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Details /////////////////////////
            row+= 2;
            var sortedDataMapping = SpriteAtlasMapping.OrderByDescending(o => o.Value.m_TotalMemoryOverPercentage).ToDictionary(o => o.Key, p => p.Value);
            int index = 0;
            System.Drawing.Color itemColor = CONTENT_DATA_COLOR_1;
            foreach (var data in sortedDataMapping)
            {
                var atlasData = data.Value;

                bool anisoLevelEx = atlasData.AnisoLevelInvalid();
                bool rwEx = atlasData.RWInvalid();
                bool formatEx = atlasData.FormatInvalid();
                bool mipmapEx = atlasData.MipmapInvalid();
                bool filterModeEx = atlasData.FilterModeInvalid();
                bool resolutionEx = atlasData.ResolutionInvalid();
                bool memoryEx = atlasData.MemoryInvalid();

                int addRow = atlasData.m_PageDataList.Count == 0 ? 0 : (atlasData.m_PageDataList.Count - 1);
                tempExcelRange = spriteAtlasWS.Cells[row, 2,row + addRow,2];
                ExcelHelper.SetExcelRange(tempExcelRange, (index ++), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 3,row + addRow,4];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasData.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 5, row + addRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(atlasData.m_FileDiskSize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 6, row + addRow, 6];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(atlasData.m_TotalMemorySize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 7, row + addRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(atlasData.m_TotalSpriteMemorySize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 8, row + addRow, 8];
                ExcelHelper.SetExcelRange(tempExcelRange, (atlasData.m_TotalMemoryOverPercentage * 100).ToString("F2") + " %", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     memoryEx ? EXCEPTION_DATA_COLOR : itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 9, row + addRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasData.m_Dependencies.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 10, row + addRow, 10];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasData.m_RW == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     rwEx ? EXCEPTION_DATA_COLOR : itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 11, row + addRow, 11];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasData.m_MaxSizeAndroid + "|" + atlasData.m_MaxSizeIOS, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     resolutionEx ? EXCEPTION_DATA_COLOR : itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 12, row + addRow, 12];
                ExcelHelper.SetExcelRange(tempExcelRange, (ETextureImporterFormat)atlasData.m_FormatAndroid + "|" + (ETextureImporterFormat)atlasData.m_FormatIOS, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     formatEx ? EXCEPTION_DATA_COLOR : itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 13, row + addRow, 13];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasData.m_GenerateMipmaps == 1 ? "开" : "关", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     mipmapEx ? EXCEPTION_DATA_COLOR : itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 14, row + addRow, 14];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasData.m_AnisoLevel, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     anisoLevelEx ? EXCEPTION_DATA_COLOR : itemColor);
                tempExcelRange = spriteAtlasWS.Cells[row, 15, row + addRow, 15];
                ExcelHelper.SetExcelRange(tempExcelRange, ((EFilterMode)atlasData.m_FilterMode).ToString(), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     filterModeEx ? EXCEPTION_DATA_COLOR : itemColor);
                if (atlasData.m_PageDataList.Count == 0)
                {
                    tempExcelRange = spriteAtlasWS.Cells[row, 16,row,20];
                    ExcelHelper.SetExcelRange(tempExcelRange, "NULL", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, true, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         EXCEPTION_DATA_COLOR);
                }
                else
                {
                    for (int k = 0; k < atlasData.m_PageDataList.Count; k++)
                    {
                        var pData = atlasData.m_PageDataList[k];

                        var transparentEx = pData.TransparentInvalid();

                        tempExcelRange = spriteAtlasWS.Cells[row + k, 16];
                        ExcelHelper.SetExcelRange(tempExcelRange, pData.m_PageName, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = spriteAtlasWS.Cells[row + k, 17];
                        ExcelHelper.SetExcelRange(tempExcelRange, pData.m_Width + " x " + pData.m_Height, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = spriteAtlasWS.Cells[row + k, 18];
                        ExcelHelper.SetExcelRange(tempExcelRange, AssetDetectionUtility.GetFileSize(pData.m_MemorySize), ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             itemColor);
                        tempExcelRange = spriteAtlasWS.Cells[row + k, 19];
                        ExcelHelper.SetExcelRange(tempExcelRange, (pData.m_TransparentPercentage * 100).ToString("F2") + " %", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                             ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                             transparentEx ? EXCEPTION_DATA_COLOR : itemColor);
                 
                        if(string.IsNullOrEmpty(pData.m_PageImageFile))
                        {
                            tempExcelRange = spriteAtlasWS.Cells[row + k, 20];
                            ExcelHelper.SetExcelRange(tempExcelRange, "", ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                                 itemColor);
                        }
                        else ExcelHelper.InsertImage(spriteAtlasWS,row + k,20 , pData.m_PageImageFile, 150, true, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
                    }
                }

                row += atlasData.m_PageDataList.Count == 0 ? 1 : atlasData.m_PageDataList.Count;
            }
        }

        static void GenerateSpriteReferencedExceptionReport(ExcelPackage package)
        {
            System.Drawing.Color CONTENT_DATA_COLOR_0 = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color CONTENT_DATA_COLOR_1 = System.Drawing.Color.FromArgb(221, 235, 247);
    
            ExcelWorksheet spriteAtlasWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_SPRITEATLAS_SHEET_1);
            spriteAtlasWS.View.ShowGridLines = false;
            spriteAtlasWS.View.FreezePanes(2, 1);
            spriteAtlasWS.Column(1).Width = 5;
            spriteAtlasWS.Column(2).Width = 15;
            spriteAtlasWS.Column(3).Width = 15;
            spriteAtlasWS.Column(4).Width = 15;
            spriteAtlasWS.Column(5).Width = 15;
            spriteAtlasWS.Column(6).Width = 15;
            spriteAtlasWS.Column(7).Width = 15;
            spriteAtlasWS.Column(8).Width = 15;
            spriteAtlasWS.Column(9).Width = 15;
            spriteAtlasWS.Column(10).Width = 15;
            spriteAtlasWS.Column(11).Width = 15;
            spriteAtlasWS.Column(12).Width = 15;

            // Title /////////////////////////////
            int row = 1;
            ExcelRange tempExcelRange = spriteAtlasWS.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITE_REF_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 3, row, 6];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITE_REF_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);
            tempExcelRange = spriteAtlasWS.Cells[row, 7, row, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITE_REF_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 CONTENT_DATA_COLOR_0);

            // Details /////////////////////////
            row ++;
            var sortedDataMapping = SpriteToAtlasMapping.OrderByDescending(o => o.Value.Count).ToDictionary(o => o.Key, p => p.Value);
            int index = 0;
            foreach (var data in sortedDataMapping)
            {
                var spritePath = data.Key;
                var atlasList = data.Value;

                if (atlasList.Count <= 1) continue;

                int addRow = atlasList.Count - 1;

                tempExcelRange = spriteAtlasWS.Cells[row, 2, row + addRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, (index++), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = spriteAtlasWS.Cells[row, 3, row + addRow, 6];
                ExcelHelper.SetExcelRange(tempExcelRange, spritePath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
                tempExcelRange = spriteAtlasWS.Cells[row, 7, row + addRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasList.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     CONTENT_DATA_COLOR_1);
             
                for (int i = 0; i < atlasList.Count; i++)
                {
                    tempExcelRange = spriteAtlasWS.Cells[row + i, 8,row + i,12];
                    ExcelHelper.SetExcelRange(tempExcelRange, atlasList[i], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         CONTENT_DATA_COLOR_1);
                  
                }
     
                row += atlasList.Count;
            }

        }

        static void GeneratePrefabDependenciesExceptionReport(ExcelPackage package)
        {
            System.Drawing.Color TitleColor = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color ItemColor_0 = System.Drawing.Color.FromArgb(255, 255, 255);

            ExcelWorksheet spriteAtlasWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_SPRITEATLAS_SHEET_2);
            spriteAtlasWS.View.ShowGridLines = false;
            spriteAtlasWS.View.FreezePanes(2, 1);
            spriteAtlasWS.Column(1).Width = 5;
            spriteAtlasWS.Column(2).Width = 15;
            spriteAtlasWS.Column(3).Width = 20;
            spriteAtlasWS.Column(4).Width = 20;
            spriteAtlasWS.Column(5).Width = 20;
            spriteAtlasWS.Column(6).Width = 15;
            spriteAtlasWS.Column(7).Width = 15;
            spriteAtlasWS.Column(8).Width = 20;
            spriteAtlasWS.Column(9).Width = 20;
            spriteAtlasWS.Column(10).Width = 20;


            // Title /////////////////////////////
            int row = 1;
            ExcelRange tempExcelRange = spriteAtlasWS.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_PREFAB_REF_ATLAS_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);
            tempExcelRange = spriteAtlasWS.Cells[row, 3, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_PREFAB_REF_ATLAS_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);
            tempExcelRange = spriteAtlasWS.Cells[row, 6, row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_PREFAB_REF_ATLAS_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);
            tempExcelRange = spriteAtlasWS.Cells[row, 8, row, 10];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_PREFAB_REF_ATLAS_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);

            // Details /////////////////////////
            row++;
            var sortedDataMapping = SpirteAtlasTool.m_PrefabToSpriteAtlasMapping.OrderByDescending(o => o.Value.Count).ToDictionary(o => o.Key, p => p.Value);
            int index = 0;
            foreach (var data in sortedDataMapping)
            {
                var prefabPath = data.Key;
                var atlasList = data.Value;

                int addRow = atlasList.Count - 1;

                tempExcelRange = spriteAtlasWS.Cells[row, 2, row + addRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, (index++), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     ItemColor_0);
                tempExcelRange = spriteAtlasWS.Cells[row, 3, row + addRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, prefabPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     ItemColor_0);
                tempExcelRange = spriteAtlasWS.Cells[row, 6, row + addRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, atlasList.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     ItemColor_0);

                for (int i = 0; i < atlasList.Count; i++)
                {
                    tempExcelRange = spriteAtlasWS.Cells[row + i, 8, row + i, 10];
                    ExcelHelper.SetExcelRange(tempExcelRange, atlasList[i], ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         ItemColor_0);

                }

                row += atlasList.Count;
            }
        }


        static void GenerateAtlasDependenciesFormatExceptionReport(ExcelPackage package)
        {
            System.Drawing.Color TitleColor = System.Drawing.Color.FromArgb(155, 194, 230);
            System.Drawing.Color ItemColor_0 = System.Drawing.Color.FromArgb(255, 255, 255);

            ExcelWorksheet spriteAtlasWS = package.Workbook.Worksheets.Add(ConstDefine.REPORT_SPRITEATLAS_SHEET_3);
            spriteAtlasWS.View.ShowGridLines = false;
            spriteAtlasWS.View.FreezePanes(2, 1);
            spriteAtlasWS.Column(1).Width = 5;
            spriteAtlasWS.Column(2).Width = 15;
            spriteAtlasWS.Column(3).Width = 20;
            spriteAtlasWS.Column(4).Width = 20;
            spriteAtlasWS.Column(5).Width = 20;
            spriteAtlasWS.Column(6).Width = 20;
            spriteAtlasWS.Column(7).Width = 20;
            spriteAtlasWS.Column(8).Width = 15;
            spriteAtlasWS.Column(9).Width = 15;
            spriteAtlasWS.Column(10).Width = 20;
            spriteAtlasWS.Column(11).Width = 20;
            spriteAtlasWS.Column(12).Width = 20;
            spriteAtlasWS.Column(13).Width = 24;
            spriteAtlasWS.Column(14).Width = 24;


            // Title /////////////////////////////
            int row = 1;
            ExcelRange tempExcelRange = spriteAtlasWS.Cells[row, 2];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITE_REF_INVALID_TITLE_0, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);
            tempExcelRange = spriteAtlasWS.Cells[row, 3, row, 5];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITE_REF_INVALID_TITLE_1, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);
            tempExcelRange = spriteAtlasWS.Cells[row, 6, row, 7];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITE_REF_INVALID_TITLE_2, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);
            tempExcelRange = spriteAtlasWS.Cells[row, 8, row, 9];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITE_REF_INVALID_TITLE_3, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);
            tempExcelRange = spriteAtlasWS.Cells[row, 10, row, 12];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITE_REF_INVALID_TITLE_4, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);
            tempExcelRange = spriteAtlasWS.Cells[row, 13];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITE_REF_INVALID_TITLE_5, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);
            tempExcelRange = spriteAtlasWS.Cells[row, 14];
            ExcelHelper.SetExcelRange(tempExcelRange, ConstDefine.REPORT_SPRITE_REF_INVALID_TITLE_6, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 12, true, true,
                 ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                 TitleColor);

            // Details /////////////////////////
            row++;
            var sortedDataMapping = SpirteAtlasTool.m_SpriteAtlasRefInvalidFormatSpriteList.OrderByDescending(o => o.m_InvalidFormatList.Count).ToList();
            int index = 0;
            foreach (var data in sortedDataMapping)
            {
                var invalidFormatList = data.m_InvalidFormatList;
                int addRow = invalidFormatList.Count - 1;

                tempExcelRange = spriteAtlasWS.Cells[row, 2, row + addRow, 2];
                ExcelHelper.SetExcelRange(tempExcelRange, (index++), ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     ItemColor_0);
                tempExcelRange = spriteAtlasWS.Cells[row, 3, row + addRow, 5];
                ExcelHelper.SetExcelRange(tempExcelRange, data.m_AssetPath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     ItemColor_0);
                tempExcelRange = spriteAtlasWS.Cells[row, 6, row + addRow, 7];
                ExcelHelper.SetExcelRange(tempExcelRange, (TextureImporterFormat)data.m_FormatAndroid + "|" + (TextureImporterFormat)data.m_FormatIOS, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     ItemColor_0);

                tempExcelRange = spriteAtlasWS.Cells[row, 8, row + addRow, 9];
                ExcelHelper.SetExcelRange(tempExcelRange, invalidFormatList.Count, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                     ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                     ItemColor_0);

                for (int i = 0; i < invalidFormatList.Count; i++)
                {
                    var spritePath = invalidFormatList[i];

                    tempExcelRange = spriteAtlasWS.Cells[row + i, 10, row + i, 12];
                    ExcelHelper.SetExcelRange(tempExcelRange, spritePath, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, 11, false, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         ItemColor_0);

                    var invalidFormatStr = "";
                    if (data.m_InvalidAndroidFormatDependencies.ContainsKey(spritePath))
                        invalidFormatStr = data.m_InvalidAndroidFormatDependencies[spritePath].ToString();

                    tempExcelRange = spriteAtlasWS.Cells[row + i, 13];
                    ExcelHelper.SetExcelRange(tempExcelRange, invalidFormatStr, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         ItemColor_0);

                    invalidFormatStr = "";
                    if (data.m_InvalidIosFormatDependencies.ContainsKey(spritePath))
                        invalidFormatStr = data.m_InvalidIosFormatDependencies[spritePath].ToString();
                    tempExcelRange = spriteAtlasWS.Cells[row + i, 14];
                    ExcelHelper.SetExcelRange(tempExcelRange, invalidFormatStr, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, 11, false, true,
                         ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin,
                         ItemColor_0);

                }

                row += invalidFormatList.Count;
            }
        }

        #endregion

        #region Internal Methods

        internal static string GenerateReport(List<SpriteAtlasData> spriteAtlasDataList,string sourceDataFolder,string moduleFolder,string tick)
        {
            ExcelHelper.SetExcelPackageLicenseContextProperty(LicenseContext.NonCommercial);

            var outputFolder = AppConfigHelper.SPRITEATLAS_SOURCE_DATA_FOLDER + "\\" + tick;
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

            var fileName = string.Format("《{0}》{1}.{2}", new DirectoryInfo(moduleFolder).Name, ConstDefine.REPORT_SPRITEATLAS_FILE_NAME, ConstDefine.REPORT_FILE_EXTENSION);
            string spriteAtlasReportPath = AssetDetectionUtility.PathCombine(outputFolder, fileName);
            if (string.IsNullOrEmpty(sourceDataFolder)) return null;

            if (ParseData(spriteAtlasDataList, sourceDataFolder))
            {
                FileInfo sourFile = null;
                try
                {
                    sourFile = new FileInfo(spriteAtlasReportPath);
                    if (sourFile.Exists)
                    {
                        sourFile.Delete();
                        sourFile = new FileInfo(spriteAtlasReportPath);
                    }
                }
                catch (System.Exception e)
                {
                    AssetDetectionUtility.MessageBox("[" + Path.GetFileName(spriteAtlasReportPath) + "]文件已经被打开，请关闭后重新生成报告！", "生成资源检测报告异常");
                    return null;
                }

                using (ExcelPackage package = new ExcelPackage(sourFile))
                {
                    // 详情报告
                    GenerateDetailsReport(package);
                    // Sprite 被多分图集引用报告
                    GenerateSpriteReferencedExceptionReport(package);
                    // Prefab 引用图集报告
                    GeneratePrefabDependenciesExceptionReport(package);
                    // 图集与引用Sprite格式异常报告
                    GenerateAtlasDependenciesFormatExceptionReport(package);

                    package.Save();
                }
            }

            return spriteAtlasReportPath;
        }

        #endregion
    }
}
