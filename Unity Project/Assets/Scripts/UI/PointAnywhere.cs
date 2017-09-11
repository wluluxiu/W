using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PointAnywhere : MonoBehaviour,IPointerClickHandler {
	
	public bool playAni = false;
	public void OnPointerClick (PointerEventData eventData)
	{   
		WUIManager.UIMCloseWindow(playTween: playAni);
	}
}
