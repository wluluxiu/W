using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.Linq;
public class FbxSync
{
    static ModelImporterTangents getImporterTangents(int tangents)
    {
        switch (tangents)
        {
            case 0:
                return ModelImporterTangents.Import;
            case 1:
                return ModelImporterTangents.CalculateMikk;
            case 2:
                return ModelImporterTangents.CalculateLegacy;
            case 3:
                return ModelImporterTangents.None;
            default:
                return ModelImporterTangents.None;

        }
    }

    //static Transform getBoneRoot(Transform bone)
    //{
    //    Transform curBone;
    //    Transform parent = bone.parent;
    //    while (parent != null)
    //    {
    //        curBone = parent;
    //        parent = parent.parent;

    //    }

    //}

    public static Transform[] initFBXBones(string objectName, string bone_root_name, string meshName, string assetPath, Transform parent, ref string fbxRootBoneName)
    {
        if (objectName == null)
            return null;
        Transform[] bones = null;
        Object[] objects = (Object[])AssetDatabase.LoadAllAssetsAtPath(assetPath);
        bool foundBone = false;
        //如果不改skinmeshrender的节点名可以找到
        for (int n = 0; n < objects.Length; n++)
        {
            GameObject gameObject = objects[n] as GameObject;
            if (gameObject && gameObject.name.Equals(objectName))
            {
                SkinnedMeshRenderer smr;
                gameObject.TryGetComponent(out smr);
                if (smr)
                {
                    bones = smr.bones;
                    if (smr.rootBone == null)
                        return null;
                    fbxRootBoneName = smr.rootBone.name;
                    foundBone = true;
                    break;

                }

            }else if (gameObject)
            {
                SkinnedMeshRenderer smr;
                gameObject.TryGetComponent(out smr);
                if (smr && smr.rootBone == null)
                    return null;
                if (smr && smr.rootBone.name.Equals(bone_root_name) && smr.sharedMesh.name.Equals(meshName))
                {
                    bones = smr.bones;
                    fbxRootBoneName = smr.rootBone.name;
                    foundBone = true;
                    break;

                }

            }
            
        }

        //if (!foundBone)
        //{
        //    //如果修改了节点名，再根据主骨骼找
        //    for (int n = 0; n < objects.Length; n++)
        //    {
        //        GameObject gameObject = objects[n] as GameObject;
        //        if (gameObject)
        //        {
        //            SkinnedMeshRenderer smr;
        //            gameObject.TryGetComponent(out smr);
        //            if (smr && smr.rootBone.name.Equals(bone_root_name) && smr.sharedMesh.name.Equals(meshName))
        //            {
        //                bones = smr.bones;
        //                fbxRootBoneName = smr.rootBone.name;
        //                foundBone = true;
        //                break;

        //            }

        //        }
        //    }
        //}

        if (!foundBone)
        {
            //如果修改了节点名，再根据引用的mesh找
            for (int n = 0; n < objects.Length; n++)
            {
                GameObject gameObject = objects[n] as GameObject;
                if (gameObject)
                {
                    SkinnedMeshRenderer smr;
                    gameObject.TryGetComponent(out smr);
                    if (smr && smr.rootBone == null)
                        return null;
                    if (smr && smr.sharedMesh.name.Equals(meshName))
                    {
                        bones = smr.bones;
                        fbxRootBoneName = smr.rootBone.name;
                        foundBone = true;
                        break;

                    }

                }
            }
        }

        //如果既修改了节点名，主骨骼也改了， 引用的mesh也不一致了，那只能提示美术把节点名改回去再重新生成
        if (bones == null)
        {
            ReportSystem.OutputLog($"同步fbx中的骨骼动画[{assetPath}]出错，[{objectName}]节点的skinmeshrender组件因节点的名字和主骨骼已修改，" +
                $"无法从fbx中加载对应的骨骼信息,请美术同学把节点的名字改回去再重新导出！");
        }

        return bones;
    }

    public static (Mesh,GameObject) getMeshFromFBX(string meshName, string assetPath)
    {
        if (meshName == null)
            return (null,null);
		Mesh mesh = null;
		GameObject meshObj = null;
        Object[] assetObjs = (Object[])AssetDatabase.LoadAllAssetsAtPath(assetPath);
        for (int n = 0; n < assetObjs.Length; n++)
        {
            Mesh meshTemp = assetObjs[n] as Mesh;
			GameObject meshObjTemp = assetObjs[n] as GameObject;
            if (meshTemp && meshTemp.name.Equals(meshName))
            {
                mesh = meshTemp;
            }else if (meshObjTemp)
			{
				MeshFilter mf = null;
				meshObjTemp.TryGetComponent<MeshFilter>(out mf);
				if(mf && mf.sharedMesh.name.Equals(meshName))
				{
					meshObj = meshObjTemp;
				}
				SkinnedMeshRenderer smr = null;
				meshObjTemp.TryGetComponent<SkinnedMeshRenderer>(out smr);
				if (smr && smr.sharedMesh.name.Equals(meshName))
				{
					meshObj = meshObjTemp;
				}
			}
        }
        return (mesh,meshObj);
    }

