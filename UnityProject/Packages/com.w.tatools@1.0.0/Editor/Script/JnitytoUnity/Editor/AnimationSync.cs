using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSync 
{

    public static AnimationClip ConvertAnimation(string jnity_clip_path, Transform animOwner)
    {
        string text = FileUtil.ReadTexTFile(jnity_clip_path);
        var jsonStr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(text);
        AnimationClip animationClip;
        if (jnity_clip_path.ToLower().EndsWith(".clip"))
        {
            animationClip = AnimationConvert.ConvertAnimationClip(jsonStr, animOwner);
        }
        else
        {
            animationClip = AnimationConvert.ConvertAnimatorClip(jsonStr, animOwner);
        }

        return animationClip;
    }
}
