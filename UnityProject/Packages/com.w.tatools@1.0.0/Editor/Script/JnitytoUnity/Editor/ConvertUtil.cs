using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

enum ParticleModuleFlag
{
    INITIAL,
    EMISSION,
    SHAPE,
    VELOCITY,
    LIMIT_VELOCITY,
    FORCE,
    SIZE,
    SIZE_BY_SPEED,
    ROTATION,
    ROTATION_BY_SPEED,
    COLOR,
    COLOR_BY_SPEED,
    COLLISION,
    UV,
    SUB,
    CUSTOM_DATA,
    TRAILS,
    NOISE
};

public class AttachNodeInfo
{
    public GameObject parentObj;
    public GameObject selfObj;
    public string attchNodeName;
}

public class AnimNodeInfo
{
    public GameObject ownerObj;
    public TreeNode treeNode;
}

public class SubEmitterInfo
{
    public GameObject parentObj;
    public long localId;
    public ParticleSystemSubEmitterProperties property;
    public ParticleSystemSubEmitterType type;
}

public class CollisionPlaneInfo
{
    public GameObject parentObj;
    public long localId;
}

public class DynamicBoneInfo
{
    public GameObject parentObj;
    public MonoBehaviour db;
    public string rootBoneName;
}

public class ConvertUtil 
{
   // public static  List<AttachNodeInfo> attachNodeList = new List<AttachNodeInfo>();
    public static List<AnimNodeInfo> animNodeList = new List<AnimNodeInfo>();
    public static List<SubEmitterInfo> subEmitterList = new List<SubEmitterInfo>();
    public static List<CollisionPlaneInfo> collisionPlaneList = new List<CollisionPlaneInfo>();
    public static List<DynamicBoneInfo> dynamicBoneList = new List<DynamicBoneInfo>();
    public static Dictionary<string, GameObject> allGameObjects = new Dictionary<string, GameObject>();
    public static List<GameObject> delayReleaseList = new List<GameObject>();

    public static bool isSceneConvert = false;

    //public static Dictionary<int, GameObject> particleSystemList = new Dictionary<int, GameObject>();


