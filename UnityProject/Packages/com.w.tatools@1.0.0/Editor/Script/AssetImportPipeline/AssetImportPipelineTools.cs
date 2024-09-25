namespace jj.TATools.Editor
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.U2D;
    using UnityEditor.U2D;

    internal class AssetImportPipelineTools
    {
        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_REIMPORT_ALL)]
        static void ReimportAllAssets()
        {
            ReimportTextures();

            ReimportsSpriteAtlas();

            ReimportModels();

            ReimportAudioClip();
        }

        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_REIMPORT_TEXTURE)]
        static void ReimportTextures()
        {
            var guids = Selection.assetGUIDs;
            if (guids == null || guids.Length == 0) return;

            var folder = AssetDatabase.GUIDToAssetPath(guids[0]);
            if (Directory.Exists(folder))
            {
                guids = AssetDatabase.FindAssets("t:Texture", new string[] { folder });
                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    TextureImporter tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                    if (tImporter == null) continue;

                    AssetDatabase.ImportAsset(assetPath);
                }
            } 
        }

        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_REIMPORT_SPRITEATLAS)]
        static void ReimportsSpriteAtlas()
        {
            var guids = Selection.assetGUIDs;
            if (guids == null || guids.Length == 0) return;

            var folder = AssetDatabase.GUIDToAssetPath(guids[0]);
            if (Directory.Exists(folder))
            {
                guids = AssetDatabase.FindAssets("t:SpriteAtlas", new string[] { folder });
                foreach (var guid in guids)
                {
                    AssetDatabase.ImportAsset(AssetDatabase.GUIDToAssetPath(guid));
                }
            }
        }

        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_REIMPORT_MODEL)]
        static void ReimportModels()
        {
            var guids = Selection.assetGUIDs;
            if (guids == null || guids.Length == 0) return;

            var folder = AssetDatabase.GUIDToAssetPath(guids[0]);
            if (Directory.Exists(folder))
            {
                guids = AssetDatabase.FindAssets("t:Model", new string[] { folder });
                foreach (var guid in guids)
                {
                    AssetDatabase.ImportAsset(AssetDatabase.GUIDToAssetPath(guid));
                }
            }
        }

        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_REIMPORT_PREFAB)]
        static void ReimportPrefabs()
        {
            var guids = Selection.assetGUIDs;
            if (guids == null || guids.Length == 0) return;

            var folder = AssetDatabase.GUIDToAssetPath(guids[0]);
            if (Directory.Exists(folder))
            {
                guids = AssetDatabase.FindAssets("t:Prefab", new string[] { folder });
                foreach (var guid in guids)
                {
                    AssetDatabase.ImportAsset(AssetDatabase.GUIDToAssetPath(guid));
                }
            }
        }

        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_REIMPORT_AUDIOCLIP)]
        static void ReimportAudioClip()
        {
            var guids = Selection.assetGUIDs;
            if (guids == null || guids.Length == 0) return;

            var folder = AssetDatabase.GUIDToAssetPath(guids[0]);
            if (Directory.Exists(folder))
            {
                guids = AssetDatabase.FindAssets("t:AudioClip", new string[] { folder });
                foreach (var guid in guids)
                {
                    AssetDatabase.ImportAsset(AssetDatabase.GUIDToAssetPath(guid));
                }
            }
        }

        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_CREATE_PIPELINE)]
        static void CreatImportPipeline()
        {
            var guids = Selection.assetGUIDs;
            if (guids == null || guids.Length == 0) return;

            var folder = AssetDatabase.GUIDToAssetPath(guids[0]);
            if (Directory.Exists(folder))
            {
                var fullFolder = AssetImportPipelineUtility.GetImportPipelineFullFolder(folder);
                if (!Directory.Exists(fullFolder)) Directory.CreateDirectory(fullFolder);

                // Texture Pipeline
                var tPipelineAssetPath = fullFolder + "/" + typeof(TextureImportPipeline).Name + ".asset";
                if (!File.Exists(tPipelineAssetPath))
                {
                    TextureImportPipeline tPipeline = ScriptableObject.CreateInstance<TextureImportPipeline>();
                    AssetDatabase.CreateAsset(tPipeline, tPipelineAssetPath);
                    AssetDatabase.Refresh();
                }

                // Model Pipeline
                var mPipelineAssetPath = fullFolder + "/" + typeof(ModelImportPipeline).Name + ".asset";
                if (!File.Exists(mPipelineAssetPath))
                {
                    ModelImportPipeline mPipeline = ScriptableObject.CreateInstance<ModelImportPipeline>();
                    AssetDatabase.CreateAsset(mPipeline, mPipelineAssetPath);
                    AssetDatabase.Refresh();
                }

                // Prefab Pipeline
                var pPipelineAssetPath = fullFolder + "/" + typeof(PrefabImportPipeline).Name + ".asset";
                if (!File.Exists(pPipelineAssetPath))
                {
                    PrefabImportPipeline pPipeline = ScriptableObject.CreateInstance<PrefabImportPipeline>();
                    AssetDatabase.CreateAsset(pPipeline, pPipelineAssetPath);
                    AssetDatabase.Refresh();
                }
                // SpriteAtlas Pipeline
                var atlasPipelinePath = fullFolder + "/" + typeof(SpriteAtlasImportPipeline).Name + ".asset";
                if (!File.Exists(atlasPipelinePath))
                {
                    SpriteAtlasImportPipeline atlasPipeline = ScriptableObject.CreateInstance<SpriteAtlasImportPipeline>();
                    AssetDatabase.CreateAsset(atlasPipeline, atlasPipelinePath);
                    AssetDatabase.Refresh();
                }
                // Audio Pipeline
                var audioPipelinePath = fullFolder + "/" + typeof(AudioClipImportPipeline).Name + ".asset";
                if (!File.Exists(audioPipelinePath))
                {
                    AudioClipImportPipeline audioPipeline = ScriptableObject.CreateInstance<AudioClipImportPipeline>();
                    AssetDatabase.CreateAsset(audioPipeline, audioPipelinePath);
                    AssetDatabase.Refresh();
                }
            }
        }

        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_AUTO_SET_SPRITEATLAS)]
        static void AutoSetSpriteAtlas()
        {
            SpriteAtlasImportPipeline.AutoSet();
        }

        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_AUTO_SET_PREFAB)]
        static void AutoSetPrefab()
        {
            PrefabImportPipeline.AutoSet();
        }

        //static void OneKeyModifyFormat(string modulePath,TextureImporterFormat targetFormat)
        //{
        //    int totalAmount = 0;

        //    // SpriteAtlas
        //    List<string> texturePackedInSpriteAtlasList = new List<string>();
        //    var guids = AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name, new string[] { modulePath });
        //    foreach (var guid in guids)
        //    {
        //        var path = AssetDatabase.GUIDToAssetPath(guid);

        //        var dependencices = AssetImportPipelineUtility.GetPathDependencies<Texture>(path, true);
        //        texturePackedInSpriteAtlasList.AddRange(dependencices);
        //        foreach (var dep in dependencices)
        //        {
        //            TextureImporter tImporter = AssetImporter.GetAtPath(dep) as TextureImporter;
        //            if (tImporter == null) continue;

        //            TextureImporterFormat standardFormat = TextureImporterFormat.RGBA32;
        //            if (!tImporter.DoesSourceTextureHaveAlpha())
        //                standardFormat = TextureImporterFormat.RGB24;

        //            bool needUpdate = false;
        //            TextureImporterPlatformSettings tAndroidPlatformSeting = tImporter.GetPlatformTextureSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_ANDROID);
        //            if (tAndroidPlatformSeting.overridden)
        //            {

        //                if (tAndroidPlatformSeting.format != standardFormat)
        //                {
        //                    needUpdate = true;
        //                    tAndroidPlatformSeting.format = standardFormat;
        //                }
        //            }
        //            else
        //            {
        //                needUpdate = true;
        //                tAndroidPlatformSeting.overridden = true;
        //                tAndroidPlatformSeting.format = standardFormat;
        //            }

        //            TextureImporterPlatformSettings tIosPlatformSeting = tImporter.GetPlatformTextureSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_IOS);
        //            if (tIosPlatformSeting.overridden)
        //            {
        //                if (tIosPlatformSeting.format != standardFormat)
        //                {
        //                    needUpdate = true;
        //                    tIosPlatformSeting.format = standardFormat;
        //                }
        //            }
        //            else
        //            {
        //                needUpdate = true;
        //                tIosPlatformSeting.overridden = true;
        //                tIosPlatformSeting.format = standardFormat;
        //            }

        //            if (needUpdate)
        //            {
        //                totalAmount++;
        //                tImporter.SetPlatformTextureSettings(tAndroidPlatformSeting);
        //                tImporter.SetPlatformTextureSettings(tIosPlatformSeting);
        //                tImporter.SaveAndReimport();
        //            }
        //        }

        //        var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
        //        var needSaveAssetIfDirty = false;
        //        // Android Platform 
        //        var androidPlatformSeting = spriteAtlas.GetPlatformSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_ANDROID);
        //        if (androidPlatformSeting != null)
        //        {
        //            var needUpdateAndroidSetting = false;

        //            if (androidPlatformSeting.overridden != true)
        //            {
        //                androidPlatformSeting.overridden = true;
        //                needSaveAssetIfDirty = true;
        //                needUpdateAndroidSetting = true;
        //            }

        //            if (androidPlatformSeting.format != targetFormat)
        //            {
        //                androidPlatformSeting.format = targetFormat;
        //                needSaveAssetIfDirty = true;
        //                needUpdateAndroidSetting = true;
        //            }

        //            if (needUpdateAndroidSetting)
        //                spriteAtlas.SetPlatformSettings(androidPlatformSeting);
        //        }

        //        // IOS Platform 
        //        var iosPlatformSeting = spriteAtlas.GetPlatformSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_IOS);
        //        if (iosPlatformSeting != null)
        //        {
        //            var needUpdateIosSetting = false;
        //            if (iosPlatformSeting.overridden != true)
        //            {
        //                iosPlatformSeting.overridden = true;
        //                needSaveAssetIfDirty = true;
        //                needUpdateIosSetting = true;
        //            }


        //            if (iosPlatformSeting.format != targetFormat)
        //            {
        //                iosPlatformSeting.format = targetFormat;
        //                needSaveAssetIfDirty = true;
        //                needUpdateIosSetting = true;
        //            }


        //            if (needUpdateIosSetting)
        //                spriteAtlas.SetPlatformSettings(iosPlatformSeting);
        //        }

        //        if (needSaveAssetIfDirty)
        //        {
        //            totalAmount++;
        //            AssetDatabase.SaveAssetIfDirty(spriteAtlas);
        //        }
        //    }

        //    // Texture
        //    guids = AssetDatabase.FindAssets("t:Texture", new string[] { modulePath });
        //    foreach (var guid in guids)
        //    {
        //        var path = AssetDatabase.GUIDToAssetPath(guid);
        //        if (texturePackedInSpriteAtlasList.Contains(path)) continue;

        //        var tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        //        if (tImporter == null) continue;

        //        bool needUpdate = false;

        //        TextureImporterPlatformSettings androidPlatformSeting = tImporter.GetPlatformTextureSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_ANDROID);
        //        if (androidPlatformSeting.overridden)
        //        {
        //            if (androidPlatformSeting.format != targetFormat)
        //            {
        //                needUpdate = true;
        //                androidPlatformSeting.format = targetFormat;
        //            }
        //        }
        //        else
        //        {
        //            needUpdate = true;
        //            androidPlatformSeting.overridden = true;
        //            androidPlatformSeting.format = targetFormat;
        //        }

        //        TextureImporterPlatformSettings iosPlatformSeting = tImporter.GetPlatformTextureSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_IOS);
        //        if (iosPlatformSeting.overridden)
        //        {
        //            if (iosPlatformSeting.format != targetFormat)
        //            {
        //                needUpdate = true;
        //                iosPlatformSeting.format = targetFormat;
        //            }
        //        }
        //        else
        //        {
        //            needUpdate = true;
        //            iosPlatformSeting.overridden = true;
        //            iosPlatformSeting.format = targetFormat;
        //        }

        //        if (needUpdate)
        //        {
        //            totalAmount++;
        //            tImporter.SetPlatformTextureSettings(androidPlatformSeting);
        //            tImporter.SetPlatformTextureSettings(iosPlatformSeting);
        //            tImporter.SaveAndReimport();
        //        }
        //    }

        //    Debug.LogError("[" + targetFormat + "]" + modulePath + ":" + totalAmount);
        //}

        //[MenuItem(AssetImportPipelineUtility.TOOL_MENU_TEXTURE_4x4_MODIFY)]
        //static void  ModifyTextureASTC4X4()
        //{
        //    string[] modulePathArray = new string[]
        //    {
        //        "Assets/JJGame/chinachess/Runtime/Resource/chinachess/UI",
        //        "Assets/JJGame/unityhall/Runtime/Resource/unityhall/UI",
        //        "Assets/JJUpdater/tkupdater/Runtime/Resource/tkupdater/UI",
        //        "Assets/JJWidget/widgetsdk/Runtime/Resource/widgetsdk/Common",
        //        "Assets/JJWidget/widgetsdk/Runtime/Resource/widgetsdk/UI",
        //        "Assets/JJWidget/tkgamewidget/Runtime/Resource/tkgamewidget/UI",
        //        "Assets/JJWidget/tkmatchwidget/Runtime/Resource/tkmatchwidget/UI"
        //    };

        //    foreach(var module in modulePathArray)
        //        OneKeyModifyFormat(module, TextureImporterFormat.ASTC_4x4);
        //}

        //[MenuItem(AssetImportPipelineUtility.TOOL_MENU_TEXTURE_6x6_MODIFY)]
        //static void ModifyTextureASTC6X6()
        //{
        //    string[] modulePathArray = new string[]
        //    {
        //        "Assets/JJGame/chinachess/Runtime/Resource/chinachess/UI",
        //        "Assets/JJGame/unityhall/Runtime/Resource/unityhall/UI",
        //        "Assets/JJUpdater/tkupdater/Runtime/Resource/tkupdater/UI",
        //        "Assets/JJWidget/widgetsdk/Runtime/Resource/widgetsdk/Common",
        //        "Assets/JJWidget/widgetsdk/Runtime/Resource/widgetsdk/UI",
        //        "Assets/JJWidget/tkgamewidget/Runtime/Resource/tkgamewidget/UI",
        //        "Assets/JJWidget/tkmatchwidget/Runtime/Resource/tkmatchwidget/UI"
        //    };

        //    foreach (var module in modulePathArray)
        //        OneKeyModifyFormat(module, TextureImporterFormat.ASTC_6x6);
        //}

        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_TEXTURE_PACKED_ATLAS_MODIFY)]
        static void ModifyTexturePackedInSpriteAtlas()
        {
            if (Selection.assetGUIDs.Length == 0) return;

            var modulePath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);

            var guids = AssetDatabase.FindAssets("t:" + typeof(UnityEngine.U2D.SpriteAtlas).Name, new string[] { modulePath });
            int totalAmount = 0;
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                var dependencices = AssetImportPipelineUtility.GetDependencies<Texture>(path,true);
                foreach (var dep in dependencices)
                {
                    TextureImporter tImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(dep)) as TextureImporter;
                    if (tImporter == null) continue;

                    TextureImporterFormat targetFormat = TextureImporterFormat.RGBA32;
                    if (!tImporter.DoesSourceTextureHaveAlpha())
                        targetFormat = TextureImporterFormat.RGB24;

                    bool needUpdate = false;
                    TextureImporterPlatformSettings androidPlatformSeting = tImporter.GetPlatformTextureSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_ANDROID);
                    if (androidPlatformSeting.overridden)
                    {
                       
                        if (androidPlatformSeting.format != targetFormat)
                        {
                            needUpdate = true;
                            androidPlatformSeting.format = targetFormat;
                        }
                    }
                    else
                    {
                        needUpdate = true;
                        androidPlatformSeting.overridden = true;
                        androidPlatformSeting.format = targetFormat;
                    }

                    TextureImporterPlatformSettings iosPlatformSeting = tImporter.GetPlatformTextureSettings(AssetImportPipelineUtility.TEXTURE_PLATFORM_IOS);
                    if (iosPlatformSeting.overridden)
                    {
                        if (iosPlatformSeting.format != targetFormat)
                        {
                            needUpdate = true;
                            iosPlatformSeting.format = targetFormat;
                        }
                    }
                    else
                    {
                        needUpdate = true;
                        iosPlatformSeting.overridden = true;
                        iosPlatformSeting.format = targetFormat;
                    }

                    if (needUpdate)
                    {
                        totalAmount++;
                        tImporter.SetPlatformTextureSettings(androidPlatformSeting);
                        tImporter.SetPlatformTextureSettings(iosPlatformSeting);
                        tImporter.SaveAndReimport();
                    }
                }
              
            }

            Debug.LogError(modulePath + ":" + totalAmount);
        }

        [MenuItem(AssetImportPipelineUtility.TOOL_MENU_CUSTOM_MODIFY)]
        static void DoCustomModify()
        {
            // SpriteAtlas :Enable Mipmaps
            var guids = AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name, new string[] { "Assets" });
            foreach (var guid in guids)
            {
                var atlasPath = AssetDatabase.GUIDToAssetPath(guid);
                var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
                if (atlas != null)
                {
                    var textureSetting = atlas.GetTextureSettings();
                    if (!textureSetting.generateMipMaps)
                    {
                        textureSetting.generateMipMaps = true;
                        atlas.SetTextureSettings(textureSetting);
                    }
                }
            }
            // Texture:Enable Mipmaps
            guids = AssetDatabase.FindAssets("t:Texture", new string[] { "Assets" });
            foreach (var guid in guids)
            {
                var texturePath = AssetDatabase.GUIDToAssetPath(guid);
                var tImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
                if (tImporter == null) continue;

                if (!tImporter.mipmapEnabled)
                {
                    tImporter.mipmapEnabled = true;
                    tImporter.SaveAndReimport();
                }
            }

            AssetDatabase.SaveAssets();
        }
    }
}

