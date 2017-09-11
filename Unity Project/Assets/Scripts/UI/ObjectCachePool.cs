using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolCacheInfo
{
	public string key;
	public Stack<ObjectCache> objects;
	public GameObject prefabObj = null;
	private float lastUseTime_ = 0;
	private Transform root;

	public PoolCacheInfo(string key, GameObject prefab, Transform root )
	{
		this.key = key;
		this.root = root;
		prefabObj = prefab;
		objects = new Stack<ObjectCache>();
	}

//	public void Update()
//	{
//		if ( lastUseTime_ != -1 && Time.time - lastUseTime_ > ClientConfig.RES_RELEASE_DELAY_TIME )
//		{
//			// release cache if time out.
//			Release();
//		}
//	}

	public void checkCache()
	{
//		Debug.Log(string.Format("[checkCache] {0} left cout:{1}", this.key, objects.Count));
	}

	public void Release()
	{
		ObjectCache oc;
		while( objects.Count != 0 )
		{
			oc = objects.Pop();
			if ( oc != null )
			{
				oc.objDisable(null);
				Object.Destroy( oc.gameObject );
	//			TankUtils.tkuDestroy(objects.Pop());
				Debug.LogError( string.Format( "Pool Cache Info -> {0} clean 1, left {1}.", key, objects.Count ) );
			}
		}
		objects.Clear();
	}
	
	public ObjectCache New( Transform parent, Vector3 localposition )
	{
		ObjectCache result = null;
		while( objects.Count > 0 && result == null )
		{
			result = objects.Pop();
			if ( result != null )
			{
				if ( parent != null )
					result.mTrans.rotation = parent.rotation;
				result.objEnable();
			}

		}
		if ( result == null )
		{
			GameObject go = MonoBehaviour.Instantiate( prefabObj ) as GameObject;
#if UNITY_EDITOR
			Renderer[] renders = go.GetComponentsInChildren<Renderer>();
			for( int i = 0; i < renders.Length; ++i )
			{
				Material[] shaderMaterials = renders[i].sharedMaterials;
				for( int j = 0; j < shaderMaterials.Length; ++j )
				{
					if ( shaderMaterials[j] != null )
						shaderMaterials[j].shader = Shader.Find(shaderMaterials[j].shader.name);
				}
			}
#endif
			result = go.GetComponent<ObjectCache>();
			result.objInit( key );
		}
		if ( result != null )
		{

			if ( parent != null )
				result.mTrans.SetParent( parent, false );
			result.mTrans.localPosition = localposition;
			result.gameObject.SetActive(true);
		}
		lastUseTime_ = -1;
		return result;
	}
	
	public void Store( ObjectCache oc )
	{
		lastUseTime_ = Time.time;
		oc.objDisable( null );
		GameObject gobj = GameObject.Find("CachePool");
		if( gobj != null){
			Transform trans = gobj.transform;
			oc.mTrans.SetParent( trans, false );
		}
		if ( oc.gameObject.activeInHierarchy )
		{
			oc.gameObject.SetActive(false);
		}
//		Debug.Log("Pool Cache store::: " + oc.name);
		objects.Push( oc );
	}
}

public class ObjectCachePool: MonoBehaviour
{
	[System.Serializable]
	public struct DebugCacheInfo
	{
		public string name;
		public int count;
	}
	static private ObjectCachePool instance_ = null;
	static public ObjectCachePool instance { get{ return instance_; } }
	private Dictionary< string, PoolCacheInfo > pools_ = new Dictionary<string, PoolCacheInfo>();


	private Transform mTrans_ = null;
	#if UNITY_EDITOR
	public List<DebugCacheInfo> debugPoolMap = new List<DebugCacheInfo>();
	#endif