    public static string getMeshName(string localId,string path,string nodeName)
    {
        string text = FileUtil.ReadTexTFile(path);
        var jsonStr = JsonConvert.DeserializeObject<Dictionary<object, object>>(text);
        string importer_str = jsonStr["FbxImporter"].ToString();
        var importer_attribute = JsonConvert.DeserializeObject<Dictionary<object, object>>(importer_str);
        var localIdList = JsonConvert.DeserializeObject<List<object>>(importer_attribute["_localIdToName"].ToString());
        for(int n = 0;  n < localIdList.Count; n++)
        {
           var localIdDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(localIdList[n].ToString());
           long localIdValue = long.Parse(localId);
           long p0 = long.Parse(localIdDict["p0"].ToString());
           if(p0 == localIdValue)
           {
                return localIdDict["p1"].ToString();
           }
        }
        ReportSystem.OutputLog($"预制体中的节点[{nodeName}]中引用的mesh[localId:{localId}]在fbx文件[{path}]中找不到对应的mesh信息！");
        return null;
    }

    public static void SyncMetaFile(ModelImporter importer, string path)
    {
        bool isHumanAnimation = false;
        string text = FileUtil.ReadTexTFile(path);
        var jsonStr = JsonConvert.DeserializeObject<Dictionary<object, object>>(text);
        string importer_str = jsonStr["FbxImporter"].ToString();
        var importer_attribute = JsonConvert.DeserializeObject<Dictionary<object, object>>(importer_str);
        int meshCompress = int.Parse(importer_attribute["ms_meshCompression"].ToString());
        bool optimizeMesh = bool.Parse(importer_attribute["ms_optimizeMesh"].ToString());
        float scaleFactor = float.Parse(importer_attribute["ms_scaleFactor"].ToString());
        bool importerBlendShapes = bool.Parse(importer_attribute["ms_importBlendShapes"].ToString());
        bool hasNormals = bool.Parse(importer_attribute["ms_hasNormals"].ToString());
        bool hasTangents = bool.Parse(importer_attribute["ms_hasTangents"].ToString());
        int importNormals = int.Parse(importer_attribute["ms_normals"].ToString());
        int importTangents = int.Parse(importer_attribute["ms_tangents"].ToString());
		bool addUV1 = false;
		if(importer_attribute.ContainsKey("ms_addUV1"))
            addUV1 = bool.Parse(importer_attribute["ms_addUV1"].ToString());
        bool readWriteEnable = bool.Parse(importer_attribute["ms_readWriteEnabled"].ToString());
        bool swapUVs = bool.Parse(importer_attribute["ms_swapUVs"].ToString());
        bool generateLightmapUVs = bool.Parse(importer_attribute["ms_generateLightmapUVs"].ToString());
        int minLightmapResolution = int.Parse(importer_attribute["ms_minLightmapResolution"].ToString());
        bool importVisibility = bool.Parse(importer_attribute["ms_importVisibility"].ToString());
        int indexFormat = int.Parse(importer_attribute["ms_indexFormat"].ToString());
        bool generateColliders = bool.Parse(importer_attribute["ms_generateColliders"].ToString());
        int calculateMode = int.Parse(importer_attribute["ms_calculateMode"].ToString());
        int animationType = int.Parse(importer_attribute["rs_animationType"].ToString());
        int avatar_def = int.Parse(importer_attribute["rs_avatarDefinition"].ToString());
        bool weldVertices = bool.Parse(importer_attribute["ms_weldVertices"].ToString());

        var animation_importer_str = importer_attribute["fbxImporterAS"].ToString();
        var animation_importer_attribute = JsonConvert.DeserializeObject<Dictionary<object, object>>(animation_importer_str);
        int animCompression = int.Parse(animation_importer_attribute["animCompression"].ToString());
        float rotationError = float.Parse(animation_importer_attribute["rotationError"].ToString());
        float positionError = float.Parse(animation_importer_attribute["positionError"].ToString());
        float scaleError = float.Parse(animation_importer_attribute["scaleError"].ToString());
        bool resampleCurves = bool.Parse(animation_importer_attribute["resampleCurves"].ToString());
        bool legacyAnimation = bool.Parse(animation_importer_attribute["legacyAnimation"].ToString());

        importer.meshCompression = (ModelImporterMeshCompression)meshCompress;
        importer.globalScale = scaleFactor;
        importer.optimizeMesh = optimizeMesh;
        importer.importBlendShapes = importerBlendShapes;
        importer.importNormals = (ModelImporterNormals)importNormals;
        importer.importTangents = getImporterTangents(importTangents);
        importer.swapUVChannels = swapUVs;
        importer.generateSecondaryUV = addUV1 | generateLightmapUVs;      
        importer.secondaryUVMinLightmapResolution = minLightmapResolution;
        importer.importVisibility = importVisibility;
        importer.weldVertices = weldVertices;
        importer.avatarSetup = (ModelImporterAvatarSetup)(avatar_def + 1);
        importer.indexFormat = (ModelImporterIndexFormat)indexFormat;
        importer.addCollider = generateColliders;
        importer.normalCalculationMode = (ModelImporterNormalCalculationMode)(calculateMode + 1);
        importer.useFileUnits = false;
        importer.useFileScale = false;
        //if (path.Contains("SWK"))
        //{
            importer.bakeAxisConversion = true;
        //}
        //else
        //{
        //    importer.bakeAxisConversion = false;
        //}
        importer.animationCompression = (ModelImporterAnimationCompression)animCompression;
        importer.animationRotationError = rotationError;
        importer.animationPositionError = positionError;
        importer.animationScaleError = scaleError;
        importer.resampleCurves = resampleCurves;
        importer.animationType = animationType == 0? ModelImporterAnimationType.None : ModelImporterAnimationType.Generic;

        if (isHumanAnimation)
        {
            importer.animationType = ModelImporterAnimationType.Human;
            importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            importer.motionNodeName = "<Root Transform>";
        }

        var animationClipsList = JsonConvert.DeserializeObject<List<object>>(animation_importer_attribute["clips"].ToString());
        if(animationClipsList != null)
        {
            ModelImporterClipAnimation[] clipAnimations = new ModelImporterClipAnimation[animationClipsList.Count];
            for (int n = 0; n < animationClipsList.Count; n++)
            {
                var animationClipsDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animationClipsList[n].ToString());
                float fbxClipLength = float.Parse(animationClipsDict["fbxClipLength"].ToString());
                float fbxSampleRate = float.Parse(animationClipsDict["fbxSampleRate"].ToString());
                float startTime = float.Parse(animationClipsDict["startTime"].ToString());
                startTime = Mathf.Min(startTime, fbxClipLength);
                float stopTime = float.Parse(animationClipsDict["stopTime"].ToString());
                stopTime = Mathf.Min(stopTime, fbxClipLength);

                clipAnimations[n] = new ModelImporterClipAnimation();
                clipAnimations[n].firstFrame = startTime * fbxSampleRate;
                clipAnimations[n].lastFrame = stopTime * fbxSampleRate;
                clipAnimations[n].loopPose = bool.Parse(animationClipsDict["loopPose"].ToString());
                clipAnimations[n].loopTime = bool.Parse(animationClipsDict["loopTime"].ToString());
                clipAnimations[n].cycleOffset = float.Parse(animationClipsDict["cycleOffset"].ToString());
                clipAnimations[n].takeName = animationClipsDict["fbxClipName"].ToString();
                clipAnimations[n].name = animationClipsDict["clipName"].ToString();
                if (isHumanAnimation)
                {
                    clipAnimations[n].keepOriginalOrientation = true;
                    clipAnimations[n].keepOriginalPositionXZ = true;
                    clipAnimations[n].keepOriginalPositionY = true;
                }
				int maskType = int.Parse(animationClipsDict["clipMask"].ToString());
				clipAnimations[n].maskType = (ClipAnimationMaskType)(maskType > 1 ? 3 : maskType);
				

				var animationEventArray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(animationClipsDict["events"].ToString());
				ConvertAnimationEvent(animationEventArray, clipAnimations[n], stopTime);
				

			}
            importer.clipAnimations = clipAnimations;
           
            if (importer.clipAnimations.Length > 0)
                importer.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;

        }
        
		 EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
       
    }


	static void ConvertAnimationEvent (List<object> animationEventArray, ModelImporterClipAnimation newClip, float stopTime)
	{
		AnimationEvent[] events = new AnimationEvent[animationEventArray.Count];
		for (int n = 0; n < animationEventArray.Count; n++)
		{
			var animationEvent_Dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(animationEventArray[n].ToString());
			events[n] = new AnimationEvent();
			events[n].time = float.Parse(animationEvent_Dict["_time"].ToString()) / stopTime;
			events[n].floatParameter = float.Parse(animationEvent_Dict["_floatParameter"].ToString());
			events[n].intParameter = int.Parse(animationEvent_Dict["_intParameter"].ToString());
			events[n].stringParameter = animationEvent_Dict["_stringParameter"].ToString();
			events[n].functionName = animationEvent_Dict["_functionName"].ToString();


		}
		newClip.events = events;
		
	}
}