    public static long PreFilter(List<object> jsonStr,ref Dictionary<long, TreeNode> treeNode_dict)
    {
        long rootId = 0;
        GameObject rootObj = null;
        for (int n = 0; n < jsonStr.Count; n++)
        {
            string object_s = (string)jsonStr[n].ToString();
            var jsonStr_n = JsonConvert.DeserializeObject<Dictionary<object, object>>(object_s);
            long localId = long.Parse(jsonStr_n["localId"].ToString());
            int typeId = int.Parse(jsonStr_n["typeId"].ToString());
            if (jsonStr_n.ContainsKey("Node"))
            {
                TreeNode node = new TreeNode(localId, typeId, jsonStr_n["Node"].ToString());
                treeNode_dict.Add(localId, node);
            }
            else if (jsonStr_n.ContainsKey("AttachNode"))
            {
                TreeNode node = new TreeNode(localId, typeId, jsonStr_n["AttachNode"].ToString());
                treeNode_dict.Add(localId, node);
            }
            else if (jsonStr_n.ContainsKey("PrefabAsset"))
            {
                var asset_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonStr_n["PrefabAsset"].ToString());
                var rootNode_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(asset_dict["_rootNode"].ToString());
                rootId = long.Parse(rootNode_dict["localId"].ToString());

            }else if (jsonStr_n.ContainsKey("PrefabInstance"))
            {
                TreeNode node = new TreeNode(localId, typeId, jsonStr_n["PrefabInstance"].ToString());
                treeNode_dict.Add(localId, node);
            }
            else if (jsonStr_n.ContainsKey("PlaneNode"))
            {
                TreeNode node = new TreeNode(localId, typeId, jsonStr_n["PlaneNode"].ToString());
                treeNode_dict.Add(localId, node);
            }
            else
            {
                TreeNode node = null;
                if (jsonStr_n.ContainsKey("Transform"))
                    node = new TreeNode(localId, typeId, jsonStr_n["Transform"].ToString());
                else if (jsonStr_n.ContainsKey("CameraComponent"))
                    node = new TreeNode(localId, typeId, jsonStr_n["CameraComponent"].ToString());
                else if (jsonStr_n.ContainsKey("MeshRendererComponent"))
                    node = new TreeNode(localId, typeId, jsonStr_n["MeshRendererComponent"].ToString());
                else if (jsonStr_n.ContainsKey("AnimationComponent"))
                    node = new TreeNode(localId, typeId, jsonStr_n["AnimationComponent"].ToString());
                else if (jsonStr_n.ContainsKey("Animator"))
                    node = new TreeNode(localId, typeId, jsonStr_n["Animator"].ToString());
                else if (jsonStr_n.ContainsKey("SkinnedMeshRendererComponent"))
                    node = new TreeNode(localId, typeId, jsonStr_n["SkinnedMeshRendererComponent"].ToString());
                else if (jsonStr_n.ContainsKey("RuntimeMeshRendererComponent"))
                    node = new TreeNode(localId, typeId, jsonStr_n["RuntimeMeshRendererComponent"].ToString());
                else if (jsonStr_n.ContainsKey("TrailRenderer"))
                    node = new TreeNode(localId, typeId, jsonStr_n["TrailRenderer"].ToString());
                else if (jsonStr_n.ContainsKey("ParticleSystemComponent"))
                    node = new TreeNode(localId, typeId, jsonStr_n["ParticleSystemComponent"].ToString());
                else if (jsonStr_n.ContainsKey("ParticleSystemRendererComponent"))
                    node = new TreeNode(localId, typeId, jsonStr_n["ParticleSystemRendererComponent"].ToString());
				else if (jsonStr_n.ContainsKey("DynamicBone"))
					node = new TreeNode(localId, typeId, jsonStr_n["DynamicBone"].ToString());
				else if (jsonStr_n.ContainsKey("ScriptComponent"))
                    node = new TreeNode(localId, typeId, jsonStr_n["ScriptComponent"].ToString());
                //else if (jsonStr_n.ContainsKey("TextMeshProUI"))
                //    node = new TreeNode(localId, typeId, jsonStr_n["TextMeshProUI"].ToString());
                if (node != null)
                    treeNode_dict.Add(localId, node);
            }
        }
        return rootId;
    }

    public static float safeGetDictValue(Dictionary<object,object> nodeDict, string key, float defalutValue)
    {
        object value = null;
        nodeDict.TryGetValue(key, out value);
        if (value != null)
            return float.Parse(value.ToString());
        else
            return defalutValue;
    }

    public static int safeGetDictValue(Dictionary<object, object> nodeDict, string key, int defalutValue)
    {
        object value = null;
        nodeDict.TryGetValue(key, out value);
        if (value != null)
            return int.Parse(value.ToString());
        else
            return defalutValue;
    }

    public static WrapMode ConvertAnimationWrapMode(int jnityWrapMode)
    {
        switch (jnityWrapMode)
        {
            case 0:
                return WrapMode.PingPong;
            case 1:
                return WrapMode.Loop;
            case 2:
                return WrapMode.Clamp;
        }
        return WrapMode.Clamp;
    }

    public static ParticleSystemVertexStream ConvertParticleSystemVertexStream(int jnityVertexStream)
    {
        switch (jnityVertexStream)
        {
            case 0:
                return ParticleSystemVertexStream.Position;
            case 1:
                return ParticleSystemVertexStream.Color;
            case 2:
                return ParticleSystemVertexStream.UV;
            case 3:
                return ParticleSystemVertexStream.UV2;
            case 4:
                return ParticleSystemVertexStream.UV3;
            case 5:
                return ParticleSystemVertexStream.UV4;
            case 6:
                return ParticleSystemVertexStream.Custom1X;
            case 7:
                return ParticleSystemVertexStream.Custom1XY;
            case 8:
                return ParticleSystemVertexStream.Custom1XYZ;
            case 9:
                return ParticleSystemVertexStream.Custom1XYZW;
            case 10:
                return ParticleSystemVertexStream.Custom2X;
            case 11:
                return ParticleSystemVertexStream.Custom2XY;
            case 12:
                return ParticleSystemVertexStream.Custom2XYZ;
            case 13:
                return ParticleSystemVertexStream.Custom2XYZW;
            case 14:
                return ParticleSystemVertexStream.Normal;
        }

        return ParticleSystemVertexStream.Position;
    }



    public static Material[] ConvertMats(string matsContent,GameObject owner)
    {
        //同步材質      
        string assetPath;
        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        var matsList = JsonConvert.DeserializeObject<List<object>>(matsContent);
        if (matsList == null)
            return null;
        Material[] newMaterials = new Material[matsList.Count];
        for (int n = 0; n < matsList.Count; n++)
        {
            if (matsList[n] == null)
                continue;
            var matInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(matsList[n].ToString());
            string mat_uuid = matInfo_dict["uuid"].ToString();
            string unity_mat_path;
            string jnity_mat_path;
            if (resConfig.resFiles.ContainsKey(mat_uuid))
            {
                jnity_mat_path = resConfig.resFiles[mat_uuid].j_path;
                assetPath = FileUtil.GetAssetPath(jnity_mat_path);
                unity_mat_path = Application.dataPath + assetPath.Replace("Assets", "");

			}
            else
            {
                jnity_mat_path = JnityBuildInRes.buildInResMap[mat_uuid].path + "x";
                assetPath = Jnity2Unity.unityRelativeAssetPath + JnityBuildInRes.buildInResMap[mat_uuid].idmap_path;
                unity_mat_path = Jnity2Unity.unityAssetPath + "/" + JnityBuildInRes.buildInResMap[mat_uuid].idmap_path;
            }

            FileInfo fileInfo = new FileInfo(unity_mat_path);
			Material newMat = null;
			if (fileInfo.Exists)
			{				
				newMat = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Material)) as Material;
			}
            MatSync.SyncMatFile(owner.gameObject.name, jnity_mat_path, ref newMat);
            newMaterials[n] = newMat;
            string dir = fileInfo.Directory.FullName;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
			if (newMat)
			{
				if (!fileInfo.Exists)
				{
					AssetDatabase.CreateAsset(newMat, assetPath);
				}
				else
				{
					AssetDatabase.SaveAssetIfDirty(newMat);
					AssetDatabase.Refresh();
				}
			}

		}
        return newMaterials;
    }

    public static Transform findChild(Transform parent, string childName)
    {
        int n = parent.childCount;
        for(int k = 0; k < n; k++)
        {
            Transform child = parent.GetChild(k);          
            if (child.name.Equals(childName))
                return child;
            Transform t = findChild(child, childName);
            if (t)
                return t;
        }
        return null;      
    }


    public static Transform getRootTransform(Transform t)
    {
        Transform parent = t.parent;
        while (parent != null)
        {
            t = parent;
            parent = t.parent;
        }
        return t;
    }

    public static bool hasComponent<T>(GameObject baseObj)
    {
        T t;
        baseObj.TryGetComponent<T>(out t);
        return t != null ? true : false;
    }

    public static bool hasComponentInChild<T>(GameObject baseObj)
    {
        T[] t = baseObj.GetComponentsInChildren<T>();       
        return t.Length > 0 ? true : false;
    }

    public static Transform getAvatarParent(Transform t)
    {
        Transform parent = t.parent;
        while (parent!=null && !hasComponent<Animator>(parent.gameObject))
        {
            t = parent;
            parent = t.parent;
        }
        return parent;
    }

    public static Transform getChildByName(Transform t,string childName)
    {
        for(int n = 0; n < t.childCount; n++)
        {
            Transform child = t.GetChild(n);
            if (child.name.Equals(childName))
            {
                return child;
            }
        }
        return null;
    }

    public static bool hasBipChild(Transform t, string boneName)
    {
        Transform[] childts = t.GetComponentsInChildren<Transform>();
        for (int n = 0; n < childts.Length; n++)
        {
            Transform child = childts[n];
            if (child.name.ToLower().Contains(boneName))
            {
                return true;
            }
        }
        return false;
    }

    public static Transform deepGetChildByName(Transform t, string childName)
    {
        Transform[] childts = t.GetComponentsInChildren<Transform>();
        for (int n = 0; n < childts.Length; n++)
        {
            Transform child = childts[n];
            if (child.name.Equals(childName))
            {
                return child;
            }
        }
        return null;
    }

    public static ParticleSystemCullingMode convertCullingMode(int jnityCullingMode)
    {
        switch (jnityCullingMode)
        {
            case 0:
                return ParticleSystemCullingMode.AlwaysSimulate;
            case 1:
                return ParticleSystemCullingMode.Pause;
            case 2:
                return ParticleSystemCullingMode.Automatic;
            case 3:
                return ParticleSystemCullingMode.PauseAndCatchup;
        }
        return ParticleSystemCullingMode.AlwaysSimulate;

    }

    public static ParticleSystemSubEmitterType convertSubEmitterType(int jnityEmitterType)
    {
        switch (jnityEmitterType)
        {
            case 0:
                return ParticleSystemSubEmitterType.Birth;
            case 1:
                return ParticleSystemSubEmitterType.Death;
            case 2:
                return ParticleSystemSubEmitterType.Collision;        
        }
        return ParticleSystemSubEmitterType.Birth;
    }

    public static ParticleSystemSubEmitterProperties convertSubEmitterProperty(int jnityEmitterProperty)
    {
        switch (jnityEmitterProperty)
        {
            case 0:
                return ParticleSystemSubEmitterProperties.InheritNothing;
            case 1:
                return ParticleSystemSubEmitterProperties.InheritEverything;
            case 2:
                return ParticleSystemSubEmitterProperties.InheritColor;
            case 3:
                return ParticleSystemSubEmitterProperties.InheritSize;
            case 4:
                return ParticleSystemSubEmitterProperties.InheritRotation;
        }
        return ParticleSystemSubEmitterProperties.InheritNothing;
    }



    public static ParticleSystemShapeType convertShapeType(int jnityShapeType)
    {
        if (jnityShapeType <= 4)
        {
            return (ParticleSystemShapeType)jnityShapeType;
        }
        else if (jnityShapeType == 5)
        {
            return ParticleSystemShapeType.Box;
        }else if( jnityShapeType == 6)
        {
            return ParticleSystemShapeType.ConeShell;
        }else if(jnityShapeType == 7)
        {
            return ParticleSystemShapeType.ConeVolume;
        }else if(jnityShapeType == 8)
        {
            return ParticleSystemShapeType.ConeVolumeShell;
        }
        else if(jnityShapeType == 10)
        {
            return ParticleSystemShapeType.Circle;
        }
        else if (jnityShapeType == 16)
            return ParticleSystemShapeType.Mesh;
        else
        {
            return (ParticleSystemShapeType)(jnityShapeType + 1);
        }
    }


    public static float getParticleSystemRotateDir(Dictionary<object,object> moduleDict)
    {
        float rotDirection = float.Parse(moduleDict["_randRotDirection"].ToString());
        if (rotDirection > 0)
            return rotDirection;
        rotDirection = float.Parse(moduleDict["_randRotDirectionY"].ToString());
        if (rotDirection > 0)
            return rotDirection;
        rotDirection = float.Parse(moduleDict["_randRotDirectionZ"].ToString());
        if (rotDirection > 0)
            return rotDirection;
        return 0;
    }

    static Gradient ConvertGradient(Dictionary<object,object> gradientDict)
    {
        var color_keys = JsonConvert.DeserializeObject<List<object>>(gradientDict["_colorKeys"].ToString());
        GradientColorKey[] colorKeys = new GradientColorKey[color_keys.Count];
        for (int n = 0; n < color_keys.Count; n++)
        {
            var colorKeysDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(color_keys[n].ToString());
            var colorValue = JsonConvert.DeserializeObject<Dictionary<object, object>>(colorKeysDict["color"].ToString());
            colorKeys[n].color = new Color(float.Parse(colorValue["r"].ToString()), float.Parse(colorValue["g"].ToString()), float.Parse(colorValue["b"].ToString()), 1);
            colorKeys[n].time = float.Parse(colorKeysDict["time"].ToString());
        }

        var alpha_keys = JsonConvert.DeserializeObject<List<object>>(gradientDict["_alphaKeys"].ToString());
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[alpha_keys.Count];
        for (int n = 0; n < alpha_keys.Count; n++)
        {
            var alphaKeysDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(alpha_keys[n].ToString());
            alphaKeys[n].alpha = float.Parse(alphaKeysDict["alpha"].ToString());
            alphaKeys[n].time = float.Parse(alphaKeysDict["time"].ToString());
        }

        Gradient gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);
        return gradient;
    }

    public static ParticleSystem.MinMaxGradient ConvertMinMaxGradient(ParticleSystem.MinMaxGradient mmg, Dictionary<object, object> curveNodeInfo)
    {
        mmg.mode = (ParticleSystemGradientMode)int.Parse(curveNodeInfo["_mode"].ToString());
        var minGradientDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(curveNodeInfo["_gradientMin"].ToString());
        mmg.gradientMin = ConvertGradient(minGradientDict);
        var maxGradientDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(curveNodeInfo["_gradientMax"].ToString());
        mmg.gradientMax = ConvertGradient(maxGradientDict);
        var colorMinDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(curveNodeInfo["_colorMin"].ToString());
        mmg.colorMin = new Color(float.Parse(colorMinDict["r"].ToString()), float.Parse(colorMinDict["g"].ToString()), float.Parse(colorMinDict["b"].ToString()), float.Parse(colorMinDict["a"].ToString()));
        var colorMaxDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(curveNodeInfo["_colorMax"].ToString());
        mmg.colorMax = new Color(float.Parse(colorMaxDict["r"].ToString()), float.Parse(colorMaxDict["g"].ToString()), float.Parse(colorMaxDict["b"].ToString()), float.Parse(colorMaxDict["a"].ToString()));
        return mmg;
    }

    public static ParticleSystem.Burst[] ConvertEmissionBurst( Dictionary<object, object> burstDataInfo)
    {
        var burstList = JsonConvert.DeserializeObject<List<object>>(burstDataInfo["data"].ToString());
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[burstList.Count];
        for(int k = 0; k < burstList.Count; k++)
        {
            bursts[k] = new ParticleSystem.Burst();
            var burstNodeInfo = JsonConvert.DeserializeObject<Dictionary<object, object>>(burstList[k].ToString());
            bursts[k].time = float.Parse(burstNodeInfo["time"].ToString());
            bursts[k].cycleCount = int.Parse(burstNodeInfo["cycles"].ToString());
            bursts[k].repeatInterval = float.Parse(burstNodeInfo["interval"].ToString());
            bursts[k].count = ConvertMinMaxCurve(bursts[k].count, JsonConvert.DeserializeObject< Dictionary<object, object>>(burstNodeInfo["count"].ToString()));
        }
        return bursts;
    }


    public static ParticleSystem.MinMaxCurve ConvertMinMaxCurve(ParticleSystem.MinMaxCurve mmc, Dictionary<object, object> curveNodeInfo, bool needFlip = false, bool Deg2Rad = false)
    {
        float multiDeg2Rad = 1;
        if (Deg2Rad)
        {
            multiDeg2Rad *= Mathf.Deg2Rad;
        }
        mmc.mode = (ParticleSystemCurveMode)int.Parse(curveNodeInfo["_mode"].ToString());
        int sign = needFlip ? -1 : 1;
        //if(mmc.mode == ParticleSystemCurveMode.Constant || mmc.mode == ParticleSystemCurveMode.TwoConstants)
        //{
            mmc.constantMin = multiDeg2Rad * float.Parse(curveNodeInfo["_min"].ToString()) * sign;
            mmc.constantMax = multiDeg2Rad * float.Parse(curveNodeInfo["_max"].ToString()) * sign;
        //}
        //else
        //{
            mmc.curveMultiplier = Mathf.Max(Mathf.Abs(mmc.constantMin), Mathf.Abs(mmc.constantMax));
            mmc.curveMin =  ConvertAnimationCurve(curveNodeInfo, "_curveMin", needFlip, false, false);
            mmc.curveMax =  ConvertAnimationCurve(curveNodeInfo, "_curveMax", needFlip, false, false);
        //}
        return mmc;
    }

    public static AnimationCurve ConvertAnimationCurve(Dictionary<object, object> curveNodeInfo, string curveName, bool needFlip, bool needRotate180, bool Deg2Rad)
    {
        float multiDeg2Rad = 1;
        if (Deg2Rad)
        {
            multiDeg2Rad *= Mathf.Deg2Rad;
        }
        AnimationCurve animCurve = new AnimationCurve();
        var curveAttribute = JsonConvert.DeserializeObject<Dictionary<object, object>>(curveNodeInfo[curveName].ToString());
        var curveKeys = JsonConvert.DeserializeObject<List<object>>(curveAttribute["_keys"].ToString());
        animCurve.preWrapMode = ConvertUtil.ConvertAnimationWrapMode(int.Parse(curveAttribute["_preInfinity"].ToString()));
        animCurve.postWrapMode = ConvertUtil.ConvertAnimationWrapMode(int.Parse(curveAttribute["_postInfinity"].ToString()));
        for (int n = 0; n < curveKeys.Count; n++)
        {
            var keyFrame_str = curveKeys[n].ToString();
            var keyFrame_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(keyFrame_str);
            var time = keyFrame_Dict["t"];
            var value = keyFrame_Dict["v"];
            var leftTangentValue = keyFrame_Dict["l"];
            var rightTangetValue = keyFrame_Dict["r"];
            var tangentMode = int.Parse(keyFrame_Dict["tangentMode"].ToString());

            Keyframe keyframe = new Keyframe();
            keyframe.time = float.Parse(time.ToString());
            keyframe.value = float.Parse(value.ToString()) * multiDeg2Rad;

            keyframe.inTangent = float.Parse(leftTangentValue.ToString());
            keyframe.outTangent = float.Parse(rightTangetValue.ToString());
            if (needFlip)
                keyframe.value = -keyframe.value;
            if (needRotate180)
                keyframe.value = (keyframe.value + 180) % 360;

            if (needFlip)
            {
                keyframe.inTangent = -keyframe.inTangent;
                keyframe.outTangent = -keyframe.outTangent;
            }
            animCurve.AddKey(keyframe);

            AnimationUtility.TangentMode leftTangent;
            AnimationUtility.TangentMode rightTangent;
            AnimationConvert.ConvertTangetMode(tangentMode, out leftTangent, out rightTangent, animCurve, n);
            AnimationUtility.SetKeyLeftTangentMode(animCurve, n, leftTangent);
            AnimationUtility.SetKeyRightTangentMode(animCurve, n, rightTangent);
        }

        return animCurve;
    }


    public static void ReleaseObjects()
    {
        for(int k = 0; k < delayReleaseList.Count; k++)
        {
            GameObject.DestroyImmediate(delayReleaseList[k]);
        }
        delayReleaseList.Clear();
    }

	public static void AdjustBakeAxisConversion (GameObject owner, GameObject meshObj)
	{
		Vector3 ownerRot = owner.transform.localRotation.eulerAngles;
		Vector3 meshRot = meshObj.transform.localRotation.eulerAngles;
		ownerRot = new Vector3(ownerRot.x + meshRot.x, ownerRot.y + meshRot.y, ownerRot.z + meshRot.z);
		owner.transform.localRotation = Quaternion.Euler(ownerRot);
	}

}
