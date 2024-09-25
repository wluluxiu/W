namespace jj.TATools.Editor
{
    using UnityEngine.Rendering;
    using UnityEditor;
    using UnityEngine;

    internal class URPSetting
    {
        internal static int GetCascadeCountFromURPSetting(Object pipelineObj)
        {
            var setting = pipelineObj == null ? GraphicsSettings.currentRenderPipeline : pipelineObj;
            if (setting == null) return -1;

            SerializedObject so = new SerializedObject(setting);
            var shadowCascadeCountProp = so.FindProperty("m_ShadowCascadeCount");

            return shadowCascadeCountProp.intValue;
        }

        internal static bool MainLightIsDisabledFromURPSetting(Object pipelineObj)
        {
            var setting = pipelineObj == null ? GraphicsSettings.currentRenderPipeline : pipelineObj;
            if (setting == null) return false;

            SerializedObject so = new SerializedObject(setting);
            var mainLightRenderingModeProp = so.FindProperty("m_MainLightRenderingMode");

            // 0-Disabled 1-Pixel
            return mainLightRenderingModeProp.intValue == 0;
        }

        internal static bool MainLightShadowsSupportedFromURPSetting(Object pipelineObj)
        {
            var setting = pipelineObj == null ? GraphicsSettings.currentRenderPipeline : pipelineObj;
            if (setting == null) return false;

            SerializedObject so = new SerializedObject(setting);
            var mainLightShadowsSupportedProp = so.FindProperty("m_MainLightShadowsSupported");

            // 1-Supported
            return mainLightShadowsSupportedProp.intValue == 1;
        }

        /// <summary>
        /// 0-Disabled
        /// 2-PerVertex
        /// 1-PerPixel
        /// </summary>
        /// <returns></returns>
        internal static int GetAdditionalLightModeFromURPSetting(Object pipelineObj)
        {
            var setting = pipelineObj == null ? GraphicsSettings.currentRenderPipeline : pipelineObj;
            if (setting == null) return 0;

            SerializedObject so = new SerializedObject(setting);
            var additionalLightsRenderingModeProp = so.FindProperty("m_AdditionalLightsRenderingMode");

            return additionalLightsRenderingModeProp.intValue;
        }

        internal static bool AdditionalLightShadowsSupportedFromURPSetting(Object pipelineObj)
        {
            var setting = pipelineObj == null ? GraphicsSettings.currentRenderPipeline : pipelineObj;
            if (setting == null) return false;

            SerializedObject so = new SerializedObject(setting);
            var additionalLightShadowsSupportedProp = so.FindProperty("m_AdditionalLightShadowsSupported");

            // 1-Supported
            return additionalLightShadowsSupportedProp.intValue == 1;
        }

        /// <summary>
        /// 0 - Auto
        /// 1 - PerVertex
        /// 2 - PerMixed
        /// 3 - PerPixel
        /// </summary>
        /// <returns></returns>
        internal static int GetSHEvaluationModeFromURPSetting(Object pipelineObj)
        {
            var setting = pipelineObj == null ? GraphicsSettings.currentRenderPipeline : pipelineObj;
            if (setting == null) return -1;

            SerializedObject so = new SerializedObject(setting);
            var shEvalModeProp = so.FindProperty("m_ShEvalMode");

            return shEvalModeProp.intValue;
        }

        internal static float GetShadowDistanceFromURPSetting(Object pipelineObj)
        {
            var setting = pipelineObj == null ? GraphicsSettings.currentRenderPipeline : pipelineObj;
            if (setting == null) return 0.0f;

            SerializedObject so = new SerializedObject(setting);
            var shadowDistanceProp = so.FindProperty("m_ShadowDistance");

            return shadowDistanceProp.floatValue;
        }

        internal static bool SoftShadowSupportedFromURPSetting(Object pipelineObj)
        {
            var setting = pipelineObj == null ? GraphicsSettings.currentRenderPipeline : pipelineObj;
            if (setting == null) return false;

            SerializedObject so = new SerializedObject(setting);
            var softShadowsSupportedProp = so.FindProperty("m_SoftShadowsSupported");

            return softShadowsSupportedProp.boolValue;
        }

        internal static bool EnabledReflectionProbeBlendingFromURPSetting(Object pipelineObj)
        {
            var setting = pipelineObj == null ? GraphicsSettings.currentRenderPipeline : pipelineObj;
            if (setting == null) return false;

            SerializedObject so = new SerializedObject(setting);
            var reflectionProbeBlendingProp = so.FindProperty("m_ReflectionProbeBlending");

            return reflectionProbeBlendingProp.boolValue;
        }

        internal static bool EnabledReflectionProbeBoxProjectionFromURPSetting(Object pipelineObj)
        {
            var setting = pipelineObj == null ? GraphicsSettings.currentRenderPipeline : pipelineObj;
            if (setting == null) return false;

            SerializedObject so = new SerializedObject(setting);
            var reflectionProbeBoxProjectionProp = so.FindProperty("m_ReflectionProbeBoxProjection");

            return reflectionProbeBoxProjectionProp.boolValue;
        }
    }
}