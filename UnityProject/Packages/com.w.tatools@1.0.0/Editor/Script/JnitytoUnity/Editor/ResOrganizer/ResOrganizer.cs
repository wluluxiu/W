using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEditor.SceneManagement;

public class ResOrganizer : EditorWindow
{
	const string dialogTitle = "ResOrganizer";
	private static string unityResPath;
	private static string unityTargetResPath;
	private static Vector2 scrollViewPos;
	private static List<string> exportFilePaths = new List<string>();


	private static void InitWindow ()
	{
		ResOrganizer myWindow = (ResOrganizer)EditorWindow.GetWindowWithRect(typeof(ResOrganizer), new Rect(100, 100, 1920, 1000), false, dialogTitle);
		myWindow.autoRepaintOnSceneChange = true;
		myWindow.Show(true);
	}

	[UnityEditor.MenuItem("Tools/Jnity2Unity/ResOrganizer")]
	public static void GameResOrganizer ()
	{
		InitWindow();
	}

	private void OnGUI ()
	{
		if (unityResPath == null)
		{
			unityResPath = EditorUserSettings.GetConfigValue("unityResPath");
			ResClassificUtil.LoadMappingTable(unityResPath);
		}
		if (unityTargetResPath == null)
			unityTargetResPath = EditorUserSettings.GetConfigValue("unityTargetResPath");

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Unity 导出资源路径:", unityResPath);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("打开"))
		{
			unityResPath = EditorUtility.OpenFolderPanel("选择打开文件", "", "");
			EditorUserSettings.SetConfigValue("unityResPath", unityResPath);
			GUIUtility.ExitGUI();
		}
		GUILayout.EndHorizontal();


		GUILayout.Space(30);
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Unity 工程目标资源目录:", unityTargetResPath);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("打开"))
		{
			unityTargetResPath = EditorUtility.OpenFolderPanel("选择打开文件", "", "");
			EditorUserSettings.SetConfigValue("unityTargetResPath", unityTargetResPath);
			GUIUtility.ExitGUI();
		}
		GUILayout.EndHorizontal();


		GUILayout.Space(30);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("生成资源分类映射表"))
		{
			ScannerPrefabFile(unityResPath);

		}
		GUILayout.EndHorizontal();

		GUILayout.Space(30);
		GUILayout.BeginHorizontal();
		GUILayout.Label("原资源路径", GUILayout.Width(1000));
		GUILayout.Label("目标一级目录", GUILayout.Width(300));
		GUILayout.Label("目标二级目录", GUILayout.Width(300));
		GUILayout.Label("目标三级目录", GUILayout.Width(300));
		GUILayout.EndHorizontal();

		//string[] targetDirLevel1 = Directory.GetDirectories(unityTargetResPath);
		DirectoryInfo targetFileInfo = null;
		if (unityTargetResPath != null)
			targetFileInfo = new DirectoryInfo(unityTargetResPath);
		if (targetFileInfo.Exists)
		{
			(string[] targetDirNamesLevel1, string[] targetDirFullNamesLevel1) = GetSubDiretories(unityTargetResPath);
			scrollViewPos = GUILayout.BeginScrollView(scrollViewPos, GUILayout.Height(400));
			Dictionary<string, ResItem> mappingTable = ResClassificUtil.GetMappingTable();
			foreach (string prefabPath in exportFilePaths)
			{
				ResItem item = mappingTable[prefabPath];
				GUILayout.BeginHorizontal();
				GUILayout.Label(prefabPath, GUILayout.Width(1000));
				int level1Index = getDirIndex(targetDirNamesLevel1, item.targetDirLevel1);
				level1Index = EditorGUILayout.Popup(level1Index, targetDirNamesLevel1, GUILayout.Width(300));
				if (level1Index != -1)
					item.targetDirLevel1 = targetDirNamesLevel1[level1Index];

				int level2Index = -1;
				string[] targetDirNamesLevel2 = null;
				string[] targetDirFullNamesLevel2 = null;
				if (level1Index != -1)
				{
					(targetDirNamesLevel2, targetDirFullNamesLevel2) = GetSubDiretories(targetDirFullNamesLevel1[level1Index]);
					level2Index = getDirIndex(targetDirNamesLevel2, item.targetDirLevel2);
					level2Index = EditorGUILayout.Popup(level2Index, targetDirNamesLevel2, GUILayout.Width(300));
					if (level2Index != -1)
						item.targetDirLevel2 = targetDirNamesLevel2[level2Index];
				}
				else
				{
					EditorGUILayout.Popup(level2Index, new string[] { });
				}
				int level3Index = -1;
				string[] targetDirNamesLevel3;
				string[] targetDirFullNamesLevel3;
				if (level2Index != -1)
				{
					(targetDirNamesLevel3, targetDirFullNamesLevel3) = GetSubDiretories(targetDirFullNamesLevel2[level2Index]);
					level3Index = getDirIndex(targetDirNamesLevel3, item.targetDirLevel3);
					level3Index = EditorGUILayout.Popup(level3Index, targetDirNamesLevel3, GUILayout.Width(300));
					if (level3Index != -1)
						item.targetDirLevel3 = targetDirNamesLevel3[level3Index];
				}
				else
				{
					EditorGUILayout.Popup(level3Index, new string[] { });
				}
				GUILayout.EndHorizontal();

			}
			GUILayout.EndScrollView();
		}

