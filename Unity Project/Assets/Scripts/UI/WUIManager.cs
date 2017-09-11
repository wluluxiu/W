using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//public delegate void Callback();
//public delegate void Callback<T>(T arg1);

public class WindowParams{
	public bool isHideLastWindow;
	public bool isPlayTween;
	public WindowType windowType;
}

public class UIConfig
{
	public const int WINDOW_LEVEL_HINT = 5;
	public const int WINDOW_LEVEL_WINDOW = 15;
	public const int WINDOW_LEVEL_FULLWINDOW = 25;
	public const int WINDOW_LEVEL_PANEL = 35;

	public const string MSC_UI_PANEL_PATH = "Build/Load_PreLoad/S_UI/{0}";

	public const string COUNTDOWN_RESEARCH = "CountDown_Research";
	public const string COUNTDOWN_ACADEMY = "CountDown_Academy";
	public const string COUNTDOWN_BARRACKS = "CountDown_Barracks";
	public const string COUNTDOWN_BARRACKSQUEUE = "CountDown_BarracksQueue";
}
	
public enum WindowType
{
	WT_PANEL,
	WT_WINDOW,
	WT_HINT,
	WT_LOADING,
	WT_EFFECT,
}

/// <summary>
/// UI manager By W.
/// </summary>
public class WUIManager: MonoBehaviour
{

    /// <summary>
    /// The instance.
    /// </summary>
    public static WUIManager instance;

	public static string currentHint = null;
    public static string currentPanel = null;
	public static string currentLoading = null;
    /// <summary>
    /// The user interface cache. save opened ui object include panel and window
    /// </summary>
    private static Dictionary<string, GameObject> uiOpendObj = new Dictionary<string, GameObject>();
    //-------------------------about widnows------------------------
    /// <summary>
    /// The window stack.
    /// </summary>
    private static List<string> windowStack = new List<string>();

	private static Dictionary< WindowType, Transform > windowTypeToRootMaps_ = new Dictionary<WindowType, Transform>();

	private static string lastRequestWindowName = null;

    /// <summary>
    /// Awake this instance.
    /// </summary>
	void Awake() //Awake
    {
        if (WUIManager.instance == null)
        {
            WUIManager.instance = this;
        }
		windowTypeToRootMaps_[ WindowType.WT_LOADING ] = GameObject.Find("UIRoot/Canvas/" + WindowType.WT_LOADING.ToString() ).transform;
		windowTypeToRootMaps_[ WindowType.WT_PANEL ] = GameObject.Find("UIRoot/Canvas/" + WindowType.WT_PANEL.ToString() ).transform;
		windowTypeToRootMaps_[ WindowType.WT_WINDOW ] = GameObject.Find("UIRoot/Canvas/" + WindowType.WT_WINDOW.ToString() ).transform;
		windowTypeToRootMaps_[ WindowType.WT_HINT ] = GameObject.Find("UIRoot/Canvas/" + WindowType.WT_HINT.ToString() ).transform;
		windowTypeToRootMaps_[ WindowType.WT_EFFECT ] = GameObject.Find("UIRoot/Canvas/" + WindowType.WT_EFFECT.ToString() ).transform;
    }

	void Start(){
		UIRoot.instance.UIInit();
		//WUIManager.UIMShowLoading("LoadingWindow");
	}

	void LateUpdate()
	{
		if ( triggerGC || Time.time - lastReleaseTime > 60 )
		{
			if ( triggerGC )
				triggerGC = false;
			lastReleaseTime = Time.time;
			gmmGC(true,true);
		}
		#if UNITY_EDITOR || UNITY_ANDROID
		if ( Input.GetKeyUp(KeyCode.Escape) )
		{
			if( !triggerWindowCancel() )
			{

			}
		}
		#endif
		ClickEffect();
	}

    #region Show

