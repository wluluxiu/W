using UnityEngine;
using UnityEditor;
using System.Collections;

public class BuildAssetbundle : Editor
{
	[MenuItem("BuildAssetbundle/BuildAndroid")]
	static void Build()
	{
		BuildPipeline.BuildAssetBundles(Application.dataPath+"/../Assetbundle", BuildAssetBundleOptions.None, BuildTarget.Android);
	}
}
