using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine.UI;

namespace jj.TATools.Editor
{
    internal class SpriteAtlasData
    {
        #region Fields

        const char CHAR_SPLIT_FIRST_FLAG = '|';
        const char CHAR_SPLIT_SECOND_FLAG = '#';
        const char CHAR_SPLIT_THIRD_FLAG = '$';

        static string SPRITEATLAS_FORMAT_THRESHOLD = "ASTC_";
        static int SPRITEATLAS_ANISOLEVEL_THRESHOLD = 1;
        static float SPRITEATLAS_MEMORY_OVER_THRESHOLD = 0.2f;
        static int SPRITEATLAS_MIPMAP_THRESHOLD = 1;
        static int SPRITEATLAS_RW_THRESHOLD = 1;
        static int SPRITEATLAS_FILTERMODE_THRESHOLD = 2;
        static int SPRITEATLAS_MAX_RESOLUTION_THRESHOLD = 2048;
        static float SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD = 0.5f;

        internal class SpriteAtlasPageData
        {
            internal string m_PageName;
            internal int m_Width;
            internal int m_Height;
            internal long m_MemorySize;
            internal float m_TransparentPercentage;

            internal Texture2D m_PageTex;

            internal SpriteAtlasPageData(string pageName, int width, int height, long memorySize, float transparentPercentage, Texture2D pageTex)
            {
                this.m_PageName = pageName;
                this.m_Width = width;
                this.m_Height = height;
                this.m_MemorySize = memorySize;
                this.m_TransparentPercentage = transparentPercentage;
                this.m_PageTex = pageTex;
            }

            internal string m_PageImageFile;

            internal SpriteAtlasPageData(string pageName, int width, int height, long memorySize, float transparentPercentage)
            {
                this.m_PageName = pageName;
                this.m_Width = width;
                this.m_Height = height;
                this.m_MemorySize = memorySize;
                this.m_TransparentPercentage = transparentPercentage;
            }

            internal bool TransparentInvalid()
            {
                return this.m_TransparentPercentage >= SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD;
            }

        }

        internal string m_AssetPath;
        internal long m_FileDiskSize;
        internal int m_SpriteAtlasType;
        internal int m_IncludeInBuild;

        internal int m_AllowRotation;
        internal int m_TightPacking;
        internal int m_AlphaDilation;
        internal int m_Padding;

        internal int m_RW;
        internal int m_GenerateMipmaps;
        internal int m_Srgb;
        internal int m_FilterMode;
        internal int m_AnisoLevel;

        internal int m_OverrideIOS;
        internal int m_MaxSizeIOS;
        internal int m_FormatIOS;
        internal int m_OverrideAndroid;
        internal int m_MaxSizeAndroid;
        internal int m_FormatAndroid;

        internal long m_TotalMemorySize;
        internal long m_TotalSpriteMemorySize;
        internal float m_TotalMemoryOverPercentage;

        internal List<string> m_Dependencies = new List<string>();
        internal List<string> m_InvalidFormatList = new List<string>();
        internal Dictionary<string, TextureImporterFormat> m_InvalidAndroidFormatDependencies = new Dictionary<string, TextureImporterFormat>();
        internal Dictionary<string, TextureImporterFormat> m_InvalidIosFormatDependencies = new Dictionary<string, TextureImporterFormat>();
        internal List<SpriteAtlasPageData> m_PageDataList = new List<SpriteAtlasPageData>();

        #endregion

        internal SpriteAtlasData(string assetPath)
        {
            SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);

            // Base
            this.m_AssetPath = assetPath;
            this.m_FileDiskSize = AssetDetectionUtility.GetFileDiskSize(assetPath);
            this.m_SpriteAtlasType = spriteAtlas.isVariant ? 1 : 0;
            this.m_IncludeInBuild = spriteAtlas.IsIncludeInBuild() ? 1 : 0;

            var packingSettings = spriteAtlas.GetPackingSettings();
            this.m_AllowRotation = packingSettings.enableRotation ? 1 : 0;
            this.m_TightPacking = packingSettings.enableTightPacking ? 1 : 0;
            this.m_AlphaDilation = packingSettings.enableAlphaDilation ? 1 : 0;
            this.m_Padding = packingSettings.padding;

