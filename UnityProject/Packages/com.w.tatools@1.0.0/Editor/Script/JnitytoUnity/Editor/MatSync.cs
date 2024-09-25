using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class MatSync
{
  
    static Texture2D FlipTextureVertical(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height);
        for (int y = 0; y < original.height; y++)
        {
            for (int x = 0; x < original.width; x++)
            {
                Color color = original.GetPixel(x, original.height - y - 1);
                flipped.SetPixel(x, y, color);
            }
        }
        flipped.Apply();
        return flipped;
    }

    static Texture FindTextureInUnity(string matName,string tex_uuid)
    {
        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        string jnity_texture_path = ""; string assetPath = "";string unity_texture_path = "";
		string jnity_texture_meta_path;
		if (!resConfig.resFiles.ContainsKey(tex_uuid))
        {
            if (!JnityBuildInRes.buildInResMap.ContainsKey(tex_uuid))
            {
                ReportSystem.OutputLog("材质[" + matName + "] 中包含未找到的纹理贴图, 贴图 uuid:" + tex_uuid);
                return null;
            }
            else
            {
                jnity_texture_path = JnityBuildInRes.buildInResMap[tex_uuid].path;
                assetPath = Jnity2Unity.unityRelativeAssetPath + JnityBuildInRes.buildInResMap[tex_uuid].idmap_path;
                unity_texture_path = Jnity2Unity.unityAssetPath +JnityBuildInRes.buildInResMap[tex_uuid].idmap_path;
				jnity_texture_meta_path = jnity_texture_path + ".metax";
			}
        }
        else
        {
            jnity_texture_path = resConfig.resFiles[tex_uuid].j_path;
            assetPath = FileUtil.GetAssetPath(jnity_texture_path);
            unity_texture_path = Application.dataPath + assetPath.Replace("Assets", "");
			jnity_texture_meta_path = jnity_texture_path + ".meta";
		}
        FileInfo textureFileInfo = new FileInfo(unity_texture_path);
      
        //if (!textureFileInfo.Exists)
        //{
            // Texture2D texture = FileUtil.GetTexrture2DFromPath(jnity_texture_path);
        FileUtil.CopyFile(jnity_texture_path, unity_texture_path);
        AssetDatabase.Refresh();         
        TextureSync.SyncMetaFile(unity_texture_path, assetPath, jnity_texture_meta_path);
        //}
        TextureImporterShape textureShape = TextureSync.getTextureShape(jnity_texture_meta_path, assetPath);
        Texture texture;
        if (textureShape == TextureImporterShape.Texture2D)
        {
            texture = (Texture2D)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));
        }
        else if(textureShape == TextureImporterShape.TextureCube)
        {
            texture = (Texture)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture));
        }else if(textureShape == TextureImporterShape.Texture2DArray)
        {
            texture = (Texture)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2DArray));
        }
        else
        {
            texture = (Texture)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture3D));
        }
        return texture;
    }

    static Shader FindShaderInUnity(string gameObjectName, string matName, string shader_uuid)
    {
        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        string jnity_shader_path = "";
        if (!resConfig.resFiles.ContainsKey(shader_uuid))
        {
            if (!JnityBuildInRes.buildInResMap.ContainsKey(shader_uuid))
            {
                ReportSystem.OutputLog($"节点[{gameObjectName}] 材质[{matName}] 中包含在jnity工程未找到的shader, shader uuid:" + shader_uuid);
                return null;
            }
            else
            {
                jnity_shader_path = JnityBuildInRes.buildInResMap[shader_uuid].path;
            }
        }
        else
        {
            jnity_shader_path = resConfig.resFiles[shader_uuid].j_path;
        }

        string shader_path = FileUtil.getShaderPath(jnity_shader_path);

        Shader shader = ShaderFind(shader_path);
        if (shader == null)
        {
            string mapping_shader_path = ShaderMapping.getUnityShaderPath(shader_path);
            if (mapping_shader_path == null)
            {
                mapping_shader_path = "DDZ/TM_fog_Transparent";
                shader = ShaderFind(mapping_shader_path);
                ReportSystem.OutputLog($"节点[{gameObjectName}] 材质[{matName}]unity工程中的shader文件[{shader_path}] 找不到,请检查!");
            }
            else
                shader = ShaderFind(mapping_shader_path);
        }
        return shader;
    }

	public static string ChangePathCharater (string path)
	{
		return path.Replace("\\", "/");
	}

	private static Shader ShaderFind(string shaderPath)
	{
		string shaderFilePath= null;
		JnityResScanner.shaderMapping.TryGetValue(shaderPath, out shaderFilePath);
		if (shaderFilePath!=null)
		{
			shaderFilePath = shaderFilePath.Replace("\\", "/");
			int startIndex = shaderFilePath.IndexOf("Assets");
			string assetPath = shaderFilePath.Substring(startIndex, shaderFilePath.Length - startIndex);
			Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(assetPath);
			return shader;
		}
		return null;
	}

    public static void SyncMat(Dictionary<object, object> mat_attribute, string gameObjectName, ref Material mat)
    {
        string matName = mat_attribute["_name"].ToString();
        bool enableGPUInstancing = bool.Parse(mat_attribute["_enableGPUInstancing"].ToString());
        int renderQueue = int.Parse(mat_attribute["_renderQueue"].ToString());

        var shader_attribute = JsonConvert.DeserializeObject<Dictionary<object, object>>(mat_attribute["_effectOPtr"].ToString());
        string shader_uuid = shader_attribute["uuid"].ToString();
        Shader shader = FindShaderInUnity(gameObjectName, matName, shader_uuid);
        if (shader == null)
        {
            return;
        }
		if (mat == null)
			mat = new Material(shader);
		else
			mat.shader = shader;
        mat.name = matName;
        mat.enableInstancing = enableGPUInstancing;
        mat.renderQueue = Mathf.Min(renderQueue, 5000);

        var savedMatOPtrData = JsonConvert.DeserializeObject<Dictionary<object, object>>(mat_attribute["_matOPtrData"].ToString());
        var _properties = JsonConvert.DeserializeObject<Dictionary<object, object>>(savedMatOPtrData["_properties"].ToString());

        var valueFloats = JsonConvert.DeserializeObject<List<object>>(_properties["_valueFloats"].ToString());
        for (int n = 0; n < valueFloats.Count; n++)
        {
            var valueFloat = JsonConvert.DeserializeObject<Dictionary<object, object>>(valueFloats[n].ToString());
            mat.SetFloat(valueFloat["_name"].ToString(), float.Parse(valueFloat["_value"].ToString()));
        }

        var valueVecFloats = JsonConvert.DeserializeObject<List<object>>(_properties["_valueVecFloats"].ToString());
        for (int n = 0; n < valueVecFloats.Count; n++)
        {
            var valueVecFloat = JsonConvert.DeserializeObject<Dictionary<object, object>>(valueVecFloats[n].ToString());
            var vecList = JsonConvert.DeserializeObject<List<object>>(valueVecFloat["_value"].ToString());
            Vector4 valueVec = new Vector4(float.Parse(vecList[0].ToString()), float.Parse(vecList[1].ToString()),
                float.Parse(vecList[2].ToString()), float.Parse(vecList[3].ToString()));
            mat.SetVector(valueVecFloat["_name"].ToString(), valueVec);
        }

        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        var valueTexs = JsonConvert.DeserializeObject<List<object>>(_properties["_valueTexs"].ToString());
        for (int n = 0; n < valueTexs.Count; n++)
        {
            var valueTex = JsonConvert.DeserializeObject<Dictionary<object, object>>(valueTexs[n].ToString());
            if (valueTex["_value"] == null)
                continue;
            var texDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(valueTex["_value"].ToString());
            string tex_uuid = texDict["uuid"].ToString();
            Texture texture = FindTextureInUnity(matName, tex_uuid);
            mat.SetTexture(valueTex["_name"].ToString(), texture);

        }
    }


    public static void SyncMatFile(string gameObjectName,string path, ref Material mat)
    {
        string jsonText = FileUtil.ReadTexTFile(path);
        var matList = JsonConvert.DeserializeObject<List<object>>(jsonText);
        var jsonStr = JsonConvert.DeserializeObject<Dictionary<object, object>>(matList[0].ToString());
        var mat_attribute = JsonConvert.DeserializeObject < Dictionary<object, object> >(jsonStr["Material"].ToString());
        SyncMat(mat_attribute, gameObjectName, ref mat);
    }
}