	public static void UIMShowWindow( string windowName, IDictionary windowParams = null, bool isHideLastWindow = true )
    {
		if(isHideLastWindow){
			UIMHideLastWindow();
		}
		int openedIndex = windowStack.IndexOf(windowName);
		if ( openedIndex >= 0 )//TODO 重复打开
		{
			GameObject win = UIMGetOpened( windowName );
			Window openedWinComp = win.GetComponent<Window>();
			openedWinComp.update(windowParams);
		}
		else
		{
			if( lastRequestWindowName == windowName && openedIndex == -1 )
				return;
			lastRequestWindowName = windowName;
			UIMGetGameObject( windowName, WindowType.WT_WINDOW, delegate(GameObject windowObj) {
				lastRequestWindowName = "";
				Window openedWinComp = windowObj.GetComponent<Window>();
				openedWinComp.open(windowParams);
				windowStack.Add(windowName);
	        });
		}
		WUIManager.instance.gmmGC( true, true, 1f );
    }

	private static void UIMHideLastWindow(){
		string topWindowName = null;
		if (windowStack.Count > 0){
			topWindowName = windowStack [windowStack.Count - 1];
		}
		else{
			topWindowName = WUIManager.currentPanel;
		}
		if(!string.IsNullOrEmpty(topWindowName)){
			GameObject topWindowObj = UIMGetOpened(topWindowName);
			Window topWindow = topWindowObj.GetComponent<Window>();
			topWindow.justHidden();
		}
	}
		
	public static void UIMShowHintWindow()
    {
		GameObject topWindow = WUIManager.UIMGetCurrentTopUI( true );
		if( topWindow != null )
		{
			if( topWindow.name == "InformWindow" )
			{
				//refresh
				return;
			}
		}
		// show window
    }
		
	public static void UIMShowPanel( string panelName, IDictionary windowParams = null, Callback<GameObject> openCallback = null )
    {
        if ( panelName.Equals(WUIManager.currentPanel) )
            return;
		
		UIMGetGameObject( panelName, WindowType.WT_PANEL, delegate (GameObject panel){
            if (panel != null)
            {
                UIMClosePanel();
                WUIManager.currentPanel = panelName;
                Window panelWindow = panel.GetComponent<Window>();
				panelWindow.open(windowParams);

				if ( openCallback != null )
					openCallback(panelWindow.gameObject);
            }
        });
    }

	public static void UIMShowLoading(string loadingName, IDictionary windowParams = null)
	{
		WUIManager.currentLoading = loadingName;
		UIMGetGameObject( loadingName, WindowType.WT_LOADING, delegate (GameObject loading){
			if (loading != null)
			{
				if ( WUIManager.currentLoading == loadingName )
				{
					//show new panel
					Window loadingWindow = loading.GetComponent<Window>();
					loadingWindow.open(windowParams);
				}
				else
				{
					UIMRemoveFromCache(loadingName);
					Destroy(loading);
				}
			}
		});
	}

    #endregion

    #region Close

	/// <summary>
	/// UIMs the close window.
	/// </summary>
	/// <param name="windowName">Window name.</param>
	/// <param name="playTween">If set to <c>true</c> play tween.</param>
	public static void UIMCloseWindow( string windowName = null, bool playTween = false, bool isCloseAll = false )
	{
		if(isCloseAll){
			UIMCloseAllWindow();
			return;
		}
		if (windowStack.Count != 0)
		{
			if(string.IsNullOrEmpty(windowName)){
				windowName = windowStack [windowStack.Count - 1];
			}
			GameObject willClose = UIMGetOpened(windowName);
			Window willCloseComp = willClose.GetComponent<Window>();
			int willCloseIdx = windowStack.IndexOf(windowName);
			windowStack.RemoveAt(willCloseIdx);
			willCloseComp.close(playTween);
			UIMRemoveFromCache( windowName );
			UIMShowLastWindow();
		}
	}

	private static void UIMShowLastWindow(){
		string lastWindowName = null;
		if (windowStack.Count > 0){
			lastWindowName = windowStack [windowStack.Count - 1];
		}
		else{
			lastWindowName = WUIManager.currentPanel;
		}
		GameObject lastWindow = UIMGetOpened(lastWindowName);
		Window lastWindowComp = lastWindow.GetComponent<Window>();
		lastWindowComp.justShow();
	}
		
