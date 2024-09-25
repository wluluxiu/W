using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    internal enum EAssetType
    {
        Model = 0,
        Mesh,
        AnimationClip,
        AnimatorController,
        Texture,
        RenderTexture,
        SpriteAtlas,
        Material,
        Shader,
        Scene,
        Prefab,
        Script,
        AudioClip,
        VideoClip,
        Other,
    }

    /// <summary>
    /// 检测项目：
    /// 1.路径
    /// 2.MD5
    /// 3.Meta文件MD5
    /// 4.DiskSize
    /// 5.Depenendices
    /// 6.Referencies
    /// </summary>
    internal class BaseRecorder
    {
        #region Fields

        const string RESOURCES_FOLDER_PART = "/Resources/";

        protected const char CHAR_SPLIT_FIRST_FLAG = '|';
        protected const char CHAR_SPLIT_SECOND_FLAG = '#';
        protected const char CHAR_SPLIT_THIRD_FLAG = '$';

        internal EAssetType m_AssetType;
        internal string m_AssetPath;
        internal string m_AssetMetaPath;
        internal string m_AssetMD5;
        internal string m_AssetMetaMD5;
        internal long m_FileDiskSize;
        internal long m_FileDiskSizeOld;

        internal List<string> m_ReferencedInCodePathList;
        internal List<string> m_DirectDepenpendices = new List<string>();
        internal List<string> m_Referencies = new List<string>();
        internal List<string> m_BuiltinDependencies = new List<string>();
        internal List<string> m_BundleNames = new List<string>(0);

        internal string m_BundleNamesStr;

        protected AssetImporter m_BaseImporter;
        protected string[] m_SourceDataArr;

        #endregion

        #region Local Methods

        void CollectBundleName()
        {
            var assetbundleName = this.m_BaseImporter.assetBundleName;
            if (string.IsNullOrEmpty(assetbundleName))
            {
                DirectoryInfo dInfo = new DirectoryInfo(this.m_AssetPath);
                DirectoryInfo parent = dInfo.Parent;
                var parentFolderPath = parent.FullName.Replace("\\", "/");
                while (parentFolderPath != Application.dataPath)
                {
                    AssetImporter folderImporter = AssetImporter.GetAtPath(parentFolderPath.Replace(Application.dataPath, "Assets"));
                    if (!string.IsNullOrEmpty(folderImporter.assetBundleName))
                    {
                        assetbundleName = folderImporter.assetBundleName;
                        break;
                    }

                    parent = parent.Parent;
                    parentFolderPath = parent.FullName.Replace("\\", "/");
                }
            }

            if (!string.IsNullOrEmpty(assetbundleName))
                AddBundleNames(assetbundleName);
        }

        #endregion

        #region Internal Methods

        internal bool InvalidBuiltinDependencies()
        {
            return this.m_BuiltinDependencies.Count > 0;
        }

        internal bool IsInReourcesFolder()
        {
            return this.m_AssetPath.Contains(RESOURCES_FOLDER_PART);
        }

        internal bool DependenciesInvalid()
        {
            return this.m_DirectDepenpendices.Count > 0;
        }


        internal bool ReferenciesInvalid()
        {
            return this.m_Referencies.Count == 0;
        }

        internal void DoCheckReferencedInCode(Dictionary<string, string> scriptsMapping)
        {
            if (ReferenciesInvalid())
            {
                this.m_ReferencedInCodePathList = AssetDetectionUtility.CheckReferencedInCode(Path.GetFileNameWithoutExtension(this.m_AssetPath), scriptsMapping);
            }
        }

        internal bool AssetbundleInvalid()
        {
            return this.m_BundleNames.Count > 1;
        }

        internal static BaseRecorder CreateRecorder(EAssetType assetType)
        {
            BaseRecorder recorder = null;
            if (assetType == EAssetType.Texture) recorder = new TextureRecorder();
            else if (assetType == EAssetType.Script) recorder = new ScriptRecorder();
            else if (assetType == EAssetType.Shader) recorder = new ShaderRecorder();
            else if (assetType == EAssetType.Scene) recorder = new SceneRecorder();
            else if (assetType == EAssetType.Material) recorder = new MaterialRecorder();
            else if (assetType == EAssetType.RenderTexture) recorder = new RenderTextureRecorder();
            else if (assetType == EAssetType.Prefab) recorder = new PrefabRecorder();
            else if (assetType == EAssetType.Mesh) recorder = new MeshRecorder();
            else if (assetType == EAssetType.AnimationClip) recorder = new AnimationClipRecorder();
            else if (assetType == EAssetType.Model) recorder = new ModelRecorder();
            else if (assetType == EAssetType.SpriteAtlas) recorder = new SpriteAtlasRecorder();
            else recorder = new BaseRecorder();

            return recorder;
        }

        internal static void GetAllRecorders(string filePath, System.Action<BaseRecorder> action)
        {
            if (action == null) return;

            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                string lineStr = sr.ReadLine();
                while (lineStr != null)
                {
                    var arr = lineStr.Split(CHAR_SPLIT_FIRST_FLAG);
                    if (arr.Length > 0 && !string.IsNullOrEmpty(arr[0]))
                    {
                        var assetType = (EAssetType)int.Parse(arr[0]);
                        var recorder = BaseRecorder.CreateRecorder(assetType);
                        recorder.ParseStrLine(lineStr);

                        action(recorder);
                    }


                    lineStr = sr.ReadLine();
                }

                sr.Close();
                sr.Dispose();
            }
        }

        internal void AddReferencies(string refAssetPath)
        {
            m_Referencies.Add(refAssetPath);
        }

        internal void AddBundleNames(string bundleName)
        {
            if (!m_BundleNames.Contains(bundleName))
                m_BundleNames.Add(bundleName);
        }

        #endregion

        #region Virtual Methods

        internal virtual void Record(string assetPath, EAssetType assetType)
        {
            this.m_AssetType = assetType;
            this.m_AssetPath = assetPath;
            this.m_AssetMD5 = AssetDetectionUtility.GetMD5HashFromFile(m_AssetPath);
            this.m_AssetMetaPath = assetPath + ".meta";
            if (!File.Exists(this.m_AssetMetaPath))
            {
                this.m_AssetMetaPath = "";
                this.m_AssetMetaMD5 = "";
            }
            else
                this.m_AssetMetaMD5 = AssetDetectionUtility.GetMD5HashFromFile(m_AssetMetaPath);

            this.m_FileDiskSize = AssetDetectionUtility.GetFileDiskSize(m_AssetPath);
            this.m_BaseImporter = AssetImporter.GetAtPath(assetPath);
            this.m_DirectDepenpendices = AssetDetectionUtility.GetDependencies(assetPath, false);

            CollectBundleName();
        }

        /// <summary>
        /// AssetType|AssetPath#AssetbundleName1#AssetbundleName2#...|AssetMD5|AssetMetaMD5|FileDiskSize|Dep1#Dep2#Dep3#...|Ref1#Ref2#Ref3#...
        /// </summary>
        /// <returns></returns>
        internal virtual string GetOutputStr()
        {
            string splitStr = CHAR_SPLIT_FIRST_FLAG.ToString();

            // AssetPath # AssetbundleName
            string assetInfoStr = "";
            foreach (var bundleName in m_BundleNames)
                assetInfoStr += bundleName + CHAR_SPLIT_SECOND_FLAG;
            if (!string.IsNullOrEmpty(assetInfoStr))
                assetInfoStr = assetInfoStr.Substring(0, assetInfoStr.Length - 1);

            assetInfoStr = string.IsNullOrEmpty(assetInfoStr) ? this.m_AssetPath : (this.m_AssetPath + CHAR_SPLIT_SECOND_FLAG + assetInfoStr);
                
            // Dep
            string depStr = "";
            foreach (var dep in m_DirectDepenpendices)
                depStr += dep + CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(depStr))
                depStr = depStr.Substring(0, depStr.Length - 1);

            // Ref
            string refStr = "";
            foreach (var r in m_Referencies)
                refStr += r + CHAR_SPLIT_SECOND_FLAG;

            if (!string.IsNullOrEmpty(refStr))
                refStr = refStr.Substring(0, refStr.Length - 1);

            if (m_ReferencedInCodePathList != null)
            {
                string codePathStr = "";
                foreach (var codePath in m_ReferencedInCodePathList)
                    codePathStr += codePath + CHAR_SPLIT_SECOND_FLAG;
                if (!string.IsNullOrEmpty(codePathStr))
                {
                    codePathStr = codePathStr.Substring(0, codePathStr.Length - 1);
                    refStr += CHAR_SPLIT_THIRD_FLAG + codePathStr;
                }
            }


            return (int)m_AssetType + splitStr +
                    assetInfoStr + splitStr + 
                    m_AssetMD5 + splitStr + 
                    m_AssetMetaMD5 + splitStr + 
                    m_FileDiskSize + splitStr +
                    depStr + splitStr +
                    refStr;
        }
        internal virtual void ParseStrLine(string stringLine)
        {
            this.m_SourceDataArr = stringLine.Split(CHAR_SPLIT_FIRST_FLAG);
            if (this.m_SourceDataArr.Length > 6)
            {
                this.m_AssetType = (EAssetType)int.Parse(this.m_SourceDataArr[0]);
                if (string.IsNullOrEmpty(this.m_SourceDataArr[1]))
                {
                    this.m_AssetPath = "";
                }
                else
                {
                    var arr = this.m_SourceDataArr[1].Split(CHAR_SPLIT_SECOND_FLAG);
                    if (arr.Length > 0)
                        this.m_AssetPath = arr[0];
                    if (arr.Length > 1)
                    {
                        this.m_BundleNamesStr = "";
                        for (int i = 1; i < arr.Length; i++)
                        {
                            this.m_BundleNames.Add(arr[i]);
                            this.m_BundleNamesStr += arr[i] + "|";
                        }

                        if (!string.IsNullOrEmpty(this.m_BundleNamesStr))
                            this.m_BundleNamesStr = this.m_BundleNamesStr.Substring(0, this.m_BundleNamesStr.Length - 1);
                    }
                }

                this.m_AssetMD5 = this.m_SourceDataArr[2];
                this.m_AssetMetaMD5 = this.m_SourceDataArr[3];
                this.m_FileDiskSize = long.Parse(this.m_SourceDataArr[4]);
                if (string.IsNullOrEmpty(this.m_SourceDataArr[5]))
                    this.m_DirectDepenpendices = new List<string>();
                else
                    this.m_DirectDepenpendices = new List<string>(this.m_SourceDataArr[5].Split(CHAR_SPLIT_SECOND_FLAG));

                if (string.IsNullOrEmpty(this.m_SourceDataArr[6]))
                {
                    this.m_Referencies = new List<string>();
                    this.m_ReferencedInCodePathList = null;
                }
                else
                {
                    var arr = this.m_SourceDataArr[6].Split(CHAR_SPLIT_THIRD_FLAG);
                    if (arr.Length > 0)
                    {
                        if(string.IsNullOrEmpty(arr[0]))
                            this.m_Referencies = new List<string>();
                        else
                            this.m_Referencies = new List<string>(arr[0].Split(CHAR_SPLIT_SECOND_FLAG));
                    }
                    if (arr.Length > 1)
                    {
                        if (string.IsNullOrEmpty(arr[1]))
                            this.m_ReferencedInCodePathList = new List<string>();
                        else
                            this.m_ReferencedInCodePathList = new List<string>(arr[1].Split(CHAR_SPLIT_SECOND_FLAG));
                    }
                }
            }
        }

        internal virtual void Release()
        {
            this.m_AssetPath = null;
            this.m_AssetMD5 = null;
            this.m_AssetMetaPath = null;
            this.m_AssetMetaMD5 = null;
            this.m_DirectDepenpendices.Clear();
            this.m_DirectDepenpendices = null;
            this.m_Referencies.Clear();
            this.m_Referencies = null;
            this.m_BuiltinDependencies.Clear();
            this.m_BuiltinDependencies = null;
            this.m_SourceDataArr = null;
            this.m_BundleNames.Clear();
            this.m_BundleNames = null;

            this.m_BaseImporter = null;
        }

        #endregion
    }
}