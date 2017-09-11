using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Localization
{
	static private Dictionary< string, string > mDictionary_ = new Dictionary<string, string>();
	static private string languageName_;

	static public void Set( string languageName, Dictionary< string, string > dictionary )
	{
		languageName_ = languageName;
		mDictionary_ = dictionary;
	}

	static public string Get( string key, bool isLocalLoginLang = false )
	{
		string result;
		if ( mDictionary_.TryGetValue( key, out result ) )
		{
			return result;
		}
		else
		{
			Debug.Log( "Miss localization key: " + key);
			return key;
		}

	}
}