		GUILayout.Space(30);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save Table", GUILayout.Width(200)))
		{
			ResClassificUtil.SaveMappingTable(unityResPath);
		}
		if (GUILayout.Button("Export", GUILayout.Width(200)))
		{
			ResClassificUtil.onlyCreateDirectory = true;
			ExportToTargetDir(unityTargetResPath);
			ResClassificUtil.onlyCreateDirectory = false;
			ExportToTargetDir(unityTargetResPath);
		}
		GUILayout.EndHorizontal();


	}

	static void ScannerPrefabFile (string unity_dir_path)
	{
		DirectoryInfo dir = Directory.CreateDirectory(unity_dir_path);
		if (!dir.Exists)
			return;
		exportFilePaths.Clear();
		var prefabFileInfos = dir.GetFiles("*.*", SearchOption.AllDirectories)
			.Where(s => s.FullName.EndsWith(".prefab") || s.FullName.EndsWith(".unity"));
		foreach (FileInfo fileInfo in prefabFileInfos)
		{
			string fullPath = ResClassificUtil.ChangePathCharater(fileInfo.FullName);
			ResClassificUtil.AddResItem(fullPath);
			exportFilePaths.Add(fullPath);
		}
	}

	static (string[], string[]) GetSubDiretories (string path)
	{
		string[] targetDirLevel1 = Directory.GetDirectories(path);
		string[] dirNames = new string[targetDirLevel1.Length];
		string[] fullDirNames = new string[targetDirLevel1.Length];
		for (int i = 0; i < targetDirLevel1.Length; i++)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(targetDirLevel1[i]);
			dirNames[i] = dirInfo.Name;
			fullDirNames[i] = dirInfo.FullName;
		}
		return (dirNames, fullDirNames);
	}


	static int getDirIndex (string[] dir, string dirName)
	{
		for (int i = 0; i < dir.Length; i++)
		{
			if (dir[i].Equals(dirName))
				return i;
		}

		return -1;

	}

	static void ExportToTargetDir (string unityResRootDir)
	{
		ResClassificUtil.exportedAssets.Clear();
		Dictionary<string, ResItem> mappingTable = ResClassificUtil.GetMappingTable();
		ResClassificUtil.LoadExportResTable();
		var sortedFiles = exportFilePaths.OrderBy(f => Path.GetExtension(f)).ToList();
		foreach (string filePath in sortedFiles)
		{
			//if (ResClassificUtil.IsInExportResTable(filePath))
			ExportDepAssets("", filePath, unityResRootDir, mappingTable, null, null);
		}
		AssetDatabase.Refresh();
	}

	static void ExportDepAssets (string prefabName, string originFilePath, string unityResRootDir, Dictionary<string, ResItem> mappingTable, ResItem resItem, string prefabPath)
	{

		if (originFilePath.EndsWith(".prefab") || originFilePath.EndsWith(".unity"))
		{
			string prefabAssetPath = ResClassificUtil.GetAssetPath(originFilePath);
			if (originFilePath.EndsWith(".unity"))
			{
				EditorSceneManager.OpenScene(prefabAssetPath, OpenSceneMode.Single);
			}
			prefabName = Path.GetFileNameWithoutExtension(originFilePath);
			if (!mappingTable.ContainsKey(originFilePath))
				return;


			resItem = mappingTable[originFilePath];
			string[] depAssetPaths = AssetDatabase.GetDependencies(prefabAssetPath, true);
			//foreach (string depPath in depAssetPaths)
			//	Debug.Log(prefabAssetPath + " depends:" + depPath);
			string uniquePrefabPath = ExportAssets(resItem, prefabName, originFilePath, unityResRootDir, mappingTable, null);
			for (int k = 0; k < depAssetPaths.Length; k++)
			{
				if (prefabAssetPath == depAssetPaths[k])
					continue;
				string fullAssetPath = Application.dataPath + depAssetPaths[k].Replace("Assets", "");
				if (depAssetPaths[k].EndsWith(".prefab") || depAssetPaths[k].EndsWith(".unity"))
				{

					ExportDepAssets(prefabName, fullAssetPath, unityResRootDir, mappingTable, resItem, null);
				}
				else
				{
					ExportAssets(resItem, prefabName, fullAssetPath, unityResRootDir, mappingTable, uniquePrefabPath);
				}
			}


		}
		else if (originFilePath.EndsWith(".controller"))
		{
			string prefabAssetPath = ResClassificUtil.GetAssetPath(originFilePath);
			string[] depAssetPaths = AssetDatabase.GetDependencies(prefabAssetPath, true);
			ExportAssets(resItem, prefabName, originFilePath, unityResRootDir, mappingTable, prefabPath);
			for (int k = 0; k < depAssetPaths.Length; k++)
			{
				string fullAssetPath = Application.dataPath + depAssetPaths[k].Replace("Assets", "");
				ExportAssets(resItem, prefabName, fullAssetPath, unityResRootDir, mappingTable, prefabPath);

			}
			AssetDatabase.Refresh();
		}
	}

	static string UnChangeExportAssets (string assetPath, string unityResRootDir, bool onlyCreateDirectory)
	{
		string exportPath;
		if (assetPath.Contains("3d/effectres"))
		{
			int startindex = assetPath.IndexOf("3d/effectres");
			string apath = assetPath.Substring(startindex, assetPath.Length - startindex);
			exportPath = unityResRootDir + "/" + apath;
		}
		else
		{
			
			return string.Empty;
		}
		string srcAssetPath = ResClassificUtil.GetAssetPath(assetPath);
		string destAssetPath = ResClassificUtil.GetAssetPath(exportPath);
		FileInfo fileInfo = new FileInfo(assetPath);
		FileInfo fileInfo2 = new FileInfo(exportPath);
		string dir = fileInfo2.Directory.FullName;
		if (!fileInfo.Exists)
			return exportPath;
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
			AssetDatabase.Refresh();
		}
		if (onlyCreateDirectory)
			return exportPath;

		if (srcAssetPath != destAssetPath)
		{
			if (!ResClassificUtil.exportedAssets.ContainsKey(destAssetPath) && fileInfo2.Exists)
			{
				Debug.Log("Assets already exist,Delete Assets:" + fileInfo2.FullName);
				fileInfo2.Delete();
			}

			string error = AssetDatabase.MoveAsset(srcAssetPath, destAssetPath);
			if (!error.Equals(string.Empty))
			{
				Debug.LogError("Move Asset error :" + error + "， srcAsset :" + srcAssetPath + "  destAssetPath:" + destAssetPath);
			}
			else
			{
				Debug.Log("Move Asset  srcAsset :" + srcAssetPath + "  destAssetPath:" + destAssetPath);
				ResClassificUtil.exportedAssets.Add(destAssetPath, srcAssetPath);
			}

		}
		return exportPath;

	}



	static string ExportAssets (ResItem resItem, string prefabName, string assetPath, string unityResRootDir, Dictionary<string, ResItem> mappingTable, string prefabPath)
	{
		if (assetPath.Contains(unityResRootDir))  //如果要导出的资源已经在目标路径下，则不做处理
		{
			return assetPath;
		}

		string targetPath = UnChangeExportAssets(assetPath, unityResRootDir, ResClassificUtil.onlyCreateDirectory);
		if (!targetPath.Equals(string.Empty))
			return targetPath;

		bool isAutoFilter = true;
		string targetResDir;
		ResItem.UnityFileType fileType = ResItem.UnityFileType.Max;
		if (isAutoFilter)
		{
			if (prefabPath != null)
				targetResDir = AutoGetExportPath(unityResRootDir, prefabPath);
			else
			{

				if (assetPath.Contains("3d/prefabs"))
				{				
					string dir = Path.GetDirectoryName(assetPath);
					dir = ResClassificUtil.ChangePathCharater(dir);
					int startindex = dir.IndexOf("3d/prefabs");
					string apath = assetPath.Substring(startindex, dir.Length - startindex);
					targetResDir = unityResRootDir + "/" + apath;
				}else if (assetPath.Contains("3D/YXW_Sub") && !assetPath.EndsWith(".unity"))
				{
					string dir = Path.GetDirectoryName(assetPath);
					dir = ResClassificUtil.ChangePathCharater(dir);
					int startindex = dir.IndexOf("3D/YXW_Sub");
					string apath = assetPath.Substring(startindex + 10, dir.Length - startindex - 10 );
					targetResDir = unityResRootDir + "/YXW_Sub" + apath;
				}
				else
				{
					targetResDir = AutoGetExportPath(unityResRootDir, assetPath);
				}
			}

			
		}
		else
		{
			targetResDir = ResClassificUtil.getTargetResDir(unityResRootDir, resItem.targetDirLevel1, resItem.targetDirLevel2, resItem.targetDirLevel3);
		}
		string fileName = Path.GetFileName(assetPath);
		if (ResClassificUtil.IsTexture(assetPath))
		{
			fileType = ResItem.UnityFileType.Texture;
		}
		else if (assetPath.ToLower().EndsWith(".fbx"))
		{
			fileType = ResItem.UnityFileType.FBX;
		}
		else if (assetPath.ToLower().EndsWith(".mat"))
		{
			if (assetPath.Contains("pipelines.universal"))//内置材质球不处理
				return string.Empty;
			fileType = ResItem.UnityFileType.Mat;
		}
		else if (assetPath.ToLower().EndsWith(".shader"))
		{
			if (assetPath.Contains("pipelines.universal"))//内置shader不处理
				return string.Empty;
			fileType = ResItem.UnityFileType.Shader;
			string shaderFileName = Path.GetFileName(assetPath);
			string exportShaderPath = unityResRootDir + "/Shader/" + shaderFileName;
			return ResClassificUtil.CopyFile(assetPath, exportShaderPath, fileType, false);
			//return string.Empty;
		}
		else if (assetPath.ToLower().EndsWith(".anim"))
		{
			fileType = ResItem.UnityFileType.Animation;
		}
		else if (assetPath.ToLower().EndsWith(".controller"))
		{
			fileType = ResItem.UnityFileType.AnimController;
		}
		else if (assetPath.ToLower().EndsWith(".prefab") || assetPath.ToLower().EndsWith(".unity"))
		{
			if(assetPath.ToLower().EndsWith(".prefab"))
				fileType = ResItem.UnityFileType.Prefab;
			else
				fileType = ResItem.UnityFileType.Scene;
			string exportPrefabPath = targetResDir + "/" + prefabName + "/" + fileName;
			FileInfo fileInfo = new FileInfo(exportPrefabPath);
			if (fileInfo.Exists)
			{
				DirectoryInfo curDirInfo = Directory.GetParent(exportPrefabPath);
				exportPrefabPath = targetResDir + "/" + prefabName + "/" + fileName;
			}
			return ResClassificUtil.CopyFile(assetPath, exportPrefabPath, fileType);
		}
		string prefabDir = Path.GetDirectoryName(prefabPath);
		string exportPath = prefabDir + "/" + ResItem.ResTypeDirNames[(int)fileType] + "/" + fileName;
		if (prefabPath!=null && prefabPath.EndsWith(".unity") && fileType == ResItem.UnityFileType.Animation)
		{
			string assetDir = Path.GetDirectoryName(assetPath);
			string dirName = new DirectoryInfo(assetDir).Name;
			exportPath = prefabDir + "/" + ResItem.ResTypeDirNames[(int)fileType] + "/" + dirName +"/" + fileName;
		}
		
		return ResClassificUtil.CopyFile(assetPath, exportPath, fileType);
	}


	static string AutoGetExportPath (string targetResDir, string originPath)
	{
		string exportPath;
		if (originPath.ToLower().Contains("effect"))
		{
			exportPath = targetResDir + "/Effect";
		}
		else if (originPath.ToLower().Contains("model"))
		{
			exportPath = targetResDir + "/Model";
		}
		else if (originPath.ToLower().EndsWith(".unity"))
		{
			exportPath = targetResDir + "/Scene";
		}
		else
		{
			exportPath = targetResDir + "/3D";
		}
		return exportPath;
	}

}
