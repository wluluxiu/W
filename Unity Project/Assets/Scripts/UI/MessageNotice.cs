using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MessageNotice : MonoBehaviour {
	
	public Transform noticeTrans;
	public GameObject gmnObj;

	public bool isOpen = false;
	private NoticeCache lastNoticeCache = null;
	
	public static MessageNotice instance;
	void Awake()
	{
		instance = this;
		Messenger.AddListener<string>( GameEvents.EVT_UI_MESSAGE_POP.ToString(), GeneralMsgNotice );
	}
	
	void Start()
	{
		if(noticeTrans == null)
			noticeTrans = GameObject.Find("transNotice").gameObject.transform;
	}
	
	void OnDestroy()
	{
		Messenger.RemoveListener<string>( GameEvents.EVT_UI_MESSAGE_POP.ToString(), GeneralMsgNotice );
	}

	#region notice
	
	private void GeneralMsgNotice(string msg)
	{
		InitObj(gmnObj,delegate( NoticeCache nc) {
			if(lastNoticeCache != null){
				nc.itemRefresh(new NoticeCache.NoticeCacheData("GeneralMsg", msg: msg));
			}
		});
	}

	public void InitObj(GameObject obj, Callback<NoticeCache> cb = null)
	{
		if(lastNoticeCache != null)
			ObjectCachePool.instance.Store(lastNoticeCache);
		lastNoticeCache = ObjectCachePool.instance.ocpNew<NoticeCache>( obj.name, obj, noticeTrans, obj.transform.localPosition );
		lastNoticeCache.transform.localScale = Vector3.one;
		
		if(cb != null)
			cb(lastNoticeCache);
	}
	#endregion
}

