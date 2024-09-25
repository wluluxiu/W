using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;


public class Jnity2Unity : EditorWindow
{
    public static JnityResConfig jnityResConfig;
    public AnimationClip clip;
    private static string jnityProjectPath;
    private static string jnityDirPath;
    public static string unityAssetPath;
    public static string unityRelativeAssetPath;
    private static Vector2 scrollViewPos;
    private static List<string> jnityFilePathList = new List<string>();
    public static bool needFlipZ;
    public static bool rotateY;

	public delegate void InitDynamicBoneCallBack (GameObject owner, Dictionary<object,object> dict);
	public static InitDynamicBoneCallBack _dynamicBoneDelegate = null;
	public static InitDynamicBoneCallBack DynamicBoneDelegate { set { _dynamicBoneDelegate = value; } }

	public delegate void SetDynamicBoneRootCallBack (MonoBehaviour dynamicBone, Transform boneT);
	public static SetDynamicBoneRootCallBack _setBoneRootCallBack = null;
	public static SetDynamicBoneRootCallBack SetDynamicBoneRootDelegate { set { _setBoneRootCallBack = value; } }

	public delegate void AddAttachNodeCallBack (GameObject owner, string attachNodeName);
	public static AddAttachNodeCallBack _addAttachNodeCallBack = null;
	public static AddAttachNodeCallBack SetAddAttachNodeDelegate { set { _addAttachNodeCallBack = value; } }

	enum ResFileType
    {
        Unkonw = 0,
        AnimationClip = 1,
        AnimatorClip = 2,
    }

    public enum ConvertTarget
    {
        Animation = 0,
        Prefab,
        Particle
    }

    const string dialogTitle = "Jnity2Unity Window";
    string[] ConvertResOptions = new string[] { "Animation", "Prefab", "Particle" };
   

    private static void InitWindow()
    {
        Jnity2Unity myWindow = (Jnity2Unity)EditorWindow.GetWindowWithRect(typeof(Jnity2Unity),new Rect(100,100,1000,600), false, dialogTitle);
        myWindow.autoRepaintOnSceneChange = true;
        myWindow.Show(true);
    }

    void OnGUI()
    {
      PrefabGUI();
    }


