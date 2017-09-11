using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Hosting;


public class Game : MonoBehaviour {

	[RuntimeInitializeOnLoadMethod]
	static void Initialize()
	{
		Debug.Log( "RuntimeInitializeOnLoadMethod" );
		GameObject.DontDestroyOnLoad(new GameObject("GameRoot",typeof(Game)) {
			//hideFlags = HideFlags.HideInHierarchy
		});

	}

}
