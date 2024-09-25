using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;

public class AnimationConvert
{
    public class Action : ScriptableObject
    {
        public Transform Att;
    }


    enum Jnity_TangentMode
    {
        ClampedAuto = 0,
        Auto = 1,
        FreeSmooth = 2,
        Flat = 3,
        Free_Free = 4,
        Free_Linear = 5,
        Free_Constant = 6,
        Linear_Free = 7,
        Linear_Linear = 8,
        Linear_Constant = 9,
        Constant_Free = 10,
        Constant_Linear = 11,
        Constant_Constant = 12,
        TangentModeCount = 13
    }

    class ElurKeyFrames
    {
        public List<Vector3> elurs;
        public ElurKeyFrames()
        {
            elurs = new List<Vector3>();

        }
    }

    static void ConvertBezierTrack(List<object> bezierTrackArray, AnimationClip clip, Transform animOwner)
    {

        for (int n = 0; n < bezierTrackArray.Count; n++)
        {
            AnimationCurve animCurve = new AnimationCurve();
            var bezierTrack_str = bezierTrackArray[n].ToString();
            var bezierTrack_Dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(bezierTrack_str);
            var curve_str = bezierTrack_Dict["curve"];
            var path = bezierTrack_Dict["path"].ToString();
            var attribute = bezierTrack_Dict["attribute"].ToString();
            bool needFlip;
            Type componetType;
            bool needRotate180;
            string bindPropertyName = AttributeConvertToBindingName(attribute, path, animOwner, out needFlip, out needRotate180, out componetType);
            var curveStr_n = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(curve_str.ToString());
            var keyFrameList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(curveStr_n["_keys"].ToString());
            animCurve.preWrapMode = ConvertUtil.ConvertAnimationWrapMode(int.Parse(curveStr_n["_preInfinity"].ToString()));
            animCurve.postWrapMode = ConvertUtil.ConvertAnimationWrapMode(int.Parse(curveStr_n["_postInfinity"].ToString()));
            ElurKeyFrames elurKeyFrames = null;
            convertKeyFrame(bindPropertyName,keyFrameList, clip, animCurve, needFlip, needRotate180, elurKeyFrames);
            clip.SetCurve(path, componetType, bindPropertyName, animCurve); 
        }
    }

    static void convertKeyFrame(string bindPropertyName, List<object> keyFrames, AnimationClip clip, AnimationCurve curve, bool needFlip, bool needRotate180,ElurKeyFrames elurKeyFrames)
    {
        for (int n = 0; n < keyFrames.Count; n++)
        {
            var keyFrame_str = keyFrames[n].ToString();
            var keyFrame_Dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(keyFrame_str);
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
            if (needFlip)
                keyframe.value = -keyframe.value;
            if (needRotate180)
            {            
                keyframe.value = (keyframe.value + 180) % 360;
            }

            if (needFlip)
            {
                keyframe.inTangent = -keyframe.inTangent;
                keyframe.outTangent = -keyframe.outTangent;
            }
            curve.AddKey(keyframe);

            AnimationUtility.TangentMode leftTangent;
            AnimationUtility.TangentMode rightTangent;
            ConvertTangetMode(tangentMode, out leftTangent, out rightTangent, curve, n);
            AnimationUtility.SetKeyLeftTangentMode(curve, n, leftTangent);
            AnimationUtility.SetKeyRightTangentMode(curve, n, rightTangent);
            if (bindPropertyName.Equals("m_IsActive"))
            {
                AnimationUtility.SetKeyLeftTangentMode(curve, n, AnimationUtility.TangentMode.Constant);
                AnimationUtility.SetKeyRightTangentMode(curve, n, AnimationUtility.TangentMode.Constant);
            }

        }
    }

