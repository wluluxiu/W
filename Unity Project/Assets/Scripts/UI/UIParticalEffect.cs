using System;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.UI;

public class UIParticalEffect : MonoBehaviour {

	public int sortOrder = 0;

	void Start () {
		Canvas ca = UIRoot.instance.uiCanvas;
		string sortLayerName = ca.sortingLayerName;
		int sortOrder = ca.sortingOrder;
		int layer = ca.gameObject.layer;
		Renderer[] renders = GetComponentsInChildren<Renderer>();
		for(int i = 0; i < renders.Length;i++)
		{
			renders[i].sortingLayerName = sortLayerName;
			if(this.sortOrder == 0)
				renders[i].sortingOrder = sortOrder + 1;
			else
				renders[i].sortingOrder = this.sortOrder;
			renders[i].gameObject.layer = layer;
		}
	}

}
