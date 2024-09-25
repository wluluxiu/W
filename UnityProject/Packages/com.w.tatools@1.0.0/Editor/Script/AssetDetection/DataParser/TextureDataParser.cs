using System.IO;
using System.Collections.Generic;

namespace jj.TATools.Editor
{
    internal class TextureData
    {
        internal string m_TextureType;
        internal string m_TexturePath;
        internal string m_TextureMD5;
        internal long m_DiskSize;
        internal long m_DiskOldSize;
        internal int m_Width;
        internal int m_OldWidth;
        internal int m_Height;
        internal int m_OldHeight;
        internal long m_GpuSize;
        internal long m_GpuOldSize;
        internal float m_TransparentPercentage;
        internal float m_TransparentOldPercentage;

        internal string GetResolutionStr()
        {
            if (m_Width <= 0 || m_Height <= 0) return "";

            return m_Width + " x " + m_Height;
        }

        internal string GetOldResolutionStr()
        {
            if (m_OldWidth <= 0 || m_OldHeight <= 0) return "";

            return m_OldWidth + " x " + m_OldHeight;
        }

        internal string GetTransparentPertanageStr()
        {
            return (m_TransparentPercentage * 100).ToString("F2") + " %";
        }

        internal string GetTransparentOldPertanageStr()
        {
            return (m_TransparentOldPercentage * 100).ToString("F2") + " %";
        }
    }

    internal class TextureDataParser
    {
        #region Fields

        internal static Dictionary<string, string> TextureTypeNameMapping; 

        internal static Dictionary<string, Dictionary<string, TextureData>> LastVerisonFilesMapping { get; set; }
        internal static Dictionary<string, Dictionary<string, TextureData>> CurrentVerisonFilesMapping { get; set; }

        internal static Dictionary<string, Dictionary<string, TextureData>> CurrentVersionDeletedMapping { get; set; }
        internal static Dictionary<string, Dictionary<string, TextureData>> CurrentVersionAddedMapping { get; set; }
        internal static Dictionary<string, Dictionary<string, TextureData>> CurrentVersionModifiedMapping { get; set; }

        //internal static Dictionary<string, Dictionary<string, Dictionary<string, TextureData>>> CurrentRepeatFilesExceptionMapping { get; set; }

        internal static List<TextureData> CurrentVersionTransparentExceptiongMapping { get; set; }
        internal static List<string> TP_SHHOW_LIST_1 = new List<string>()
        {
             "不透明",
             "[0 % - 10 %]",
             "[10 % - 20 %]",
             "[20 % - 30 %]",
             "[40 % - 50 %]",
             "[50 % - 60 %]",
             "[60 % - 70 %]",
             "[70 % - 80 %]",
             "[80 % - 90 %]",
             "[90 %- 99.99 %]",
             "完全透明",
        };
        internal static Dictionary<string, int> CurrentVersionTransparentCountMapping1 { get; set; }
        internal static List<string> TP_SHHOW_LIST_2 = new List<string>()
        {
             "[大于 20 %]",
             "[大于 30 %]",
             "[大于 50 %]",
             "[大于 60 %]",
             "[大于 70 %]",
             "[大于 80 %]",
             "[大于 90 %]",
        };
        internal static Dictionary<string, int> CurrentVersionTransparentCountMapping2 { get; set; }

        internal static int LastTotalFileAmount { get; set; }
        internal static int CurrentTotalFileAmount { get; set; }

        internal static Dictionary<string, long> LastFilesSizeMapping { get; set; }
        internal static Dictionary<string,long> LastFilesGpuSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentFilesSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentFilesGpuSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentAddedFilesSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentAddedFilesGpuSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentBeforeModifiedFilesSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentBeforeModifiedFilesGpuSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentAfterModifiedFilesSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentAfterModifiedFilesGpuSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentDeletedFilesSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentDeletedFilesGpuSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentRepeatFilesSizeMapping { get; set; }
        internal static Dictionary<string, long> CurrentRepeatFilesGpuSizeMapping { get; set; }

        static string[] FindFileFilters { get; set; }

        #endregion

        #region Local Methods