    public static void ConvertTangetMode(int tangentMode, out AnimationUtility.TangentMode leftTangent, out AnimationUtility.TangentMode rightTagent, AnimationCurve curve, int index)
    {

        Jnity_TangentMode mode = (Jnity_TangentMode)tangentMode;
        switch (mode)
        {
            case Jnity_TangentMode.Auto:
                leftTangent = rightTagent = AnimationUtility.TangentMode.Auto;
                break;
            case Jnity_TangentMode.ClampedAuto:
                leftTangent = AnimationUtility.TangentMode.Linear;
                rightTagent = AnimationUtility.TangentMode.ClampedAuto;
                break;
            case Jnity_TangentMode.Flat:
            case Jnity_TangentMode.FreeSmooth:
                leftTangent = rightTagent = AnimationUtility.TangentMode.Free;
                break;
            case Jnity_TangentMode.Free_Free:
                leftTangent = rightTagent = AnimationUtility.TangentMode.Free;
                AnimationUtility.SetKeyBroken(curve, index, true);
                break;
            case Jnity_TangentMode.Free_Linear:
                leftTangent = AnimationUtility.TangentMode.Free;
                rightTagent = AnimationUtility.TangentMode.Linear;
                AnimationUtility.SetKeyBroken(curve, index, true);
                break;
            case Jnity_TangentMode.Free_Constant:
                leftTangent = AnimationUtility.TangentMode.Free;
                rightTagent = AnimationUtility.TangentMode.Constant;
                AnimationUtility.SetKeyBroken(curve, index, true);
                break;
            case Jnity_TangentMode.Linear_Linear:
                leftTangent = rightTagent = AnimationUtility.TangentMode.Linear;
                AnimationUtility.SetKeyBroken(curve, index, true);
                break;
            case Jnity_TangentMode.Linear_Free:
                leftTangent = AnimationUtility.TangentMode.Linear;
                rightTagent = AnimationUtility.TangentMode.Free;
                AnimationUtility.SetKeyBroken(curve, index, true);
                break;
            case Jnity_TangentMode.Linear_Constant:
                leftTangent = AnimationUtility.TangentMode.Linear;
                rightTagent = AnimationUtility.TangentMode.Constant;
                AnimationUtility.SetKeyBroken(curve, index, true);
                break;
            case Jnity_TangentMode.Constant_Constant:
                leftTangent = rightTagent = AnimationUtility.TangentMode.Constant;
                AnimationUtility.SetKeyBroken(curve, index, true);
                break;
            case Jnity_TangentMode.Constant_Free:
                leftTangent = AnimationUtility.TangentMode.Constant;
                rightTagent = AnimationUtility.TangentMode.Free;
                AnimationUtility.SetKeyBroken(curve, index, true);
                break;
            case Jnity_TangentMode.Constant_Linear:
                leftTangent = AnimationUtility.TangentMode.Constant;
                rightTagent = AnimationUtility.TangentMode.Linear;
                AnimationUtility.SetKeyBroken(curve, index, true);
                break;
            default:
                Debug.LogError("Convert Tangent Mode error:" + tangentMode);
                leftTangent = rightTagent = AnimationUtility.TangentMode.ClampedAuto;
                break;
        }
    }


    static string AttributeConvertToBindingName(string attribute,string path,Transform animOwner, out bool needFlip, out bool needRotate180, out Type componentType)
    {
        needFlip = false;
        needRotate180 = false;
        componentType = typeof(Transform);
        if (attribute.StartsWith("Transform"))
        {
            attribute = AttributeConvert_Transform(attribute, path, animOwner, ref needFlip, ref needRotate180);
            componentType = typeof(Transform);
        }
        else if (attribute.Contains("Renderer.Material"))
        {
            if (attribute.Contains("Skinned Mesh Renderer"))
            {
                componentType = typeof(SkinnedMeshRenderer);
            }
            else if (attribute.Contains("Mesh Renderer"))
            {
                componentType = typeof(MeshRenderer);
            }
            else if (attribute.Contains("Trail Renderer"))
            {
                componentType = typeof(TrailRenderer);
            }
            attribute = AttributeConvert_Material(attribute);

        }
        else if (attribute.Trim().Equals("visible"))
        {
            attribute = "m_IsActive";
            componentType = typeof(GameObject);
        }
		else if (attribute.Trim().Equals("Camera.far_plane"))
		{
			attribute = "far clip plane";
			componentType = typeof(Camera);
		}
		else if (attribute.Trim().Equals("Camera.fov"))
        {
            attribute = "field of view";
            componentType = typeof(Camera);
        }else if (attribute.Trim().Contains("Particle System."))
        {
            attribute = attribute.Replace("Particle System.", "");
            if (attribute.Equals("Enabled"))
            {
                attribute = "m_Enabled";
            }
            componentType = typeof(ParticleSystem);
        }
        else if (attribute.Trim().Contains("Particle System Renderer."))
        {
            attribute = attribute.Replace("Particle System Renderer.", "");
            if (attribute.Equals("Enabled"))
            {
                attribute = "m_Enabled";
            }else if(attribute.Equals("length.scale"))
            {
                attribute = "m_LengthScale";
            }
            componentType = typeof(ParticleSystemRenderer);
        }
        return attribute;
    }
    
