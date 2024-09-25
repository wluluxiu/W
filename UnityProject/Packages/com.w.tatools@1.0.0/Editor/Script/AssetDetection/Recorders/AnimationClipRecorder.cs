using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    //
    // 摘要:
    //     Determines how time is treated outside of the keyframed range of an AnimationClip
    //     or AnimationCurve.
    internal enum EAnimWrapMode
    {
        //
        // 摘要:
        //     Reads the default repeat mode set higher up.
        Default = 0,
        //
        // 摘要:
        //     When time reaches the end of the animation clip, the clip will automatically
        //     stop playing and time will be reset to beginning of the clip.
        Once = 1,
        Clamp = 1,
        //
        // 摘要:
        //     When time reaches the end of the animation clip, time will continue at the beginning.
        Loop = 2,
        //
        // 摘要:
        //     When time reaches the end of the animation clip, time will ping pong back between
        //     beginning and end.
        PingPong = 4,
        //
        // 摘要:
        //     Plays back the animation. When it reaches the end, it will keep playing the last
        //     frame and never stop playing.
        ClampForever = 8
    }

    /// <summary>
    /// 检测项目：
    /// 1.Anim Type
    /// 2.Length
    /// 3.WrapMode
    /// 4.LoopTime
    /// </summary>
    internal class AnimationClipRecorder : BaseRecorder
    {
        #region Fields

        internal int m_Legacy;
        internal float m_Length;
        // Legacy
        internal int m_WrapMode;
        // New
        internal int m_LoopTime;

        #endregion

        #region Override Methods

        internal override void Record(string assetPath, EAssetType assetType)
        {
            base.Record(assetPath, assetType);

            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);

            this.m_Legacy = animationClip.legacy ? 1 : 0;
            this.m_Length = animationClip.length;
            this.m_WrapMode = (int)animationClip.wrapMode;
            this.m_LoopTime = animationClip.isLooping ? 1 : 0;

            animationClip = null;
        }

        /// <summary>
        /// BaseFormat|Legacy|Length|WrapMode|Loop
        /// </summary>
        /// <returns></returns>
        internal override string GetOutputStr()
        {
            var baseOutputStr = base.GetOutputStr();

            string spiltStr = CHAR_SPLIT_FIRST_FLAG.ToString();

            return baseOutputStr + spiltStr +
                   this.m_Legacy + spiltStr +
                   this.m_Length + spiltStr +
                   this.m_WrapMode + spiltStr +
                   this.m_LoopTime;
        }

        internal override void ParseStrLine(string stringLine)
        {
            base.ParseStrLine(stringLine);

            if (base.m_SourceDataArr.Length > 7 && !string.IsNullOrEmpty(base.m_SourceDataArr[7]))
            {
                this.m_Legacy = int.Parse(base.m_SourceDataArr[7]);
            }

            if (base.m_SourceDataArr.Length > 8 && !string.IsNullOrEmpty(base.m_SourceDataArr[8]))
            {
                this.m_Length = float.Parse(base.m_SourceDataArr[8]);
            }

            if (base.m_SourceDataArr.Length > 9 && !string.IsNullOrEmpty(base.m_SourceDataArr[9]))
            {
                this.m_WrapMode = int.Parse(base.m_SourceDataArr[9]);
            }

            if (base.m_SourceDataArr.Length > 10 && !string.IsNullOrEmpty(base.m_SourceDataArr[10]))
            {
                this.m_LoopTime = int.Parse(base.m_SourceDataArr[10]);
            }
        }

        #endregion
    }
}
