namespace jj.TATools.Editor
{
    using System.IO;
    using UnityEngine;
    using UnityEditor;

    [CreateAssetMenu(fileName = "ToolsConfig", menuName = "AssetsTool/ToolsConfig")]
    internal class ToolsConfig : ScriptableObject
    {
        #region Fields

        public string PROJECT_DATA_BASE_FOLDER = "UnityAssetToolData";

        // 资源检测工具 ///////////////
        // Folders
        public string ASSET_CUSTOM_RULE_FOLDER = "自定义规则";
        public string ASSET_WHITELIST_FLODER = "白名单";
        public string ASSET_COMPARE_SOURCE_FLODER = "对比数据";
        public string ASSET_COMPARE_REPORT_FLODER = "资源检测报告";
        // Texture Condition
        public string TEXTURE_FORMAT_THRESHOLD = "ASTC_";
        public int TEXTURE_POT_THRESHOLD = 1;
        public int TEXTURE_MAX_RESOLUTION_THRESHOLD = 2048;
        public int TEXTURE_FILTERMODE_THRESHOLD = 2;
        public int TEXTURE_RW_THRESHOLD = 1;
        public int TEXTURE_ANISOLEVEL_THRESHOLD = 1;
        // Model Condition
        public int MODEL_MAX_BONES_THRESHOLD = 240;
        public string MODEL_UV_CHANNEL_THRESHOLD = "0|0|1|1|1|1|1|1";
        public int MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD = 0;
        public int MODEL_AUTO_GENERATE_UV2_THRESHOLD = 1;
        public int MODEL_ANIM_COMPRESSION_THRESHOLD = 0;
        public int MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD = 50000;
        public int MODEL_VERTEX_MAX_AMOUNT_THRESHOLD = 50000;
        public int MODEL_VERTEX_COLOR_THRESHOLD = 1;
        public int MODEL_RW_THRESHOLD = 1;
        // Collect Data Condition
        public int m_RecorderMaxStepAmount = 50000;
        // Exception Tips
        public string TIPS_TEXTURE_FORMAT = "Format格式错误|包体、内存|Android或IOS平台纹理Format格式设置错误,编写批量工具修复。";
        public string TIPS_TEXTURE_RESOLUTION_NPOT = "NPOT纹理|包体、内存|当前平台纹理为NPOT纹理，若纹理Format不是ASTC格式,压缩失败，需要修改纹理分辨率尺寸变成POT纹理。";
        public string TIPS_TEXTURE_RESOLUTION_SIZE = "尺寸过大|包体、内存|当前平台纹理分辨率尺寸Width或Height超过2048上限。";
        public string TIPS_TEXTURE_FILTERMODE = "FilterMode异常|GPU采样效率|纹理FilterMode为Triliner模式，检查是否有开启必要，如果必要设置为Biliner。(AnisoLevel、Mipmap、Trilinear配套使用)";
        public string TIPS_TEXTURE_ANISOLEVEL = "AnisoLevel异常|GPU采样效率|纹理AnisoLevel等级大于1，检查是否有开启必要，如果必要设置为1。(AnisoLevel、Mipmap、Trilinear配套使用)";
        public string TIPS_TEXTURE_RW = "读写权限开启|内存|纹理开启了读写权限，导致运行时内存占用翻倍,编写批量工具修复。";
        public string TIPS_TEXTURE_NOUSED = "疑似冗余|包体|查看《冗余资源详情报告》排查不是代码中需要动态加载的资源后，备份后移出工程。";
        public string TIPS_MODEL_RW = "读写权限开启|内存|模型开启了读写权限，导致运行时内存占用翻倍,编写批量工具修复。";

        public string TIPS_MODEL_MESH_TRIANGLE_LIMIT = "模型面数异常|包体、内存、GPU|模型总面数超过50000上限，需要减面优化。";
        public string TIPS_MODEL_MESH_VERTEX_LIMIT = "模型顶点数异常|包体、内存、GPU|模型总顶点数超过50000上限，需要减面优化。";
        public string TIPS_MODEL_MESH_BLENDSHAPE_LIMIT = "模型BlendShape异常|包体、内存|模型Mesh数据含有BlendShape数据，检查是否需要。";
        public string TIPS_MODEL_MESH_VC_LIMIT = "模型顶点色异常|包体、内存|模型Mesh数据含有顶点色数据，检查是否需要。([ProjectSettings/OptimizeMeshData]勾选则包体和内存不会冗余，但要提醒美术按标准规范制作资源，防止'埋坑')";
        public string TIPS_MODEL_MESH_UV_NOUSED = "模型UV异常|包体、内存|模型Mesh数据含有冗余uv数据，检查是否需要。([ProjectSettings/OptimizeMeshData]勾选则包体和内存不会冗余，但要提醒美术按标准规范制作资源，防止'埋坑')";
        public string TIPS_MODEL_MESH_UV2_AUTO = "UV2自动生成|包体、内存|模型勾选了自动生成uv2设置，检查是否需要。([ProjectSettings/OptimizeMeshData]勾选则包体和内存不会冗余，但要提醒美术按标准规范制作资源，防止'埋坑')";
        public string TIPS_MODEL_BONE_LIMIT = "骨骼数异常|CPU|模型总骨骼数超过240上限，需要优化。";
        public string TIPS_MODEL_ANIM_COMPRESSION = "动画压缩异常|包体、内存、CPU|模型导入的动画文件没有进行压缩设置，需要优化。";
        public string TIPS_MODEL_NOUSED = "疑似冗余|包体|查看《冗余资源详情报告》排查不是代码中需要动态加载的资源后，备份后移出工程。";

