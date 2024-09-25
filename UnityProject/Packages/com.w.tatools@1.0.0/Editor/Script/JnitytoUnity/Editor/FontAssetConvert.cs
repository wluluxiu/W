using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;
using UnityEditor;
public class FontAssetConvert 
{
    public static TMP_FontAsset GetFontAsset(Dictionary<object, object> fontAssetDict, Transform owner)
    {
        string uuid = fontAssetDict["uuid"].ToString();
        long localId = long.Parse(fontAssetDict["localId"].ToString());

        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        string jnity_meta_path;
        string jnity_fontAsset_path;
        string assetPath;
        string unity_fontAsset_path;
        if (resConfig.resFiles.ContainsKey(uuid))
        {
            jnity_fontAsset_path = resConfig.resFiles[uuid].j_path;
            jnity_meta_path = resConfig.resFiles[uuid].j_metaPath;
            assetPath = FileUtil.GetAssetPath(jnity_fontAsset_path);
            assetPath = assetPath.Replace(".fontasset", ".asset");
            unity_fontAsset_path = Application.dataPath + assetPath.Replace("Assets", "");
            unity_fontAsset_path = unity_fontAsset_path.Replace(".fontasset", ".asset");
        }
        else
        {
            jnity_fontAsset_path = JnityBuildInRes.buildInResMap[uuid].path;
            jnity_meta_path = JnityBuildInRes.buildInResMap[uuid].path + ".metax";
            assetPath = Jnity2Unity.unityRelativeAssetPath + JnityBuildInRes.buildInResMap[uuid].idmap_path;
            assetPath = assetPath.Replace(".fontasset", ".asset");
            unity_fontAsset_path = Jnity2Unity.unityAssetPath + JnityBuildInRes.buildInResMap[uuid].idmap_path;
            unity_fontAsset_path = unity_fontAsset_path.Replace(".fontasset", ".asset");
        }
       
        FileInfo fileInfo = new FileInfo(unity_fontAsset_path);
        TMP_FontAsset fontAsset;
        //if (!fileInfo.Exists)
        //{
        fontAsset = CreateFontAsset(jnity_fontAsset_path, owner, fileInfo, assetPath);
         
        //}
        //else
        //{
        //    fontAsset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(TMP_FontAsset)) as TMP_FontAsset;
        //}

        return fontAsset;
    }

    static TMP_FontAsset CreateFontAsset(string jnity_fontAsset_path, Transform owner, FileInfo fileInfo, string assetPath)
    {
        string text = FileUtil.ReadTexTFile(jnity_fontAsset_path);
        var jsonStr = JsonConvert.DeserializeObject<List<object>>(text);
        Dictionary<object, object> matAssetDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonStr[0].ToString());
        var fontMat_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(matAssetDict["Material"].ToString());
        Material fontMat = null;
        MatSync.SyncMat(fontMat_Dict, owner.name, ref fontMat);
        
