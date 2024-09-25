namespace jj.TATools.Editor
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [System.Serializable]
    internal class TextureSetting
    {
        #region Fields

        public ESpecialRuleType m_RuleType = ESpecialRuleType._0_Default;
        public List<string> m_AssetParentFolderListForPartFolderRule = new List<string>();
        public List<string> m_AssetParentFolderListForFullFolderRule = new List<string>();
        public List<string> m_AssetNameListForAssetNameRule = new List<string>();
        public List<string> m_AssetPathListForAssetPathRule = new List<string>();

        public PropertySetMode m_TextureTypeSetMode = PropertySetMode.Ignore;
        public TextureImporterType m_TextureType = TextureImporterType.Default;
        public PropertySetMode m_SRGBSetMode = PropertySetMode.Once;
        public bool m_SRGB = true;
        public PropertySetMode m_AlphaSourceSetMode = PropertySetMode.Once;
        public TextureImporterAlphaSource m_AlphaSource = TextureImporterAlphaSource.FromInput;
        public PropertySetMode m_AlphaIsTransparencySetMode = PropertySetMode.Once;
        public bool m_AlphaIsTransparency = false;
        public PropertySetMode m_ReadableSetMode = PropertySetMode.Force;
        public bool m_Readable = false;
        public PropertySetMode m_VtOnlySetMode = PropertySetMode.Once;
        public bool m_VtOnly = false;
        public PropertySetMode m_MipmapSetMode = PropertySetMode.Once;
        public bool m_Mipmap = false;
        public PropertySetMode m_IgnorePngGammaSetMode = PropertySetMode.Once;
        public bool m_IgnorePngGamma = false;
        public PropertySetMode m_WrapModeSetMode = PropertySetMode.Once;
        public TextureWrapMode m_WrapMode = TextureWrapMode.Repeat;
        public PropertySetMode m_FilterModeSetMode = PropertySetMode.Once;
        public FilterMode m_FilterMode = FilterMode.Bilinear;
        public PropertySetMode m_AnisoLevelSetMode = PropertySetMode.Once;
        public int m_AnisoLevel = 1;
        public PropertySetMode m_MaxSizeForAndroidSetMode = PropertySetMode.Once;
        public int m_MaxSizeForAndroid = 2048;
        public PropertySetMode m_ResizeAlgorithmForAndroidSetMode = PropertySetMode.Once;
        public TextureResizeAlgorithm m_ResizeAlgorithmForAndroid = TextureResizeAlgorithm.Mitchell;
        public PropertySetMode m_FormatForAndroidSetMode = PropertySetMode.Force;
        public TextureImporterFormat m_FormatForAndroid = TextureImporterFormat.ASTC_6x6;
        public PropertySetMode m_CompressionForAndroidSetMode = PropertySetMode.Once;
        public TextureImporterCompression m_CompressionForAndroid = TextureImporterCompression.Compressed;
        public PropertySetMode m_MaxSizeForIosSetMode = PropertySetMode.Once;
        public int m_MaxSizeForIos = 2048;
        public PropertySetMode m_ResizeAlgorithmForIosSetMode = PropertySetMode.Once;
        public TextureResizeAlgorithm m_ResizeAlgorithmForIos = TextureResizeAlgorithm.Mitchell;
        public PropertySetMode m_FormatForIosSetMode = PropertySetMode.Force;
        public TextureImporterFormat m_FormatForIos = TextureImporterFormat.ASTC_6x6;
        public PropertySetMode m_CompressionForIosSetMode = PropertySetMode.Once;
        public TextureImporterCompression m_CompressionForIos = TextureImporterCompression.Compressed;

        private bool m_TextureTypeNeedUpdate = false;
        private bool m_SRGBNeedUpdate = false;
        private bool m_AlphaIsTransparencyNeedUpdate = false;
        private bool m_AlphaSourceNeedUpdate = false;
        private bool m_ReadableNeedUpdate = false;
        private bool m_VtOnlyNeedUpdate = false;
        private bool m_MipmapNeedUpdate = false;
        private bool m_IgnorePngGammaNeedUpdate = false;
        private bool m_WrapModeNeedUpdate = false;
        private bool m_FilterModeNeedUpdate = false;
        private bool m_AnisoLevelNeedUpdate = false;
        private bool m_AndroidPlatformOverriddenNeedUpdate = false;
        private bool m_MaxSizeForAndroidNeedUpdate = false;
        private bool m_ResizeAlgorithmForAndroidNeedUpdate = false;
        private bool m_FormatForAndroidNeedUpdate = false;
        private bool m_CompressionForAndroidNeedUpdate = false;
        private bool m_IosPlatformOverriddenNeedUpdate = false;
        private bool m_MaxSizeForIosNeedUpdate = false;
        private bool m_ResizeAlgorithmForIosNeedUpdate = false;
        private bool m_FormatForIosNeedUpdate = false;
        private bool m_CompressionForIosNeedUpdate = false;

        #endregion

        #region Internal Methods

        internal static TextureSetting GetDefaultSetting()
        {
            var defaultSetting = new TextureSetting();

            defaultSetting.m_RuleType = ESpecialRuleType._0_Default;
            defaultSetting.m_AssetParentFolderListForPartFolderRule = new List<string>();
            defaultSetting.m_AssetParentFolderListForFullFolderRule = new List<string>();
            defaultSetting.m_AssetNameListForAssetNameRule = new List<string>();
            defaultSetting.m_AssetPathListForAssetPathRule = new List<string>();

            defaultSetting.m_TextureTypeSetMode = PropertySetMode.Ignore;
            defaultSetting.m_TextureType = TextureImporterType.Default;
            defaultSetting.m_SRGBSetMode = PropertySetMode.Once;
            defaultSetting.m_SRGB = true;
            defaultSetting.m_AlphaSourceSetMode = PropertySetMode.Once;
            defaultSetting.m_AlphaSource = TextureImporterAlphaSource.FromInput;
            defaultSetting.m_AlphaIsTransparencySetMode = PropertySetMode.Once;
            defaultSetting.m_AlphaIsTransparency = false;
            defaultSetting.m_ReadableSetMode = PropertySetMode.Force;
            defaultSetting.m_Readable = false;
            defaultSetting.m_VtOnlySetMode = PropertySetMode.Once;
            defaultSetting.m_VtOnly = false;
            defaultSetting.m_MipmapSetMode = PropertySetMode.Once;
            defaultSetting.m_Mipmap = false;
            defaultSetting.m_IgnorePngGammaSetMode = PropertySetMode.Once;
            defaultSetting.m_IgnorePngGamma = false;
            defaultSetting.m_WrapModeSetMode = PropertySetMode.Once;
            defaultSetting.m_WrapMode = TextureWrapMode.Repeat;
            defaultSetting.m_FilterModeSetMode = PropertySetMode.Once;
            defaultSetting.m_FilterMode = FilterMode.Bilinear;
            defaultSetting.m_AnisoLevelSetMode = PropertySetMode.Once;
            defaultSetting.m_AnisoLevel = 1;
            defaultSetting.m_MaxSizeForAndroidSetMode = PropertySetMode.Once;
            defaultSetting.m_MaxSizeForAndroid = 2048;
            defaultSetting.m_ResizeAlgorithmForAndroidSetMode = PropertySetMode.Once;
            defaultSetting.m_ResizeAlgorithmForAndroid = TextureResizeAlgorithm.Mitchell;
            defaultSetting.m_FormatForAndroidSetMode = PropertySetMode.Force;
            defaultSetting.m_FormatForAndroid = TextureImporterFormat.ASTC_6x6;
            defaultSetting.m_CompressionForAndroidSetMode = PropertySetMode.Once;
            defaultSetting.m_CompressionForAndroid = TextureImporterCompression.Compressed;
            defaultSetting.m_MaxSizeForIosSetMode = PropertySetMode.Once;
            defaultSetting.m_MaxSizeForIos = 2048;
            defaultSetting.m_ResizeAlgorithmForIosSetMode = PropertySetMode.Once;
            defaultSetting.m_ResizeAlgorithmForIos = TextureResizeAlgorithm.Mitchell;
            defaultSetting.m_FormatForIosSetMode = PropertySetMode.Force;
            defaultSetting.m_FormatForIos = TextureImporterFormat.ASTC_6x6;
            defaultSetting.m_CompressionForIosSetMode = PropertySetMode.Once;
            defaultSetting.m_CompressionForIos = TextureImporterCompression.Compressed;

            return defaultSetting;
        }

        internal bool CheckRule(string assetPath)
        {
            var assetPathLower = assetPath.ToLower();
            bool result = false;
            switch (m_RuleType)
            {
                case ESpecialRuleType._0_Default:
                    result = true;
                    break;
                case ESpecialRuleType._1_AssetParentPartFolder:
                    foreach (var partFolder in m_AssetParentFolderListForPartFolderRule)
                    {
                        if (assetPathLower.Contains(partFolder.ToLower()))
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                case ESpecialRuleType._2_AssetParentFullFolder:
                    foreach (var fullFolder in m_AssetParentFolderListForFullFolderRule)
                    {
                        if (assetPathLower.StartsWith(fullFolder.ToLower()))
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                case ESpecialRuleType._3_AssetName:
                    var assetName = Path.GetFileNameWithoutExtension(assetPathLower);
                    foreach (var nameSuffix in m_AssetNameListForAssetNameRule)
                    {
                        if (assetName.Contains(nameSuffix.ToLower()))
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                case ESpecialRuleType._4_AssetPath:
                    result = m_AssetPathListForAssetPathRule.Contains(assetPath);
                    break;
            }

            return result;
        }

        internal void DoPreImport(TextureImporter tImporter,TextureSetting globalSetting)
        {
            // Source NPOT Checking
            int sourceTexWidth = 0;
            int sourceTexHeight = 0;
            tImporter.GetSourceTextureWidthAndHeight(out sourceTexWidth, out sourceTexHeight);
            bool isNPOT = AssetImportPipelineUtility.IsNonPowerOfTwo(sourceTexWidth, sourceTexHeight);

            // First Imported //////////////////
            TextureImporterPlatformSettings androidPlatformSeting = tImporter.GetPlatformTextureSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_ANDROID);
            TextureImporterPlatformSettings iosPlatformSeting = tImporter.GetPlatformTextureSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_IOS);
            bool isFirstImported = (androidPlatformSeting != null && !androidPlatformSeting.overridden) || (iosPlatformSeting != null && !iosPlatformSeting.overridden);

            // Texture Type
            if (m_TextureTypeSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_TextureTypeNeedUpdate = true;
                    globalSetting.m_TextureType = m_TextureType;
                }
            }
            else if (m_TextureTypeSetMode == PropertySetMode.Force)
            {
                globalSetting.m_TextureTypeNeedUpdate = true;
                globalSetting.m_TextureType = m_TextureType;
            }
            // srgb
            if (m_SRGBSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_SRGBNeedUpdate = true;
                    globalSetting.m_SRGB = m_SRGB;
                }
            }
            else if (m_SRGBSetMode == PropertySetMode.Force)
            {
                globalSetting.m_SRGBNeedUpdate = true;
                globalSetting.m_SRGB = m_SRGB;
            }
            // alphaSource
            if (m_AlphaSourceSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_AlphaSourceNeedUpdate = true;
                    globalSetting.m_AlphaSource = m_AlphaSource;
                }
            }
            else if (m_AlphaSourceSetMode == PropertySetMode.Force)
            {
                globalSetting.m_AlphaSourceNeedUpdate = true;
                globalSetting.m_AlphaSource = m_AlphaSource;
            }
            // alphaIsTransparency
            if (m_AlphaIsTransparencySetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_AlphaIsTransparencyNeedUpdate = true;
                    globalSetting.m_AlphaIsTransparency = m_AlphaIsTransparency;
                }
            }
            else if (m_AlphaIsTransparencySetMode == PropertySetMode.Force)
            {
                globalSetting.m_AlphaIsTransparencyNeedUpdate = true;
                globalSetting.m_AlphaIsTransparency = m_AlphaIsTransparency;
            }
            // vtOnly
            if (m_VtOnlySetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_VtOnlyNeedUpdate = true;
                    globalSetting.m_VtOnly = m_VtOnly;
                }
            }
            else if (m_VtOnlySetMode == PropertySetMode.Force)
            {
                globalSetting.m_VtOnlyNeedUpdate = true;
                globalSetting.m_VtOnly = m_VtOnly;
            }
            // mipmapEnabled
            if (m_MipmapSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_MipmapNeedUpdate = true;
                    globalSetting.m_Mipmap = m_Mipmap;
                }
            }
            else if (m_MipmapSetMode == PropertySetMode.Force)
            {
                globalSetting.m_MipmapNeedUpdate = true;
                globalSetting.m_Mipmap = m_Mipmap;
            }
            // ignorePngGamma
            if (m_IgnorePngGammaSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_IgnorePngGammaNeedUpdate = true;
                    globalSetting.m_IgnorePngGamma = m_IgnorePngGamma;
                }
            }
            else if (m_IgnorePngGammaSetMode == PropertySetMode.Force)
            {
                globalSetting.m_IgnorePngGammaNeedUpdate = true;
                globalSetting.m_IgnorePngGamma = m_IgnorePngGamma;
            }
            // wrapMode
            if (m_WrapModeSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_WrapModeNeedUpdate = true;
                    globalSetting.m_WrapMode = m_WrapMode;
                }
            }
            else if (m_WrapModeSetMode == PropertySetMode.Force)
            {
                globalSetting.m_WrapModeNeedUpdate = true;
                globalSetting.m_WrapMode = m_WrapMode;
            }
            // filterMode
            if (m_FilterModeSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_FilterModeNeedUpdate = true;
                    globalSetting.m_FilterMode = m_FilterMode;
                }
            }
            else if (m_FilterModeSetMode == PropertySetMode.Force)
            {
                globalSetting.m_FilterModeNeedUpdate = true;
                globalSetting.m_FilterMode = m_FilterMode;
            }
            // anisoLevel
            if (m_AnisoLevelSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_AnisoLevelNeedUpdate = true;
                    globalSetting.m_AnisoLevel = m_AnisoLevel;
                }
            }
            else if (m_AnisoLevelSetMode == PropertySetMode.Force)
            {
                globalSetting.m_AnisoLevelNeedUpdate = true;
                globalSetting.m_AnisoLevel = m_AnisoLevel;
            }
            // isReadable
            if (m_ReadableSetMode == PropertySetMode.Once)
            {
                if (isFirstImported)
                {
                    globalSetting.m_ReadableNeedUpdate = true;
                    globalSetting.m_Readable = m_Readable;
                }
            }
            else if (m_ReadableSetMode == PropertySetMode.Force)
            {
                globalSetting.m_ReadableNeedUpdate = true;
                globalSetting.m_Readable = m_Readable;
            }
            // Android Platform 
            if (androidPlatformSeting != null)
            {
                if (!androidPlatformSeting.overridden)
                {
                    globalSetting.m_AndroidPlatformOverriddenNeedUpdate = true;
                }
                // Max Size
                if (m_MaxSizeForAndroidSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        globalSetting.m_MaxSizeForAndroidNeedUpdate = true;
                        globalSetting.m_MaxSizeForAndroid = m_MaxSizeForAndroid;
                    }
                }
                else if (m_MaxSizeForAndroidSetMode == PropertySetMode.Force)
                {
                    globalSetting.m_MaxSizeForAndroidNeedUpdate = true;
                    globalSetting.m_MaxSizeForAndroid = m_MaxSizeForAndroid;
                }
                // Resizi Algorithm
                if (m_ResizeAlgorithmForAndroidSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        globalSetting.m_ResizeAlgorithmForAndroidNeedUpdate = true;
                        globalSetting.m_ResizeAlgorithmForAndroid = m_ResizeAlgorithmForAndroid;
                    }
                }
                else if (m_ResizeAlgorithmForAndroidSetMode == PropertySetMode.Force)
                {
                    globalSetting.m_ResizeAlgorithmForAndroidNeedUpdate = true;
                    globalSetting.m_ResizeAlgorithmForAndroid = m_ResizeAlgorithmForAndroid;
                }
                // Format
                if (m_FormatForAndroidSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        globalSetting.m_FormatForAndroidNeedUpdate = true;
                        globalSetting.m_FormatForAndroid = m_FormatForAndroid;
                    }
                }
                else if (m_FormatForAndroidSetMode == PropertySetMode.Force)
                {
                    globalSetting.m_FormatForAndroidNeedUpdate = true;
                    globalSetting.m_FormatForAndroid = m_FormatForAndroid;
                }

                // Compressor Quality
                if (m_CompressionForAndroidSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        globalSetting.m_CompressionForAndroidNeedUpdate = true;
                        globalSetting.m_CompressionForAndroid = m_CompressionForAndroid;
                    }
                }
                else if (m_CompressionForAndroidSetMode == PropertySetMode.Force)
                {
                    globalSetting.m_CompressionForAndroidNeedUpdate = true;
                    globalSetting.m_CompressionForAndroid = m_CompressionForAndroid;
                }

                // ASTC: Only POT texture can be compressed if mip-maps are enabled.
                if (isNPOT && AssetImportPipelineUtility.TextureFormatIsASTC(m_FormatForAndroid) && tImporter.npotScale == TextureImporterNPOTScale.None)
                {
                    globalSetting.m_MipmapNeedUpdate = true;
                    globalSetting.m_Mipmap = false;
                }
            }

            // IOS Platform 
            if (iosPlatformSeting != null)
            {
                if (!iosPlatformSeting.overridden)
                {
                    globalSetting.m_IosPlatformOverriddenNeedUpdate = true;
                }

                // Max Size
                if (m_MaxSizeForIosSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        globalSetting.m_MaxSizeForIosNeedUpdate = true;
                        globalSetting.m_MaxSizeForIos = m_MaxSizeForIos;
                    }
                }
                else if (m_MaxSizeForIosSetMode == PropertySetMode.Force)
                {
                    globalSetting.m_MaxSizeForIosNeedUpdate = true;
                    globalSetting.m_MaxSizeForIos = m_MaxSizeForIos;
                }
                // Resizi Algorithm
                if (m_ResizeAlgorithmForIosSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        globalSetting.m_ResizeAlgorithmForIosNeedUpdate = true;
                        globalSetting.m_ResizeAlgorithmForIos = m_ResizeAlgorithmForIos;
                    }
                }
                else if (m_ResizeAlgorithmForIosSetMode == PropertySetMode.Force)
                {
                    globalSetting.m_ResizeAlgorithmForIosNeedUpdate = true;
                    globalSetting.m_ResizeAlgorithmForIos = m_ResizeAlgorithmForIos;
                }
                // Format
                if (m_FormatForIosSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        globalSetting.m_FormatForIosNeedUpdate = true;
                        globalSetting.m_FormatForIos = m_FormatForIos;
                    }
                }
                else if (m_FormatForIosSetMode == PropertySetMode.Force)
                {
                    globalSetting.m_FormatForIosNeedUpdate = true;
                    globalSetting.m_FormatForIos = m_FormatForIos;
                }
                // Compressor Quality
                if (m_CompressionForIosSetMode == PropertySetMode.Once)
                {
                    if (isFirstImported)
                    {
                        globalSetting.m_CompressionForIosNeedUpdate = true;
                        globalSetting.m_CompressionForIos = m_CompressionForIos;
                    }
                }
                else if (m_CompressionForIosSetMode == PropertySetMode.Force)
                {
                    globalSetting.m_CompressionForIosNeedUpdate = true;
                    globalSetting.m_CompressionForIos = m_CompressionForIos;
                }

                if (isNPOT && AssetImportPipelineUtility.TextureFormatIsASTC(m_FormatForIos) && tImporter.npotScale == TextureImporterNPOTScale.None)
                {
                    globalSetting.m_MipmapNeedUpdate = true;
                    globalSetting.m_Mipmap = false;
                }
            }
        }

        internal void ClearUpdateState()
        {
            m_TextureTypeNeedUpdate = false;
            m_SRGBNeedUpdate = false;
            m_AlphaIsTransparencyNeedUpdate = false;
            m_AlphaSourceNeedUpdate = false;
            m_ReadableNeedUpdate = false;
            m_VtOnlyNeedUpdate = false;
            m_MipmapNeedUpdate = false;
            m_IgnorePngGammaNeedUpdate = false;
            m_WrapModeNeedUpdate = false;
            m_FilterModeNeedUpdate = false;
            m_AnisoLevelNeedUpdate = false;
            m_AndroidPlatformOverriddenNeedUpdate = false;
            m_MaxSizeForAndroidNeedUpdate = false;
            m_ResizeAlgorithmForAndroidNeedUpdate = false;
            m_FormatForAndroidNeedUpdate = false;
            m_CompressionForAndroidNeedUpdate = false;
            m_IosPlatformOverriddenNeedUpdate = false;
            m_MaxSizeForIosNeedUpdate = false;
            m_ResizeAlgorithmForIosNeedUpdate = false;
            m_FormatForIosNeedUpdate = false;
            m_CompressionForIosNeedUpdate = false;
        }

        internal void DoRealPreImport(TextureImporter tImporter,List<string> packedInSpriteAtlas)
        {
            // Check Packed In SpriteAtlas
            /* Ensure that the source Texture of the Sprite is always uncompressed. While packing Sprite Atlas , 
             * pixel data is read from the source texture and if it uses any compressed format,
            /* it may result in loss of precision as it must be uncompressed first before packing. 
            /* If a Sprite is packed to a Sprite Atlas, only the Sprite Atlas Texture needs to be compressed.
            */
            var packedInSpriteAtlasFlag = packedInSpriteAtlas.Contains(tImporter.assetPath);
            // Texture Type
            if (m_TextureTypeNeedUpdate)
            {
                tImporter.textureType = m_TextureType;
            }
            // srgb
            if (m_SRGBNeedUpdate)
            {
                tImporter.sRGBTexture = m_SRGB;
            }
            // alphaSource
            if (m_AlphaSourceNeedUpdate)
            {
                tImporter.alphaSource = m_AlphaSource;
            }
            // alphaIsTransparency
            if (m_AlphaIsTransparencyNeedUpdate)
            {
                tImporter.alphaIsTransparency = m_AlphaIsTransparency;
            }
            else if (m_AlphaIsTransparencySetMode == PropertySetMode.Force)
                tImporter.alphaIsTransparency = m_AlphaIsTransparency;
            // vtOnly
            if (m_VtOnlyNeedUpdate)
            {
                tImporter.vtOnly = m_VtOnly;
            }
            // mipmapEnabled
            if (m_MipmapNeedUpdate)
            {
                tImporter.mipmapEnabled = m_Mipmap;
            }
            // ignorePngGamma
            if (m_IgnorePngGammaNeedUpdate)
            {
                tImporter.ignorePngGamma = m_IgnorePngGamma;
            }
            // wrapMode
            if (m_WrapModeNeedUpdate)
            {
                tImporter.wrapMode = m_WrapMode;
            }
            // filterMode
            if (m_FilterModeNeedUpdate)
            {
                tImporter.filterMode = m_FilterMode;
            }
            // anisoLevel
            if (m_AnisoLevelNeedUpdate)
            {
                tImporter.anisoLevel = m_AnisoLevel;
            }
            // isReadable
            if (m_ReadableNeedUpdate)
            {
                tImporter.isReadable = m_Readable;
            }
            // Android Platform 
            TextureImporterPlatformSettings androidPlatformSeting = tImporter.GetPlatformTextureSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_ANDROID);
            if (androidPlatformSeting != null)
            {
                var needUpdateAndroidSetting = false;
                if (m_AndroidPlatformOverriddenNeedUpdate)
                {
                    androidPlatformSeting.overridden = true;
                    needUpdateAndroidSetting = true;
                }

                // Max Size
                if (m_MaxSizeForAndroidNeedUpdate)
                {
                    androidPlatformSeting.maxTextureSize = m_MaxSizeForAndroid;
                    needUpdateAndroidSetting = true;
                }
                // Resizi Algorithm
                if (m_ResizeAlgorithmForAndroidNeedUpdate)
                {
                    androidPlatformSeting.resizeAlgorithm = m_ResizeAlgorithmForAndroid;
                    needUpdateAndroidSetting = true;
                }
                // Format
                if (packedInSpriteAtlasFlag)
                {
                    TextureImporterFormat targetFormat = TextureImporterFormat.RGB24;
                    if(tImporter.DoesSourceTextureHaveAlpha())
                        targetFormat = TextureImporterFormat.RGBA32;

                    if (androidPlatformSeting.format != targetFormat)
                    {
                        androidPlatformSeting.format = targetFormat;
                        needUpdateAndroidSetting = true;
                    }
                }
                else
                {
                    if (m_FormatForAndroidNeedUpdate)
                    {
                        androidPlatformSeting.format = m_FormatForAndroid;
                        needUpdateAndroidSetting = true;
                    }
                }
                // Compressor Quality
                if (m_CompressionForAndroidNeedUpdate)
                {
                    androidPlatformSeting.textureCompression = m_CompressionForAndroid;
                    needUpdateAndroidSetting = true;
                }

                if (needUpdateAndroidSetting)
                    tImporter.SetPlatformTextureSettings(androidPlatformSeting);
            }

            // IOS Platform 
            TextureImporterPlatformSettings iosPlatformSeting = tImporter.GetPlatformTextureSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_IOS);
            if (iosPlatformSeting != null)
            {
                var needUpdateIosSetting = false;

                if (m_IosPlatformOverriddenNeedUpdate)
                {
                    iosPlatformSeting.overridden = true;
                    needUpdateIosSetting = true;
                }

                // Max Size
                if (m_MaxSizeForIosNeedUpdate)
                {
                    iosPlatformSeting.maxTextureSize = m_MaxSizeForIos;
                    needUpdateIosSetting = true;
                }
                // Resizi Algorithm
                if (m_ResizeAlgorithmForIosNeedUpdate)
                {
                    iosPlatformSeting.resizeAlgorithm = m_ResizeAlgorithmForIos;
                    needUpdateIosSetting = true;
                }
                // Format
                if (packedInSpriteAtlasFlag)
                {
                    TextureImporterFormat targetFormat = TextureImporterFormat.RGB24;
                    if (tImporter.DoesSourceTextureHaveAlpha())
                        targetFormat = TextureImporterFormat.RGBA32;

                    if (iosPlatformSeting.format != targetFormat)
                    {
                        iosPlatformSeting.format = targetFormat;
                        needUpdateIosSetting = true;
                    }
                }
                else
                {
                    if (m_FormatForIosNeedUpdate)
                    {
                        iosPlatformSeting.format = m_FormatForIos;
                        needUpdateIosSetting = true;
                    }
                }
                // Compressor Quality
                if (m_CompressionForIosNeedUpdate)
                {
                    iosPlatformSeting.textureCompression = m_CompressionForIos;
                    needUpdateIosSetting = true;
                }

                if (needUpdateIosSetting)
                    tImporter.SetPlatformTextureSettings(iosPlatformSeting);
            }
        }

        #endregion
    }

    //[CreateAssetMenu(menuName = AssetImportPipelineUtility.TOOL_MENU_TEXTURE_SETTING, fileName = "TextureImportPipeline")]
    internal class TextureImportPipeline : ScriptableObject
    {
        #region Fields

        [SerializeField]
        List<TextureSetting> m_SpecialSettings;

        List<TextureSetting> m_SortedSpecialSettings;

        private static TextureSetting m_PipeFinalSetting = new TextureSetting();

        internal List<string> PackedInSpriteAtlas { get; set; }

        #endregion

        #region Local Methods

        void CollectTexturePackedInSpriteAtlas()
        {
            if (PackedInSpriteAtlas == null) PackedInSpriteAtlas = new List<string>();
            else PackedInSpriteAtlas.Clear();

            var pipelinePath = AssetDatabase.GetAssetPath(this);
            var folder = pipelinePath.Replace(AssetImportPipelineUtility.SubPipelineRelativePath + Path.GetFileName(pipelinePath), "");
            var guids = AssetDatabase.FindAssets("t:" + typeof(UnityEngine.U2D.SpriteAtlas).Name, new string[] { folder });
            foreach (var guid in guids)
            {
                var atlasPath = AssetDatabase.GUIDToAssetPath(guid);
                var parentFolder = Directory.GetParent(atlasPath);
                var allFiles = Directory.GetFiles(parentFolder.FullName, "*.*", SearchOption.AllDirectories);
                foreach (var file in allFiles)
                {
                    if (file.EndsWith(AssetImportPipelineUtility.META_FILE_EXTENSION)) continue;

                    var assetPath = file.Replace("\\", "/").Replace(Application.dataPath.Replace("\\", "/"), "Assets");
                    PackedInSpriteAtlas.Add(assetPath);
                }
            }
        }

        #endregion

        #region Internal Methods


        internal void AutoSortSettings()
        {
            m_SortedSpecialSettings = new List<TextureSetting>(m_SpecialSettings);
            m_SortedSpecialSettings.Sort((x, y) => string.CompareOrdinal(((int)x.m_RuleType).ToString(), ((int)y.m_RuleType).ToString()));

            CollectTexturePackedInSpriteAtlas();
        }

        internal void DoPreImport(TextureImporter tImporter)
        {
            if (m_SortedSpecialSettings != null && m_SortedSpecialSettings.Count > 0)
            {
                // unity:在OnPreprocessTexture多次设置TextureImporter属性值，会导致一些属性无法修改，只能一次设置完毕
                m_PipeFinalSetting.ClearUpdateState();

                foreach (var setting in m_SortedSpecialSettings)
                {
                    if (setting.CheckRule(tImporter.assetPath))
                        setting.DoPreImport(tImporter, m_PipeFinalSetting);
                }

                m_PipeFinalSetting.DoRealPreImport(tImporter, PackedInSpriteAtlas);
            }
        }

        #endregion
    }

    [CustomEditor(typeof(TextureImportPipeline))]
    internal class TextureImportPipelineInspector : Editor
    {
        #region Fields

        class Styles
        {
            internal static readonly GUIContent SetMode = new GUIContent("", "Once-资源首次导入工程修改，Force资源每次重新导入时修改。");
            internal static readonly GUIContent[] Platforms = new GUIContent[] {
                new GUIContent("Android",""),
                new GUIContent("IOS","")
            };

            internal static readonly GUIContent SpecialSettings = new GUIContent("自定义配置:");

            internal static readonly GUIContent RuleType = new GUIContent("规则类型", "1.None:通用规则;\n2.AssetPath:资源路径;\n3.AssetName:资源名;\n4.AssetParentFullFolder:全目录;\n5.AssetParentPartFolder:部分目录;");
            internal static readonly GUIContent AssetPath = new GUIContent("资源路径", "格式：Assets/XXX/YYY/test.png");
            internal static readonly GUIContent AssetName = new GUIContent("资源名", "格式: _ASTC_4x4 或 _RW");
            internal static readonly GUIContent AssetParentFullFolder = new GUIContent("全目录", "格式：Assets/XXX/YYY/");
            internal static readonly GUIContent AssetParentPartFolder = new GUIContent("部分目录", "格式：/XXX/YYY/ ");
            internal static readonly GUIContent TextureType = new GUIContent("Texture Type", "");
            internal static readonly GUIContent SRGB = new GUIContent("sRGB(Color Texture)", "Texture content is stored in gamma space. Non-HDR color textures should enable this flag (except if used for IMGUI).");
            internal static readonly GUIContent AlphaSource = new GUIContent("Alpha Source", "How is the alpha generated for the imported texture.");
            internal static readonly GUIContent AlphaIsTransparency = new GUIContent("Alpha Is Transparency", "If the alpha channel of your texture represents transparency, enable this property to dilate the color channels of visible texels into fully transparent areas. This effectively adds padding around transparent areas that prevents filtering artifacts from forming on their edges. Unity does not support this property for HDR textures. \n\nThis property makes the color data of invisible texels undefined. Disable this property to preserve invisible texels' original color data.");
            internal static readonly GUIContent VtOnly = new GUIContent("Virtual Texture Only", "Texture is optimized for use as a virtual texture and can only be used as a virtual texture.");
            internal static readonly GUIContent Readable = new GUIContent("Read/Write", "Enable to be able to access the raw pixel data from code.");
            internal static readonly GUIContent Mipmap = new GUIContent("Generate Mipmaps", "Create progressively smaller versions of the texture, for reduced texture shimmering and better GPU performance when the texture is viewed at a distance.");
            internal static readonly GUIContent IgnorePngGamma = new GUIContent("Ignore PNG Gamma", "Ignore the Gamma attribute value in PNG files.");
            internal static readonly GUIContent WrapMode = new GUIContent("Wrap Mode", "");
            internal static readonly GUIContent FilterMode = new GUIContent("Filter Mode", "");
            internal static readonly GUIContent AnisoLevel = new GUIContent("Aniso Level", "");
            internal static readonly GUIContent MaxSizeForAndroid = new GUIContent("Max Size", "");
            internal static readonly GUIContent ResizeAlgorithmForAndroid = new GUIContent("Resize Algorithm", "");
            internal static readonly GUIContent FormatForAndroid = new GUIContent("Format", "Android平台纹理压缩格式。");
            internal static readonly GUIContent CompressionForAndroid = new GUIContent("Compressor Quality", "");
            internal static readonly GUIContent MaxSizeForIos = new GUIContent("Max Size", "");
            internal static readonly GUIContent ResizeAlgorithmForIos = new GUIContent("Resize Algorithm", "");
            internal static readonly GUIContent FormatForIos = new GUIContent("Format", "IOS平台纹理压缩格式。");
            internal static readonly GUIContent CompressionForIos = new GUIContent("Compressor Quality", "");
        }

        // Base Property
        private SerializedProperty m_SpecialSettings;

        // Children Property
        private SerializedProperty m_RuleType;

        private SerializedProperty m_AssetParentFolderListForPartFolderRule;
        private SerializedProperty m_AssetParentFolderListForFullFolderRule;
        private SerializedProperty m_AssetNameListForAssetNameRule;
        private SerializedProperty m_AssetPathListForAssetPathRule;

        private SerializedProperty m_TextureType;
        private SerializedProperty m_SRGB;
        private SerializedProperty m_AlphaSource;
        private SerializedProperty m_AlphaIsTransparency;
        private SerializedProperty m_VtOnly;
        private SerializedProperty m_Readable;
        private SerializedProperty m_Mipmap;
        private SerializedProperty m_IgnorePngGamma;
        private SerializedProperty m_WrapMode;
        private SerializedProperty m_FilterMode;
        private SerializedProperty m_AnisoLevel;
        private SerializedProperty m_MaxSizeForAndroid;
        private SerializedProperty m_ResizeAlgorithmForAndroid;
        private SerializedProperty m_FormatForAndroid;
        private SerializedProperty m_CompressionForAndroid;
        private SerializedProperty m_MaxSizeForIos;
        private SerializedProperty m_ResizeAlgorithmForIos;
        private SerializedProperty m_FormatForIos;
        private SerializedProperty m_CompressionForIos;

        private SerializedProperty m_TextureTypeSetMode;
        private SerializedProperty m_SRGBSetMode;
        private SerializedProperty m_AlphaSourceSetMode;
        private SerializedProperty m_AlphaIsTransparencySetMode;
        private SerializedProperty m_VtOnlySetMode;
        private SerializedProperty m_ReadableSetMode;
        private SerializedProperty m_MipmapSetMode;
        private SerializedProperty m_IgnorePngGammaSetMode;
        private SerializedProperty m_WrapModeSetMode;
        private SerializedProperty m_FilterModeSetMode;
        private SerializedProperty m_AnisoLevelSetMode;
        private SerializedProperty m_MaxSizeForAndroidSetMode;
        private SerializedProperty m_ResizeAlgorithmForAndroidSetMode;
        private SerializedProperty m_FormatForAndroidSetMode;
        private SerializedProperty m_CompressionForAndroidSetMode;
        private SerializedProperty m_MaxSizeForIosSetMode;
        private SerializedProperty m_ResizeAlgorithmForIosSetMode;
        private SerializedProperty m_FormatForIosSetMode;
        private SerializedProperty m_CompressionForIosSetMode;

        private int m_PlatformIndex = 0;
        private List<bool> m_SpecialChildSettingFoldouts = new List<bool>();

        private static Dictionary<int, string> m_RuleTypeValueToDisplayMapping = null;

        #endregion

        #region Editor Methods

        void OnEnable()
        {
            m_SpecialSettings = serializedObject.FindProperty("m_SpecialSettings");

            BuildRuleTypeValueToDisplayMapping();
        }

        public override void OnInspectorGUI()
        {
            if (m_SpecialChildSettingFoldouts.Count == 0)
            {
                for (int i = 0; i < m_SpecialSettings.arraySize; i++)
                {
                    m_SpecialChildSettingFoldouts.Add(false);
                }
            }
            serializedObject.Update();

            EditorGUILayout.LabelField(Styles.SpecialSettings);

            int deleteIndex = -1;
            for (int i = 0; i < m_SpecialSettings.arraySize; i++)
            {
                var setting = m_SpecialSettings.GetArrayElementAtIndex(i);
                var ruleTypeIntValue = setting.FindPropertyRelative("m_RuleType").enumValueFlag;
                var ruleTypeDisplayName = m_RuleTypeValueToDisplayMapping[ruleTypeIntValue];
                string title = "";
                if (ruleTypeIntValue == (int)ESpecialRuleType._1_AssetParentPartFolder)
                {
                    var assetParentFolderListForPartFolderRule = setting.FindPropertyRelative("m_AssetParentFolderListForPartFolderRule");
                    if (assetParentFolderListForPartFolderRule.arraySize > 0)
                    {
                        title = assetParentFolderListForPartFolderRule.GetArrayElementAtIndex(0).stringValue;
                    }
                }
                if (ruleTypeIntValue == (int)ESpecialRuleType._2_AssetParentFullFolder)
                {
                    var assetParentFolderListForFullFolderRule = setting.FindPropertyRelative("m_AssetParentFolderListForFullFolderRule");
                    if (assetParentFolderListForFullFolderRule.arraySize > 0)
                    {
                        title = assetParentFolderListForFullFolderRule.GetArrayElementAtIndex(0).stringValue;
                    }
                }
                if (ruleTypeIntValue == (int)ESpecialRuleType._3_AssetName)
                {
                    var assetNameListForAssetNameRule = setting.FindPropertyRelative("m_AssetNameListForAssetNameRule");
                    if (assetNameListForAssetNameRule.arraySize > 0)
                    {
                        title = assetNameListForAssetNameRule.GetArrayElementAtIndex(0).stringValue;
                    }
                }
                if (ruleTypeIntValue == (int)ESpecialRuleType._4_AssetPath)
                {
                    var assetPathListForAssetPathRule = setting.FindPropertyRelative("m_AssetPathListForAssetPathRule");
                    if (assetPathListForAssetPathRule.arraySize > 0)
                    {
                        title = Path.GetFileName(assetPathListForAssetPathRule.GetArrayElementAtIndex(0).stringValue);
                    }
                }

                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();

                m_SpecialChildSettingFoldouts[i] = EditorGUILayout.Foldout(m_SpecialChildSettingFoldouts[i], ruleTypeDisplayName + (string.IsNullOrEmpty(title) ? "" : ("[" + title + "]")));

                GUI.color = Color.yellow;

                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    deleteIndex = i;
                }

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                if (m_SpecialChildSettingFoldouts[i])
                {
                    DoTextureSettingUILayout(setting);
                }

                EditorGUI.indentLevel--;

                GUILayout.Space(6);
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            GUI.color = Color.red;

            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                AddNewItem(m_SpecialSettings);
                m_SpecialChildSettingFoldouts.Add(false);
            }

            GUI.color = Color.white;

            EditorGUILayout.EndHorizontal();

            if (deleteIndex >= 0 && deleteIndex < m_SpecialSettings.arraySize)
            {
                m_SpecialSettings.DeleteArrayElementAtIndex(deleteIndex);
                m_SpecialChildSettingFoldouts.RemoveAt(deleteIndex);
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Local Methods

        void AddNewItem(SerializedProperty ownerProperty)
        {
            ownerProperty.arraySize++;
            serializedObject.ApplyModifiedProperties();
            var newItemProperty = ownerProperty.GetArrayElementAtIndex(ownerProperty.arraySize - 1);
            var newValue = TextureSetting.GetDefaultSetting();
            newItemProperty.boxedValue = newValue;
            serializedObject.ApplyModifiedProperties();
        }

        void BuildRuleTypeValueToDisplayMapping()
        {
            m_RuleTypeValueToDisplayMapping = new Dictionary<int, string>();
            var enumType = typeof(ESpecialRuleType);
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                PipelineEnumAttribute attribute = field.GetCustomAttributes(typeof(PipelineEnumAttribute), false).FirstOrDefault() as PipelineEnumAttribute;
                if (attribute != null)
                {
                    m_RuleTypeValueToDisplayMapping[(int)field.GetValue(null)] = attribute.DisplayName;
                }
            }
        }

        void DoTexturePropertyGroupUILayout(SerializedProperty data, GUIContent dataGUIContent, SerializedProperty setMode)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(data, dataGUIContent);

            EditorGUILayout.PropertyField(setMode, Styles.SetMode, GUILayout.Width(100));

            EditorGUILayout.EndHorizontal();
        }

        void DoTextureSettingUILayout(SerializedProperty parent)
        {
            // Get Child Properties
            m_RuleType = parent.FindPropertyRelative("m_RuleType");
            m_AssetParentFolderListForPartFolderRule = parent.FindPropertyRelative("m_AssetParentFolderListForPartFolderRule");
            m_AssetParentFolderListForFullFolderRule = parent.FindPropertyRelative("m_AssetParentFolderListForFullFolderRule");
            m_AssetNameListForAssetNameRule = parent.FindPropertyRelative("m_AssetNameListForAssetNameRule");
            m_AssetPathListForAssetPathRule = parent.FindPropertyRelative("m_AssetPathListForAssetPathRule");
            m_TextureType = parent.FindPropertyRelative("m_TextureType");
            m_SRGB = parent.FindPropertyRelative("m_SRGB");
            m_AlphaSource = parent.FindPropertyRelative("m_AlphaSource");
            m_AlphaIsTransparency = parent.FindPropertyRelative("m_AlphaIsTransparency");
            m_VtOnly = parent.FindPropertyRelative("m_VtOnly");
            m_Readable = parent.FindPropertyRelative("m_Readable");
            m_Mipmap = parent.FindPropertyRelative("m_Mipmap");
            m_IgnorePngGamma = parent.FindPropertyRelative("m_IgnorePngGamma");
            m_WrapMode = parent.FindPropertyRelative("m_WrapMode");
            m_FilterMode = parent.FindPropertyRelative("m_FilterMode");
            m_AnisoLevel = parent.FindPropertyRelative("m_AnisoLevel");
            m_MaxSizeForAndroid = parent.FindPropertyRelative("m_MaxSizeForAndroid");
            m_ResizeAlgorithmForAndroid = parent.FindPropertyRelative("m_ResizeAlgorithmForAndroid");
            m_FormatForAndroid = parent.FindPropertyRelative("m_FormatForAndroid");
            m_CompressionForAndroid = parent.FindPropertyRelative("m_CompressionForAndroid");
            m_MaxSizeForIos = parent.FindPropertyRelative("m_MaxSizeForIos");
            m_ResizeAlgorithmForIos = parent.FindPropertyRelative("m_ResizeAlgorithmForIos");
            m_FormatForIos = parent.FindPropertyRelative("m_FormatForIos");
            m_CompressionForIos = parent.FindPropertyRelative("m_CompressionForIos");

            m_TextureTypeSetMode = parent.FindPropertyRelative("m_TextureTypeSetMode");
            m_SRGBSetMode = parent.FindPropertyRelative("m_SRGBSetMode");
            m_AlphaSourceSetMode = parent.FindPropertyRelative("m_AlphaSourceSetMode");
            m_AlphaIsTransparencySetMode = parent.FindPropertyRelative("m_AlphaIsTransparencySetMode");
            m_VtOnlySetMode = parent.FindPropertyRelative("m_VtOnlySetMode");
            m_ReadableSetMode = parent.FindPropertyRelative("m_ReadableSetMode");
            m_MipmapSetMode = parent.FindPropertyRelative("m_MipmapSetMode");
            m_IgnorePngGammaSetMode = parent.FindPropertyRelative("m_IgnorePngGammaSetMode");
            m_WrapModeSetMode = parent.FindPropertyRelative("m_WrapModeSetMode");
            m_FilterModeSetMode = parent.FindPropertyRelative("m_FilterModeSetMode");
            m_AnisoLevelSetMode = parent.FindPropertyRelative("m_AnisoLevelSetMode");
            m_MaxSizeForAndroidSetMode = parent.FindPropertyRelative("m_MaxSizeForAndroidSetMode");
            m_ResizeAlgorithmForAndroidSetMode = parent.FindPropertyRelative("m_ResizeAlgorithmForAndroidSetMode");
            m_FormatForAndroidSetMode = parent.FindPropertyRelative("m_FormatForAndroidSetMode");
            m_CompressionForAndroidSetMode = parent.FindPropertyRelative("m_CompressionForAndroidSetMode");
            m_MaxSizeForIosSetMode = parent.FindPropertyRelative("m_MaxSizeForIosSetMode");
            m_ResizeAlgorithmForIosSetMode = parent.FindPropertyRelative("m_ResizeAlgorithmForIosSetMode");
            m_FormatForIosSetMode = parent.FindPropertyRelative("m_FormatForIosSetMode");
            m_CompressionForIosSetMode = parent.FindPropertyRelative("m_CompressionForIosSetMode");

            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical("Box");

            float spaceHeight = 4;

            var ruleTypeIntValue = m_RuleType.enumValueFlag;
            var ruleTypeDisplayName = m_RuleTypeValueToDisplayMapping[ruleTypeIntValue];
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Styles.RuleType);
            if (GUILayout.Button(ruleTypeDisplayName))
            {
                GenericMenu window = new GenericMenu();
                foreach (var data in m_RuleTypeValueToDisplayMapping)
                {
                    var value = data.Key;
                    var displayName = data.Value;
                    window.AddItem(new GUIContent(displayName == ruleTypeDisplayName ? (displayName + "     √") : displayName), false, (System.Object value) =>
                    {
                        m_RuleType.enumValueFlag = (int)value;
                        serializedObject.ApplyModifiedProperties();
                    }, value);

                    window.AddSeparator("");
                }

                window.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            EditorGUI.indentLevel++;

            var ruleType = (ESpecialRuleType)m_RuleType.enumValueFlag;
            switch (ruleType)
            {
                case ESpecialRuleType._1_AssetParentPartFolder:
                    EditorGUILayout.PropertyField(m_AssetParentFolderListForPartFolderRule, Styles.AssetParentPartFolder);
                    break;
                case ESpecialRuleType._2_AssetParentFullFolder:
                    EditorGUILayout.PropertyField(m_AssetParentFolderListForFullFolderRule, Styles.AssetParentFullFolder);
                    break;
                case ESpecialRuleType._3_AssetName:
                    EditorGUILayout.PropertyField(m_AssetNameListForAssetNameRule, Styles.AssetName);
                    break;
                case ESpecialRuleType._4_AssetPath:
                    EditorGUILayout.PropertyField(m_AssetPathListForAssetPathRule, Styles.AssetPath);
                    break;
            }

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_TextureType, Styles.TextureType, m_TextureTypeSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_SRGB, Styles.SRGB, m_SRGBSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_AlphaSource, Styles.AlphaSource, m_AlphaSourceSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_AlphaIsTransparency, Styles.AlphaIsTransparency, m_AlphaIsTransparencySetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_VtOnly, Styles.VtOnly, m_VtOnlySetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_Readable, Styles.Readable, m_ReadableSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_Mipmap, Styles.Mipmap, m_MipmapSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_IgnorePngGamma, Styles.IgnorePngGamma, m_IgnorePngGammaSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_WrapMode, Styles.WrapMode, m_WrapModeSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_FilterMode, Styles.FilterMode, m_FilterModeSetMode);

            GUILayout.Space(spaceHeight);

            DoTexturePropertyGroupUILayout(m_AnisoLevel, Styles.AnisoLevel, m_AnisoLevelSetMode);

            GUILayout.Space(spaceHeight);

            m_PlatformIndex = GUILayout.Toolbar(m_PlatformIndex, Styles.Platforms);

            EditorGUI.indentLevel++;

            switch (m_PlatformIndex)
            {
                case 0:

                    DoTexturePropertyGroupUILayout(m_MaxSizeForAndroid, Styles.MaxSizeForAndroid, m_MaxSizeForAndroidSetMode);

                    DoTexturePropertyGroupUILayout(m_ResizeAlgorithmForAndroid, Styles.ResizeAlgorithmForAndroid, m_ResizeAlgorithmForAndroidSetMode);

                    DoTexturePropertyGroupUILayout(m_FormatForAndroid, Styles.FormatForAndroid, m_FormatForAndroidSetMode);

                    DoTexturePropertyGroupUILayout(m_CompressionForAndroid, Styles.CompressionForAndroid, m_CompressionForAndroidSetMode);

                    break;
                case 1:

                    DoTexturePropertyGroupUILayout(m_MaxSizeForIos, Styles.MaxSizeForIos, m_MaxSizeForIosSetMode);

                    DoTexturePropertyGroupUILayout(m_ResizeAlgorithmForIos, Styles.ResizeAlgorithmForIos, m_ResizeAlgorithmForIosSetMode);

                    DoTexturePropertyGroupUILayout(m_FormatForIos, Styles.FormatForIos, m_FormatForIosSetMode);

                    DoTexturePropertyGroupUILayout(m_CompressionForIos, Styles.CompressionForIos, m_CompressionForIosSetMode);

                    break;
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }

        #endregion
    }
}

