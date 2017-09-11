using UnityEngine;
using System.Collections;

public interface IObjectCache
{
	void objInit( string key );
	void objReset();
	void objEnable();
	void objDisable( Transform poolroot );
}

public class ObjectCache : MonoBehaviour, IObjectCache
{
	[HideInInspector]
	public string objKey = "";
	public bool isUsing = false;
	public Transform mTrans;

	protected virtual void Awake()
	{
		mTrans = transform;
	}

	public virtual void objInit( string key )
	{
		objKey = key;
		objEnable();
	}

	public virtual void objReset()
	{
		
	}

	public virtual void objEnable()
	{
		isUsing = true;
	}

	public virtual void objDisable( Transform poolroot )
	{
		isUsing = false;
	}
	
	protected virtual void OnEnable()
	{
//		Debug.Log("enabled: " + name);
//		objEnable();
	}
	
	protected virtual void OnDisable()
	{
//		Debug.Log("disable: " + name);
//		objDisable(null);
	}

	protected virtual void OnDestroy()
	{
		objDisable(null);
	}
}