    static string AttributeConvert_Transform(string attribute, string path,Transform animOwner, ref bool needFlip, ref bool needRotate180)
    {
        Transform attributeTransform;
        if (!path.Trim().Equals(String.Empty))
        {
            attributeTransform = animOwner.transform.Find(path);
        }
        else
        {
            attributeTransform = animOwner;
        }
        bool hasCamera = false;
        if (attributeTransform)
        {
            hasCamera = ConvertUtil.hasComponent<Camera>(attributeTransform.gameObject);
        }

        switch (attribute)
        {
            case "Transform.position.x":
                return "m_LocalPosition.x";
            case "Transform.position.y":
                return "m_LocalPosition.y";
            case "Transform.position.z":
                    needFlip = true;
                return "m_LocalPosition.z";
            case "Transform.rotation.x":
                    needFlip = true;
                return "localEulerAnglesRaw.x";
            case "Transform.rotation.y":
                needFlip = true;
                return "localEulerAnglesRaw.y";
            case "Transform.rotation.z":
                needFlip = true; //for 粒子动画修改 particle_animation
                return "localEulerAnglesRaw.z";
            case "Transform.scale.x":
                return "m_LocalScale.x";
            case "Transform.scale.y":
                return "m_LocalScale.y";
            case "Transform.scale.z":
                return "m_LocalScale.z";
            default:
                return "unknow BindingName";

        }
    }

    static string[] Componet_Material_Names = { "Skinned Mesh Renderer.Material", "Mesh Renderer.Material", "Trail Renderer.Material" };
    static string AttributeConvert_Material(string attribute)
    {
        if (attribute.ToLower().Contains("color"))
        {
            attribute = attribute.Replace(".x", ".r");
            attribute = attribute.Replace(".y", ".g");
            attribute = attribute.Replace(".z", ".b");
            attribute = attribute.Replace(".w", ".a");
        }
        for (int i = 0; i < Componet_Material_Names.Length; i++)
        {
            attribute = attribute.Replace(Componet_Material_Names[i], "");
        }

        attribute = "material" + attribute;
        return attribute;
    }



    public static AnimationClip ConvertAnimationClip(List<object> jsonStr, Transform animOwner)
    {
        AnimationClip newClip = new AnimationClip();
        for (int n = 0; n < jsonStr.Count; n++)
        {
            string object_s = (string)jsonStr[n].ToString();
            var jsonStr_n = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(object_s);
            var localId = jsonStr_n["localId"].ToString();
            var typeId = jsonStr_n["typeId"].ToString();
            if (jsonStr_n.ContainsKey("AnimationClip"))
            {
                var AnimationClip_Dict_str = jsonStr_n["AnimationClip"].ToString();
                var AnimationClip_Dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(AnimationClip_Dict_str);
                // newClip.frameRate = AnimationClip_Dict["SampleCount"].ToString() ;
                newClip.frameRate = float.Parse(AnimationClip_Dict["_sampleCount"].ToString());
                newClip.name = AnimationClip_Dict["_name"].ToString();
                AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(newClip);
                bool loop = bool.Parse(AnimationClip_Dict["_loop"].ToString());             
                newClip.wrapMode = loop? WrapMode.Loop : WrapMode.Once;
                AnimationUtility.SetAnimationClipSettings(newClip, clipSetting);

                var bezierTrackArray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(AnimationClip_Dict["_bezierTrackFloat"].ToString());
                ConvertBezierTrack(bezierTrackArray, newClip, animOwner);
                var animationEventArray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(AnimationClip_Dict["_events"].ToString());
                ConvertAnimationEvent(animationEventArray, newClip);
            }
        }

        return newClip;
    }

