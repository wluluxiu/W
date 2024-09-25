namespace jj.TATools.Editor
{
    using System.Collections.Generic;

    internal enum EDetectionType
    { 
        Normal = 0,
        AnimatorController,
        AnimatorOverrideController,
        Asmdef,
        Material,
        Prefab,
        Scene,
        Script,
        Shader,
        SpriteAtlas,
        TMP_SDF,
        TMP_Setting,
        TMP_Sprite,
    }

    internal class BaseDetection
    {
        #region Fields

        internal EDetectionType m_Type;
        internal string m_AssetPath;

        internal List<string> m_LossGuids;
        internal List<string> m_LossGuidPropNames;

        #endregion

        #region Virtual Methods

        internal virtual void DoDetection(string assetPath) 
        {
            this.m_AssetPath = assetPath;
        }

        internal virtual bool IsInvalid()
        {
            return true;
        }

        #endregion

        #region Static Methods

        internal static BaseDetection CreateDetection(EDetectionType assetType)
        {
            BaseDetection baseDetection = null;

            if (assetType == EDetectionType.AnimatorController) baseDetection = new AnimatorControllerDetection();
            else if (assetType == EDetectionType.AnimatorOverrideController) baseDetection = new AnimatorOverrideControllerDetection();
            else if (assetType == EDetectionType.Asmdef) baseDetection = new AsmdefDetection();
            else if (assetType == EDetectionType.Material) baseDetection = new MaterialDetection();
            else if (assetType == EDetectionType.Prefab) baseDetection = new PrefabDetection();
            else if (assetType == EDetectionType.Scene) baseDetection = new SceneDetection();
            else if (assetType == EDetectionType.Script) baseDetection = new ScriptDetection();
            else if (assetType == EDetectionType.Shader) baseDetection = new ShaderDetection();
            else if (assetType == EDetectionType.SpriteAtlas) baseDetection = new SpriteAtlasDetection();
            else if (assetType == EDetectionType.TMP_SDF) baseDetection = new TMPSdfDetection();
            else if (assetType == EDetectionType.TMP_Setting) baseDetection = new TMPSettingDetection();
            else if (assetType == EDetectionType.TMP_Sprite) baseDetection = new TMPSpriteDetection();
            else baseDetection = new NormalDetection();

            baseDetection.m_Type = assetType;

            return baseDetection;
        }

        #endregion
    }
}