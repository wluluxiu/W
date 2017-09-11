using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UIParticalMaskHelper : UIBehaviour {

	public ParticleSystem ps;
	public RectTransform rectTransform;
	// Use this for initialization
	public void Change () {
		Vector3[] corners = new Vector3[4]; 
		rectTransform.GetWorldCorners (corners);
		float minX = corners [0].x;
		float minY = corners [0].y;
		float maxX = corners [2].x;
		float maxY = corners [2].y;

		Renderer[] renders = GetComponentsInChildren<Renderer>();
		for(int i = 0; i < renders.Length;i++)
		{
			renders[i].material.SetFloat("_MinX",minX);
			renders[i].material.SetFloat("_MinY",minY);
			renders[i].material.SetFloat("_MaxX",maxX);
			renders[i].material.SetFloat("_MaxY",maxY);
		}

	}
}
