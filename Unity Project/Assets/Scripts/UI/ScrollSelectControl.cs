using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollSelectControl : MonoBehaviour {

	public float cellhalfWidth = 50f;
	public float hSlotWidth = 10f;
	public ScrollRect scrollRect;
	public float toPos;

	private float hideLength = 0;

	// debug.
//	public float curIdx = 0;
//	public float allCount = 0;

	// Use this for initialization
	void Start () {
//		ScrollItemTest[] scrollItems = GetComponentsInChildren<ScrollItemTest>();
//		for( int i = 0; i < scrollItems.Length; ++i )
//		{
//			if ( scrollItems[i].isSelect )
//			{
//				scrollItems[i].Select();
//				break;
//			}
//		}
	}

	// Update is called once per frame
	void Update () {
//		scrollRect.horizontalNormalizedPosition = (curIdx + 1 )/allCount; //110 * curIdx / (110 * allCount + 10);
//		scrollRect.horizontalNormalizedPosition = Mathf.Lerp( scrollRect.horizontalNormalizedPosition, toPos, Time.deltaTime );
	}
	
	public void sscClean()
	{
		toPos = 0;
		hideLength = 0;
	}

	public void UpdateScrollRectPosition( float hPos )
	{
//		return;
		float width = ((RectTransform)scrollRect.transform).rect.width;
		float rangeWidth = scrollRect.content.rect.width - width;
		if ( rangeWidth <= 0 )
			return;
		// check right.
		float diff = ( hPos + cellhalfWidth + hSlotWidth ) - ( width + hideLength );
		if ( diff > 0 )
		{
			hideLength += diff;
			// move diff. move to right.
			toPos += diff / rangeWidth;
		}
		else if ( hideLength > 0 )
		{
			// check left.
			diff = hideLength - ( hPos - cellhalfWidth - hSlotWidth );
			if ( diff > 0 )
			{
				hideLength -= diff;
				// move to left.
				toPos -= diff / rangeWidth;
			}
		}
		if ( float.IsInfinity(toPos) )
			toPos = 0;
		scrollRect.horizontalNormalizedPosition = toPos;

//		targetpos = 110 * hPos / scrollRect;
//		scrollRect.horizontalNormalizedPosition = 110 * hPos / (110 * allCount + 10);//(hPos - 50)/scrollRect.content.rect.width;
	}
}
