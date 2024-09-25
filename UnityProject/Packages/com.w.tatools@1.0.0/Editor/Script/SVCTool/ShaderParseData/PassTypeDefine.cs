namespace jj.TATools.Editor
{
    using UnityEngine.Rendering;
    using System.Collections.Generic;

    internal class PassTypeDefine
    {
        internal enum ELightModeURP
        {
            // The Pass renders object geometry and evaluates all light contributions. URP uses this tag value in the Forward Rendering Path.
            UniversalForward = 0,
            // The Pass renders object geometry without evaluating any light contribution. URP uses this tag value in the Deferred Rendering Path.
            UniversalGBuffer,
            /* The Pass renders object geometry and evaluates all light contributions, similarly to when LightMode has the UniversalForward value.
             * The difference from UniversalForward is that URP can use the Pass for both the Forward and the Deferred Rendering Paths.
             * Use this value if a certain Pass must render objects with the Forward Rendering Path when URP is using the Deferred Rendering Path.
             * For example, use this tag if URP renders a Scene using the Deferred Rendering Path and the Scene contains objects with shader data that does not fit the GBuffer,
             * such as Clear Coat normals.
             * If a shader must render in both the Forward and the Deferred Rendering Paths, declare two Passes with the UniversalForward and UniversalGBuffer tag values.
             * If a shader must render using the Forward Rendering Path regardless of the Rendering Path that the URP Renderer uses, 
             * declare only a Pass with the LightMode tag set to UniversalForwardOnly.
             */ 
            UniversalForwardOnly,
            // The Pass renders objects and evaluates 2D light contributions. URP uses this tag value in the 2D Renderer.
            Universal2D,
            // The Pass renders object depth from the perspective of lights into the Shadow map or a depth texture.
            ShadowCaster,
            // The Pass renders only depth information from the perspective of a Camera into a depth texture.
            DepthOnly,
            //Unity executes this Pass only when baking lightmaps in the Unity Editor. Unity strips this Pass from shaders when building a Player.
            Meta,
            /* Use this LightMode tag value to draw an extra Pass when rendering objects. 
             * Application example: draw an object outline. This tag value is valid for both the Forward and the Deferred Rendering Paths.
             * URP uses this tag value as the default value when a Pass does not have a LightMode tag.
             */
            SRPDefaultUnlit
            // Not Support:Always, ForwardAdd, PrepassBase, PrepassFinal, Vertex, VertexLMRGBM, VertexLM.
        }

        internal enum ELightModeBuiltin
        {
            // Always rendered; does not apply any lighting. This is the default value.
            Always = 0,
            // Used in Forward rendering; applies ambient, main directional light, vertex/SH lights and lightmaps.
            ForwardBase,
            // Used in Forward rendering; applies additive per-pixel lights, one Pass per light.
            ForwardAdd,
            // Used in Deferred Shading; renders G-buffer.
            Deferred,
            // Renders object depth into the shadowmap or a depth texture.
            ShadowCaster,
            // Used to calculate per-object motion vectors.
            MotionVectors,
            // Used in legacy Vertex Lit rendering when the object is not lightmapped; applies all vertex lights.
            Vertex,
            // Used in legacy Vertex Lit rendering when the object is lightmapped, and on platforms where the lightmap is RGBM encoded (PC & console).
            VertexLMRGBM,
            // Used in legacy Vertex Lit rendering when the object is lightmapped, and on platforms where lightmap is double-LDR encoded (mobile platforms).
            VertexLM,
            // This Pass is not used during regular rendering, only for lightmap baking or Enlighten Realtime Global Illumination .For more information, see Lightmapping and shaders.
            Meta
        }

        static readonly Dictionary<string, PassType> LightMode_Mapping = new Dictionary<string, PassType>() {
            // Common
            {"", PassType.Normal},
            {"shadowcaster", PassType.ShadowCaster},
            {"meta", PassType.Meta},
            // URP
            {"universalforward", PassType.ScriptableRenderPipeline},
            {"depthonly", PassType.ScriptableRenderPipeline},
            {"depthnormals", PassType.ScriptableRenderPipeline},
            {"universal2d", PassType.ScriptableRenderPipeline},
            {"universalforwardonly", PassType.ScriptableRenderPipeline},
            {"srpdefaultunlit", PassType.ScriptableRenderPipelineDefaultUnlit},
            {"universalgbuffer", PassType.Deferred},
            // Builtin
            {"always", PassType.Normal},
            {"forwardbase", PassType.ForwardBase},
            {"forwardadd", PassType.ForwardAdd},
            {"deferred", PassType.Deferred},
            {"motionvectors", PassType.MotionVectors},
            {"vertex", PassType.Vertex},
            {"vertexlm", PassType.VertexLM},
            {"vertexlmrgbm", PassType.VertexLMRGBM},
        };

        internal static PassType GetPassTypeByLightMode(string lightmodeStr)
        {
            PassType passType = PassType.Normal;
            LightMode_Mapping.TryGetValue(lightmodeStr.ToLower(), out passType);

            return passType;
        }

        internal static bool PassTypeInvalid(PassType passType)
        {
            var pipelineType = SVCUtility.GetProjectRenderPipelineType();
            if (pipelineType == ERenderPipelineType.URP)
            {
                return passType == PassType.ForwardBase || passType == PassType.ForwardAdd ||
                    passType == PassType.Meta || passType == PassType.Deferred ||
                    passType == PassType.Vertex || passType == PassType.VertexLM ||
                    passType == PassType.VertexLMRGBM || passType == PassType.MotionVectors  ||
                    passType == PassType.GrabPass || passType == PassType.Deferred;
            }
            else
            {
                return passType == PassType.ScriptableRenderPipeline || passType == PassType.ScriptableRenderPipelineDefaultUnlit ||
                    passType == PassType.Meta || passType == PassType.Deferred ||
                    passType == PassType.GrabPass;
            }
        }
    }
}