	public static void UIMCloseHint(bool isPlayAnimation = false )
	{
		GameObject willClose = UIMGetOpened(WUIManager.currentHint);
		if(willClose != null)
		{
			Window willCloseComp = willClose.GetComponent<Window>();
			willCloseComp.close(false);
		}
		WUIManager.currentHint = null;
	}
		
	public static void UIMClosePanel( bool isPlayAnimation = false )
    {
        WUIManager.UIMCloseAllWindow();
        GameObject willClose = UIMGetOpened(WUIManager.currentPanel);
        if(willClose != null)
        {
            Window willCloseComp = willClose.GetComponent<Window>();
			willCloseComp.close(isPlayAnimation);
		}
		WUIManager.currentPanel = null;
    }
	
	public static void UIMCloseLoading( bool isPlayAnimation = false )
	{
		GameObject willClose = UIMGetOpened(WUIManager.currentLoading);
		if(willClose != null)
		{
			Window willCloseComp = willClose.GetComponent<Window>();
			willCloseComp.close(isPlayAnimation);
		}
		WUIManager.currentLoading = null;
	}

    private static void UIMCloseAllWindow()
	{
        while (windowStack.Count > 0)
        {
            string name = windowStack [windowStack.Count - 1];
            windowStack.RemoveAt(windowStack.Count - 1);
            GameObject willClose = UIMGetOpened(name);
            if(willClose != null)
            {
                Window win = willClose.GetComponent<Window>();
                win.close(false);
            }
        }
    }
		
	/// <summary>
	/// UIMs the pop UI from cache.
	/// </summary>
	/// <param name="go">Go.</param>
	public static void UIMRemoveFromCache( string windowName )
	{
		if (uiOpendObj.ContainsKey(windowName))
		{
			uiOpendObj.Remove( windowName );
		}
	}

    #endregion

	#region Get GameObject

	/// <summary>
	/// UIMs the get game object.
	/// </summary>
	/// <param name="windowName">Window name.</param>
	/// <param name="parent">Parent.</param>
	/// <param name="callback">Callback.</param>
	private static void UIMGetGameObject( string windowName, WindowType parent, Callback<GameObject> callback)
    {
		GameObject targetWindow = UIMGetOpened(windowName);
        if (targetWindow == null)
        {
	       string windowLoadPath = hlpGetUIPath(windowName);
	       GetSignleAsset(windowLoadPath, delegate(Object obj){
				if (obj != null){
					targetWindow = (GameObject)Instantiate(obj);
	                targetWindow.name = windowName;
	                uiOpendObj [windowName] = targetWindow;
	                if(callback != null)
	                {
						callback(UIMAddUI(targetWindow, parent));
	                }
	            }
	            else{
					Debug.Log(string.Format("load window failure {0}", windowLoadPath));
	            }
			});
        }
		else{
			if(callback != null)
            {
                callback(UIMAddUI(targetWindow, parent));
            }
        }
    }
		
	public static GameObject UIMGetOpened(string uiObjectName)
    {
        if(uiObjectName != null && uiOpendObj.ContainsKey(uiObjectName))
        {
            return uiOpendObj[uiObjectName];
        }
        return null;
    }

	public static Window UIMGetOpenedWindow(string uiObjectName)
	{
		if(uiObjectName != null && uiOpendObj.ContainsKey(uiObjectName))
		{
			return uiOpendObj[uiObjectName].GetComponent<Window>();
		}
		return null;
	}
		
	public static GameObject UIMAddUI( GameObject uiObject, WindowType windowType )
	{
		if(uiObject != null)
		{
			Transform t = uiObject.transform;
			t.SetParent( windowTypeToRootMaps_[windowType], false );
		}
		return uiObject;
	}

	public static GameObject UIMGetCurrentTopUI( bool isOnlyWindow )
	{
		GameObject result = null;
		// get current window
		if (windowStack.Count > 0)
		{
			string lastWindowName = windowStack [windowStack.Count - 1];
			result = UIMGetOpened(lastWindowName);
		}
		else if ( !isOnlyWindow )
		{
			result = UIMGetOpened(WUIManager.currentPanel);
		}
		return result;
	}
		
