using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//using LuaInterface;

public interface IPanelItem
{
	void itemRefresh( object value );
}

public class ItemPanel : MonoBehaviour {
	public enum Pivot
	{
		Horizontal,
		Vertical
	}
	public int cellWidth;
	public int cellHeight;
	public Pivot pivot;
	public bool autoLayout = true;
	public ScrollRect itpScr;

	private bool canRefresh = false;
	private string curPrefabPath = "";
	private RectTransform mTrans_;
	private int defaultSelectIdx = -1;
	private const string itemSlotName = "item&";
	private const int callMaxTime = 10;
	
	public List<ObjectCache> itemList = new List<ObjectCache>();
	private IList itemDatas = null;
	private float callBottomCallbackTime = 0;

	public bool onlyShowOnePage = false;
	public int curTopIdx = 0;
	public int curLastIdx = -1;
	public int pageCount = 10;

	public bool testb = false;
	public CallbackRetBool toBottomCallback = null;
	public ScrollSelectControl scrollSelectControl = null;
	public bool luaImp = false;
	private GameObject preloadItemObj = null;

	private bool _isInit = false;

	void Awake()
	{
		mTrans_ = (RectTransform)transform; 
		if ( onlyShowOnePage )
		{
			if ( itpScr == null ){
				itpScr = mTrans_.parent.GetComponent<ScrollRect>();
			}
			if ( itpScr != null )
				itpScr.onValueChanged.AddListener(scrollValueChanged);
		}
		scrollSelectControl = mTrans_.parent.GetComponent<ScrollSelectControl>();
	}

//	// Use this for initialization
//	void Start ()
//	{
//
//	}

//	public bool selectTest = false;
//	public int selectTestIdx = 0;
//	void Update()
//	{
//		if ( selectTest )
//		{
//			selectTest = false;
//			itpSelectItem(selectTestIdx);
//		}
//	}

	private bool loadingNextPart = false;
	void scrollValueChanged( Vector2 offset )
	{
		if ( itemDatas == null || itemDatas.Count < pageCount || !_isInit )
			return;
		if ( ( pivot == Pivot.Vertical ?  itpScr.verticalNormalizedPosition <= 0f : itpScr.horizontalNormalizedPosition >= 1 )
		    && !loadingNextPart && curLastIdx != 0 )
		{
			
			// loading next part.
			loadingNextPart = true;
			List<object> datas = new List<object>();
			int startIdx = curLastIdx + 1;
			if ( startIdx != itemDatas.Count )
			{
				int endCount = startIdx + pageCount;
				if( endCount > itemDatas.Count )
					endCount = itemDatas.Count;
				for( int i = startIdx; i < endCount; ++i )
				{
					datas.Add( itemDatas[i] );
				}
				_itpAddItemsToShow( datas, curPrefabPath, delegate() {
					curLastIdx += datas.Count;
					curLastIdx = Mathf.Min( itemDatas.Count - 1, curLastIdx );
					loadingNextPart = false;
				} );
			}
			else if ( toBottomCallback != null && ( Time.time - callBottomCallbackTime >= callMaxTime ) )
			{
				callBottomCallbackTime = Time.time;
				if ( toBottomCallback() )
				{
					// some datas will push here, so we show loading in item panel if necessary.
				}
			}
		}
	}
	
	void OnDestroy()
	{
		itmClean();
//		while (mTrans_.childCount > 0 && ObjectCachePool.instance.Store( mTrans_.GetChild(0).gameObject ))
//			;
	}

	public void itmClean()
	{
		if ( itemDatas != null )
			itemDatas.Clear();
		int idx = 0;
		while( itemList.Count > 0 )
		{
			idx = itemList.Count - 1;
			ObjectCachePool.instance.Store(itemList[idx]);
			itemList.RemoveAt(idx);
		}
		itemList.Clear();
		preloadItemObj = null;
		curLastIdx = -1;
		defaultSelectIdx = -1;
	}

	public void itmPushRereshItemsToEnd( object oitemDatas )
	{
		loadingNextPart = false;
		callBottomCallbackTime = 0;
		IList data = (IList)oitemDatas;
		for( int i = 0 ; i < data.Count ; i++ )
		{
			itemDatas.Add(data[i]);
		}
	}

