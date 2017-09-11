using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ClientConfig 
 {
    private static string DEFAULT_SERVER_IP         = "192.168.1.201:7005";
    // ----------------
    // resource manager setting.
    // ----------------
	public static string DATA_RES_CHANNEL_FILE = "channel.pkg";
    public static string DATA_RES_VERSION_FILE = "version";
    public static string DATA_GAME_CONFIG_FILE = "conf.json";
    public static string DATA_ASSETS_CONFIG_FILE = "assets.json";
	public const string DATA_MANIFEST_FILE = "manifests";

	public static string COMPRESS_FILE_SUFFIX = ".7z";

	public const string BUFF_EFFECT_KEY_ADD_ATK	= "ADD_ATK";

	// ----------------
	// item data base const.
	// ----------------
	public const int IDB_TYPE_ITEM			= 1;

	public const string RES_KEY_UI_ICON = "icon_item";

	public const string MSC_UI_PANEL_PATH		= "Prefab/UI/Panel/{0}";
	public const string MSC_UNKNOW_FLAG = "UnKnow";

	// ----------------
	// tag const.
	// ----------------
	public const string TAG_EDITOR_MODEL	= "EditorOnly";
	public const string TAG_SCENE_ROOT		= "Scene";
	public const string TAG_PLAYER			= "Player";
	public const string TAG_UI_ROOT			= "UIRoot";
	public const string TAG_MAIN_CAMERA		= "MainCamera";
	public const string TAG_UI_CAMERA		= "UICamera";

	// ----------------
	// layer const.
	// ----------------
	public const int LAYER_DEFAULT			= 0;
	public const int LAYER_IGNORE_RAYCAST	= 2;
	public const int LAYER_UI				= 5;
	public const int LAYER_GROUND			= 8;
	public const int LAYER_PLAYER			= 9;
	public const int LAYER_AIPLAYER			= 13;
	public const int LAYER_AVATAR			= 14;
	public const int LAYER_BUILDING			= 15;
	public const int LAYER_UI_HIDE			= 16;

	public const int LAYER_MASK_BATTLE_CAMERA = ~( 1<<ClientConfig.LAYER_UI | 1 << ClientConfig.LAYER_UI_HIDE );
	public const int LAYER_MASK_BATTLE_CAMERA_SCOPE = ~( 1<<LAYER_PLAYER | 1<<ClientConfig.LAYER_UI | 1 << ClientConfig.LAYER_UI_HIDE );
	public const int LAYER_MASK_PLAYER = ~( (1<<ClientConfig.LAYER_PLAYER) | ( 1<<ClientConfig.LAYER_IGNORE_RAYCAST ) );
	public const int LAYER_MASK_AIPLAYER = ~( (1<<ClientConfig.LAYER_AIPLAYER) | ( 1<<ClientConfig.LAYER_IGNORE_RAYCAST ) );
	public const int LAYER_PLAYER_LOCK = ~( ( 1<<ClientConfig.LAYER_IGNORE_RAYCAST | 1<<ClientConfig.LAYER_BUILDING ) );

	public const int LAYER_MASK_CAMERA = ~( (1<<ClientConfig.LAYER_PLAYER) | ( 1<<ClientConfig.LAYER_AVATAR ) | (1<<ClientConfig.LAYER_AIPLAYER) | ( 1<<ClientConfig.LAYER_IGNORE_RAYCAST ) );
	public const int LAYER_ALLATTACK = (1<<ClientConfig.LAYER_PLAYER) | ( 1<<ClientConfig.LAYER_AIPLAYER | 1<< ClientConfig.LAYER_AVATAR | 1<< ClientConfig.LAYER_BUILDING );

	public static Dictionary< string, int > DIC = new Dictionary<string, int>{
		{ "", 0 },
	};
		
	public static List< string > LIST = new List< string >
	{
		"",
	};
		
	#region Help Function

    // ----------------
    // functions.
    // ----------------

	private static Hashtable ConfigJsonData = new Hashtable();

    public static void AddConfig(string key, object value)
    {
        ConfigJsonData[key] = value;
    }

	public static void ConfigClean()
	{
		ConfigJsonData.Clear();
	}

	#endregion
}