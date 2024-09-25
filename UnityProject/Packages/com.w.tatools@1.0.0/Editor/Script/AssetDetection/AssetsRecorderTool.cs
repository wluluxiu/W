using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace jj.TATools.Editor
{
    internal class AssetsRecorderTool
    {
        #region Fields

        const string EDITOR_FOLDER = "/Editor/";
        const string PREFAB_FILE_EXTENSION = ".prefab";

        const string ASSET_RECORDING_DATA_FOLDER = "AssetsRecordingData";
        const string ASSET_RECORDING_CACHE_0 = "AssetsData";
        const string ASSET_RECORDING_CACHE_1 = ".cache";

        #endregion

        #region Internal Methods

        internal static string DoAssetsRecordData(string moduleFolder, string tick)
        {
            string outputBaseFolder = GetOutputBaseFoler(ASSET_RECORDING_DATA_FOLDER,false) + tick + "\\";
            if (!Directory.Exists(outputBaseFolder)) Directory.CreateDirectory(outputBaseFolder);
            var dInfo = new DirectoryInfo(moduleFolder);
            outputBaseFolder += dInfo.Name + "\\";
            if (!Directory.Exists(outputBaseFolder)) Directory.CreateDirectory(outputBaseFolder);
            var fileName = string.Format("{0}_{1}{2}", dInfo.Name, ASSET_RECORDING_CACHE_0,ASSET_RECORDING_CACHE_1);
            var recordingCacheFile = outputBaseFolder + fileName;

            var guids = AssetDatabase.FindAssets("", new string[] { moduleFolder });
            if (guids == null || guids.Length == 0) return null;

            // Record Assets ////////////////////////////////////////////////
            int stepAmount = ToolsConfig.Instance.m_RecorderMaxStepAmount;
            int stepIndex = 0;
            int stepID = 0;

            Dictionary<string, BaseRecorder> recordersMapping = new Dictionary<string, BaseRecorder>();
            System.Type foderType = typeof(UnityEditor.DefaultAsset);
            List<string> pathList = new List<string>();
            int len = guids.Length;
            for (int i = 0; i < len; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                if (assetPath.Contains(EDITOR_FOLDER)) continue;

                bool result = EditorUtility.DisplayCancelableProgressBar("Assets Recording[" + stepID + "|" + stepIndex + "|" + stepAmount + "]", "Record..." + assetPath, (stepIndex + 1) * 1.0f / stepAmount);
                if (result)
                {
                    EditorUtility.ClearProgressBar();
                    return null;
                }
                var assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                if (assetType == foderType) continue;
                if (pathList.Contains(assetPath)) continue;
                pathList.Add(assetPath);

                var recorder = RegisterRecorder(assetPath, assetType);

                recordersMapping.Add(recorder.m_AssetPath, recorder);

                stepIndex++;
                if (stepIndex == stepAmount)
                {
                    stepID++;
                    stepIndex = 0;
                    OutputRecorders(recordersMapping, recordingCacheFile, true);
                }
            }
            EditorUtility.ClearProgressBar();

            if (stepIndex < stepAmount && recordersMapping.Count > 0)
                OutputRecorders(recordersMapping, recordingCacheFile, true);

            DoAssetsRecordCollectReferencies(recordingCacheFile, moduleFolder);

            //FreeMemory();

            return recordingCacheFile;
        }

        #endregion

        #region Local Methods

        static void OutputRecorders(Dictionary<string, BaseRecorder> recorders, string filePath, bool append)
        {
            int count = recorders.Count;
            int index = 0;
            string outputStr = "";
            foreach (var path in recorders.Keys)
            {
                var recorder = recorders[path];

                EditorUtility.DisplayProgressBar("Output Asset Info[" + index + "|" + count + "]", "Record..." + recorder.m_AssetPath, (index + 1) * 1.0f / count);

                outputStr += recorder.GetOutputStr() + "\n";

                recorder.Release();
                recorder = null;

                index++;
            }
            recorders.Clear();
            EditorUtility.ClearProgressBar();

            StreamWriter sw = new StreamWriter(filePath, append, System.Text.Encoding.UTF8);
            sw.Write(outputStr);
            sw.Flush();
            sw.Close();
            sw.Dispose();

            FreeMemory();
        }

        static void CollectRecorderReferencies(Dictionary<string, BaseRecorder> recorders)
        {
            int count = recorders.Count;
            int index = 0;
            foreach (var recorder in recorders.Values)
            {
                EditorUtility.DisplayProgressBar("Collect Asset Referencies[" + index + "|" + count + "]", "Collect..." + recorder.m_AssetPath, (index + 1) * 1.0f / count);
                index++;

                int depCount = recorder.m_DirectDepenpendices.Count;
                for (int k = 0; k < depCount; k++)
                {
                    var assetPath = recorder.m_DirectDepenpendices[k];
                    BaseRecorder depRecorder = null;
                    if (recorders.TryGetValue(assetPath, out depRecorder))
                    {
                        depRecorder.AddReferencies(recorder.m_AssetPath);
                    }
                }
            }

            EditorUtility.ClearProgressBar();
        }

        static void DoCheckReferencedInCode(Dictionary<string, BaseRecorder> recorders,string moduleFolder)
        {
            var guids = AssetDatabase.FindAssets("t:Script", new string[] { moduleFolder });
            var scriptsMapping = new Dictionary<string, string>();
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                scriptsMapping[assetPath] = File.ReadAllText(assetPath);
            }

            int count = recorders.Count;
            int index = 0;
            foreach (var recorder in recorders.Values)
            {
                EditorUtility.DisplayProgressBar("Do Check Referenced In Code[" + index + "|" + count + "]", "Check..." + recorder.m_AssetPath, (index + 1) * 1.0f / count);
                index++;

                recorder.DoCheckReferencedInCode(scriptsMapping);
            }

            EditorUtility.ClearProgressBar();
        }

        //static List<string> CollectBundles(BaseRecorder recorder, Dictionary<string, BaseRecorder> totalRecorders)
        //{
        //    var refCount = recorder.m_Referencies.Count;
        //    if (refCount == 0) return null;
        //    List<string> bundleNames = new List<string>();
        //    for (int k = 0; k < refCount; k++)
        //    {
        //        BaseRecorder refRecorder = null;
        //        if (totalRecorders.TryGetValue(recorder.m_Referencies[k], out refRecorder))
        //        {
        //            if (refRecorder.m_BundleNames.Count > 0)
        //            {
        //                foreach (var bundleName in refRecorder.m_BundleNames)
        //                {
        //                    if (!bundleNames.Contains(bundleName))
        //                        bundleNames.Add(bundleName);
        //                }
        //            }
        //            else
        //            {
        //                var refBundleNames = CollectBundles(refRecorder, totalRecorders);
        //                if (refBundleNames != null && refBundleNames.Count > 0)
        //                {
        //                    foreach (var bundleName in refBundleNames)
        //                    {
        //                        if (!bundleNames.Contains(bundleName))
        //                            bundleNames.Add(bundleName);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return bundleNames;
        //}

        //static void CollectBundles(Dictionary<string, BaseRecorder> totalRecorders, List<BaseRecorder> noBundleNameRecorders)
        //{
        //    int count = noBundleNameRecorders.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        var recorder = noBundleNameRecorders[i];
        //        EditorUtility.DisplayProgressBar("Collect Asset Bundle Info[" + i + "|" + count + "]", "Collect..." + recorder.m_AssetPath, (i + 1) * 1.0f / count);

        //        var bundleNames = CollectBundles(recorder, totalRecorders);
        //        if (bundleNames != null && bundleNames.Count > 0)
        //        {
        //            foreach (var bundleName in bundleNames)
        //                recorder.AddBundleNames(bundleName);
        //        }
        //    }

        //    EditorUtility.ClearProgressBar();
        //}

        static void DoAssetsRecordCollectReferencies(string recordingCacheFile,string moduleFolder)
        {
            Dictionary<string, BaseRecorder> cacheRecorders = new Dictionary<string, BaseRecorder>();
            List<BaseRecorder> noBundleNameRecorders = new List<BaseRecorder>();
            BaseRecorder.GetAllRecorders(recordingCacheFile, (BaseRecorder recorder) =>
            {
                cacheRecorders.Add(recorder.m_AssetPath, recorder);

                if (recorder.m_BundleNames.Count == 0)
                    noBundleNameRecorders.Add(recorder);
            });

            CollectRecorderReferencies(cacheRecorders);

            DoCheckReferencedInCode(cacheRecorders, moduleFolder);

            //CollectBundles(cacheRecorders, noBundleNameRecorders);

            OutputRecorders(cacheRecorders, recordingCacheFile, false);
        }

        static void FreeMemory()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.UnloadUnusedAssetsImmediate();
            System.GC.Collect();
        }

        static string GetOutputBaseFoler(string folderName, bool deleteExistsFiles = true)
        {
            string outputFolder = AppConfigHelper.ASSET_TEMP_CACHE_FLODER + "\\" + folderName + "\\";
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
            else
            {
                if (deleteExistsFiles)
                {
                    var oldFiles = Directory.GetFiles(outputFolder, "*.*", SearchOption.AllDirectories);
                    foreach (var oldFile in oldFiles)
                    {
                        File.Delete(oldFile);
                    }
                }
            }

            return outputFolder;
        }

        static BaseRecorder RegisterRecorder(string assetPath, System.Type type)
        {
            string fileExtension = Path.GetExtension(assetPath);
            EAssetType assetType = EAssetType.Other;
            if (PREFAB_FILE_EXTENSION == fileExtension) assetType = EAssetType.Prefab;
            else
            {
                if (type == typeof(Texture2D)) assetType = EAssetType.Texture;
                else if (type == typeof(Cubemap)) assetType = EAssetType.Texture;
                else if (type == typeof(UnityEditor.MonoScript)) assetType = EAssetType.Script;
                else if (type == typeof(Shader)) assetType = EAssetType.Shader;
                else if (type == typeof(UnityEditor.SceneAsset)) assetType = EAssetType.Scene;
                else if (type == typeof(Material)) assetType = EAssetType.Material;
                else if (type == typeof(RenderTexture)) assetType = EAssetType.RenderTexture;
                else if (type == typeof(AudioClip)) assetType = EAssetType.AudioClip;
                else if (type == typeof(UnityEngine.Video.VideoClip)) assetType = EAssetType.VideoClip;
                else if (type == typeof(Mesh)) assetType = EAssetType.Mesh;
                else if (type == typeof(AnimationClip)) assetType = EAssetType.AnimationClip;
                else if (type == typeof(GameObject)) assetType = EAssetType.Model;
                else if (type == typeof(UnityEngine.U2D.SpriteAtlas)) assetType = EAssetType.SpriteAtlas;
                else if (type == typeof(UnityEditor.Animations.AnimatorController)) assetType = EAssetType.AnimatorController;
                else assetType = EAssetType.Other;
            }

            BaseRecorder recorder = BaseRecorder.CreateRecorder(assetType);
            recorder.Record(assetPath, assetType);

            return recorder;
        }

        #endregion
    }
}