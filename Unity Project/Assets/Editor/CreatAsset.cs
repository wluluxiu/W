using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using DG.Tweening;

public enum TweenType
{
	Open = 0,
	Close,
}

[Serializable]
public class CustomTween : ScriptableObject {

	public TweenType tweenType = TweenType.Open;
	public Tweener tweener = null;
}


public class CreateAsset : Editor {

	// 在菜单栏创建功能项
	[MenuItem("CreateAsset/Asset")]
	static void Create()
	{
		// 实例化类  Bullet
		ScriptableObject ct = ScriptableObject.CreateInstance<CustomTween>();

		// 如果实例化 Bullet 类为空，返回
		if (!ct)
		{
			Debug.LogWarning("Bullet not found");
			return;
		}

		// 自定义资源保存路径
		string path = Application.dataPath + "/Resources/Build/Load_PreLoad/G_TweenAsset";

		// 如果项目总不包含该路径，创建一个
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		//将类名 Bullet 转换为字符串
		//拼接保存自定义资源（.asset） 路径
		path = string.Format("Assets/Resources/Build/Load_PreLoad/G_TweenAsset/{0}.asset", (typeof(CustomTween).ToString()));

		// 生成自定义资源到指定路径
		AssetDatabase.CreateAsset(ct, path);
	}

	[MenuItem("CreateAsset/GetAsset")]
	static void GetAsset()
	{
		//读取 .asset 文件, 直接转换为 类  Bullet
		CustomTween ct = AssetDatabase.LoadAssetAtPath<CustomTween>("Assets/BulletAeeet/CustomTween.asset");

		// 打印保存的数据
		Debug.Log("TweenType  :" + Enum.GetName(typeof(CustomTween), ct.tweenType));

		if (ct.tweener != null)
		{
			Debug.Log("Tweener   :" + ct.tweener);
		}
	}

}

