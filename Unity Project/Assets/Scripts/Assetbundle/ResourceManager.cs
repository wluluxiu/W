using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {
	private static AssetBundle manifestBundle;
	public static ResourceManager instance;
	void Awake(){
		instance = this;
		DontDestroyOnLoad(this);
		manifestBundle = AssetBundle.LoadFromFile(Application.dataPath +"/../Assetbundle/Assetbundle");
	}

	public static T InstantiateResourcesLoad<T>(string path, UnityAction callback = null ) where T:UnityEngine.Object {
		T obj = Instantiate(Resources.Load<T>( path ));
		if(callback != null){
			callback();
		}
		return obj;
	}

	public static GameObject InstantiateAssetbundleLoad(string name, UnityAction callback = null){
		if(manifestBundle!=null){
			AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
			string[] prafabDepends = manifest.GetAllDependencies("assets/resources/"+ name + ".prefab");
			AssetBundle[] dependsAssetbundle = new AssetBundle[prafabDepends.Length];
			for(int index = 0;index<prafabDepends.Length;index++){
				dependsAssetbundle[index] = AssetBundle.LoadFromFile(Application.dataPath +"/../Assetbundle/" +prafabDepends[index]);
			}
			AssetBundle prefabBundle = AssetBundle.LoadFromFile(Application.dataPath +"/../Assetbundle/assets/resources/" + name + ".prefab" );
			Object obj = prefabBundle.LoadAsset (name);
			if (obj != null) {
				GameObject gobj = (GameObject)Instantiate (obj);
				prefabBundle.Unload (false);
				return gobj;
			} else {
				return null;
			}
		}
		else {
			return null;
		}
	}
}