    void PrefabGUI()
    {
        if (jnityDirPath == null)
            jnityDirPath = EditorUserSettings.GetConfigValue("jnityDirPath");
        if (jnityProjectPath == null)
            jnityProjectPath = EditorUserSettings.GetConfigValue("jnityProjectPath");
        if(unityAssetPath == null)
            unityAssetPath = EditorUserSettings.GetConfigValue("unityAssetPath");
        if (unityRelativeAssetPath == null)
            unityRelativeAssetPath = EditorUserSettings.GetConfigValue("unityRelativeAssetPath");

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Jnity 工程路径:", jnityProjectPath);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("打开"))
        {
            jnityProjectPath = EditorUtility.OpenFolderPanel("选择打开文件", "", "");
            EditorUserSettings.SetConfigValue("jnityProjectPath", jnityProjectPath);
            GUIUtility.ExitGUI();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("刷新工程资源配置"))
        {
            if (jnityProjectPath == null)
            {
                EditorUtility.DisplayDialog(dialogTitle, "请先配置jnity工程路径再点击刷新！", "ok");
                return;
            }
            else
            {
                JnityResScanner.doScanner(jnityProjectPath);
                EditorUtility.DisplayDialog(dialogTitle, "刷新工程配置文件完成！", "ok");
            }

        }
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Unity工程导出目录:", unityAssetPath);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("打开"))
        {
            unityAssetPath = EditorUtility.OpenFolderPanel("选择打开文件", "", "");
            unityAssetPath += "/";
            int startIndex = unityAssetPath.IndexOf("Assets");
            unityRelativeAssetPath = unityAssetPath.Substring(startIndex, unityAssetPath.Length - startIndex);
            EditorUserSettings.SetConfigValue("unityAssetPath", unityAssetPath);
            EditorUserSettings.SetConfigValue("unityRelativeAssetPath", unityRelativeAssetPath);
            GUIUtility.ExitGUI();
        }
        GUILayout.EndHorizontal();


        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("多选Jnity[Prefab/World]文件:");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("打开"))
        {
            string[] selFiles = EditorHelper.OpenFilesPanel("选择打开文件", jnityDirPath, new string[] { "All files (*.*)|*.*" });
            jnityFilePathList.AddRange(selFiles);         
            EditorUserSettings.SetConfigValue("jnityDirPath", jnityDirPath);
            GUIUtility.ExitGUI();

        }
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("已选中文件列表(不超过10个):");
        GUILayout.EndHorizontal();
        scrollViewPos = GUILayout.BeginScrollView(scrollViewPos, GUILayout.Height(200));
        foreach(String jnityFilePath in jnityFilePathList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Button(jnityFilePath, GUILayout.Height(30));
            if (GUILayout.Button("删除", GUILayout.Height(30)))
            {
                jnityFilePathList.Remove(jnityFilePath);
                GUILayout.EndHorizontal();
                break;
            }
            GUILayout.EndHorizontal();

        }
        GUILayout.EndScrollView();

        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("清空选中文件列表"))
        {
            jnityFilePathList.Clear();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("导出成 Unity Prefab/Scene"))
        {
            ReportSystem.OnEnable();
            ConvertStart();
            ReportSystem.OnDisable();
           
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(30);
    }



    [UnityEditor.MenuItem("Tools/Jnity2Unity/ConvertArtRes")]
    public static void ConvertArtResouce()
    {
        InitWindow();

    }

    private static void ConvertStart()
    {
        JnityBuildInRes.loadBuildInIdMap();
        ShaderMapping.loadMappingFile();
        EditorSettingSync.SyncEditorSettings(jnityProjectPath);
		JnityResScanner.ScannerUnityShader(unityAssetPath);

		string unity_config_path = UnityEngine.Application.dataPath + "/JnityResConfig.json";
        FileInfo configFile = new FileInfo(unity_config_path);
        if (!configFile.Exists)
        {
            EditorUtility.DisplayDialog(dialogTitle, "请先刷新jnity工程配置！", "ok");
            return;
        }
        if(jnityFilePathList.Count > 10)
        {
            EditorUtility.DisplayDialog(dialogTitle, "批量导出暂时不支持超过10个文件！", "ok");
            return;
        }
		if (jnityFilePathList.Count == 0)
		{
			EditorUtility.DisplayDialog(dialogTitle, "导出列表中没有选择文件！", "ok");
			return;
		}

		jnityResConfig = JnityResConfig.getJnityResConfig(unity_config_path);
        for(int k = 0; k < jnityFilePathList.Count; k++)
        {
            string fileName = Path.GetFileName(jnityFilePathList[k]);
            if (jnityFilePathList[k].EndsWith("world"))
            {
                ConvertUtil.isSceneConvert = true;
				ConvertArtScene(unity_config_path, jnityFilePathList[k]);
            }
            else
            {
                ConvertUtil.isSceneConvert = false;
                ConvertArtPrefab(unity_config_path, jnityFilePathList[k]);             
            }
            Debug.Log($"导出文件{fileName}成功！");
        }
        EditorUtility.DisplayDialog(dialogTitle, $"批量导出jnity文件完成！", "ok");

    }

    private static void ConvertArtScene(string unity_config_path, string jnityFilePath)
    {
        string prefabFile = jnityFilePath;
        string assetPath = FileUtil.GetAssetPath(jnityFilePath);
        assetPath = assetPath.Replace(".world", ".unity");
        string unityPrefabPath = assetPath;
        string FilePath = Application.dataPath.Replace("Assets", "");
        unityPrefabPath = FilePath + unityPrefabPath;
        FileInfo unityPrefabFile = new FileInfo(unityPrefabPath);
        string dir = unityPrefabFile.Directory.FullName;

        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        GameObject[] gameObjects = newScene.GetRootGameObjects();
        foreach(GameObject inSceneObj in gameObjects)
        {
            GameObject.DestroyImmediate(inSceneObj);
        }
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string text = FileUtil.ReadTexTFile(prefabFile);
        var jsonStr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(text);

        string secneMetaPath = prefabFile + ".meta";
        string metaText = FileUtil.ReadTexTFile(secneMetaPath);
        var metaJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object,object>>(metaText);
        string scneeUUID = metaJson["_uuid"].ToString();

		SceneConvert.ConvertScene(scneeUUID, jsonStr);
		ConvertUtil.ReleaseObjects();
		EditorSceneManager.SaveScene(newScene, assetPath);

    }

   

    private static void ConvertArtPrefab(string unity_config_path, string jnityFilePath)
    {
		//ConvertUtil.particleSystemList.Clear();
		Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
		ConvertUtil.allGameObjects.Clear();
        string prefabMetaPath = jnityFilePath + ".meta";
        string metaText = FileUtil.ReadTexTFile(prefabMetaPath);
        var metaJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(metaText);
        string prefabUUID = metaJson["_uuid"].ToString();
        PrefabConvert.ConvertPrefab(null, prefabUUID, jnityFilePath, true, true);
        ConvertUtil.ReleaseObjects();
        string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(jnityResConfig);
        FileUtil.WriteTextFile(unity_config_path, jsonText);
    }


    //private static void ConvertArtResource()
    //{
    //    ResFileType resFileType = ResFileType.Unkonw;
    //    if (jnityAnimPath.ToLower().EndsWith(".clip"))
    //    {
    //        resFileType = ResFileType.AnimationClip;
    //    }
    //    else if(jnityAnimPath.ToLower().EndsWith(".animatorclip"))
    //    {
    //        resFileType = ResFileType.AnimatorClip;
    //    }

    //    if(resFileType == ResFileType.Unkonw)
    //    {
    //        EditorUtility.DisplayDialog(dialogTitle, "导入的jnity资源文件无法识别,请检查后缀名", "ok");
    //        return;
    //    }

    //    string animationFile = jnityAnimPath;
    //    int index = unityAnimDir.IndexOf("Assets");
    //    string exportDir = unityAnimDir.Substring(index);
    //    string text = FileUtil.ReadTexTFile(animationFile);
    //    var jsonStr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(text);
    //    AnimationClip newClip = null;
    //    switch (resFileType)
    //    {
    //        case ResFileType.AnimationClip:
    //            newClip = AnimationConvert.ConvertAnimationClip(jsonStr);
    //            break;
    //        case ResFileType.AnimatorClip:
    //            newClip = AnimationConvert.ConvertAnimatorClip(jsonStr);
    //            break;
    //    }


    //    if (newClip)
    //    {
    //        string clipPath = unityAnimDir + "/" + newClip.name + ".anim";
    //        if (File.Exists(clipPath))
    //        {
    //            File.Delete(clipPath);
    //        }

    //        clipPath = exportDir + "/" + newClip.name + ".anim";

    //        AssetDatabase.CreateAsset(newClip, clipPath);
    //        AssetDatabase.SaveAssets();
    //        AssetDatabase.Refresh();
    //    }

    //}

   

  
    
}
 