            var textureSettings = spriteAtlas.GetTextureSettings();
            this.m_RW = textureSettings.readable ? 1 : 0;
            this.m_GenerateMipmaps = textureSettings.generateMipMaps ? 1 : 0;
            this.m_Srgb = textureSettings.sRGB ? 1 : 0;
            this.m_FilterMode = (int)textureSettings.filterMode;
            this.m_AnisoLevel = textureSettings.anisoLevel;

            var iosTS = spriteAtlas.GetPlatformSettings("iPhone");
            if (iosTS != null)
            {
                this.m_OverrideIOS = iosTS.overridden ? 1 : 0;
                this.m_MaxSizeIOS = iosTS.maxTextureSize;
                this.m_FormatIOS = (int)iosTS.format;
            }
            else
            {
                this.m_MaxSizeIOS = -1;
            }
            var androidTS = spriteAtlas.GetPlatformSettings("Android");
            if (androidTS != null)
            {
                this.m_OverrideAndroid = androidTS.overridden ? 1 : 0;
                this.m_MaxSizeAndroid = androidTS.maxTextureSize;
                this.m_FormatAndroid = (int)androidTS.format;
            }
            else
            {
                this.m_MaxSizeAndroid = -1;
            }

            // Depend Sprite //////////////////////////////////////
            var dependencies = AssetDetectionUtility.GetDependencies<Texture>(assetPath, true);
            foreach (var dep in dependencies)
            {
                var tImporter = AssetImporter.GetAtPath(dep) as TextureImporter;
                if (tImporter != null)
                {
                    if (!this.m_Dependencies.Contains(dep))
                    {
                        this.m_Dependencies.Add(dep);
                        Texture tex = AssetDatabase.LoadAssetAtPath<Texture>(dep);
                        long texMemorySize = AssetDetectionUtility.GetTextureFileSize(tex);
                        this.m_TotalSpriteMemorySize += texMemorySize;

                        var androidSetting = tImporter.GetPlatformTextureSettings("Android");
                        var iosSetting = tImporter.GetPlatformTextureSettings("iPhone");
                        var invalidFormat = false;
                        if (!(androidSetting.format == TextureImporterFormat.RGB24 || androidSetting.format == TextureImporterFormat.RGBA32))
                        {
                            invalidFormat = true;
                            this.m_InvalidAndroidFormatDependencies[dep] = androidSetting.format;
                        }
                        if (!(iosSetting.format == TextureImporterFormat.RGB24 || iosSetting.format == TextureImporterFormat.RGBA32))
                        {
                            invalidFormat = true;
                            this.m_InvalidIosFormatDependencies[dep] = iosSetting.format;
                        }

                        if(invalidFormat && !m_InvalidFormatList.Contains(dep))
                            m_InvalidFormatList.Add(dep);
                    }
                }
            }

            // Page //////////////////////////////////////
            var pageTexArr = AssetDetectionUtility.GetPreviewTextures(spriteAtlas);
            foreach (var tex in pageTexArr)
            {
                long memorySize = AssetDetectionUtility.GetTextureFileSize(tex);
                this.m_TotalMemorySize += memorySize;
                Texture2D readableTex = AssetDetectionUtility.DuplicateTexture(tex);
                readableTex.name = tex.name;
                Color[] colors = readableTex.GetPixels();
                int invalidPixel = 0;
                foreach (var color in colors)
                {
                    float value = color.a;
                    if (value == 0.0f)
                    {
                        invalidPixel++;
                    }
                }

                float transparenPercentage = invalidPixel * 1.0f / colors.Length;

                SpriteAtlasPageData sapData = new SpriteAtlasPageData(tex.name, tex.width, tex.height, memorySize, transparenPercentage,readableTex);
                m_PageDataList.Add(sapData);
            }
        }

        internal void OutputPagePreviewTextures(string outputFoler)
        {
            string finalOutputFoler = outputFoler + Path.GetFileNameWithoutExtension(this.m_AssetPath) + "\\";
            if (!Directory.Exists(finalOutputFoler)) Directory.CreateDirectory(finalOutputFoler);

            foreach (var pData in this.m_PageDataList)
            {
                byte[] bytes = ImageConversion.EncodeToPNG(pData.m_PageTex);
                File.WriteAllBytes(finalOutputFoler + pData.m_PageTex.name + ".png", bytes);
            }
        }

