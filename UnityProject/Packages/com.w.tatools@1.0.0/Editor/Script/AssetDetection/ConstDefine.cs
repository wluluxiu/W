namespace jj.TATools.Editor
{
    internal class ConstDefine
    {
        internal const string APP_VERSION = "v.0.0.1";

        internal const string CONFIG_PROJECT_DATA_BASE_FOLDER = "PROJECT_DATA_BASE_FOLDER";

        internal const string REPORT_FILE_EXTENSION = "xlsx";

        internal const string REPORT_RETURN = "返回";

        internal const string REPORT_LEGEND_TITLE = "图例";
        internal const string REPORT_LEGEND_CURRENT = "当前版本迭代";
        internal const string REPORT_LEGEND_LAST = "历史版本遗留";
        internal const string REPORT_LEGEND_REFERENCED_IN_CODE = "代码动态加载";
        internal const string REPORT_LEGEND_WHITELIST = "白名单资源";
        internal const string REPORT_LEGEND_EXCEPTION = "资源异常项";

        internal const string MODULE_FOLDER_AMOUNT = "MODULE_FOLDER_AMOUNT_KEY";
        internal const string MODULE_FOLDER_ITEM_DIR = "MODULE_FOLDER_ITEM_DIR_KEY_";
        internal const string MODULE_FOLDER_ITEM_STATE = "MODULE_FOLDER_ITEM_STATE_KEY_";

        #region Asset Compare Tool

        internal const string PATH_SPLIT_FLAG = "\\";

        internal const char SPLIT_CHAR_FLAG = '|';

        internal const string CONFIG_ASSET_CUSTOM_RULE_FOLDER = "ASSET_CUSTOM_RULE_FOLDER";
        internal const string CONFIG_ASSET_WHITELIST_FLODER = "ASSET_WHITELIST_FLODER";
        internal const string CONFIG_ASSET_COMPARE_SOURCE_FLODER = "ASSET_COMPARE_SOURCE_FLODER";
        internal const string CONFIG_ASSET_COMPARE_REPORT_FLODER = "ASSET_COMPARE_REPORT_FLODER";
        internal const string CONFIG_LAST_VERSION_ASSET_FILE = "LAST_VERSION_ASSET_FILE";

        internal const string CONFIG_WHITELIST_FILE_NAME = "资源白名单配置.xlsx";
        internal const string CONFIG_WHITELIST_DYNAMIC_LOADED = "动态加载";
        internal const string CONFIG_WHITELIST_TITLE_1 = "类型";
        internal const string CONFIG_WHITELIST_TITLE_2 = "路径";

        internal const string CONFIG_CUSTOM_RULE_FILE_NAME = "自定义规则配置.xlsx";
        internal const string CONFIG_CUSTOM_RULE_0_FOLDER_PATH = "按路径划分";
        internal const string CONFIG_PATH_RULE_TITLE_1 = "目录名";
        internal const string CONFIG_PATH_RULE_TITLE_2 = "目录路径";

        internal const string CONFIG_GENERATE_FULL_REPORTER_CHECKBOX = "GENERATE_FULL_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_NEWER_REPORTER_CHECKBOX = "GENERATE_NEWER_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_DELETED_REPORTER_CHECKBOX = "GENERATE_DELETED_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_MODIFIED_REPORTER_CHECKBOX = "GENERATE_MODIFIED_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_REPEATED_REPORTER_CHECKBOX = "GENERATE_REPEATED_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_NOUSED_REPORTER_CHECKBOX = "GENERATE_NOUSED_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_TEXTURE_REPORTER_CHECKBOX = "GENERATE_TEXTURE_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_MODEL_REPORTER_CHECKBOX = "GENERATE_MODEL_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_MATERIAL_REPORTER_CHECKBOX = "GENERATE_MATERIAL_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_SHADER_REPORTER_CHECKBOX = "GENERATE_SHADER_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_SCRIPT_REPORTER_CHECKBOX = "GENERATE_SCRIPT_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_DEP_BUILTIN_REPORTER_CHECKBOX = "GENERATE_DEP_BUILTIN_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_PREFAB_REPORTER_CHECKBOX = "GENERATE_PREFAB_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_SCENE_REPORTER_CHECKBOX = "GENERATE_SCENE_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_AB_STRATEGY_REPORTER_CHECKBOX = "GENERATE_AB_STRATEGY_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_AB_BUILTIN_REPORTER_CHECKBOX = "GENERATE_AB_BUILTIN_REPORTER_CHECKBOX";
        internal const string CONFIG_GENERATE_RESOURCES_FOLDER_REPORTER_CHECKBOX = "GENERATE_RESOURCES_FOLDER_REPORTER_CHECKBOX";



        internal const string SELECT_FILE_FILTER = "cache";
        internal const string SELECT_FILE_TITLE_0 = "选择原始版本资源Cache文件";
        internal const string SELECT_FILE_TITLE_1 = "选择当前版本资源Cache文件";

        internal const string REPORT_FILE_NAME = "资源检测报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_0 = "整体统计报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_1 = "当前版本资源详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_2 = "资源迭代新增详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_3 = "资源迭代删除详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_4 = "资源迭代修改详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_5 = "重复资源详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_6 = "冗余资源详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_7 = "纹理异常详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_8 = "模型异常详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_9 = "材质异常详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_10 = "Shader详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_11 = "脚本异常引用详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_12 = "引用内置资源详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_13 = "Prefab异常详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_14 = "Scene异常详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_15 = "Assetbundle分包策略异常详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_16 = "Assetbundle内置资源详情报告";
        internal const string REPORT_ASSETCOMPARE_SHEET_17 = "Resources目录资源详情报告";
       // internal const string REPORT_ASSETCOMPARE_SHEET_14 = "定制文件夹详情报告";


        internal const string REPORT_TOTAL_CONTENT_TITLE_0 = "当前版本资源统计";
        internal const string REPORT_TOTAL_CONTENT_TITLE_1 = "原始版本资源统计";
        internal const string REPORT_TOTAL_CONTENT_TITLE_2 = "资源增量统计";
        internal const string REPORT_TOTAL_CONTENT_TITLE_3 = "类型";
        internal const string REPORT_TOTAL_CONTENT_TITLE_4 = "物理容量";
        internal const string REPORT_TOTAL_CONTENT_TITLE_6 = "详情报告目录";
        internal const string REPORT_TOTAL_CONTENT_TITLE_7 = "数量";

        internal const string REPORT_DETAIL_TITLE_0 = "总计";

        internal const string REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_0 = "异常资源统计";
        internal const string REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_1 = "资源类型";
        internal const string REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_2 = "数量";
        internal const string REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_3 = "异常项";
        internal const string REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_5 = "影响范围";
        internal const string REPORT_TOTAL_EXCEPTION_CONTENT_TITLE_4 = "优化方案";

        internal const string CONFIG_TIPS_TEXTURE_FORMAT = "TIPS_TEXTURE_FORMAT";
        internal const string CONFIG_TIPS_TEXTURE_RESOLUTION_NPOT = "TIPS_TEXTURE_RESOLUTION_NPOT";
        internal const string CONFIG_TIPS_TEXTURE_RESOLUTION_SIZE = "TIPS_TEXTURE_RESOLUTION_SIZE";
        internal const string CONFIG_TIPS_TEXTURE_FILTERMODE = "TIPS_TEXTURE_FILTERMODE";
        internal const string CONFIG_TIPS_TEXTURE_ANISOLEVEL = "TIPS_TEXTURE_ANISOLEVEL";
        internal const string CONFIG_TIPS_TEXTURE_RW = "TIPS_TEXTURE_RW";
        internal const string CONFIG_TIPS_TEXTURE_NOUSED = "TIPS_TEXTURE_NOUSED";

        // Default
        internal const string REPORT_DETAIL_DEFAULT_TITLE_1 = "资源类型";
        internal const string REPORT_DETAIL_DEFAULT_TITLE_2 = "物理容量";
        internal const string REPORT_DETAIL_DEFAULT_TITLE_4 = "资源路径";
        internal const string REPORT_DETAIL_DEFAULT_TITLE_5 = "引用计数";
        internal const string REPORT_DETAIL_DEFAULT_TITLE_6 = "引用内置";
        internal const string REPORT_DETAIL_DEFAULT_TITLE_7 = "默认引用";
        // Model
        internal const string REPORT_DETAIL_MODEL_TITLE_0 = "读写";
        internal const string REPORT_DETAIL_MODEL_TITLE_1 = "面数";
        internal const string REPORT_DETAIL_MODEL_TITLE_2 = "顶点数";
        internal const string REPORT_DETAIL_MODEL_TITLE_3 = "骨骼数";
        internal const string REPORT_DETAIL_MODEL_TITLE_4 = "Normal";
        internal const string REPORT_DETAIL_MODEL_TITLE_5 = "Tangent";
        internal const string REPORT_DETAIL_MODEL_TITLE_6 = "顶点色";
        internal const string REPORT_DETAIL_MODEL_TITLE_7 = "UV通道";
        internal const string REPORT_DETAIL_MODEL_TITLE_8 = "BlendShape";
        internal const string REPORT_DETAIL_MODEL_TITLE_9 = "UV2生成";
        internal const string REPORT_DETAIL_MODEL_TITLE_10 = "动画压缩";
        // Mesh
        internal const string REPORT_DETAIL_MESH_TITLE_0 = "读写";
        internal const string REPORT_DETAIL_MESH_TITLE_1 = "面数";
        internal const string REPORT_DETAIL_MESH_TITLE_2 = "顶点数";
        internal const string REPORT_DETAIL_MESH_TITLE_3 = "Normal";
        internal const string REPORT_DETAIL_MESH_TITLE_4 = "Tangent";
        internal const string REPORT_DETAIL_MESH_TITLE_5 = "顶点色";
        internal const string REPORT_DETAIL_MESH_TITLE_6 = "UV通道";
        internal const string REPORT_DETAIL_MESH_TITLE_7 = "BlendShape";
        //  Texture
        internal const string REPORT_DETAIL_TEXTURE_TITLE_0 = "读写";
        internal const string REPORT_DETAIL_TEXTURE_TITLE_1 = "分辨率";
        internal const string REPORT_DETAIL_TEXTURE_TITLE_2 = "压缩格式";
        internal const string REPORT_DETAIL_TEXTURE_TITLE_3 = "Mipmap";
        internal const string REPORT_DETAIL_TEXTURE_TITLE_4 = "AnisoLevel";
        internal const string REPORT_DETAIL_TEXTURE_TITLE_5 = "FilterMode";
        internal const string REPORT_DETAIL_TEXTURE_TITLE_6 = "内存大小";
        //  SpriteAtlas
        internal const string REPORT_DETAIL_ATLAS_TITLE_0 = "读写";
        internal const string REPORT_DETAIL_ATLAS_TITLE_1 = "MaxSize";
        internal const string REPORT_DETAIL_ATLAS_TITLE_2 = "压缩格式";
        internal const string REPORT_DETAIL_ATLAS_TITLE_3 = "Mipmap";
        internal const string REPORT_DETAIL_ATLAS_TITLE_4 = "AnisoLevel";
        internal const string REPORT_DETAIL_ATLAS_TITLE_5 = "FilterMode";
        internal const string REPORT_DETAIL_ATLAS_TITLE_6 = "BuildInclude";
        //  RenderTexture
        internal const string REPORT_DETAIL_RT_TITLE_0 = "RandomWrite";
        internal const string REPORT_DETAIL_RT_TITLE_1 = "分辨率";
        internal const string REPORT_DETAIL_RT_TITLE_2 = "C-Format";
        internal const string REPORT_DETAIL_RT_TITLE_3 = "D-Format";
        internal const string REPORT_DETAIL_RT_TITLE_4 = "Mipmap";
        internal const string REPORT_DETAIL_RT_TITLE_5 = "AntiAliasing";
        internal const string REPORT_DETAIL_RT_TITLE_6 = "FilterMode";
        internal const string REPORT_DETAIL_RT_TITLE_7 = "内存大小";
        // Clip
        internal const string REPORT_DETAIL_CLIP_TITLE_0 = "循环";
        internal const string REPORT_DETAIL_CLIP_TITLE_1 = "时长";
        internal const string REPORT_DETAIL_CLIP_TITLE_2 = "WrapMode";
        internal const string REPORT_DETAIL_CLIP_TITLE_3 = "动画类型";
        // Shader
        internal const string REPORT_DETAIL_SHADER_TITLE_0 = "默认引用";
        internal const string REPORT_DETAIL_SHADER_TITLE_1 = "Keyword 数量";
        // Material
        internal const string REPORT_DETAIL_MATERIAL_TITLE_0 = "丢失Shader";
        internal const string REPORT_DETAIL_MATERIAL_TITLE_1 = "冗余纹理";
        internal const string REPORT_DETAIL_MATERIAL_TITLE_2 = "冗余Float";
        internal const string REPORT_DETAIL_MATERIAL_TITLE_3 = "冗余Color";
        internal const string REPORT_DETAIL_MATERIAL_TITLE_4 = "内置引用";
        // Scene
        internal const string REPORT_DETAIL_SCENE_TITLE_0 = "脚本丢失";
        internal const string REPORT_DETAIL_SCENE_TITLE_1 = "Collider";
        internal const string REPORT_DETAIL_SCENE_TITLE_2 = "内置引用";
        // Prefab
        internal const string REPORT_DETAIL_PREFAB_TITLE_0 = "脚本丢失";
        internal const string REPORT_DETAIL_PREFAB_TITLE_1 = "Collider";
        internal const string REPORT_DETAIL_PREFAB_TITLE_2 = "内置引用";
        internal const string REPORT_DETAIL_PREFAB_TITLE_3 = "丢失材质";
        // Assetbundle
        internal const string REPORT_DETAIL_ASSETBUNDLE_TITLE_0 = "ID";
        internal const string REPORT_DETAIL_ASSETBUNDLE_TITLE_1 = "Assetbundle";
        internal const string REPORT_DETAIL_ASSETBUNDLE_TITLE_2 = "内置资源";

        internal const string REPORT_ADDED_TITLE = "当前版本资源新增统计";
        internal const string REPORT_DELETED_TITLE = "当前版本资源删除统计";
        internal const string REPORT_MODIFIED_TITLE = "当前版本资源修改统计";
        internal const string REPORT_REPEAT_TITLE = "重复资源统计";
        internal const string REPORT_NO_REFERENCES_TITLE = "冗余资源统计";
        internal const string REPORT_TEXTURE_EXCEPTION_TITLE = "纹理异常统计";
        internal const string REPORT_MODEL_EXCEPTION_TITLE = "模型异常统计";
        internal const string REPORT_MATERIAL_EXCEPTION_TITLE = "材质异常统计";
        internal const string REPORT_PREFAB_EXCEPTION_TITLE = "Prefab异常统计";
        internal const string REPORT_SCENE_EXCEPTION_TITLE = "Scene异常统计";
        internal const string REPORT_SCRIPT_DEPENDENDENCIES_TITLE = "脚本异常引用统计";
        internal const string REPORT_DEPEND_BUILTIN_ASSET_TITLE = "引用内置资源统计";
        internal const string REPORT_ASSETBUNDLE_STRATEGY_EXCEPTION_TITLE = "Assetbundle分包策略异常统计";
        internal const string REPORT_REOURCES_FOLDER_ASSET_TITLE = "Resources目录资源统计";

        internal const string REPORT_MODIFIED_CONTENT_TITLE_0 = "类型";
        internal const string REPORT_MODIFIED_CONTENT_TITLE_1 = "修改后物理容量";
        internal const string REPORT_MODIFIED_CONTENT_TITLE_2 = "修改后内存";
        internal const string REPORT_MODIFIED_CONTENT_TITLE_3 = "修改前物理容量";
        internal const string REPORT_MODIFIED_CONTENT_TITLE_4 = "修改前内存";
        internal const string REPORT_MODIFIED_CONTENT_TITLE_5 = "物理容量差值";

        internal const string REPORT_REPEAT_CONTENT_TITLE_1 = "资源名称";

        internal const string REPORT_TEXTURE_EXCEPTION_CONTENT_TITLE_0 = "数量";
        internal const string REPORT_TEXTURE_EXCEPTION_CONTENT_TITLE_1 = "ID";
        internal const string REPORT_TEXTURE_EXCEPTION_CONTENT_TITLE_2 = "内存容量";

        internal const string REPORT_MODEL_EXCEPTION_CONTENT_TITLE_0 = "数量";
        internal const string REPORT_MODEL_EXCEPTION_CONTENT_TITLE_1 = "ID";

        internal const string REPORT_PREFAB_EXCEPTION_CONTENT_TITLE_0 = "数量";
        internal const string REPORT_PREFAB_EXCEPTION_CONTENT_TITLE_1 = "ID";

        internal const string REPORT_SCENE_EXCEPTION_CONTENT_TITLE_0 = "数量";
        internal const string REPORT_SCENE_EXCEPTION_CONTENT_TITLE_1 = "ID";

        internal const string REPORT_SCRIPT_EXCEPTION_CONTENT_TITLE_0 = "数量";
        internal const string REPORT_SCRIPT_EXCEPTION_CONTENT_TITLE_1 = "ID";

        internal const string REPORT_MATERIAL_EXCEPTION_CONTENT_TITLE_0 = "数量";
        internal const string REPORT_MATERIAL_EXCEPTION_CONTENT_TITLE_1 = "ID";

        internal const string REPORT_CUSTOM_DIR_TITLE = "定制文件资源统计";
        internal const string REPORT_CUSTOM_DIR_CONTENT_0 = "目录名称";
        internal const string REPORT_CUSTOM_DIR_CONTENT_1 = "目录路径";
        internal const string REPORT_CUSTOM_DIR_CONTENT_2 = "文件数量";
        internal const string REPORT_CUSTOM_DIR_CONTENT_3 = "物理容量";

        #endregion

        #region Texture Compare Tool

        internal const string TGA_EXTENSION = ".tga";

        internal const string REPORT_ASSET_TEXTURE_FILE_NAME = "Texture资源检测报告";
        internal const string REPORT_ASSET_TEXTURE_COMPARE_SHEET_0 = "整体统计报告";
        internal const string REPORT_ASSET_TEXTURE_COMPARE_SHEET_1 = "当前版本Texture详情报告";
        internal const string REPORT_ASSET_TEXTURE_COMPARE_SHEET_2 = "Texture迭代新增详情报告";
        internal const string REPORT_ASSET_TEXTURE_COMPARE_SHEET_3 = "Texture迭代删除详情报告";
        internal const string REPORT_ASSET_TEXTURE_COMPARE_SHEET_4 = "Texture迭代修改详情报告";
        //internal const string REPORT_ASSET_TEXTURE_COMPARE_SHEET_5 = "重复资源详情报告";
        internal const string REPORT_ASSET_TEXTURE_COMPARE_SHEET_6 = "纹理透明度占比详情报告";

        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_0 = "当前版本Texture统计";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_1 = "原始版本Texture统计";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_2 = "Texture资源增量统计";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_3 = "类型";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_4 = "物理容量";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_5 = "内存容量";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_6 = "详情报告目录";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_7 = "数量";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_8 = "资源路径";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_9 = "分辨率";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_10 = "透明占比";
        internal const string REPORT_TEXTURE_TOTAL_CONTENT_TITLE_11 = "ID";
        internal const string REPORT_TEXTURE_REPEAT_CONTENT_TITLE_0 = "资源名称";
        internal const string REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_1 = "当前物理容量";
        internal const string REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_2 = "当前内存容量";
        internal const string REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_3 = "原始物理容量";
        internal const string REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_4 = "原始内存容量";
        internal const string REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_5 = "物理容量差值";
        internal const string REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_6 = "内存容量差值";
        internal const string REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_7 = "当前分辨率";
        internal const string REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_8 = "原始分辨率";
        internal const string REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_9 = "当前透明占比";
        internal const string REPORT_TEXTURE_MODIFIED_CONTENT_TITLE_10 = "原始透明占比";

        internal const string REPORT_TEXTURE_DETAIL_TITLE_0 = "总计";

        internal const string REPORT_TEXTURE_ADDED_TITLE = "当前版本Texture资源新增统计";
        internal const string REPORT_TEXTURE_DELETED_TITLE = "当前版本Texture资源删除统计";
        internal const string REPORT_TEXTURE_MODIFIED_TITLE = "当前版本Texture资源修改统计";
        internal const string REPORT_TEXTURE_REPEAT_TITLE = "重复资源统计";

        internal const string CONFIG_LAST_VERSION_TEXTURE_FLODER = "LAST_VERSION_TEXTURE_FLODER";
        internal const string CONFIG_CURRENT_VERSION_TEXTURE_FLODER = "CURRENT_VERSION_TEXTURE_FLODER";

        internal const string CONFIG_TEXTURE_COMPARE_REPORT_FLODER = "TEXTURE_COMPARE_REPORT_FLODER";

        internal const string SELECT_TEXTURE_FOLER_TITLE_0 = "选择原始版本Texture根路径";
        internal const string SELECT_TEXTURE_FOLER_TITLE_1 = "选择当前版本Texture根路径";

        internal const string TRANSPARNET_PERCENTAGE_TITLE_0 = "透明度占比统计";
        internal const string TRANSPARNET_PERCENTAGE_TITLE_1 = "透明度占比";
        internal const string TRANSPARNET_PERCENTAGE_TITLE_2 = "数量分布";
        internal const string TRANSPARNET_PERCENTAGE_TITLE_3 = "比例分布";

        #endregion

        #region Sprite Atlas Tool

        internal const string SPRITEATLAS_CACHE_FOLDER = "SpriteAtlasData";

        internal const string CONFIG_SPRITEATLAS_SOURCE_DATA_FLODER = "SPRITEATLAS_SOURCE_DATA_FLODER";

        internal const string REPORT_SPRITEATLAS_FILE_NAME = "SpirteAtlas检测报告";
        internal const string SELECT_SPRITEATLAS_FILE_FILTER = "txt";
        internal const string SELECT_SPRITEATLAS_FILE_TITLE = "选择SpiteAtlas数据文件";

        internal const string REPORT_SPRITEATLAS_DETAIL_TITLE = "图集资源统计";
        internal const string REPORT_SPRITEATLAS_CONTENT_TITLE_0 = "数量";
        internal const string REPORT_SPRITEATLAS_CONTENT_TITLE_1 = "文件大小";
        internal const string REPORT_SPRITEATLAS_CONTENT_TITLE_2 = "图集内存";
        internal const string REPORT_SPRITEATLAS_CONTENT_TITLE_3 = "Sprite内存";
        internal const string REPORT_SPRITEATLAS_CONTENT_TITLE_4 = "内存溢出";

        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_0 = "ID";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_1 = "读写";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_2 = "分辨率";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_3 = "压缩格式";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_4 = "Mipmap";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_5 = "AnisoLevel";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_6 = "FilterMode";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_7 = "MaxSize";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_8 = "Page Info";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_8_0 = "Name";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_8_1 = "分辨率";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_8_2 = "内存";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_8_3 = "透明度";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_8_4 = "快照";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_9 = "Sprite Info";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_9_0 = "内存";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_9_1 = "内存溢出";
        internal const string REPORT_DETAIL_SPRITEATLAS_TITLE_9_2 = "数量";


        internal const string REPORT_SPRITEATLAS_SHEET_0 = "图集详情报告";
        internal const string REPORT_SPRITEATLAS_SHEET_1 = "Sprite重复引用报告";
        internal const string REPORT_SPRITEATLAS_SHEET_2 = "Prefab引用图集报告";
        internal const string REPORT_SPRITEATLAS_SHEET_3 = "图集引用纹理设置异常报告";

        internal const string REPORT_SPRITE_REF_TITLE_0 = "ID";
        internal const string REPORT_SPRITE_REF_TITLE_1 = "Sprite 路径";
        internal const string REPORT_SPRITE_REF_TITLE_2 = "引用图集路径";

        internal const string REPORT_PREFAB_REF_ATLAS_TITLE_0 = "ID";
        internal const string REPORT_PREFAB_REF_ATLAS_TITLE_1 = "Prefab 路径";
        internal const string REPORT_PREFAB_REF_ATLAS_TITLE_2 = "引用图集数量";
        internal const string REPORT_PREFAB_REF_ATLAS_TITLE_3 = "引用图集路径";

        internal const string REPORT_SPRITE_REF_INVALID_TITLE_0 = "ID";
        internal const string REPORT_SPRITE_REF_INVALID_TITLE_1 = "图集路径";
        internal const string REPORT_SPRITE_REF_INVALID_TITLE_2 = "图集Format(Android|IOS)";
        internal const string REPORT_SPRITE_REF_INVALID_TITLE_3 = "异常Sprite数量";
        internal const string REPORT_SPRITE_REF_INVALID_TITLE_4 = "异常Sprite路径";
        internal const string REPORT_SPRITE_REF_INVALID_TITLE_5 = "Format-Android";
        internal const string REPORT_SPRITE_REF_INVALID_TITLE_6 = "Format-IOS";

        #endregion

    }
}
