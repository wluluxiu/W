using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System;

public class TextureSync 
{

    enum  JnityTextureFormat
    {
        NONE = -1,

        BGRA8888 = 1, // Color with alpha texture format, 8-bits per channel. Not support in editor.
        RGBA8888,     // Color with alpha texture format, 8-bits per channel.

        // https://forum.unity.com/threads/serious-ios-texture-memory-allocation-bug.122784/
        // https://stackoverflow.com/questions/9438009/ios-textures-taking-33-extra-memory
        // https://developer.apple.com/metal/Metal-Feature-Set-Tables.pdf
        // Be careful use of RGB888 pixel format. iOS and Android platform only support POT bit per pixel.
        // So 24 bit RGB888 will always convert to 32 bit in these two platform.
        // And also Metal is not support RGB888, so most of iOS device that support Metal will allocate an extra memory to
        // do pixel conversion between 24 and 32 bit, this cause a large memory consumption within our test.
        RGB888, // Color texture format, 8-bits per channel.

        RGB565, // A 16 bit color texture format.
        A8,     // Alpha-only texture format.

        ABANDON0, // I8, removed in 50909 because OpenGL is not support(only compatible) in latest version.
        ABANDON1, // I8A8, removed in 50909 because OpenGL is not support(only compatible) in latest version.

        RGBA4444 = 8, // A 16 bits/pixel texture format. Texture stores color with an alpha channel.
        RGBA5551,     // A 16 bits/pixel texture format. Texture stores color with an alpha channel. Not support in editor.

        // This is PVRTC1, PVRTC2 is not support very well in some iOS device.
        // http://cdn.imgtec.com/sdk-documentation/PVRTC%20Specification%20and%20User%20Guide.pdf
        PVRTC4,  // PowerVR (iOS) 4 bits/pixel compressed color texture format.
        PVRTC4A, // PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format.
        PVRTC2,  // PowerVR (iOS) 2 bits/pixel compressed color texture format.
        PVRTC2A, // PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format.

        ETC, // ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
        // https://www.khronos.org/opengl/wiki/S3_Texture_Compression
        S3TC_DXT1, // Compressed color texture format. Only support no-alpha for DXT1
        S3TC_DXT3,
        S3TC_DXT5,    // Compressed color with alpha channel texture format.
        ETC2_RGB8,    // ETC2 RGB (GLES3.0)
        ETC2_RGB8_A1, // ETC2 RGB with 1-bit Alpha (GLES3.0)
        ETC2_RGBA8,   // ETC2 RGB with 8-bit Alpha (GLES3.0)

        // depth and stencil format
        DEPTH_STENCIL,
        SHADOW_DEPTH,
        DEPTH24,

        // float format
        FLOAT_RGBA,    // R16F G16F B16F A16F
        R32G32B32A32F, // in unreal:it's PF_A32B32G32R32F,we just correct its sequence
        R16F,
        R32F,
        R16G16F,
        R11G11B10F,

        // ASTC LDR format with different block
        RGBA_ASTC_LDR_4x4,
        RGBA_ASTC_LDR_5x5,
        RGBA_ASTC_LDR_6x6,
        RGBA_ASTC_LDR_8x8,
        RGBA_ASTC_LDR_10x10,
        RGBA_ASTC_LDR_12x12,

        // ASTC HDR format with different block
        RGBA_ASTC_HDR_4x4,
        RGBA_ASTC_HDR_5x5,
        RGBA_ASTC_HDR_6x6,
        RGBA_ASTC_HDR_8x8,
        RGBA_ASTC_HDR_10x10,
        RGBA_ASTC_HDR_12x12,

        COUNT,
    };


    static TextureImporterType getTextureImporterType(int textureType)
    {
        switch (textureType)
        {
            case 0:
                return TextureImporterType.Default;
            case 1:
                return TextureImporterType.NormalMap;
            case 2:
                return TextureImporterType.Sprite;
            case 3:
                return TextureImporterType.Lightmap;
            default:
                return TextureImporterType.Shadowmask;

        }
    }

