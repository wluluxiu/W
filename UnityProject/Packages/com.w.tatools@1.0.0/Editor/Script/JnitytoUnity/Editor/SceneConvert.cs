using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConvert
{
    public static void ConvertScene(string sceneUUID, List<object> jsonStr)
    {
        ConvertUtil.allGameObjects.Clear();
        List<long> worldChildrenIds = new List<long>();
        for (int n = 0; n < jsonStr.Count; n++)
        {
            string object_s = (string)jsonStr[n].ToString();
            var jsonStr_n = JsonConvert.DeserializeObject<Dictionary<object, object>>(object_s);
            long localId = long.Parse(jsonStr_n["localId"].ToString());
            int typeId = int.Parse(jsonStr_n["typeId"].ToString());
            if (jsonStr_n.ContainsKey("World3D"))
            {
                var world3D_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonStr_n["World3D"].ToString());
                var world_childrens = JsonConvert.DeserializeObject<List<object>>(world3D_dict["_children"].ToString());
                for(int k = 0; k < world_childrens.Count; k++)
                {
                    var LocalIdDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(world_childrens[k].ToString());
                    worldChildrenIds.Add(long.Parse(LocalIdDict["localId"].ToString()));
                }  
               
            }
        }

		//ConvertUtil.attachNodeList.Clear();
		ConvertUtil.animNodeList.Clear();
		ConvertUtil.subEmitterList.Clear();
		ConvertUtil.collisionPlaneList.Clear();
		ConvertUtil.dynamicBoneList.Clear();

		GameObject[] childObjs = new GameObject[worldChildrenIds.Count];
        for(int n = 0; n < worldChildrenIds.Count; n++)
        {
            Dictionary<long, TreeNode> treeNode_dict = new Dictionary<long, TreeNode>();
            ConvertUtil.PreFilter(jsonStr, ref treeNode_dict);
           
            TreeNode rootNode = treeNode_dict[worldChildrenIds[n]];
          
            childObjs[n] = PrefabConvert.ConvertChildNode(sceneUUID, rootNode, treeNode_dict, null);
           
		}

		//for (int k = 0; k < ConvertUtil.attachNodeList.Count; k++)
		//{
		//	AttachNodeInfo attachInfo = ConvertUtil.attachNodeList[k];
		//	string attachBoneName = attachInfo.attchNodeName;
		//	Transform boneT = null;
		//	GameObject boneObj = GameObject.Find(attachBoneName);
		//	if (boneObj)
		//		boneT = boneObj.transform;
		//	if (boneT)
		//	{
		//		attachInfo.selfObj.transform.parent = boneT;
		//		attachInfo.selfObj.transform.localPosition = Vector3.zero;
		//		attachInfo.selfObj.transform.localRotation = Quaternion.identity;
		//		attachInfo.selfObj.transform.localScale = Vector3.one;
		//	}
		//	else
		//	{
		//		ReportSystem.OutputLog($"解析AttachNode节点[{attachInfo.selfObj.name}]出错，绑定的骨骼{attachBoneName}找不到！");
		//	}
		//}
		//ConvertUtil.attachNodeList.Clear();

		for (int k = 0; k < ConvertUtil.animNodeList.Count; k++)
		{
			AnimNodeInfo animNodeInfo = ConvertUtil.animNodeList[k];
			PrefabConvert.ParseAnimation(animNodeInfo.ownerObj, animNodeInfo.treeNode);
		}
		ConvertUtil.animNodeList.Clear();

		for (int k = 0; k < ConvertUtil.subEmitterList.Count; k++)
		{
			string globalId = sceneUUID + "|" + ConvertUtil.subEmitterList[k].localId;
			GameObject subObj = ConvertUtil.allGameObjects[globalId];
			ParticleSystem subps = subObj.GetComponent<ParticleSystem>();
			ParticleSystem.ShapeModule shapeModule = subps.shape;
			shapeModule.rotation = new Vector3(shapeModule.rotation.x + 90, shapeModule.rotation.y, shapeModule.rotation.z);
			ParticleSystem parentPs = ConvertUtil.subEmitterList[k].parentObj.GetComponent<ParticleSystem>();
			parentPs.subEmitters.AddSubEmitter(subps, ConvertUtil.subEmitterList[k].type, ConvertUtil.subEmitterList[k].property);
		}
		ConvertUtil.subEmitterList.Clear();

		for (int k = 0; k < ConvertUtil.collisionPlaneList.Count; k++)
		{
			string globalId = sceneUUID + "|" + ConvertUtil.collisionPlaneList[k].localId;
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

}
