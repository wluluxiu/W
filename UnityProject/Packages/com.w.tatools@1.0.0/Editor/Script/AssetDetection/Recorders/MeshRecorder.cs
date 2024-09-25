using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    /// <summary>
    /// 检测项目：
    /// 1.VertexCount
    /// 2.Triangles
    /// 3.UV
    /// 4.Normal
    /// 5.Tangent
    /// 6.Color
    /// 7.BlendShapeCount
    /// 8.RW
    /// </summary>
    internal class MeshRecorder : BaseRecorder
    {
        #region Fields

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
        internal int m_Readable;

        #endregion

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

        internal bool UVChannalInvalid()
        {
            bool invalid = this.m_UV3 == ModelRecorder.MODEL_UV_CHANNEL_THRESHOLD[2] ||
                this.m_UV4 == ModelRecorder.MODEL_UV_CHANNEL_THRESHOLD[3] ||
                this.m_UV5 == ModelRecorder.MODEL_UV_CHANNEL_THRESHOLD[4] ||
                this.m_UV6 == ModelRecorder.MODEL_UV_CHANNEL_THRESHOLD[5] ||
                this.m_UV7 == ModelRecorder.MODEL_UV_CHANNEL_THRESHOLD[6] ||
                this.m_UV8 == ModelRecorder.MODEL_UV_CHANNEL_THRESHOLD[7];
            return invalid;
        }

        internal bool BlendShapeInvalid()
        {
            return this.m_BlendShapeCount > ModelRecorder.MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD;
        }

        internal bool TriangleInvalid()
        {
            return this.m_Triangle > ModelRecorder.MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD;
        }

        internal bool VertexCountInvalid()
        {
            return this.m_VertexCount > ModelRecorder.MODEL_VERTEX_MAX_AMOUNT_THRESHOLD;
        }

        internal bool VertexColorInvalid()
        {
            return this.m_Color == ModelRecorder.MODEL_VERTEX_COLOR_THRESHOLD;
        }

        internal bool RWInvalid()
        {
            return this.m_Readable == ModelRecorder.MODEL_RW_THRESHOLD;
        }

        #region Override Methods

        internal override void Record(string assetPath, EAssetType assetType)
        {
            base.Record(assetPath, assetType);

            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);

            this.m_VertexCount = mesh.vertexCount;
            this.m_Triangle = mesh.triangles.Length / 3;
            this.m_UV = mesh.uv.Length > 0 ? 1 : 0;
            this.m_UV2 = mesh.uv2.Length > 0 ? 1 : 0;
            this.m_UV3 = mesh.uv3.Length > 0 ? 1 : 0;
            this.m_UV4 = mesh.uv4.Length > 0 ? 1 : 0;
            this.m_UV5 = mesh.uv5.Length > 0 ? 1 : 0;
            this.m_UV6 = mesh.uv6.Length > 0 ? 1 : 0;
            this.m_UV7 = mesh.uv7.Length > 0 ? 1 : 0;
            this.m_UV8 = mesh.uv8.Length > 0 ? 1 : 0;
            this.m_Normal = mesh.normals.Length > 0 ? 1 : 0;
            this.m_Tangent = mesh.tangents.Length > 0 ? 1 : 0;
            this.m_Color = mesh.colors.Length > 0 ? 1 : 0;
            this.m_BlendShapeCount = mesh.blendShapeCount;
            this.m_Readable = mesh.isReadable ? 1 :0;

            mesh = null;
        }

        /// <summary>
        /// BaseFormat|VertexCount|Triangle|UV|UV2|UV3|UV4|U5V|UV6|UV7|UV8|Normal|Tangent|Color|BlendShapeCount|Readable
        /// </summary>
        /// <returns></returns>
        internal override string GetOutputStr()
        {
            var baseOutputStr = base.GetOutputStr();

            string spiltStr = CHAR_SPLIT_FIRST_FLAG.ToString();

            return baseOutputStr + spiltStr +
                    this.m_VertexCount + spiltStr +
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
                    this.m_BlendShapeCount  + spiltStr +
                    this.m_Readable;
        }

        internal override void ParseStrLine(string stringLine)
        {
            base.ParseStrLine(stringLine);

            if (base.m_SourceDataArr.Length > 7 && !string.IsNullOrEmpty(base.m_SourceDataArr[7]))
            {
                this.m_VertexCount = int.Parse(base.m_SourceDataArr[7]);
            }
  
            if (base.m_SourceDataArr.Length > 8 && !string.IsNullOrEmpty(base.m_SourceDataArr[8]))
            {
                this.m_Triangle = int.Parse(base.m_SourceDataArr[8]);
            }

            if (base.m_SourceDataArr.Length > 9 && !string.IsNullOrEmpty(base.m_SourceDataArr[9]))
            {
                this.m_UV = int.Parse(base.m_SourceDataArr[9]);
            }

            if (base.m_SourceDataArr.Length > 10 && !string.IsNullOrEmpty(base.m_SourceDataArr[10]))
            {
                this.m_UV2 = int.Parse(base.m_SourceDataArr[10]);
            }

            if (base.m_SourceDataArr.Length > 11 && !string.IsNullOrEmpty(base.m_SourceDataArr[11]))
            {
                this.m_UV3 = int.Parse(base.m_SourceDataArr[11]);
            }

            if (base.m_SourceDataArr.Length > 12 && !string.IsNullOrEmpty(base.m_SourceDataArr[12]))
            {
                this.m_UV4 = int.Parse(base.m_SourceDataArr[12]);
            }

            if (base.m_SourceDataArr.Length > 13 && !string.IsNullOrEmpty(base.m_SourceDataArr[13]))
            {
                this.m_UV5 = int.Parse(base.m_SourceDataArr[13]);
            }

            if (base.m_SourceDataArr.Length > 14 && !string.IsNullOrEmpty(base.m_SourceDataArr[14]))
            {
                this.m_UV6 = int.Parse(base.m_SourceDataArr[14]);
            }

            if (base.m_SourceDataArr.Length > 15 && !string.IsNullOrEmpty(base.m_SourceDataArr[15]))
            {
                this.m_UV7 = int.Parse(base.m_SourceDataArr[15]);
            }

            if (base.m_SourceDataArr.Length > 16 && !string.IsNullOrEmpty(base.m_SourceDataArr[16]))
            {
                this.m_UV8 = int.Parse(base.m_SourceDataArr[16]);
            }

            if (base.m_SourceDataArr.Length > 17 && !string.IsNullOrEmpty(base.m_SourceDataArr[17]))
            {
                this.m_Normal = int.Parse(base.m_SourceDataArr[17]);
            }

            if (base.m_SourceDataArr.Length > 18 && !string.IsNullOrEmpty(base.m_SourceDataArr[18]))
            {
                this.m_Tangent = int.Parse(base.m_SourceDataArr[18]);
            }

            if (base.m_SourceDataArr.Length > 19 && !string.IsNullOrEmpty(base.m_SourceDataArr[19]))
            {
                this.m_Color = int.Parse(base.m_SourceDataArr[19]);
            }

            if (base.m_SourceDataArr.Length > 20 && !string.IsNullOrEmpty(base.m_SourceDataArr[20]))
            {
                this.m_BlendShapeCount = int.Parse(base.m_SourceDataArr[20]);
            }

            if (base.m_SourceDataArr.Length > 21 && !string.IsNullOrEmpty(base.m_SourceDataArr[21]))
            {
                this.m_Readable = int.Parse(base.m_SourceDataArr[21]);
            }
        }

        #endregion
    }
}
