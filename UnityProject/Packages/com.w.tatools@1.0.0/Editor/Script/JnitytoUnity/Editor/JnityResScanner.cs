using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class JnityResScanner 
{
	public static Dictionary<string, string> shaderMapping = new Dictionary<string, string>();


	public static void ScannerUnityShader (string unityProjectPath)
	{
		shaderMapping.Clear();
		DirectoryInfo dir = Directory.CreateDirectory(unityProjectPath);
		if (!dir.Exists)
			return;
		FileInfo[] shaderFileInfos = dir.GetFiles("*.shader", SearchOption.AllDirectories);
		foreach (FileInfo fileInfo in shaderFileInfos)
		{
			string shaderFilePath = fileInfo.FullName;
			string shaderPath = FileUtil.getShaderPath(shaderFilePath);
			if (shaderMapping.ContainsKey(shaderPath))
			{
				ReportSystem.OutputLog($"项目中存在相同shaderpath的shader,[{shaderFilePath}],[{shaderMapping[shaderPath]}]");
			}
			else
			{
				shaderMapping.Add(shaderPath, shaderFilePath);
			}
		}
	}

    static void ScannerDirectory(JnityResConfig resConfig , string jnity_dir_path)
    {
        DirectoryInfo dir = Directory.CreateDirectory(jnity_dir_path);
        if (!dir.Exists)
            return;
        FileInfo[] textureFileInfos = dir.GetFiles("*.*", SearchOption.AllDirectories);
        foreach (FileInfo fileInfo in textureFileInfos)
        {
			if (fileInfo.FullName.Contains(".svn"))
			{
				continue;
			}
            JnityFileType fileType;
            if (fileInfo.Name.ToLower().EndsWith("png") || fileInfo.Name.ToLower().EndsWith("jpg") || fileInfo.Name.ToLower().EndsWith("tga"))
            {
                fileType = JnityFileType.Texture;

            }
            else if (fileInfo.Name.ToLower().EndsWith("clip") || fileInfo.Name.ToLower().EndsWith("animatorclip"))
            {
                fileType = JnityFileType.Animation;
            }
            else if (fileInfo.Name.ToLower().EndsWith("fbx"))
            {
                fileType = JnityFileType.FBX;
            }
            else if (fileInfo.Name.ToLower().EndsWith("mat"))
            {
                fileType = JnityFileType.Mat;
            }
            else if (fileInfo.Name.ToLower().EndsWith("shader"))
            {
                fileType = JnityFileType.Shader;
				
			}
            else if (fileInfo.Name.ToLower().EndsWith("controller"))
            {
                fileType = JnityFileType.AnimController;
            }
            else if (fileInfo.Name.ToLower().EndsWith("prefab"))
            {
                fileType = JnityFileType.Prefab;
            }
            else if (fileInfo.Name.ToLower().EndsWith("anim"))
            {
                fileType = JnityFileType.Anim;
            }
            else if (fileInfo.Name.ToLower().EndsWith("mod"))
            {
                fileType = JnityFileType.Mod;
            }
            else if (fileInfo.Name.ToLower().EndsWith("skel"))
            {
                fileType = JnityFileType.Skel;
            }
            else if (fileInfo.Name.ToLower().EndsWith("fontasset"))
            {
                fileType = JnityFileType.FontAsset;
            }else if (fileInfo.Name.ToString().EndsWith("ttf"))
            {
                fileType = JnityFileType.SystemFont;
            }
            else
            {
                continue;
            }
            JnityResFile resFile = new JnityResFile();
            resFile.fileType = fileType;
            resFile.j_path = fileInfo.FullName;
            string metaFilePath = fileInfo.FullName + ".meta";
            string uuid = readUUID(metaFilePath);
            resFile.j_uuid = uuid;
            resFile.j_metaPath = metaFilePath;
            if (!resConfig.resFiles.ContainsKey(uuid))
            {
                resConfig.resFiles.Add(uuid, resFile);
            }
            else
            {
                ReportSystem.OutputLog($"资源映射文件中已包含了相同UUID[{uuid}]的文件,当前文件路径[{resFile.j_path}]," +
                    $"同uuid文件路径[{resConfig.resFiles[uuid].j_path}]");
            }
          
        }
    }

    public static void doScanner(string jnity_proj_path)
    {
        string unity_config_path = Application.dataPath + "/JnityResConfig.json";
        FileInfo configFile = new FileInfo(unity_config_path);
        if (configFile.Exists)
        {
            configFile.Delete();
        }


        JnityResConfig resConfig = new JnityResConfig();
        resConfig.resFiles = new Dictionary<string, JnityResFile>();
        ScannerDirectory(resConfig, jnity_proj_path);
        string jnity_plugin_path = jnity_proj_path.Replace("assets", "") + "plugins";
        ScannerDirectory(resConfig, jnity_plugin_path);
        string jsonText = JsonConvert.SerializeObject(resConfig);
        FileUtil.WriteTextFile(unity_config_path, jsonText);
    }

    private static string readUUID(string metaFilePath)
    {
        string text = FileUtil.ReadTexTFile(metaFilePath);
        var jsonStr = JsonConvert.DeserializeObject<Dictionary<object, object>>(text);
        string uuid = jsonStr["_uuid"].ToString();
        return uuid;
    }
}
