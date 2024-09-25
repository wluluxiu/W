using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    //
    // 摘要:
    //     Material import options for ModelImporter.
    internal enum EModelImporterMaterialImportMode
    {
        //
        // 摘要:
        //     The model importer does not import materials.
        None = 0,
        //
        // 摘要:
        //     The model importer imports materials in standard mode using OnPostprocessMaterial
        //     callback.
        ImportStandard = 1,
        LegacyImport = 1,
        //
        // 摘要:
        //     The model importer creates materials using the PreprocessMaterialDescription
        //     AssetPostprocessor.
        ImportViaMaterialDescription = 2,
        Import = 2
    }

    //
    // 摘要:
    //     Animation mode for ModelImporter.
    internal enum EModelImporterAnimationType
    {
        //
        // 摘要:
        //     Generate no animation data.
        None = 0,
        //
        // 摘要:
        //     Generate a legacy animation type.
        Legacy = 1,
        //
        // 摘要:
        //     Generate a generic animator.
        Generic = 2,
        //
        // 摘要:
        //     Generate a human animator.
        Human = 3
    }

    //
    // 摘要:
    //     Set the Avatar generation mode for ModelImporter.
    internal enum EModelImporterAvatarSetup
    {
        //
        // 摘要:
        //     Disable Avatar creation for this model.
        NoAvatar = 0,
        //
        // 摘要:
        //     Create the Avatar from this model and save it as a sub-asset.
        CreateFromThisModel = 1,
        //
        // 摘要:
        //     Copy the Avatar from another model.
        CopyFromOther = 2
    }

    //
    // 摘要:
    //     Mesh compression options for ModelImporter.
    internal enum EModelImporterMeshCompression
    {
        //
        // 摘要:
        //     No mesh compression (default).
        Off = 0,
        //
        // 摘要:
        //     Low amount of mesh compression.
        Low = 1,
        //
        // 摘要:
        //     Medium amount of mesh compression.
        Medium = 2,
        //
        // 摘要:
        //     High amount of mesh compression.
        High = 3
    }

    //
    // 摘要:
    //     Format of the imported mesh index buffer data.
    internal enum EModelImporterIndexFormat
    {
        //
        // 摘要:
        //     Use 16 or 32 bit index buffer depending on mesh size.
        Auto = 0,
        //
        // 摘要:
        //     Use 16 bit index buffer.
        UInt16 = 1,
        //
        // 摘要:
        //     Use 32 bit index buffer.
        UInt32 = 2
    }

    //
    // 摘要:
    //     Options to control the optimization of mesh data during asset import.
    internal enum EMeshOptimizationFlags
    {
        //
        // 摘要:
        //     Perform maximum optimization of the mesh data, enables all optimization options.
        Everything = -1,
        //
        // 摘要:
        //     Optimize the order of polygons in the mesh to make better use of the GPUs internal
        //     caches to improve rendering performance.
        PolygonOrder = 1,
        //
        // 摘要:
        //     Optimize the order of vertices in the mesh to make better use of the GPUs internal
        //     caches to improve rendering performance.
        VertexOrder = 2
    }

    //
    // 摘要:
    //     Animation compression options for ModelImporter.
    internal enum EModelImporterAnimationCompression
    {
        //
        // 摘要:
        //     No animation compression.
        Off = 0,
        //
        // 摘要:
        //     Perform keyframe reduction.
        KeyframeReduction = 1,
        //
        // 摘要:
        //     Perform keyframe reduction and compression.
        KeyframeReductionAndCompression = 2,
        //
        // 摘要:
        //     Perform keyframe reduction and choose the best animation curve representation
        //     at runtime to reduce memory footprint (default).
        Optimal = 3
    }

    /// <summary>
    /// 检测项目：
    /// </summary>
    internal class ModelRecorder : BaseRecorder
    {
        #region Fields

        internal static int MODEL_MAX_BONES_THRESHOLD = 240;
        internal static int[] MODEL_UV_CHANNEL_THRESHOLD = new int[] { 0, 0, 1, 1, 1, 1, 1, 1 };
        internal static int MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD = 0;
        internal static int MODEL_AUTO_GENERATE_UV2_THRESHOLD = 1;
        internal static int MODEL_ANIM_COMPRESSION_THRESHOLD = 0;
        internal static int MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD = 50000;
        internal static int MODEL_VERTEX_MAX_AMOUNT_THRESHOLD = 50000;
        internal static int MODEL_VERTEX_COLOR_THRESHOLD = 1;
        internal static int MODEL_RW_THRESHOLD = 1;

        // Model
        internal float m_ScaleFactor;
        internal int m_ConvertUnits;
        internal int m_ImportBlendShape;
        internal int m_ImportVisibility;
        internal int m_ImportCameras;
        internal int m_ImportLights;
        internal int m_PreserveHierarchy;
        internal int m_MeshCompression;
        internal int m_RW;
        internal int m_OptimizeMesh;
        internal int m_GenerateColliders;
        internal int m_KeepQuads;
        internal int m_WeldVertices;
        internal int m_IndexFormat;
        internal int m_GenerateLightmapUVs;

        // Rig
        internal int m_AnimationType;
        internal int m_AvatarDefinition;

        // Animation
        internal int m_ImportConstraints;
        internal int m_ImportAnimation;
        internal int m_AnimCompression;
        internal float m_RotationError;
        internal float m_PositionError;
        internal float m_ScaleError;
        internal List<int> m_Legacys = new List<int>();
        internal List<float> m_Lengths = new List<float>();
        internal List<int> m_WrapModes = new List<int>();// Legacy
        internal List<int> m_LoopTimes = new List<int>();// New

        // Materials
        internal int m_MaterialCreationMode;

        // Mesh
        internal int m_VertexCount;
        internal int m_Triangle;
        internal int m_UV;
        internal int m_UV2;
        internal int m_UV3;
        internal int m_UV4;
        internal int m_UV5;
        internal int m_UV6;
        internal int m_UV7;
        internal int m_UV8;
        internal int m_Normal;
        internal int m_Tangent;
        internal int m_Color;
        internal int m_BlendShapeCount;
        internal int m_Bones;

        #endregion

        #region Internal Methods
        internal static void UpdateDataFromConfig()
        {
            MODEL_MAX_BONES_THRESHOLD = AppConfigHelper.MODEL_MAX_BONES_THRESHOLD;
            MODEL_UV_CHANNEL_THRESHOLD = AppConfigHelper.MODEL_UV_CHANNEL_THRESHOLD;
            MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD = AppConfigHelper.MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD;
            MODEL_AUTO_GENERATE_UV2_THRESHOLD = AppConfigHelper.MODEL_AUTO_GENERATE_UV2_THRESHOLD;
            MODEL_ANIM_COMPRESSION_THRESHOLD = AppConfigHelper.MODEL_ANIM_COMPRESSION_THRESHOLD;
            MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD = AppConfigHelper.MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD;
            MODEL_VERTEX_MAX_AMOUNT_THRESHOLD = AppConfigHelper.MODEL_VERTEX_MAX_AMOUNT_THRESHOLD;
            MODEL_VERTEX_COLOR_THRESHOLD = AppConfigHelper.MODEL_VERTEX_COLOR_THRESHOLD;
            MODEL_RW_THRESHOLD = AppConfigHelper.MODEL_RW_THRESHOLD;
        }

        internal string GetUVShowStr()
        {
            string uvShowStr = "";

            if (this.m_UV == 1) uvShowStr += "0|";
            if (this.m_UV2 == 1) uvShowStr += "2|";
            if (this.m_UV3 == 1) uvShowStr += "3|";
            if (this.m_UV4 == 1) uvShowStr += "4|";
            if (this.m_UV5 == 1) uvShowStr += "5|";
            if (this.m_UV6 == 1) uvShowStr += "6|";
            if (this.m_UV7 == 1) uvShowStr += "7|";
            if (this.m_UV8 == 1) uvShowStr += "8|";

            if (!string.IsNullOrEmpty(uvShowStr))
                uvShowStr = uvShowStr.Substring(0, uvShowStr.Length - 1);

            return uvShowStr;
        }

        internal bool BoneCountInvalid()
        {
            return this.m_Bones > MODEL_MAX_BONES_THRESHOLD;
        }

        internal bool UVChannalInvalid()
        {
            bool invalid = this.m_UV3 == MODEL_UV_CHANNEL_THRESHOLD[2] ||
                this.m_UV4 == MODEL_UV_CHANNEL_THRESHOLD[3] ||
                this.m_UV5 == MODEL_UV_CHANNEL_THRESHOLD[4] ||
                this.m_UV6 == MODEL_UV_CHANNEL_THRESHOLD[5] ||
                this.m_UV7 == MODEL_UV_CHANNEL_THRESHOLD[6] ||
                this.m_UV8 == MODEL_UV_CHANNEL_THRESHOLD[7];
            return invalid;
        }

        internal bool BlendShapeInvalid()
        {
            return this.m_BlendShapeCount > MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD;
        }

        internal bool GenerateUV2Invalid()
        {
            return this.m_GenerateLightmapUVs == MODEL_AUTO_GENERATE_UV2_THRESHOLD;
        }

        internal bool AnimCompressionInvalid()
        {
            if (this.m_ImportAnimation == 1)
            {
                return this.m_AnimCompression == MODEL_ANIM_COMPRESSION_THRESHOLD; // EModelImporterAnimationCompression
            }

            return false;
        }

        internal bool TriangleInvalid()
        {
            return this.m_Triangle > MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD;
        }

        internal bool VertexCountInvalid()
        {
            return this.m_VertexCount > MODEL_VERTEX_MAX_AMOUNT_THRESHOLD;
        }

        internal bool VertexColorInvalid()
        {
            return this.m_Color == MODEL_VERTEX_COLOR_THRESHOLD;
        }

        internal bool RWInvalid()
        {
            return this.m_RW == MODEL_RW_THRESHOLD;
        }

        #endregion

        #region Override Methods

        internal override void Record(string assetPath, EAssetType assetType)
        {
            base.Record(assetPath, assetType);

            var modelImporter = base.m_BaseImporter as ModelImporter;

            // Model
            this.m_ScaleFactor = modelImporter.globalScale;
            this.m_ConvertUnits = modelImporter.useFileScale ? 1 : 0;
            this.m_ImportBlendShape = modelImporter.importBlendShapes ? 1 : 0;
            this.m_ImportVisibility = modelImporter.importVisibility ? 1 : 0;
            this.m_ImportCameras = modelImporter.importCameras ? 1 : 0;
            this.m_ImportLights = modelImporter.importLights ? 1 : 0;
            this.m_PreserveHierarchy = modelImporter.preserveHierarchy ? 1 : 0;
            this.m_MeshCompression = (int)modelImporter.meshCompression;
            this.m_RW = modelImporter.isReadable ? 1 : 0;
            this.m_OptimizeMesh = (int)modelImporter.meshOptimizationFlags;
            this.m_KeepQuads = modelImporter.keepQuads ? 1 : 0;
            this.m_WeldVertices = modelImporter.weldVertices ? 1 : 0;
            this.m_IndexFormat = (int)modelImporter.indexFormat;
            this.m_GenerateLightmapUVs = modelImporter.generateSecondaryUV ? 1 : 0;
            // Rig
            this.m_AnimationType = (int)modelImporter.animationType;
            this.m_AvatarDefinition = (int)modelImporter.avatarSetup;
            // Animation
            this.m_ImportConstraints = modelImporter.importConstraints ? 1 : 0;
            this.m_ImportAnimation = modelImporter.importAnimation ? 1 : 0;
            this.m_AnimCompression = (int)modelImporter.animationCompression;
            this.m_RotationError = modelImporter.animationRotationError;
            this.m_PositionError = modelImporter.animationPositionError;
            this.m_ScaleError = modelImporter.animationScaleError;
            if (modelImporter.importAnimation && modelImporter.clipAnimations.Length > 0)
            {
                var allAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                foreach (var asset in allAssets)
                {
                    if (asset is AnimationClip)
                    {
                        AnimationClip animationClip = asset as AnimationClip;
                        if (animationClip.name.StartsWith("__preview__")) continue;

                        this.m_Legacys.Add(animationClip.legacy ? 1 : 0);
                        this.m_Lengths.Add(animationClip.length);
                        this.m_WrapModes.Add((int)animationClip.wrapMode);
                        this.m_LoopTimes.Add(animationClip.isLooping ? 1 : 0);
                    }
                }
            }
            // Materials
            this.m_MaterialCreationMode = (int)modelImporter.materialImportMode;
            // Mesh
            GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (model != null)
            {
                // MeshFilter
                MeshFilter[] mfs = model.GetComponentsInChildren<MeshFilter>(true);
                foreach (var mf in mfs)
                {
                    Mesh mesh = mf.sharedMesh;
                    if (mesh != null)
                    {
                        this.m_VertexCount += mesh.vertexCount;
                        this.m_Triangle += mesh.triangles.Length / 3;
                        if (this.m_UV == 0) this.m_UV = mesh.uv.Length > 0 ? 1 : 0;
                        if (this.m_UV2 == 0) this.m_UV2 = mesh.uv2.Length > 0 ? 1 : 0;
                        if (this.m_UV3 == 0) this.m_UV3 = mesh.uv3.Length > 0 ? 1 : 0;
                        if (this.m_UV4 == 0) this.m_UV4 = mesh.uv4.Length > 0 ? 1 : 0;
                        if (this.m_UV5 == 0) this.m_UV5 = mesh.uv5.Length > 0 ? 1 : 0;
                        if (this.m_UV6 == 0) this.m_UV6 = mesh.uv6.Length > 0 ? 1 : 0;
                        if (this.m_UV7 == 0) this.m_UV7 = mesh.uv7.Length > 0 ? 1 : 0;
                        if (this.m_UV8 == 0) this.m_UV8 = mesh.uv8.Length > 0 ? 1 : 0;
                        if (this.m_Normal == 0) this.m_Normal = mesh.normals.Length > 0 ? 1 : 0;
                        if (this.m_Tangent == 0) this.m_Tangent = mesh.tangents.Length > 0 ? 1 : 0;
                        if (this.m_Color == 0) this.m_Color = mesh.colors.Length > 0 ? 1 : 0;
                        this.m_BlendShapeCount += mesh.blendShapeCount;
                    }
                }
                // SkinnedMeshRenderer
                SkinnedMeshRenderer[] smrs = model.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                foreach (var smr in smrs)
                {
                    Mesh mesh = smr.sharedMesh;
                    if (mesh != null)
                    {
                        this.m_VertexCount += mesh.vertexCount;
                        this.m_Triangle += mesh.triangles.Length / 3;
                        if (this.m_UV == 0) this.m_UV = mesh.uv.Length > 0 ? 1 : 0;
                        if (this.m_UV2 == 0) this.m_UV2 = mesh.uv2.Length > 0 ? 1 : 0;
                        if (this.m_UV3 == 0) this.m_UV3 = mesh.uv3.Length > 0 ? 1 : 0;
                        if (this.m_UV4 == 0) this.m_UV4 = mesh.uv4.Length > 0 ? 1 : 0;
                        if (this.m_UV5 == 0) this.m_UV5 = mesh.uv5.Length > 0 ? 1 : 0;
                        if (this.m_UV6 == 0) this.m_UV6 = mesh.uv6.Length > 0 ? 1 : 0;
                        if (this.m_UV7 == 0) this.m_UV7 = mesh.uv7.Length > 0 ? 1 : 0;
                        if (this.m_UV8 == 0) this.m_UV8 = mesh.uv8.Length > 0 ? 1 : 0;
                        if (this.m_Normal == 0) this.m_Normal = mesh.normals.Length > 0 ? 1 : 0;
                        if (this.m_Tangent == 0) this.m_Tangent = mesh.tangents.Length > 0 ? 1 : 0;
                        if (this.m_Color == 0) this.m_Color = mesh.colors.Length > 0 ? 1 : 0;
                        this.m_BlendShapeCount += mesh.blendShapeCount;
                    }
                    this.m_Bones += smr.bones.Length;
                }

                model = null;
            }

            modelImporter = null;
        }

        /// <summary>
        /// BaseFormat|
        /// ScaleFactor#ConvertUnits#ImportBlendShape#ImportVisibility#ImportCameras#ImportLights#PreserveHierarchy#MeshCompression#RW#OptimizeMesh#KeepQuads#WeldVertices#IndexFormat#GenerateLightmapUVs|
        /// AnimationType#AvatarDefinition|
        /// ImportConstraints#ImportAnimation#AnimCompression#RotationError#PositionError#ScaleError#Legacy1$Legacy2$Legacy3$...#Length1$Length2$Length3$...#WrapMode1$WrapMode2$WrapMode3$...#LoopTime1$LoopTime2$LoopTime3$...|
        /// MaterialCreationMode|
        /// VertexCount|Triangle|UV|UV2|UV3|UV4|U5V|UV6|UV7|UV8|Normal|Tangent|Color|BlendShapeCount|Bones
        /// </summary>
        /// <returns></returns>
        internal override string GetOutputStr()
        {
            var baseOutputStr = base.GetOutputStr();

            string spiltStr = CHAR_SPLIT_FIRST_FLAG.ToString();
            string secondSplitStr = CHAR_SPLIT_SECOND_FLAG.ToString();
            string thirdSplitStr = CHAR_SPLIT_THIRD_FLAG.ToString();

            // Model
            string modelStr = this.m_ScaleFactor + secondSplitStr +
                              this.m_ConvertUnits + secondSplitStr +
                              this.m_ImportBlendShape + secondSplitStr +
                              this.m_ImportVisibility + secondSplitStr +
                              this.m_ImportCameras + secondSplitStr +
                              this.m_ImportLights + secondSplitStr +
                              this.m_PreserveHierarchy + secondSplitStr +
                              this.m_MeshCompression + secondSplitStr +
                              this.m_RW + secondSplitStr +
                              this.m_OptimizeMesh + secondSplitStr +
                              this.m_GenerateColliders + secondSplitStr +
                              this.m_KeepQuads + secondSplitStr +
                              this.m_WeldVertices + secondSplitStr +
                              this.m_IndexFormat + secondSplitStr +
                              this.m_GenerateLightmapUVs;
            // Rig
            string rigStr = this.m_AnimationType + secondSplitStr + this.m_AvatarDefinition;
            // Animation
            string legacyStr = "";
            foreach (var legacy in this.m_Legacys)
            {
                legacyStr += legacy + thirdSplitStr;
            }

            if (!string.IsNullOrEmpty(legacyStr))
                legacyStr = legacyStr.Substring(0, legacyStr.Length - 1);

            string lengthStr = "";
            foreach (var length in this.m_Lengths)
            {
                lengthStr += length + thirdSplitStr;
            }

            if (!string.IsNullOrEmpty(lengthStr))
                lengthStr = lengthStr.Substring(0, lengthStr.Length - 1);

            string wrapModeStr = "";
            foreach (var wrapMode in this.m_WrapModes)
            {
                wrapModeStr += wrapMode + thirdSplitStr;
            }

            if (!string.IsNullOrEmpty(wrapModeStr))
                wrapModeStr = wrapModeStr.Substring(0, wrapModeStr.Length - 1);

            string loopTimeStr = "";
            foreach (var loopTime in this.m_LoopTimes)
            {
                loopTimeStr += loopTime + thirdSplitStr;
            }

            if (!string.IsNullOrEmpty(loopTimeStr))
                loopTimeStr = loopTimeStr.Substring(0, loopTimeStr.Length - 1);

            string animationStr = this.m_ImportConstraints + secondSplitStr +
                                  this.m_ImportAnimation + secondSplitStr +
                                  this.m_AnimCompression + secondSplitStr +
                                  this.m_RotationError + secondSplitStr +
                                  this.m_PositionError + secondSplitStr +
                                  this.m_ScaleError + secondSplitStr +
                                  legacyStr + secondSplitStr +
                                  lengthStr + secondSplitStr +
                                  wrapModeStr + secondSplitStr +
                                  loopTimeStr;
            // Material
            string materialsStr = this.m_MaterialCreationMode.ToString();
            // Mesh
            string meshStr = this.m_VertexCount + spiltStr +
                    this.m_Triangle + spiltStr +
                    this.m_UV + spiltStr +
                    this.m_UV2 + spiltStr +
                    this.m_UV3 + spiltStr +
                    this.m_UV4 + spiltStr +
                    this.m_UV5 + spiltStr +
                    this.m_UV6 + spiltStr +
                    this.m_UV7 + spiltStr +
                    this.m_UV8 + spiltStr +
                    this.m_Normal + spiltStr +
                    this.m_Tangent + spiltStr +
                    this.m_Color + spiltStr +
                    this.m_BlendShapeCount + spiltStr +
                    this.m_Bones;

            return baseOutputStr + spiltStr +
                   modelStr + spiltStr +
                   rigStr + spiltStr +
                   animationStr + spiltStr +
                   materialsStr + spiltStr +
                   meshStr;
        }

        internal override void ParseStrLine(string stringLine)
        {
            base.ParseStrLine(stringLine);

            if (base.m_SourceDataArr.Length > 7 && !string.IsNullOrEmpty(base.m_SourceDataArr[7]))
            {
                string[] childArr = base.m_SourceDataArr[7].Split(CHAR_SPLIT_SECOND_FLAG);

                if (childArr.Length > 0 && !string.IsNullOrEmpty(childArr[0]))
                    this.m_ScaleFactor = float.Parse(childArr[0]);
                if (childArr.Length > 1 && !string.IsNullOrEmpty(childArr[1]))
                    this.m_ConvertUnits = int.Parse(childArr[1]);
                if (childArr.Length > 2 && !string.IsNullOrEmpty(childArr[2]))
                    this.m_ImportBlendShape = int.Parse(childArr[2]);
                if (childArr.Length > 3 && !string.IsNullOrEmpty(childArr[3]))
                    this.m_ImportVisibility = int.Parse(childArr[3]);
                if (childArr.Length > 4 && !string.IsNullOrEmpty(childArr[4]))
                    this.m_ImportCameras = int.Parse(childArr[4]);
                if (childArr.Length > 5 && !string.IsNullOrEmpty(childArr[5]))
                    this.m_ImportLights = int.Parse(childArr[5]);
                if (childArr.Length > 6 && !string.IsNullOrEmpty(childArr[6]))
                    this.m_PreserveHierarchy = int.Parse(childArr[6]);
                if (childArr.Length > 7 && !string.IsNullOrEmpty(childArr[7]))
                    this.m_MeshCompression = int.Parse(childArr[7]);
                if (childArr.Length > 8 && !string.IsNullOrEmpty(childArr[8]))
                    this.m_RW = int.Parse(childArr[8]);
                if (childArr.Length > 9 && !string.IsNullOrEmpty(childArr[9]))
                    this.m_OptimizeMesh = int.Parse(childArr[9]);
                if (childArr.Length > 10 && !string.IsNullOrEmpty(childArr[10]))
                    this.m_GenerateColliders = int.Parse(childArr[10]);
                if (childArr.Length > 11 && !string.IsNullOrEmpty(childArr[11]))
                    this.m_KeepQuads = int.Parse(childArr[11]);
                if (childArr.Length > 12 && !string.IsNullOrEmpty(childArr[12]))
                    this.m_WeldVertices = int.Parse(childArr[12]);
                if (childArr.Length > 13 && !string.IsNullOrEmpty(childArr[13]))
                    this.m_IndexFormat = int.Parse(childArr[13]);
                if (childArr.Length > 14 && !string.IsNullOrEmpty(childArr[14]))
                    this.m_GenerateLightmapUVs = int.Parse(childArr[14]);
            }

            if (base.m_SourceDataArr.Length > 8 && !string.IsNullOrEmpty(base.m_SourceDataArr[8]))
            {
                string[] childArr = base.m_SourceDataArr[8].Split(CHAR_SPLIT_SECOND_FLAG);

                if (childArr.Length > 0 && !string.IsNullOrEmpty(childArr[0]))
                    this.m_AnimationType = int.Parse(childArr[0]);
                if (childArr.Length > 1 && !string.IsNullOrEmpty(childArr[1]))
                    this.m_AvatarDefinition = int.Parse(childArr[1]);
            }

            if (base.m_SourceDataArr.Length > 9 && !string.IsNullOrEmpty(base.m_SourceDataArr[9]))
            {
                string[] childArr = base.m_SourceDataArr[9].Split(CHAR_SPLIT_SECOND_FLAG);

                if (childArr.Length > 0 && !string.IsNullOrEmpty(childArr[0]))
                    this.m_ImportConstraints = int.Parse(childArr[0]);
                if (childArr.Length > 1 && !string.IsNullOrEmpty(childArr[1]))
                    this.m_ImportAnimation = int.Parse(childArr[1]);
                if (childArr.Length > 2 && !string.IsNullOrEmpty(childArr[2]))
                    this.m_AnimCompression = int.Parse(childArr[2]);
                if (childArr.Length > 3 && !string.IsNullOrEmpty(childArr[3]))
                    this.m_RotationError = float.Parse(childArr[3]);
                if (childArr.Length > 4 && !string.IsNullOrEmpty(childArr[4]))
                    this.m_PositionError = float.Parse(childArr[4]);
                if (childArr.Length > 5 && !string.IsNullOrEmpty(childArr[5]))
                    this.m_ScaleError = float.Parse(childArr[5]);
                if (childArr.Length > 6 && !string.IsNullOrEmpty(childArr[6]))
                {
                    this.m_Legacys = new List<int>();
                    string[] grandArr = childArr[6].Split(CHAR_SPLIT_THIRD_FLAG);
                    foreach (var legacy in grandArr)
                        this.m_Legacys.Add(int.Parse(legacy));
                }
                if (childArr.Length > 7 && !string.IsNullOrEmpty(childArr[7]))
                {
                    this.m_Lengths = new List<float>();
                    string[] grandArr = childArr[7].Split(CHAR_SPLIT_THIRD_FLAG);
                    foreach (var length in grandArr)
                        this.m_Lengths.Add(float.Parse(length));
                }
                if (childArr.Length > 8 && !string.IsNullOrEmpty(childArr[8]))
                {
                    this.m_WrapModes = new List<int>();
                    string[] grandArr = childArr[8].Split(CHAR_SPLIT_THIRD_FLAG);
                    foreach (var wrapMode in grandArr)
                        this.m_WrapModes.Add(int.Parse(wrapMode));
                }
                if (childArr.Length > 9 && !string.IsNullOrEmpty(childArr[9]))
                {
                    this.m_LoopTimes = new List<int>();
                    string[] grandArr = childArr[8].Split(CHAR_SPLIT_THIRD_FLAG);
                    foreach (var loop in grandArr)
                        this.m_LoopTimes.Add(int.Parse(loop));
                }
            }

            if (base.m_SourceDataArr.Length > 10 && !string.IsNullOrEmpty(base.m_SourceDataArr[10]))
            {
                this.m_MaterialCreationMode = int.Parse(base.m_SourceDataArr[10]);
            }

            if (base.m_SourceDataArr.Length > 11 && !string.IsNullOrEmpty(base.m_SourceDataArr[11]))
            {
                this.m_VertexCount = int.Parse(base.m_SourceDataArr[11]);
            }

            if (base.m_SourceDataArr.Length > 12 && !string.IsNullOrEmpty(base.m_SourceDataArr[12]))
            {
                this.m_Triangle = int.Parse(base.m_SourceDataArr[12]);
            }

            if (base.m_SourceDataArr.Length > 13 && !string.IsNullOrEmpty(base.m_SourceDataArr[13]))
            {
                this.m_UV = int.Parse(base.m_SourceDataArr[13]);
            }

            if (base.m_SourceDataArr.Length > 14 && !string.IsNullOrEmpty(base.m_SourceDataArr[14]))
            {
                this.m_UV2 = int.Parse(base.m_SourceDataArr[14]);
            }

            if (base.m_SourceDataArr.Length > 15 && !string.IsNullOrEmpty(base.m_SourceDataArr[15]))
            {
                this.m_UV3 = int.Parse(base.m_SourceDataArr[15]);
            }

            if (base.m_SourceDataArr.Length > 16 && !string.IsNullOrEmpty(base.m_SourceDataArr[16]))
            {
                this.m_UV4 = int.Parse(base.m_SourceDataArr[16]);
            }

            if (base.m_SourceDataArr.Length > 17 && !string.IsNullOrEmpty(base.m_SourceDataArr[17]))
            {
                this.m_UV5 = int.Parse(base.m_SourceDataArr[17]);
            }

            if (base.m_SourceDataArr.Length > 18 && !string.IsNullOrEmpty(base.m_SourceDataArr[18]))
            {
                this.m_UV6 = int.Parse(base.m_SourceDataArr[18]);
            }

            if (base.m_SourceDataArr.Length > 19 && !string.IsNullOrEmpty(base.m_SourceDataArr[19]))
            {
                this.m_UV7 = int.Parse(base.m_SourceDataArr[19]);
            }

            if (base.m_SourceDataArr.Length > 20 && !string.IsNullOrEmpty(base.m_SourceDataArr[20]))
            {
                this.m_UV8 = int.Parse(base.m_SourceDataArr[20]);
            }

            if (base.m_SourceDataArr.Length > 21 && !string.IsNullOrEmpty(base.m_SourceDataArr[21]))
            {
                this.m_Normal = int.Parse(base.m_SourceDataArr[21]);
            }

            if (base.m_SourceDataArr.Length > 22 && !string.IsNullOrEmpty(base.m_SourceDataArr[22]))
            {
                this.m_Tangent = int.Parse(base.m_SourceDataArr[22]);
            }

            if (base.m_SourceDataArr.Length > 23 && !string.IsNullOrEmpty(base.m_SourceDataArr[23]))
            {
                this.m_Color = int.Parse(base.m_SourceDataArr[23]);
            }

            if (base.m_SourceDataArr.Length > 24 && !string.IsNullOrEmpty(base.m_SourceDataArr[24]))
            {
                this.m_BlendShapeCount = int.Parse(base.m_SourceDataArr[24]);
            }

            if (base.m_SourceDataArr.Length > 25 && !string.IsNullOrEmpty(base.m_SourceDataArr[25]))
            {
                this.m_Bones = int.Parse(base.m_SourceDataArr[25]);
            }
        }

        internal override void Release()
        {
            base.Release();

            this.m_Legacys.Clear();
            this.m_Legacys = null;
            this.m_Lengths.Clear();
            this.m_Lengths = null;
            this.m_WrapModes.Clear();
            this.m_WrapModes = null;
            this.m_LoopTimes.Clear();
            this.m_LoopTimes = null;
        }

        #endregion
    }
}
