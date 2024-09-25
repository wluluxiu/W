namespace jj.TATools.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using UnityEditor;
    using UnityEngine;

    internal class AssetImportPipelineUtility
    {
        #region Fields

        internal const string SubPipelineRelativePath = "/Editor/Resource/AssetImport/";

        internal const string TOOL_MENU_CUSTOM_MODIFY = "Tools/TATools/AssetImportPipeline/Modify/Custom(Texture-SpriteAtlas-Mipmap)";

        internal const string TOOL_MENU_TEXTURE_4x4_MODIFY = "Tools/TATools/AssetImportPipeline/Modify/Texture ASTC_4x4";
        internal const string TOOL_MENU_TEXTURE_6x6_MODIFY = "Tools/TATools/AssetImportPipeline/Modify/Texture ASTC_6x6";
        internal const string TOOL_MENU_TEXTURE_PACKED_ATLAS_MODIFY = "Tools/TATools/AssetImportPipeline/Modify/Texture Packed Into SpriteAtlas";

        internal const string TOOL_MENU_AUTO_SET_SPRITEATLAS = "Tools/TATools/AssetImportPipeline/Auto Set Assets/SpriteAtlas";
        internal const string TOOL_MENU_AUTO_SET_PREFAB = "Tools/TATools/AssetImportPipeline/Auto Set Assets/Prefab";
        internal const string TOOL_MENU_REIMPORT_ALL = "Tools/TATools/AssetImportPipeline/Reimport Assets/ALL";
        internal const string TOOL_MENU_REIMPORT_TEXTURE = "Tools/TATools/AssetImportPipeline/Reimport Assets/Texture";
        internal const string TOOL_MENU_REIMPORT_MODEL = "Tools/TATools/AssetImportPipeline/Reimport Assets/Model";
        internal const string TOOL_MENU_REIMPORT_PREFAB = "Tools/TATools/AssetImportPipeline/Reimport Assets/Prefab";
        internal const string TOOL_MENU_REIMPORT_AUDIOCLIP = "Tools/TATools/AssetImportPipeline/Reimport Assets/AudioClip";
        internal const string TOOL_MENU_REIMPORT_SPRITEATLAS = "Tools/TATools/AssetImportPipeline/Reimport Assets/SpriteAtlas";
        internal const string TOOL_MENU_CREATE_PIPELINE = "Tools/TATools/AssetImportPipeline/Create Import Pipeline";

        internal const string TOOL_MENU_MODEL_SETTING = "AssetImportPipeline/ModelPipeline";
        internal const string TOOL_MENU_TEXTURE_SETTING = "AssetImportPipeline/TexturePipeline";

        internal const string TEXTURE_PLATFORM_ANDROID = "Android";
        internal const string TEXTURE_PLATFORM_IOS = "iPhone";

        const string ASTC_FORMAT_SUFFIX = "ASTC_";

        internal const string META_FILE_EXTENSION = ".meta";

        const string ASSET_PIPELINE_FOLDER = "AssetImport";

        internal const string ASSET_FIRST_IMPORTED = "FirstImported";

        internal const char ENUM_DATA_SPLIT_CHAR = '|';

        #endregion

        #region Internal Methods

        internal static Dictionary<string, T> GetImportModuleSetting<T>(Action<T> callback = null) where T : ScriptableObject
        {
            Dictionary<string, T> mapping = new Dictionary<string, T>();
            var tempMapping = new Dictionary<string, T>();
            var subPipelineRelativePath = AssetImportPipelineUtility.SubPipelineRelativePath;
            var guids = AssetDatabase.FindAssets("t:" + typeof(T), new string[] { "Assets" });
            if (guids != null)
            { 
                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    T t = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                    if (callback != null)
                        callback(t);
                    var fileName = Path.GetFileName(assetPath);
                    tempMapping[assetPath.Replace(subPipelineRelativePath + fileName,"")] = t;
                }
            }

            foreach (var data in tempMapping)
            {
                var folder = data.Key;
                var pipeline = data.Value;
                var subFiles = Directory.GetFiles(folder, "*.*",SearchOption.AllDirectories);
                foreach (var subFile in subFiles)
                {
                    if (subFile.EndsWith(META_FILE_EXTENSION)) continue;

                    var assetPath = subFile.Replace(Application.dataPath, "Assets").Replace("\\", "/");
                    mapping[assetPath] = pipeline;
                }
            }

            return mapping;
        }

        internal static bool TextureFormatIsASTC(TextureImporterFormat format)
        {
            return format.ToString().Contains(ASTC_FORMAT_SUFFIX);
        }

        internal static string GetImportPipelineFullFolder(string baseFolder)
        {
            if (baseFolder.EndsWith(ASSET_PIPELINE_FOLDER))
                return baseFolder;
            else
                return baseFolder + "/" + ASSET_PIPELINE_FOLDER;
        }

        internal static string GetPropName(Expression<System.Func<System.Object>> expr)
        {
            Expression e = expr.Body;
            MemberExpression me = e as MemberExpression;
            if (me == null)
            {
                UnaryExpression ue = e as UnaryExpression;
                me = ue.Operand as MemberExpression;
            }
            return me.Member.Name;
        }

        internal static bool IsNonPowerOfTwo(int sourceWidth,int sourceHeight)
        {
            int widthFactor = sourceWidth & (sourceWidth - 1);
            int heightFactor = sourceHeight & (sourceHeight - 1); ;

            return widthFactor != 0 || heightFactor != 0;
        }

        internal static List<T> GetDependencies<T>(string assetPath, bool recursive) where T : UnityEngine.Object
        {
            List<T> dependencies = new List<T>();
            var dependenciePathArray = AssetDatabase.GetDependencies(assetPath, recursive);
            foreach (var depPath in dependenciePathArray)
            {
                var depObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(depPath);
                T realObj = depObj as T;
                if (realObj != null)
                {
                    if (!dependencies.Contains(realObj))
                        dependencies.Add(realObj);
                }
            }
            return dependencies;
        }

        internal static List<string> GetPathDependencies<T>(string assetPath, bool recursive) where T : UnityEngine.Object
        {
            List<string> dependencies = new List<string>();
            var dependenciePathArray = AssetDatabase.GetDependencies(assetPath, recursive);
            foreach (var depPath in dependenciePathArray)
            {
                var depObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(depPath);
                T realObj = depObj as T;
                if (realObj != null)
                {
                    if (!dependencies.Contains(depPath))
                        dependencies.Add(depPath);
                }
            }
            return dependencies;
        }

        #endregion
    }
}