	public void itpRefreshItem( object oitemDatas, string loadPrefabKey = "" ,Callback overMethod = null, int selectIdx = -1,bool isAdd = false, bool isHoldPosition = false )
	{
		_isInit = false;
		if ( mTrans_ == null )
			mTrans_ = (RectTransform)transform;
		if ( scrollSelectControl != null )
			scrollSelectControl.sscClean();
		loadingNextPart = false;
		callBottomCallbackTime = 0;
		itemDatas = (IList)oitemDatas;
		if ( itpScr != null && !isHoldPosition )
		{
			if ( pivot == Pivot.Horizontal )
				itpScr.horizontalNormalizedPosition = 0;
			else if ( pivot == Pivot.Vertical )
				itpScr.verticalNormalizedPosition = 1;
		}
		curLastIdx = -1;
		List<object> datas = new List<object>();
		int endCount = onlyShowOnePage ? Mathf.Min( pageCount, itemDatas.Count ) : itemDatas.Count;
		for( int i = 0; i < endCount; ++i )
		{
			datas.Add( itemDatas[i] );
		}
		defaultSelectIdx = selectIdx;
		canRefresh = true;
		if(isAdd){
			_itpAddItemToShow (datas, loadPrefabKey , delegate() {
				curLastIdx += datas.Count;
				if ( overMethod != null )
					overMethod();
				_isInit = true;
			} );
		}else
		{
			_itpRefreshItem (datas, loadPrefabKey, delegate() {
				curLastIdx += datas.Count;
				if ( overMethod != null )
					overMethod();
				_isInit = true;
			}  );
		}
	}

	public void itpRefreshStop()
	{
		canRefresh = false;
		callBottomCallbackTime = 0;
	}

	public void itpSelectItem( int idx )
	{
		if ( idx != -1 && itemDatas.Count > 0 )
		{
			string childName = string.Format( "{0}{1}", itemSlotName, idx );
			Transform childObj = transform.Find( childName );
			childObj.SendMessage( "OnSelectItem" );
		}
	}

	void _itpLoadRes( string loadPrefabKey, Callback<Object> cb )
	{
		if( preloadItemObj != null && curPrefabPath == loadPrefabKey )
		{
			cb(preloadItemObj);
		}
		else
		{
			preloadItemObj = null;
			WUIManager.GetSignleAsset(WUIManager.hlpGetUIPath(loadPrefabKey), delegate( Object prefabObj){
				preloadItemObj = prefabObj as GameObject;
				cb( prefabObj );
			});
		}
	}

	void _itpRefreshItem( object oitemDatas, string loadPrefabKey = "" ,Callback overMethod = null )
	{
		if ( string.IsNullOrEmpty( loadPrefabKey ) )
		{
			Debug.LogError( "Miss load prefab key.");
		}
		int idx = 0;
		if ( !string.IsNullOrEmpty( curPrefabPath ) && curPrefabPath != loadPrefabKey )
		{
			while( itemList.Count > 0 )
			{
				idx = itemList.Count - 1;
				ObjectCachePool.instance.Store(itemList[idx]);
				itemList.RemoveAt(idx);
			}
			preloadItemObj = null;
			itemList.Clear();
		}
		
		curPrefabPath = loadPrefabKey;
		int curExistChildCount =  mTrans_.childCount;
		IList refreshItems = ( IList ) oitemDatas;
		if ( refreshItems.Count < curExistChildCount )
		{
			while (itemList.Count > refreshItems.Count)
			{
				idx = itemList.Count - 1;
				ObjectCachePool.instance.Store(itemList[idx]);
				itemList.RemoveAt(idx);
			}
		}
		idx = 0;
		string childNameKey = "";
//		itemList.Clear();
		_itpLoadRes(loadPrefabKey, delegate(Object prefabObj) {
//        ResourceManager.Instance().GetSignleAsset(HelperUtils.hlpGetResourcesPath(loadPrefabKey), delegate(string bundleName, Object prefabObj) {
			for( int i = 0; i < refreshItems.Count; ++i )
            {
				object itemData = refreshItems[i];
                if ( !canRefresh )
                    break;
				ObjectCache itemObj = null;
                if ( idx > curExistChildCount - 1 )
                {
					itemObj = ObjectCachePool.instance.ocpNew<ObjectCache>( prefabObj.name, prefabObj as GameObject, mTrans_, Vector3.zero );
                    itemObj.name = string.Format( "{0}{1}", itemSlotName, idx );
					if ( autoLayout )
					{
	                    if ( pivot == Pivot.Horizontal )
	                    {
							itemObj.mTrans.localPosition = new Vector3( cellWidth * idx, 0, 1 );
	                    }
	                    else
	                    {
							itemObj.mTrans.localPosition = new Vector3( 0, -cellHeight * idx, 1 );
	                    }
					}
					itemList.Add(itemObj);
                }
                else
                {
					itemObj = itemList[i];
                }
				if(!itemObj.gameObject.activeSelf)
					itemObj.gameObject.SetActive(true);
				((IPanelItem)itemObj).itemRefresh( itemData );
                idx++;
            }

            if( idx == refreshItems.Count )
            {
				if( autoLayout )
				{
					if( pivot == Pivot.Horizontal )
						mTrans_.sizeDelta = new Vector2( cellWidth * mTrans_.childCount, 0 );
					else if( pivot == Pivot.Vertical )
						mTrans_.sizeDelta = new Vector2( 0, cellHeight * mTrans_.childCount );
				}
				if( gameObject.activeInHierarchy )
					StartCoroutine(_itpWaitFinished(delegate {
						if( overMethod != null )
						{
							overMethod();
						}
						if ( defaultSelectIdx != -1 && refreshItems.Count > 0 )
						{
							itpSelectItem(defaultSelectIdx);
						}
					}));
            }
		});
	}

