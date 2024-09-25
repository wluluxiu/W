using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using System;
using System.Security.Cryptography;
using System.Text;

public class ResItem
{

	public enum UnityFileType
	{
		FBX,
		Mat,
		Shader,
		Texture,
		Animation,
		AnimController,
		Prefab,
		FontAsset,
		SystemFont,
		Scene,
		Max
	}

	public static string[] ResTypeDirNames = new string[] { "Fbx", "Mat", "Shader", "Texture", "Animation", "AnimController", "Prefab","","","Scene","Unknown" }; 



	public string unityOriginPath;
	public string targetDirLevel1;
	public string targetDirLevel2;
	public string targetDirLevel3;

	public ResItem(string _unityOriginPath, string _targetDirLevel1, string _targetDirLevel2, string _targetDirLevel3)
	{
		unityOriginPath = _unityOriginPath;
		targetDirLevel1 = _targetDirLevel1;
		targetDirLevel2 = _targetDirLevel2;
		targetDirLevel3 = _targetDirLevel3;
	}
}



public class ResClassificUtil 
{
	public static bool onlyCreateDirectory = true;
	public static Dictionary<string, ResItem> mappingTable = new Dictionary<string, ResItem>();
	public static Dictionary<string, string> exportedAssets = new Dictionary<string, string>();
	public static List<string> exportResList = new List<string>();
	const string PACKAGE_PATH = "/Packages/com.jj.tatools@1.0.0/Editor/Script/JnitytoUnity/";
	public static void AddResItem(string prefabPath)
	{
		if (!mappingTable.ContainsKey(prefabPath)){
			ResItem item = new ResItem(prefabPath, string.Empty, string.Empty, string.Empty);
			mappingTable.Add(prefabPath, item);
		}
	}

	public static Dictionary<string, ResItem> GetMappingTable ()
	{
		return mappingTable;
	}

	public static void SaveMappingTable (string jnityProjectPath)
	{
		string tablePath = jnityProjectPath + "/ResMappingTable.json";
		string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(mappingTable);
		FileUtil.WriteTextFile(tablePath, jsonStr);
	}

	public static void LoadMappingTable (string jnityProjectPath)
	{
		string tablePath = jnityProjectPath + "/ResMappingTable.json";
		FileInfo file = new FileInfo(tablePath);
		if (!file.Exists)
			return;
		var jsonStr = FileUtil.ReadTexTFile(tablePath);
		mappingTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,ResItem>>(jsonStr);
	}

	public static void LoadExportResTable ()
	{
		exportResList.Clear();
		string path =  Application.dataPath + "/.." + PACKAGE_PATH + "ExportResTable.csv";
		if (File.Exists(path))
		{
			string info = File.ReadAllText(path);
			string[] lines = info.Split("\r\n");
			for (int i = 0; i < lines.Length - 1; i++)
			{
				string apath = ChangePathCharater(lines[i]);
				if(!lines[i].Equals(string.Empty))
				    exportResList.Add(apath);
			}
		}
	}

	public static bool IsTexture(string assetPath)
	{
		if(assetPath.ToLower().EndsWith(".png") || assetPath.ToLower().EndsWith(".jpg") || assetPath.ToLower().EndsWith(".tga"))
		{
			return true;
		}
		return false;
	}

	public static string getTargetResDir(string unityTargetPath, string targetLevel1Dir, string targetLevel2Dir, string targetLevel3Dir)
	{
		if (targetLevel1Dir.Equals(string.Empty))
		{
			return unityTargetPath + "/" + "DefaultResDir";
		}
		else if(targetLevel2Dir.Equals(string.Empty))
		{
			return unityTargetPath + "/" + targetLevel1Dir;
		}
		else if(targetLevel3Dir.Equals(string.Empty))
		{
			return unityTargetPath + "/" + targetLevel1Dir + "/" + targetLevel2Dir;
		}
		else
		{
			return unityTargetPath + "/" + targetLevel1Dir + "/" + targetLevel2Dir + "/" + targetLevel3Dir;
		}
	}

	public static string CopyFile(string originAssetPath, string targetAssetPath, ResItem.UnityFileType fileType, bool deleteDest = true)
	{
		string finalTargetPath = targetAssetPath;
		FileInfo fileInfo = new FileInfo(originAssetPath);
		FileInfo fileInfo2 = new FileInfo(targetAssetPath);
		string dir = fileInfo2.Directory.FullName;
		if (!fileInfo.Exists)
			return finalTargetPath;
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
			AssetDatabase.Refresh();
		}
		if (onlyCreateDirectory)
			return finalTargetPath;
		string srcAssetPath = GetAssetPath(originAssetPath);
		string destAssetPath = GetAssetPath(targetAssetPath);
	
