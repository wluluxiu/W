using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimControllerSync
{
    public class ControllerAttri
    {
        public string localId;
        public string typeId;
        public string attributeName;
        public string attributeContent;
    }

    static AnimationClip getAnimationInFbx(Transform animOwner, string fbx_uuid,long clip_localId, string jnity_controller_path)
    {
        JnityResConfig resConfig = Jnity2Unity.jnityResConfig;
        string jnity_meta_path;
        string jnity_fbx_path;
        string assetPath;
        string unity_fbx_path;
        if (resConfig.resFiles.ContainsKey(fbx_uuid))
        {
            jnity_fbx_path = resConfig.resFiles[fbx_uuid].j_path;
            jnity_meta_path = resConfig.resFiles[fbx_uuid].j_metaPath;
            assetPath = FileUtil.GetAssetPath(jnity_fbx_path);
            unity_fbx_path = Application.dataPath + assetPath.Replace("Assets", "");
        }
        else
        {
            jnity_fbx_path = JnityBuildInRes.buildInResMap[fbx_uuid].path;
            jnity_meta_path = JnityBuildInRes.buildInResMap[fbx_uuid].path + ".metax";
            assetPath = Jnity2Unity.unityRelativeAssetPath + JnityBuildInRes.buildInResMap[fbx_uuid].idmap_path;
            unity_fbx_path = Jnity2Unity.unityAssetPath + JnityBuildInRes.buildInResMap[fbx_uuid].idmap_path;
        }

        if (jnity_fbx_path.EndsWith(".anim"))
        {
            ReportSystem.OutputLog($"解析AnimatorController[{jnity_controller_path}]出错, AnimationState引用了老版本的anim文件[{jnity_fbx_path}]!");
            return null;
        }else if (jnity_fbx_path.EndsWith(".animatorclip"))
        {
            return AnimationSync.ConvertAnimation(jnity_fbx_path, animOwner);
        }

        FileInfo file = new FileInfo(unity_fbx_path);
        //同步mesh文件 from jnity to unity
        //if (!file.Exists)
        //{
        try
        {
            FileUtil.CopyFile(jnity_fbx_path, unity_fbx_path);
            AssetDatabase.Refresh();
        }catch(Exception ex)
        {
            Debug.Log("getAnimationInFbx error:" + ex.ToString());
        }
        ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(assetPath);
        FbxSync.SyncMetaFile(importer, jnity_meta_path);
        //}

        string text = FileUtil.ReadTexTFile(jnity_meta_path);
        var jsonStr = JsonConvert.DeserializeObject<Dictionary<object, object>>(text);
        string importer_str = jsonStr["FbxImporter"].ToString();
        var importer_attribute = JsonConvert.DeserializeObject<Dictionary<object, object>>(importer_str);
        var idToNameList = JsonConvert.DeserializeObject<List<object>>(importer_attribute["_localIdToName"].ToString());
        string assetName = null;
        for(int n = 0; n < idToNameList.Count; n++)
        {
            var idToNameDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(idToNameList[n].ToString());
            if (clip_localId == long.Parse(idToNameDict["p0"].ToString()))
            {
                assetName = idToNameDict["p1"].ToString();
            }
        }
        if (assetName == null)
            return null;


        AnimationClip clip = null;
        UnityEngine.Object[] fbxObjects = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        for(int n = 0; n < fbxObjects.Length; n++)
        {
            if(fbxObjects[n].name.Equals(assetName) && fbxObjects[n] is AnimationClip)
            {
                clip = (AnimationClip)fbxObjects[n];
                break;
            }
        }
        return clip;
    }


    public static void ConvertController(Transform animOwner, string jnity_path,string assetPath)
    {
        string text = FileUtil.ReadTexTFile(jnity_path);
        var attributeList = JsonConvert.DeserializeObject<List<object>>(text);
        var controller = AnimatorController.CreateAnimatorControllerAtPath(assetPath);
        Dictionary<string, ControllerAttri> animLayers = new Dictionary<string, ControllerAttri>();
        Dictionary<string, ControllerAttri> animStates = new Dictionary<string, ControllerAttri>();
        Dictionary<string, ControllerAttri> animStateMachines = new Dictionary<string, ControllerAttri>();
        Dictionary<string, ControllerAttri> motionEntitys = new Dictionary<string, ControllerAttri>();
		Dictionary<string, ControllerAttri> stateTransition = new Dictionary<string, ControllerAttri>();
		for (int n = 0; n < attributeList.Count; n++)
        {
            ControllerAttri attri = new ControllerAttri();
            var attributeDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(attributeList[n].ToString());
            attri.localId = attributeDict["localId"].ToString();
            attri.typeId = attributeDict["typeId"].ToString();
            
            if (attributeDict.ContainsKey("AnimatorControllerLayer"))
            {
                attri.attributeName = "AnimatorControllerLayer";
                attri.attributeContent = attributeDict["AnimatorControllerLayer"].ToString();
                animLayers.Add(attri.localId, attri);
            }else if (attributeDict.ContainsKey("AnimatorState"))
            {
                attri.attributeName = "AnimatorState";
                attri.attributeContent = attributeDict["AnimatorState"].ToString();
                animStates.Add(attri.localId, attri);
            }
            else if (attributeDict.ContainsKey("AnimatorStateMachine"))
            {
                attri.attributeName = "AnimatorStateMachine";
                attri.attributeContent = attributeDict["AnimatorStateMachine"].ToString();
                animStateMachines.Add(attri.localId, attri);
            }
            else if (attributeDict.ContainsKey("MotionEntity"))
            {
                attri.attributeName = "MotionEntity";
                attri.attributeContent = attributeDict["MotionEntity"].ToString();
                motionEntitys.Add(attri.localId, attri);
            }
			else if (attributeDict.ContainsKey("AnimatorStateTransition"))
			{
				attri.attributeName = "AnimatorStateTransition";
				attri.attributeContent = attributeDict["AnimatorStateTransition"].ToString();
				stateTransition.Add(attri.localId, attri);
			}

        }

        
        for(int n = 0; n < attributeList.Count; n++)
        {
            var attributeDict = JsonConvert.DeserializeObject<Dictionary<object,object>>(attributeList[n].ToString());
            if (attributeDict.ContainsKey("AnimatorController"))
            {
                var animatorControllerDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(attributeDict["AnimatorController"].ToString());
                var animatorLayers = JsonConvert.DeserializeObject<List<object>>(animatorControllerDict["_animatorLayers"].ToString());           
                for (int k = 0; k < animatorLayers.Count; k++)
                {
                    var animatorLayerDict = JsonConvert.DeserializeObject<Dictionary<object,object>>(animatorLayers[k].ToString());
                    string localId = animatorLayerDict["localId"].ToString();

                    //解析animLayer
                    var animLayerDict = JsonConvert.DeserializeObject < Dictionary<object, object> > (animLayers[localId].attributeContent);
                    if(k > 0)
                        controller.AddLayer(animLayerDict["_name"].ToString());
                    var stateMachineDict = JsonConvert.DeserializeObject<Dictionary<object, object>> (animLayerDict["_stateMachine"].ToString());
                    controller.layers[k].blendingMode = (AnimatorLayerBlendingMode)int.Parse(animLayerDict["_blendMode"].ToString());
                    controller.layers[k].defaultWeight = int.Parse(animLayerDict["_weight"].ToString());
                    AnimatorControllerLayer layer = controller.layers[k];
                    
                    //解析statemachine
                    var animStateMachineDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateMachines[stateMachineDict["localId"].ToString()].attributeContent);              
                    string stateMachine_name = animStateMachineDict["_name"].ToString();
                    var stateMachine_position_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateMachineDict["_position"].ToString());
                    AnimatorStateMachine stateMachine = layer.stateMachine;//.AddStateMachine(stateMachine_name, new Vector3(float.Parse(stateMachine_position_dict["x"].ToString()), float.Parse(stateMachine_position_dict["y"].ToString())));
                    var entryPositionDict = JsonConvert.DeserializeObject < Dictionary<object, object> > (animStateMachineDict["_entryPosition"].ToString());
                    stateMachine.entryPosition = new UnityEngine.Vector3(float.Parse(entryPositionDict["x"].ToString()), float.Parse(entryPositionDict["y"].ToString()), 0);
                    var anyPositionDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateMachineDict["_anyPosition"].ToString());
                    stateMachine.anyStatePosition = new UnityEngine.Vector3(float.Parse(anyPositionDict["x"].ToString()), float.Parse(anyPositionDict["y"].ToString()), 0);
                    var exitPositionDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateMachineDict["_exitPosition"].ToString());
                    stateMachine.exitPosition = new UnityEngine.Vector3(float.Parse(exitPositionDict["x"].ToString()), float.Parse(exitPositionDict["y"].ToString()), 0);
                    var parentStateMachinePositionDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateMachineDict["_parentStateMachinePosition"].ToString());
                    stateMachine.parentStateMachinePosition = new UnityEngine.Vector3(float.Parse(parentStateMachinePositionDict["x"].ToString()), float.Parse(parentStateMachinePositionDict["y"].ToString()), 0);
                    var defaultStateDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateMachineDict["_defaultState"].ToString());
                    string defaultState_localId = defaultStateDict["localId"].ToString();

					//解析animatorstate
					Dictionary<string, AnimatorState> animatorStateTable = new Dictionary<string, AnimatorState>();
                    var animatorstateList = JsonConvert.DeserializeObject<List<object>>(animStateMachineDict["_animatorStates"].ToString());
                    for(int m = 0; m < animatorstateList.Count; m++)
                    {
                        var animatorstateDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animatorstateList[m].ToString());
                        var animstate_localId = animatorstateDict["localId"].ToString();
                        var animStateDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStates[animstate_localId].attributeContent);
                        var position_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateDict["_position"].ToString());
                        AnimatorState animatorState = stateMachine.AddState(animStateDict["_name"].ToString(), new Vector3(float.Parse(position_dict["x"].ToString()), float.Parse(position_dict["y"].ToString())));
                        if (defaultState_localId.Equals(animstate_localId))
                        {
                            stateMachine.defaultState = animatorState;
                        }
                        var speed_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateDict["_speed"].ToString());
                        animatorState.speed = float.Parse(speed_dict["defaultValue"].ToString());
                        animatorState.speedParameterActive = bool.Parse(speed_dict["active"].ToString());
                        animatorState.speedParameter = speed_dict["parameter"].ToString();
                        var cycleOffset_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateDict["_cycleOffset"].ToString());
                        animatorState.cycleOffset = float.Parse(cycleOffset_dict["defaultValue"].ToString());
                        animatorState.cycleOffsetParameter = cycleOffset_dict["parameter"].ToString();
                        animatorState.cycleOffsetParameterActive = bool.Parse(cycleOffset_dict["active"].ToString());
                        var mirror_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateDict["_mirror"].ToString());
                        animatorState.mirror = bool.Parse(mirror_dict["defaultValue"].ToString());
                        animatorState.mirrorParameter = mirror_dict["parameter"].ToString();
                        animatorState.mirrorParameterActive = bool.Parse(mirror_dict["active"].ToString());
                        animatorState.iKOnFeet = bool.Parse(animStateDict["_iKOnFeet"].ToString());
                        
                        //解析motion
                        var motion_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(animStateDict["_motion"].ToString());
                        string motion_localId = motion_dict["localId"].ToString();
                        var motion_entity_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(motionEntitys[motion_localId].attributeContent);
                        var clip_dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(motion_entity_dict["_clip"].ToString());
                        AnimationClip clip = getAnimationInFbx(animOwner,clip_dict["uuid"].ToString(), long.Parse(clip_dict["localId"].ToString()), jnity_path);
                        animatorState.motion = clip;

						animatorStateTable.Add(animstate_localId, animatorState);

					   // stateMachine.AddAnyStateTransition(animatorState);
					}

					//解析stateTransition
					var transitionList = JsonConvert.DeserializeObject<List<object>>(animStateMachineDict["_stateTransitions"].ToString());
					for(int m = 0; m < transitionList.Count; m++)
					{
						var transitionDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(transitionList[m].ToString());
						var transition_localId = transitionDict["localId"].ToString();
						transitionDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(stateTransition[transition_localId].attributeContent);
						var sourceState = JsonConvert.DeserializeObject<Dictionary<object, object>>(transitionDict["_sourceState"].ToString());
						string sourceState_id = sourceState["localId"].ToString();
						var destinationState = JsonConvert.DeserializeObject<Dictionary<object, object>>(transitionDict["_destinationState"].ToString());
						string destinationState_id = destinationState["localId"].ToString();
						bool _isExit = bool.Parse(transitionDict["_isExit"].ToString());
						AnimatorStateTransition transition = animatorStateTable[sourceState_id].AddTransition(animatorStateTable[destinationState_id], _isExit);
						transition.mute = bool.Parse(transitionDict["_mute"].ToString());
						transition.solo = bool.Parse(transitionDict["_solo"].ToString());
						transition.name = transitionDict["_name"].ToString();
						transition.canTransitionToSelf = bool.Parse(transitionDict["_canTransitionToSelf"].ToString());
						transition.hasExitTime = bool.Parse(transitionDict["_hasExitTime"].ToString());
						transition.hasFixedDuration = bool.Parse(transitionDict["_hasFixedDuration"].ToString());
						transition.orderedInterruption = bool.Parse(transitionDict["_orderedInterruption"].ToString());
						transition.duration = float.Parse(transitionDict["_duration"].ToString());
						transition.offset = float.Parse(transitionDict["_offset"].ToString());
						transition.exitTime = float.Parse(transitionDict["_exitTime"].ToString());
						transition.interruptionSource = (TransitionInterruptionSource)float.Parse(transitionDict["_interruptionSource"].ToString());
					}
				}
            }
        }

    }
}