        public string TIPS_MATERIAL_MISS_SHADER = "引用Shader丢失|包体、内存、美术效果|查看《材质异常详情报告》，根据丢失的Shader guid还原Shader引用。";
        public string TIPS_MATERIAL_BUILTIN_REF = "内置资源引用|包体、内存|查看《材质异常详情报告》，去除内置引用。";
        public string TIPS_MATERIAL_NO_USED_DATA = "数据冗余|包体、内存|多次切换shader导致冗余Property数据残留，写工具一键清除。";
        public string TIPS_MATERIAL_NO_USED = "疑似冗余|包体|查看《冗余资源详情报告》排查不是代码中需要动态加载的资源后，备份后移出工程。";

        public string TIPS_SHADER_DEP = "默认引用|包体、内存|查看《Shader详情报告》，去除不必要的默认引用。";
        public string TIPS_SHADER_NO_USED = "疑似冗余|包体|查看《冗余资源详情报告》排查不是代码中需要动态加载的资源后，备份后移出工程。";

        public string TIPS_PREFAB_MISS_SCRIPT = "脚本丢失|逻辑功能|查看《Prefab异常详情报告》，修复对应节点上丢失组件。";
        public string TIPS_PREFAB_COLLIDER = "Collider|CPU|查看《Prefab异常详情报告》，删除对应节点上Collider组件。";
        public string TIPS_PREFAB_BUILTIN_REF = "内置资源引用|包体、内存|查看《Prefab异常详情报告》，去除内置引用。";
        public string TIPS_PREFAB_MISS_MAT = "材质丢失|包体、内存、美术效果|查看《Prefab异常详情报告》，修复对应节点上丢失材质。";

        public string TIPS_SCENE_MISS_SCRIPT = "脚本丢失|逻辑功能|查看《Scene异常详情报告》，修复对应节点上丢失组件。";
        public string TIPS_SCENE_COLLIDER = "Collider|CPU|查看《Scene异常详情报告》，删除对应节点上Collider组件。";
        public string TIPS_SCENE_BUILTIN_REF = "内置资源引用|包体、内存|查看《Scene异常详情报告》，去除内置引用。";

        public string TIPS_SCRIPT_DEP = "默认引用|包体、内存|查看《脚本异常引用详情报告》，去除不必要的默认引用。";

        public string TIPS_ANIMATORCONTROLLER_NOUSED = "疑似冗余|包体|查看《冗余资源详情报告》排查不是代码中需要动态加载的资源后，备份后移出工程。";

        public string TIPS_AUDIO_VIDEO_NOUSED = "疑似冗余|包体|查看《冗余资源详情报告》排查不是代码中需要动态加载的资源后，备份后移出工程。";

        public string TIPS_REPEAT_FILES = "重复资源|包体、内存|查看《重复资源详情报告》，根据业务需求进行修复。";

        public string TIPS_RESOURCES_FOLDER = "Resources目录资源|包体、内存|查看《Resources目录资源详情报告》，根据业务需求判断是否修改路径。";

        public string TIPS_ASSETBUNDLE_STRATEGY = "分包策略异常|包体、内存|资源被打入多个Assetbundle中，查看《Assetbundle分包策略异常详情报告》，修改资源分包策略。";
        public string TIPS_ASSETBUNDLE_BUILTIN = "内置资源异常|包体、内存|内置资源被打入Assetbundle中，查看《Assetbunle内置资源详情报告》，进行内置资源本地化进行打包、Standard Shader变体剔除等操作。";

        // 纹理检测工具 ///////////////
        public string TEXTURE_COMPARE_REPORT_FOLDER = "纹理资源检测报告";
        public string TEXTURE_FILE_EXTENSION = "tga|png|bmp|jpg";

        // 图集检测工具 ///////////////
        public string SPRITEATLAS_SOURCE_DATA_FOLDER = "SpirteAtlas检测报告";
        public string SPRITEATLAS_FORMAT_THRESHOLD = "ASTC_";
        public int SPRITEATLAS_ANISOLEVEL_THRESHOLD = 1;
        public float SPRITEATLAS_MEMORY_OVER_THRESHOLD = 0.2f;
        public float SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD = 0.5f;
        public int SPRITEATLAS_MAX_RESOLUTION_THRESHOLD = 2048;
        public int SPRITEATLAS_FILTERMODE_THRESHOLD = 2;
        public int SPRITEATLAS_RW_THRESHOLD = 1;
        public int SPRITEATLAS_MIPMAP_THRESHOLD = 1;


        static ToolsConfig m_Instance = null;
        internal static ToolsConfig Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = LoadConfig();
                }