	void Awake()
	{
		if ( instance_ == null )
		{
			instance_ = this;
			GameObject go = new GameObject("CachePool");
			mTrans_ = go.transform;
			mTrans_.SetParent( transform, false );
		}
		else
		{
//			Logger.Log (string.Format("DDestory----> {0}",name),ClientConfig.LOG_COLOR_ERROR);
			Destroy( gameObject );
		}
	}

//	#if UNITY_EDITOR
//	void LateUpdate()
//	{
//		if ( Time.frameCount % 5 == 0 )
//		{
//			debugPoolMap.Clear();
//			string[] keys = new string[ pools_.Keys.Count ];
//			pools_.Keys.CopyTo(keys, 0 );
//			for( int i = 0; i < keys.Length; ++i )
//			{
//				pools_[keys[i]].Update();
//				DebugCacheInfo debugInfo;
//				debugInfo.name = keys[i];
//				debugInfo.count = pools_[keys[i]].objects.Count;
//				debugPoolMap.Add(debugInfo);
//			}
//		}
//	}
//	#endif

	void OnDestroy()
	{
		//ocpRelease();
	}

	public void ocpCheck()
	{
		string[] keys = new string[ pools_.Keys.Count ];
		pools_.Keys.CopyTo(keys, 0 );
		for( int i = 0; i < keys.Length; ++i )
		{
			pools_[keys[i]].checkCache();
		}

//		Texture[] holdTextures =  Resources.FindObjectsOfTypeAll<Texture>();
//		string logStr = "";
//		for( int i = 0; i < holdTextures.Length; ++i )
//		{
//			logStr += holdTextures[i].name + "\n";
//		}
//		Debug.Log("--> used textures: \n" + logStr);
	}

	public void ocpRelease()
	{
		PoolCacheInfo[] cacheInfos = new PoolCacheInfo[pools_.Count];
		pools_.Values.CopyTo(cacheInfos,0);
		for( int i = cacheInfos.Length-1; i >= 0; i-- )
		{
			cacheInfos[i].Release();
			cacheInfos[i] = null;
		}
		pools_.Clear();
	}

	public GameObject ocpNew( string key, GameObject prefab, Transform parent, Vector3 localposition )
	{
		PoolCacheInfo pci = null;
		if ( !pools_.TryGetValue( key, out pci ) || pci.objects.Count == 0 )
		{
			pci = new PoolCacheInfo( key, prefab, mTrans_ );
			pools_[key] = pci;
		}
		return pci.New( parent, localposition ).gameObject;
	}

	public T ocpNew<T>( string key, GameObject prefab, Transform parent, Vector3 localposition ) where T:ObjectCache
	{
		PoolCacheInfo pci = null;
		if ( !pools_.TryGetValue( key, out pci ) || pci.objects.Count == 0 )
		{
			pci = new PoolCacheInfo( key, prefab, mTrans_ );
			pools_[key] = pci;
		}
		return (T)pci.New( parent, localposition );
	}

	public ObjectCache ocpNewDefault( string key, GameObject prefab, Transform parent, Vector3 localposition )
	{
		PoolCacheInfo pci = null;
		if ( !pools_.TryGetValue( key, out pci ) || pci.objects.Count == 0 )
		{
			pci = new PoolCacheInfo( key, prefab, mTrans_ );
			pools_[key] = pci;
		}
		return (ObjectCache)pci.New( parent, localposition );
	}

//	public bool Store( GameObject go )
//	{
//		if ( go == null )
//			return false;
//		PoolCacheInfo poolCacheInfo = null;
//		ObjectCache objCache = go.GetComponent<ObjectCache>();
//		if ( objCache == null || !objCache.isUsing )
//			return false;
//		if ( objCache != null )
//			pools_.TryGetValue( objCache.objKey, out poolCacheInfo );
//		if ( poolCacheInfo != null )
//		{
//			poolCacheInfo.Store( go );
//			return true;
//		}
//		else
//			return false;
//	}


	public bool Store<T>( T go, bool failedFocusDestroy = false ) where T:ObjectCache
	{
		bool result = false;
		if ( go != null )
		{
			PoolCacheInfo poolCacheInfo = null;
			ObjectCache objCache = null;
			objCache = (ObjectCache)go;
			if ( objCache != null && objCache.isUsing )
			{
				if ( objCache != null )
					pools_.TryGetValue( objCache.objKey, out poolCacheInfo );
				if ( poolCacheInfo != null )
				{
					poolCacheInfo.Store( go );
					result = true;
				}
			}

			if( !result && failedFocusDestroy && go.gameObject != null )
			{
				Object.Destroy(go.gameObject);
			}
		}
		return result;
	}


}
