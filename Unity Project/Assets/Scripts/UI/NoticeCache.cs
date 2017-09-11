using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class NoticeCache : ObjectCache, IPanelItem {

	public struct NoticeCacheData
	{
		public string type;
		public string msg;

		public NoticeCacheData(string type = null, string msg = null)
		{
			this.type = type;
			this.msg = msg;
		}
	}

	public Text noticeTx;
	public CanvasGroup noticeCg;

	private string bundleKey = null;
	private string iconName = null;
	private bool isAnimat1 = false;

	private NoticeCacheData ncd;

	public override void objEnable ()
	{
		base.objEnable ();
//		noticeCg.DOFade(1f,0.01f);
	}

	public override void objDisable (Transform poolroot)
	{
		base.objDisable (poolroot);
	}

	public void finishPlay()
	{
		ObjectCachePool.instance.Store(this);
	}

	public void itemRefresh (object value)
	{
		NoticeCacheData ncd = (NoticeCacheData)value;
		this.ncd = ncd;
		switch( ncd.type )
		{
		case "GeneralMsg":
			Item_GeneralMsg();
			break;
		default :
			return;
			break;
		}
	}
		
	public void Item_GeneralMsg()
	{
		noticeTx.text = ncd.msg;
		Invoke("PlayAni",1.5f);
	}

	public void PlayAni()
	{
//		noticeCg.DOFade(0f,0.2f);
		Invoke("finishPlay",0.2f);
	}
}
