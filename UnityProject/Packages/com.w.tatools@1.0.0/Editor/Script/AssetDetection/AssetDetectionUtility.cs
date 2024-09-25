using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using System.Security;
using System.Runtime.ExceptionServices;

using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;

namespace jj.TATools.Editor
{
    internal delegate void VoidTwoParamsCallback<T, O>(T t,O o);

    internal sealed class AssetDetectionUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        internal static void OpenFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) return;

            System.Diagnostics.Process.Start(filePath);
        }

        internal static void DeleteFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) return;

            System.IO.File.Delete(filePath);
        }

        internal static string OpenFolderPanel(string title,string defaultFolder)
        {
            string finalSelectedFolder = EditorUtility.OpenFolderPanel(title, defaultFolder, Application.dataPath);
            return finalSelectedFolder;
        }

        internal static string OpenFilePanel(string title,string extension, string folder)
        {
            string filePath = EditorUtility.OpenFilePanel(title, folder, extension);
            return filePath;
        }

        internal static string SaveFilePanel(string title, string fileName,string extension, string folder)
        {
            string saveFilePath = EditorUtility.SaveFilePanel(title, folder, fileName + "." + extension, extension);
            return saveFilePath;
        }

        internal static void MessageBox(string text,string caption)
        {
            EditorUtility.DisplayDialog(caption, text, "OK");
        }

        internal static string GetFirstStr(string[] arr, int count)
        {
            string firstStr = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == count)
                {
                    break;
                }
                else
                {
                    if (!string.IsNullOrEmpty(arr[i]))
                    {
                        firstStr = arr[i];
                        break;
                    }
                }
            }
            return firstStr;
        }

        internal static string GetNoEmptyStr(string[] arr, int targetIndex)
        {
            int strIndex = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                string str = arr[i];
                if (!string.IsNullOrEmpty(str))
                {
                    if (strIndex == targetIndex)
                    {
                        return str;
                    }
                    strIndex++;
                }
            }
            return null;
        }

        internal static string GetFileSize(long fileSize)
        {
            float sizeF = fileSize;
            bool negitiveFlag = sizeF < 0;
            string sign = negitiveFlag ? "-" : "";
            string unit = "";
            float sizeAbs = negitiveFlag ? -1 * sizeF : sizeF;
            if (sizeAbs > 1024.0f)
            {
                sizeAbs /= 1024.0f;
                if (sizeAbs > 1024.0f)
                {
                    sizeAbs /= 1024.0f;
                    if (sizeAbs > 1024.0f)
                    {
                        sizeAbs /= 1024.0f;
                        unit = "GB";
                    }
                    else
                    {
                        unit = "MB";
                    }
                }
                else
                {
                    unit = "KB";
                }
            }
            else
            {
                unit = "B";
            }

            return sign + sizeAbs.ToString("F2") + " " + unit;
        }

        internal static string PathCombine(string path0, string path1)
        {
            if (path0.EndsWith(ConstDefine.PATH_SPLIT_FLAG))
                return path0 + path1;
            else
                return path0 + ConstDefine.PATH_SPLIT_FLAG + path1;
        }

        internal static string GetMD5HashFromFile(string filePath)
        {
            try
            {
                FileStream filestream = new FileStream(filePath, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(filestream);
                filestream.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(filePath + ":GetMD5HashFromFile Fail,Error:" + ex.ToString());
            }
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        internal static bool GetImageRGBA8888Info(string imagePath,out int width,out int height,out long gpuSize,out float transparentPertanage)
        {
            width = 0;
            height = 0;
            gpuSize = 0;
            transparentPertanage = 0.0f;

            // tga
            if (imagePath.ToLower().EndsWith(ConstDefine.TGA_EXTENSION))
            {
                try
                {
                    var targaImage = new Paloma.TargaImage(imagePath);
                    var bitmap = targaImage.Image;

                    width = bitmap.Width;
                    height = bitmap.Height;

                    gpuSize = width * height * 4;

                    int zeroAlphaAmount = 0;
                    for (int w = 0; w < width; w++)
                    {
                        for (int h = 0; h < height; h++)
                        {
                            System.Drawing.Color pixelColor = bitmap.GetPixel(w, h);
                            if (pixelColor.A == 0)
                            {
                                zeroAlphaAmount++;
                            }
                        }
                    }

                    transparentPertanage = 1.0f * zeroAlphaAmount / (width * height);

                    bitmap.Dispose();

                }
                catch (System.Exception e)
                {

                }
               
                return true;
            }
            else
            {
                try
                {
                    // bmp jgp png
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath))
                    {
                        width = image.Width;
                        height = image.Height;

                        gpuSize = width * height * 4;

                        if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                        {
                            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);

                            int bitWidth = bitmap.Width;
                            int bitHeight = bitmap.Height;
                            int zeroAlphaAmount = 0;
                            for (int w = 0; w < bitWidth; w++)
                            {
                                for (int h = 0; h < bitHeight; h++)
                                {
                                    System.Drawing.Color pixelColor = bitmap.GetPixel(w, h);
                                    if (pixelColor.A == 0)
                                    {
                                        zeroAlphaAmount++;
                                    }
                                }
                            }

                            transparentPertanage = 1.0f * zeroAlphaAmount / (bitWidth * bitHeight);

                            bitmap.Dispose();
                        }
                        else
                        {
                            transparentPertanage = 0.0f;
                        }

                        image.Dispose();

                        return true;
                    }
                }
                catch (System.Exception e)
                {

                }
            }

            return false;
        }

        internal static string GetTextureMD5FilePath(string baseFolder, string targetFolder)
        {
            string flagStr = "/";
            int index = targetFolder.IndexOf(flagStr) + 1;
            string fileName = targetFolder.Substring(index, targetFolder.Length - index);
            return PathCombine(baseFolder, fileName.Replace(flagStr, "-") + ".cache");
        }

        internal static bool IsNonPowerOfTwo(Texture texture)
        {
            int width = texture.width;
            int height = texture.height;
            int widthFactor = width & (width - 1);
            int heightFactor = height & (height - 1); ;

            return widthFactor != 0 || heightFactor != 0;
        }

        internal static bool IsNonPowerOfTwo(int width, int height)
        {
            int widthFactor = width & (width - 1);
            int heightFactor = height & (height - 1); ;

            return widthFactor != 0 || heightFactor != 0;
        }

        static Type UnityEditor_TextureUtil;
        static Type UnityEditor_U2D_SpriteAtlasExtensions;


        internal static string GetTransformNodePath(Transform node)
        {
            string nodePath = node.name;

            var parent = node.parent;
            while (parent != null)
            {
                nodePath = parent.name + "/" + nodePath;
                parent = parent.parent;
            }

            return nodePath;
        }

        internal static long GetFileDiskSize(string assetPath)
        {
            FileInfo fInfo = new FileInfo(assetPath);
            return fInfo.Length;
        }

        internal static string[] GetShaderGlobalKeywords(Shader shader)
        {
            var type = typeof(ShaderUtil);
            MethodInfo getShaderGlobalKeywordsMethod = type.GetMethod("GetShaderGlobalKeywords", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            string[] globalKeywords = (string[])getShaderGlobalKeywordsMethod.Invoke(null, new object[] { shader });
            return globalKeywords;
        }

        internal static string[] GetShaderLocalKeywords(Shader shader)
        {
            var type = typeof(ShaderUtil);
            MethodInfo getShaderLocalKeywordsMethod = type.GetMethod("GetShaderLocalKeywords", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            string[] localKeywords = (string[])getShaderLocalKeywordsMethod.Invoke(null, new object[] { shader });
            return localKeywords;
        }

        internal static long GetTextureRuntimeMemorySize(Texture texture)
        {
            long memorySize = 0;
            if (UnityEditor_TextureUtil == null) UnityEditor_TextureUtil = typeof(TextureImporter).Assembly.GetType("UnityEditor.TextureUtil");
            MethodInfo getRuntimeMemorySizeLongMethod = UnityEditor_TextureUtil.GetMethod("GetRuntimeMemorySizeLong", BindingFlags.Static | BindingFlags.Public);
            memorySize = (long)getRuntimeMemorySizeLongMethod.Invoke(null, new object[] { texture });
            return memorySize;
        }

        internal static Texture2D[] GetPreviewTextures(SpriteAtlas spriteAtlas)
        {
            if (UnityEditor_U2D_SpriteAtlasExtensions == null) UnityEditor_U2D_SpriteAtlasExtensions = typeof(UnityEditor.U2D.SpriteAtlasAsset).Assembly.GetType("UnityEditor.U2D.SpriteAtlasExtensions");
            MethodInfo getPreviewTexturesMethod = UnityEditor_U2D_SpriteAtlasExtensions.GetMethod("GetPreviewTextures", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            Texture2D[] textures = (Texture2D[])getPreviewTexturesMethod.Invoke(null, new object[] { spriteAtlas });
            return textures;
        }

        internal static long GetTextureFileSize(Texture texture)
        {
            long fileSize = 0;
            if (UnityEditor_TextureUtil == null) UnityEditor_TextureUtil = typeof(TextureImporter).Assembly.GetType("UnityEditor.TextureUtil");
            MethodInfo getStorageMemorySizeLongMethod = UnityEditor_TextureUtil.GetMethod("GetStorageMemorySizeLong", BindingFlags.Static | BindingFlags.Public);
            fileSize = (long)getStorageMemorySizeLongMethod.Invoke(null, new object[] { texture });
            return fileSize;
        }

        internal static List<string> GetBuiltinDenpendencies(UnityEngine.Object asset)
        {
            List<string> builtinDeps = new List<string>();

            if (asset == null) return builtinDeps;

            var allDependenices = EditorUtility.CollectDependencies(new UnityEngine.Object[] { asset });

            foreach (var depObj in allDependenices)
            {
                if (depObj == asset) continue;

                if (depObj is Texture2D || depObj is Material || depObj is Shader || depObj is Mesh)
                {
                    var assetPath = AssetDatabase.GetAssetPath(depObj);
                    if (!assetPath.StartsWith("Assets/"))
                    {
                        var assetName = depObj.name + "(" + depObj.GetType().Name + ")";
                        if (!builtinDeps.Contains(assetName))
                            builtinDeps.Add(assetName);
                    }
                }
            }

            return builtinDeps;
        }

        internal static List<string> GetDependencies(string assetPath, bool recursive)
        {
            var dependencies = AssetDatabase.GetDependencies(assetPath, recursive);
            return new List<string>(dependencies);
        }

        internal static List<string> GetDependencies<T>(string assetPath, bool recursive) where T : UnityEngine.Object
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

        internal static Texture2D DuplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.sRGB);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }

        internal static string GetMaterialRefShaderGuid(string materialFilePath)
        {
            string guid = "";
            StreamReader sr = new StreamReader(materialFilePath);
            string lineStr = sr.ReadLine();
            string shaderPropName = "m_Shader:";
            string guidPropName = "guid:";
            while (lineStr != null)
            {
                if (lineStr.Contains(shaderPropName))
                {
                    var tempStr = lineStr.Replace(" ", "");
                    var arr = tempStr.Split(',');
                    if (arr.Length == 3)
                    {
                        guid = arr[1].Replace(guidPropName, "");
                    }
                    break;
                }

                lineStr = sr.ReadLine();
            }

            sr.Close();
            sr.Dispose();

            return guid;
        }

        internal static List<string> CheckReferencedInCode(string filter, Dictionary<string,string> scriptsMapping, bool isCheckSpecialRule = true)
        {
            var codeList = MatchScripts(filter, scriptsMapping);
     
            if (isCheckSpecialRule)
            {
                filter = SpecialRule(filter);
                if (!string.IsNullOrEmpty(filter))
                {
                    codeList = MatchScripts(filter, scriptsMapping);
                }
            }

            return codeList;
        }

        static List<string> MatchScripts(string filter, Dictionary<string, string> scriptsMapping)
        {
            var codeList = new List<string>();
            foreach (var data in scriptsMapping)
            {
                var content = data.Value;
                if (Regex.IsMatch(content, filter))
                {
                    codeList.Add(data.Key);
                }
            }
            return codeList;
        }

        static string SpecialRule(string filter)
        {
            string result = "";
            if (filter.Contains("_"))
            {
                var chars = filter.Split('_');
                if (chars.Length >= 4)
                {
                    List<string> newStrs = new List<string>();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        if (!Regex.IsMatch(chars[i], @"\d"))
                        {
                            newStrs.Add(chars[i]);
                        }
                    }

                    result = newStrs[0];
                    for (int i = 1; i < newStrs.Count; i++)
                    {
                        result = result + "_" + newStrs[i];
                    }
                }
                else
                {
                    int value = 0;
                    if (int.TryParse(chars[chars.Length - 1], out value))
                    {
                        int lastIndex = filter.LastIndexOf('_');
                        result = filter.Substring(0, lastIndex);
                    }
                }
            }
            return result;
        }
    }
}
