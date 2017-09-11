using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIRoot : MonoBehaviour {

	static public UIRoot instance = null;

	public Camera UICamera 	= null;
	public Canvas uiCanvas = null;
	public CanvasScaler UIResolution;
	public GraphicRaycaster graphicRaycaster = null;
	public EventSystem eventSystem = null;

	private int resSize = 1;

	void Awake()
	{
		if ( instance == null )
		{
			DontDestroyOnLoad( gameObject );
			instance = this;
		}
		else
		{
			Debug.LogError( "UIRoot have exist" );
		}
	}

	public void UIInit()
	{
		Debug.LogFormat("UIInit start");
		Screen.SetResolution((Screen.currentResolution.width * resSize), (Screen.currentResolution.height * resSize), true);
//		Resolution[] res = Screen.resolutions;//支持的分辨率，android为空
		eventSystem.pixelDragThreshold = 5;
		Debug.LogFormat("UIInit finished");
	}

	public Vector2 PixelRatio()
	{
		Vector2 result = UIResolution.referenceResolution;
		result.x = result.x / Screen.width;
		result.y = result.y / Screen.height;
		return result;
	}

	public Vector2 ScreenSize()
	{
		RectTransform rc = ((RectTransform) uiCanvas.transform);
		Vector2 result = new Vector2( rc.rect.width, rc.rect.height);
		return result;
	}

	public Vector3 MouseScreenPos(){
		Vector2 pos = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle( uiCanvas.transform as RectTransform, Input.mousePosition, uiCanvas.worldCamera, out pos)){
			Vector2 vc2 = ScreenSize();
			float maxX = vc2.x/2;
			float maxY = vc2.y/2;
			pos = new Vector2( Mathf.Clamp( pos.x,-maxX, maxX),Mathf.Clamp( pos.y,-maxY, maxY));
		}
		return new Vector3(pos.x, pos.y, 0);
	}
}
