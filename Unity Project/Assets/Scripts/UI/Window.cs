using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[SerializeField]
public class WindowMaskConfig{
	public Image maskImg;
	public bool pointAnywhere;
}
	
public enum WindowState{
	OPEN,
	CLOSE,
	HIDE,
}

public enum TweenType
{
	NONE,
}
	
public class Window : MonoBehaviour{

	[HideInInspector]
	public WindowState state = WindowState.CLOSE; 
	[HideInInspector]
	public TweenType WindowTween = TweenType.NONE;

	public string windowName{get; protected set;}

	public Window windowComponent;
	public WindowMaskConfig WindowMask;
	public GameObject defaultSelectableObj;
	public string AniOpen;
	public string AniClose;

	public virtual void open ( IDictionary windowParams = null, bool playTween = false ){
		windowName = gameObject.name;
		this.state = WindowState.OPEN;
		if(playTween){
			playAnimation( AniOpen );
		}
		DefaultSelected();
	}

	public virtual void justShow(){
		this.state = WindowState.OPEN;
		playAnimation( AniOpen );
	}

	public virtual void update( IDictionary windowParams = null )
	{

	}

	public virtual void justHidden( GameObject WillOpenObj = null ){
		this.state = WindowState.HIDE;
		playAnimation( AniClose );
	}
		
	public virtual void close(bool playTween = false)
	{
		this.state = WindowState.CLOSE;
		if( playTween ){
			playAnimation( AniClose );
		}
		else{
			closeWindow();
		}
	}

	private void closeWindow()
	{
		destroy();
	}

	public void destroy(){
		HelperUtils.hlpUILayoutGroupEnable(gameObject, false);
		StartCoroutine( _destroy() );
	}

	private IEnumerator _destroy()
	{
		yield return new WaitForEndOfFrame();
		Destroy(gameObject);
	}

	public virtual void OpenAnimationFinished()
	{

	}

	public virtual void CloseAnimationFinished()
	{
		
	}

	protected void playAnimation( string key )
	{
		//this.transform.DOScale();
	}

	public virtual bool onCancelTriggerFunc()
	{
		WUIManager.UIMCloseWindow(windowName);
		return true;
	}

	public virtual void DefaultSelected()
	{
		if ( defaultSelectableObj != null )
			EventSystem.current.SetSelectedGameObject( defaultSelectableObj );
	}
}