                return m_Instance;
            }
        }

        #endregion

        #region Local Methods

        static ToolsConfig LoadConfig()
        {
            ToolsConfig config = null;

            var ms = MonoScript.FromScriptableObject(new ToolsConfig());
            var scriptFilePath = AssetDatabase.GetAssetPath(ms);
            var scriptDirectoryPath = Path.GetDirectoryName(scriptFilePath);
            var typeName = typeof(ToolsConfig).Name;
            var guids = AssetDatabase.FindAssets("t:" + typeName, new string[] { scriptDirectoryPath });
            if (guids.Length == 0)
            {
                config = ScriptableObject.CreateInstance<ToolsConfig>();

                AssetDatabase.CreateAsset(config, scriptDirectoryPath + "\\" + typeName + ".asset");
            }
            else
            {
                foreach (var guid in guids)
                {
                    config = AssetDatabase.LoadAssetAtPath<ToolsConfig>(AssetDatabase.GUIDToAssetPath(guid));
                    break;
                }
            }

            return config;
        }

        #endregion
    }

    [CustomEditor(typeof(ToolsConfig))]
    internal class ToolsConfigInspector : Editor
    {
        #region Fields

        private GUIContent PROJECT_DATA_BASE_FOLDER_CONTENT = new GUIContent("根目录:");
        private SerializedProperty PROJECT_DATA_BASE_FOLDER;

        private string[] m_ToolTypeNames = new string[] {"资源检测工具","图集检测工具", "纹理检测工具" };
        private int m_ToolIndex = 0;

        // 资源检测工具 /////////////////
        //Folders
        private bool m_FoldersFoldout = false;
        private GUIContent ASSET_CUSTOM_RULE_FOLDER_CONTENT = new GUIContent("自定义规则配置目录:");
        private SerializedProperty ASSET_CUSTOM_RULE_FOLDER;
        private GUIContent ASSET_WHITELIST_FLODER_CONTENT = new GUIContent("白名单配置目录:");
        private SerializedProperty ASSET_WHITELIST_FLODER;
        private GUIContent ASSET_COMPARE_SOURCE_FLODER_CONTENT = new GUIContent("对比数据目录:");
        private SerializedProperty ASSET_COMPARE_SOURCE_FLODER;
        private GUIContent ASSET_COMPARE_REPORT_FLODER_CONTENT = new GUIContent("资源检测报告目录:");
        private SerializedProperty ASSET_COMPARE_REPORT_FLODER;
        // Texture Condition
        private bool m_AssetTextureConditionFoldout = false;
        private GUIContent TEXTURE_FORMAT_THRESHOLD_CONTENT = new GUIContent("纹理Format检测项:");
        private SerializedProperty TEXTURE_FORMAT_THRESHOLD;
        private GUIContent TEXTURE_POT_THRESHOLD_CONTENT = new GUIContent("纹理POT检测:");
        private SerializedProperty TEXTURE_POT_THRESHOLD;
        private GUIContent TEXTURE_MAX_RESOLUTION_THRESHOLD_CONTENT = new GUIContent("纹理最大分辨率:");
        private SerializedProperty TEXTURE_MAX_RESOLUTION_THRESHOLD;
        private GUIContent TEXTURE_FILTERMODE_THRESHOLD_CONTENT = new GUIContent("纹理非法滤波模式:");
        private SerializedProperty TEXTURE_FILTERMODE_THRESHOLD;
        private GUIContent TEXTURE_RW_THRESHOLD_CONTENT = new GUIContent("纹理读写权限检测:");
        private SerializedProperty TEXTURE_RW_THRESHOLD;
        private GUIContent TEXTURE_ANISOLEVEL_THRESHOLD_CONTENT = new GUIContent("纹理AnisoLevel最小阈值:");
        private SerializedProperty TEXTURE_ANISOLEVEL_THRESHOLD;
        // Model Condition
        private bool m_AssetModelConditionFoldout = false;
        private GUIContent MODEL_MAX_BONES_THRESHOLD_CONTENT = new GUIContent("模型最大骨骼数:");
        private SerializedProperty MODEL_MAX_BONES_THRESHOLD;
        private GUIContent MODEL_UV_CHANNEL_THRESHOLD_CONTENT = new GUIContent("模型uv通道检测:");
        private SerializedProperty MODEL_UV_CHANNEL_THRESHOLD;
        private GUIContent MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD_CONTENT = new GUIContent("BlendShape最大值:");
        private SerializedProperty MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD;
        private GUIContent MODEL_AUTO_GENERATE_UV2_THRESHOLD_CONTENT = new GUIContent("模型自动开启UV2生成检测:");
        private SerializedProperty MODEL_AUTO_GENERATE_UV2_THRESHOLD;
        private GUIContent MODEL_ANIM_COMPRESSION_THRESHOLD_CONTENT = new GUIContent("模型动画压缩检测:");
        private SerializedProperty MODEL_ANIM_COMPRESSION_THRESHOLD;
        private GUIContent MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD_CONTENT = new GUIContent("模型最大面数:");
        private SerializedProperty MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD;
        private GUIContent MODEL_VERTEX_MAX_AMOUNT_THRESHOLD_CONTENT = new GUIContent("模型最大顶点数:");
        private SerializedProperty MODEL_VERTEX_MAX_AMOUNT_THRESHOLD;
        private GUIContent MODEL_VERTEX_COLOR_THRESHOLD_CONTENT = new GUIContent("模型顶点色检测:");
        private SerializedProperty MODEL_VERTEX_COLOR_THRESHOLD;
        private GUIContent MODEL_RW_THRESHOLD_CONTENT = new GUIContent("模型读写权限检测:");
        private SerializedProperty MODEL_RW_THRESHOLD;
        // Collect Data Condition
        private bool m_AssetCollectDataConditionFoldout = false;
        private GUIContent RecorderMaxStepAmount = new GUIContent("单次最大采集数量:");
        private SerializedProperty m_RecorderMaxStepAmount;
        // Exception Tips
        private bool m_AssetExceptionTipsFoldout = false;
        private GUIContent TIPS_TEXTURE_FORMAT_CONTENT = new GUIContent("纹理Format异常:");
        private SerializedProperty TIPS_TEXTURE_FORMAT;
        private GUIContent TIPS_TEXTURE_RESOLUTION_NPOT_CONTENT = new GUIContent("纹理NPOT异常:");
        private SerializedProperty TIPS_TEXTURE_RESOLUTION_NPOT;
        private GUIContent TIPS_TEXTURE_RESOLUTION_SIZE_CONTENT = new GUIContent("纹理最大分辨率异常:");
        private SerializedProperty TIPS_TEXTURE_RESOLUTION_SIZE;
        private GUIContent TIPS_TEXTURE_FILTERMODE_CONTENT = new GUIContent("纹理FilterMode异常:");
        private SerializedProperty TIPS_TEXTURE_FILTERMODE;
        private GUIContent TIPS_TEXTURE_ANISOLEVEL_CONTENT = new GUIContent("纹理AnisoLevel异常:");
        private SerializedProperty TIPS_TEXTURE_ANISOLEVEL;
        private GUIContent TIPS_TEXTURE_RW_CONTENT = new GUIContent("纹理读写权限异常:");
        private SerializedProperty TIPS_TEXTURE_RW;
        private GUIContent TIPS_TEXTURE_NOUSED_CONTENT = new GUIContent("纹理冗余异常:");
        private SerializedProperty TIPS_TEXTURE_NOUSED;

        private GUIContent TIPS_MODEL_RW_CONTENT = new GUIContent("模型读写权限异常:");
        private SerializedProperty TIPS_MODEL_RW;
        private GUIContent TIPS_MODEL_MESH_TRIANGLE_LIMIT_CONTENT = new GUIContent("模型面数异常:");
        private SerializedProperty TIPS_MODEL_MESH_TRIANGLE_LIMIT;
        private GUIContent TIPS_MODEL_MESH_VERTEX_LIMIT_CONTENT = new GUIContent("模型顶点数异常:");
        private SerializedProperty TIPS_MODEL_MESH_VERTEX_LIMIT;
        private GUIContent TIPS_MODEL_MESH_BLENDSHAPE_LIMIT_CONTENT = new GUIContent("模型BlendShape异常:");
        private SerializedProperty TIPS_MODEL_MESH_BLENDSHAPE_LIMIT;
        private GUIContent TIPS_MODEL_MESH_VC_LIMIT_CONTENT = new GUIContent("模型顶点色异常:");
        private SerializedProperty TIPS_MODEL_MESH_VC_LIMIT;
        private GUIContent TIPS_MODEL_MESH_UV_NOUSED_CONTENT = new GUIContent("模型冗余uv异常:");
        private SerializedProperty TIPS_MODEL_MESH_UV_NOUSED;
        private GUIContent TIPS_MODEL_MESH_UV2_AUTO_CONTENT = new GUIContent("模型自动生成uv2异常:");
        private SerializedProperty TIPS_MODEL_MESH_UV2_AUTO;
        private GUIContent TIPS_MODEL_BONE_LIMIT_CONTENT = new GUIContent("模型骨骼数异常:");
        private SerializedProperty TIPS_MODEL_BONE_LIMIT;
        private GUIContent TIPS_MODEL_ANIM_COMPRESSION_CONTENT = new GUIContent("模型动画压缩异常:");
        private SerializedProperty TIPS_MODEL_ANIM_COMPRESSION;
        private GUIContent TIPS_MODEL_NOUSED_CONTENT = new GUIContent("模型冗余异常:");
        private SerializedProperty TIPS_MODEL_NOUSED;

        private GUIContent TIPS_MATERIAL_MISS_SHADER_CONTENT = new GUIContent("材质丢失Shader异常:");
        private SerializedProperty TIPS_MATERIAL_MISS_SHADER;
        private GUIContent TIPS_MATERIAL_BUILTIN_REF_CONTENT = new GUIContent("材质内置引用异常:");
        private SerializedProperty TIPS_MATERIAL_BUILTIN_REF;
        private GUIContent TIPS_MATERIAL_NO_USED_DATA_CONTENT = new GUIContent("材质冗余数据异常:");
        private SerializedProperty TIPS_MATERIAL_NO_USED_DATA;
        private GUIContent TIPS_MATERIAL_NO_USED_CONTENT = new GUIContent("材质冗余异常:");
        private SerializedProperty TIPS_MATERIAL_NO_USED;

        private GUIContent TIPS_SHADER_DEP_CONTENT = new GUIContent("Shader默认引用异常:");
        private SerializedProperty TIPS_SHADER_DEP;
        private GUIContent TIPS_SHADER_NO_USED_CONTENT = new GUIContent("Shader冗余异常:");
        private SerializedProperty TIPS_SHADER_NO_USED;

        private GUIContent TIPS_PREFAB_MISS_SCRIPT_CONTENT = new GUIContent("Prefab丢失脚本异常:");
        private SerializedProperty TIPS_PREFAB_MISS_SCRIPT;
        private GUIContent TIPS_PREFAB_COLLIDER_CONTENT = new GUIContent("Prefab-Collider异常:");
        private SerializedProperty TIPS_PREFAB_COLLIDER;
        private GUIContent TIPS_PREFAB_BUILTIN_REF_CONTENT = new GUIContent("Prefab内置引用异常:");
        private SerializedProperty TIPS_PREFAB_BUILTIN_REF;
        private GUIContent TIPS_PREFAB_MISS_MAT_CONTENT = new GUIContent("Prefab丢失材质异常:");
        private SerializedProperty TIPS_PREFAB_MISS_MAT;

        private GUIContent TIPS_SCENE_MISS_SCRIPT_CONTENT = new GUIContent("场景丢失脚本异常:");
        private SerializedProperty TIPS_SCENE_MISS_SCRIPT;
        private GUIContent TIPS_SCENE_COLLIDER_CONTENT = new GUIContent("场景Collider异常:");
        private SerializedProperty TIPS_SCENE_COLLIDER;
        private GUIContent TIPS_SCENE_BUILTIN_REF_CONTENT = new GUIContent("场景内置引用异常:");
        private SerializedProperty TIPS_SCENE_BUILTIN_REF;

        private GUIContent TIPS_SCRIPT_DEP_CONTENT = new GUIContent("脚本默认引用异常:");
        private SerializedProperty TIPS_SCRIPT_DEP;

        private GUIContent TIPS_ANIMATORCONTROLLER_NOUSED_CONTENT = new GUIContent("AnimatorController冗余异常:");
        private SerializedProperty TIPS_ANIMATORCONTROLLER_NOUSED;

        private GUIContent TIPS_AUDIO_VIDEO_NOUSED_CONTENT = new GUIContent("音频冗余异常:");
        private SerializedProperty TIPS_AUDIO_VIDEO_NOUSED;

        private GUIContent TIPS_REPEAT_FILES_CONTENT = new GUIContent("重复异常:");
        private SerializedProperty TIPS_REPEAT_FILES;

        private GUIContent TIPS_RESOURCES_FOLDER_CONTENT = new GUIContent("Resources目录异常:");
        private SerializedProperty TIPS_RESOURCES_FOLDER;

        private GUIContent TIPS_ASSETBUNDLE_STRATEGY_CONTENT = new GUIContent("Assetbundle分包策略异常:");
        private SerializedProperty TIPS_ASSETBUNDLE_STRATEGY;
        private GUIContent TIPS_ASSETBUNDLE_BUILTIN_CONTENT = new GUIContent("Assetbundle内置资源异常:");
        private SerializedProperty TIPS_ASSETBUNDLE_BUILTIN;

        // 纹理检测工具 ///////////////
        private GUIContent TEXTURE_COMPARE_REPORT_FOLDER_CONTENT = new GUIContent("纹理检测报告目录:");
        private SerializedProperty TEXTURE_COMPARE_REPORT_FOLDER;
        private GUIContent TEXTURE_FILE_EXTENSION_CONTENT = new GUIContent("纹理检测后缀:");
        private SerializedProperty TEXTURE_FILE_EXTENSION;

        // 图集检测工具 ///////////////
        private GUIContent SPRITEATLAS_SOURCE_DATA_FOLDER_CONTENT = new GUIContent("图集检测报告目录:");
        private SerializedProperty SPRITEATLAS_SOURCE_DATA_FOLDER;
        private GUIContent SPRITEATLAS_FORMAT_THRESHOLD_CONTENT = new GUIContent("Format检测项:");
        private SerializedProperty SPRITEATLAS_FORMAT_THRESHOLD;
        private GUIContent SPRITEATLAS_ANISOLEVEL_THRESHOLD_CONTENT = new GUIContent("AnisoLevel最小阈值:");
        private SerializedProperty SPRITEATLAS_ANISOLEVEL_THRESHOLD;
        private GUIContent SPRITEATLAS_MEMORY_OVER_THRESHOLD_CONTENT = new GUIContent("图集内存最小溢出比:");
        private SerializedProperty SPRITEATLAS_MEMORY_OVER_THRESHOLD;
        private GUIContent SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD_CONTENT = new GUIContent("图集透明像素比例最小阈值:");
        private SerializedProperty SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD;
        private GUIContent SPRITEATLAS_MAX_RESOLUTION_THRESHOLD_CONTENT = new GUIContent("图集最大分辨率:");
        private SerializedProperty SPRITEATLAS_MAX_RESOLUTION_THRESHOLD;
        private GUIContent SPRITEATLAS_FILTERMODE_THRESHOLD_CONTENT = new GUIContent("图集非法滤波模式:");
        private SerializedProperty SPRITEATLAS_FILTERMODE_THRESHOLD;
        private GUIContent SPRITEATLAS_RW_THRESHOLD_CONTENT = new GUIContent("图集读写权限检测:");
        private SerializedProperty SPRITEATLAS_RW_THRESHOLD;
        private GUIContent SPRITEATLAS_MIPMAP_THRESHOLD_CONTENT = new GUIContent("图集Mipmap检测:");
        private SerializedProperty SPRITEATLAS_MIPMAP_THRESHOLD;

        #endregion

        #region Local Methods

        void AssetToolOnInspecorGUI()
        {
            // Folders ////////////////////////////
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(10);

            m_FoldersFoldout = EditorGUILayout.Foldout(m_FoldersFoldout, "1. Folders");

            EditorGUILayout.EndHorizontal();

            if (m_FoldersFoldout)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(ASSET_CUSTOM_RULE_FOLDER, ASSET_CUSTOM_RULE_FOLDER_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(ASSET_WHITELIST_FLODER, ASSET_WHITELIST_FLODER_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(ASSET_COMPARE_SOURCE_FLODER, ASSET_COMPARE_SOURCE_FLODER_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(ASSET_COMPARE_REPORT_FLODER, ASSET_COMPARE_REPORT_FLODER_CONTENT);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // Texture Condition ////////////////////////////
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(10);

            m_AssetTextureConditionFoldout = EditorGUILayout.Foldout(m_AssetTextureConditionFoldout, "2. Texture Condition");

            EditorGUILayout.EndHorizontal();

            if (m_AssetTextureConditionFoldout)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TEXTURE_FORMAT_THRESHOLD, TEXTURE_FORMAT_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TEXTURE_POT_THRESHOLD, TEXTURE_POT_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TEXTURE_MAX_RESOLUTION_THRESHOLD, TEXTURE_MAX_RESOLUTION_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TEXTURE_FILTERMODE_THRESHOLD, TEXTURE_FILTERMODE_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TEXTURE_RW_THRESHOLD, TEXTURE_RW_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TEXTURE_ANISOLEVEL_THRESHOLD, TEXTURE_ANISOLEVEL_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // Model Condition ////////////////////////////
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(10);

            m_AssetModelConditionFoldout = EditorGUILayout.Foldout(m_AssetModelConditionFoldout, "3. Model Condition");

            EditorGUILayout.EndHorizontal();

            if (m_AssetModelConditionFoldout)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(MODEL_MAX_BONES_THRESHOLD, MODEL_MAX_BONES_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(MODEL_UV_CHANNEL_THRESHOLD, MODEL_UV_CHANNEL_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD, MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(MODEL_AUTO_GENERATE_UV2_THRESHOLD, MODEL_AUTO_GENERATE_UV2_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(MODEL_ANIM_COMPRESSION_THRESHOLD, MODEL_ANIM_COMPRESSION_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD, MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(MODEL_VERTEX_MAX_AMOUNT_THRESHOLD, MODEL_VERTEX_MAX_AMOUNT_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(MODEL_VERTEX_COLOR_THRESHOLD, MODEL_VERTEX_COLOR_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(MODEL_RW_THRESHOLD, MODEL_RW_THRESHOLD_CONTENT);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // Collect Data Condition ////////////////////////////
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(10);

            m_AssetCollectDataConditionFoldout = EditorGUILayout.Foldout(m_AssetCollectDataConditionFoldout, "4. Collect Data Condition");

            EditorGUILayout.EndHorizontal();

            if (m_AssetCollectDataConditionFoldout)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(m_RecorderMaxStepAmount, RecorderMaxStepAmount);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // Exception Tips ////////////////////////////
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(10);

            m_AssetExceptionTipsFoldout = EditorGUILayout.Foldout(m_AssetExceptionTipsFoldout, "5. Exception Tips");

            EditorGUILayout.EndHorizontal();

            if (m_AssetExceptionTipsFoldout)
            {
                // Texture Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("纹理相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_TEXTURE_FORMAT, TIPS_TEXTURE_FORMAT_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_TEXTURE_RESOLUTION_NPOT, TIPS_TEXTURE_RESOLUTION_NPOT_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_TEXTURE_RESOLUTION_SIZE, TIPS_TEXTURE_RESOLUTION_SIZE_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_TEXTURE_FILTERMODE, TIPS_TEXTURE_FILTERMODE_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_TEXTURE_ANISOLEVEL, TIPS_TEXTURE_ANISOLEVEL_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_TEXTURE_RW, TIPS_TEXTURE_RW_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_TEXTURE_NOUSED, TIPS_TEXTURE_NOUSED_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // Model Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("模型相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MODEL_RW, TIPS_MODEL_RW_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MODEL_MESH_TRIANGLE_LIMIT, TIPS_MODEL_MESH_TRIANGLE_LIMIT_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MODEL_MESH_VERTEX_LIMIT, TIPS_MODEL_MESH_VERTEX_LIMIT_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MODEL_MESH_BLENDSHAPE_LIMIT, TIPS_MODEL_MESH_BLENDSHAPE_LIMIT_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MODEL_MESH_VC_LIMIT, TIPS_MODEL_MESH_VC_LIMIT_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MODEL_MESH_UV_NOUSED, TIPS_MODEL_MESH_UV_NOUSED_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MODEL_MESH_UV2_AUTO, TIPS_MODEL_MESH_UV2_AUTO_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MODEL_BONE_LIMIT, TIPS_MODEL_BONE_LIMIT_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MODEL_ANIM_COMPRESSION, TIPS_MODEL_ANIM_COMPRESSION_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MODEL_NOUSED, TIPS_MODEL_NOUSED_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // Material Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("材质相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MATERIAL_MISS_SHADER, TIPS_MATERIAL_MISS_SHADER_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MATERIAL_BUILTIN_REF, TIPS_MATERIAL_BUILTIN_REF_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MATERIAL_NO_USED_DATA, TIPS_MATERIAL_NO_USED_DATA_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_MATERIAL_NO_USED, TIPS_MATERIAL_NO_USED_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // Shder Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("Shader相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_SHADER_DEP, TIPS_SHADER_DEP_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_SHADER_NO_USED, TIPS_SHADER_NO_USED_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // Prefab Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("Prefab相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_PREFAB_MISS_SCRIPT, TIPS_PREFAB_MISS_SCRIPT_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_PREFAB_COLLIDER, TIPS_PREFAB_COLLIDER_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_PREFAB_BUILTIN_REF, TIPS_PREFAB_BUILTIN_REF_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_PREFAB_MISS_MAT, TIPS_PREFAB_MISS_MAT_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // Scene Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("Scene相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_SCENE_MISS_SCRIPT, TIPS_SCENE_MISS_SCRIPT_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_SCENE_COLLIDER, TIPS_SCENE_COLLIDER_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_SCENE_BUILTIN_REF, TIPS_SCENE_BUILTIN_REF_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // Script Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("脚本相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_SCRIPT_DEP, TIPS_SCRIPT_DEP_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // AnimatorController Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("AnimatorController相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_ANIMATORCONTROLLER_NOUSED, TIPS_ANIMATORCONTROLLER_NOUSED_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // Audio & Video Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("音频相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_AUDIO_VIDEO_NOUSED, TIPS_AUDIO_VIDEO_NOUSED_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // Repeat File Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("重复资源相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_REPEAT_FILES, TIPS_REPEAT_FILES_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // Resources Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("Resources目录资源相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_RESOURCES_FOLDER, TIPS_RESOURCES_FOLDER_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                // Assetbundle Tips
                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(5);

                GUI.color = Color.red;

                EditorGUILayout.LabelField("Assetbundle相关:");

                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_ASSETBUNDLE_STRATEGY, TIPS_ASSETBUNDLE_STRATEGY_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.PropertyField(TIPS_ASSETBUNDLE_BUILTIN, TIPS_ASSETBUNDLE_BUILTIN_CONTENT);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }

        void TextureToolOnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(TEXTURE_COMPARE_REPORT_FOLDER, TEXTURE_COMPARE_REPORT_FOLDER_CONTENT);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(TEXTURE_FILE_EXTENSION, TEXTURE_FILE_EXTENSION_CONTENT);

            EditorGUILayout.EndHorizontal();
        }

        void SpriteAtlasToolOnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(SPRITEATLAS_SOURCE_DATA_FOLDER, SPRITEATLAS_SOURCE_DATA_FOLDER_CONTENT);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(SPRITEATLAS_FORMAT_THRESHOLD, SPRITEATLAS_FORMAT_THRESHOLD_CONTENT);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(SPRITEATLAS_ANISOLEVEL_THRESHOLD, SPRITEATLAS_ANISOLEVEL_THRESHOLD_CONTENT);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(SPRITEATLAS_MEMORY_OVER_THRESHOLD, SPRITEATLAS_MEMORY_OVER_THRESHOLD_CONTENT);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD, SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD_CONTENT);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(SPRITEATLAS_MAX_RESOLUTION_THRESHOLD, SPRITEATLAS_MAX_RESOLUTION_THRESHOLD_CONTENT);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(SPRITEATLAS_FILTERMODE_THRESHOLD, SPRITEATLAS_FILTERMODE_THRESHOLD_CONTENT);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(SPRITEATLAS_RW_THRESHOLD, SPRITEATLAS_RW_THRESHOLD_CONTENT);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(SPRITEATLAS_MIPMAP_THRESHOLD, SPRITEATLAS_MIPMAP_THRESHOLD_CONTENT);

            EditorGUILayout.EndHorizontal();
        }

        #endregion


        #region Editor Methods

        private void OnEnable()
        {
            PROJECT_DATA_BASE_FOLDER = serializedObject.FindProperty("PROJECT_DATA_BASE_FOLDER");

            // 资源检测工具 /////////////////////////
            // Folders
            ASSET_CUSTOM_RULE_FOLDER = serializedObject.FindProperty("ASSET_CUSTOM_RULE_FOLDER");
            ASSET_WHITELIST_FLODER = serializedObject.FindProperty("ASSET_WHITELIST_FLODER");
            ASSET_COMPARE_SOURCE_FLODER = serializedObject.FindProperty("ASSET_COMPARE_SOURCE_FLODER");
            ASSET_COMPARE_REPORT_FLODER = serializedObject.FindProperty("ASSET_COMPARE_REPORT_FLODER");
            // Texture Condition
            TEXTURE_FORMAT_THRESHOLD = serializedObject.FindProperty("TEXTURE_FORMAT_THRESHOLD");
            TEXTURE_POT_THRESHOLD = serializedObject.FindProperty("TEXTURE_POT_THRESHOLD");
            TEXTURE_MAX_RESOLUTION_THRESHOLD = serializedObject.FindProperty("TEXTURE_MAX_RESOLUTION_THRESHOLD");
            TEXTURE_FILTERMODE_THRESHOLD = serializedObject.FindProperty("TEXTURE_FILTERMODE_THRESHOLD");
            TEXTURE_RW_THRESHOLD = serializedObject.FindProperty("TEXTURE_RW_THRESHOLD");
            TEXTURE_ANISOLEVEL_THRESHOLD = serializedObject.FindProperty("TEXTURE_ANISOLEVEL_THRESHOLD");
            // Model Condition
            MODEL_MAX_BONES_THRESHOLD = serializedObject.FindProperty("MODEL_MAX_BONES_THRESHOLD");
            MODEL_UV_CHANNEL_THRESHOLD = serializedObject.FindProperty("MODEL_UV_CHANNEL_THRESHOLD");
            MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD = serializedObject.FindProperty("MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD");
            MODEL_AUTO_GENERATE_UV2_THRESHOLD = serializedObject.FindProperty("MODEL_AUTO_GENERATE_UV2_THRESHOLD");
            MODEL_ANIM_COMPRESSION_THRESHOLD = serializedObject.FindProperty("MODEL_ANIM_COMPRESSION_THRESHOLD");
            MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD = serializedObject.FindProperty("MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD");
            MODEL_VERTEX_MAX_AMOUNT_THRESHOLD = serializedObject.FindProperty("MODEL_VERTEX_MAX_AMOUNT_THRESHOLD");
            MODEL_VERTEX_COLOR_THRESHOLD = serializedObject.FindProperty("MODEL_VERTEX_COLOR_THRESHOLD");
            MODEL_RW_THRESHOLD = serializedObject.FindProperty("MODEL_RW_THRESHOLD");
            // Collect Data Condition
            m_RecorderMaxStepAmount = serializedObject.FindProperty("m_RecorderMaxStepAmount");
            // Exception Tips
            TIPS_TEXTURE_FORMAT = serializedObject.FindProperty("TIPS_TEXTURE_FORMAT");
            TIPS_TEXTURE_RESOLUTION_NPOT = serializedObject.FindProperty("TIPS_TEXTURE_RESOLUTION_NPOT");
            TIPS_TEXTURE_RESOLUTION_SIZE = serializedObject.FindProperty("TIPS_TEXTURE_RESOLUTION_SIZE");
            TIPS_TEXTURE_FILTERMODE = serializedObject.FindProperty("TIPS_TEXTURE_FILTERMODE");
            TIPS_TEXTURE_ANISOLEVEL = serializedObject.FindProperty("TIPS_TEXTURE_ANISOLEVEL");
            TIPS_TEXTURE_RW = serializedObject.FindProperty("TIPS_TEXTURE_RW");
            TIPS_TEXTURE_NOUSED = serializedObject.FindProperty("TIPS_TEXTURE_NOUSED");
            TIPS_MODEL_RW = serializedObject.FindProperty("TIPS_MODEL_RW");

            TIPS_MODEL_MESH_TRIANGLE_LIMIT = serializedObject.FindProperty("TIPS_MODEL_MESH_TRIANGLE_LIMIT");
            TIPS_MODEL_MESH_VERTEX_LIMIT = serializedObject.FindProperty("TIPS_MODEL_MESH_VERTEX_LIMIT");
            TIPS_MODEL_MESH_BLENDSHAPE_LIMIT = serializedObject.FindProperty("TIPS_MODEL_MESH_BLENDSHAPE_LIMIT");
            TIPS_MODEL_MESH_VC_LIMIT = serializedObject.FindProperty("TIPS_MODEL_MESH_VC_LIMIT");
            TIPS_MODEL_MESH_UV_NOUSED = serializedObject.FindProperty("TIPS_MODEL_MESH_UV2_AUTO");
            TIPS_MODEL_MESH_UV2_AUTO = serializedObject.FindProperty("TIPS_MODEL_MESH_UV2_AUTO");
            TIPS_MODEL_BONE_LIMIT = serializedObject.FindProperty("TIPS_MODEL_BONE_LIMIT");
            TIPS_MODEL_ANIM_COMPRESSION = serializedObject.FindProperty("TIPS_MODEL_ANIM_COMPRESSION");
            TIPS_MODEL_NOUSED = serializedObject.FindProperty("TIPS_MODEL_NOUSED");

            TIPS_MATERIAL_MISS_SHADER = serializedObject.FindProperty("TIPS_MATERIAL_MISS_SHADER");
            TIPS_MATERIAL_BUILTIN_REF = serializedObject.FindProperty("TIPS_MATERIAL_BUILTIN_REF");
            TIPS_MATERIAL_NO_USED_DATA = serializedObject.FindProperty("TIPS_MATERIAL_NO_USED_DATA");
            TIPS_MATERIAL_NO_USED = serializedObject.FindProperty("TIPS_MATERIAL_NO_USED");

            TIPS_SHADER_DEP = serializedObject.FindProperty("TIPS_SHADER_DEP");
            TIPS_SHADER_NO_USED = serializedObject.FindProperty("TIPS_SHADER_NO_USED");

            TIPS_PREFAB_MISS_SCRIPT = serializedObject.FindProperty("TIPS_PREFAB_MISS_SCRIPT");
            TIPS_PREFAB_COLLIDER = serializedObject.FindProperty("TIPS_PREFAB_COLLIDER");
            TIPS_PREFAB_BUILTIN_REF = serializedObject.FindProperty("TIPS_PREFAB_BUILTIN_REF");
            TIPS_PREFAB_MISS_MAT = serializedObject.FindProperty("TIPS_PREFAB_MISS_MAT");

            TIPS_SCENE_MISS_SCRIPT = serializedObject.FindProperty("TIPS_SCENE_MISS_SCRIPT");
            TIPS_SCENE_COLLIDER = serializedObject.FindProperty("TIPS_SCENE_COLLIDER");
            TIPS_SCENE_BUILTIN_REF = serializedObject.FindProperty("TIPS_SCENE_BUILTIN_REF");

            TIPS_SCRIPT_DEP = serializedObject.FindProperty("TIPS_SCRIPT_DEP");

            TIPS_ANIMATORCONTROLLER_NOUSED = serializedObject.FindProperty("TIPS_ANIMATORCONTROLLER_NOUSED");

            TIPS_AUDIO_VIDEO_NOUSED = serializedObject.FindProperty("TIPS_AUDIO_VIDEO_NOUSED");

            TIPS_REPEAT_FILES = serializedObject.FindProperty("TIPS_REPEAT_FILES");

            TIPS_RESOURCES_FOLDER = serializedObject.FindProperty("TIPS_RESOURCES_FOLDER");

            TIPS_ASSETBUNDLE_STRATEGY = serializedObject.FindProperty("TIPS_ASSETBUNDLE_STRATEGY");
            TIPS_ASSETBUNDLE_BUILTIN = serializedObject.FindProperty("TIPS_ASSETBUNDLE_BUILTIN");

            // 纹理检测工具 /////////////////////////
            TEXTURE_COMPARE_REPORT_FOLDER = serializedObject.FindProperty("TEXTURE_COMPARE_REPORT_FOLDER");
            TEXTURE_FILE_EXTENSION = serializedObject.FindProperty("TEXTURE_FILE_EXTENSION");

            // 图集检测工具 /////////////////////////
            SPRITEATLAS_SOURCE_DATA_FOLDER = serializedObject.FindProperty("SPRITEATLAS_SOURCE_DATA_FOLDER");
            SPRITEATLAS_FORMAT_THRESHOLD = serializedObject.FindProperty("SPRITEATLAS_FORMAT_THRESHOLD");
            SPRITEATLAS_ANISOLEVEL_THRESHOLD = serializedObject.FindProperty("SPRITEATLAS_ANISOLEVEL_THRESHOLD");
            SPRITEATLAS_MEMORY_OVER_THRESHOLD = serializedObject.FindProperty("SPRITEATLAS_MEMORY_OVER_THRESHOLD");
            SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD = serializedObject.FindProperty("SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD");
            SPRITEATLAS_MAX_RESOLUTION_THRESHOLD = serializedObject.FindProperty("SPRITEATLAS_MAX_RESOLUTION_THRESHOLD");
            SPRITEATLAS_FILTERMODE_THRESHOLD = serializedObject.FindProperty("SPRITEATLAS_FILTERMODE_THRESHOLD");
            SPRITEATLAS_RW_THRESHOLD = serializedObject.FindProperty("SPRITEATLAS_RW_THRESHOLD");
            SPRITEATLAS_MIPMAP_THRESHOLD = serializedObject.FindProperty("SPRITEATLAS_MIPMAP_THRESHOLD");
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.BeginHorizontal("Box");

            EditorGUILayout.PropertyField(PROJECT_DATA_BASE_FOLDER, PROJECT_DATA_BASE_FOLDER_CONTENT);

            EditorGUILayout.EndHorizontal();

            m_ToolIndex = GUILayout.Toolbar(m_ToolIndex, m_ToolTypeNames);
            switch (m_ToolIndex)
            {
                case 0:
                    AssetToolOnInspecorGUI();
                    break;
                case 1:
                    SpriteAtlasToolOnInspectorGUI();
                    break;
                case 2:
                    TextureToolOnInspectorGUI();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}

