using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Newtonsoft.Json;
using UnityEditor;
using System;
using System.IO;
using UnityEditor.Animations;
using TMPro;
public class PrefabConvert
{

    public static GameObject ConvertPrefabRootNode(Transform parentT, string prefabUUID, List<object> jsonStr)
    {
        Dictionary<long, TreeNode> treeNode_dict = new Dictionary<long, TreeNode>();
        long rootId = ConvertUtil.PreFilter(jsonStr, ref treeNode_dict);
        GameObject rootObj = null;
        if (rootId != 0)
        {
            TreeNode rootNode = treeNode_dict[rootId];
            rootObj = ConvertRootNode(parentT, prefabUUID, rootNode, treeNode_dict);
        }
        else
        {
            Debug.LogError("rootId is not correct:" + rootId);
        }

        return rootObj;
    }

    public static GameObject ConvertPrefab(Transform parentT,string prefabUUID, string jnityPrefabPath, bool doAttach=false, bool needRelease = false)
    {
        string prefabFile = jnityPrefabPath;
        string assetPath = FileUtil.GetAssetPath(jnityPrefabPath);
        string unityPrefabPath = assetPath;
        unityPrefabPath = Application.dataPath.Replace("Assets","") + unityPrefabPath;
       
        FileInfo unityPrefabFile = new FileInfo(unityPrefabPath);
        string dir = unityPrefabFile.Directory.FullName;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string text = FileUtil.ReadTexTFile(prefabFile);
        var jsonStr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(text);
        if (doAttach)
        {
            //ConvertUtil.attachNodeList.Clear();
            ConvertUtil.animNodeList.Clear();
            ConvertUtil.subEmitterList.Clear();
            ConvertUtil.collisionPlaneList.Clear();
            ConvertUtil.dynamicBoneList.Clear();
        }
        GameObject prefabObj = PrefabConvert.ConvertPrefabRootNode(parentT, prefabUUID, jsonStr);
        if (doAttach)
        {
            //for (int k = 0; k < ConvertUtil.attachNodeList.Count; k++)
            //{
            //    AttachNodeInfo attachInfo = ConvertUtil.attachNodeList[k];
            //    string attachBoneName = attachInfo.attchNodeName;
            //    Transform boneT = ConvertUtil.deepGetChildByName(ConvertUtil.getRootTransform(attachInfo.parentObj.transform), attachBoneName);
            //    if (boneT)
            //    {
            //        attachInfo.selfObj.transform.parent = boneT;
            //        attachInfo.selfObj.transform.localPosition = Vector3.zero;
            //        attachInfo.selfObj.transform.localRotation = Quaternion.identity;
            //        attachInfo.selfObj.transform.localScale = Vector3.one;
            //    }
            //    else
            //    {
            //        ReportSystem.OutputLog($"解析AttachNode节点[{attachInfo.selfObj.name}]出错，绑定的骨骼{attachBoneName}找不到！");
            //    }
            //}
            //ConvertUtil.attachNodeList.Clear();

            for(int k = 0; k < ConvertUtil.animNodeList.Count; k++)
            {
                AnimNodeInfo animNodeInfo = ConvertUtil.animNodeList[k];
                ParseAnimation(animNodeInfo.ownerObj, animNodeInfo.treeNode);
            }
            ConvertUtil.animNodeList.Clear();

            for(int k = 0; k < ConvertUtil.subEmitterList.Count; k++)
            {
                string globalId = prefabUUID + "|" + ConvertUtil.subEmitterList[k].localId;
                GameObject subObj = ConvertUtil.allGameObjects[globalId];
                ParticleSystem subps = subObj.GetComponent<ParticleSystem>();
                ParticleSystem.ShapeModule shapeModule = subps.shape;
                shapeModule.rotation = new Vector3(shapeModule.rotation.x + 90, shapeModule.rotation.y, shapeModule.rotation.z);
                ParticleSystem parentPs = ConvertUtil.subEmitterList[k].parentObj.GetComponent<ParticleSystem>();
                parentPs.subEmitters.AddSubEmitter(subps, ConvertUtil.subEmitterList[k].type, ConvertUtil.subEmitterList[k].property);
            }
            ConvertUtil.subEmitterList.Clear();

            for(int k = 0; k < ConvertUtil.collisionPlaneList.Count; k++)
            {
                string globalId = prefabUUID + "|" + ConvertUtil.collisionPlaneList[k].localId;
                GameObject planeObj = ConvertUtil.allGameObjects[globalId];
                ParticleSystem ps = ConvertUtil.collisionPlaneList[k].parentObj.GetComponent<ParticleSystem>();
                ps.collision.AddPlane(planeObj.transform);
            }
            ConvertUtil.collisionPlaneList.Clear();

			for (int k = 0; k < ConvertUtil.dynamicBoneList.Count; k++)
			{
				GameObject dynamicBoneObj = ConvertUtil.dynamicBoneList[k].parentObj;
				
				string rootBoneName = ConvertUtil.dynamicBoneList[k].rootBoneName;
				Transform boneT = ConvertUtil.deepGetChildByName(dynamicBoneObj.transform, rootBoneName);
				if (boneT == null)
				{
					ReportSystem.OutputLog($"同步DynamicBone出错，[{dynamicBoneObj.name}]节点引用的根骨骼[{rootBoneName}]找不到,请检查是否和骨骼动画都放在了当前prefab里导出！");
					continue;
				}
				MonoBehaviour db = ConvertUtil.dynamicBoneList[k].db;
				Jnity2Unity._setBoneRootCallBack(db, boneT);
			}
			ConvertUtil.dynamicBoneList.Clear();
		}


        //GameObject rootObj = new GameObject();
        //prefabObj.transform.parent = rootObj.transform;
        ////rootObj.transform.Rotate(Vector3.up * 180);
        //prefabObj.transform.parent = null;
        //GameObject.DestroyImmediate(rootObj);

        if (prefabObj)
        {
			//if (File.Exists(unityPrefabPath))
			//{
			//	File.Delete(unityPrefabPath);
			//}

			string prefabPath = assetPath;
            PrefabUtility.SaveAsPrefabAsset(prefabObj, prefabPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            if (needRelease)
            {
                if (!ConvertUtil.isSceneConvert)
                {
                    GameObject.DestroyImmediate(prefabObj);
                    return null;
                }
                else
                    ConvertUtil.delayReleaseList.Add(prefabObj);

            }
        }

        return prefabObj;
    }

    public static GameObject ConvertRootNode(Transform parentT,string prefabUUID, TreeNode rootNode, Dictionary<long, TreeNode> treeNode_dict)
    {
        return ConvertChildNode(prefabUUID, rootNode, treeNode_dict, parentT);
    }

    static Transform GetChildByName(Transform parent, string name)
    {
        if (parent == null)
            return null;
        Transform[] transforms = parent.GetComponentsInChildren<Transform>();
        foreach(Transform t in transforms)
        {
            if (t.name.Equals(name))
            {
                return t;
            }
        }
        return null;
    }

    static string GetTagNameByIndex(int index)
    {   
        string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
        if (index < 0)
            return "Untagged";
        if (index >= tags.Length)
            return tags[0];
        return tags[index];
    }

    public static GameObject ConvertChildNode(string prefabUUID, TreeNode rootNode, Dictionary<long, TreeNode> treeNode_dict, Transform parent)
    {
        GameObject baseObject=null;
        try
        {          
            var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(rootNode.nodeInfo);
            string objectName = nodeInfo_dict["_name"].ToString();
            baseObject = new GameObject();
            baseObject.transform.parent = parent;

            if (nodeInfo_dict["_prefabInstance"] != null)
            {
                var prefabInstance = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_prefabInstance"].ToString());
                if (prefabInstance != null)
                {
                    long prefab_instance_id = long.Parse(prefabInstance["localId"].ToString());
                    if (treeNode_dict.ContainsKey(prefab_instance_id))
                    {
                        TreeNode prefab_instance_node = treeNode_dict[prefab_instance_id];
                        ParsePrefabInstance(ref baseObject, prefab_instance_node, treeNode_dict);
                    }
                }
            }else if(rootNode.typeId == 24) //attachnode
            {
				//AttachNodeInfo attachInfo = new AttachNodeInfo();
				//attachInfo.parentObj = parent.gameObject;
				//attachInfo.selfObj = baseObject;
				//attachInfo.attchNodeName = nodeInfo_dict["_attachBoneName"].ToString();
				//ConvertUtil.attachNodeList.Add(attachInfo);
				Jnity2Unity._addAttachNodeCallBack(baseObject, nodeInfo_dict["_attachBoneName"].ToString());
            }
            baseObject.name = objectName;
            baseObject.SetActive(bool.Parse(nodeInfo_dict["_visible"].ToString()));
			baseObject.layer = EditorSettingSync.getUnityLayerByJnityLayer(int.Parse(nodeInfo_dict["_layer"].ToString()));
			baseObject.tag = EditorSettingSync.getUnityTagNameByJnityTagValue(int.Parse(nodeInfo_dict["_tag"].ToString()));

			var componentsList = JsonConvert.DeserializeObject<List<object>>(nodeInfo_dict["_components"].ToString());
            for (int n = 0; n < componentsList.Count; n++)
            {
                var component_id_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(componentsList[n].ToString());
                long component_id = long.Parse(component_id_dict["localId"].ToString());
                if (treeNode_dict.ContainsKey(component_id))
                {
                    TreeNode component_node = treeNode_dict[component_id];
                    component_node.objectName = objectName;
                    if (component_node.typeId == 102)
                    {
                        ParseTransform(baseObject, component_node);
                    }
                    else if (component_node.typeId == 63)
                    {
                        ParseCamera(baseObject, component_node);
                    }
                    else if (component_node.typeId == 75 || component_node.typeId == 74 )
                    {
                        ParseMeshRender(baseObject, component_node, component_node.typeId);
                    }else if(component_node.typeId == 65)
                    {
                        // ParseAnimation(parentObject, component_node);
                        AnimNodeInfo animNode = new AnimNodeInfo();
                        animNode.ownerObj = baseObject;
                        animNode.treeNode = component_node;
                        ConvertUtil.animNodeList.Add(animNode);
                    }else if( component_node.typeId == 64)
                    {
                        ParseAnimator(ref baseObject, component_node);
                    }else if(component_node.typeId == 76)
                    {
                        ParseSkinMeshRender(baseObject, component_node);
                    }else if(component_node.typeId == 77)
                    {
                        ParseTrailRender(baseObject, component_node);
                    }
					//else if(component_node.typeId == 78)
     //               {
     //                   ParseParticleSystem(baseObject, component_node);
     //               }else if(component_node.typeId == 79)
     //               {
     //                   ParseParticleSystemRender(baseObject, component_node);
     //               }
					else if (component_node.typeId == 95)
					{
						Dictionary<object, object> dynamicBone_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
						Jnity2Unity._dynamicBoneDelegate(baseObject, dynamicBone_dict);
						//ParseDynamicBone(baseObject, component_node);
					}
					else if(component_node.typeId == 430)
                    {
                        ParseScriptComponent(baseObject, component_node);
                    }
					//else if(component_node.typeId == 499)
     //               {
     //                   ParseTextMeshPro(baseObject, component_node);
     //               }
                }
            }

			var particleComponents = new List<TreeNode>();
			for (int n = 0; n < componentsList.Count; n++)
			{
				var component_id_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(componentsList[n].ToString());
				long component_id = long.Parse(component_id_dict["localId"].ToString());
				if (treeNode_dict.ContainsKey(component_id))
				{
					TreeNode component_node = treeNode_dict[component_id];
					if (component_node.typeId == 78)
					{
						particleComponents.Add(component_node);
					}
					else if (component_node.typeId == 79)
					{
						particleComponents.Add(component_node);
					}
				}
			}

			for(int n = particleComponents.Count - 1; n >=0; n--)
			{
				TreeNode component_node = particleComponents[n];
				if (component_node.typeId == 78)
				{
					ParseParticleSystem(baseObject, component_node);
				}
				else if (component_node.typeId == 79)
				{
					ParseParticleSystemRender(baseObject, component_node);
				}
			}


		    var childrenList = JsonConvert.DeserializeObject<List<object>>(nodeInfo_dict["_children"].ToString());
            for (int n = 0; n < childrenList.Count; n++)
            {
                var children_id_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(childrenList[n].ToString());
                long children_id = long.Parse(children_id_dict["localId"].ToString());
                TreeNode children_node = treeNode_dict[children_id];
                GameObject childObj = ConvertChildNode(prefabUUID, children_node, treeNode_dict, baseObject.transform);
                string globalId = prefabUUID + "|" + children_id;
                ConvertUtil.allGameObjects.Add(globalId, childObj);
            }


        }
        catch (Exception ex)
        {
            if(baseObject)
                GameObject.DestroyImmediate(baseObject);
            baseObject = null;
            Debug.LogError(ex);
        }
        return baseObject;
    }

    static Vector3 getJnityEulerRotation(Quaternion q)
    {
        Vector3 v = Vector3.zero;

        float rotationX = Mathf.Atan2(2.0f * (q.w * q.x + q.y * q.z), 1.0f - 2.0f * (q.x * q.x + q.y * q.y));
        float sy = 2.0f * (q.w * q.y - q.z * q.x);
        sy = Mathf.Clamp(sy, -1.0f, 1.0f);
        float rotationY = Mathf.Asin(sy);
        float rotationZ = Mathf.Atan2(2.0f * (q.w * q.z + q.x * q.y), 1.0f - 2.0f * (q.y * q.y + q.z * q.z));

        v.x = Mathf.Rad2Deg * rotationX;
        v.y = Mathf.Rad2Deg * rotationY;
        v.z = -Mathf.Rad2Deg * rotationZ;
        return v;
    }

    static void ParseTransform(GameObject owner, TreeNode component_node)
    {
        Transform transform = owner.transform;
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
        var position_vector = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_localPosition"].ToString());
        transform.localPosition = new Vector3(float.Parse(position_vector["x"].ToString()), float.Parse(position_vector["y"].ToString()), -float.Parse(position_vector["z"].ToString()));
        var quater_vector = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_localRotation"].ToString());
        Quaternion initq = new Quaternion(float.Parse(quater_vector["x"].ToString()), float.Parse(quater_vector["y"].ToString()), -float.Parse(quater_vector["z"].ToString()), -float.Parse(quater_vector["w"].ToString()));
        transform.localRotation = initq;
        var scale_vector = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_localScale"].ToString());
        transform.localScale = new Vector3(float.Parse(scale_vector["x"].ToString()), float.Parse(scale_vector["y"].ToString()), float.Parse(scale_vector["z"].ToString()));
    }

    static void ParseCamera(GameObject owner, TreeNode component_node)
    {
        Camera camera = owner.AddComponent<Camera>();
        //owner.transform.localRotation = owner.transform.localRotation * Quaternion.Euler(0, 180, 0);
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
        camera.enabled = bool.Parse(nodeInfo_dict["_enabled"].ToString());
        camera.fieldOfView = float.Parse(nodeInfo_dict["_fieldOfView"].ToString());
        camera.nearClipPlane = float.Parse(nodeInfo_dict["_nearPlane"].ToString());
        camera.farClipPlane = float.Parse(nodeInfo_dict["_farPlane"].ToString());
        camera.orthographic = bool.Parse(nodeInfo_dict["_isOrthographic"].ToString());
        camera.orthographicSize = float.Parse(nodeInfo_dict["_orthHeight"].ToString());
    }

    static void ParseAnimator(ref GameObject owner, TreeNode component_node)
    {
        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        Animator animator = owner.AddComponent<Animator>();
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
        if(nodeInfo_dict["_avatar"] != null)
        {
            var avatar_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_avatar"].ToString());
            string fbx_uuid = avatar_dict["uuid"].ToString();               
            if (resConfig.resFiles.ContainsKey(fbx_uuid))
            {
                string jnity_fbx_path = resConfig.resFiles[fbx_uuid].j_path;
                if (!jnity_fbx_path.EndsWith(".skel"))
                {

                    string assetPath = FileUtil.GetAssetPath(jnity_fbx_path);
                    string unity_fbx_path = Application.dataPath + assetPath.Replace("Assets", "");
                    FileInfo file = new FileInfo(unity_fbx_path);
                    //同步fbx文件 from jnity to unity
                    //if (!file.Exists)
                    //{
                    FileUtil.CopyFile(jnity_fbx_path, unity_fbx_path);
                    AssetDatabase.Refresh();
                    ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(assetPath);
                    FbxSync.SyncMetaFile(importer, resConfig.resFiles[fbx_uuid].j_metaPath);
                    resConfig.resFiles[fbx_uuid].u_path = unity_fbx_path;
                    //}

                    UnityEngine.Object[] fbxObjects = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                    for (int n = 0; n < fbxObjects.Length; n++)
                    {
                        if (fbxObjects[n] is Avatar)
                        {
                            Avatar avartar = (Avatar)fbxObjects[n];
                            animator.avatar = avartar;
                            animator.ApplyBuiltinRootMotion();
                            break;
                        }
                    }
                }
                else
                {
                    ReportSystem.OutputLog($"解析节点[{owner}]的Animator组件出错, Animattor引用了老版本的骨骼文件[{jnity_fbx_path}]!");

                }

            }
        }

        if (nodeInfo_dict["_controller"] != null)
        {
            var controller_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_controller"].ToString());
            string controller_uuid = controller_dict["uuid"].ToString();
            string jnity_controller_path = resConfig.resFiles[controller_uuid].j_path;
            string assetPath = FileUtil.GetAssetPath(jnity_controller_path);
            string unity_controller_path = Application.dataPath + assetPath.Replace("Assets", "");
            FileInfo file = new FileInfo(unity_controller_path);
            //if (!file.Exists)
            //{
            string dir = file.Directory.FullName;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            AnimControllerSync.ConvertController(owner.transform,jnity_controller_path, assetPath);
            //}

            AnimatorController animatorController = AssetDatabase.LoadAssetAtPath(assetPath, typeof(AnimatorController)) as AnimatorController;
            animator.runtimeAnimatorController = animatorController;
        }
    }

    public static void ParseAnimation(GameObject owner, TreeNode component_node)
    {
        Animation animation = owner.AddComponent<Animation>();
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
        animation.enabled = bool.Parse(nodeInfo_dict["_enabled"].ToString());
        animation.playAutomatically = true;// bool.Parse(nodeInfo_dict["_autoPlay"].ToString());  
        string defualt_clip_name = nodeInfo_dict["_defaultClip"].ToString();
        var clipsList = JsonConvert.DeserializeObject<List<object>>(nodeInfo_dict["_clips"].ToString());
        for(int n = 0; n < clipsList.Count; n++)
        {
            if (clipsList[n] == null)
                continue;
            var clipsDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(clipsList[n].ToString());
            string clip_uuid = clipsDict["uuid"].ToString();
            JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
            string jnity_clip_path = resConfig.resFiles[clip_uuid].j_path;
            string assetPath = FileUtil.GetAssetPath(jnity_clip_path);
            assetPath = assetPath.Replace(".clip", "");
            assetPath = assetPath.Replace(".animatorclip", "");
            assetPath = assetPath + ".anim";
            string unity_clip_path = Application.dataPath + assetPath.Replace("Assets", "");
            FileInfo file = new FileInfo(unity_clip_path);
            AnimationClip animationClip;
            //if (!file.Exists)
            //{
            string dir = file.Directory.FullName;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            animationClip = AnimationSync.ConvertAnimation(jnity_clip_path, owner.transform);          
            AssetDatabase.CreateAsset(animationClip, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            //}
            animationClip = AssetDatabase.LoadAssetAtPath(assetPath, typeof(AnimationClip)) as AnimationClip;
            animationClip.legacy = true;
            animation.AddClip(animationClip, animationClip.name);
            if (defualt_clip_name.Equals(animationClip.name))
            {
                animation.clip = animationClip;
            }
        }

    }


    static void ParseSkinMeshRender(GameObject owner, TreeNode component_node)
    {
        SkinnedMeshRenderer smr;
        owner.TryGetComponent(out smr);
        if(!smr)
            smr = owner.AddComponent<SkinnedMeshRenderer>();
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
        smr.enabled = bool.Parse(nodeInfo_dict["_enabled"].ToString());
        smr.shadowCastingMode = (ShadowCastingMode)int.Parse(nodeInfo_dict["_castShadows"].ToString());
        smr.receiveShadows = bool.Parse(nodeInfo_dict["_receiveShadows"].ToString());
        smr.sortingOrder = int.Parse(nodeInfo_dict["_sortingOrder"].ToString());
        smr.sortingLayerID = int.Parse(nodeInfo_dict["_sortingLayerId"].ToString());
        smr.staticShadowCaster = bool.Parse(nodeInfo_dict["_isStaticShadowCaster"].ToString());
        var bounds_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_bounds"].ToString());
        var center_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(bounds_dict["center"].ToString());
        var extents_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(bounds_dict["extents"].ToString());
        Bounds bounds = new Bounds();
        bounds.center = new Vector3(float.Parse(center_dict["x"].ToString()), float.Parse(center_dict["y"].ToString()), float.Parse(center_dict["z"].ToString()));
        bounds.extents = new Vector3(float.Parse(extents_dict["x"].ToString()), float.Parse(extents_dict["y"].ToString()), float.Parse(extents_dict["z"].ToString()));
        smr.localBounds = bounds;

        ////同步mesh
        var mesh = nodeInfo_dict["_mesh"].ToString();
        var meshInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(mesh);
        string mesh_uuid = meshInfo_dict["uuid"].ToString();
        string localId = meshInfo_dict["localId"].ToString(); //fbx中用哪个mesh资源
        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        string jnity_mesh_path = resConfig.resFiles[mesh_uuid].j_path;
        string assetPath = FileUtil.GetAssetPath(jnity_mesh_path);
        string unity_mesh_path = Application.dataPath + assetPath.Replace("Assets", "");
        if (!unity_mesh_path.EndsWith(".mod"))
        {
            FileInfo file = new FileInfo(unity_mesh_path);
            //同步mesh文件 from jnity to unity
            //if (!file.Exists)
            //{
            FileUtil.CopyFile(jnity_mesh_path, unity_mesh_path);
            AssetDatabase.Refresh();
            ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(assetPath);
            FbxSync.SyncMetaFile(importer, resConfig.resFiles[mesh_uuid].j_metaPath);
            resConfig.resFiles[mesh_uuid].u_path = unity_mesh_path;
            //}
            string meshName = FbxSync.getMeshName(localId, resConfig.resFiles[mesh_uuid].j_metaPath, owner.name);
            (Mesh fbxMesh, GameObject meshObj) = FbxSync.getMeshFromFBX(meshName, assetPath);
			ConvertUtil.AdjustBakeAxisConversion(owner, meshObj);

			string rootBoneName = nodeInfo_dict["_rootBone"].ToString();
            string fbxRootBoneName = string.Empty;
            Transform[] bones = FbxSync.initFBXBones(component_node.objectName, rootBoneName, meshName, assetPath, owner.transform.parent, ref fbxRootBoneName);
            Transform avatarParent = ConvertUtil.getAvatarParent(owner.transform);
			if (avatarParent == null)//如果父节点中没有带avatar,则骨骼直接挂在当前节点下
				avatarParent = owner.transform;
            if (bones!=null) {
                bool findBone = false;
                UnityEngine.Object[] objects = (UnityEngine.Object[])AssetDatabase.LoadAllAssetsAtPath(assetPath);
                for (int n = 0; n < objects.Length; n++)
                {
                    if (objects[n] is GameObject && (objects[n].name.ToLower().Contains("bone") || objects[n].name.ToLower().Contains("bip") || objects[n].name.ToLower().Contains("root") || objects[n].name.ToLower().Contains("yuandian") || objects[n].name.ToLower().Contains("dummy")))
                    {
                        if (ConvertUtil.deepGetChildByName(avatarParent, objects[n].name) == null && !((GameObject)objects[n]).transform.parent.name.ToLower().Contains("bip") && !((GameObject)objects[n]).transform.parent.name.ToLower().Contains("bone") && !((GameObject)objects[n]).transform.parent.name.ToLower().Contains("dummy"))
                        {
                            GameObject boneRootObject = GameObject.Instantiate((GameObject)objects[n]);
                            boneRootObject.name = boneRootObject.name.Replace("(Clone)", "").Trim();
                            boneRootObject.transform.parent = avatarParent;
                            boneRootObject.transform.localPosition = ((GameObject)objects[n]).transform.localPosition;
                            boneRootObject.transform.localRotation = ((GameObject)objects[n]).transform.localRotation;
                            boneRootObject.transform.localScale = ((GameObject)objects[n]).transform.localScale;
							if (objects[n].name.ToLower().Contains("dummy"))
							{
								int num = boneRootObject.transform.childCount;
								List<Transform> boneChilds = new List<Transform>();
								for (int m = 0; m < num; m++)
								{
									Transform child = boneRootObject.transform.GetChild(m);
									boneChilds.Add(child);
								}

								foreach(Transform t in boneChilds)
								{
									t.parent = avatarParent;
								}
								
							}

                            findBone = true;
                            break;
                        }

                        // rootBone = GetChildByName(owner.transform.parent, rootBoneName);
                    }
                }

                if (!findBone)
                {
                    for (int n = 0; n < objects.Length; n++)
                    {
                        if (objects[n] is GameObject && !ConvertUtil.hasComponentInChild<Animator>((GameObject)objects[n]) && !ConvertUtil.hasComponentInChild<SkinnedMeshRenderer>((GameObject)objects[n]))
                        {
                            if (ConvertUtil.deepGetChildByName(avatarParent, objects[n].name) == null && !ConvertUtil.hasBipChild(avatarParent, "bone") && !ConvertUtil.hasBipChild(avatarParent, "bip"))
                            {

                                GameObject boneRootObject = GameObject.Instantiate((GameObject)objects[n]);
                                boneRootObject.name = boneRootObject.name.Replace("(Clone)", "").Trim();
                                boneRootObject.transform.parent = avatarParent;
                                boneRootObject.transform.localPosition = ((GameObject)objects[n]).transform.localPosition;
                                boneRootObject.transform.localRotation = ((GameObject)objects[n]).transform.localRotation;
                                boneRootObject.transform.localScale = ((GameObject)objects[n]).transform.localScale;
                                findBone = true;
                                break;
                            }
                            // rootBone = GetChildByName(owner.transform.parent, rootBoneName);

                        }
                    }
                }


                string[] boneNames = new string[bones.Length];
                for (int i = 0; i < boneNames.Length; i++)
                {
                    boneNames[i] = bones[i].name;
                }
                if (!fbxRootBoneName.Equals(string.Empty))
                {
                    rootBoneName = fbxRootBoneName;
                }
                Transform[] childs = avatarParent.GetComponentsInChildren<Transform>();
                Dictionary<string, Transform> childDicts = new Dictionary<string, Transform>();
                for (int i = 0; i < childs.Length; i++)
                {
                    if (!childDicts.ContainsKey(childs[i].name))
                    {
                        childDicts.Add(childs[i].name, childs[i]);
                    }
                    if (childs[i].name.Equals(rootBoneName))
                    {
                        smr.rootBone = childs[i];
                    }
                }
                Transform[] newBones = new Transform[boneNames.Length];
                for (int i = 0; i < newBones.Length; i++)
                {
                    if (!childDicts.ContainsKey(boneNames[i]))
                    {
                        ReportSystem.OutputLog($"同步fbx中的骨骼动画[{assetPath}]出错，[{owner.name}]节点的skinmeshrender组件的骨骼[{boneNames[i]}]找不到。");
                    }
                    else
                        newBones[i] = childDicts[boneNames[i]];
                }
                smr.bones = newBones;
                if (smr.rootBone == null)
                {
                    ReportSystem.OutputLog($"同步fbx中的骨骼动画[{assetPath}]出错，[{owner.name}]节点的skinmeshrender组件的根骨骼[{rootBoneName}]找不到。");
                }

               
            }

            smr.sharedMesh = fbxMesh;
        }
        else
        {
            ReportSystem.OutputLog($"解析节点[{owner.gameObject.name}]出错, SkinMeshRender组件中引用了老版本的模型mod文件[{jnity_mesh_path}]!");
        }
 
        //同步材質
        Material[] mats = ConvertUtil.ConvertMats(nodeInfo_dict["_materials"].ToString(), owner);
        smr.sharedMaterials = mats;
    }

    static void ParsePrefabInstance(ref GameObject owner,TreeNode prefab_node, Dictionary<long, TreeNode> treeNode_dict)
    {
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(prefab_node.nodeInfo);
		if (nodeInfo_dict["_sourcePrefab"] == null)
			return;
        var sourcePrefab_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_sourcePrefab"].ToString());
        string prefab_uuid = sourcePrefab_dict["uuid"].ToString();
        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        string jnity_prefab_path = resConfig.resFiles[prefab_uuid].j_path;
        string assetPath = FileUtil.GetAssetPath(jnity_prefab_path);
        string unity_prefab_path = Application.dataPath + assetPath.Replace("Assets", "");
        FileInfo file = new FileInfo(unity_prefab_path);
        GameObject prefabInstance;
        //同步prefab文件 from jnity to unity
        //if (!file.Exists)
        //{
        prefabInstance = ConvertPrefab(owner.transform.parent, prefab_uuid, jnity_prefab_path, false,false);       
        resConfig.resFiles[prefab_uuid].u_path = unity_prefab_path;
        prefabInstance.transform.parent = owner.transform.parent;
        //}
        //else
        //{
        //    GameObject prefabAsset = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
        //    prefabInstance = GameObject.Instantiate(prefabAsset);
        //    prefabInstance.name = prefabInstance.name.Replace("(Clone)", "");
        //    prefabInstance.transform.parent = owner.transform.parent;
        //}
        owner.transform.parent = null;
        ConvertUtil.delayReleaseList.Add(owner);
        owner = prefabInstance;

		var _modification_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_modification"].ToString());
		if (_modification_dict["strippedNodes"] != null)
		{
			var strippedNodes_List = JsonConvert.DeserializeObject<List<object>>(_modification_dict["strippedNodes"].ToString());
			for(int k = 0; k < strippedNodes_List.Count; k++)
			{
				var strippedNodes = JsonConvert.DeserializeObject<Dictionary<object, object>>(strippedNodes_List[k].ToString());
				if (strippedNodes["addedChildren"] != null)
				{
					var childrenList = JsonConvert.DeserializeObject<List<object>>(strippedNodes["addedChildren"].ToString());
					for (int n = 0; n < childrenList.Count; n++)
					{
						var children_id_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(childrenList[n].ToString());
						long children_id = long.Parse(children_id_dict["localId"].ToString());
						TreeNode children_node = treeNode_dict[children_id];
						GameObject childObj = ConvertChildNode(prefab_uuid, children_node, treeNode_dict, owner.transform);
						string globalId = prefab_uuid + "|" + children_id;
						ConvertUtil.allGameObjects.Add(globalId, childObj);
					}
				}
			}
		}
		
	}

    static void ParseMeshRender(GameObject owner, TreeNode component_node, int componetType)
    {
        MeshRenderer mr = owner.AddComponent<MeshRenderer>();      
        MeshFilter mf = owner.AddComponent<MeshFilter>();
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
        mr.enabled = bool.Parse(nodeInfo_dict["_enabled"].ToString());
        mr.shadowCastingMode = (ShadowCastingMode)int.Parse(nodeInfo_dict["_castShadows"].ToString());
        mr.receiveShadows = bool.Parse(nodeInfo_dict["_receiveShadows"].ToString());
        mr.sortingOrder = int.Parse(nodeInfo_dict["_sortingOrder"].ToString());
        mr.sortingLayerID = int.Parse(nodeInfo_dict["_sortingLayerId"].ToString());
        mr.staticShadowCaster = bool.Parse(nodeInfo_dict["_isStaticShadowCaster"].ToString());
        mr.scaleInLightmap = ConvertUtil.safeGetDictValue(nodeInfo_dict, "_scaleInLightmap",1);
        mr.receiveGI = (ReceiveGI)ConvertUtil.safeGetDictValue(nodeInfo_dict, "_receiveGIType", 1);

        //同步mesh
        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        string jnity_mesh_path = string.Empty;
		string jnity_mesh_meta_path = string.Empty;
		string assetPath = string.Empty;
		string unity_mesh_path = string.Empty;
        string mesh_uuid = string.Empty;
		string localId = string.Empty;
		if (componetType != 74)
        {
            var mesh = nodeInfo_dict["_mesh"].ToString();
            var meshInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(mesh);
            mesh_uuid = meshInfo_dict["uuid"].ToString();
            localId = meshInfo_dict["localId"].ToString(); //fbx中用哪个mesh资源
        }
        else
        {
            mesh_uuid = "00000000-0000-0000-0000-75303266a5d1";
            localId = "-1";
        }

        if (resConfig.resFiles.ContainsKey(mesh_uuid))
        {
            jnity_mesh_path = resConfig.resFiles[mesh_uuid].j_path;
            jnity_mesh_meta_path = resConfig.resFiles[mesh_uuid].j_metaPath;
            assetPath = FileUtil.GetAssetPath(jnity_mesh_path);
            unity_mesh_path = Application.dataPath + assetPath.Replace("Assets", "");
        }
        else
        {
			if (JnityBuildInRes.buildInResMap.ContainsKey(mesh_uuid))
			{
				jnity_mesh_path = JnityBuildInRes.buildInResMap[mesh_uuid].path;
				jnity_mesh_meta_path = JnityBuildInRes.buildInResMap[mesh_uuid].path + ".metax";
				assetPath = "Assets/" + JnityBuildInRes.buildInResMap[mesh_uuid].idmap_path;
				unity_mesh_path = Application.dataPath + "/" + JnityBuildInRes.buildInResMap[mesh_uuid].idmap_path;
			}
        }

		if (!unity_mesh_path.Equals(string.Empty))
		{
			FileInfo file = new FileInfo(unity_mesh_path);
			//同步mesh文件 from jnity to unity
			//if (!file.Exists)
			//{
			FileUtil.CopyFile(jnity_mesh_path, unity_mesh_path);
			AssetDatabase.Refresh();
			ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(assetPath);
			FbxSync.SyncMetaFile(importer, jnity_mesh_meta_path);
			//}

			string meshName;
			if (localId.Equals("-1"))
				meshName = "Quad";
			else
				meshName = FbxSync.getMeshName(localId, jnity_mesh_meta_path, owner.name);
			(Mesh fbxMesh, GameObject meshObj) = FbxSync.getMeshFromFBX(meshName, assetPath);
			ConvertUtil.AdjustBakeAxisConversion(owner, meshObj);
			mf.mesh = fbxMesh;
		}


        //同步材質
        Material[] mats = ConvertUtil.ConvertMats(nodeInfo_dict["_materials"].ToString(), owner);
        mr.sharedMaterials = mats;

    }

    static void ParseParticleSystemRender(GameObject owner, TreeNode component_node)
    {
        //GameObject baseObj = ConvertUtil.particleSystemList[owner.GetHashCode()];
        ParticleSystemRenderer psr;
        owner.TryGetComponent<ParticleSystemRenderer>(out psr);
        if (psr == null)
            psr = owner.AddComponent<ParticleSystemRenderer>();
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
        psr.enabled = bool.Parse(nodeInfo_dict["_enabled"].ToString());

        Material[] mats = ConvertUtil.ConvertMats(nodeInfo_dict["_materials"].ToString(), owner);
        psr.sharedMaterials = mats;

        psr.shadowCastingMode = (ShadowCastingMode)int.Parse(nodeInfo_dict["_castShadows"].ToString());
        psr.receiveShadows = bool.Parse(nodeInfo_dict["_receiveShadows"].ToString());
        psr.sortingOrder = int.Parse(nodeInfo_dict["_sortingOrder"].ToString());
        psr.sortingLayerID = int.Parse(nodeInfo_dict["_sortingLayerId"].ToString());
        psr.staticShadowCaster = bool.Parse(nodeInfo_dict["_isStaticShadowCaster"].ToString());
        var renderData_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_rendererData"].ToString());
        psr.renderMode = (ParticleSystemRenderMode)int.Parse(renderData_dict["mode"].ToString());
        psr.alignment = (ParticleSystemRenderSpace)int.Parse(renderData_dict["alignment"].ToString());
        psr.lengthScale = float.Parse(renderData_dict["lengthScale"].ToString()); 
        psr.velocityScale = float.Parse(renderData_dict["velocityScale"].ToString());
        psr.sortingFudge = float.Parse(renderData_dict["sortingFudge"].ToString());
        psr.rotateWithStretchDirection = false;
        psr.freeformStretching = false;
        bool useCustomVertexStreams = bool.Parse(renderData_dict["useCustomVertexStreams"].ToString()); 
        var vertexStreamList = JsonConvert.DeserializeObject<List<object>>(renderData_dict["vertexStreams"].ToString());
        List<ParticleSystemVertexStream> psvsList = new List<ParticleSystemVertexStream>(vertexStreamList.Count);
        for (int n = 0; n < vertexStreamList.Count; n++)
        {
            ParticleSystemVertexStream psvs = ConvertUtil.ConvertParticleSystemVertexStream(int.Parse(vertexStreamList[n].ToString()));
            if(psvs == ParticleSystemVertexStream.Custom1XYZW)
            {
                psvsList.Add(ParticleSystemVertexStream.UV2);
            }
            psvsList.Add(psvs);
        }
        if (useCustomVertexStreams)
        {
            psr.SetActiveVertexStreams(psvsList);
        }

        if (nodeInfo_dict["_emitMesh"] != null)
        {
            var meshInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_emitMesh"].ToString());
            psr.mesh = getMesh(meshInfo_dict, owner, component_node.typeId);
        }

        var bounds_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_bounds"].ToString());
        var center_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(bounds_dict["center"].ToString());
        var extents_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(bounds_dict["extents"].ToString());
        Bounds bounds = new Bounds();
        bounds.center = new Vector3(float.Parse(center_dict["x"].ToString()), float.Parse(center_dict["y"].ToString()),float.Parse(center_dict["z"].ToString()));
        bounds.extents = new Vector3(float.Parse(extents_dict["x"].ToString()), float.Parse(extents_dict["y"].ToString()), float.Parse(extents_dict["z"].ToString()));
        psr.bounds = bounds;
       
    }

    static bool IsModuleEnable(int enableFlag, ParticleModuleFlag moduleFlag)
    {
        return ((enableFlag >> ((int)moduleFlag)) & 0x0001) > 0 ? true : false;
    }

    static void ParseParticleSystem(GameObject owner, TreeNode component_node)
    {
		ParticleSystemRenderer psr = owner.GetComponent<ParticleSystemRenderer>();
        ParticleSystem ps = owner.AddComponent<ParticleSystem>();
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
        int enableFlag = int.Parse(nodeInfo_dict["_enableFlag"].ToString());

        //InitialModule
        var module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["InitialModule"].ToString());
        var mainModule = ps.main;
        
        mainModule.duration = float.Parse(module_dict["_duration"].ToString());
        mainModule.startDelay = float.Parse(module_dict["_startDelay"].ToString());
        mainModule.simulationSpeed = float.Parse(module_dict["_simulationSpeed"].ToString());
        mainModule.simulationSpace = (ParticleSystemSimulationSpace)float.Parse(module_dict["_simulationSpace"].ToString());
        mainModule.cullingMode = ConvertUtil.convertCullingMode(int.Parse(module_dict["_cullingMode"].ToString()));
        mainModule.maxParticles = int.Parse(module_dict["_maxParticles"].ToString());
        mainModule.loop = bool.Parse(module_dict["_loop"].ToString());
        //bool playOnAwake = bool.Parse(module_dict["_playOnEnter"].ToString());
        mainModule.playOnAwake = true;
        mainModule.flipRotation = ConvertUtil.getParticleSystemRotateDir(module_dict);
        mainModule.startSize3D = bool.Parse(module_dict["_startSize3D"].ToString());
        mainModule.startRotation3D = bool.Parse(module_dict["_startRotation3D"].ToString());
        ps.randomSeed = (uint)int.Parse(module_dict["_randomSeed"].ToString());
        ps.useAutoRandomSeed = bool.Parse(module_dict["_autoRandomSeed"].ToString());
        mainModule.stopAction = int.Parse(module_dict["_stopAction"].ToString()) > 0 ? ParticleSystemStopAction.Callback : ParticleSystemStopAction.None;
        mainModule.startLifetime = ConvertUtil.ConvertMinMaxCurve(mainModule.startLifetime, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_startLifetime"].ToString()));
        mainModule.startSpeed = ConvertUtil.ConvertMinMaxCurve(mainModule.startSpeed, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_startSpeed"].ToString()));
        mainModule.startColor = ConvertUtil.ConvertMinMaxGradient(mainModule.startColor, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_startColor"].ToString()));
        mainModule.startSize = ConvertUtil.ConvertMinMaxCurve(mainModule.startSize, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_startSize"].ToString()));
        mainModule.startSizeY = ConvertUtil.ConvertMinMaxCurve(mainModule.startSizeY, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_startSizeY"].ToString()));
        mainModule.startSizeZ = ConvertUtil.ConvertMinMaxCurve(mainModule.startSizeZ, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_startSizeZ"].ToString()));
        mainModule.startRotationX = ConvertUtil.ConvertMinMaxCurve(mainModule.startRotationX, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_startRotationX"].ToString()), false, true);
        mainModule.startRotationY = ConvertUtil.ConvertMinMaxCurve(mainModule.startRotationY, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_startRotationY"].ToString()), false, true);
        mainModule.startRotation = ConvertUtil.ConvertMinMaxCurve(mainModule.startRotation, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_startRotation"].ToString()),false, true);
        mainModule.gravityModifier = ConvertUtil.ConvertMinMaxCurve(mainModule.gravityModifier, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_gravityModifier"].ToString()));
        mainModule.scalingMode = ParticleSystemScalingMode.Hierarchy;

        //EmissionModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["EmissionModule"].ToString());
        var emissionModule = ps.emission;
        emissionModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.EMISSION);
        if (emissionModule.enabled)
        {
            emissionModule.rateOverTime = ConvertUtil.ConvertMinMaxCurve(emissionModule.rateOverTime, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_rateOverTime"].ToString()));
            emissionModule.rateOverDistance = ConvertUtil.ConvertMinMaxCurve(emissionModule.rateOverDistance, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_rateOverDistance"].ToString()));
            emissionModule.SetBursts(ConvertUtil.ConvertEmissionBurst(JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_bursts"].ToString())));
        }


        //ShapeModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["ShapeModule"].ToString());
        var shapeModule = ps.shape;
        bool shapeEnable = IsModuleEnable(enableFlag, ParticleModuleFlag.SHAPE);
        shapeModule.enabled = true;
        ParticleSystemShapeType shapeType = ConvertUtil.convertShapeType(int.Parse(module_dict["_type"].ToString()));
        if(shapeType == ParticleSystemShapeType.BoxShell || shapeType == ParticleSystemShapeType.ConeShell || shapeType == ParticleSystemShapeType.ConeVolumeShell
            || shapeType == ParticleSystemShapeType.HemisphereShell || shapeType == ParticleSystemShapeType.SphereShell)
        {
            shapeModule.radiusThickness = 0;
            if (shapeType == ParticleSystemShapeType.ConeShell)
                shapeModule.shapeType = ParticleSystemShapeType.Cone;
            else if (shapeType == ParticleSystemShapeType.ConeVolumeShell)
                shapeModule.shapeType = ParticleSystemShapeType.ConeVolume;
            else if (shapeType == ParticleSystemShapeType.HemisphereShell)
                shapeModule.shapeType = ParticleSystemShapeType.Hemisphere;
            else if (shapeType == ParticleSystemShapeType.SphereShell)
                shapeModule.shapeType = ParticleSystemShapeType.SphereShell;
            else
                shapeModule.shapeType = shapeType;
        }
        else
        {
            shapeModule.radiusThickness = 1;
            shapeModule.shapeType = shapeType;
        }
        
        shapeModule.radius = float.Parse(module_dict["_radius"].ToString());     
        shapeModule.angle = float.Parse(module_dict["_angle"].ToString());
        shapeModule.length = float.Parse(module_dict["_length"].ToString());
        shapeModule.arc = float.Parse(module_dict["_arc"].ToString());
        shapeModule.radiusMode = (ParticleSystemShapeMultiModeValue)int.Parse(module_dict["_radiusMode"].ToString());
        shapeModule.arcMode = shapeModule.radiusMode;
        shapeModule.radiusSpread = float.Parse(module_dict["_radiusSpread"].ToString());
        shapeModule.arcSpread = shapeModule.radiusSpread;
        shapeModule.radiusSpeed = ConvertUtil.ConvertMinMaxCurve(shapeModule.radiusSpeed, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_radiusSpeed"].ToString()));
        shapeModule.arcSpeed = shapeModule.radiusSpeed;
        shapeModule.alignToDirection = bool.Parse(module_dict["_alignToDirection"].ToString());
        shapeModule.randomDirectionAmount = float.Parse(module_dict["_randomDirectionAmount"].ToString());
        shapeModule.sphericalDirectionAmount = float.Parse(module_dict["_sphericalDirectionAmount"].ToString());
        shapeModule.randomPositionAmount = float.Parse(module_dict["_randomPositionAmount"].ToString());
        shapeModule.meshShapeType = (ParticleSystemMeshShapeType)int.Parse(module_dict["_placementMode"].ToString());
        shapeModule.meshMaterialIndex = int.Parse(module_dict["_singleMeshIndex"].ToString());
        shapeModule.normalOffset = float.Parse(module_dict["_meshNormalOffset"].ToString());
        shapeModule.useMeshMaterialIndex = bool.Parse(module_dict["_useSingleMeshIndex"].ToString());
        shapeModule.useMeshColors = bool.Parse(module_dict["_useMeshColors"].ToString());
       
        if (module_dict["_mesh"] != null)
        {
            var meshInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_mesh"].ToString());
            shapeModule.mesh = getMesh(meshInfo_dict, owner, component_node.typeId);
        }
        var position_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_position"].ToString());
        shapeModule.position = new Vector3(float.Parse(position_dict["x"].ToString()), float.Parse(position_dict["y"].ToString()), float.Parse(position_dict["z"].ToString()));
        var rotation_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_rotation"].ToString());
        if (shapeModule.shapeType == ParticleSystemShapeType.Mesh)
            shapeModule.rotation = new Vector3(float.Parse(rotation_dict["x"].ToString()), float.Parse(rotation_dict["y"].ToString()), float.Parse(rotation_dict["z"].ToString()));
        else
            shapeModule.rotation = new Vector3(float.Parse(rotation_dict["x"].ToString()) - 90, float.Parse(rotation_dict["y"].ToString()), float.Parse(rotation_dict["z"].ToString()));

        var scale_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_scale"].ToString());
        if (shapeModule.shapeType == ParticleSystemShapeType.Circle || shapeModule.shapeType == ParticleSystemShapeType.Cone)
            shapeModule.scale = new Vector3(float.Parse(scale_dict["x"].ToString()), -float.Parse(scale_dict["y"].ToString()), float.Parse(scale_dict["z"].ToString()));
        else
            shapeModule.scale = new Vector3(float.Parse(scale_dict["x"].ToString()), float.Parse(scale_dict["y"].ToString()), float.Parse(scale_dict["z"].ToString()));
        var box_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_box"].ToString());
        Vector3 box_v = new Vector3(float.Parse(box_dict["x"].ToString()), float.Parse(box_dict["y"].ToString()), float.Parse(box_dict["z"].ToString()));
        if (shapeModule.shapeType == ParticleSystemShapeType.Box)
        {
            shapeModule.scale = box_v;
        }
        if (!shapeEnable)
        {
            shapeModule.shapeType = ParticleSystemShapeType.Cone;
            shapeModule.scale = Vector3.one;
            shapeModule.angle = 0;
            shapeModule.radius = 0.0001f;
        }
        //VelocityModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["VelocityModule"].ToString());
        var velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.VELOCITY);
        if (velocityModule.enabled)
        {
            velocityModule.space = (ParticleSystemSimulationSpace)int.Parse(module_dict["_space"].ToString());
            velocityModule.x = ConvertUtil.ConvertMinMaxCurve(velocityModule.x, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_x"].ToString()));
            velocityModule.y = ConvertUtil.ConvertMinMaxCurve(velocityModule.y, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_y"].ToString()));
            velocityModule.z = ConvertUtil.ConvertMinMaxCurve(velocityModule.z, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_z"].ToString()), true);
        }
        //LimitVelocityModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["LimitVelocityModule"].ToString());
        var limitVelocityModule = ps.limitVelocityOverLifetime;
        limitVelocityModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.LIMIT_VELOCITY);
        if (limitVelocityModule.enabled)
        {
            limitVelocityModule.separateAxes = bool.Parse(module_dict["_separateAxes"].ToString());
            if (limitVelocityModule.separateAxes)
            {
                limitVelocityModule.limitX = ConvertUtil.ConvertMinMaxCurve(limitVelocityModule.limitX, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_limitX"].ToString()), true);
                limitVelocityModule.limitY = ConvertUtil.ConvertMinMaxCurve(limitVelocityModule.limitY, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_limitY"].ToString()));
                limitVelocityModule.limitZ = ConvertUtil.ConvertMinMaxCurve(limitVelocityModule.limitZ, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_limitZ"].ToString()), true);
            }
            else
            {
                limitVelocityModule.limit = ConvertUtil.ConvertMinMaxCurve(limitVelocityModule.limit, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_limit"].ToString()));
            }
            limitVelocityModule.dampen = float.Parse(module_dict["_dampen"].ToString());
            limitVelocityModule.space = (ParticleSystemSimulationSpace)int.Parse(module_dict["_space"].ToString());
        }
        //ForceModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["ForceModule"].ToString());
        var forceModule = ps.forceOverLifetime;
        forceModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.FORCE);
        if (forceModule.enabled)
        {
            forceModule.space = (ParticleSystemSimulationSpace)int.Parse(module_dict["_space"].ToString());
            forceModule.x = ConvertUtil.ConvertMinMaxCurve(forceModule.x, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_x"].ToString()));
            forceModule.y = ConvertUtil.ConvertMinMaxCurve(forceModule.y, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_y"].ToString()));
            forceModule.z = ConvertUtil.ConvertMinMaxCurve(forceModule.z, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_z"].ToString()));
            forceModule.randomized = bool.Parse(module_dict["_randomized"].ToString());
        }
        //SizeModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["SizeModule"].ToString());
        var sizeModule = ps.sizeOverLifetime;
        sizeModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.SIZE);
        if (sizeModule.enabled)
        {
            sizeModule.separateAxes = bool.Parse(module_dict["_separateAxes"].ToString());
            if (sizeModule.separateAxes)
            {
                sizeModule.x = ConvertUtil.ConvertMinMaxCurve(sizeModule.x, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_x"].ToString()));
                sizeModule.y = ConvertUtil.ConvertMinMaxCurve(sizeModule.y, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_y"].ToString()));
                sizeModule.z = ConvertUtil.ConvertMinMaxCurve(sizeModule.z, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_z"].ToString()));
            }
            else
            {
                sizeModule.size = ConvertUtil.ConvertMinMaxCurve(sizeModule.size, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_size"].ToString()));
            }
        }
     
        //SizeBySpeedModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["SizeBySpeedModule"].ToString());
        var sizeBySpeedModule = ps.sizeBySpeed;
        sizeBySpeedModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.SIZE_BY_SPEED);
        if (sizeBySpeedModule.enabled)
        {
            sizeBySpeedModule.separateAxes = bool.Parse(module_dict["_separateAxes"].ToString());
            if (sizeBySpeedModule.separateAxes)
            {
                sizeBySpeedModule.x = ConvertUtil.ConvertMinMaxCurve(sizeBySpeedModule.x, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_x"].ToString()));
                sizeBySpeedModule.y = ConvertUtil.ConvertMinMaxCurve(sizeBySpeedModule.y, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_y"].ToString()));
                sizeBySpeedModule.z = ConvertUtil.ConvertMinMaxCurve(sizeBySpeedModule.z, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_z"].ToString()));
            }
            else
            {
                sizeBySpeedModule.size = ConvertUtil.ConvertMinMaxCurve(sizeBySpeedModule.size, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_size"].ToString()));
            }
            var range_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_range"].ToString());
            sizeBySpeedModule.range = new Vector2(float.Parse(range_dict["x"].ToString()), float.Parse(range_dict["y"].ToString()));
        }

        //RotationModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["RotationModule"].ToString());
        var rotationModule = ps.rotationOverLifetime;
        rotationModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.ROTATION);
        if (rotationModule.enabled)
        {
            rotationModule.separateAxes = bool.Parse(module_dict["_separateAxes"].ToString());
            rotationModule.x = ConvertUtil.ConvertMinMaxCurve(rotationModule.x, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_x"].ToString()), false, true);
            rotationModule.y = ConvertUtil.ConvertMinMaxCurve(rotationModule.y, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_y"].ToString()), false, true);
			bool flipZ = false;
			if (psr.renderMode == ParticleSystemRenderMode.Mesh)
			{
				rotationModule.separateAxes = true;
				flipZ = true;
			}
			rotationModule.z = ConvertUtil.ConvertMinMaxCurve(rotationModule.z, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_z"].ToString()), flipZ, true);
        }
        //RotationBySpeedModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["RotationBySpeedModule"].ToString());
        var rotationBySpeedModule = ps.rotationBySpeed;
        rotationBySpeedModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.ROTATION_BY_SPEED);
        if (rotationBySpeedModule.enabled)
        {
            rotationBySpeedModule.separateAxes = bool.Parse(module_dict["_separateAxes"].ToString());
            rotationBySpeedModule.x = ConvertUtil.ConvertMinMaxCurve(rotationBySpeedModule.x, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_x"].ToString()), false, true);
            rotationBySpeedModule.y = ConvertUtil.ConvertMinMaxCurve(rotationBySpeedModule.y, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_y"].ToString()), false, true);
            rotationBySpeedModule.z = ConvertUtil.ConvertMinMaxCurve(rotationBySpeedModule.z, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_z"].ToString()), false, true);

            var range_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_range"].ToString());
            rotationBySpeedModule.range = new Vector2(float.Parse(range_dict["x"].ToString()), float.Parse(range_dict["y"].ToString()));
        }
        //ColorModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["ColorModule"].ToString());
        var colorModule = ps.colorOverLifetime;
        colorModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.COLOR);
        if (colorModule.enabled)
        {
            colorModule.color = ConvertUtil.ConvertMinMaxGradient(colorModule.color, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_color"].ToString()));
        }
        //ColorBySpeedModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["ColorBySpeedModule"].ToString());
        var colorBySpeedModule = ps.colorBySpeed;
        colorBySpeedModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.COLOR_BY_SPEED);
        if (colorBySpeedModule.enabled)
        {
            colorBySpeedModule.color = ConvertUtil.ConvertMinMaxGradient(colorBySpeedModule.color, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_color"].ToString()));
            var range_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_range"].ToString());
            colorBySpeedModule.range = new Vector2(float.Parse(range_dict["x"].ToString()), float.Parse(range_dict["y"].ToString()));
        }
        //CollisionModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["CollisionModule"].ToString());
        var collisionModule = ps.collision;
        collisionModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.COLLISION);
        if (collisionModule.enabled)
        {
            collisionModule.dampen = ConvertUtil.ConvertMinMaxCurve(collisionModule.dampen, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_dampen"].ToString()));
            collisionModule.bounce = ConvertUtil.ConvertMinMaxCurve(collisionModule.bounce, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_bounce"].ToString()));
            collisionModule.lifetimeLoss = ConvertUtil.ConvertMinMaxCurve(collisionModule.lifetimeLoss, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_lifetimeLoss"].ToString()));
            collisionModule.minKillSpeed = float.Parse(module_dict["_minKillSpeed"].ToString());
            collisionModule.maxKillSpeed = float.Parse(module_dict["_maxKillSpeed"].ToString());
            collisionModule.radiusScale = float.Parse(module_dict["_radiusScale"].ToString());
            collisionModule.colliderForce = float.Parse(module_dict["_friction"].ToString());

            var planes_list = JsonConvert.DeserializeObject<List<object>>(module_dict["_planes"].ToString());
            for (int k = 0; k < planes_list.Count; k++)
            {
                if (planes_list[k] == null)
                {
                    ReportSystem.OutputLog($"节点[{owner.name}]的ParticleSystem组件中引用的plane数据为null！");
                    break;
                }
                var planes = JsonConvert.DeserializeObject<Dictionary<object, object>>(planes_list[k].ToString());
                long localId = long.Parse(planes["localId"].ToString());
                CollisionPlaneInfo planeInfo = new CollisionPlaneInfo();
                planeInfo.localId = localId;
                planeInfo.parentObj = owner;
                ConvertUtil.collisionPlaneList.Add(planeInfo);
            }
        }

        //UVModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["UVModule"].ToString());
        var uvModule = ps.textureSheetAnimation;
        uvModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.UV);
        if (uvModule.enabled)
        {
            uvModule.animation = (ParticleSystemAnimationType)int.Parse(module_dict["_animType"].ToString());
            uvModule.rowMode = bool.Parse(module_dict["_randomRow"].ToString()) ? ParticleSystemAnimationRowMode.Random : ParticleSystemAnimationRowMode.Custom;
            uvModule.rowIndex = int.Parse(module_dict["_rowIndex"].ToString());
            uvModule.startFrame = ConvertUtil.ConvertMinMaxCurve(uvModule.startFrame, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_startFrame"].ToString()));
            uvModule.frameOverTime = ConvertUtil.ConvertMinMaxCurve(uvModule.frameOverTime, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_curve"].ToString()));
            uvModule.numTilesX = int.Parse(module_dict["_tilesX"].ToString());
            uvModule.numTilesY = int.Parse(module_dict["_tilesY"].ToString());
            uvModule.cycleCount = int.Parse(module_dict["_cycles"].ToString());
            uvModule.mode = (ParticleSystemAnimationMode)int.Parse(module_dict["_spriteMode"].ToString());
        }
        /** _spriteOPtrList没有解析**/
        //SubModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["SubModule"].ToString());
        var subModule = ps.subEmitters;
        subModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.SUB);
        if (subModule.enabled)
        {
            var emitters = JsonConvert.DeserializeObject<List<object>>(module_dict["_emitters"].ToString());
            for (int n = 0; n < emitters.Count; n++)
            {
                var emitter_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(emitters[n].ToString());
                var path_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(emitter_dict["path"].ToString());
                long localId = long.Parse(path_dict["localId"].ToString());
                ParticleSystemSubEmitterType emitType = ConvertUtil.convertSubEmitterType(int.Parse(emitter_dict["type"].ToString()));
                ParticleSystemSubEmitterProperties emitProperty = ConvertUtil.convertSubEmitterProperty(int.Parse(emitter_dict["properties"].ToString()));
                SubEmitterInfo subemitterInfo = new SubEmitterInfo();
                subemitterInfo.parentObj = owner;
                subemitterInfo.localId = localId;
                subemitterInfo.property = emitProperty;
                subemitterInfo.type = emitType;
                ConvertUtil.subEmitterList.Add(subemitterInfo);
            }
        }

        //CustomData
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["CustomData"].ToString());
        var customModule = ps.customData;
        customModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.CUSTOM_DATA);
        if (customModule.enabled)
        {
            customModule.SetMode(ParticleSystemCustomData.Custom1, (ParticleSystemCustomDataMode)int.Parse(module_dict["_mode0"].ToString()));
            customModule.SetVectorComponentCount(ParticleSystemCustomData.Custom1, int.Parse(module_dict["_vectorComponentCount0"].ToString()));
            ParticleSystem.MinMaxCurve x0 = new ParticleSystem.MinMaxCurve();
            x0 = ConvertUtil.ConvertMinMaxCurve(x0, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_x0"].ToString()));
            customModule.SetVector(ParticleSystemCustomData.Custom1, 0, x0);
            ParticleSystem.MinMaxCurve y0 = new ParticleSystem.MinMaxCurve();
            y0 = ConvertUtil.ConvertMinMaxCurve(y0, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_y0"].ToString()));
            customModule.SetVector(ParticleSystemCustomData.Custom1, 1, y0);
            ParticleSystem.MinMaxCurve z0 = new ParticleSystem.MinMaxCurve();
            z0 = ConvertUtil.ConvertMinMaxCurve(z0, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_z0"].ToString()));
            customModule.SetVector(ParticleSystemCustomData.Custom1, 2, z0);
            ParticleSystem.MinMaxCurve w0 = new ParticleSystem.MinMaxCurve();
            w0 = ConvertUtil.ConvertMinMaxCurve(w0, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_w0"].ToString()));
            customModule.SetVector(ParticleSystemCustomData.Custom1, 3, w0);
            ParticleSystem.MinMaxGradient color0 = new ParticleSystem.MinMaxGradient();
            color0 = ConvertUtil.ConvertMinMaxGradient(color0, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_color0"].ToString()));
            customModule.SetColor(ParticleSystemCustomData.Custom1, color0);
            customModule.SetMode(ParticleSystemCustomData.Custom2, (ParticleSystemCustomDataMode)int.Parse(module_dict["_mode1"].ToString()));
            customModule.SetVectorComponentCount(ParticleSystemCustomData.Custom2, int.Parse(module_dict["_vectorComponentCount1"].ToString()));
            ParticleSystem.MinMaxCurve x1 = new ParticleSystem.MinMaxCurve();
            x1 = ConvertUtil.ConvertMinMaxCurve(x1, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_x1"].ToString()));
            customModule.SetVector(ParticleSystemCustomData.Custom2, 0, x1);
            ParticleSystem.MinMaxCurve y1 = new ParticleSystem.MinMaxCurve();
            y1 = ConvertUtil.ConvertMinMaxCurve(y1, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_y1"].ToString()));
            customModule.SetVector(ParticleSystemCustomData.Custom2, 1, y1);
            ParticleSystem.MinMaxCurve z1 = new ParticleSystem.MinMaxCurve();
            z1 = ConvertUtil.ConvertMinMaxCurve(z1, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_z1"].ToString()));
            customModule.SetVector(ParticleSystemCustomData.Custom2, 2, z1);
            ParticleSystem.MinMaxCurve w1 = new ParticleSystem.MinMaxCurve();
            w1 = ConvertUtil.ConvertMinMaxCurve(w1, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_w1"].ToString()));
            customModule.SetVector(ParticleSystemCustomData.Custom2, 3, w1);
            ParticleSystem.MinMaxGradient color1 = new ParticleSystem.MinMaxGradient();
            color1 = ConvertUtil.ConvertMinMaxGradient(color1, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_color1"].ToString()));
            customModule.SetColor(ParticleSystemCustomData.Custom2, color1);
        }
        //TrailsModule
        module_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["TrailsModule"].ToString());
        var trailModule = ps.trails;
        trailModule.enabled = IsModuleEnable(enableFlag, ParticleModuleFlag.TRAILS);
        if (trailModule.enabled)
        {
            trailModule.mode = (ParticleSystemTrailMode)int.Parse(module_dict["_mode"].ToString());
            trailModule.textureMode = (ParticleSystemTrailTextureMode)int.Parse(module_dict["_textureMode"].ToString());
            trailModule.worldSpace = bool.Parse(module_dict["_worldSpace"].ToString());
            trailModule.dieWithParticles = bool.Parse(module_dict["_dieWithParticles"].ToString());
            trailModule.inheritParticleColor = bool.Parse(module_dict["_inheritParticleColor"].ToString());
            trailModule.sizeAffectsWidth = bool.Parse(module_dict["_sizeAffectsWidth"].ToString());
            trailModule.sizeAffectsLifetime = bool.Parse(module_dict["_sizeAffectsLifetime"].ToString());
            trailModule.lifetime = ConvertUtil.ConvertMinMaxCurve(trailModule.lifetime, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_lifetime"].ToString()));
            trailModule.ratio = float.Parse(module_dict["_ratio"].ToString());
            trailModule.minVertexDistance = float.Parse(module_dict["_minVertexDistance"].ToString());
            trailModule.colorOverLifetime = ConvertUtil.ConvertMinMaxGradient(trailModule.colorOverLifetime, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_colorOverLifetime"].ToString()));
            trailModule.colorOverTrail = ConvertUtil.ConvertMinMaxGradient(trailModule.colorOverTrail, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_colorOverTrail"].ToString()));
            trailModule.widthOverTrail = ConvertUtil.ConvertMinMaxCurve(trailModule.widthOverTrail, JsonConvert.DeserializeObject<Dictionary<object, object>>(module_dict["_widthOverTrail"].ToString()));
        }
    }

    static Mesh getMesh(Dictionary<object, object> meshInfo_dict, GameObject owner, int componetType)
    {
            JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
            string jnity_mesh_path;
            string jnity_mesh_meta_path;
            string assetPath;
            string unity_mesh_path;
            string mesh_uuid;
            string localId;
            if (componetType != 74)
            {
                mesh_uuid = meshInfo_dict["uuid"].ToString();
                localId = meshInfo_dict["localId"].ToString(); //fbx中用哪个mesh资源
            }
            else
            {
                mesh_uuid = "00000000-0000-0000-0000-75303266a5d1";
                localId = "-1";
            }

            if (resConfig.resFiles.ContainsKey(mesh_uuid))
            {
                jnity_mesh_path = resConfig.resFiles[mesh_uuid].j_path;
                jnity_mesh_meta_path = resConfig.resFiles[mesh_uuid].j_metaPath;
                assetPath = FileUtil.GetAssetPath(jnity_mesh_path);
                unity_mesh_path = Application.dataPath + assetPath.Replace("Assets", "");
            }
            else
            {
                jnity_mesh_path = JnityBuildInRes.buildInResMap[mesh_uuid].path;
                jnity_mesh_meta_path = JnityBuildInRes.buildInResMap[mesh_uuid].path + ".metax";
                assetPath = Jnity2Unity.unityRelativeAssetPath + JnityBuildInRes.buildInResMap[mesh_uuid].idmap_path;
                unity_mesh_path = Jnity2Unity.unityAssetPath+ JnityBuildInRes.buildInResMap[mesh_uuid].idmap_path;
            }

            FileInfo file = new FileInfo(unity_mesh_path);
            //同步mesh文件 from jnity to unity
            //if (!file.Exists)
            //{
            FileUtil.CopyFile(jnity_mesh_path, unity_mesh_path);
            AssetDatabase.Refresh();
            ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(assetPath);
            FbxSync.SyncMetaFile(importer, jnity_mesh_meta_path);
            //}

            string meshName;
            if (localId.Equals("-1"))
                meshName = "Quad";
            else
                meshName = FbxSync.getMeshName(localId, jnity_mesh_meta_path, owner.name);
            (Mesh fbxMesh, GameObject meshObj) = FbxSync.getMeshFromFBX(meshName, assetPath);
		    //ConvertUtil.AdjustBakeAxisConversion(owner, meshObj);
		    return fbxMesh;
    }


    static void ParseTrailRender(GameObject owner, TreeNode component_node)
    {
        TrailRenderer tr = owner.AddComponent<TrailRenderer>();
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
        tr.enabled = bool.Parse(nodeInfo_dict["_enabled"].ToString());
        tr.shadowCastingMode = (ShadowCastingMode)int.Parse(nodeInfo_dict["_castShadows"].ToString());
        tr.receiveShadows = bool.Parse(nodeInfo_dict["_receiveShadows"].ToString());
        tr.sortingOrder = int.Parse(nodeInfo_dict["_sortingOrder"].ToString());
        tr.sortingLayerID = int.Parse(nodeInfo_dict["_sortingLayerId"].ToString());
        tr.staticShadowCaster = bool.Parse(nodeInfo_dict["_isStaticShadowCaster"].ToString());
        tr.widthMultiplier = float.Parse(nodeInfo_dict["_widthMultiplier"].ToString());
        tr.alignment = (LineAlignment)int.Parse(nodeInfo_dict["_alignment"].ToString());
        tr.textureMode = (LineTextureMode)int.Parse(nodeInfo_dict["_textureMode"].ToString());
        tr.time = float.Parse(nodeInfo_dict["_lifeTime"].ToString());
        tr.minVertexDistance = float.Parse(nodeInfo_dict["_minVertexDistance"].ToString());
        tr.autodestruct = bool.Parse(nodeInfo_dict["_autodestruct"].ToString());

        var color_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_color"].ToString());
        var color_keys = JsonConvert.DeserializeObject<List<object>>(color_dict["_colorKeys"].ToString());
        GradientColorKey[] colorKeys = new GradientColorKey[color_keys.Count];
        for (int n = 0; n < color_keys.Count; n++)
        {
            var colorKeysDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(color_keys[n].ToString());
            var colorValue = JsonConvert.DeserializeObject<Dictionary<object, object>>(colorKeysDict["color"].ToString());
            colorKeys[n].color = new Color(float.Parse(colorValue["r"].ToString()), float.Parse(colorValue["g"].ToString()), float.Parse(colorValue["b"].ToString()), 1);
            colorKeys[n].time = float.Parse(colorKeysDict["time"].ToString());
        }
        tr.colorGradient.mode = (GradientMode)int.Parse(color_dict["_mode"].ToString());
        var alpha_keys = JsonConvert.DeserializeObject<List<object>>(color_dict["_alphaKeys"].ToString());
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[alpha_keys.Count];
        for (int n = 0; n < alpha_keys.Count; n++)
        {
            var alphaKeysDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(alpha_keys[n].ToString());
            alphaKeys[n].alpha = float.Parse(alphaKeysDict["alpha"].ToString());
            alphaKeys[n].time = float.Parse(alphaKeysDict["time"].ToString());
        }
        Gradient gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);
        tr.colorGradient = gradient;

        var widthCurveDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_widthCurve"].ToString());

        var curveKeyList = JsonConvert.DeserializeObject<List<object>>(widthCurveDict["_keys"].ToString());
        AnimationCurve animCurve = new AnimationCurve();
        for (int n = 0; n < curveKeyList.Count; n++)
        {
            var keyFrame_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(curveKeyList[n].ToString());
            var time = keyFrame_Dict["t"];
            var value = keyFrame_Dict["v"];
            var leftTangentValue = keyFrame_Dict["l"];
            var rightTangetValue = keyFrame_Dict["r"];
            var tangentMode = int.Parse(keyFrame_Dict["tangentMode"].ToString());
            Keyframe keyframe = new Keyframe();
            keyframe.time = float.Parse(time.ToString());
            keyframe.value = float.Parse(value.ToString());
            keyframe.inTangent = float.Parse(leftTangentValue.ToString());
            keyframe.outTangent = float.Parse(rightTangetValue.ToString());
            animCurve.AddKey(keyframe);
            AnimationUtility.TangentMode leftTangent;
            AnimationUtility.TangentMode rightTangent;
            AnimationConvert.ConvertTangetMode(tangentMode, out leftTangent, out rightTangent, animCurve, n);
            AnimationUtility.SetKeyLeftTangentMode(animCurve, n, leftTangent);
            AnimationUtility.SetKeyRightTangentMode(animCurve, n, rightTangent);
        }
        tr.widthCurve = animCurve;

        Material[] mats = ConvertUtil.ConvertMats(nodeInfo_dict["_materials"].ToString(), owner);
        tr.sharedMaterials = mats;
    }



    //static void ParseDynamicBone(GameObject owner, TreeNode component_node)
    //{
    //    DynamicBone dynamicBone = owner.AddComponent<DynamicBone>();
    //    var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
    //    dynamicBone.enabled = bool.Parse(nodeInfo_dict["_enabled"].ToString());
    //    string rootBoneName = nodeInfo_dict["_rootBoneName"].ToString();
    //    dynamicBone.m_UpdateRate = float.Parse(nodeInfo_dict["_updateRate"].ToString());
    //    dynamicBone.m_Damping = float.Parse(nodeInfo_dict["_damping"].ToString());
    //    dynamicBone.m_Elasticity = float.Parse(nodeInfo_dict["_elasticity"].ToString());
    //    dynamicBone.m_Stiffness = float.Parse(nodeInfo_dict["_stiffness"].ToString());
    //    dynamicBone.m_Inert = float.Parse(nodeInfo_dict["_inert"].ToString());
    //    dynamicBone.m_Radius = float.Parse(nodeInfo_dict["_radius"].ToString());
    //    dynamicBone.m_DampingDistrib = ConvertUtil.ConvertAnimationCurve(nodeInfo_dict, "_dampingCurve", false, false, false);
    //    dynamicBone.m_ElasticityDistrib = ConvertUtil.ConvertAnimationCurve(nodeInfo_dict, "_elasticityCurve", false, false, false);
    //    dynamicBone.m_StiffnessDistrib = ConvertUtil.ConvertAnimationCurve(nodeInfo_dict, "_stiffnessCurve", false, false, false);
    //    dynamicBone.m_InertDistrib = ConvertUtil.ConvertAnimationCurve(nodeInfo_dict, "_inertCurve", false, false, false);
    //    dynamicBone.m_RadiusDistrib = ConvertUtil.ConvertAnimationCurve(nodeInfo_dict, "_radiusCurve", false, false, false);
    //    dynamicBone.m_EndLength = float.Parse(nodeInfo_dict["_endLength"].ToString());
    //    var endoffset_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_endOffset"].ToString());
    //    dynamicBone.m_EndOffset = new Vector3(float.Parse(endoffset_dict["x"].ToString()), float.Parse(endoffset_dict["y"].ToString()), float.Parse(endoffset_dict["z"].ToString()));
    //    var gravity_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_gravity"].ToString());
    //    dynamicBone.m_Gravity = new Vector3(float.Parse(gravity_dict["x"].ToString()), float.Parse(gravity_dict["y"].ToString()), float.Parse(gravity_dict["z"].ToString()));
    //    var force_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_force"].ToString());
    //    dynamicBone.m_Force = new Vector3(float.Parse(force_dict["x"].ToString()), float.Parse(force_dict["y"].ToString()), float.Parse(force_dict["z"].ToString()));
    //    DynamicBoneInfo dbi = new DynamicBoneInfo();
    //    dbi.parentObj = owner;
    //    dbi.rootBoneName = rootBoneName;
    //    dbi.db = dynamicBone;
    //    ConvertUtil.dynamicBoneList.Add(dbi);

    //}

    static void ParseTextMeshPro(GameObject owner, TreeNode component_node)
    {
        TextMeshProUGUI tmp = owner.AddComponent<TextMeshProUGUI>();
        var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
        tmp.enabled = bool.Parse(nodeInfo_dict["_enabled"].ToString());
        tmp.raycastTarget = bool.Parse(nodeInfo_dict["_raycastTarget"].ToString());
        var raycastPadding_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_raycastPadding"].ToString());
        tmp.raycastPadding = new Vector4(float.Parse(raycastPadding_dict["x"].ToString()), float.Parse(raycastPadding_dict["y"].ToString()),
            float.Parse(raycastPadding_dict["z"].ToString()), float.Parse(raycastPadding_dict["w"].ToString()));
        tmp.maskable = bool.Parse(nodeInfo_dict["_maskable"].ToString());
        
        var textTMP_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(nodeInfo_dict["_textTMP"].ToString());
        tmp.text = textTMP_Dict["_text"].ToString();
        var fontAssetDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(textTMP_Dict["_fontAsset"].ToString());
        tmp.font = FontAssetConvert.GetFontAsset(fontAssetDict, owner.transform);
        var spaceingOption_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(textTMP_Dict["_spacingOptions"].ToString());
        tmp.characterSpacing = float.Parse(spaceingOption_Dict["characterSpacing"].ToString());
        tmp.wordSpacing = float.Parse(spaceingOption_Dict["characterSpacing"].ToString());
        tmp.lineSpacing = float.Parse(spaceingOption_Dict["lineSpacing"].ToString());
        tmp.paragraphSpacing = float.Parse(spaceingOption_Dict["paragraphSpacing"].ToString());
        var fontcolor_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(textTMP_Dict["_fontColor"].ToString());
        tmp.color = new Color(float.Parse(fontcolor_dict["r"].ToString()), float.Parse(fontcolor_dict["g"].ToString()),
            float.Parse(fontcolor_dict["b"].ToString()), float.Parse(fontcolor_dict["a"].ToString()));
        var facecolor_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(textTMP_Dict["_faceColor"].ToString());
        tmp.faceColor = new Color32(byte.Parse(fontcolor_dict["r"].ToString()), byte.Parse(fontcolor_dict["g"].ToString()),
            byte.Parse(fontcolor_dict["b"].ToString()), byte.Parse(fontcolor_dict["a"].ToString()));
        tmp.enableVertexGradient = int.Parse(textTMP_Dict["_colorGradientUseMode"].ToString()) > 0;
        var localColorGradientValue_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(textTMP_Dict["_localColorGradientValue"].ToString());
        TMP_ColorGradient localColorGradient = new TMP_ColorGradient();
        localColorGradient.colorMode = (ColorMode)int.Parse(localColorGradientValue_Dict["colorMode"].ToString());
        var colors_List = JsonConvert.DeserializeObject<List<object>>(localColorGradientValue_Dict["colors"].ToString());
        var topLeft_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(colors_List[0].ToString());
        localColorGradient.topLeft = new Color(float.Parse(topLeft_dict["r"].ToString()), float.Parse(topLeft_dict["g"].ToString()),
            float.Parse(topLeft_dict["b"].ToString()), float.Parse(topLeft_dict["a"].ToString()));
        var topRight_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(colors_List[1].ToString());
        localColorGradient.topRight = new Color(float.Parse(topRight_dict["r"].ToString()), float.Parse(topRight_dict["g"].ToString()),
            float.Parse(topRight_dict["b"].ToString()), float.Parse(topRight_dict["a"].ToString()));
        var bottomLeft_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(colors_List[2].ToString());
        localColorGradient.bottomLeft = new Color(float.Parse(bottomLeft_dict["r"].ToString()), float.Parse(bottomLeft_dict["g"].ToString()),
            float.Parse(bottomLeft_dict["b"].ToString()), float.Parse(bottomLeft_dict["a"].ToString()));
        var bottomRight_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(colors_List[1].ToString());
        localColorGradient.bottomRight = new Color(float.Parse(bottomRight_dict["r"].ToString()), float.Parse(bottomRight_dict["g"].ToString()),
            float.Parse(bottomRight_dict["b"].ToString()), float.Parse(bottomRight_dict["a"].ToString()));
        tmp.colorGradientPreset = localColorGradient;
        tmp.richText = bool.Parse(textTMP_Dict["_isRichText"].ToString());
        tmp.isRightToLeftText = bool.Parse(textTMP_Dict["_isRightToLeft"].ToString());
        tmp.enableAutoSizing = bool.Parse(textTMP_Dict["_enableAutoSizing"].ToString());
        var autoSizeOption_Dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(textTMP_Dict["_autoSizeOptions"].ToString());
        tmp.fontSizeMin = float.Parse(autoSizeOption_Dict["fontSizeMin"].ToString());
        tmp.fontSizeMax = float.Parse(autoSizeOption_Dict["fontSizeMax"].ToString());
        tmp.characterWidthAdjustment = float.Parse(autoSizeOption_Dict["wdPercent"].ToString());
        tmp.lineSpacingAdjustment = float.Parse(autoSizeOption_Dict["lineSpacingMax"].ToString());
        tmp.fontSize = float.Parse(textTMP_Dict["_fontSize"].ToString());
        tmp.fontWeight = (FontWeight)int.Parse(textTMP_Dict["_fontWeight"].ToString());
        tmp.fontStyle = (FontStyles)int.Parse(textTMP_Dict["_fontStyle"].ToString());
        tmp.horizontalAlignment = (HorizontalAlignmentOptions)int.Parse(textTMP_Dict["_horizontalAlignment"].ToString());
        tmp.verticalAlignment = (VerticalAlignmentOptions)int.Parse(textTMP_Dict["_verticalAlignment"].ToString());
        tmp.geometrySortingOrder = (VertexSortingOrder)int.Parse(textTMP_Dict["_geometrySortingOrder"].ToString());
        tmp.wordWrappingRatios = float.Parse(textTMP_Dict["_wordWrappingRatios"].ToString());
        tmp.overflowMode = (TextOverflowModes)int.Parse(textTMP_Dict["_overflowMode"].ToString());
        var margin_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(textTMP_Dict["_margin"].ToString());
        tmp.margin = new Vector4(float.Parse(margin_dict["x"].ToString()), float.Parse(margin_dict["y"].ToString()),
            float.Parse(margin_dict["z"].ToString()), float.Parse(margin_dict["w"].ToString()));
        tmp.horizontalMapping = (TextureMappingOptions)int.Parse(textTMP_Dict["_horizontalMapping"].ToString());
        tmp.verticalMapping = (TextureMappingOptions)int.Parse(textTMP_Dict["_verticalMapping"].ToString());
        tmp.isTextObjectScaleStatic = bool.Parse(textTMP_Dict["_isTextObjectScaleStatic"].ToString());
        tmp.isOrthographic = bool.Parse(textTMP_Dict["_isOrthographicMode"].ToString());
        tmp.overrideColorTags = bool.Parse(textTMP_Dict["_overrideHtmlColors"].ToString());
        tmp.parseCtrlCharacters = bool.Parse(textTMP_Dict["_isParseCtrlCharacters"].ToString());
        tmp.extraPadding = bool.Parse(textTMP_Dict["_enableExtraPadding"].ToString());
        tmp.enableKerning = bool.Parse(textTMP_Dict["_enableKerning"].ToString());
        tmp.tintAllSprites = bool.Parse(textTMP_Dict["_isTintAllSprites"].ToString());
        tmp.vertexBufferAutoSizeReduction = bool.Parse(textTMP_Dict["_isVertexBufferAutoSizeReduction"].ToString());
        tmp.characterWidthAdjustment = float.Parse(textTMP_Dict["_charWidthMaxAdj"].ToString());
        tmp.pageToDisplay = int.Parse(textTMP_Dict["_pageToDisplay"].ToString());
        tmp.useMaxVisibleDescender = bool.Parse(textTMP_Dict["_useMaxVisibleDescender"].ToString());
        tmp.isVolumetricText = bool.Parse(textTMP_Dict["_isVolumetricText"].ToString());
        tmp.enableWordWrapping = bool.Parse(textTMP_Dict["_enableWordWrapping"].ToString());
        tmp.enableCulling = bool.Parse(textTMP_Dict["_isCullingEnabled"].ToString());
    }

    static void ParseScriptComponent(GameObject owner, TreeNode component_node)
    {
        string domainName = string.Empty;
        string className = string.Empty;
        try
        {
            var nodeInfo_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(component_node.nodeInfo);
            bool enabled = bool.Parse(nodeInfo_dict["_enabled"].ToString());
            domainName = nodeInfo_dict["_domainName"].ToString();
            className = nodeInfo_dict["_className"].ToString();
            Type type = Type.GetType(domainName + "." + className);
            Component component = (Component)type.Assembly.CreateInstance(type.FullName);
            owner.AddComponent<Component>();
        }catch(Exception ex)
        {
            ReportSystem.OutputLog($"解析节点[{owner.name}]引用的ScriptComponent脚本组件[{domainName}.{className}]出错.");
        }

    }

}