        static void ClearCachesMappingFile(string textureBaseFolder)
        {
            var md5FilePath = AssetDetectionUtility.GetTextureMD5FilePath(AppConfigHelper.TEXTURE_COMPARE_REPORT_FOLDER, textureBaseFolder);
            if (File.Exists(md5FilePath))
            {
                File.Delete(md5FilePath);
            }
        }

        static Dictionary<string, Dictionary<string, TextureData>> GetCacheMapping(string textureBaseFolder)
        {
            Dictionary<string, Dictionary<string, TextureData>> cachesMapping = null;
            Dictionary<string, TextureData> tempDic = null;
            var md5FilePath = AssetDetectionUtility.GetTextureMD5FilePath(AppConfigHelper.TEXTURE_COMPARE_REPORT_FOLDER, textureBaseFolder);
            if (File.Exists(md5FilePath))
            {
                cachesMapping = new Dictionary<string, Dictionary<string, TextureData>>();
                
                StreamReader sr = new StreamReader(md5FilePath, System.Text.Encoding.UTF8);
                var lineStr = sr.ReadLine();
                while (lineStr != null)
                {
                    var arr = lineStr.Split('|');
                    if (arr.Length != 13) continue;

                    // TextureType|TexturePath|TextureMD5|DiskSize|DiskOldSize|Width|WidthOld|Height|HeightOld|GpuSize|GpuOldSize|TransparentPercentage|TransparentOldPercentage
                    TextureData tData = new TextureData();
                    tData.m_TextureType = arr[0].ToLower();
                    tData.m_TexturePath = arr[1];
                    tData.m_TextureMD5 = arr[2];
                    tData.m_DiskSize = long.Parse(arr[3]);
                    tData.m_DiskOldSize = long.Parse(arr[4]);
                    tData.m_Width = int.Parse(arr[5]);
                    tData.m_OldWidth = int.Parse(arr[6]);
                    tData.m_Height = int.Parse(arr[7]);
                    tData.m_OldHeight = int.Parse(arr[8]);
                    tData.m_GpuSize =  long.Parse(arr[9]);
                    tData.m_GpuOldSize = long.Parse(arr[10]);
                    tData.m_TransparentPercentage = float.Parse(arr[11]);
                    tData.m_TransparentOldPercentage = float.Parse(arr[12]);

                    if (!cachesMapping.TryGetValue(tData.m_TextureType, out tempDic))
                    {
                        tempDic = new Dictionary<string, TextureData>();
                        cachesMapping[tData.m_TextureType] = tempDic;
                    }

                    tempDic[tData.m_TexturePath] = tData;

                    lineStr = sr.ReadLine();
                }

                sr.Close();
                sr.Dispose();
            }

            return cachesMapping;
        }