    static TextureImporterFormat ConvertTextureFormat(JnityTextureFormat jFormat)
    {
        switch (jFormat)
        {
            case JnityTextureFormat.RGBA8888:
                return TextureImporterFormat.RGBA32;
            case JnityTextureFormat.RGBA4444:
                return TextureImporterFormat.RGBA16;
            case JnityTextureFormat.A8:
                return TextureImporterFormat.Alpha8;
            case JnityTextureFormat.RGB888:
                return TextureImporterFormat.RGB24;
            case JnityTextureFormat.ETC:
                return TextureImporterFormat.ETC_RGB4;
            case JnityTextureFormat.ETC2_RGBA8:
                return TextureImporterFormat.ETC2_RGBA8;
            case JnityTextureFormat.ETC2_RGB8:
                return TextureImporterFormat.ETC2_RGB4;
            case JnityTextureFormat.RGBA_ASTC_LDR_4x4:
                return TextureImporterFormat.ASTC_4x4;
            case JnityTextureFormat.RGBA_ASTC_LDR_5x5:
                return TextureImporterFormat.ASTC_5x5;
            case JnityTextureFormat.RGBA_ASTC_LDR_6x6:
                return TextureImporterFormat.ASTC_6x6;
            case JnityTextureFormat.RGBA_ASTC_LDR_8x8:
                return TextureImporterFormat.ASTC_8x8;
            case JnityTextureFormat.RGBA_ASTC_LDR_10x10:
                return TextureImporterFormat.ASTC_10x10;
            case JnityTextureFormat.RGBA_ASTC_LDR_12x12:
                return TextureImporterFormat.ASTC_12x12;
            case JnityTextureFormat.PVRTC2:
                return TextureImporterFormat.PVRTC_RGB2;
            case JnityTextureFormat.PVRTC4:
                return TextureImporterFormat.PVRTC_RGB4;
            case JnityTextureFormat.PVRTC2A:
                return TextureImporterFormat.PVRTC_RGBA2;
            case JnityTextureFormat.PVRTC4A:
                return TextureImporterFormat.PVRTC_RGBA4;
            default:
                return TextureImporterFormat.Automatic;
        }
    }