        #region Internal Methods

        internal static void PackAllAtlases()
        {
            SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);
        }
        internal static void UpdateDataFromConfig()
        {
            SPRITEATLAS_FORMAT_THRESHOLD = AppConfigHelper.SPRITEATLAS_FORMAT_THRESHOLD;
            SPRITEATLAS_ANISOLEVEL_THRESHOLD = AppConfigHelper.SPRITEATLAS_ANISOLEVEL_THRESHOLD;
            SPRITEATLAS_MEMORY_OVER_THRESHOLD = AppConfigHelper.SPRITEATLAS_MEMORY_OVER_THRESHOLD;
            SPRITEATLAS_MIPMAP_THRESHOLD = AppConfigHelper.SPRITEATLAS_MIPMAP_THRESHOLD;
            SPRITEATLAS_RW_THRESHOLD = AppConfigHelper.SPRITEATLAS_RW_THRESHOLD;
            SPRITEATLAS_FILTERMODE_THRESHOLD = AppConfigHelper.SPRITEATLAS_FILTERMODE_THRESHOLD;
            SPRITEATLAS_MAX_RESOLUTION_THRESHOLD = AppConfigHelper.SPRITEATLAS_MAX_RESOLUTION_THRESHOLD;
            SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD = AppConfigHelper.SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD;
        }

        internal bool FormatInvalid()
        {
            ETextureImporterFormat androidFormat = (ETextureImporterFormat)this.m_FormatAndroid;
            string androidFormatName = androidFormat.ToString();
            ETextureImporterFormat iosFormat = (ETextureImporterFormat)this.m_FormatIOS;
            string iosFormatName = iosFormat.ToString();
            return !androidFormatName.Contains(SPRITEATLAS_FORMAT_THRESHOLD) || (m_OverrideIOS == 1 && !iosFormatName.Contains(SPRITEATLAS_FORMAT_THRESHOLD));
        }

        internal bool AnisoLevelInvalid()
        {
            return this.m_AnisoLevel > SPRITEATLAS_ANISOLEVEL_THRESHOLD;
        }

        internal bool MipmapInvalid()
        {
            return this.m_GenerateMipmaps == SPRITEATLAS_MIPMAP_THRESHOLD;
        }

        internal bool RWInvalid()
        {
            return this.m_RW == SPRITEATLAS_RW_THRESHOLD;
        }

        internal bool FilterModeInvalid()
        {
            return this.m_FilterMode == SPRITEATLAS_FILTERMODE_THRESHOLD;
        }

        internal bool ResolutionInvalid()
        {
            return this.m_MaxSizeAndroid > SPRITEATLAS_MAX_RESOLUTION_THRESHOLD || this.m_MaxSizeIOS > SPRITEATLAS_MAX_RESOLUTION_THRESHOLD;
        }

        internal bool MemoryInvalid()
        {
            return this.m_TotalMemoryOverPercentage >= SPRITEATLAS_MEMORY_OVER_THRESHOLD;
        }

        internal void GetPageImageFiles(string baseFolder)
        {
            string folderPath = baseFolder + Path.GetFileNameWithoutExtension(this.m_AssetPath);
            if (!Directory.Exists(folderPath)) return;

            var imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
            Dictionary<string, string> imageMapping = new Dictionary<string, string>();
            foreach (var imageFile in imageFiles)
            {
                var imageFileName = Path.GetFileNameWithoutExtension(imageFile);
                imageMapping[imageFileName] = imageFile;
            }

            foreach (var pData in this.m_PageDataList)
            {
                var pageName = pData.m_PageName;
                string imageFile = null;
                if (imageMapping.TryGetValue(pageName, out imageFile))
                {
                    pData.m_PageImageFile = imageFile;
                }
                else
                    pData.m_PageImageFile = "";
            }
        }

        #endregion
    }

    internal class SpirteAtlasTool
    {
        #region Fields

        internal static Dictionary<string, List<string>> m_PrefabToSpriteAtlasMapping = new Dictionary<string, List<string>>();
        internal static List<SpriteAtlasData> m_SpriteAtlasRefInvalidFormatSpriteList = new List<SpriteAtlasData>();

        #endregion

        #region Internal Methods

        internal static List<SpriteAtlasData> DoCheckSpriteAtlas(string moduleFolder,string tick,out string outputFolder)
        {
            m_PrefabToSpriteAtlasMapping.Clear();
            m_SpriteAtlasRefInvalidFormatSpriteList.Clear();
            outputFolder = "";
            List<SpriteAtlasData> spriteAtlasDataList = new List<SpriteAtlasData>();

            var guids = AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name, new string[] { moduleFolder });
            if (guids == null || guids.Length == 0) return null;

            // Create Folder Or Delete Old Files ////////////////////////
            outputFolder = AppConfigHelper.ASSET_TEMP_CACHE_FLODER + "\\" + ConstDefine.SPRITEATLAS_CACHE_FOLDER;
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
            outputFolder = outputFolder + "\\" + tick;
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
            DirectoryInfo dInfo = new DirectoryInfo(moduleFolder);
            outputFolder = outputFolder + "\\" + dInfo.Name + "\\";
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
            else
            {
                var oldFiles = Directory.GetFiles(outputFolder, "*.*", SearchOption.AllDirectories);
                foreach (var oldFile in oldFiles)
                {
                    File.Delete(oldFile);
                }
            }

            // Pack All Atlas ///////////////////////////////////////////
            SpriteAtlasData.PackAllAtlases();

            // Check All Atlas ///////////////////////////////////////////
            Dictionary<string, List<string>> spriteToAtlasMapping = new Dictionary<string, List<string>>();
            List<string> tempList = null;
            List<SpriteAtlasData> dataList = new List<SpriteAtlasData>();
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                EditorUtility.DisplayProgressBar("Check SpriteAtlas[" + dInfo.Name + "]", "Check..." + assetPath, (i + 1) * 1.0f / guids.Length);

                var saData = new SpriteAtlasData(assetPath);
                saData.OutputPagePreviewTextures(outputFolder);

                spriteAtlasDataList.Add(saData);
                foreach (var spritePath in saData.m_Dependencies)
                {
                    if (!spriteToAtlasMapping.TryGetValue(spritePath, out tempList))
                    {
                        tempList = new List<string>();
                        spriteToAtlasMapping[spritePath] = tempList;
                    }

                    tempList.Add(assetPath);
                }

                if (saData.m_InvalidFormatList.Count > 0)
                {
                    m_SpriteAtlasRefInvalidFormatSpriteList.Add(saData);
                }
            }

            EditorUtility.ClearProgressBar();

            // Check All Prefab
            guids = AssetDatabase.FindAssets("t:Prefab", new string[] { moduleFolder });
            Dictionary<string, List<string>> prefabToSpriteMapping = new Dictionary<string, List<string>>();
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                EditorUtility.DisplayProgressBar("Check Prefab Ref SpriteAtlas[" + dInfo.Name + "]", "Check..." + assetPath, (i + 1) * 1.0f / guids.Length);

                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                var images = prefab.GetComponentsInChildren<Image>(true);
                if (images == null || images.Length == 0) continue;

                if (!prefabToSpriteMapping.TryGetValue(assetPath, out tempList))
                {
                    tempList = new List<string>();
                    prefabToSpriteMapping[assetPath] = tempList;
                }

                foreach (var image in images)
                {
                    var sprite = image.sprite;
                    if (sprite == null) continue;

                    var spritePath = AssetDatabase.GetAssetPath(sprite);
                    if (!tempList.Contains(spritePath))
                        tempList.Add(spritePath);
                }
            }

            EditorUtility.ClearProgressBar();

            foreach (var data in prefabToSpriteMapping)
            {
                var prefabPath = data.Key;
                var spritePathList = data.Value;
                var refAtlasList = new List<string>();
                foreach (var spritePath in spritePathList)
                {
                    if (spriteToAtlasMapping.TryGetValue(spritePath, out tempList))
                    {
                        foreach (var atlasPath in tempList)
                        {
                            if (!refAtlasList.Contains(atlasPath))
                                refAtlasList.Add(atlasPath);
                        }
                    }
                }

                if (refAtlasList.Count > 0)
                {
                    m_PrefabToSpriteAtlasMapping[prefabPath] = refAtlasList;
                }
            }

            return spriteAtlasDataList;
        }

        #endregion
    }
}