        Dictionary<object, object>  fontAssetDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonStr[1].ToString());
        var fontAssetTMP_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(fontAssetDict["FontAssetTMP"].ToString());
        var sourceFont_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(fontAssetTMP_Dict["_sourceFontFile"].ToString());
        Font systemFont = GetSystemFont(sourceFont_Dict);

        AtlasPopulationMode atlasPopulationMode = (AtlasPopulationMode)int.Parse(fontAssetTMP_Dict["_atlasPopulationMode"].ToString());
        var faceInfo_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(fontAssetTMP_Dict["_faceInfo"].ToString());
        FaceInfo faceInfo = new FaceInfo();
        faceInfo.ascentLine = float.Parse(faceInfo_Dict["ascentLine"].ToString());
        faceInfo.baseline = float.Parse(faceInfo_Dict["baseline"].ToString());
        faceInfo.capLine = float.Parse(faceInfo_Dict["capLine"].ToString());
        faceInfo.descentLine = float.Parse(faceInfo_Dict["descentLine"].ToString());
        faceInfo.familyName = faceInfo_Dict["familyName"].ToString();
        faceInfo.lineHeight = float.Parse(faceInfo_Dict["lineHeight"].ToString());
        faceInfo.meanLine = float.Parse(faceInfo_Dict["meanLine"].ToString());
        faceInfo.pointSize = int.Parse(faceInfo_Dict["pointSize"].ToString());
        faceInfo.scale = float.Parse(faceInfo_Dict["scale"].ToString());
        faceInfo.strikethroughOffset = float.Parse(faceInfo_Dict["strikethroughOffset"].ToString());
        faceInfo.strikethroughThickness = float.Parse(faceInfo_Dict["strikethroughThickness"].ToString());
        faceInfo.styleName = faceInfo_Dict["styleName"].ToString();
        faceInfo.subscriptOffset = float.Parse(faceInfo_Dict["subscriptOffset"].ToString());
        faceInfo.subscriptSize = float.Parse(faceInfo_Dict["subscriptSize"].ToString());
        faceInfo.superscriptOffset = float.Parse(faceInfo_Dict["superscriptOffset"].ToString());
        faceInfo.superscriptSize = float.Parse(faceInfo_Dict["superscriptSize"].ToString());
        faceInfo.tabWidth = float.Parse(faceInfo_Dict["tabWidth"].ToString());
        faceInfo.underlineOffset = float.Parse(faceInfo_Dict["underlineOffset"].ToString());
        faceInfo.underlineThickness = float.Parse(faceInfo_Dict["underlineThickness"].ToString());

        FontAssetCreationSettings creationSettings = new FontAssetCreationSettings();
        var creationSett_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(fontAssetTMP_Dict["_creationSettings"].ToString());
        creationSettings.sourceFontFileName = creationSett_Dict["sourceFontFileName"].ToString();
        creationSettings.pointSizeSamplingMode = int.Parse(creationSett_Dict["pointSizeSamplingMode"].ToString());
        creationSettings.pointSize = int.Parse(creationSett_Dict["pointSize"].ToString());
        creationSettings.padding = int.Parse(creationSett_Dict["padding"].ToString());
        creationSettings.packingMode = int.Parse(creationSett_Dict["packingMode"].ToString());
        creationSettings.atlasWidth = int.Parse(creationSett_Dict["atlasWidth"].ToString());
        creationSettings.atlasHeight = int.Parse(creationSett_Dict["atlasHeight"].ToString());
        creationSettings.characterSetSelectionMode = int.Parse(creationSett_Dict["characterSetSelectionMode"].ToString());
        creationSettings.characterSequence = creationSett_Dict["characterSequence"].ToString();
        creationSettings.fontStyle = int.Parse(creationSett_Dict["fontStyle"].ToString());
        creationSettings.fontStyleModifier = int.Parse(creationSett_Dict["fontStyleModifier"].ToString());
        creationSettings.renderMode = int.Parse(creationSett_Dict["renderMode"].ToString());
        creationSettings.includeFontFeatures = bool.Parse(creationSett_Dict["includeFontFeatures"].ToString());

        GlyphRenderMode atlasRenderMode = ConvertAtlasRenderMode(int.Parse(fontAssetTMP_Dict["_atlasRenderMode"].ToString()));
        int atalasPadding = int.Parse(fontAssetTMP_Dict["_atlasPadding"].ToString());
        //TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(systemFont, faceInfo.pointSize, atalasPadding, atlasRenderMode,
        //    creationSettings.atlasWidth, creationSettings.atlasHeight, atlasPopulationMode);
        TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(systemFont, faceInfo.pointSize, atalasPadding, atlasRenderMode,
          256, 256, atlasPopulationMode);
        string dir = fileInfo.Directory.FullName;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        if (fontAsset)
            AssetDatabase.CreateAsset(fontAsset, assetPath);

        //TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(systemFont);
        fontAsset.isMultiAtlasTexturesEnabled = false;
        fontAsset.material = fontMat;
        fontAsset.faceInfo = faceInfo;
        fontAsset.creationSettings = creationSettings;
        fontAsset.name = fontAssetTMP_Dict["_name"].ToString();
        fontAsset.normalStyle = float.Parse(fontAssetTMP_Dict["_normalStyle"].ToString());
        fontAsset.normalSpacingOffset = float.Parse(fontAssetTMP_Dict["_normalSpacingOffset"].ToString());
        fontAsset.boldStyle = float.Parse(fontAssetTMP_Dict["_boldStyle"].ToString());
        fontAsset.boldSpacing = float.Parse(fontAssetTMP_Dict["_boldSpacing"].ToString());
        fontAsset.italicStyle = byte.Parse(fontAssetTMP_Dict["_italicStyle"].ToString());
        fontAsset.tabSize = byte.Parse(fontAssetTMP_Dict["_tabSize"].ToString());
        // fontAsset.characterTable = int.Parse(creationSett_Dict["_missingGlyphCharacter"].ToString());
        //var tableList = JsonConvert.DeserializeObject<List<object>>(fontAssetTMP_Dict["_characterTable"].ToString());
        //List<TMP_Character> character_table = new List<TMP_Character>();
        //for (int k = 0; k < tableList.Count; k++)
        //{
        //    Glyph glyph = new Glyph();
        //    var character_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(tableList[k].ToString());
        //    glyph.index = uint.Parse(character_dict["glyphIndex"].ToString());
        //    glyph.scale = float.Parse(character_dict["scale"].ToString());
        //    uint unicode = uint.Parse(character_dict["unicode"].ToString());
        //    TMP_Character tmp_char = new TMP_Character(unicode, glyph);
        //   // TextElementType elementType = (TextElementType)(uint.Parse(character_dict["elementType"].ToString()) + 1);
        //   // ReflectHelper.ModifyFieldsValue<TMP_Character, TextElementType>(tmp_char, "m_ElementType", elementType);
        //    //character_table.Add(tmp_char);
        //    fontAsset.TryAddCharacters(tmp_char.ToString());
        //}
        //ReflectHelper.ModifyFieldsValue<TMP_FontAsset, TMP_Character>(fontAsset, "characterTable", null, character_table, true, 0, character_table.Count);

        fontAsset.atlasTextures = new Texture2D[1];
        // Create atlas texture of size zero.
        Texture2D texture = new Texture2D(0, 0, TextureFormat.Alpha8, false);
        texture.name = fontAsset.name + " Atlas";
        fontAsset.atlasTextures[0] = texture;
        fontAsset.material.SetTexture("_MainTex", texture);
        AssetDatabase.AddObjectToAsset(texture, fontAsset);
        AssetDatabase.AddObjectToAsset(fontMat, fontAsset);

        TMP_Settings.fallbackFontAssets.Add(fontAsset);

        EditorUtility.SetDirty(fontAsset);

        AssetDatabase.SaveAssets();
        return fontAsset;
    }

    static Font GetSystemFont(Dictionary<object, object> systemFontDict)
    {
        string uuid = systemFontDict["uuid"].ToString();
        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        string jnity_meta_path;
        string jnity_fontAsset_path;
        string assetPath;
        string unity_fontAsset_path;
        if (resConfig.resFiles.ContainsKey(uuid))
        {
            jnity_fontAsset_path = resConfig.resFiles[uuid].j_path;
            jnity_meta_path = resConfig.resFiles[uuid].j_metaPath;
            assetPath = FileUtil.GetAssetPath(jnity_fontAsset_path);
            unity_fontAsset_path = Application.dataPath + assetPath.Replace("Assets", "");
        }
        else
        {
            jnity_fontAsset_path = JnityBuildInRes.buildInResMap[uuid].path;
            jnity_meta_path = JnityBuildInRes.buildInResMap[uuid].path + ".metax";
            assetPath = Jnity2Unity.unityRelativeAssetPath + JnityBuildInRes.buildInResMap[uuid].idmap_path;
            unity_fontAsset_path = Jnity2Unity.unityAssetPath + JnityBuildInRes.buildInResMap[uuid].idmap_path;
        }

        FileInfo file = new FileInfo(unity_fontAsset_path);
        if (!file.Exists)
        {
            FileUtil.CopyFile(jnity_fontAsset_path, unity_fontAsset_path);
            AssetDatabase.Refresh();
        }
        TrueTypeFontImporter importer= (TrueTypeFontImporter)AssetImporter.GetAtPath(assetPath);
        string text = FileUtil.ReadTexTFile(jnity_meta_path);
        var jsonStr = JsonConvert.DeserializeObject<Dictionary<object, object>>(text);
        string importer_str = jsonStr["FontImporter"].ToString();
        var importer_attribute = JsonConvert.DeserializeObject<Dictionary<object, object>>(importer_str);
        importer.fontSize = int.Parse(importer_attribute["_fontSize"].ToString());
        importer.fontTextureCase = (FontTextureCase)int.Parse(importer_attribute["_forceTextureCase"].ToString());
        importer.customCharacters = importer_attribute["_customCharacters"].ToString();
        importer.fontRenderingMode = ConvertFontRenderingMode(int.Parse(importer_attribute["_fontRenderingMode"].ToString()));
        importer.characterSpacing = int.Parse(importer_attribute["_characterSpacing"].ToString());
        importer.characterPadding = int.Parse(importer_attribute["_characterPadding"].ToString());
        importer.includeFontData = true;
        importer.SaveAndReimport();
        Font systemFont = (Font)AssetDatabase.LoadAssetAtPath<Font>(assetPath);
        return systemFont;
    }

    static GlyphRenderMode ConvertAtlasRenderMode(int jnityRenderMode)
    {
        switch (jnityRenderMode)
        {
            case 0:
                return GlyphRenderMode.SMOOTH;
            case 1:
                return GlyphRenderMode.SMOOTH_HINTED;
            case 2:
                return GlyphRenderMode.RASTER;
            case 3:
                return GlyphRenderMode.RASTER_HINTED;
            case 4:
                return GlyphRenderMode.SDF;
            case 5:
                return GlyphRenderMode.SDFAA_HINTED;
            case 6:
                return GlyphRenderMode.SDFAA;
            default:
                return GlyphRenderMode.SMOOTH;
        }
    }

    static FontRenderingMode ConvertFontRenderingMode(int jnityRenderMode)
    {
        switch (jnityRenderMode)
        {
            case 0:
                return FontRenderingMode.Smooth;
            case 1:
                return FontRenderingMode.HintedSmooth;
            case 3:
                return FontRenderingMode.HintedRaster;
            case 7:
                return FontRenderingMode.OSDefault;
            default:
                return FontRenderingMode.OSDefault;
        }
    }


    
}