    static Texture2D FlipTextureVertical(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height);
        for (int y = 0; y < original.height; y++)
        {
            for (int x = 0; x < original.width; x++)
            {
                Color color = original.GetPixel(x, original.height - y - 1);
                flipped.SetPixel(x, y, color);
            }
        }
        flipped.Apply();
        return flipped;
    }

    public static TextureImporterShape getTextureShape(string path, string assetPath)
    {
		string text = FileUtil.ReadTexTFile(path);
        var jsonStr = JsonConvert.DeserializeObject<Dictionary<object, object>>(text);
        string importer_str = jsonStr["TextureImporter"].ToString();
        var importer_attribute = JsonConvert.DeserializeObject<Dictionary<object, object>>(importer_str);
        int textureShape = int.Parse(importer_attribute["textureShape"].ToString());
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
        importer.textureShape = (TextureImporterShape)(textureShape + 1);
        return importer.textureShape;
    }

    public static void SyncMetaFile(string unityPath, string assetPath, string path)
    {
        string text = FileUtil.ReadTexTFile(path);
        var jsonStr = JsonConvert.DeserializeObject<Dictionary<object, object>>(text);
        string importer_str = jsonStr["TextureImporter"].ToString();
        var importer_attribute = JsonConvert.DeserializeObject<Dictionary<object, object>>(importer_str);
        int textureType = int.Parse(importer_attribute["textureType"].ToString());
        int textureShape = int.Parse(importer_attribute["textureShape"].ToString());
        bool createFromGrayScale = bool.Parse(importer_attribute["createFromGrayScale"].ToString());
        int alphaSource = int.Parse(importer_attribute["alphaSource"].ToString());
        bool sRGB = bool.Parse(importer_attribute["sRGB"].ToString());
        bool alphaIsTransparency = true;// bool.Parse(importer_attribute["alphaIsTransparency"].ToString());
        int filterMode = int.Parse(importer_attribute["filterMode"].ToString());
        int wrapMode = int.Parse(importer_attribute["wrapMode"].ToString());
        int wrapModeU = int.Parse(importer_attribute["wrapModeU"].ToString());
        int wrapModeV = int.Parse(importer_attribute["wrapModeV"].ToString());
        int anisoLevel = int.Parse(importer_attribute["anisoLevel"].ToString());
        int maxSize = int.Parse(importer_attribute["maxSize"].ToString());
        int format = int.Parse(importer_attribute["format"].ToString());
        int spriteMode = int.Parse(importer_attribute["spriteMode"].ToString());
        int spritePixelsToUnits = int.Parse(importer_attribute["spritePixelsToUnits"].ToString());
        bool readWriteEnabled = bool.Parse(importer_attribute["readWriteEnabled"].ToString());
        bool streamingMipMaps = bool.Parse(importer_attribute["streamingMipMaps"].ToString());
        bool isGenerateMipMaps = bool.Parse(importer_attribute["isGenerateMipMaps"].ToString());
        int mipmapPriority = int.Parse(importer_attribute["mipmapPriority"].ToString());
        int mipmapFiltering = int.Parse(importer_attribute["mipmapFiltering"].ToString());
        bool borderMipMaps = bool.Parse(importer_attribute["borderMipMaps"].ToString());
        bool mipmapsPreserveCoverage = bool.Parse(importer_attribute["mipmapsPreserveCoverage"].ToString());
        bool fadeoutMipMaps = bool.Parse(importer_attribute["fadeoutMipMaps"].ToString());
        int fadeRangeBegin = int.Parse(importer_attribute["fadeRangeBegin"].ToString());
        int fadeRangeEnd = int.Parse(importer_attribute["fadeRangeEnd"].ToString());
        bool flipY = bool.Parse(importer_attribute["flipY"].ToString());
        //if (flipY)
        //    texture = FlipTextureVertical(texture);

        //FileUtil.SaveTextureToPath(unityPath, texture);
        //AssetDatabase.Refresh();

        int spriteMeshType = int.Parse(importer_attribute["spriteMeshType"].ToString());
        int spriteExtrude = int.Parse(importer_attribute["spriteExtrude"].ToString());
        int alignment = int.Parse(importer_attribute["alignment"].ToString());
        var spritePivotVec = JsonConvert.DeserializeObject<Dictionary<object, object>>(importer_attribute["spritePivot"].ToString());
        var spriteBorderVec = JsonConvert.DeserializeObject<Dictionary<object, object>>(importer_attribute["spriteBorder"].ToString());

        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
        importer.textureType = getTextureImporterType(textureType);
        importer.textureShape = (TextureImporterShape)(textureShape + 1);
        importer.grayscaleToAlpha = createFromGrayScale;
        importer.alphaSource = (TextureImporterAlphaSource)alphaSource;
        importer.sRGBTexture = sRGB;
        importer.alphaIsTransparency = alphaIsTransparency;
        importer.filterMode = (FilterMode)filterMode;
        if(wrapMode >=4 || wrapModeU>=4 || wrapModeV >= 4)
        {
            ReportSystem.OutputLog($"[{path}] jnity 纹理导入设置中的 wrapMode or wrapModeU or wrapModeV 值超出范围!");
        }
        importer.wrapMode = (TextureWrapMode)wrapMode;
        importer.anisoLevel = anisoLevel;
        importer.maxTextureSize = maxSize;
        importer.textureFormat = ConvertTextureFormat((JnityTextureFormat)format);
        importer.spriteImportMode = (SpriteImportMode)spriteMode + 1;
        importer.spritePixelsPerUnit = spritePixelsToUnits;
        importer.isReadable =  readWriteEnabled;
      
        importer.streamingMipmaps = streamingMipMaps;
        importer.mipmapEnabled = isGenerateMipMaps;
        importer.streamingMipmapsPriority = mipmapPriority;
        importer.mipmapFilter = (TextureImporterMipFilter)mipmapFiltering;
        importer.borderMipmap = borderMipMaps;
        importer.mipMapsPreserveCoverage = mipmapsPreserveCoverage;
        importer.fadeout = fadeoutMipMaps;
        importer.mipmapFadeDistanceEnd = fadeRangeEnd;
        importer.mipmapFadeDistanceStart = fadeRangeBegin;

        TextureImporterSettings textureSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(textureSettings);
        textureSettings.spriteMeshType = (SpriteMeshType)spriteMeshType;
        textureSettings.spriteExtrude = (uint)spriteExtrude;
        textureSettings.spriteAlignment = alignment;
        textureSettings.spritePivot = new Vector2(float.Parse(spritePivotVec["x"].ToString()), float.Parse(spritePivotVec["y"].ToString()));
        textureSettings.spriteBorder = new Vector4(float.Parse(spriteBorderVec["x"].ToString()), float.Parse(spriteBorderVec["y"].ToString()),
                                                   float.Parse(spriteBorderVec["z"].ToString()), float.Parse(spriteBorderVec["w"].ToString()));
        importer.SetTextureSettings(textureSettings);

        TextureImporterPlatformSettings winSettings = importer.GetPlatformTextureSettings("Standalone");
        int maxSizeWin = int.Parse(importer_attribute["maxSizeWin"].ToString());
        int resizeAlgorithmWin = int.Parse(importer_attribute["resizeAlgorithmWin"].ToString());
        int formatWin = int.Parse(importer_attribute["formatWin"].ToString());
        bool overrideDefaultWin = bool.Parse(importer_attribute["overrideDefaultWin"].ToString());
        winSettings.maxTextureSize = maxSizeWin;
        winSettings.resizeAlgorithm = (TextureResizeAlgorithm)resizeAlgorithmWin;
        winSettings.format = ConvertTextureFormat((JnityTextureFormat)formatWin);
        winSettings.overridden = overrideDefaultWin;
        importer.SetPlatformTextureSettings(winSettings);

        TextureImporterPlatformSettings androidSettings = importer.GetPlatformTextureSettings("Android");
        int maxSizeAndroid = int.Parse(importer_attribute["maxSizeAndroid"].ToString());
        int resizeAlgorithmAndroid = int.Parse(importer_attribute["resizeAlgorithmAndroid"].ToString());
        int formatAndroid = int.Parse(importer_attribute["formatAndroid"].ToString());
        bool overrideDefaultAndroid = bool.Parse(importer_attribute["overrideDefaultAndroid"].ToString());
        androidSettings.maxTextureSize = maxSizeAndroid;
        androidSettings.resizeAlgorithm = (TextureResizeAlgorithm)resizeAlgorithmAndroid;
        androidSettings.format = ConvertTextureFormat((JnityTextureFormat)formatAndroid); 
        androidSettings.overridden = overrideDefaultAndroid;
        importer.SetPlatformTextureSettings(androidSettings);

        TextureImporterPlatformSettings iosSettings = importer.GetPlatformTextureSettings("iPhone");
        int maxSizeiOS = int.Parse(importer_attribute["maxSizeiOS"].ToString());
        int resizeAlgorithmiOS = int.Parse(importer_attribute["resizeAlgorithmiOS"].ToString());
        int formatiOS = int.Parse(importer_attribute["formatiOS"].ToString());
        bool overrideDefaultiOS = bool.Parse(importer_attribute["overrideDefaultiOS"].ToString());
        iosSettings.maxTextureSize = maxSizeiOS;
        iosSettings.resizeAlgorithm = (TextureResizeAlgorithm)resizeAlgorithmiOS;
        iosSettings.format = ConvertTextureFormat((JnityTextureFormat)formatiOS);
        iosSettings.overridden = overrideDefaultiOS;
        importer.SetPlatformTextureSettings(iosSettings);

        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
    }
}
