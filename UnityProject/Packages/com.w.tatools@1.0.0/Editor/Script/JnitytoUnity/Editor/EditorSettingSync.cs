using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorSettingSync 
{
	static Dictionary<int, string> tagSettingDict = new Dictionary<int, string>();
	static Dictionary<int, string> layerSettingDict = new Dictionary<int, string>();


	public static string getUnityTagNameByJnityTagValue(int value)
	{
		string jnityTagName = tagSettingDict[value];
		string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
		foreach(string tag in tags)
		{
			if (tag.Equals(jnityTagName))
			{
				return tag;
			}
		}
		ReportSystem.OutputLog($"Unity项目中没有与jnity对应的Tag Name:{jnityTagName},需要联系何灿加入到TagSetting");
		return string.Empty;
	}

	public static int getUnityLayerByJnityLayer(int value)
	{
		string jnityLayerName = layerSettingDict[value];
		string[] layers = UnityEditorInternal.InternalEditorUtility.layers;
		foreach(string layer in layers)
		{
			if (layer.Equals(jnityLayerName))
			{
				return LayerMask.NameToLayer(layer);
			}
		}
		ReportSystem.OutputLog($"Unity项目中没有与jnity对应的Layer Name:{jnityLayerName},需要联系何灿加入到LayerSetting");
		return 0;
	}


	public static void SyncEditorSettings(string jnityProjPath)
    {
        string editorSettingPath = jnityProjPath + "/../settings/EditorProjectSetting.setting";
        string jsonText = FileUtil.ReadTexTFile(editorSettingPath);
        var rootList = JsonConvert.DeserializeObject<List<object>>(jsonText);
        var rootDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(rootList[0].ToString());
        var settingsDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(rootDict["EditorProjectSettings"].ToString());

        SyncTag(settingsDict);
        SyncLayer(settingsDict);
    }

    static void SyncTag(Dictionary<object, object> settingsDict)
    {
        //Dictionary<string, string> defaultTags = new Dictionary<string, string>() ;
        //defaultTags.Add("Untagged", "Untagged");
        //defaultTags.Add("Respawn", "Respawn");
        //defaultTags.Add("Finish", "Finish");
        //defaultTags.Add("EditorOnly", "EditorOnly");
        //defaultTags.Add("MainCamera", "MainCamera");
        //defaultTags.Add("Player", "Player");


        var nodeTagsDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(settingsDict["_nodeTags"].ToString());
        var tagPresets = JsonConvert.DeserializeObject<List<object>>(nodeTagsDict["_presets"].ToString());
		//string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
		//for(int i = 0; i < tags.Length;i++)
		//{
		//    if (!defaultTags.ContainsKey(tags[i]))
		//    {
		//        UnityEditorInternal.InternalEditorUtility.RemoveTag(tags[i]);
		//    }
		//}
		tagSettingDict.Clear();
		for (int n = 0; n < tagPresets.Count; n++)
        {
            var tagPresetsDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(tagPresets[n].ToString());
            string jnityTag = tagPresetsDict["key"].ToString();
			int jnityTagVallue = int.Parse(tagPresetsDict["value"].ToString());

			//if (!defaultTags.ContainsKey(jnityTag))
			//{
			//    UnityEditorInternal.InternalEditorUtility.AddTag(tagPresetsDict["key"].ToString());
			//}
			tagSettingDict.Add(jnityTagVallue, jnityTag);
        }
    
    }

    static void setLayerAt(SerializedProperty layers, int index, string layerName)
    {
        if (index >= layers.arraySize)
            layers.arraySize = index + 1;

        var element = layers.GetArrayElementAtIndex(index);
        element.stringValue = layerName;
    }

    static void setSortLayerAt(SerializedProperty layers, int index, string layerName)
    {
        if (index >= layers.arraySize)
            layers.arraySize = index + 1;

        var dataPoint = layers.GetArrayElementAtIndex(index);
        while (dataPoint.NextVisible(true))
        {
            if(dataPoint.name == "name")
            {
                dataPoint.stringValue = layerName;
            }
        }

    }

    static void SyncLayer(Dictionary<object, object> settingsDict)
    {
		//Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
		//SerializedObject serializedObject = new SerializedObject(asset[0]);
		//SerializedProperty layers = serializedObject.FindProperty("layers");
		var nodeLayersDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(settingsDict["_nodeLayers"].ToString());
		var layerPresets = JsonConvert.DeserializeObject<List<object>>(nodeLayersDict["_nodeLayers"].ToString());
		layerSettingDict.Clear();
		for (int n = 0; n < layerPresets.Count; n++)
		{
			var layerDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(layerPresets[n].ToString());
			string layerName = layerDict["name"].ToString();
			int mask = int.Parse(layerDict["mask"].ToString());
			//setLayerAt(layers, n, layerDict["name"].ToString());
			layerSettingDict.Add(mask, layerName);
		}
		//serializedObject.ApplyModifiedProperties();
		//serializedObject.Update();
		//layers = serializedObject.FindProperty("m_SortingLayers");

		//var tagMgrsDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(settingsDict["_tagManager"].ToString());
		//var sortingLayers = JsonConvert.DeserializeObject<List<object>>(tagMgrsDict["_sortingLayers"].ToString());
		//for (int n = 0; n < sortingLayers.Count; n++)
		//{
			//var sortlayerDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(sortingLayers[n].ToString());
			//setSortLayerAt(layers, n, sortlayerDict["name"].ToString());
			//string layerName = sortlayerDict["name"].ToString();
			//int layerValue = int.Parse(sortlayerDict["uniqueId"].ToString());
			//sortLayerSettingDict.Add(layerValue, layerName);
		//}
		//serializedObject.ApplyModifiedProperties();
		//serializedObject.Update();
	}
}
