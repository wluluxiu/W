using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;
using UnityEditor.U2D;

namespace jj.TATools.Editor
{
    internal enum ESpriteAtlasType
    { 
        Master = 0,
        Variant = 1
    }

    /// <summary>
    /// 检测项目：
    /// 1.Keyword
    /// </summary>
    internal class SpriteAtlasRecorder : BaseRecorder
    {
        #region Fields

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

        #endregion

        #region Override Methods

        internal override void Record(string assetPath, EAssetType assetType)
        {
            base.Record(assetPath, assetType);

            var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);

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

            spriteAtlas = null;
        }

        /// <summary>
        /// BaseFormat|SpriteAtlasType|IncludeInBuild|
        /// AllowRotation|TightPacking|AlphaDilation|Padding|
        /// RW|GenerateMipmaps|Srgb|FilterMode|AnisoLevel|
        /// OverrideIOS|MaxSizeIOS|FormatIOS|OverrideAndroid|MaxSizeAndroid|FormatAndroid
        /// </summary>
        /// <returns></returns>
        internal override string GetOutputStr()
        {
            var baseOutputStr = base.GetOutputStr();

            string spiltStr = CHAR_SPLIT_FIRST_FLAG.ToString();

            return baseOutputStr + spiltStr +
                    this.m_SpriteAtlasType + spiltStr +
                    this.m_IncludeInBuild + spiltStr +
                    this.m_AllowRotation + spiltStr +
                    this.m_TightPacking + spiltStr +
                    this.m_AlphaDilation + spiltStr +
                    this.m_Padding + spiltStr +
                    this.m_RW + spiltStr +
                    this.m_GenerateMipmaps + spiltStr +
                    this.m_Srgb + spiltStr +
                    this.m_FilterMode + spiltStr +
                    this.m_AnisoLevel + spiltStr +
                    this.m_OverrideIOS + spiltStr +
                    this.m_MaxSizeIOS + spiltStr +
                    this.m_FormatIOS + spiltStr +
                    this.m_OverrideAndroid + spiltStr +
                    this.m_MaxSizeAndroid + spiltStr +
                    this.m_FormatAndroid;
        }

        internal override void ParseStrLine(string stringLine)
        {
            base.ParseStrLine(stringLine);

            if (base.m_SourceDataArr.Length > 7 && !string.IsNullOrEmpty(base.m_SourceDataArr[7]))
                this.m_SpriteAtlasType = int.Parse(base.m_SourceDataArr[7]);

            if (base.m_SourceDataArr.Length > 8 && !string.IsNullOrEmpty(base.m_SourceDataArr[8]))
                this.m_IncludeInBuild = int.Parse(base.m_SourceDataArr[8]);

            if (base.m_SourceDataArr.Length > 9 && !string.IsNullOrEmpty(base.m_SourceDataArr[9]))
                this.m_AllowRotation = int.Parse(base.m_SourceDataArr[9]);

            if (base.m_SourceDataArr.Length > 10 && !string.IsNullOrEmpty(base.m_SourceDataArr[10]))
                this.m_TightPacking = int.Parse(base.m_SourceDataArr[10]);

            if (base.m_SourceDataArr.Length > 11 && !string.IsNullOrEmpty(base.m_SourceDataArr[11]))
                this.m_AlphaDilation = int.Parse(base.m_SourceDataArr[11]);

            if (base.m_SourceDataArr.Length > 12 && !string.IsNullOrEmpty(base.m_SourceDataArr[12]))
                this.m_Padding = int.Parse(base.m_SourceDataArr[12]);

            if (base.m_SourceDataArr.Length > 13 && !string.IsNullOrEmpty(base.m_SourceDataArr[13]))
                this.m_RW = int.Parse(base.m_SourceDataArr[13]);

            if (base.m_SourceDataArr.Length > 14 && !string.IsNullOrEmpty(base.m_SourceDataArr[14]))
                this.m_GenerateMipmaps = int.Parse(base.m_SourceDataArr[14]);

            if (base.m_SourceDataArr.Length > 15 && !string.IsNullOrEmpty(base.m_SourceDataArr[15]))
                this.m_Srgb = int.Parse(base.m_SourceDataArr[15]);

            if (base.m_SourceDataArr.Length > 16 && !string.IsNullOrEmpty(base.m_SourceDataArr[16]))
                this.m_FilterMode = int.Parse(base.m_SourceDataArr[16]);

            if (base.m_SourceDataArr.Length > 17 && !string.IsNullOrEmpty(base.m_SourceDataArr[17]))
                this.m_AnisoLevel = int.Parse(base.m_SourceDataArr[17]);

            if (base.m_SourceDataArr.Length > 18 && !string.IsNullOrEmpty(base.m_SourceDataArr[18]))
                this.m_OverrideIOS = int.Parse(base.m_SourceDataArr[18]);

            if (base.m_SourceDataArr.Length > 19 && !string.IsNullOrEmpty(base.m_SourceDataArr[19]))
                this.m_MaxSizeIOS = int.Parse(base.m_SourceDataArr[19]);

            if (base.m_SourceDataArr.Length > 20 && !string.IsNullOrEmpty(base.m_SourceDataArr[20]))
                this.m_FormatIOS = int.Parse(base.m_SourceDataArr[20]);

            if (base.m_SourceDataArr.Length > 21 && !string.IsNullOrEmpty(base.m_SourceDataArr[21]))
                this.m_OverrideAndroid = int.Parse(base.m_SourceDataArr[21]);

            if (base.m_SourceDataArr.Length > 22 && !string.IsNullOrEmpty(base.m_SourceDataArr[22]))
                this.m_MaxSizeAndroid = int.Parse(base.m_SourceDataArr[22]);

            if (base.m_SourceDataArr.Length > 23 && !string.IsNullOrEmpty(base.m_SourceDataArr[23]))
                this.m_FormatAndroid = int.Parse(base.m_SourceDataArr[23]);
        }

        #endregion
    }
}
