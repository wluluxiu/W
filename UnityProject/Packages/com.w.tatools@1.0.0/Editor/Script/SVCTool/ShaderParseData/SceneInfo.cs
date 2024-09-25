namespace jj.TATools.Editor
{
    using System.IO;
    using System.Reflection;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.SceneManagement;
    using UnityEditor;

    internal class SceneInfo
    {
        internal bool m_MainDirLight = false;
        internal bool m_MainDirLightWithShadow = false;
        internal bool m_MainDirLightWithSoftShadow = false;

        internal bool m_ExistContributeGITrue = false;
        internal bool m_ExistContributeGIFalse = false;

        internal bool m_EnabledRealtimeGI = false;
        internal bool m_EnabledBakedGI = false;
        internal bool m_IsLightingDataAssetNull = false;
        internal MixedLightingMode m_MixedBakeMode = MixedLightingMode.IndirectOnly;
        internal LightmapsMode m_LightingDirectionalMode = LightmapsMode.NonDirectional;
        internal bool m_EnableFog = false;
        internal FogMode m_FogMode = FogMode.Linear;

        public SceneInfo()
        {
            // Lighting //////////////////////////////////////////////////////
            // EnabledRealtimeGI
            m_EnabledRealtimeGI = Lightmapping.realtimeGI;
            // EnabledBakedGI
            m_EnabledBakedGI = Lightmapping.bakedGI;
            // isLightingDataAssetNull
            m_IsLightingDataAssetNull = Lightmapping.lightingDataAsset == null;
            if (!m_IsLightingDataAssetNull)
            {
                if (Lightmapping.lightingSettings != null)
                {
                    // MixedBakeMode
                    m_MixedBakeMode = Lightmapping.lightingSettings.mixedBakeMode;
                    // DirectionalMode
                    m_LightingDirectionalMode = Lightmapping.lightingSettings.directionalityMode;
                }
            }

            // Fog Mode //////////////////////////////////////////////////////
            m_EnableFog = RenderSettings.fog;
            m_FogMode = RenderSettings.fogMode;
        }
    }
}