	IEnumerator _itpWaitFinished( Callback callback )
	{
		yield return new WaitForEndOfFrame();
		if( callback != null )
			callback();
	}

	void _itpAddItemToShow( object oitemDatas, string loadPrefabKey = "" ,Callback overMethod = null )
	{
		if ( string.IsNullOrEmpty( loadPrefabKey ) )
		{
			Debug.LogError( "Miss load prefab key.");
			//	yield return 0;
		}
		if ( !string.IsNullOrEmpty( curPrefabPath ) && curPrefabPath != loadPrefabKey )
		{
			return;
		}
		curPrefabPath = loadPrefabKey;
		int curExistChildCount = mTrans_.childCount;
		IList refreshItems = ( IList ) oitemDatas;
		
		WUIManager.GetSignleAsset(WUIManager.hlpGetUIPath(loadPrefabKey), delegate(Object prefabObj) {

			ObjectCache itemObj = null;
			itemObj = ObjectCachePool.instance.ocpNew<ObjectCache>( prefabObj.name, prefabObj as GameObject, mTrans_, Vector3.zero );
			itemObj.gameObject.name = string.Format( "{0}{1}", itemSlotName, curExistChildCount++ );
			itemList.Add(itemObj);
			((IPanelItem)itemObj).itemRefresh( refreshItems[refreshItems.Count - 1] );
			StartCoroutine(_itpWaitFinished(delegate {
				if( overMethod != null )
				{
					overMethod();
				}
			}));
		});
	}
	
	void _itpAddItemsToShow( object oitemDatas, string loadPrefabKey = "" ,Callback overMethod = null )
	{
		if ( string.IsNullOrEmpty( loadPrefabKey ) )
		{
			Debug.LogError( "Miss load prefab key.");
		}
		if ( !string.IsNullOrEmpty( curPrefabPath ) && curPrefabPath != loadPrefabKey )
		{
			return;
		}
		int curExistChildCount = mTrans_.childCount;
		IList refreshItems = ( IList ) oitemDatas;
		int idx = curExistChildCount;
		WUIManager.GetSignleAsset(WUIManager.hlpGetUIPath(loadPrefabKey), delegate(Object prefabObj) {
			
			ObjectCache itemObj = null;
			for( int i = 0; i < refreshItems.Count; ++i )
			{
				itemObj = ObjectCachePool.instance.ocpNew<ObjectCache>( prefabObj.name, prefabObj as GameObject, mTrans_, Vector3.zero );
				itemObj.gameObject.name = string.Format( "{0}{1}", itemSlotName, curExistChildCount++ );
				if ( autoLayout )
				{
					if ( pivot == Pivot.Horizontal )
					{
						itemObj.mTrans.localPosition = new Vector3( cellWidth * idx, 0, 1 );
					}
					else
					{
						itemObj.mTrans.localPosition = new Vector3( 0, -cellHeight * idx, 1 );
					}
				}
				itemList.Add(itemObj);
				((IPanelItem)itemObj).itemRefresh( refreshItems[i] );
				idx++;
			}
			if( overMethod != null )
			{
				overMethod();
			}
		});
	}
}
