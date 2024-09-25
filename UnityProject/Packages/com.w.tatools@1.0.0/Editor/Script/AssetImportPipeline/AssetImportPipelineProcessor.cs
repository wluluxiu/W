namespace jj.TATools.Editor
{
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEditor.U2D;
    using UnityEditor.VersionControl;
    using UnityEngine;
    using UnityEngine.U2D;

    internal enum ESpecialRuleType
    {
        [PipelineEnumAttribute("默认规则")]
        _0_Default = 0,
        [PipelineEnumAttribute("Part Parent路径规则")]
        _1_AssetParentPartFolder = 1,
        [PipelineEnumAttribute("Full Parent路径规则")]
        _2_AssetParentFullFolder = 2,
        [PipelineEnumAttribute("资源名规则")]
        _3_AssetName = 3,
        [PipelineEnumAttribute("资源路径规则")]
        _4_AssetPath = 4
    }

    internal enum PropertySetMode
    { 
        Once = 0,
        Force,
        Ignore
    }

#if !ASSET_IMPORT_PIPELINE_OFF
    internal class AssetImportPipelineProcessor : AssetPostprocessor
    {
        #region Fields

        static bool m_StartAudioProcessor = false;
        static Dictionary<string, AudioClipImportPipeline> m_AudioClipImportSubPipelineMapping = null;
        static bool m_StartTextureProcessor = false;
        static Dictionary<string, TextureImportPipeline> m_TextureImportSubPipelineMapping = null;
        static bool m_StartModelProcessor = false;
        static Dictionary<string, ModelImportPipeline> m_ModelImportSubPipelineMapping = null;

        #endregion

        #region  AssetPostprocessor Methods

        /// <summary>
        /// Smaller priorities will be imported first. The GetPostprocessOrder function does not affect the order of OnPostprocessAllAssets calls.
        /// </summary>
        /// <returns></returns>
        public override int GetPostprocessOrder()
        {
            return 100; // Default Post Processor Order : 0
        }
		
        /// <summary>
        /// Add this function to a subclass to get a notification just before a model (.fbx, .mb file etc.) is imported.
        /// This lets you control the import settings through code.
        /// </summary>
        void OnPreprocessModel()
        {
            EnableModelImportPipeline(true);

            var mPipeline = GetModelImportPipelineByAssetPath(assetImporter.assetPath);
            if (mPipeline != null)
                mPipeline.DoPreImport(assetImporter as ModelImporter);
        }

        /// <summary>
        /// Add this function to a subclass to get a notification just before the texture importer is run.
        /// This lets you set up default values for the import settings.
        /// </summary>
        void OnPreprocessTexture()
        {
            EnableTextureImportPipeline(true);

            var tPipeline = GetTextureImportPipelineByAssetPath(assetImporter.assetPath);
            if (tPipeline != null)
                tPipeline.DoPreImport(assetImporter as TextureImporter);
        }

        /// <summary>
        /// Add this function to a subclass to get a notification just before an audio clip is being imported.
        /// This lets you control the import settings trough code.
        /// </summary>
        void OnPreprocessAudio()
        {
            EnableAudioClipImportPipeline(true);

            var aPipeline = GetAudioClipImportPipelineByAssetPath(assetImporter.assetPath);
            if (aPipeline != null)
                aPipeline.DoPreImport(assetImporter as AudioImporter);
        }

        /// <summary>
        /// This is called after importing of any number of assets is complete (when the Assets progress bar has reached the end).
        /// </summary>
        /// <param name="importedAssets"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        /// <param name="didDomainReload"></param>
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            EnableAudioClipImportPipeline(false);
            EnableTextureImportPipeline(false);
            EnableModelImportPipeline(false);
        }

        #endregion

        #region Local Static Methods

        static void EnableAudioClipImportPipeline(bool enabled)
        {
            if (enabled)
            {
                if (!m_StartAudioProcessor)
                {
                    m_StartAudioProcessor = true;
                    m_AudioClipImportSubPipelineMapping = AssetImportPipelineUtility.GetImportModuleSetting<AudioClipImportPipeline>((AudioClipImportPipeline t) => { t.AutoSortSettings(); });
                }
            }
            else
            {
                if (m_StartAudioProcessor)
                {
                    m_StartAudioProcessor = false;
                    m_AudioClipImportSubPipelineMapping = null;
                }
            }
        }

        static void EnableTextureImportPipeline(bool enabled)
        {
            if (enabled)
            {
                if (!m_StartTextureProcessor)
                {
                    m_StartTextureProcessor = true;
                    m_TextureImportSubPipelineMapping = AssetImportPipelineUtility.GetImportModuleSetting<TextureImportPipeline>((TextureImportPipeline t) => { t.AutoSortSettings(); });
                }
            }
            else
            {
                if (m_StartTextureProcessor)
                {
                    m_StartTextureProcessor = false;
                    m_TextureImportSubPipelineMapping = null;
                }
            }
        }

        static void EnableModelImportPipeline(bool enabled)
        {
            if (enabled)
            {
                if (!m_StartModelProcessor)
                {
                    m_StartModelProcessor = true;
                    m_ModelImportSubPipelineMapping = AssetImportPipelineUtility.GetImportModuleSetting<ModelImportPipeline>((ModelImportPipeline t) => { t.AutoSortSettings(); });
                }
            }
            else
            {
                if (m_StartModelProcessor)
                {
                    m_StartModelProcessor = false;
                    m_ModelImportSubPipelineMapping = null;
                }
            }
        }

        static TextureImportPipeline GetTextureImportPipelineByAssetPath(string assetPath)
        {
            TextureImportPipeline tImportPipeline = null;

            if (!m_TextureImportSubPipelineMapping.TryGetValue(assetPath, out tImportPipeline))
            {
                tImportPipeline = null;
            }

            return tImportPipeline;
        }

        static AudioClipImportPipeline GetAudioClipImportPipelineByAssetPath(string assetPath)
        {
            AudioClipImportPipeline aImportPipeline = null;

            if (!m_AudioClipImportSubPipelineMapping.TryGetValue(assetPath, out aImportPipeline))
            {
                aImportPipeline = null;
            }

            return aImportPipeline;
        }

        static ModelImportPipeline GetModelImportPipelineByAssetPath(string assetPath)
        {
            ModelImportPipeline mImportPipeline = null;

            if (!m_ModelImportSubPipelineMapping.TryGetValue(assetPath, out mImportPipeline))
            {
                mImportPipeline = null;
            }

            return mImportPipeline;
        }

        #endregion
    }
#endif
}