		if (srcAssetPath != destAssetPath)
		{
			if (deleteDest && !exportedAssets.ContainsKey(destAssetPath) && fileInfo2.Exists)
			{
				Debug.Log("Assets already exist,Delete Assets:" + fileInfo2.FullName);
				fileInfo2.Delete();
			}

			if (exportedAssets.ContainsKey(destAssetPath))
			{
				string fileName = Path.GetFileName(destAssetPath);
				string fileNameNoExt = Path.GetFileNameWithoutExtension(destAssetPath);
				string directory = Path.GetDirectoryName(destAssetPath);
				directory = ChangePathCharater(directory);
				if (fileType == ResItem.UnityFileType.Prefab || fileType == ResItem.UnityFileType.Scene)
				{
					int lastIndex = directory.LastIndexOf("/");
					directory = directory.Substring(0, lastIndex);
				}
				string srcDirectory = Path.GetDirectoryName(srcAssetPath);
				DirectoryInfo srcDirInfo = new DirectoryInfo(srcDirectory);
				string dir1 = srcDirInfo.Name;
				string dir2 = srcDirInfo.Parent.Name;
				destAssetPath = directory + "/" + fileNameNoExt + "_" + dir1 + "_"+dir2 + "/" + fileName;
				//string md5Name = GetMD5(srcAssetPath);
				FileInfo newFileInfo = new FileInfo(Application.dataPath.Replace("Assets", "") + "/" + destAssetPath);
				Directory.CreateDirectory(newFileInfo.Directory.FullName);
				AssetDatabase.Refresh();
				finalTargetPath = newFileInfo.FullName;

			}
			string error = AssetDatabase.MoveAsset(srcAssetPath, destAssetPath);
			if (!error.Equals(string.Empty))
			{
				Debug.LogError("Move Asset error :" + error + "， srcAsset :" + srcAssetPath + "  destAssetPath:" + destAssetPath);
			}
			else
			{
				Debug.Log("Move Asset  srcAsset :" + srcAssetPath + "  destAssetPath:" + destAssetPath);
				exportedAssets.Add(destAssetPath, srcAssetPath);
			}


			//AssetDatabase.Refresh();

		}
		return finalTargetPath;
	}

	public static string GetAssetPath(string path)
	{
		string assetPath;
		int startIndex = path.ToLower().IndexOf("assets");
		assetPath = path.Substring(startIndex, path.Length - startIndex);
		return assetPath;
	}


	public static  void OrganizeDependencies (string assetPath, string unityResRootPath)
	{
		var dependencies = AssetDatabase.GetDependencies(assetPath, true);
		var categorizedDependencies = new Dictionary<string, List<string>>();

		foreach (var dependency in dependencies)
		{
			if (dependency == assetPath) continue; 

			var extension = Path.GetExtension(dependency).ToLower();
			if (extension == ".png" || extension == ".jpg")
				extension = "texture";
			if (!categorizedDependencies.ContainsKey(extension))
				categorizedDependencies[extension] = new List<string>();
			categorizedDependencies[extension].Add(dependency);
		}

		var assetDirectory = Path.GetDirectoryName(assetPath);
		var assetName = Path.GetFileNameWithoutExtension(assetPath);
		var mainFolder = Path.Combine(assetDirectory, assetName);

		if (!Directory.Exists(mainFolder))
			Directory.CreateDirectory(mainFolder);

		foreach (var category in categorizedDependencies)
		{
			var categoryFolder = Path.Combine(mainFolder, category.Key.TrimStart('.'));
			if (!Directory.Exists(categoryFolder))
				Directory.CreateDirectory(categoryFolder);

			foreach (var dependency in category.Value)
			{
				var fileName = Path.GetFileName(dependency);
				var destinationPath = Path.Combine(categoryFolder, fileName);

				if (category.Key == ".controller")
				{
					MoveControllerAnimations(dependency, mainFolder);
				}else if(category.Key == ".shader"){

					destinationPath = unityResRootPath + "/Shader/" + fileName;
				}
				FileInfo fileInfo = new FileInfo(destinationPath);
				if (!Directory.Exists(fileInfo.DirectoryName))
				{
					Directory.CreateDirectory(fileInfo.DirectoryName);
				}
				AssetDatabase.MoveAsset(dependency, destinationPath);
			}
		}

		AssetDatabase.Refresh();
		Debug.Log("Dependencies organized successfully.");
	}


	static void MoveControllerAnimations (string controllerPath, string assetDirectory)
	{
		var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
		if (controller == null) return;

		var animationsFolder = Path.Combine(assetDirectory, "animation");
		if (!Directory.Exists(animationsFolder))
			Directory.CreateDirectory(animationsFolder);

		foreach (var layer in controller.layers)
		{
			foreach (var state in layer.stateMachine.states)
			{
				var motion = state.state.motion;
				if (motion is AnimationClip)
				{
					var animationPath = AssetDatabase.GetAssetPath(motion);
					var animationFileName = Path.GetFileName(animationPath);
					var destinationPath = Path.Combine(animationsFolder, animationFileName);
					AssetDatabase.MoveAsset(animationPath, destinationPath);
				}
			}
		}
	}

	public static string ChangePathCharater(string path)
	{
		return path.Replace("\\", "/");
	}

	public static string GetMD5 (string source)
	{
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
		byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
		md5.Clear();

		string destString = string.Empty;
		for (int i = 0; i < md5Data.Length; i++)
		{
			destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
		}
		destString = destString.PadLeft(32, '0');
		return destString;
	}

	public static bool IsInExportResTable(string assetPath)
	{
		for(int i = 0; i < exportResList.Count; i++)
		{
			string path = exportResList[i];
			if (assetPath.ToLower().Contains(path.ToLower()))
			{
				return true;
			}
		}
		return false;
	}
                        
}
