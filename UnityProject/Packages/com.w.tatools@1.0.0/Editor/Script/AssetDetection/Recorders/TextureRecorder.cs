using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    //
    // 摘要:
    //     Select this to set basic parameters depending on the purpose of your texture.
    internal enum ETextureImporterType
    {
        //
        // 摘要:
        //     This is the most common setting used for all the textures in general.
        Image = int.MinValue,
        HDRI = -9,
        Advanced = -5,
        Cubemap = -3,
        Reflection = -3,
        Bump = -1,
        //
        // 摘要:
        //     This is the most common setting used for all the textures in general.
        Default = 0,
        //
        // 摘要:
        //     Select this to turn the color channels into a format suitable for real-time normal
        //     mapping.
        NormalMap = 1,
        //
        // 摘要:
        //     Use this if your texture is going to be used on any HUD/GUI Controls.
        GUI = 2,
        //
        // 摘要:
        //     This sets up your texture with the basic parameters used for the Cookies of your
        //     lights.
        Cookie = 4,
        //
        // 摘要:
        //     This sets up your texture with the parameters used by the lightmap.
        Lightmap = 6,
        //
        // 摘要:
        //     Use this if your texture is going to be used as a cursor.
        Cursor = 7,
        //
        // 摘要:
        //     Select this if you will be using your texture for Sprite graphics.
        Sprite = 8,
        //
        // 摘要:
        //     Use this for texture containing a single channel.
        SingleChannel = 10,
        //
        // 摘要:
        //     Use this for textures that contain shadowmask data.
        Shadowmask = 11,
        //
        // 摘要:
        //     Use this for textures that contain directional lightmap data.
        DirectionalLightmap = 12
    }

    //
    // 摘要:
    //     The shape of the imported texture.
    internal enum ETextureImporterShape
    {
        //
        // 摘要:
        //     Import the texture as a 2D texture (default).
        Texture2D = 1,
        //
        // 摘要:
        //     Import the texture as a Cubemap.
        TextureCube = 2,
        //
        // 摘要:
        //     Import the texture as a 2D Array texture.
        Texture2DArray = 4,
        //
        // 摘要:
        //     Import the texture as a 3D texture.
        Texture3D = 8
    }

    //
    // 摘要:
    //     Filtering mode for textures. Corresponds to the settings in a.
    internal enum EFilterMode
    {
        //
        // 摘要:
        //     Point filtering - texture pixels become blocky up close.
        Point = 0,
        //
        // 摘要:
        //     Bilinear filtering - texture samples are averaged.
        Bilinear = 1,
        //
        // 摘要:
        //     Trilinear filtering - texture samples are averaged and also blended between mipmap
        //     levels.
        Trilinear = 2
    }

    //
    // 摘要:
    //     Imported texture format for TextureImporter.
    internal enum ETextureImporterFormat
    {
        //
        // 摘要:
        //     Choose a compressed HDR format automatically.
        AutomaticCompressedHDR = -7,
        //
        // 摘要:
        //     Choose an HDR format automatically.
        AutomaticHDR = -6,
        //
        // 摘要:
        //     Choose a crunched format automatically.
        AutomaticCrunched = -5,
        //
        // 摘要:
        //     Choose a Truecolor format automatically.
        AutomaticTruecolor = -3,
        //
        // 摘要:
        //     Choose a 16 bit format automatically.
        Automatic16bit = -2,
        //
        // 摘要:
        //     Choose texture format automatically based on the texture parameters.
        Automatic = -1,
        //
        // 摘要:
        //     Choose a compressed format automatically.
        AutomaticCompressed = -1,
        //
        // 摘要:
        //     TextureFormat.Alpha8 texture format.
        Alpha8 = 1,
        //
        // 摘要:
        //     TextureFormat.ARGB4444 texture format.
        ARGB16 = 2,
        //
        // 摘要:
        //     TextureFormat.RGB24 texture format.
        RGB24 = 3,
        //
        // 摘要:
        //     TextureFormat.RGBA32 texture format.
        RGBA32 = 4,
        //
        // 摘要:
        //     TextureFormat.ARGB32 texture format.
        ARGB32 = 5,
        //
        // 摘要:
        //     TextureFormat.RGB565 texture format.
        RGB16 = 7,
        //
        // 摘要:
        //     TextureFormat.R16 texture format.
        R16 = 9,
        //
        // 摘要:
        //     TextureFormat.DXT1 (BC1) compressed texture format.
        DXT1 = 10,
        //
        // 摘要:
        //     TextureFormat.DXT5 (BC3) compressed texture format.
        DXT5 = 12,
        //
        // 摘要:
        //     TextureFormat.RGBA4444 texture format.
        RGBA16 = 13,
        //
        // 摘要:
        //     TextureFormat.RHalf half-precision floating point texture format.
        RHalf = 15,
        //
        // 摘要:
        //     TextureFormat.RGHalf half-precision floating point texture format.
        RGHalf = 16,
        //
        // 摘要:
        //     TextureFormat.RGBAHalf half-precision floating point texture format.
        RGBAHalf = 17,
        //
        // 摘要:
        //     TextureFormat.RFloat floating point texture format.
        RFloat = 18,
        //
        // 摘要:
        //     TextureFormat.RGFloat floating point texture format.
        RGFloat = 19,
        //
        // 摘要:
        //     TextureFormat.RGBAFloat floating point RGBA texture format.
        RGBAFloat = 20,
        //
        // 摘要:
        //     TextureFormat.RGB9e5Float packed unsigned floating point texture format with
        //     shared exponent.
        RGB9E5 = 22,
        //
        // 摘要:
        //     TextureFormat.BC6H compressed HDR texture format.
        BC6H = 24,
        //
        // 摘要:
        //     TextureFormat.BC7 compressed texture format.
        BC7 = 25,
        //
        // 摘要:
        //     TextureFormat.BC4 compressed texture format.
        BC4 = 26,
        //
        // 摘要:
        //     TextureFormat.BC5 compressed texture format.
        BC5 = 27,
        //
        // 摘要:
        //     DXT1 (BC1) compressed texture format using Crunch compression for smaller storage
        //     sizes.
        DXT1Crunched = 28,
        //
        // 摘要:
        //     DXT5 (BC3) compressed texture format using Crunch compression for smaller storage
        //     sizes.
        DXT5Crunched = 29,
        //
        // 摘要:
        //     PowerVR/iOS TextureFormat.PVRTC_RGB2 compressed texture format.
        PVRTC_RGB2 = 30,
        //
        // 摘要:
        //     PowerVR/iOS TextureFormat.PVRTC_RGBA2 compressed texture format.
        PVRTC_RGBA2 = 31,
        //
        // 摘要:
        //     PowerVR/iOS TextureFormat.PVRTC_RGB4 compressed texture format.
        PVRTC_RGB4 = 32,
        //
        // 摘要:
        //     PowerVR/iOS TextureFormat.PVRTC_RGBA4 compressed texture format.
        PVRTC_RGBA4 = 33,
        //
        // 摘要:
        //     ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
        ETC_RGB4 = 34,
        ATC_RGB4 = 35,
        ATC_RGBA8 = 36,
        //
        // 摘要:
        //     ETC2EAC compressed 4 bits pixel unsigned R texture format.
        EAC_R = 41,
        //
        // 摘要:
        //     ETC2EAC compressed 4 bits pixel signed R texture format.
        EAC_R_SIGNED = 42,
        //
        // 摘要:
        //     ETC2EAC compressed 8 bits pixel unsigned RG texture format.
        EAC_RG = 43,
        //
        // 摘要:
        //     ETC2EAC compressed 4 bits pixel signed RG texture format.
        EAC_RG_SIGNED = 44,
        //
        // 摘要:
        //     ETC2 compressed 4 bits / pixel RGB texture format.
        ETC2_RGB4 = 45,
        //
        // 摘要:
        //     ETC2 compressed 4 bits / pixel RGB + 1-bit alpha texture format.
        ETC2_RGB4_PUNCHTHROUGH_ALPHA = 46,
        //
        // 摘要:
        //     ETC2 compressed 8 bits / pixel RGBA texture format.
        ETC2_RGBA8 = 47,
        //
        // 摘要:
        //     ASTC compressed RGB(A) texture format, 4x4 block size.
        ASTC_4x4 = 48,
        //
        // 摘要:
        //     ASTC compressed RGB texture format, 4x4 block size.
        ASTC_RGB_4x4 = 48,
        //
        // 摘要:
        //     ASTC compressed RGB(A) texture format, 5x5 block size.
        ASTC_5x5 = 49,
        //
        // 摘要:
        //     ASTC compressed RGB texture format, 5x5 block size.
        ASTC_RGB_5x5 = 49,
        //
        // 摘要:
        //     ASTC compressed RGB(A) texture format, 6x6 block size.
        ASTC_6x6 = 50,
        //
        // 摘要:
        //     ASTC compressed RGB texture format, 6x6 block size.
        ASTC_RGB_6x6 = 50,
        //
        // 摘要:
        //     ASTC compressed RGB(A) texture format, 8x8 block size.
        ASTC_8x8 = 51,
        //
        // 摘要:
        //     ASTC compressed RGB texture format, 8x8 block size.
        ASTC_RGB_8x8 = 51,
        //
        // 摘要:
        //     ASTC compressed RGB(A) texture format, 10x10 block size.
        ASTC_10x10 = 52,
        //
        // 摘要:
        //     ASTC compressed RGB texture format, 10x10 block size.
        ASTC_RGB_10x10 = 52,
        //
        // 摘要:
        //     ASTC compressed RGB(A) texture format, 12x12 block size.
        ASTC_12x12 = 53,
        //
        // 摘要:
        //     ASTC compressed RGB texture format, 12x12 block size.
        ASTC_RGB_12x12 = 53,
        //
        // 摘要:
        //     ASTC compressed RGBA texture format, 4x4 block size.
        ASTC_RGBA_4x4 = 54,
        //
        // 摘要:
        //     ASTC compressed RGBA texture format, 5x5 block size.
        ASTC_RGBA_5x5 = 55,
        //
        // 摘要:
        //     ASTC compressed RGBA texture format, 6x6 block size.
        ASTC_RGBA_6x6 = 56,
        //
        // 摘要:
        //     ASTC compressed RGBA texture format, 8x8 block size.
        ASTC_RGBA_8x8 = 57,
        //
        // 摘要:
        //     ASTC compressed RGBA texture format, 10x10 block size.
        ASTC_RGBA_10x10 = 58,
        //
        // 摘要:
        //     ASTC compressed RGBA texture format, 12x12 block size.
        ASTC_RGBA_12x12 = 59,
        //
        // 摘要:
        //     ETC (Nintendo 3DS) 4 bits/pixel compressed RGB texture format.
        ETC_RGB4_3DS = 60,
        //
        // 摘要:
        //     ETC (Nintendo 3DS) 8 bits/pixel compressed RGBA texture format.
        ETC_RGBA8_3DS = 61,
        //
        // 摘要:
        //     TextureFormat.RG16 texture format.
        RG16 = 62,
        //
        // 摘要:
        //     TextureFormat.R8 texture format.
        R8 = 63,
        //
        // 摘要:
        //     ETC_RGB4 compressed texture format using Crunch compression for smaller storage
        //     sizes.
        ETC_RGB4Crunched = 64,
        //
        // 摘要:
        //     ETC2_RGBA8 compressed color with alpha channel texture format using Crunch compression
        //     for smaller storage sizes.
        ETC2_RGBA8Crunched = 65,
        //
        // 摘要:
        //     ASTC compressed RGB(A) HDR texture format, 4x4 block size.
        ASTC_HDR_4x4 = 66,
        //
        // 摘要:
        //     ASTC compressed RGB(A) HDR texture format, 5x5 block size.
        ASTC_HDR_5x5 = 67,
        //
        // 摘要:
        //     ASTC compressed RGB(A) HDR texture format, 6x6 block size.
        ASTC_HDR_6x6 = 68,
        //
        // 摘要:
        //     ASTC compressed RGB(A) HDR texture format, 8x8 block size.
        ASTC_HDR_8x8 = 69,
        //
        // 摘要:
        //     ASTC compressed RGB(A) HDR texture format, 10x10 block size.
        ASTC_HDR_10x10 = 70,
        //
        // 摘要:
        //     ASTC compressed RGB(A) HDR texture format, 12x12 block size.
        ASTC_HDR_12x12 = 71,
        //
        // 摘要:
        //     TextureFormat.RG32 texture format.
        RG32 = 72,
        //
        // 摘要:
        //     TextureFormat.RGB48 texture format.
        RGB48 = 73,
        //
        // 摘要:
        //     TextureFormat.RGBA64 texture format.
        RGBA64 = 74
    }

    //
    // 摘要:
    //     Wrap mode for textures.
    internal enum ETextureWrapMode
    {
        //
        // 摘要:
        //     Tiles the texture, creating a repeating pattern.
        Repeat = 0,
        //
        // 摘要:
        //     Clamps the texture to the last pixel at the edge.
        Clamp = 1,
        //
        // 摘要:
        //     Tiles the texture, creating a repeating pattern by mirroring it at every integer
        //     boundary.
        Mirror = 2,
        //
        // 摘要:
        //     Mirrors the texture once, then clamps to edge pixels.
        MirrorOnce = 3
    }

    /// <summary>
    /// 检测项目：
    /// 1.TextureType
    /// 2.TextureShape
    /// 3.Readable
    /// 4.Mipmap
    /// 5.StreamingMipmaps
    /// 6.FilterMode
    /// 7.WrapMode
    /// 8.AnisoLevel
    /// 9.OverrideIOS
    /// 10.MaxSizeIOS
    /// 11.FormatIOS
    /// 12.OverrideAndroid
    /// 13.MaxSizeAndroid
    /// 14.FormatAndroid
    /// 15.Width
    /// 16.Height
    /// 17.Npot
    /// 18.MemorySize
    /// </summary>
    internal class TextureRecorder : BaseRecorder
    {
        #region Fields

        internal static string TEXTURE_FORMAT_THRESHOLD = "ASTC_";
        internal static int TEXTURE_POT_THRESHOLD = 1;
        internal static int TEXTURE_MAX_RESOLUTION_THRESHOLD = 2048;
        internal  static int TEXTURE_FILTERMODE_THRESHOLD = 2;
        internal static int TEXTURE_RW_THRESHOLD = 1;
        internal static int TEXTURE_ANISOLEVEL_THRESHOLD = 1;

        internal int m_TextureType;
        internal int m_TextureShape;
        internal int m_Readable;
        internal int m_Mipmap;
        internal int m_StreamingMipmaps;
        internal int m_FilterMode;
        internal int m_WrapMode;
        internal int m_AnisoLevel;
        internal int m_OverrideIOS;
        internal int m_MaxSizeIOS;
        internal int m_FormatIOS;
        internal int m_OverrideAndroid;
        internal int m_MaxSizeAndroid;
        internal int m_FormatAndroid;
        internal int m_Width;
        internal int m_Height;
        internal int m_Npot;
        internal long m_MemorySize;
        internal long m_MemorySizeOld;

        #endregion

        #region Internal Methods

        internal static void UpdateDataFromConfig()
        {
            TEXTURE_FORMAT_THRESHOLD = AppConfigHelper.TEXTURE_FORMAT_THRESHOLD;
            TEXTURE_POT_THRESHOLD = AppConfigHelper.TEXTURE_POT_THRESHOLD;
            TEXTURE_MAX_RESOLUTION_THRESHOLD = AppConfigHelper.TEXTURE_MAX_RESOLUTION_THRESHOLD;
            TEXTURE_FILTERMODE_THRESHOLD = AppConfigHelper.TEXTURE_FILTERMODE_THRESHOLD;
            TEXTURE_RW_THRESHOLD = AppConfigHelper.TEXTURE_RW_THRESHOLD;
            TEXTURE_ANISOLEVEL_THRESHOLD = AppConfigHelper.TEXTURE_ANISOLEVEL_THRESHOLD;
        }

        internal bool AnisoLevelInvalid()
        {
            return this.m_AnisoLevel > TEXTURE_ANISOLEVEL_THRESHOLD;
        }

        internal bool RWInvalid()
        {
            return this.m_Readable == TEXTURE_RW_THRESHOLD;
        }

        internal bool FilterModeInvalid()
        {
            return this.m_FilterMode == TEXTURE_FILTERMODE_THRESHOLD;
        }

        internal bool ResolutionInvalid()
        {
            return this.m_Width > TEXTURE_MAX_RESOLUTION_THRESHOLD || this.m_Height > TEXTURE_MAX_RESOLUTION_THRESHOLD;
        }

        internal bool POTInvalid()
        {
            return this.m_Npot == TEXTURE_POT_THRESHOLD;
        }

        internal bool FormatInvalid()
        {
            ETextureImporterFormat androidFormat = (ETextureImporterFormat)this.m_FormatAndroid;
            string androidFormatName = androidFormat.ToString();
            ETextureImporterFormat iosFormat = (ETextureImporterFormat)this.m_FormatIOS;
            string iosFormatName = iosFormat.ToString();
            return !androidFormatName.Contains(TEXTURE_FORMAT_THRESHOLD) || (m_OverrideIOS == 1 && !iosFormatName.Contains(TEXTURE_FORMAT_THRESHOLD));
        }

        #endregion

        #region Override Methods

        internal override void Record(string assetPath, EAssetType assetType)
        {
            base.Record(assetPath, assetType);

            var texture = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);

            this.m_Width = texture.width;
            this.m_Height = texture.height;
            this.m_Npot = AssetDetectionUtility.IsNonPowerOfTwo(texture) ? 1 : 0;
            this.m_MemorySize = AssetDetectionUtility.GetTextureFileSize(texture);

            var textureImporter = base.m_BaseImporter as TextureImporter;
            if (textureImporter != null)
            {
                this.m_TextureType = (int)textureImporter.textureType;
                this.m_TextureShape = (int)textureImporter.textureShape;
                this.m_Readable = textureImporter.isReadable ? 1 : 0;
                this.m_Mipmap = textureImporter.mipmapEnabled ? 1 : 0;
                this.m_StreamingMipmaps = textureImporter.streamingMipmaps ? 1 : 0;
                this.m_FilterMode = (int)textureImporter.filterMode;
                this.m_WrapMode = (int)textureImporter.wrapMode;
                this.m_AnisoLevel = textureImporter.anisoLevel;
                var iosTS = textureImporter.GetPlatformTextureSettings("iPhone");
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
                var androidTS = textureImporter.GetPlatformTextureSettings("Android");
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

                textureImporter = null;
            }
            else
            {
                this.m_Readable = texture.isReadable ? 1 : 0;
                this.m_Mipmap = texture.mipmapCount > 1 ? 1 : 0;
                this.m_FilterMode = (int)texture.filterMode;
                this.m_WrapMode = (int)texture.wrapMode;
                this.m_AnisoLevel = texture.anisoLevel;
                this.m_MaxSizeIOS = -1;
                this.m_MaxSizeAndroid = -1;
                SerializedObject so = new SerializedObject(texture);
                var tfProp = so.FindProperty("m_TextureFormat");
                if (tfProp != null)
                {
                    this.m_FormatIOS = tfProp.intValue;
                    this.m_FormatAndroid = tfProp.intValue;
                }
            }

            texture = null;
        }

        /// <summary>
        /// BaseFormat|TextureType|TextureShape|Readable|Mipmap|StreamingMipmaps|FilterMode|WrapMode|AnisoLevel|OverrideIOS|MaxSizeIOS|FormatIOS|OverrideAndroid|MaxSizeAndroid|FormatAndroid|Width|Height|Npot|MemorySize
        /// </summary>
        /// <returns></returns>
        internal override string GetOutputStr()
        {
            var baseOutputStr = base.GetOutputStr();

            string splitStr = CHAR_SPLIT_FIRST_FLAG.ToString();

            return baseOutputStr + splitStr +
                    this.m_TextureType + splitStr +
                    this.m_TextureShape + splitStr +
                    this.m_Readable + splitStr +
                    this.m_Mipmap + splitStr +
                    this.m_StreamingMipmaps + splitStr +
                    this.m_FilterMode + splitStr +
                    this.m_WrapMode + splitStr +
                    this.m_AnisoLevel + splitStr +
                    this.m_OverrideIOS + splitStr +
                    this.m_MaxSizeIOS + splitStr +
                    this.m_FormatIOS + splitStr +
                    this.m_OverrideAndroid + splitStr +
                    this.m_MaxSizeAndroid + splitStr +
                    this.m_FormatAndroid + splitStr +
                    this.m_Width + splitStr +
                    this.m_Height + splitStr +
                    this.m_Npot + splitStr +
                    this.m_MemorySize;
        }

        internal override void ParseStrLine(string stringLine)
        {
            base.ParseStrLine(stringLine);

            if (base.m_SourceDataArr.Length > 7 && !string.IsNullOrEmpty(base.m_SourceDataArr[7]))
                this.m_TextureType = int.Parse(base.m_SourceDataArr[7]);
            if (base.m_SourceDataArr.Length > 8 && !string.IsNullOrEmpty(base.m_SourceDataArr[8]))
                this.m_TextureShape = int.Parse(base.m_SourceDataArr[8]);
            if (base.m_SourceDataArr.Length > 9 && !string.IsNullOrEmpty(base.m_SourceDataArr[9]))
                this.m_Readable = int.Parse(base.m_SourceDataArr[9]);
            if (base.m_SourceDataArr.Length > 10 && !string.IsNullOrEmpty(base.m_SourceDataArr[10]))
                this.m_Mipmap = int.Parse(base.m_SourceDataArr[10]);
            if (base.m_SourceDataArr.Length > 11 && !string.IsNullOrEmpty(base.m_SourceDataArr[11]))
                this.m_StreamingMipmaps = int.Parse(base.m_SourceDataArr[11]);
            if (base.m_SourceDataArr.Length > 12 && !string.IsNullOrEmpty(base.m_SourceDataArr[12]))
                this.m_FilterMode = int.Parse(base.m_SourceDataArr[12]);
            if (base.m_SourceDataArr.Length > 13 && !string.IsNullOrEmpty(base.m_SourceDataArr[13]))
                this.m_WrapMode = int.Parse(base.m_SourceDataArr[13]);
            if (base.m_SourceDataArr.Length > 14 && !string.IsNullOrEmpty(base.m_SourceDataArr[14]))
                this.m_AnisoLevel = int.Parse(base.m_SourceDataArr[14]);
            if (base.m_SourceDataArr.Length > 15 && !string.IsNullOrEmpty(base.m_SourceDataArr[15]))
                this.m_OverrideIOS = int.Parse(base.m_SourceDataArr[15]);
            if (base.m_SourceDataArr.Length > 16 && !string.IsNullOrEmpty(base.m_SourceDataArr[16]))
                this.m_MaxSizeIOS = int.Parse(base.m_SourceDataArr[16]);
            if (base.m_SourceDataArr.Length > 17 && !string.IsNullOrEmpty(base.m_SourceDataArr[17]))
                this.m_FormatIOS = int.Parse(base.m_SourceDataArr[17]);
            if (base.m_SourceDataArr.Length > 18 && !string.IsNullOrEmpty(base.m_SourceDataArr[18]))
                this.m_OverrideAndroid = int.Parse(base.m_SourceDataArr[18]);
            if (base.m_SourceDataArr.Length > 19 && !string.IsNullOrEmpty(base.m_SourceDataArr[19]))
                this.m_MaxSizeAndroid = int.Parse(base.m_SourceDataArr[19]);
            if (base.m_SourceDataArr.Length > 20 && !string.IsNullOrEmpty(base.m_SourceDataArr[20]))
                this.m_FormatAndroid = int.Parse(base.m_SourceDataArr[20]);
            if (base.m_SourceDataArr.Length > 21 && !string.IsNullOrEmpty(base.m_SourceDataArr[21]))
                this.m_Width = int.Parse(base.m_SourceDataArr[21]);
            if (base.m_SourceDataArr.Length > 22 && !string.IsNullOrEmpty(base.m_SourceDataArr[22]))
                this.m_Height = int.Parse(base.m_SourceDataArr[22]);
            if (base.m_SourceDataArr.Length > 23 && !string.IsNullOrEmpty(base.m_SourceDataArr[23]))
                this.m_Npot = int.Parse(base.m_SourceDataArr[23]);
            if (base.m_SourceDataArr.Length > 24 && !string.IsNullOrEmpty(base.m_SourceDataArr[24]))
                this.m_MemorySize = long.Parse(base.m_SourceDataArr[24]);

        }

        internal override void Release()
        {
            base.Release();


        }

        #endregion
    }
}