        /// <summary>
        ///  TextureType|TexturePath|TextureMD5|DiskSize|DiskOldSize|Width|WidthOld|Height|HeightOld|GpuSize|GpuOldSize|TransparentPercentage|TransparentOldPercentage
        /// </summary>
        /// <param name="textureBaseFolder"></param>
        /// <param name="cachesMapping"></param>
        static void UpdateTextureCachesMapping(string textureBaseFolder,Dictionary<string, Dictionary<string, TextureData>> cachesMapping)
        {
            string cachesStr = "";

            foreach (var data in cachesMapping)
            {
                var assetType = data.Key;
                foreach (var childData in data.Value.Values)
                {
                    cachesStr += assetType.ToLower() + "|" + 
                                 childData.m_TexturePath + "|" +
                                 childData.m_TextureMD5 + "|" +
                                 childData.m_DiskSize + "|" +
                                 childData.m_DiskOldSize + "|" +
                                 childData.m_Width + "|" +
                                 childData.m_OldWidth + "|" +
                                 childData.m_Height + "|" +
                                 childData.m_OldHeight + "|" +
                                 childData.m_GpuSize + "|" +
                                 childData.m_GpuOldSize + "|" +
                                 childData.m_TransparentPercentage + "|" +
                                 childData.m_TransparentOldPercentage + "\n";
                }
            }
            
            var md5FilePath = AssetDetectionUtility.GetTextureMD5FilePath(AppConfigHelper.TEXTURE_COMPARE_REPORT_FOLDER, textureBaseFolder);
            StreamWriter sw = new StreamWriter(md5FilePath, false, System.Text.Encoding.UTF8);
            sw.Write(cachesStr);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        static List<string> GetAllFiles(string folder)
        {
            List<string> totalFiles = new List<string>();
            foreach (var filter in FindFileFilters)
            {
                string[] files = Directory.GetFiles(folder, "*." + filter, SearchOption.AllDirectories);
                totalFiles.AddRange(files);
            }

            return totalFiles;
        }

        static Dictionary<string, Dictionary<string, TextureData>> GetTextureDataMapping(string textureBaseFolder, out int fileAmount,ref Dictionary<string, string> textureTypeMapping,bool onlyCache)
        {
            fileAmount = 0;

            if (!Directory.Exists(textureBaseFolder))
            {
                ClearCachesMappingFile(textureBaseFolder);
                return null;
            }

            var textureCachesMapping = GetCacheMapping(textureBaseFolder);
            if (onlyCache)
            {
                if (textureCachesMapping == null) return null;

                foreach (var data in textureCachesMapping)
                {
                    var textureType = data.Key;
                    fileAmount+= data.Value.Count;
                    if (!textureTypeMapping.ContainsKey(textureType))
                    {
                        textureTypeMapping[textureType] = textureType.StartsWith(".") ? textureType.Replace(".", "") : textureType;
                    }
                }
               
                return textureCachesMapping;
            }

            Dictionary<string, TextureData> tempCachesDic = null;

            var findBaseFolder = textureBaseFolder;
            if (!findBaseFolder.EndsWith("\\")) findBaseFolder += "\\";

            var files = GetAllFiles(textureBaseFolder);
            if (files == null || files.Count == 0) return null;

            Dictionary<string, Dictionary<string, TextureData>> mapping = new Dictionary<string, Dictionary<string, TextureData>>();
            Dictionary<string, TextureData> tempDic = null;
            fileAmount = files.Count;
            bool needUpadeCacheFile = false;
            foreach (var file in files)
            {
                var md5 = AssetDetectionUtility.GetMD5HashFromFile(file);
                var assetType = Path.GetExtension(file).ToLower();
                var relativeFilePath = file.Replace(findBaseFolder, "");

                TextureData tData = null;
                if (textureCachesMapping != null && textureCachesMapping.TryGetValue(assetType,out tempCachesDic))
                {
                    tempCachesDic.TryGetValue(relativeFilePath, out tData);
                }

                bool needCalculateInfo = false;
                if (tData == null)
                {
                    needUpadeCacheFile = true;
                    needCalculateInfo = true;
                   
                    tData = new TextureData();
                    tData.m_TexturePath = relativeFilePath;
                    tData.m_TextureType = assetType;
                    tData.m_TextureMD5 = md5;
                }
                else
                {
                    if (tData.m_TextureMD5 != md5)
                    {
                        needUpadeCacheFile = true;
                        needCalculateInfo = true;

                        tData.m_TextureMD5 = md5;
                    }
                }

                if (needCalculateInfo)
                {
                    FileInfo fInfo = new FileInfo(file);
                    int width = 0;
                    int height = 0;
                    long gpuSize = 0;
                    float transparentPertanage = 0.0f;
                    AssetDetectionUtility.GetImageRGBA8888Info(file, out width, out height, out gpuSize, out transparentPertanage);

                    tData.m_DiskSize = fInfo.Length;
                    tData.m_Width = width;
                    tData.m_Height = height;
                    tData.m_GpuSize = gpuSize;
                    tData.m_TransparentPercentage = transparentPertanage;
                }

                if (!mapping.TryGetValue(tData.m_TextureType, out tempDic))
                {
                    tempDic = new Dictionary<string, TextureData>();
                    mapping[tData.m_TextureType] = tempDic;
                }
               
                tempDic[relativeFilePath] = tData;

                if (!textureTypeMapping.ContainsKey(tData.m_TextureType))
                {
                    textureTypeMapping[tData.m_TextureType] = tData.m_TextureType.StartsWith(".") ? tData.m_TextureType.Replace(".", "") : tData.m_TextureType;
                }
            }

            if(needUpadeCacheFile) UpdateTextureCachesMapping(textureBaseFolder,mapping);

            return mapping;
        }

        static Dictionary<string, Dictionary<string, Dictionary<string, TextureData>>> getRepeatFilesExceptionData(Dictionary<string, Dictionary<string, TextureData>> currentVerisonFilesMapping)
        {
            CurrentRepeatFilesSizeMapping = new Dictionary<string, long>();
            CurrentRepeatFilesGpuSizeMapping = new Dictionary<string, long>();

            if (currentVerisonFilesMapping == null || currentVerisonFilesMapping.Count == 0) return null;

            Dictionary<string, Dictionary<string, Dictionary<string, TextureData>>> mapping = new Dictionary<string, Dictionary<string, Dictionary<string, TextureData>>>();
            Dictionary<string, Dictionary<string, TextureData>> tempDic = null;
            Dictionary<string, TextureData> tempChildDic = null;
            foreach (var data in currentVerisonFilesMapping)
            {
                string assetType = data.Key;
                if (!mapping.TryGetValue(assetType, out tempDic))
                {
                    tempDic = new Dictionary<string, Dictionary<string, TextureData>>();
                    mapping[assetType] = tempDic;
                }

                List<string> checkedList = new List<string>();

                long currentRepeatFileSize = 0;
                if (!CurrentRepeatFilesSizeMapping.TryGetValue(assetType, out currentRepeatFileSize)) currentRepeatFileSize = 0;
                long currentRepeatFileGpuSize = 0;
                if (CurrentRepeatFilesGpuSizeMapping.TryGetValue(assetType, out currentRepeatFileGpuSize)) currentRepeatFileGpuSize = 0;

                Dictionary<string, TextureData> dic = data.Value;
                foreach (var childData in dic.Values)
                {
                    string path = childData.m_TexturePath;
                    string md5 = childData.m_TextureMD5;
                    if (string.IsNullOrEmpty(md5) || checkedList.Contains(path)) continue;

                    foreach (var childData_1 in dic.Values)
                    {
                        string path_1 = childData_1.m_TexturePath;
                        string md5_1 = childData_1.m_TextureMD5;
                        if (path_1 == path) continue;

                        if (md5 == md5_1)
                        {
                            if (!tempDic.TryGetValue(path, out tempChildDic))
                            {
                                tempChildDic = new Dictionary<string, TextureData>();
                                tempDic[path] = tempChildDic;
                            }

                            if (!tempChildDic.ContainsKey(path))
                            {
                                tempChildDic.Add(path, childData);
                                currentRepeatFileSize += childData.m_DiskSize;
                                currentRepeatFileGpuSize += childData.m_GpuSize;

                                checkedList.Add(path);
                            }

                            if (!tempChildDic.ContainsKey(path_1))
                            {
                                tempChildDic.Add(path_1, childData_1);
                                currentRepeatFileSize += childData_1.m_DiskSize;
                                currentRepeatFileGpuSize += childData_1.m_GpuSize;

                                checkedList.Add(path_1);
                            }
                        }
                    }
                }

                CurrentRepeatFilesSizeMapping[assetType] = currentRepeatFileSize;
                CurrentRepeatFilesGpuSizeMapping[assetType] = currentRepeatFileGpuSize;
            }

            return mapping;
        }

        static List<TextureData> getTransparentExceptionData(Dictionary<string, Dictionary<string, TextureData>> currentVerisonFilesMapping)
        {
            CurrentVersionTransparentCountMapping1 = new Dictionary<string, int>();
            CurrentVersionTransparentCountMapping2 = new Dictionary<string, int>();
            float level_0 = 0.0f;
            float level_1 = 0.1f;
            float level_2 = 0.2f;
            float level_3 = 0.3f;
            float level_4 = 0.4f;
            float level_5 = 0.5f;
            float level_6 = 0.6f;
            float level_7 = 0.8f;
            float level_8 = 0.9f;
            float level_9 = 1.0f;

            List<TextureData> transparentExceptionList = new List<TextureData>();
            foreach (var data in currentVerisonFilesMapping.Values)
            {
                foreach (var childData in data.Values)
                {
                    var transparentPercentage = childData.m_TransparentPercentage;
                    if (transparentPercentage > 0f)
                    {
                        transparentExceptionList.Add(childData);
                    }

                    // TP 1
                    string key = "";
                    if (transparentPercentage <= level_0)
                    {
                        key = TP_SHHOW_LIST_1[0];
                    }
                    else if (transparentPercentage > level_0 && transparentPercentage <= level_1)
                    {
                        key = TP_SHHOW_LIST_1[1];
                    }
                    else if (transparentPercentage > level_1 && transparentPercentage <= level_2)
                    {
                        key = TP_SHHOW_LIST_1[2];
                    }
                    else if (transparentPercentage > level_2 && transparentPercentage <= level_3)
                    {
                        key = TP_SHHOW_LIST_1[3];
                    }
                    else if (transparentPercentage > level_3 && transparentPercentage <= level_4)
                    {
                        key = TP_SHHOW_LIST_1[4];
                    }
                    else if (transparentPercentage > level_4 && transparentPercentage <= level_5)
                    {
                        key = TP_SHHOW_LIST_1[5];
                    }
                    else if (transparentPercentage > level_5 && transparentPercentage <= level_6)
                    {
                        key = TP_SHHOW_LIST_1[6];
                    }
                    else if (transparentPercentage > level_6 && transparentPercentage <= level_7)
                    {
                        key = TP_SHHOW_LIST_1[7];
                    }
                    else if (transparentPercentage > level_7 && transparentPercentage <= level_8)
                    {
                        key = TP_SHHOW_LIST_1[8];
                    }
                    else if (transparentPercentage > level_8 && transparentPercentage < level_9)
                    {
                        key = TP_SHHOW_LIST_1[9];
                    }
                    else if (transparentPercentage >= level_9)
                    {
                        key = TP_SHHOW_LIST_1[10];
                    }

                    int amount = 0;
                    if (!CurrentVersionTransparentCountMapping1.TryGetValue(key, out amount)) amount = 0;

                    amount++;
                    CurrentVersionTransparentCountMapping1[key] = amount;

                    // TP 2
                    string key1 = "";
                    if (transparentPercentage >= level_2)
                    {
                        key1 = TP_SHHOW_LIST_2[0];

                        int amount1 = 0;
                        if (!CurrentVersionTransparentCountMapping2.TryGetValue(key1, out amount1)) amount1 = 0;

                        amount1++;
                        CurrentVersionTransparentCountMapping2[key1] = amount1;
                    }
                    if (transparentPercentage >= level_3)
                    {
                        key1 = TP_SHHOW_LIST_2[1];
         
                        int amount1 = 0;
                        if (!CurrentVersionTransparentCountMapping2.TryGetValue(key1, out amount1)) amount1 = 0;

                        amount1++;
                        CurrentVersionTransparentCountMapping2[key1] = amount1;
                    }
                    if (transparentPercentage >= level_4)
                    {
                        key1 = TP_SHHOW_LIST_2[2];

                        int amount1 = 0;
                        if (!CurrentVersionTransparentCountMapping2.TryGetValue(key1, out amount1)) amount1 = 0;

                        amount1++;
                        CurrentVersionTransparentCountMapping2[key1] = amount1;
                    }
                    if (transparentPercentage >= level_5)
                    {
                        key1 = TP_SHHOW_LIST_2[3];

                        int amount1 = 0;
                        if (!CurrentVersionTransparentCountMapping2.TryGetValue(key1, out amount1)) amount1 = 0;

                        amount1++;
                        CurrentVersionTransparentCountMapping2[key1] = amount1;
                    }
                    if (transparentPercentage >= level_6)
                    {
                        key1 = TP_SHHOW_LIST_2[4];

                        int amount1 = 0;
                        if (!CurrentVersionTransparentCountMapping2.TryGetValue(key1, out amount1)) amount1 = 0;

                        amount1++;
                        CurrentVersionTransparentCountMapping2[key1] = amount1;
                    }
                    if (transparentPercentage >= level_7)
                    {
                        key1 = TP_SHHOW_LIST_2[5];

                        int amount1 = 0;
                        if (!CurrentVersionTransparentCountMapping2.TryGetValue(key1, out amount1)) amount1 = 0;

                        amount1++;
                        CurrentVersionTransparentCountMapping2[key1] = amount1;
                    }
                    if (transparentPercentage >= level_8)
                    {
                        key1 = TP_SHHOW_LIST_2[6];

                        int amount1 = 0;
                        if (!CurrentVersionTransparentCountMapping2.TryGetValue(key1, out amount1)) amount1 = 0;

                        amount1++;
                        CurrentVersionTransparentCountMapping2[key1] = amount1;
                    }             
                }
            }

            return transparentExceptionList;
        }

        #endregion

        #region Internal Methods

        internal static bool ParseData(string lastVersionAsset2DFolder, string currentVersionAsset2DFolder,string findFilefilters,bool onlyCache)
        {
            FindFileFilters = findFilefilters.Split('|');
            TextureTypeNameMapping = new Dictionary<string, string>();

            int _lastFileTotalAmount = 0;
            LastVerisonFilesMapping = GetTextureDataMapping(lastVersionAsset2DFolder,out _lastFileTotalAmount,ref TextureTypeNameMapping, onlyCache);
            LastTotalFileAmount = _lastFileTotalAmount;
            int _currentFileTotalAmount = 0;
            CurrentVerisonFilesMapping = GetTextureDataMapping(currentVersionAsset2DFolder, out _currentFileTotalAmount,ref TextureTypeNameMapping, onlyCache);
            CurrentTotalFileAmount = _currentFileTotalAmount;
            if (CurrentVerisonFilesMapping != null)
            {
                CurrentFilesSizeMapping = new Dictionary<string, long>();
                CurrentFilesGpuSizeMapping = new Dictionary<string, long>();
                CurrentAddedFilesSizeMapping = new Dictionary<string, long>();
                CurrentAddedFilesGpuSizeMapping = new Dictionary<string, long>();
                CurrentBeforeModifiedFilesSizeMapping = new Dictionary<string, long>();
                CurrentBeforeModifiedFilesGpuSizeMapping = new Dictionary<string, long>();
                CurrentAfterModifiedFilesSizeMapping = new Dictionary<string, long>();
                CurrentAfterModifiedFilesGpuSizeMapping = new Dictionary<string, long>();
                CurrentDeletedFilesSizeMapping = new Dictionary<string, long>();
                CurrentDeletedFilesGpuSizeMapping = new Dictionary<string, long>();
                CurrentVersionDeletedMapping = new Dictionary<string, Dictionary<string, TextureData>>();
                CurrentVersionAddedMapping = new Dictionary<string, Dictionary<string, TextureData>>();
                CurrentVersionModifiedMapping = new Dictionary<string, Dictionary<string, TextureData>>();
                //CurrentRepeatFilesExceptionMapping = getRepeatFilesExceptionData(CurrentVerisonFilesMapping);
                CurrentVersionTransparentExceptiongMapping = getTransparentExceptionData(CurrentVerisonFilesMapping);
                Dictionary<string, TextureData> tempDic = null;

                // New Version Deleted
                if (LastVerisonFilesMapping != null)
                {
                    LastFilesSizeMapping = new Dictionary<string, long>();
                    LastFilesGpuSizeMapping = new Dictionary<string, long>();
                    foreach (var key in LastVerisonFilesMapping.Keys)
                    {
                        // Deleted File Size Collection
                        long currentDeletedFileSize = 0;
                        long currentDeletedFileGpuSize = 0;
                        if (!CurrentDeletedFilesSizeMapping.TryGetValue(key, out currentDeletedFileSize))
                        {
                            currentDeletedFileSize = 0;
                        }
                        if (!CurrentDeletedFilesGpuSizeMapping.TryGetValue(key, out currentDeletedFileGpuSize))
                        {
                            currentDeletedFileGpuSize = 0;
                        }
                        // File Size Collection
                        var lastDataList = LastVerisonFilesMapping[key];
                        long lastFileSize = 0;
                        long lastFileGpuSize = 0;
                        if (!LastFilesSizeMapping.TryGetValue(key, out lastFileSize))
                        {
                            lastFileSize = 0;
                        }
                        if (!LastFilesGpuSizeMapping.TryGetValue(key, out lastFileGpuSize))
                        {
                            lastFileGpuSize = 0;
                        }

                        foreach (var lastData in lastDataList)
                        {
                            lastFileSize += lastData.Value.m_DiskSize;
                            lastFileGpuSize += lastData.Value.m_GpuSize;
                        }

                        // File Update Collection
                        if (!CurrentVerisonFilesMapping.ContainsKey(key))
                        {
                            CurrentVersionDeletedMapping[key] = LastVerisonFilesMapping[key];
                            foreach (var childData in LastVerisonFilesMapping[key])
                            {
                                currentDeletedFileSize += childData.Value.m_DiskSize;
                                currentDeletedFileGpuSize += childData.Value.m_GpuSize;
                            }
                        }
                        else
                        {
                            Dictionary<string, TextureData> lastVersionDic = LastVerisonFilesMapping[key];
                            Dictionary<string, TextureData> newVersionDic = CurrentVerisonFilesMapping[key];
                            foreach (var file in lastVersionDic.Keys)
                            {
                                if (!newVersionDic.ContainsKey(file))
                                {
                                    if (!CurrentVersionDeletedMapping.TryGetValue(key, out tempDic))
                                    {
                                        tempDic = new Dictionary<string, TextureData>();
                                        CurrentVersionDeletedMapping[key] = tempDic;
                                    }

                                    tempDic.Add(file, lastVersionDic[file]);

                                    currentDeletedFileSize += lastVersionDic[file].m_DiskSize;
                                    currentDeletedFileGpuSize += lastVersionDic[file].m_GpuSize;
                                }
                            }
                        }

                        LastFilesSizeMapping[key] = lastFileSize;
                        LastFilesGpuSizeMapping[key] = lastFileGpuSize;

                        CurrentDeletedFilesSizeMapping[key] = currentDeletedFileSize;
                        CurrentDeletedFilesGpuSizeMapping[key] = currentDeletedFileGpuSize;
                    }
                }

                // New Version Added & Modified
                foreach (var key in CurrentVerisonFilesMapping.Keys)
                {
                    // Total File Size Collection
                    var currentDataList = CurrentVerisonFilesMapping[key];
                    long currentFileSize = 0;
                    long currentFileGpuSize = 0;
                    if (!CurrentFilesSizeMapping.TryGetValue(key, out currentFileSize))
                    {
                        currentFileSize = 0;
                    }
                    if (!CurrentFilesGpuSizeMapping.TryGetValue(key, out currentFileGpuSize))
                    {
                        currentFileGpuSize = 0;
                    }
                    foreach (var currentData in currentDataList)
                    {
                        currentFileSize += currentData.Value.m_DiskSize;
                        currentFileGpuSize += currentData.Value.m_GpuSize;
                    }
                    // Added File Size Collection
                    long currentAddedFileSize = 0;
                    long currentAddedFileGpuSize = 0;
                    if (!CurrentAddedFilesSizeMapping.TryGetValue(key, out currentAddedFileSize))
                    {
                        currentAddedFileSize = 0;
                    }
                    if (!CurrentAddedFilesGpuSizeMapping.TryGetValue(key, out currentAddedFileGpuSize))
                    {
                        currentAddedFileGpuSize = 0;
                    }
                    // Modified File Size Collection
                    long currentBeforeModifiedFileSize = 0;
                    long currentBeforeModifiedFileGpuSize = 0;
                    long currentAfterModifiedFileSize = 0;
                    long currentAfterModifiedFileGpuSize = 0;
                    if (!CurrentBeforeModifiedFilesSizeMapping.TryGetValue(key, out currentBeforeModifiedFileSize))
                    {
                        currentBeforeModifiedFileSize = 0;
                    }
                    if (!CurrentBeforeModifiedFilesGpuSizeMapping.TryGetValue(key, out currentBeforeModifiedFileGpuSize))
                    {
                        currentBeforeModifiedFileGpuSize = 0;
                    }
                    if (!CurrentAfterModifiedFilesSizeMapping.TryGetValue(key, out currentAfterModifiedFileSize))
                    {
                        currentAfterModifiedFileSize = 0;
                    }
                    if (!CurrentAfterModifiedFilesGpuSizeMapping.TryGetValue(key, out currentAfterModifiedFileGpuSize))
                    {
                        currentAfterModifiedFileGpuSize = 0;
                    }
                    // File Update Collection
                    if (LastVerisonFilesMapping == null || !LastVerisonFilesMapping.ContainsKey(key))
                    {
                        CurrentVersionAddedMapping[key] = currentDataList;
                        foreach (var currentData in currentDataList)
                        {
                            currentAddedFileSize += currentData.Value.m_DiskSize;
                            currentAddedFileGpuSize += currentData.Value.m_GpuSize;
                        }
                    }
                    else
                    {
                        Dictionary<string, TextureData> lastVersionDic = LastVerisonFilesMapping[key];
                        Dictionary<string, TextureData> newVersionDic = CurrentVerisonFilesMapping[key];
                        foreach (var file in newVersionDic.Keys)
                        {
                            if (!lastVersionDic.ContainsKey(file))
                            {
                                if (!CurrentVersionAddedMapping.TryGetValue(key, out tempDic))
                                {
                                    tempDic = new Dictionary<string, TextureData>();
                                    CurrentVersionAddedMapping[key] = tempDic;
                                }

                                tempDic.Add(file, newVersionDic[file]);
                                currentAddedFileSize += newVersionDic[file].m_DiskSize;
                                currentAddedFileGpuSize += newVersionDic[file].m_GpuSize;
                            }
                            else
                            {
                                if (newVersionDic[file].m_TextureMD5 != lastVersionDic[file].m_TextureMD5)
                                {
                                    if (!CurrentVersionModifiedMapping.TryGetValue(key, out tempDic))
                                    {
                                        tempDic = new Dictionary<string, TextureData>();
                                        CurrentVersionModifiedMapping[key] = tempDic;
                                    }

                                    tempDic.Add(file, newVersionDic[file]);
                                    newVersionDic[file].m_DiskOldSize = lastVersionDic[file].m_DiskSize;
                                    newVersionDic[file].m_GpuOldSize = lastVersionDic[file].m_GpuSize;
                                    newVersionDic[file].m_OldWidth = lastVersionDic[file].m_Width;
                                    newVersionDic[file].m_OldHeight = lastVersionDic[file].m_Height;
                                    newVersionDic[file].m_TransparentOldPercentage = lastVersionDic[file].m_TransparentPercentage;
                                    currentBeforeModifiedFileSize += lastVersionDic[file].m_DiskSize;
                                    currentBeforeModifiedFileGpuSize += lastVersionDic[file].m_GpuSize;
                                    currentAfterModifiedFileSize += newVersionDic[file].m_DiskSize;
                                    currentAfterModifiedFileGpuSize += newVersionDic[file].m_GpuSize;
                                }
                            }
                        }
                    }

                    CurrentFilesSizeMapping[key] = currentFileSize;
                    CurrentFilesGpuSizeMapping[key] = currentFileGpuSize;
                    CurrentAddedFilesSizeMapping[key] = currentAddedFileSize;
                    CurrentAddedFilesGpuSizeMapping[key] = currentAddedFileGpuSize;
                    CurrentBeforeModifiedFilesSizeMapping[key] = currentBeforeModifiedFileSize;
                    CurrentBeforeModifiedFilesGpuSizeMapping[key] = currentBeforeModifiedFileGpuSize;
                    CurrentAfterModifiedFilesSizeMapping[key] = currentAfterModifiedFileSize;
                    CurrentAfterModifiedFilesGpuSizeMapping[key] = currentAfterModifiedFileGpuSize;
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