    #endregion

    #region Other
	public GameObject uiClickEffectObj;
	public void ClickEffect(){
		if(Input.GetMouseButtonDown(0)){
			ObjectCachePool.instance.ocpNew<UIClickEffect>( uiClickEffectObj.name,
				uiClickEffectObj , windowTypeToRootMaps_[WindowType.WT_EFFECT], UIRoot.instance.MouseScreenPos() );
		}
	}

	public static bool triggerWindowCancel()
	{
		if (windowStack.Count != 0)
		{
			int idx = windowStack.Count - 1;
			while( idx >= 0 )
			{
				string name = windowStack [idx];
				GameObject willClose = UIMGetOpened(name);
				if( willClose != null )
				{
					Window willCloseComp = willClose.GetComponent<Window>();
					if( willCloseComp != null )
					{
						if( willCloseComp.onCancelTriggerFunc() )
							break;
						else
							idx--;
					}
				}
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	public static string hlpGetUIPath( string panelKey )
	{
		return string.Format(UIConfig.MSC_UI_PANEL_PATH, panelKey);
	}

	public static void GetSignleAsset(string bundleName, Callback<Object> callBack)
	{
		callBack( Resources.Load<GameObject>( bundleName ));
	}
	#endregion

	#region CountDown
	public Dictionary<string, Coroutine> ieDic = new Dictionary<string, Coroutine>();
	public Dictionary<string,List<Text>> ieTxDic = new Dictionary<string, List<Text>>();

	public void StartCountDown(Text Tx, string key, int countDownTime, System.Action ac = null){
		StopCountDown( key, ac );
		ieTxDic.Add( key, new List<Text>(){Tx} );
		Messenger.Broadcast<string>(GameEvents.MESSAGE_COUNTDOWN_REGISTER.ToString(), key);
		ieDic.Add(key, StartCoroutine(CountDown( key, countDownTime, ac )));
	}

	public void AddCountDownTx( Text Tx, string key ){
		if(ieTxDic.ContainsKey(key)){
			ieTxDic[key].Add(Tx);
		}
	}

	public void StopCountDown( string key, System.Action ac = null){
		if(ieDic.ContainsKey(key)){
			StopCoroutine( ieDic[key] );
			ieDic.Remove(key);
			ieTxDic.Remove(key);
			if(ac != null){
				ac();
			}
		}
	}

	public void RemoveCountDownTx( string key ){
		if(ieDic.ContainsKey(key)){
			ieTxDic.Remove(key);
		}
	}
	[HideInInspector]
	public bool isCountDowning = false;
	private IEnumerator CountDown(string key, int countDownTime, System.Action ac = null){
		isCountDowning = true;
		int time = countDownTime;
		while(time > 0){
			SetTextContent(key, HelperUtils.hlpGetCountDownTimeOriginalStringMS(time));
			yield return new WaitForSecondsRealtime(1f);
			time--;
		}
		SetTextContent(key,"");
		isCountDowning = false;
		StopCountDown( key, ac );
	}

	public void SetTextContent(string key, string content){
		Text[] txs = ieTxDic[key].ToArray();
		for(int i = 0; i < txs.Length; i++){
			if( txs[i] != null ){
				txs[i].text = content;
			}
		}
	}

    #endregion
   
	#region GC

	float lastGC = 0;
	float lastReleaseTime = 0;
	[HideInInspector]
	public bool triggerGC = false;

	public void gmmGC( bool unloadUnusedAssets, bool gc, float delayTime )
	{
		lastReleaseTime = Time.time - 60 + delayTime;
	}

	public void gmmGC( bool unloadUnusedAssets, bool gc )
	{
		if ( Time.time - lastGC < 20 )
			return;
		if ( unloadUnusedAssets )
			Resources.UnloadUnusedAssets();
		if ( gc )
			System.GC.Collect();
		lastGC = Time.time;
	}

	#endregion



}
