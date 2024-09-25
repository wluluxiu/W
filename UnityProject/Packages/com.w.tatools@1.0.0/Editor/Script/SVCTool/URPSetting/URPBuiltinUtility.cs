namespace jj.TATools.Editor
{
    /**************************************************************************** 
     * 
     *  URP Verison : 14.0.11
     * 
     ****************************************************************************/

    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    using UnityEngineObj = UnityEngine.Object;
    using UnityEngine.Rendering;
    using UnityEngine.SceneManagement;
    using UnityEditor.SceneManagement;

    class VolumeProfileParser
    {
        #region Fields

        const string COMPONENTS_PROP_NAME = "components";

        const string BLOOM_TYPE = "UnityEngine.Rendering.Universal.Bloom";
        const string TONEMAPPING_TYPE = "UnityEngine.Rendering.Universal.Tonemapping";
        const string DEPTHOFFIELD_TYPE = "UnityEngine.Rendering.Universal.DepthOfField";
        const string PANINI_PROJECTION_TYPE = "UnityEngine.Rendering.Universal.PaniniProjection";
        const string LENS_DISTORTION_TYPE = "UnityEngine.Rendering.Universal.LensDistortion";
        const string CHROMATIC_ABERRATION_TYPE = "UnityEngine.Rendering.Universal.ChromaticAberration";
        const string FILM_GRAIN_TYPE = "UnityEngine.Rendering.Universal.FilmGrain";

        private string m_VolumeProfilePath;
        private UnityEngineObj m_VolumeProfileObj;
        private Dictionary<string, object> m_ComponentsMapping = new Dictionary<string, object>();// Component Type Name is Key

        #endregion

        VolumeProfileParser(string profilePath)
        {
            m_VolumeProfilePath = profilePath;
            var profileObj = AssetDatabase.LoadAssetAtPath<UnityEngineObj>(m_VolumeProfilePath);

            var profileObjSO = new SerializedObject(profileObj);
            var componentsProp = profileObjSO.FindProperty(COMPONENTS_PROP_NAME);
            for (int i = 0; i < componentsProp.arraySize; i++)
            {
                var compProp = componentsProp.GetArrayElementAtIndex(i);
                var comp = compProp.boxedValue;
                m_ComponentsMapping[comp.GetType().FullName] = comp;
            }
        }

        #region Static Methods

        internal static VolumeProfileParser GetVolumeProfile(string profilePath)
        {
            if (!File.Exists(profilePath)) return null;

            return new VolumeProfileParser(profilePath);
        }

        #endregion

        #region Tonemapping

        internal List<string> GetTonemappingLutBuilderHdrKeywords(bool hdr,out int mode)
        {
            mode = -1;
            List<string> totalKeywords = new List<string>();

            object tonemappingComp = null;
            if (m_ComponentsMapping.TryGetValue(TONEMAPPING_TYPE, out tonemappingComp))
            {
                var tonemappingCompType = tonemappingComp.GetType();
                var activeField = tonemappingCompType.GetField("active");
                var activeFieldValue = (bool)activeField.GetValue(tonemappingComp);
                if (!activeFieldValue)
                {
                    totalKeywords.Add("");
                    totalKeywords.Add("HDR_COLORSPACE_CONVERSION");
                }
                else
                {
                    var modeField = tonemappingCompType.GetField("mode");
                    var modeFieldValue = modeField.GetValue(tonemappingComp);
                    var modeFieldValueType = modeFieldValue.GetType();
                    var valueField = modeFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var valueFieldValue = (int)valueField.GetValue(modeFieldValue); // 0-None,1-Neutral,2-ACES
                    var overrideStateField = modeFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var overrideStateFieldValue = (bool)overrideStateField.GetValue(modeFieldValue);
                    if (overrideStateFieldValue)
                    {
                        mode = valueFieldValue;

                        // None
                        if (valueFieldValue == 0)
                        {
                            totalKeywords.Add("");
                            totalKeywords.Add("HDR_COLORSPACE_CONVERSION");
                        }
                        // Neutral
                        else if (valueFieldValue == 1)
                        {
                            totalKeywords.Add("_TONEMAP_NEUTRAL");
                            totalKeywords.Add("HDR_COLORSPACE_CONVERSION|_TONEMAP_NEUTRAL");
                        }
                        // ACES
                        else if (valueFieldValue == 2)
                        {
                            /* 运行时： SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 && Graphics.minOpenGLESVersion <= OpenGLESVersion.OpenGLES30 && SystemInfo.graphicsDeviceName.StartsWith("Adreno (TM) 3")
                             * _TONEMAP_ACES -> _TONEMAP_NEUTRAL
                             */
                            totalKeywords.Add("_TONEMAP_NEUTRAL");
                            totalKeywords.Add("_TONEMAP_ACES");
                            totalKeywords.Add("HDR_COLORSPACE_CONVERSION|_TONEMAP_NEUTRAL");
                            totalKeywords.Add("HDR_COLORSPACE_CONVERSION|_TONEMAP_ACES");
                        }
                    }
                    else
                    {
                        mode = 0;
                        totalKeywords.Add("");// Mode为False 默认值为None
                        totalKeywords.Add("HDR_COLORSPACE_CONVERSION");
                    }
                }
            }

            if (!hdr)
            {
                totalKeywords.Clear();
            }

            return totalKeywords;
        }

        #endregion

        #region Bloom

        internal List<string> GetBloomKeywords(out int highQualityFiltering,out float dirtIntensity)
        {
            highQualityFiltering = -1;
            dirtIntensity = 0;

            List<string> totalKeywords = new List<string>();

            object bloomComp = null;
            if (m_ComponentsMapping.TryGetValue(BLOOM_TYPE, out bloomComp))
            {
                var bloomCompType = bloomComp.GetType();
                var activeField = bloomCompType.GetField("active");
                var activeFieldValue = (bool)activeField.GetValue(bloomComp);
                if (!activeFieldValue)
                {
                    totalKeywords.Add("");
                }
                else
                {
                    var highQualityFilteringField = bloomCompType.GetField("highQualityFiltering");
                    var highQualityFilteringFieldValue = highQualityFilteringField.GetValue(bloomComp);
                    var highQualityFilteringFieldValueType = highQualityFilteringFieldValue.GetType();
                    var valueField = highQualityFilteringFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var valueFieldValue = (bool)valueField.GetValue(highQualityFilteringFieldValue);
                    var overrideStateField = highQualityFilteringFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var overrideStateFieldValue = (bool)overrideStateField.GetValue(highQualityFilteringFieldValue);
                    if (overrideStateFieldValue)
                    {
                        highQualityFiltering = valueFieldValue ? 1 : 0;

                        if (valueFieldValue)
                        {
                            totalKeywords.Add("_BLOOM_HQ");
                            totalKeywords.Add("_BLOOM_HQ|_USE_RGBM");
                        }
                        else
                        {
                            totalKeywords.Add("");
                            totalKeywords.Add("_USE_RGBM");
                        }
                    }
                    else
                    {
                        highQualityFiltering = 0;
                        totalKeywords.Add("");// Mode为False 默认值为False
                        totalKeywords.Add("_USE_RGBM");
                    }

                    var dirtIntensityField = bloomCompType.GetField("dirtIntensity");
                    var dirtIntensityFieldValue = dirtIntensityField.GetValue(bloomComp);
                    var dirtIntensityFieldValueType = dirtIntensityFieldValue.GetType();
                    var valueField1 = dirtIntensityFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var valueFieldValue1 = (float)valueField1.GetValue(dirtIntensityFieldValue);
                    var overrideStateField1 = dirtIntensityFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var overrideStateFieldValue1 = (bool)overrideStateField.GetValue(dirtIntensityFieldValue);
                    if (overrideStateFieldValue1)
                    {
                        dirtIntensity = valueFieldValue1;
                    }
                    else
                    {
                        valueFieldValue1 = 0;
                    }
                }
            }

            return totalKeywords;
        }

        #endregion

        #region Gaussian DOF

        internal List<string> GetGaussianDofKeywords()
        {
            List<string> totalKeywords = new List<string>();

            object dofComp = null;
            if (m_ComponentsMapping.TryGetValue(DEPTHOFFIELD_TYPE, out dofComp))
            {
                var dofCompType = dofComp.GetType();
                var activeField = dofCompType.GetField("active");
                var activeFieldValue = (bool)activeField.GetValue(dofComp);
                if (!activeFieldValue)
                {
                    totalKeywords.Add("");
                }
                else
                {
                    var modeField = dofCompType.GetField("mode");
                    var modeFieldValue = modeField.GetValue(dofComp);
                    var modeFieldValueType = modeFieldValue.GetType();
                    var valueField = modeFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var valueFieldValue = (int)valueField.GetValue(modeFieldValue); // 0-Off,1-Gaussian,2-Bokeh
                    var overrideStateField = modeFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var overrideStateFieldValue = (bool)overrideStateField.GetValue(modeFieldValue);
                    if (overrideStateFieldValue)
                    {
                        if (valueFieldValue == 1)
                        {
                            var highQualitySamplingField = dofCompType.GetField("highQualitySampling");
                            var highQualitySamplingFieldValue = highQualitySamplingField.GetValue(dofComp);
                            var highQualitySamplingFieldValueType = highQualitySamplingFieldValue.GetType();
                            var valueField1 = highQualitySamplingFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                            var valueFieldValue1 = (bool)valueField1.GetValue(highQualitySamplingFieldValue);
                            var overrideStateField1 = highQualitySamplingFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                            var overrideStateFieldValue1 = (bool)overrideStateField.GetValue(highQualitySamplingFieldValue);
                            if (overrideStateFieldValue1)
                            {
                                if (valueFieldValue1)
                                    totalKeywords.Add("_HIGH_QUALITY_SAMPLING");
                            }
                            else
                            {
                                //Mode为False 默认值为False
                                totalKeywords.Add("");
                            }
                        }
                        else
                        {
                            totalKeywords.Add("");
                        }
                    }
                    else
                    {
                        //Mode为False 默认值为Off
                        totalKeywords.Add(""); 
                    }
                }
            }

            return totalKeywords;
        }

        #endregion

        #region Bokeh DOF

        internal List<string> GetBokehDofKeywords(bool useFastSRGBLinearConversion)
        {
            List<string> totalKeywords = new List<string>();

            object dofComp = null;
            if (m_ComponentsMapping.TryGetValue(DEPTHOFFIELD_TYPE, out dofComp))
            {
                var dofCompType = dofComp.GetType();
                var activeField = dofCompType.GetField("active");
                var activeFieldValue = (bool)activeField.GetValue(dofComp);
                if (!activeFieldValue)
                {
                    totalKeywords.Add("");
                }
                else
                {
                    var modeField = dofCompType.GetField("mode");
                    var modeFieldValue = modeField.GetValue(dofComp);
                    var modeFieldValueType = modeFieldValue.GetType();
                    var valueField = modeFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var valueFieldValue = (int)valueField.GetValue(modeFieldValue); // 0-Off,1-Gaussian,2-Bokeh
                    var overrideStateField = modeFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var overrideStateFieldValue = (bool)overrideStateField.GetValue(modeFieldValue);
                    if (overrideStateFieldValue)
                    {
                        if (valueFieldValue == 2)
                        {
                            if(useFastSRGBLinearConversion)
                                totalKeywords.Add("_USE_FAST_SRGB_LINEAR_CONVERSION");
                        }
                        else
                        {
                            totalKeywords.Add("");
                        }
                    }
                    else
                    {
                        //Mode为False 默认值为Off
                        totalKeywords.Add("");
                    }
                }
            }

            return totalKeywords;
        }

        #endregion

        #region PaniniProjection

        internal List<string> GetPaniniProjectionKeywords()
        {
            List<string> totalKeywords = new List<string>();

            object paniniProjectionComp = null;
            if (m_ComponentsMapping.TryGetValue(PANINI_PROJECTION_TYPE, out paniniProjectionComp))
            {
                var paniniProjectionCompType = paniniProjectionComp.GetType();
                var activeField = paniniProjectionCompType.GetField("active");
                var activeFieldValue = (bool)activeField.GetValue(paniniProjectionComp);
                if (!activeFieldValue)
                {
                    totalKeywords.Add("_GENERIC");
                }
                else
                {
                    var distanceField = paniniProjectionCompType.GetField("distance");
                    var distanceFieldValue = distanceField.GetValue(paniniProjectionComp);
                    var distanceFieldValueType = distanceFieldValue.GetType();
                    var valueField = distanceFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var valueFieldValue = (float)valueField.GetValue(distanceFieldValue);
                    var overrideStateField = distanceFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var overrideStateFieldValue = (bool)overrideStateField.GetValue(distanceFieldValue);
                    var finalDistance = 0.0f;
                    if (overrideStateFieldValue)
                    {
                        finalDistance = valueFieldValue;
                    }

                    if ((1f - Mathf.Abs(finalDistance)) > float.Epsilon)
                        totalKeywords.Add("_GENERIC");
                    else
                        totalKeywords.Add("_UNIT_DISTANCE");
                }
            }
            else
            {
                totalKeywords.Add("_GENERIC");
            }

            return totalKeywords;
        }

        #endregion

        #region LensDistortion

        internal bool GetLensDistortionActive()
        {
            object lensDistortionComp = null;
            if (m_ComponentsMapping.TryGetValue(LENS_DISTORTION_TYPE, out lensDistortionComp))
            {
                var lensDistortionCompType = lensDistortionComp.GetType();
                var activeField = lensDistortionCompType.GetField("active");
                var activeFieldValue = (bool)activeField.GetValue(lensDistortionComp);
                if (!activeFieldValue)
                {
                    return false;
                }
                else
                {
                    // intensity
                    var intensityField = lensDistortionCompType.GetField("intensity");
                    var intensityFieldValue = intensityField.GetValue(lensDistortionComp);
                    var intensityFieldValueType = intensityFieldValue.GetType();
                    var intensityValueField = intensityFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var intensity = (float)intensityValueField.GetValue(intensityFieldValue);
                    var intensityOverrideStateField = intensityFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var intensityOverride = (bool)intensityOverrideStateField.GetValue(intensityFieldValue);
                    if (!intensityOverride)
                        intensity = 0f;

                    // xMultiplier
                    var xMultiplierField = lensDistortionCompType.GetField("xMultiplier");
                    var xMultiplierFieldValue = xMultiplierField.GetValue(lensDistortionComp);
                    var xMultiplierFieldValueType = xMultiplierFieldValue.GetType();
                    var xMultiplierValueField = xMultiplierFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var xMultiplier = (float)xMultiplierValueField.GetValue(xMultiplierFieldValue);
                    var xMultiplierOverrideStateField = xMultiplierFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var xMultiplierOverride = (bool)xMultiplierOverrideStateField.GetValue(xMultiplierFieldValue);
                    if (!xMultiplierOverride)
                        xMultiplier = 1f;

                    // yMultiplier
                    var yMultiplierField = lensDistortionCompType.GetField("yMultiplier");
                    var yMultiplierFieldValue = yMultiplierField.GetValue(lensDistortionComp);
                    var yMultiplierFieldValueType = yMultiplierFieldValue.GetType();
                    var yMultiplierValueField = yMultiplierFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var yMultiplier = (float)yMultiplierValueField.GetValue(yMultiplierFieldValue);
                    var yMultiplierOverrideStateField = yMultiplierFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var yMultiplierOverride = (bool)yMultiplierOverrideStateField.GetValue(yMultiplierFieldValue);
                    if (!yMultiplierOverride)
                        yMultiplier = 1f;

                    if(Mathf.Abs(intensity) > 0 && (xMultiplier > 0f || yMultiplier > 0f))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region ChromaticAberration

        internal bool GetChromaticAberrationActive()
        {
            object chromaticAberrationComp = null;
            if (m_ComponentsMapping.TryGetValue(CHROMATIC_ABERRATION_TYPE, out chromaticAberrationComp))
            {
                var chromaticAberrationCompType = chromaticAberrationComp.GetType();
                var activeField = chromaticAberrationCompType.GetField("active");
                var activeFieldValue = (bool)activeField.GetValue(chromaticAberrationComp);
                if (!activeFieldValue)
                {
                    return false;
                }
                else
                {
                    // intensity
                    var intensityField = chromaticAberrationCompType.GetField("intensity");
                    var intensityFieldValue = intensityField.GetValue(chromaticAberrationComp);
                    var intensityFieldValueType = intensityFieldValue.GetType();
                    var intensityValueField = intensityFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var intensity = (float)intensityValueField.GetValue(intensityFieldValue);
                    var intensityOverrideStateField = intensityFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var intensityOverride = (bool)intensityOverrideStateField.GetValue(intensityFieldValue);
                    if (!intensityOverride)
                        intensity = 0f;

                    if (intensity > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region FilmGrain

        internal bool GetFilmGrainActive()
        {
            object filmGrainComp = null;
            if (m_ComponentsMapping.TryGetValue(FILM_GRAIN_TYPE, out filmGrainComp))
            {
                var filmGrainCompType = filmGrainComp.GetType();
                var activeField = filmGrainCompType.GetField("active");
                var activeFieldValue = (bool)activeField.GetValue(filmGrainComp);
                if (!activeFieldValue)
                {
                    return false;
                }
                else
                {
                    // intensity
                    var intensityField = filmGrainCompType.GetField("intensity");
                    var intensityFieldValue = intensityField.GetValue(filmGrainComp);
                    var intensityFieldValueType = intensityFieldValue.GetType();
                    var intensityValueField = intensityFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var intensity = (float)intensityValueField.GetValue(intensityFieldValue);
                    var intensityOverrideStateField = intensityFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var intensityOverride = (bool)intensityOverrideStateField.GetValue(intensityFieldValue);
                    if (!intensityOverride)
                        intensity = 0f;

                    // type
                    var typeField = filmGrainCompType.GetField("type");
                    var typeFieldValue = typeField.GetValue(filmGrainComp);
                    var typeFieldValueType = typeFieldValue.GetType();
                    var typeValueField = typeFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var type = (int)typeValueField.GetValue(typeFieldValue); //0-Thin1,1-Thin2,2-Medium1,3-Medium2,4-Medium3,5-Medium4,6-Medium5,7-Medium6,8-Large01,9-Large02,10-Custom
                    var typeOverrideStateField = typeFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var typeOverride = (bool)typeOverrideStateField.GetValue(typeFieldValue);
                    if (!typeOverride)
                        type = 0;

                    // texture
                    var textureField = filmGrainCompType.GetField("texture");
                    var textureFieldValue = textureField.GetValue(filmGrainComp);
                    var textureFieldValueType = textureFieldValue.GetType();
                    var textureValueField = textureFieldValueType.GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var texture = (Texture)textureValueField.GetValue(textureFieldValue);
                    var textureOverrideStateField = textureFieldValueType.GetField("m_OverrideState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var textureOverride = (bool)textureOverrideStateField.GetValue(textureFieldValue);
                    if (!textureOverride)
                        texture = null;

                    if (intensity > 0f && (type != 10 || texture != null))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }

    internal class URPBuiltinUtility
    {
        #region Const Fields

        const string URP_SHADER_CAMERA_MOTION_VECTORS = "Hidden/Universal Render Pipeline/CameraMotionVectors";
        const string URP_SHADER_DEBUG_REPLACEMENT = "Hidden/Universal Render Pipeline/Debug/DebugReplacement";
        const string URP_SHADER_HDR_DEBUG_VIEW = "Hidden/Universal Render Pipeline/Debug/HDRDebugView";
        const string URP_SHADER_OBJ_MOTION_VECTORS = "Hidden/Universal Render Pipeline/ObjectMotionVectors";
        const string URP_SHADER_BLIT = "Hidden/Universal Render Pipeline/Blit";
        const string URP_SHADER_BLIT_HDR_OVERLAY = "Hidden/Universal/BlitHDROverlay";
        const string URP_SHADER_COPY_DEPTH = "Hidden/Universal Render Pipeline/CopyDepth";
        const string URP_SHADER_CORE_BLIT = "Hidden/Universal/CoreBlit";
        const string URP_SHADER_CORE_BLIT_COLOR_AND_DEPTH = "Hidden/Universal/CoreBlitColorAndDepth";
        const string URP_SHADER_FALLBACK_ERROR = "Hidden/Universal Render Pipeline/FallbackError";
        const string URP_SHADER_FALLBACK_LOADING = "Hidden/Universal Render Pipeline/FallbackLoading";
        const string URP_SHADER_MATERIAL_ERROR = "Hidden/Universal Render Pipeline/MaterialError";
        const string URP_SHADER_SAMPLING = "Hidden/Universal Render Pipeline/Sampling";
        const string URP_SHADER_SCREEN_SPACE_SHADOW = "Hidden/Universal Render Pipeline/ScreenSpaceShadows";
        const string URP_SHADER_STENCIAL_DEFERRED = "Hidden/Universal Render Pipeline/StencilDeferred";

        const string URP_POSTPROCESS_BLOOM = "Hidden/Universal Render Pipeline/Bloom";
        const string URP_POSTPROCESS_LUTBUILDER_LDR = "Hidden/Universal Render Pipeline/LutBuilderLdr";
        const string URP_POSTPROCESS_LUTBUILDER_HDR = "Hidden/Universal Render Pipeline/LutBuilderHdr";
        const string URP_POSTPROCESS_GAUSSIAN_DOF = "Hidden/Universal Render Pipeline/GaussianDepthOfField";
        const string URP_POSTPROCESS_BOKEH_DOF = "Hidden/Universal Render Pipeline/BokehDepthOfField";
        const string URP_POSTPROCESS_STOP_NAN = "Hidden/Universal Render Pipeline/Stop NaN";
        const string URP_POSTPROCESS_CAMERA_MOTION_BLUR = "Hidden/Universal Render Pipeline/CameraMotionBlur";
        const string URP_POSTPROCESS_SCALING_SETUP = "Hidden/Universal Render Pipeline/Scaling Setup";
        const string URP_POSTPROCESS_LENSFLARE_DATA_DRIVEN = "Hidden/Universal Render Pipeline/LensFlareDataDriven";
        const string URP_POSTPROCESS_PANIN_PROJECTION = "Hidden/Universal Render Pipeline/PaniniProjection";
        const string URP_POSTPROCESS_EASU = "Hidden/Universal Render Pipeline/Edge Adaptive Spatial Upsampling";
        const string URP_POSTPROCESS_TAA = "Hidden/Universal Render Pipeline/TemporalAA";
        const string URP_POSTPROCESS_SMAA = "Hidden/Universal Render Pipeline/SubpixelMorphologicalAntialiasing";

        const string URP_POSTPROCESS_UBER_POST = "Hidden/Universal Render Pipeline/UberPost";
        const string URP_FINAL_POST = "Hidden/Universal Render Pipeline/FinalPost";

        const string POST_PROCESS_TONEMAPPING_HDR_GRADING = "_HDR_GRADING";
        const string POST_PROCESS_TONEMAPPING_TONEMAP_NEUTRAL = "_TONEMAP_NEUTRAL";
        const string POST_PROCESS_TONEMAPPING_TONEMAP_ACES = "_TONEMAP_ACES";

        const string POST_PROCESS_BLOOM_LQ = "_BLOOM_LQ";
        const string POST_PROCESS_BLOOM_LQ_DIRT = "_BLOOM_LQ_DIRT";
        const string POST_PROCESS_BLOOM_HQ = "_BLOOM_HQ";
        const string POST_PROCESS_BLOOM_HQ_DIRT = "_BLOOM_HQ_DIRT";

        const string POST_PROCESS_BROKEH_DOF = "_USE_FAST_SRGB_LINEAR_CONVERSION";

        const string URP_CORE_ASSEMBLY_NAME = "Unity.RenderPipelines.Core.Runtime";
        const string URP_VOLUMEPROFILE_TYPE_NAME = "UnityEngine.Rendering.VolumeProfile";
        const string URP_LENSFLAREDATASRP_TYPE_NAME = "UnityEngine.Rendering.LensFlareDataSRP";

        internal const string UniversalAdditionalCameraDataTypeName = "UnityEngine.Rendering.Universal.UniversalAdditionalCameraData";

        #endregion


        #region Internal Methods

        internal static System.Type GetURPVolumeProfileType()
        {
            var assembly = Assembly.Load(URP_CORE_ASSEMBLY_NAME);
            return assembly.GetType(URP_VOLUMEPROFILE_TYPE_NAME);
        }

        internal static System.Type GetURPLensFlareDataSRPType()
        {
            var assembly = Assembly.Load(URP_CORE_ASSEMBLY_NAME);
            return assembly.GetType(URP_LENSFLAREDATASRP_TYPE_NAME);
        }

        internal static List<ShaderVariantCollection.ShaderVariant> GenerateSvForModule(string moduleFolder,
            List<string> allVolumeProfilePathList,
            List<string> allLensFlarePathList,
            bool hasTAA, bool hasFXAA, bool hasSMAA, bool hasDithering,bool hasUseScreenCoordOverride)
        {
            List<ShaderVariantCollection.ShaderVariant> mapping = new List<ShaderVariantCollection.ShaderVariant>();

            #region PostProcessingData //////////////////////////////////////////////
            bool gradingHDR = false;
            bool useFastSRGBLinearConversion = false;
            bool supportDataDrivenLensFlare = false;
            var renderPipelineList = SVCSettings.Instance.GetUniversalRenderPipelineAssetForModule(moduleFolder);
            foreach (var renderPipeline in renderPipelineList)
            {
                SerializedObject pipelineSO = new SerializedObject(renderPipeline);
                var colorGradingModeProp = pipelineSO.FindProperty("m_ColorGradingMode"); // m_ColorGradingMode:0-LowDynamicRange 1-HighDynamicRange
                if (colorGradingModeProp.enumValueFlag == 1)
                    gradingHDR = true;
                var useFastSRGBLinearConversionProp = pipelineSO.FindProperty("m_UseFastSRGBLinearConversion");
                if (useFastSRGBLinearConversionProp.boolValue)
                    useFastSRGBLinearConversion = true;
                var supportDataDrivenLensFlareProp = pipelineSO.FindProperty("m_SupportDataDrivenLensFlare");
                if (supportDataDrivenLensFlareProp.boolValue)
                    supportDataDrivenLensFlare = true;
            }

            #endregion

            #region 遍历模块内所有VolumeProfile,LensFlareDataSRP配置 ////////////////////////////////

            string emptyKW = "";
            var totalLutBuilderHdrKeywords = new List<string>();
            var combineLutBuilderHdrKeywords = new List<string[]>();
            var tonemappingModeList = new List<int>();
            var totalBloomKeywords = new List<string>();
            var combineBloomKeywords = new List<string[]>();
            var bloomHighQualityDic = new Dictionary<string, int>();
            var bloomDirtIntensityDic = new Dictionary<string, float>();
            var totalGuassianDofKeywords = new List<string>();
            var combineGuassianDofKeywords = new List<string[]>();
            var totalBokenDofKeywords = new List<string>();
            var combineBokenDofKeywords = new List<string[]>();
            var totalPaniniProjectionKeywords = new List<string>();
            var combinePaniniProjectionKeywords = new List<string[]>();

            // LensFlareDataSRP
            var lensFlareDataSRPType = GetURPLensFlareDataSRPType();
            List<string[]> lensFlareKeywords = new List<string[]> { new string[0] };
            if (supportDataDrivenLensFlare)
            {
                if (allLensFlarePathList.Count > 0)
                {
                    var lensFlareDataSRP_elements_field = lensFlareDataSRPType.GetField("elements");
                    bool existElements = false;
                    foreach (var guid in allLensFlarePathList)
                    {
                        var lensFlareData = AssetDatabase.LoadAssetAtPath<UnityEngineObj>(AssetDatabase.GUIDToAssetPath(guid));
                        if (lensFlareData == null) continue;

                        var elementsValue = (object[])lensFlareDataSRP_elements_field.GetValue(lensFlareData);
                        if (elementsValue.Length > 0)
                        {
                            existElements = true;
                            break;
                        }
                    }

                    if (existElements)
                    {
                        lensFlareKeywords = new List<string[]>()
                        {
                             new string[]{ ""},

                             new string[]{ "FLARE_CIRCLE"},
                             new string[]{ "FLARE_CIRCLE","FLARE_HAS_OCCLUSION"},
                             new string[]{ "FLARE_CIRCLE", "FLARE_HAS_OCCLUSION", "FLARE_INVERSE_SDF"},
                             //new string[]{ "FLARE_CIRCLE", "FLARE_HAS_OCCLUSION", "FLARE_INVERSE_SDF", "FLARE_OPENGL3_OR_OPENGLCORE"}, // FLARE_OPENGL3_OR_OPENGLCORE 报错
                             //new string[]{ "FLARE_CIRCLE", "FLARE_HAS_OCCLUSION", "FLARE_OPENGL3_OR_OPENGLCORE"},// FLARE_OPENGL3_OR_OPENGLCORE 报错
                             new string[]{ "FLARE_CIRCLE","FLARE_INVERSE_SDF"},
                             //new string[]{ "FLARE_CIRCLE", "FLARE_INVERSE_SDF", "FLARE_OPENGL3_OR_OPENGLCORE"},// FLARE_OPENGL3_OR_OPENGLCORE 报错
                             //new string[]{ "FLARE_CIRCLE","FLARE_OPENGL3_OR_OPENGLCORE"},

                             new string[]{ "FLARE_HAS_OCCLUSION"},
                             new string[]{ "FLARE_HAS_OCCLUSION","FLARE_INVERSE_SDF"},
                             //new string[]{ "FLARE_HAS_OCCLUSION", "FLARE_INVERSE_SDF", "FLARE_OPENGL3_OR_OPENGLCORE"},// FLARE_OPENGL3_OR_OPENGLCORE 报错
                             //new string[]{ "FLARE_HAS_OCCLUSION", "FLARE_INVERSE_SDF", "FLARE_OPENGL3_OR_OPENGLCORE", "FLARE_POLYGON"},// FLARE_OPENGL3_OR_OPENGLCORE 报错
                             new string[]{ "FLARE_HAS_OCCLUSION", "FLARE_INVERSE_SDF", "FLARE_POLYGON"},
                             new string[]{ "FLARE_HAS_OCCLUSION","FLARE_OPENGL3_OR_OPENGLCORE"},
                             //new string[]{ "FLARE_HAS_OCCLUSION","FLARE_OPENGL3_OR_OPENGLCORE", "FLARE_POLYGON"},// FLARE_OPENGL3_OR_OPENGLCORE 报错
                             new string[]{ "FLARE_HAS_OCCLUSION","FLARE_POLYGON"},

                             new string[]{ "FLARE_INVERSE_SDF"},
                             //new string[]{ "FLARE_INVERSE_SDF","FLARE_OPENGL3_OR_OPENGLCORE"},// FLARE_OPENGL3_OR_OPENGLCORE 报错
                             //new string[]{ "FLARE_INVERSE_SDF", "FLARE_OPENGL3_OR_OPENGLCORE", "FLARE_POLYGON"},// FLARE_OPENGL3_OR_OPENGLCORE 报错
                             new string[]{ "FLARE_INVERSE_SDF","FLARE_POLYGON"},

                             new string[]{ "FLARE_OPENGL3_OR_OPENGLCORE"},
                             //new string[]{ "FLARE_OPENGL3_OR_OPENGLCORE","FLARE_POLYGON"},// FLARE_OPENGL3_OR_OPENGLCORE 报错

                             new string[]{ "FLARE_POLYGON"},
                        };
                    }
                }
            }

            // Volume Profile
            var lensDistortionExist = false;
            var chromaticAberrationExist = false;
            var filmGrainExist = false;
            foreach (var path in allVolumeProfilePathList)
            {
                var volumeProfile = VolumeProfileParser.GetVolumeProfile(path);

                // Tonemapping 
                int mode = -1;
                var lutBuilderHdrKeywordsPerProfile = volumeProfile.GetTonemappingLutBuilderHdrKeywords(gradingHDR, out mode);
                if (lutBuilderHdrKeywordsPerProfile.Count == 0)
                {
                    if (!totalLutBuilderHdrKeywords.Contains(emptyKW))
                        totalLutBuilderHdrKeywords.Add(emptyKW);
                }
                else
                {
                    foreach (var kwGroup in lutBuilderHdrKeywordsPerProfile)
                    {
                        if (!totalLutBuilderHdrKeywords.Contains(kwGroup))
                            totalLutBuilderHdrKeywords.Add(kwGroup);
                    }
                }

                if (mode != -1 && !tonemappingModeList.Contains(mode))
                    tonemappingModeList.Add(mode);
                // Bloom
                int highQuality = -1;
                float dirtIntensity = 0;
                var bloomKeywordsPerProfile = volumeProfile.GetBloomKeywords(out highQuality, out dirtIntensity);
                if (bloomKeywordsPerProfile.Count == 0)
                {
                    if (!totalBloomKeywords.Contains(emptyKW))
                        totalBloomKeywords.Add(emptyKW);
                }
                else
                {
                    foreach (var kwGroup in bloomKeywordsPerProfile)
                    {
                        if (!totalBloomKeywords.Contains(kwGroup))
                            totalBloomKeywords.Add(kwGroup);
                    }
                }

                bloomHighQualityDic[path] = highQuality;
                bloomDirtIntensityDic[path] = dirtIntensity;
                // Gaussian DOF
                var gaussianDofKeywordsPerProfile = volumeProfile.GetGaussianDofKeywords();
                if (gaussianDofKeywordsPerProfile.Count == 0)
                {
                    if (!totalGuassianDofKeywords.Contains(emptyKW))
                        totalGuassianDofKeywords.Add(emptyKW);
                }
                else
                {
                    foreach (var kwGroup in gaussianDofKeywordsPerProfile)
                    {
                        if (!totalGuassianDofKeywords.Contains(kwGroup))
                            totalGuassianDofKeywords.Add(kwGroup);
                    }
                }
                // Bokeh DOF
                var bokehDofKeywordsPerProfile = volumeProfile.GetBokehDofKeywords(useFastSRGBLinearConversion);
                if (bokehDofKeywordsPerProfile.Count == 0)
                {
                    if (!totalBokenDofKeywords.Contains(emptyKW))
                        totalBokenDofKeywords.Add(emptyKW);
                }
                else
                {
                    foreach (var kwGroup in bokehDofKeywordsPerProfile)
                    {
                        if (!totalBokenDofKeywords.Contains(kwGroup))
                            totalBokenDofKeywords.Add(kwGroup);
                    }
                }
                // PaniniProjection
                var paniniProjectionKeywordsPerProfile = volumeProfile.GetPaniniProjectionKeywords();
                if (paniniProjectionKeywordsPerProfile.Count == 0)
                {
                    if (!totalPaniniProjectionKeywords.Contains(emptyKW))
                        totalPaniniProjectionKeywords.Add(emptyKW);
                }
                else
                {
                    foreach (var kwGroup in paniniProjectionKeywordsPerProfile)
                    {
                        if (!totalPaniniProjectionKeywords.Contains(kwGroup))
                            totalPaniniProjectionKeywords.Add(kwGroup);
                    }
                }
                // LensDistortion
                if (volumeProfile.GetLensDistortionActive())
                {
                    lensDistortionExist = true;
                }
                // ChromaticAberration
                if (volumeProfile.GetChromaticAberrationActive())
                {
                    chromaticAberrationExist = true;
                }
                // FilmGrain
                if (volumeProfile.GetFilmGrainActive())
                {
                    filmGrainExist = true;
                }

            }

            foreach (var kwGroup in totalLutBuilderHdrKeywords)
            {
                combineLutBuilderHdrKeywords.Add(kwGroup.Split('|'));
            }

            foreach (var kwGroup in totalBloomKeywords)
            {
                combineBloomKeywords.Add(kwGroup.Split('|'));
            }

            foreach (var kwGroup in totalGuassianDofKeywords)
            {
                combineGuassianDofKeywords.Add(kwGroup.Split('|'));
            }

            foreach (var kwGroup in totalBokenDofKeywords)
            {
                combineBokenDofKeywords.Add(kwGroup.Split('|'));
            }

            foreach (var kwGroup in totalPaniniProjectionKeywords)
            {
                combinePaniniProjectionKeywords.Add(kwGroup.Split('|'));
            }

            #endregion

            #region UberPost /////////////////////////////////////////////
            var uberPostKeywords = new List<string[]>();
            var tempKeywords = new List<string>();
            // ColorGrading
            if (gradingHDR)
            {
                tempKeywords.Add(POST_PROCESS_TONEMAPPING_HDR_GRADING);
            }
            else
            {
                string tonemappingStr = "";
                foreach (var tonemappingMode in tonemappingModeList)
                {
                    if (tonemappingMode == 1)
                    {
                        tonemappingStr += POST_PROCESS_TONEMAPPING_TONEMAP_NEUTRAL + "|";
                    }
                    else if (tonemappingMode == 2)
                    {
                        tonemappingStr += POST_PROCESS_TONEMAPPING_TONEMAP_ACES + "|";
                    }
                }

                if (!string.IsNullOrEmpty(tonemappingStr))
                {
                    tonemappingStr = tonemappingStr.Substring(0, tonemappingStr.Length - 1);
                    tempKeywords.Add(tonemappingStr);
                }
            }
            // Bloom
            string bloomStr = "";
            foreach (var data in bloomHighQualityDic)
            {
                var path = data.Key;
                var highQuality = data.Value;
                var dirtIntensity = bloomDirtIntensityDic[path];
                if (highQuality == -1)
                {
                    continue;
                }
                else if (highQuality == 0)
                {
                    if (dirtIntensity > 0f)
                    {
                        if (!bloomStr.Contains(POST_PROCESS_BLOOM_LQ_DIRT))
                            bloomStr += POST_PROCESS_BLOOM_LQ_DIRT + "|";
                    }
                    else
                    {
                        if (!bloomStr.Contains(POST_PROCESS_BLOOM_LQ))
                            bloomStr += POST_PROCESS_BLOOM_LQ + "|";
                    }

                }
                else if (highQuality == 1)
                {
                    if (dirtIntensity > 0f)
                    {
                        if (!bloomStr.Contains(POST_PROCESS_BLOOM_HQ_DIRT))
                            bloomStr += POST_PROCESS_BLOOM_HQ_DIRT + "|";
                    }
                    else
                    {
                        if (!bloomStr.Contains(POST_PROCESS_BLOOM_HQ))
                            bloomStr += POST_PROCESS_BLOOM_HQ + "|";
                    }
                }
            }
            if (!string.IsNullOrEmpty(bloomStr))
            {
                bloomStr = bloomStr.Substring(0, bloomStr.Length - 1);
                tempKeywords.Add(bloomStr);
            }

            // Gaussian DOF - NONE
            // Brokeh Dof
            if (useFastSRGBLinearConversion)
                tempKeywords.Add(POST_PROCESS_BROKEH_DOF);
            // ColorSpace
            tempKeywords.Add("|_GAMMA_20|_LINEAR_TO_SRGB_CONVERSION");
            // LensDistortion
            if (lensDistortionExist)
                tempKeywords.Add("_DISTORTION");
 
            // ChromaticAberration
            if (chromaticAberrationExist)
                tempKeywords.Add("_CHROMATIC_ABERRATION");
            // FilmGrain
            if (filmGrainExist)
                tempKeywords.Add("_FILM_GRAIN");
            // Dithering
            if(hasDithering)
                tempKeywords.Add("_DITHERING");
            // SCREEN_COORD_OVERRIDE
            if (hasUseScreenCoordOverride)
                tempKeywords.Add("SCREEN_COORD_OVERRIDE");
            // HDR_INPUT HDR_ENCODING
            tempKeywords.Add("|HDR_INPUT|HDR_ENCODING");

            // Final Combine
            uberPostKeywords = SVCUtility.GetCombination(tempKeywords.ToArray());
            uberPostKeywords.Add(new string[0]);

            #endregion

            #region FinalPost  /////////////////////////////////////////////
            var finalPostKeywords = new List<string[]>();
            tempKeywords.Clear();
            // ColorSpace
            tempKeywords.Add("_LINEAR_TO_SRGB_CONVERSION");
            // FilmGrain
            if (filmGrainExist)
                tempKeywords.Add("_FILM_GRAIN");
            // Dithering
            if (hasDithering)
                tempKeywords.Add("_DITHERING");
            // FXAA
            if (hasFXAA)
                tempKeywords.Add("_FXAA");
            // SCREEN_COORD_OVERRIDE
            if(hasUseScreenCoordOverride)
                tempKeywords.Add("SCREEN_COORD_OVERRIDE");
            //_POINT_SAMPLING _RCAS _EASU_RCAS_AND_HDR_INPUT
            tempKeywords.Add("|_POINT_SAMPLING|_RCAS|_EASU_RCAS_AND_HDR_INPUT");
            //HDR_INPUT HDR_COLORSPACE_CONVERSION HDR_ENCODING HDR_COLORSPACE_CONVERSION_AND_ENCODING
            tempKeywords.Add("|HDR_INPUT|HDR_COLORSPACE_CONVERSION|HDR_ENCODING|HDR_COLORSPACE_CONVERSION_AND_ENCODING");

            finalPostKeywords = SVCUtility.GetCombination(tempKeywords.ToArray());
            finalPostKeywords.Add(new string[0]);

            #endregion

            #region Generate SV /////////////////////////////////////////////

            foreach (var renderPipeline in renderPipelineList)
            {
                var assetPath = AssetDatabase.GetAssetPath(renderPipeline);
                var shaders = SVCUtility.GetDependencies<Shader>(assetPath, true);
                foreach (var shader in shaders)
                {
                    PassType passType = PassType.Normal;
                    List<string[]> keywords = new List<string[]> { new string[0] };
                    if (shader.name == URP_SHADER_CAMERA_MOTION_VECTORS || shader.name == URP_SHADER_BLIT || shader.name == URP_SHADER_FALLBACK_LOADING ||
                        shader.name == URP_SHADER_FALLBACK_ERROR || shader.name == URP_SHADER_MATERIAL_ERROR || shader.name == URP_SHADER_SAMPLING ||
                        shader.name == URP_SHADER_SCREEN_SPACE_SHADOW || shader.name == URP_SHADER_STENCIAL_DEFERRED || shader.name == URP_SHADER_HDR_DEBUG_VIEW ||
                        shader.name == URP_POSTPROCESS_LUTBUILDER_LDR || shader.name == URP_POSTPROCESS_STOP_NAN || shader.name == URP_POSTPROCESS_CAMERA_MOTION_BLUR ||
                        shader.name == URP_POSTPROCESS_EASU)
                    {
                        passType = PassType.Normal;
                        keywords = null;
                    }
                    else if (shader.name == URP_SHADER_DEBUG_REPLACEMENT)
                    {
                        passType = PassType.ScriptableRenderPipeline;
                        keywords = null;
                    }
                    else if (shader.name == URP_SHADER_OBJ_MOTION_VECTORS)
                    {
                        passType = PassType.MotionVectors;
                        keywords = null;
                    }
                    else if (shader.name == URP_SHADER_BLIT_HDR_OVERLAY)
                    {
                        passType = PassType.Normal;
                        keywords = new List<string[]>() {
                        new string[]{ ""},
                        new string[]{ "BLIT_SINGLE_SLICE"},
                        new string[]{ "DISABLE_TEXTURE2D_X_ARRAY"},
                        new string[]{ "BLIT_SINGLE_SLICE","DISABLE_TEXTURE2D_X_ARRAY"},
                        new string[]{ "HDR_ENCODING"},
                        new string[]{ "HDR_ENCODING","BLIT_SINGLE_SLICE" },
                        new string[]{ "HDR_ENCODING","DISABLE_TEXTURE2D_X_ARRAY" },
                        new string[]{ "HDR_ENCODING","BLIT_SINGLE_SLICE","DISABLE_TEXTURE2D_X_ARRAY"},
                        new string[]{ "HDR_COLORSPACE_CONVERSION"},
                        new string[]{ "HDR_COLORSPACE_CONVERSION","BLIT_SINGLE_SLICE" },
                        new string[]{ "HDR_COLORSPACE_CONVERSION","DISABLE_TEXTURE2D_X_ARRAY" },
                        new string[]{ "HDR_COLORSPACE_CONVERSION","BLIT_SINGLE_SLICE","DISABLE_TEXTURE2D_X_ARRAY"},
                        new string[]{ "HDR_COLORSPACE_CONVERSION_AND_ENCODING"},
                        new string[]{ "HDR_COLORSPACE_CONVERSION_AND_ENCODING","BLIT_SINGLE_SLICE" },
                        new string[]{ "HDR_COLORSPACE_CONVERSION_AND_ENCODING","DISABLE_TEXTURE2D_X_ARRAY" },
                        new string[]{ "HDR_COLORSPACE_CONVERSION_AND_ENCODING","BLIT_SINGLE_SLICE","DISABLE_TEXTURE2D_X_ARRAY"},
                    };
                    }
                    else if (shader.name == URP_SHADER_COPY_DEPTH)
                    {
                        passType = PassType.Normal;
                        keywords = new List<string[]>() {
                        new string[]{ ""},
                        new string[]{ "_OUTPUT_DEPTH"},
                    };
                    }
                    else if (shader.name == URP_SHADER_CORE_BLIT)
                    {
                        passType = PassType.Normal;
                        keywords = new List<string[]>() {
                        new string[]{ ""},
                        new string[]{ "BLIT_DECODE_HDR"},
                        new string[]{ "BLIT_SINGLE_SLICE"},
                        new string[]{ "DISABLE_TEXTURE2D_X_ARRAY"},
                        new string[]{ "BLIT_DECODE_HDR","BLIT_SINGLE_SLICE"},
                        new string[]{ "BLIT_DECODE_HDR","DISABLE_TEXTURE2D_X_ARRAY"},
                        new string[]{ "BLIT_SINGLE_SLICE","DISABLE_TEXTURE2D_X_ARRAY"},
                        new string[]{ "BLIT_DECODE_HDR","BLIT_SINGLE_SLICE","DISABLE_TEXTURE2D_X_ARRAY" },
                    };
                    }
                    else if (shader.name == URP_SHADER_CORE_BLIT_COLOR_AND_DEPTH)
                    {
                        passType = PassType.Normal;
                        keywords = new List<string[]>() {
                        new string[]{ ""},
                        new string[]{ "BLIT_SINGLE_SLICE"},
                        new string[]{ "DISABLE_TEXTURE2D_X_ARRAY"},
                        new string[]{ "BLIT_SINGLE_SLICE", "DISABLE_TEXTURE2D_X_ARRAY"},
                    };
                    }
                    else if (shader.name == URP_POSTPROCESS_LUTBUILDER_HDR)
                    {
                        passType = PassType.Normal;
                        keywords = combineLutBuilderHdrKeywords;
                    }
                    else if (shader.name == URP_POSTPROCESS_BLOOM)
                    {
                        passType = PassType.Normal;
                        keywords = combineBloomKeywords;
                    }
                    else if (shader.name == URP_POSTPROCESS_GAUSSIAN_DOF)
                    {
                        passType = PassType.Normal;
                        keywords = combineGuassianDofKeywords;
                    }
                    else if (shader.name == URP_POSTPROCESS_BOKEH_DOF)
                    {
                        passType = PassType.Normal;
                        keywords = combineBokenDofKeywords;
                    }
                    else if (shader.name == URP_POSTPROCESS_SCALING_SETUP)
                    {
                        passType = PassType.Normal;
                        if (PlayerSettings.colorSpace == ColorSpace.Linear)
                        {
                            keywords = new List<string[]>()
                        {
                            new string[]{ "_FXAA"},
                            new string[]{ "_GAMMA_20"},
                            new string[]{ "_FXAA_AND_GAMMA_20"},
                            new string[]{ "_FXAA", "HDR_INPUT"},
                            new string[]{ "_GAMMA_20", "HDR_INPUT"},
                            new string[]{ "_FXAA_AND_GAMMA_20", "HDR_INPUT"},
                        };
                        }
                        else
                        {
                            keywords = new List<string[]>()
                        {
                            new string[]{ "_FXAA"},
                            new string[]{ "_FXAA", "HDR_INPUT"},
                        };
                        }
                    }
                    else if (shader.name == URP_POSTPROCESS_PANIN_PROJECTION)
                    {
                        passType = PassType.Normal;
                        keywords = combinePaniniProjectionKeywords;
                    }
                    else if (shader.name == URP_POSTPROCESS_LENSFLARE_DATA_DRIVEN)
                    {
                        passType = PassType.Normal;
                        keywords = lensFlareKeywords;
                    }
                    else if (shader.name == URP_POSTPROCESS_TAA)
                    {
                        passType = PassType.Normal;
                        if (hasTAA)
                        {
                            keywords = new List<string[]>() {
                            new string[]{ ""},
                            new string[]{ "_USE_DRAW_PROCEDURAL"},
                            new string[]{ "TAA_LOW_PRECISION_SOURCE"},
                            new string[]{ "TAA_LOW_PRECISION_SOURCE", "_USE_DRAW_PROCEDURAL"},
                        };
                        }
                    }
                    else if (shader.name == URP_POSTPROCESS_SMAA)
                    {
                        passType = PassType.Normal;
                        if (hasSMAA)
                        {
                            keywords = new List<string[]>() {
                            new string[]{ "_SMAA_PRESET_LOW"},
                            new string[]{ "_SMAA_PRESET_MEDIUM"},
                            new string[]{ "_SMAA_PRESET_HIGH"},
                        };
                        }
                    }
                    else if (shader.name == URP_POSTPROCESS_UBER_POST)
                    {
                        passType = PassType.Normal;
                        keywords = uberPostKeywords;
                    }
                    else if (shader.name == URP_FINAL_POST)
                    {
                        passType = PassType.Normal;
                        keywords = finalPostKeywords;
                    }

                    if (keywords != null)
                    {
                        foreach (var kwGroup in keywords)
                        {
                            try
                            {
                                var sv = new ShaderVariantCollection.ShaderVariant(shader, passType, kwGroup);
                                mapping.Add(sv);
                            }
                            catch (System.Exception e)
                            {
                                // TODO
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            var sv = new ShaderVariantCollection.ShaderVariant(shader, passType, null);
                            mapping.Add(sv);
                        }
                        catch (System.Exception e)
                        {
                            // TODO
                        }
                    }
                }
            }
          

            #endregion

            return mapping;
        }

        #endregion
    }
}