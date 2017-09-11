using UnityEngine;  
using System.Collections;  
using UnityEditor;  

public class AutoSetAssetBundleName :AssetPostprocessor   
{  

	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
	{
		foreach (var str in importedAssets)
		{
			Debug.Log("Reimported Asset: " + str);
			if(!str.EndsWith(".cs"))
			{
				AssetImporter importer=AssetImporter.GetAtPath(str);
				importer.assetBundleName=str;
				Debug.Log("AssetBundleName : " + str);
            }
		}

		foreach (var str in deletedAssets) 
		{
			Debug.Log("Deleted Asset: " + str);
        }
		
		for (var i=0; i<movedAssets.Length; i++)
		{
			Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
		}
	}
}  