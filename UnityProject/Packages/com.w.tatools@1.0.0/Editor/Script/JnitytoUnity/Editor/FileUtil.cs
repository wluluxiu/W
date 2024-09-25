using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize;
    public IntPtr dlgOwner;
    public IntPtr instance;
    public string filter;
    public string customFilter;
    public int maxCustFilter;
    public int filterIndex;
    public string file;
    public int maxFile;
    public string fileTitle;
    public int maxFileTitle;
    public string initialDir;  //打开路径     null
    public string title;
    public int flags;
    public short fileOffset;
    public short fileExtension;
    public string defExt;
    public IntPtr custData;
    public IntPtr hook;
    public string templateName;
    public IntPtr reservedPtr;
    public int reservedInt;
    public int flagsEx;
}


public class FileUtil 
{
    public static string ReadTexTFile(string path)
    {
        string content;
        using (StreamReader sr = File.OpenText(path))
        {
           content =  sr.ReadToEnd();
           sr.Close();
        }
        return content;
    }

    public static void WriteTextFile(string path, string content)
    {
        FileInfo fileInfo = new FileInfo(path);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        StreamWriter sw = fileInfo.CreateText();
        sw.WriteLine(content);
        sw.Close();
        sw.Dispose();
    }

    public static void CopyFile(string srcPath, string dstPath)
    {
        FileInfo fileInfo = new FileInfo(dstPath);
        //if (fileInfo.Exists)
        //    return;
        string dir = fileInfo.Directory.FullName;
        if (!Directory.Exists(dir)){
            Directory.CreateDirectory(dir);
        }
        File.Copy(srcPath, dstPath, true);
    }

    public static void SaveTextureToPath(string path,Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        FileInfo fileInfo = new FileInfo(path);
        //if (fileInfo.Exists)
        //    return;
        string dir = fileInfo.Directory.FullName;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        FileStream file = File.Open(path, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        writer.Close();
        file.Close();
    }

    public static Texture2D GetTexrture2DFromPath(string imgPath)
    {
        FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
        int byteLength = (int)fs.Length;
        byte[] imgBytes = new byte[byteLength];
        fs.Read(imgBytes, 0, byteLength);
        fs.Close();
        fs.Dispose();
        Image img = Image.FromStream(new MemoryStream(imgBytes));
        Texture2D t2d = new Texture2D(img.Width, img.Height);
        img.Dispose();
        t2d.LoadImage(imgBytes);
        t2d.Apply();
        return t2d;
    }

    public static string GetAssetPath(string path)
    {
        string assetPath;
        int startIndex = path.ToLower().IndexOf("assets");
        if(startIndex == -1)
        {
            startIndex = path.ToLower().IndexOf("plugins");
            assetPath = path.Substring(startIndex, path.Length - startIndex);
            assetPath = assetPath.Replace("plugins", "");
            assetPath = Jnity2Unity.unityRelativeAssetPath + assetPath;
            return assetPath;

        }
        assetPath = path.Substring(startIndex, path.Length - startIndex);
        assetPath = assetPath.Replace("assets", "");
        assetPath = Jnity2Unity.unityRelativeAssetPath + assetPath;
        return assetPath;
    }

    public static string getShaderPath(string path)
    {
        StreamReader sr = new StreamReader(path);
		string firstline=string.Empty;
		while (sr.Peek() >= 0)
		{
			firstline = sr.ReadLine();
			if (firstline.Trim().StartsWith("Shader"))
			{
				break;
			}
		}
		sr.Close();
		if (firstline != string.Empty)
		{
			
			int startIndex = firstline.IndexOf("\"") + 1;
			int endIndex = firstline.LastIndexOf("\"") - 1;
			firstline = firstline.Substring(startIndex, endIndex - startIndex + 1);
			return firstline.Trim();
		}
		return string.Empty;
    }

    //public static List<string> OpenFilesDialog(string title, ref string defaultDir, string filter)
    //{
    //    List<string> selectedFiles = new List<string>();
    //    OpenFileDialog ofd = new OpenFileDialog();
    //    ofd.Multiselect = true;
    //    ofd.Title = title;
    //    ofd.Filter = filter;
    //    ofd.InitialDirectory = defaultDir;
    //    ofd.RestoreDirectory = true;
    //    DialogResult result = ofd.ShowDialog();
    //    if (result == DialogResult.OK)
    //    {
    //        foreach (string filePath in ofd.FileNames)
    //        {
    //            selectedFiles.Add(filePath);
    //            defaultDir = Path.GetDirectoryName(filePath);
    //        }
    //    }
    //    return selectedFiles;
    //}


    //void OpenOfnInit()
    //{
    //    OpenFileName Openofn = new OpenFileName();
    //    Openofn.structSize = Marshal.SizeOf(Openofn);
    //    Openofn.dlgOwner = DllScript.GetForegroundWindow();
    //    Openofn.filter = "图片文件或PDF文件(*.jpg;*.png;*.jpeg;*.pdf)\0*.jpg;*.png;*.jpeg;*.pdf\0";
    //    Openofn.file = new string(new char[1024]);
    //    Openofn.maxFile = Openofn.file.Length;
    //    Openofn.fileTitle = new string(new char[64]);
    //    Openofn.maxFileTitle = Openofn.fileTitle.Length;
    //    Openofn.initialDir = "C:\\";
    //    Openofn.title = "选择图片或PDF文件";
    //    Openofn.defExt = "PDF";
    //    Openofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;   //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR    
    //}

    //[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    //public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
}
