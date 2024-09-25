using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class JnityResConfig 
{
    public Dictionary<string,JnityResFile> resFiles;

    public static JnityResConfig getJnityResConfig(string path)
    {
        string unity_config_path = Application.dataPath + "/JnityResConfig.json";
        string config_json = FileUtil.ReadTexTFile(unity_config_path);
        JnityResConfig config = Newtonsoft.Json.JsonConvert.DeserializeObject<JnityResConfig>(config_json);
        return config;
    }
}

[System.Serializable]
public class JnityResFile
{
    public string j_uuid;
    public string u_uuid;
    public string j_path;
    public string u_path;
    public string j_metaPath;
    public JnityFileType fileType;
}


public enum JnityFileType
{
    FBX,
    Mat,
    Shader,
    Texture,
    Animation,
    AnimController,
    Prefab,
    Anim,
    Mod,
    Skel,
    FontAsset,
    SystemFont

}


public class JnityBuildInRes
{
    public static Dictionary<string, BuildInRes> buildInResMap = new Dictionary<string, BuildInRes>();
	public const string PACKAGE_PATH = "/Packages/com.jj.tatools@1.0.0/Editor/Script/JnitytoUnity/";

	public class BuildInRes
    {
        public string uuid;
        public string path;
        public string idmap_path;
        public JnityFileType fileType;
    }

    public static void loadBuildInIdMap()
    {
        buildInResMap.Clear();

        string idmapPath = Application.dataPath + "/.."+PACKAGE_PATH+"JlichtBuiltInResEx.idmap";
        string text = FileUtil.ReadTexTFile(idmapPath);
        var jsonStr = JsonConvert.DeserializeObject<Dictionary<object, object>>(text);
        var uuidlist = JsonConvert.DeserializeObject<List<object>>(jsonStr["uuidlist"].ToString());
        for (int n = 0; n < uuidlist.Count; n++)
        {
            BuildInRes buildInRes = new BuildInRes();
            var resInfo = JsonConvert.DeserializeObject<Dictionary<object, object>>(uuidlist[n].ToString());
            buildInRes.uuid = resInfo["uuid"].ToString();
            buildInRes.uuid = buildInRes.uuid.Replace(":", "");
            buildInRes.idmap_path = resInfo["path"].ToString();
            if (buildInRes.idmap_path.EndsWith(".shader"))
            {
				buildInRes.idmap_path = buildInRes.idmap_path + "x";
				buildInRes.fileType = JnityFileType.Shader;
            }else if (buildInRes.idmap_path.EndsWith(".png"))
            {
                buildInRes.fileType = JnityFileType.Texture;
            }else if (buildInRes.idmap_path.EndsWith(".prefab"))
            {
                buildInRes.fileType = JnityFileType.Prefab;
            }else if(buildInRes.idmap_path.EndsWith(".fbx"))
            {
                buildInRes.fileType = JnityFileType.FBX;
            }
            buildInRes.path = Application.dataPath + "/.."+PACKAGE_PATH + buildInRes.idmap_path;

            buildInResMap.Add(buildInRes.uuid, buildInRes);
        }
    }
}

public class ShaderMapping
{
    static Dictionary<string, string> mappingDict = new Dictionary<string, string>();
    static void SplitString(string line)
    {
        string[] value = line.Split(",");
        string jnity_shader_path = value[0];
        string unity_shader_path = value[1];

        mappingDict.Add(jnity_shader_path, unity_shader_path);
    }

    public static void loadMappingFile()
    {
        mappingDict.Clear();
        string path = Application.dataPath + "/.."+ JnityBuildInRes.PACKAGE_PATH + "ShaderMapping.csv";
        if (File.Exists(path))
        {
            string info = File.ReadAllText(path);
            string[] lines = info.Split("\r\n");
            for (int i = 1; i < lines.Length - 1; i++)
            {
                SplitString(lines[i]);
            }
        }
        else
        {
            ReportSystem.OutputLog("工程中没有配置shadermapping文件!");
        }   
    }

    public static string getUnityShaderPath(string jnity_shader_path)
    {
        string unity_shader_path = null;
        mappingDict.TryGetValue(jnity_shader_path, out unity_shader_path);
        return unity_shader_path;
    }
}
