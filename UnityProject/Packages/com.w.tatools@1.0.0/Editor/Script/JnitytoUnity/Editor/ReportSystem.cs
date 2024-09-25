using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class ReportSystem 
{
    static StreamWriter fileWriter;

    public static void OnEnable()
    {
        string dirPath = Application.dataPath + "/../Packages/com.jj.jnitytounity/jnitytounity/Log";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        string filePath = dirPath + "/Report_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")+".csv";
        fileWriter = File.CreateText(filePath);
        fileWriter.AutoFlush = true;
    }

    public static void OnDisable()
    {
        fileWriter.Close();
        fileWriter = null;
    }

    public static void OutputLog(string message)
    {
        if (fileWriter != null)
        {
            fileWriter.WriteLine(message);
        }
        Debug.LogError(message);
    }


}
