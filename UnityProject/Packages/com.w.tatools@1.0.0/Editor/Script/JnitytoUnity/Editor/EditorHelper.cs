using System;
using System.IO;
using System.Runtime.InteropServices;

public class EditorHelper
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct OpenFileName
    {
        public int structSize;
        public IntPtr dlgOwner;
        public IntPtr instance;
        public String filter;
        public String customFilter;
        public int maxCustFilter;
        public int filterIndex;
        public String file;
        public int maxFile;
        public String fileTitle;
        public int maxFileTitle;
        public String initialDir;
        public String title;
        public int flags;
        public short fileOffset;
        public short fileExtension;
        public String defExt;
        public IntPtr custData;
        public IntPtr hook;
        public String templateName;
        public IntPtr reservedPtr;
        public int reservedInt;
        public int flagsEx;
    }

    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

    // 批量选择文件
    public static string[] OpenFilesPanel(string title, string defaultPath, string[] filter)
    {
        OpenFileName dlg = new OpenFileName();
        dlg.structSize = Marshal.SizeOf(dlg);
        dlg.file = new string(new char[1024]);
        dlg.maxFile = dlg.file.Length;
        dlg.fileTitle = new string(new char[64]);
        dlg.maxFileTitle = dlg.fileTitle.Length;
        dlg.initialDir = defaultPath;
        dlg.title = title;
        // dlg.defExt = "TXT";
        //0x00080000 | ==  OFN_EXPLORER |对于旧风格对话框，目录 和文件字符串是被空格分隔的，函数为带有空格的文件名使用短文件名
        dlg.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        //string filterStr = "";
        //for (int index = 0; index < filter.Length; index += 2)
        //{
        //    if (!string.IsNullOrEmpty(filterStr))
        //    {
        //        filterStr = filterStr + '\0';
        //    }
        //    filterStr = filterStr + string.Format("{0}({1})", filter[index], filter[index + 1]) + '\0' + filter[index + 1];
        //}
        dlg.filter = "全部文件\0*.*\0\0";
        if (GetOpenFileName(dlg))
        {

            string[] SplitStr = { "\0" };
            string[] fileNames = dlg.file.Split(SplitStr, StringSplitOptions.RemoveEmptyEntries);
            if (fileNames.Length > 0 && !string.IsNullOrEmpty(fileNames[0]))
            {
                string[] result;
                if (1 == fileNames.Length)
                {
                    result = fileNames;
                }
                else
                {
                    result = new string[fileNames.Length - 1];
                    for (int index = 1; index < fileNames.Length; index++)
                    {
                        result[index - 1] = Path.Combine(fileNames[0], fileNames[index]);
                    }
                }
                return result;
            }
        }
        return null;
    }
}