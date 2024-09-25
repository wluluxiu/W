using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    /// <summary>
    /// 检测项目：
    /// 1.冗余引用信息
    /// 2.引用Shader资源丢失
    /// 3.Builtin Dep
    /// </summary>
    internal class MaterialRecorder : BaseRecorder
    {
        #region Fields

        const string PROP_ROOT_NAME = "m_SavedProperties";
        const string PROP_TEXENVS_NAME = "m_TexEnvs";
        const string PROP_FLOAT_INT_NAME = "m_Floats";
        const string PROP_COLOR_VECTOR_NAME = "m_Colors";

        internal Dictionary<string,string> m_NoUsedTexEnvsProps = new Dictionary<string, string>();
        internal List<string> m_NoUsedFloatAndIntProps = new List<string>();
        internal List<string> m_NoUsedColorAndVectorProps = new List<string>();

        internal int m_ShaderRefState;
        internal int m_RefBuiltinShader;
        internal string m_MissShaderGuid;

        #endregion

        #region Internal Methods

        internal bool RefShaderValid()
        {
            return this.m_ShaderRefState == 0;
        }

        internal bool ExistNoUsedTexProps()
        {
            return this.m_NoUsedTexEnvsProps.Count > 0;
        }

        internal bool ExistNoUsedFloatAndIntProps()
        {
            return this.m_NoUsedFloatAndIntProps.Count > 0;
        }

        internal bool ExistNoUsedColorAndVectorProps()
        {
            return this.m_NoUsedColorAndVectorProps.Count > 0;
        }

        #endregion

        #region Override Methods

        internal override void Record(string assetPath, EAssetType assetType)
        {
            base.Record(assetPath, assetType);

            var material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);

            SerializedObject materialProp = new SerializedObject(material);

            SerializedProperty savedProperties = materialProp.FindProperty(PROP_ROOT_NAME);
            SerializedProperty texEnvsProp = savedProperties.FindPropertyRelative(PROP_TEXENVS_NAME);
            SerializedProperty floatAndIntProp = savedProperties.FindPropertyRelative(PROP_FLOAT_INT_NAME);
            SerializedProperty colorAndVectorProp = savedProperties.FindPropertyRelative(PROP_COLOR_VECTOR_NAME);
           
            ////////// TexEnvs///////////////
            for (int i = texEnvsProp.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty sp = texEnvsProp.GetArrayElementAtIndex(i);

                if (!material.HasProperty(sp.displayName))
                {
                    var tex = material.GetTexture(sp.displayName);
                    this.m_NoUsedTexEnvsProps.Add(sp.displayName, (tex == null ? "" : AssetDatabase.GetAssetPath(tex)));
                }
            }

            ////////// Float Int///////////////
            for (int i = floatAndIntProp.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty sp = floatAndIntProp.GetArrayElementAtIndex(i);

                if (!material.HasProperty(sp.displayName))
                {
                    this.m_NoUsedFloatAndIntProps.Add(sp.displayName);
                }
            }

            ////////// Color Vector///////////////
            for (int i = colorAndVectorProp.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty sp = colorAndVectorProp.GetArrayElementAtIndex(i);

                if (!material.HasProperty(sp.displayName))
                {
                    this.m_NoUsedColorAndVectorProps.Add(sp.displayName);
                }
            }

            // Miss Ref Shader
            SerializedProperty shaderRefProp = materialProp.FindProperty("m_Shader");
            var shader = shaderRefProp.objectReferenceValue;
            this.m_ShaderRefState = shader ? 1 : 0;
            if (shader != null)
            {
                var refShaderPath = AssetDatabase.GetAssetPath(material.shader);
                this.m_RefBuiltinShader = refShaderPath.StartsWith("Assets/") ? 0 : 1;
            }
            else
            {
                this.m_MissShaderGuid = AssetDetectionUtility.GetMaterialRefShaderGuid(assetPath);
            }

            // Builtin Dep
            base.m_BuiltinDependencies = AssetDetectionUtility.GetBuiltinDenpendencies(material);

            material = null;
        }

        /// <summary>
        /// BaseFormat|
        /// ShaderMissState|MissShaderGuid|RefBuiltinShader|
        /// TexProp1$TexPath1#TexProp2$TexPath2#TexProp3$TexPath3#...|
        /// FloatAndIntProp1#FloatAndIntProp2#FloatAndIntProp3#...|
        /// ColorAndVectorProp1#ColorAndVectorProp2#ColorAndVectorProp3#...|
        /// BuiltinDep1#BuiltinDep2#BuiltinDep3#...
        /// </summary>
        /// <returns></returns>
        internal override string GetOutputStr()
        {
            var baseOutputStr = base.GetOutputStr();

            string spiltStr = CHAR_SPLIT_FIRST_FLAG.ToString();

            // NoUsed TexProp
            string noUsedTexPropStr = "";
            foreach (var tpData in this.m_NoUsedTexEnvsProps)
            {
                noUsedTexPropStr += tpData.Key + CHAR_SPLIT_THIRD_FLAG + tpData.Value + CHAR_SPLIT_SECOND_FLAG;
            }

            if (!string.IsNullOrEmpty(noUsedTexPropStr))
                noUsedTexPropStr = noUsedTexPropStr.Substring(0, noUsedTexPropStr.Length - 1);

            // NoUsed Float And Int Prop
            string noUsedFloatAndIntPropStr = "";
            foreach (var fip in this.m_NoUsedFloatAndIntProps)
                noUsedFloatAndIntPropStr += fip +  CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(noUsedFloatAndIntPropStr))
                noUsedFloatAndIntPropStr = noUsedFloatAndIntPropStr.Substring(0, noUsedFloatAndIntPropStr.Length - 1);

            // NoUsed Color And Vector Prop
            string noUsedColorAndVectorPropStr = "";
            foreach (var cvp in this.m_NoUsedColorAndVectorProps)
                noUsedColorAndVectorPropStr += cvp + CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(noUsedColorAndVectorPropStr))
                noUsedColorAndVectorPropStr = noUsedColorAndVectorPropStr.Substring(0, noUsedColorAndVectorPropStr.Length - 1);

            // Builtin Dep
            string builtinDepStr = "";
            foreach (var nodePath in base.m_BuiltinDependencies)
                builtinDepStr += nodePath + CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(builtinDepStr))
                builtinDepStr = builtinDepStr.Substring(0, builtinDepStr.Length - 1);

            return baseOutputStr + spiltStr +
                   this.m_ShaderRefState + spiltStr +
                   this.m_MissShaderGuid + spiltStr +
                   this.m_RefBuiltinShader + spiltStr +
                   noUsedTexPropStr + spiltStr +
                   noUsedFloatAndIntPropStr + spiltStr +
                   noUsedColorAndVectorPropStr + spiltStr +
                   builtinDepStr;
        }

        internal override void ParseStrLine(string stringLine)
        {
            base.ParseStrLine(stringLine);

            if (base.m_SourceDataArr.Length > 7 && !string.IsNullOrEmpty(base.m_SourceDataArr[7]))
            {
                this.m_ShaderRefState = int.Parse(base.m_SourceDataArr[7]);
            }

            if (base.m_SourceDataArr.Length > 8 && !string.IsNullOrEmpty(base.m_SourceDataArr[8]))
            {
                this.m_MissShaderGuid = base.m_SourceDataArr[8];
            }

            if (base.m_SourceDataArr.Length > 9 && !string.IsNullOrEmpty(base.m_SourceDataArr[9]))
            {
                this.m_RefBuiltinShader = int.Parse(base.m_SourceDataArr[9]);
            }

            if (base.m_SourceDataArr.Length > 10 && !string.IsNullOrEmpty(base.m_SourceDataArr[10]))
            {
                string[] arr = base.m_SourceDataArr[10].Split(CHAR_SPLIT_SECOND_FLAG);
                this.m_NoUsedTexEnvsProps = new Dictionary<string, string>();
                foreach (var str in arr)
                {
                    var childArr = str.Split(CHAR_SPLIT_THIRD_FLAG);
                    this.m_NoUsedTexEnvsProps[childArr[0]] = childArr[1];
                }
            }

            if (base.m_SourceDataArr.Length > 11 && !string.IsNullOrEmpty(base.m_SourceDataArr[11]))
            {
                string[] arr = base.m_SourceDataArr[11].Split(CHAR_SPLIT_SECOND_FLAG);
                this.m_NoUsedFloatAndIntProps = new List<string>(arr);
            }

            if (base.m_SourceDataArr.Length > 12 && !string.IsNullOrEmpty(base.m_SourceDataArr[12]))
            {
                string[] arr = base.m_SourceDataArr[12].Split(CHAR_SPLIT_SECOND_FLAG);
                this.m_NoUsedColorAndVectorProps = new List<string>(arr);
            }

            if (base.m_SourceDataArr.Length > 13 && !string.IsNullOrEmpty(base.m_SourceDataArr[13]))
            {
                string[] arr = base.m_SourceDataArr[13].Split(CHAR_SPLIT_SECOND_FLAG);
                base.m_BuiltinDependencies = new List<string>(arr);
            }
        }

        internal override void Release()
        {
            base.Release();

            this.m_NoUsedTexEnvsProps.Clear();
            this.m_NoUsedTexEnvsProps = null;

            this.m_NoUsedColorAndVectorProps.Clear();
            this.m_NoUsedColorAndVectorProps = null;

            this.m_NoUsedFloatAndIntProps.Clear();
            this.m_NoUsedFloatAndIntProps = null;

            this.m_MissShaderGuid = null;
        }

        #endregion
    }
}