    static void ConvertAnimationEvent(List<object> animationEventArray, AnimationClip newClip)
    {
        AnimationEvent[] events = AnimationUtility.GetAnimationEvents(newClip);
        for (int n = 0; n < animationEventArray.Count; n++)
        {
            var animationEvent_Dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(animationEventArray[n].ToString());
            AnimationEvent animEvent = new AnimationEvent();
            animEvent.time = float.Parse(animationEvent_Dict["_time"].ToString());
            animEvent.floatParameter = float.Parse(animationEvent_Dict["_floatParameter"].ToString());
            animEvent.intParameter = int.Parse(animationEvent_Dict["_intParameter"].ToString());
            animEvent.stringParameter = animationEvent_Dict["_stringParameter"].ToString();
            animEvent.functionName = animationEvent_Dict["_functionName"].ToString();
            events = events.Concat(new[] { animEvent }).ToArray();
        }
        AnimationUtility.SetAnimationEvents(newClip, events);
    }


    public static AnimationClip ConvertAnimatorClip(List<object> jsonStr, Transform animOwner)
    {
        AnimationClip newClip = new AnimationClip();
        for (int n = 0; n < jsonStr.Count; n++)
        {
            string object_s = (string)jsonStr[n].ToString();
            var jsonStr_n = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(object_s);
            var localId = jsonStr_n["localId"].ToString();
            var typeId = jsonStr_n["typeId"].ToString();
            if (jsonStr_n.ContainsKey("AnimatorClip"))
            {
                var AnimatorClip_Dict_str = jsonStr_n["AnimatorClip"].ToString();
                var AnimatorClip_Dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(AnimatorClip_Dict_str);
                // newClip.frameRate = AnimationClip_Dict["SampleCount"].ToString() ;

                newClip.name = AnimatorClip_Dict["_name"].ToString();
                var AnimatorClip_settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(AnimatorClip_Dict["_settings"].ToString());
                AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(newClip);
                newClip.frameRate = float.Parse(AnimatorClip_settings["sampleRate"].ToString());
                clipSetting.loopTime = bool.Parse(AnimatorClip_settings["loopTime"].ToString());
                newClip.wrapMode = clipSetting.loopTime ? WrapMode.Loop : WrapMode.Once;
                clipSetting.startTime = float.Parse(AnimatorClip_settings["startTime"].ToString());
                clipSetting.stopTime = float.Parse(AnimatorClip_settings["stopTime"].ToString());
                clipSetting.level = float.Parse(AnimatorClip_settings["level"].ToString());
                clipSetting.cycleOffset = float.Parse(AnimatorClip_settings["cycleOffset"].ToString());
                clipSetting.loopBlend = bool.Parse(AnimatorClip_settings["loopBlend"].ToString());
                clipSetting.loopBlendOrientation = bool.Parse(AnimatorClip_settings["loopBlendOrient"].ToString());
                clipSetting.loopBlendPositionY = bool.Parse(AnimatorClip_settings["loopBlendPosY"].ToString());
                clipSetting.loopBlendPositionXZ = bool.Parse(AnimatorClip_settings["loopBlendPosXZ"].ToString());
                clipSetting.keepOriginalOrientation = bool.Parse(AnimatorClip_settings["keepOriginOrient"].ToString());
                clipSetting.keepOriginalPositionY = bool.Parse(AnimatorClip_settings["keepOriginPosY"].ToString());
                clipSetting.keepOriginalPositionXZ = bool.Parse(AnimatorClip_settings["keepOriginPosXZ"].ToString());
                clipSetting.heightFromFeet = bool.Parse(AnimatorClip_settings["heightFromFeet"].ToString());
                clipSetting.mirror = bool.Parse(AnimatorClip_settings["mirror"].ToString());

                AnimationUtility.SetAnimationClipSettings(newClip, clipSetting);
                var KeyFrameClip_Dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<object, object>>(AnimatorClip_Dict["_keyframeClip"].ToString());
                var bezierTrackArray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(KeyFrameClip_Dict["_floatTracks"].ToString());
                ConvertBezierTrack(bezierTrackArray, newClip, animOwner);
                var animationEventArray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(AnimatorClip_Dict["_events"].ToString());
                ConvertAnimationEvent(animationEventArray, newClip);
            }
        }

        return newClip;
    }
}
