using System;
using System.IO;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace jj.TATools.Editor
{
    internal class AppConfigHelper
    {
        #region Fields

        // Default ////////////////////////////////////////////////////////////////////
        internal static string PROJECT_DATA_BASE_FOLDER { get; set; }
        internal static string ASSET_CUSTOM_RULE_FOLDER { get; set; }
        internal static string ASSET_WHITELIST_FLODER { get; set; }
        internal static string ASSET_COMPARE_SOURCE_FLODER { get; set; }
        internal static string ASSET_COMPARE_REPORT_FLODER { get; set; }
        internal static string ASSET_TEMP_CACHE_FLODER { get; set; }
        internal static string LAST_VERSION_ASSET_FILE { get { return AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_LAST_VERSION_ASSET_FILE); } }
        internal static string CONFIG_CUSTOM_RULE_FILE_PATH { get { return AssetDetectionUtility.PathCombine(ASSET_CUSTOM_RULE_FOLDER, ConstDefine.CONFIG_CUSTOM_RULE_FILE_NAME); } }
        internal static string CONFIG_WHITE_LIST_FILE_PATH { get { return AssetDetectionUtility.PathCombine(ASSET_WHITELIST_FLODER, ConstDefine.CONFIG_WHITELIST_FILE_NAME); } }
        internal static string TEXTURE_FORMAT_THRESHOLD { get { return ToolsConfig.Instance.TEXTURE_FORMAT_THRESHOLD; } }
        internal static int TEXTURE_POT_THRESHOLD { get { return ToolsConfig.Instance.TEXTURE_POT_THRESHOLD; } }

        internal static int TEXTURE_MAX_RESOLUTION_THRESHOLD { get { return ToolsConfig.Instance.TEXTURE_MAX_RESOLUTION_THRESHOLD; } }

        internal static int TEXTURE_FILTERMODE_THRESHOLD { get { return ToolsConfig.Instance.TEXTURE_FILTERMODE_THRESHOLD; } }

        internal static int TEXTURE_RW_THRESHOLD { get { return ToolsConfig.Instance.TEXTURE_RW_THRESHOLD; } }
 
        internal static int TEXTURE_ANISOLEVEL_THRESHOLD { get { return ToolsConfig.Instance.TEXTURE_ANISOLEVEL_THRESHOLD; } }

        internal static int MODEL_MAX_BONES_THRESHOLD { get { return ToolsConfig.Instance.MODEL_MAX_BONES_THRESHOLD; } }

        internal static int[] MODEL_UV_CHANNEL_THRESHOLD
        {
            get
            {
                var strValue = ToolsConfig.Instance.MODEL_UV_CHANNEL_THRESHOLD;
                if (string.IsNullOrEmpty(strValue)) return new int[] { 0, 0, 1, 1, 1, 1, 1, 1 };
                else
                {
                    var arr = strValue.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    if (arr.Length == 8)
                    {
                        int[] uvStates = new int[8];
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (string.IsNullOrEmpty(arr[i]))
                                uvStates[i] = 1;
                            else
                                uvStates[i] = int.Parse(arr[i]);
                        }

                        return uvStates;
                    }
                    else
                        return new int[] { 0, 0, 1, 1, 1, 1, 1, 1 };
                }
            }
        }

        internal static int MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD { get { return ToolsConfig.Instance.MODEL_BLENDSHAPE_MAX_AMOUNT_THRESHOLD; } }

        internal static int MODEL_AUTO_GENERATE_UV2_THRESHOLD { get { return ToolsConfig.Instance.MODEL_AUTO_GENERATE_UV2_THRESHOLD; } }

        internal static int MODEL_ANIM_COMPRESSION_THRESHOLD { get { return ToolsConfig.Instance.MODEL_ANIM_COMPRESSION_THRESHOLD; } }

        internal static int MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD { get { return ToolsConfig.Instance.MODEL_TRIANGLE_MAX_AMOUNT_THRESHOLD; } }

        internal static int MODEL_VERTEX_MAX_AMOUNT_THRESHOLD { get { return ToolsConfig.Instance.MODEL_VERTEX_MAX_AMOUNT_THRESHOLD; } }

        internal static int MODEL_VERTEX_COLOR_THRESHOLD { get { return ToolsConfig.Instance.MODEL_VERTEX_COLOR_THRESHOLD; } }

        internal static int MODEL_RW_THRESHOLD { get { return ToolsConfig.Instance.MODEL_RW_THRESHOLD; } }

        // Texture Transparent ////////////////////////////////////////////////////////////////////
        internal static string LAST_VERSION_TEXTURE_FLODER { get { return AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_LAST_VERSION_TEXTURE_FLODER); } }
        internal static string CURRENT_VERSION_TEXTURE_FLODER { get { return AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_CURRENT_VERSION_TEXTURE_FLODER); } }
        internal static string TEXTURE_FILE_EXTENSION { get { return ToolsConfig.Instance.TEXTURE_FILE_EXTENSION; } }
        internal static string TEXTURE_COMPARE_REPORT_FOLDER { get; set; }

        // Sprite Atlas ////////////////////////////////////////////////////////////////////
        internal static string SPRITEATLAS_SOURCE_DATA_FOLDER { get; set; }
        internal static string SPRITEATLAS_FORMAT_THRESHOLD { get { return ToolsConfig.Instance.SPRITEATLAS_FORMAT_THRESHOLD; } }
        internal static int SPRITEATLAS_ANISOLEVEL_THRESHOLD { get { return ToolsConfig.Instance.SPRITEATLAS_ANISOLEVEL_THRESHOLD; } }

        internal static float SPRITEATLAS_MEMORY_OVER_THRESHOLD { get { return ToolsConfig.Instance.SPRITEATLAS_MEMORY_OVER_THRESHOLD; } }

        internal static float SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD { get { return ToolsConfig.Instance.SPRITEATLAS_TRANSPARENT_PERCENTAGE_THRESHOLD; } }

        internal static int SPRITEATLAS_MAX_RESOLUTION_THRESHOLD { get { return ToolsConfig.Instance.SPRITEATLAS_MAX_RESOLUTION_THRESHOLD; } }

        internal static int SPRITEATLAS_FILTERMODE_THRESHOLD { get { return ToolsConfig.Instance.SPRITEATLAS_FILTERMODE_THRESHOLD; } }
   
        internal static int SPRITEATLAS_RW_THRESHOLD { get { return ToolsConfig.Instance.SPRITEATLAS_RW_THRESHOLD; } }

        internal static int SPRITEATLAS_MIPMAP_THRESHOLD { get { return ToolsConfig.Instance.SPRITEATLAS_MIPMAP_THRESHOLD; } }

        // Exception - Texture //////////////////////////////////
        // RW
        static string[] _TIPS_TEXTURE_RW;
        internal static string[] TIPS_TEXTURE_RW
        {
            get
            {
                if (_TIPS_TEXTURE_RW == null)
                {
                    var str = ToolsConfig.Instance.TIPS_TEXTURE_RW;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_TEXTURE_RW = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_TEXTURE_RW;
            }
        }
        // Format
        static string[] _TIPS_TEXTURE_FORMAT;
        internal static string[] TIPS_TEXTURE_FORMAT
        {
            get
            {
                if (_TIPS_TEXTURE_FORMAT == null)
                {
                    var str = ToolsConfig.Instance.TIPS_TEXTURE_FORMAT;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_TEXTURE_FORMAT = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_TEXTURE_FORMAT;
            }
        }
        // RES NPOT
        static string[] _TIPS_TEXTURE_RESOLUTION_NPOT;
        internal static string[] TIPS_TEXTURE_RESOLUTION_NPOT
        {
            get
            {
                if (_TIPS_TEXTURE_RESOLUTION_NPOT == null)
                {
                    var str = ToolsConfig.Instance.TIPS_TEXTURE_RESOLUTION_NPOT;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_TEXTURE_RESOLUTION_NPOT = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_TEXTURE_RESOLUTION_NPOT;
            }
        }
        // RES SIZE
        static string[] _TIPS_TEXTURE_RESOLUTION_SIZE;
        internal static string[] TIPS_TEXTURE_RESOLUTION_SIZE
        {
            get
            {
                if (_TIPS_TEXTURE_RESOLUTION_SIZE == null)
                {
                    var str = ToolsConfig.Instance.TIPS_TEXTURE_RESOLUTION_SIZE;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_TEXTURE_RESOLUTION_SIZE = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_TEXTURE_RESOLUTION_SIZE;
            }
        }
        // FILTER MODE
        static string[] _TIPS_TEXTURE_FILTERMODE;
        internal static string[] TIPS_TEXTURE_FILTERMODE
        {
            get
            {
                if (_TIPS_TEXTURE_FILTERMODE == null)
                {
                    var str = ToolsConfig.Instance.TIPS_TEXTURE_FILTERMODE;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_TEXTURE_FILTERMODE = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_TEXTURE_FILTERMODE;
            }
        }
        // ANISOLEVEL
        static string[] _TIPS_TEXTURE_ANISOLEVEL;
        internal static string[] TIPS_TEXTURE_ANISOLEVEL
        {
            get
            {
                if (_TIPS_TEXTURE_ANISOLEVEL == null)
                {
                    var str = ToolsConfig.Instance.TIPS_TEXTURE_ANISOLEVEL;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_TEXTURE_ANISOLEVEL = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_TEXTURE_ANISOLEVEL;
            }
        }
        // NO USED
        static string[] _TIPS_TEXTURE_NOUSED;
        internal static string[] TIPS_TEXTURE_NOUSED
        {
            get
            {
                if (_TIPS_TEXTURE_NOUSED == null)
                {
                    var str = ToolsConfig.Instance.TIPS_TEXTURE_NOUSED;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_TEXTURE_NOUSED = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_TEXTURE_NOUSED;
            }
        }
        // Exception - Model & Mesh//////////////////////////////////
        // RW
        static string[] _TIPS_MODEL_RW;
        internal static string[] TIPS_MODEL_RW
        {
            get
            {
                if (_TIPS_MODEL_RW == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MODEL_RW;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MODEL_RW = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_MODEL_RW;
            }
        }
        // Triangle
        static string[] _TIPS_MODEL_MESH_TRIANGLE_LIMIT;
        internal static string[] TIPS_MODEL_MESH_TRIANGLE_LIMIT
        {
            get
            {
                if (_TIPS_MODEL_MESH_TRIANGLE_LIMIT == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MODEL_MESH_TRIANGLE_LIMIT;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MODEL_MESH_TRIANGLE_LIMIT = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_MODEL_MESH_TRIANGLE_LIMIT;
            }
        }
        // Vertex Count
        static string[] _TIPS_MODEL_MESH_VERTEX_LIMIT;
        internal static string[] TIPS_MODEL_MESH_VERTEX_LIMIT
        {
            get
            {
                if (_TIPS_MODEL_MESH_VERTEX_LIMIT == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MODEL_MESH_VERTEX_LIMIT;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MODEL_MESH_VERTEX_LIMIT = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }


                return _TIPS_MODEL_MESH_VERTEX_LIMIT;
            }
        }
        // BlendShape
        static string[] _TIPS_MODEL_MESH_BLENDSHAPE_LIMIT;
        internal static string[] TIPS_MODEL_MESH_BLENDSHAPE_LIMIT
        {
            get
            {
                if (_TIPS_MODEL_MESH_BLENDSHAPE_LIMIT == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MODEL_MESH_BLENDSHAPE_LIMIT;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MODEL_MESH_BLENDSHAPE_LIMIT = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_MODEL_MESH_BLENDSHAPE_LIMIT;
            }
        }
        // Vertex Color
        static string[] _TIPS_MODEL_MESH_VC_LIMIT;
        internal static string[] TIPS_MODEL_MESH_VC_LIMIT
        {
            get
            {
                if (_TIPS_MODEL_MESH_VC_LIMIT == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MODEL_MESH_VC_LIMIT;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MODEL_MESH_VC_LIMIT = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }


                return _TIPS_MODEL_MESH_VC_LIMIT;
            }
        }
        // UV Noused
        static string[] _TIPS_MODEL_MESH_UV_NOUSED;
        internal static string[] TIPS_MODEL_MESH_UV_NOUSED
        {
            get
            {
                if (_TIPS_MODEL_MESH_UV_NOUSED == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MODEL_MESH_UV_NOUSED;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MODEL_MESH_UV_NOUSED = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }


                return _TIPS_MODEL_MESH_UV_NOUSED;
            }
        }
        // UV2 Auto
        static string[] _TIPS_MODEL_MESH_UV2_AUTO;
        internal static string[] TIPS_MODEL_MESH_UV2_AUTO
        {
            get
            {
                if (_TIPS_MODEL_MESH_UV2_AUTO == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MODEL_MESH_UV2_AUTO;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MODEL_MESH_UV2_AUTO = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }


                return _TIPS_MODEL_MESH_UV2_AUTO;
            }
        }

        // Bone
        static string[] _TIPS_MODEL_BONE_LIMIT;
        internal static string[] TIPS_MODEL_BONE_LIMIT
        {
            get
            {
                if (_TIPS_MODEL_BONE_LIMIT == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MODEL_BONE_LIMIT;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MODEL_BONE_LIMIT = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }


                return _TIPS_MODEL_BONE_LIMIT;
            }
        }

        // Anim Compression
        static string[] _TIPS_MODEL_ANIM_COMPRESSION;
        internal static string[] TIPS_MODEL_ANIM_COMPRESSION
        {
            get
            {
                if (_TIPS_MODEL_ANIM_COMPRESSION == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MODEL_ANIM_COMPRESSION;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MODEL_ANIM_COMPRESSION = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }


                return _TIPS_MODEL_ANIM_COMPRESSION;
            }
        }

        // No Used
        static string[] _TIPS_MODEL_NOUSED;
        internal static string[] TIPS_MODEL_NOUSED
        {
            get
            {
                if (_TIPS_MODEL_NOUSED == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MODEL_NOUSED;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MODEL_NOUSED = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }


                return _TIPS_MODEL_NOUSED;
            }
        }
        // Exception - Material //////////////////////////////////
        // Miss Shader
        static string[] _TIPS_MATERIAL_MISS_SHADER;
        internal static string[] TIPS_MATERIAL_MISS_SHADER
        {
            get
            {
                if (_TIPS_MATERIAL_MISS_SHADER == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MATERIAL_MISS_SHADER;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MATERIAL_MISS_SHADER = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_MATERIAL_MISS_SHADER;
            }
        }
        // Builtin Ref
        static string[] _TIPS_MATERIAL_BUILTIN_REF;
        internal static string[] TIPS_MATERIAL_BUILTIN_REF
        {
            get
            {
                if (_TIPS_MATERIAL_BUILTIN_REF == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MATERIAL_BUILTIN_REF;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MATERIAL_BUILTIN_REF = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_MATERIAL_BUILTIN_REF;
            }
        }
        // No Used Data
        static string[] _TIPS_MATERIAL_NO_USED_DATA;
        internal static string[] TIPS_MATERIAL_NO_USED_DATA
        {
            get
            {
                if (_TIPS_MATERIAL_NO_USED_DATA == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MATERIAL_NO_USED_DATA;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MATERIAL_NO_USED_DATA = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_MATERIAL_NO_USED_DATA;
            }
        }
        // No Used
        static string[] _TIPS_MATERIAL_NO_USED;
        internal static string[] TIPS_MATERIAL_NO_USED
        {
            get
            {
                if (_TIPS_MATERIAL_NO_USED == null)
                {
                    var str = ToolsConfig.Instance.TIPS_MATERIAL_NO_USED;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_MATERIAL_NO_USED = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_MATERIAL_NO_USED;
            }
        }
        // Exception - Shader //////////////////////////////////
        // Dep
        static string[] _TIPS_SHADER_DEP;
        internal static string[] TIPS_SHADER_DEP
        {
            get
            {
                if (_TIPS_SHADER_DEP == null)
                {
                    var str = ToolsConfig.Instance.TIPS_SHADER_DEP;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_SHADER_DEP = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_SHADER_DEP;
            }
        }
        // No Used
        static string[] _TIPS_SHADER_NO_USED;
        internal static string[] TIPS_SHADER_NO_USED
        {
            get
            {
                if (_TIPS_SHADER_NO_USED == null)
                {
                    var str = ToolsConfig.Instance.TIPS_SHADER_NO_USED;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_SHADER_NO_USED = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_SHADER_NO_USED;
            }
        }
        // Exception - Prefab //////////////////////////////////
        // Miss Script
        static string[] _TIPS_PREFAB_MISS_SCRIPT;
        internal static string[] TIPS_PREFAB_MISS_SCRIPT
        {
            get
            {
                if (_TIPS_PREFAB_MISS_SCRIPT == null)
                {
                    var str = ToolsConfig.Instance.TIPS_PREFAB_MISS_SCRIPT;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_PREFAB_MISS_SCRIPT = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_PREFAB_MISS_SCRIPT;
            }
        }
        // Collider
        static string[] _TIPS_PREFAB_COLLIDER;
        internal static string[] TIPS_PREFAB_COLLIDER
        {
            get
            {
                if (_TIPS_PREFAB_COLLIDER == null)
                {
                    var str = ToolsConfig.Instance.TIPS_PREFAB_COLLIDER;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_PREFAB_COLLIDER = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_PREFAB_COLLIDER;
            }
        }
        // Builtin Ref
        static string[] _TIPS_PREFAB_BUILTIN_REF;
        internal static string[] TIPS_PREFAB_BUILTIN_REF
        {
            get
            {
                if (_TIPS_PREFAB_BUILTIN_REF == null)
                {
                    var str = ToolsConfig.Instance.TIPS_PREFAB_BUILTIN_REF;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_PREFAB_BUILTIN_REF = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_PREFAB_BUILTIN_REF;
            }
        }
        // Miss Mat
        static string[] _TIPS_PREFAB_MISS_MAT;
        internal static string[] TIPS_PREFAB_MISS_MAT
        {
            get
            {
                if (_TIPS_PREFAB_MISS_MAT == null)
                {
                    var str = ToolsConfig.Instance.TIPS_PREFAB_MISS_MAT;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_PREFAB_MISS_MAT = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_PREFAB_MISS_MAT;
            }
        }
        // Exception - Scene //////////////////////////////////
        // Builtin Ref
        static string[] _TIPS_SCENE_BUILTIN_REF;
        internal static string[] TIPS_SCENE_BUILTIN_REF
        {
            get
            {
                if (_TIPS_SCENE_BUILTIN_REF == null)
                {
                    var str = ToolsConfig.Instance.TIPS_SCENE_BUILTIN_REF;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_SCENE_BUILTIN_REF = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_SCENE_BUILTIN_REF;
            }
        }
        // Collider
        static string[] _TIPS_SCENE_COLLIDER;
        internal static string[] TIPS_SCENE_COLLIDER
        {
            get
            {
                if (_TIPS_SCENE_COLLIDER == null)
                {
                    var str = ToolsConfig.Instance.TIPS_SCENE_COLLIDER;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_SCENE_COLLIDER = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_SCENE_COLLIDER;
            }
        }
        // Miss Script
        static string[] _TIPS_SCENE_MISS_SCRIPT;
        internal static string[] TIPS_SCENE_MISS_SCRIPT
        {
            get
            {
                if (_TIPS_SCENE_MISS_SCRIPT == null)
                {
                    var str = ToolsConfig.Instance.TIPS_SCENE_MISS_SCRIPT;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_SCENE_MISS_SCRIPT = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_SCENE_MISS_SCRIPT;
            }
        }
        // Exception - Script //////////////////////////////////
        // Dep
        static string[] _TIPS_SCRIPT_DEP;
        internal static string[] TIPS_SCRIPT_DEP
        {
            get
            {
                if (_TIPS_SCRIPT_DEP == null)
                {
                    var str = ToolsConfig.Instance.TIPS_SCRIPT_DEP;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_SCRIPT_DEP = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_SCRIPT_DEP;
            }
        }
        // Exception - AnimatorController //////////////////////////////////
        //  No Used
        static string[] _TIPS_ANIMATORCONTROLLER_NOUSED;
        internal static string[] TIPS_ANIMATORCONTROLLER_NOUSED
        {
            get
            {
                if (_TIPS_ANIMATORCONTROLLER_NOUSED == null)
                {
                    var str = ToolsConfig.Instance.TIPS_ANIMATORCONTROLLER_NOUSED;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_ANIMATORCONTROLLER_NOUSED = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_ANIMATORCONTROLLER_NOUSED;
            }
        }
        // Exception - Audio & Video //////////////////////////////////
        static string[] _TIPS_AUDIO_VIDEO_NOUSED;
        internal static string[] TIPS_AUDIO_VIDEO_NOUSED
        {
            get
            {
                if (_TIPS_AUDIO_VIDEO_NOUSED == null)
                {
                    var str = ToolsConfig.Instance.TIPS_AUDIO_VIDEO_NOUSED;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_AUDIO_VIDEO_NOUSED = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_AUDIO_VIDEO_NOUSED;
            }
        }
        // Exception - Repeat Files //////////////////////////////////
        static string[] _TIPS_REPEAT_FILES;
        internal static string[] TIPS_REPEAT_FILES
        {
            get
            {
                if (_TIPS_REPEAT_FILES == null)
                {
                    var str = ToolsConfig.Instance.TIPS_REPEAT_FILES;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_REPEAT_FILES = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_REPEAT_FILES;
            }
        }
        // Exception - Resources Folder //////////////////////////////////
        static string[] _TIPS_RESOURCES_FOLDER;
        internal static string[] TIPS_RESOURCES_FOLDER
        {
            get
            {
                if (_TIPS_RESOURCES_FOLDER == null)
                {
                    var str = ToolsConfig.Instance.TIPS_RESOURCES_FOLDER;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_RESOURCES_FOLDER = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_RESOURCES_FOLDER;
            }
        }

        // Exception - Assetbundle Strategy //////////////////////////////////
        static string[] _TIPS_ASSETBUNDLE_STRATEGY;
        internal static string[] TIPS_ASSETBUNDLE_STRATEGY
        {
            get
            {
                if (_TIPS_ASSETBUNDLE_STRATEGY == null)
                {
                    var str = ToolsConfig.Instance.TIPS_ASSETBUNDLE_STRATEGY;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_ASSETBUNDLE_STRATEGY = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_ASSETBUNDLE_STRATEGY;
            }
        }
        // Exception - Assetbundle Builtin //////////////////////////////////
        static string[] _TIPS_ASSETBUNDLE_BUILTIN;
        internal static string[] TIPS_ASSETBUNDLE_BUILTIN
        {
            get
            {
                if (_TIPS_ASSETBUNDLE_BUILTIN == null)
                {
                    var str = ToolsConfig.Instance.TIPS_ASSETBUNDLE_BUILTIN;
                    if (!string.IsNullOrEmpty(str))
                    {
                        _TIPS_ASSETBUNDLE_BUILTIN = str.Split(ConstDefine.SPLIT_CHAR_FLAG);
                    }
                }

                return _TIPS_ASSETBUNDLE_BUILTIN;
            }
        }

        // CheckBox State ////////////////////////////////////////////////////
        internal static bool CONFIG_GENERATE_FULL_REPORTER_CHECKBOX { 
            get { 
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_FULL_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            } 
        }

        internal static bool CONFIG_GENERATE_NEWER_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_NEWER_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_DELETED_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_DELETED_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_MODIFIED_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_MODIFIED_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_REPEATED_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_REPEATED_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_NOUSED_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_NOUSED_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_TEXTURE_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_TEXTURE_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_MODEL_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_MODEL_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_MATERIAL_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_MATERIAL_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_SHADER_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_SHADER_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_SCRIPT_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_SCRIPT_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_DEP_BUILTIN_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_DEP_BUILTIN_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_PREFAB_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_PREFAB_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_SCENE_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_SCENE_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_AB_STRATEGY_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_AB_STRATEGY_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_AB_BUILTIN_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_AB_BUILTIN_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        internal static bool CONFIG_GENERATE_RESOURCES_FOLDER_REPORTER_CHECKBOX
        {
            get
            {
                var valueStr = AppConfigHelper.ReadConfigFromEditorPrefs(ConstDefine.CONFIG_GENERATE_RESOURCES_FOLDER_REPORTER_CHECKBOX);
                if (string.IsNullOrEmpty(valueStr)) return true;
                int valueInt = int.Parse(valueStr);
                return valueInt == 1;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// 创建白名单配置
        /// </summary>
        internal static void CreateWhileListConfigFile(bool openFile)
        {
            string whiteListConfigFilePath = CONFIG_WHITE_LIST_FILE_PATH;
            if (File.Exists(whiteListConfigFilePath)) return;

            ConfigDataParser.GenerateWhiteListConfigFile(whiteListConfigFilePath);

            if (openFile)
                AssetDetectionUtility.OpenFile(whiteListConfigFilePath);
        }

        /// <summary>
        /// 创建自定义规则配置
        /// </summary>
        /// <param name="filePath"></param>
        internal static void CreateCustomRuleConfigFile(bool openFile)
        {
            // Custom Rule
            string customRuleConfigFilePath = CONFIG_CUSTOM_RULE_FILE_PATH;
            if (File.Exists(customRuleConfigFilePath)) return;

            ConfigDataParser.GenerateCustomRuleConfigFile(customRuleConfigFilePath);

            if (openFile)
                AssetDetectionUtility.OpenFile(customRuleConfigFilePath);
        }

        internal static string ReadConfigFromEditorPrefs(string key)
        {
            return EditorPrefs.GetString(key);
        }

        internal static void AddConfigToEditorPrefs(string key, string appValue)
        {
            EditorPrefs.SetString(key, appValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static bool CreateFolders()
        {
            var toolSetting = ToolsConfig.Instance;
            if (toolSetting == null)
            {
                AssetDetectionUtility.MessageBox("Not find 'ToolsConfig.asset' !!!", "Create Folders");
                return false;
            }
            // Base Data Folder
            PROJECT_DATA_BASE_FOLDER = toolSetting.PROJECT_DATA_BASE_FOLDER;
            if (string.IsNullOrEmpty(PROJECT_DATA_BASE_FOLDER))
            {
                AssetDetectionUtility.MessageBox("Not find \'" + ConstDefine.CONFIG_PROJECT_DATA_BASE_FOLDER + "\' from ToolsConfig", "Create Folders");
                return false;
            }

            DirectoryInfo baseFolderInfo = new DirectoryInfo(Application.dataPath);
            PROJECT_DATA_BASE_FOLDER = baseFolderInfo.Parent.FullName + "\\" + PROJECT_DATA_BASE_FOLDER;
            if (!Directory.Exists(PROJECT_DATA_BASE_FOLDER)) Directory.CreateDirectory(PROJECT_DATA_BASE_FOLDER);

            // Temp Cache File Folder
            ASSET_TEMP_CACHE_FLODER = AssetDetectionUtility.PathCombine(PROJECT_DATA_BASE_FOLDER, "Temp");
            if (!Directory.Exists(ASSET_TEMP_CACHE_FLODER)) Directory.CreateDirectory(ASSET_TEMP_CACHE_FLODER);

            // White List Config Folder
            ASSET_WHITELIST_FLODER = toolSetting.ASSET_WHITELIST_FLODER;
            if (string.IsNullOrEmpty(ASSET_WHITELIST_FLODER))
            {
                AssetDetectionUtility.MessageBox("Not find \'" + ConstDefine.CONFIG_ASSET_WHITELIST_FLODER + "\' from ToolsConfig", "Create Folders");
                return false;
            }
            ASSET_WHITELIST_FLODER = AssetDetectionUtility.PathCombine(PROJECT_DATA_BASE_FOLDER ,ASSET_WHITELIST_FLODER);
            if (!Directory.Exists(ASSET_WHITELIST_FLODER)) Directory.CreateDirectory(ASSET_WHITELIST_FLODER);

            // Asset Compare Source File Folder
            ASSET_COMPARE_SOURCE_FLODER = toolSetting.ASSET_COMPARE_SOURCE_FLODER;
            if (string.IsNullOrEmpty(ASSET_COMPARE_SOURCE_FLODER))
            {
                AssetDetectionUtility.MessageBox("Not find \'" + ConstDefine.CONFIG_ASSET_COMPARE_SOURCE_FLODER + "\' from ToolsConfig", "Create Folders");
                return false;
            }
            ASSET_COMPARE_SOURCE_FLODER = AssetDetectionUtility.PathCombine(PROJECT_DATA_BASE_FOLDER, ASSET_COMPARE_SOURCE_FLODER);
            if (!Directory.Exists(ASSET_COMPARE_SOURCE_FLODER)) Directory.CreateDirectory(ASSET_COMPARE_SOURCE_FLODER);

            // Asset Compare Report Folder
            ASSET_COMPARE_REPORT_FLODER = toolSetting.ASSET_COMPARE_REPORT_FLODER;
            if (string.IsNullOrEmpty(ASSET_COMPARE_REPORT_FLODER))
            {
                AssetDetectionUtility.MessageBox("Not find \'" + ConstDefine.CONFIG_ASSET_COMPARE_REPORT_FLODER + "\' from ToolsConfig", "Create Folders");
                return false;
            }
            ASSET_COMPARE_REPORT_FLODER = AssetDetectionUtility.PathCombine(PROJECT_DATA_BASE_FOLDER, ASSET_COMPARE_REPORT_FLODER);
            if (!Directory.Exists(ASSET_COMPARE_REPORT_FLODER)) Directory.CreateDirectory(ASSET_COMPARE_REPORT_FLODER);

            // Asset Custom Rule Folder
            ASSET_CUSTOM_RULE_FOLDER = toolSetting.ASSET_CUSTOM_RULE_FOLDER;
            if (string.IsNullOrEmpty(ASSET_CUSTOM_RULE_FOLDER))
            {
                AssetDetectionUtility.MessageBox("Not find \'" + ConstDefine.CONFIG_ASSET_CUSTOM_RULE_FOLDER + "\' from ToolsConfig", "Create Folders");
                return false;
            }
            ASSET_CUSTOM_RULE_FOLDER = AssetDetectionUtility.PathCombine(PROJECT_DATA_BASE_FOLDER, ASSET_CUSTOM_RULE_FOLDER);
            if (!Directory.Exists(ASSET_CUSTOM_RULE_FOLDER)) Directory.CreateDirectory(ASSET_CUSTOM_RULE_FOLDER);

            // Texture Compare Report Folder
            TEXTURE_COMPARE_REPORT_FOLDER = toolSetting.TEXTURE_COMPARE_REPORT_FOLDER;
            if (string.IsNullOrEmpty(TEXTURE_COMPARE_REPORT_FOLDER))
            {
                AssetDetectionUtility.MessageBox("Not find \'" + ConstDefine.CONFIG_TEXTURE_COMPARE_REPORT_FLODER + "\' from ToolsConfig", "Create Folders");
                return false;
            }
            TEXTURE_COMPARE_REPORT_FOLDER = AssetDetectionUtility.PathCombine(PROJECT_DATA_BASE_FOLDER, TEXTURE_COMPARE_REPORT_FOLDER);
            if (!Directory.Exists(TEXTURE_COMPARE_REPORT_FOLDER)) Directory.CreateDirectory(TEXTURE_COMPARE_REPORT_FOLDER);

            // Sprite Atlas
            SPRITEATLAS_SOURCE_DATA_FOLDER = toolSetting.SPRITEATLAS_SOURCE_DATA_FOLDER;
            if (string.IsNullOrEmpty(SPRITEATLAS_SOURCE_DATA_FOLDER))
            {
                AssetDetectionUtility.MessageBox("Not find \'" + ConstDefine.CONFIG_SPRITEATLAS_SOURCE_DATA_FLODER + "\' from ToolsConfig", "Create Folders");
                return false;
            }
            SPRITEATLAS_SOURCE_DATA_FOLDER = AssetDetectionUtility.PathCombine(PROJECT_DATA_BASE_FOLDER, SPRITEATLAS_SOURCE_DATA_FOLDER);
            if (!Directory.Exists(SPRITEATLAS_SOURCE_DATA_FOLDER)) Directory.CreateDirectory(SPRITEATLAS_SOURCE_DATA_FOLDER);


            return true;
        }

        #endregion
    }
}
