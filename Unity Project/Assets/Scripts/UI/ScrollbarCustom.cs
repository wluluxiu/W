using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScrollbarCustom : MonoBehaviour {

	public ScrollRect scrollRect;
	public Slider slider;

	public void OnDrag1 (Vector2 eventData){
		slider.value = eventData.x;
	}

	public void OnDrag2 (float x){
		scrollRect.horizontalNormalizedPosition = x;
	}
}
