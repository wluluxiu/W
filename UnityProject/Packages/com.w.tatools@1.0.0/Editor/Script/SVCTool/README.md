# SVC工具（URP版本）

## 一. 引擎版本

​		**1. Unity 版本**

​				a. `2022.3.32`

​				b. `其他版本`暂未做兼容

​		**2. URP 版本**

​				a. `14.0.11`

​				b. `其他版本`暂未做兼容

## 二. Shader编写规范

​		**1. Shader中未使用Builtin光照计算，不允许出现以下声明：**

​				a. `#pragma multi_compile_fwdbase`

​				b. `#pragma multi_compile_fwdadd`

​		**2. LightMode声明必须在当前Pass内Tags声明，SubShader中Tags内不允许声明**

| 样例                                                         | 结果         |
| ------------------------------------------------------------ | ------------ |
| SubShader {<br/>	Blend SrcAlpha OneMinusSrcAlpha<br/>	Tags {"Queue" = "Transparent" **"LightMode" = "SRPDefaultUnlit"** "RenderPipeline" = "UniversalPipeline"}<br/>	Pass {<br/>		...<br/>	}<br/>} | **错    误** |
| SubShader<br/>{<br/>	Tags { <br/>		"RenderType" = "Opaque"<br/>		"RenderPipeline" = "UniversalPipeline"<br/>		"UniversalMaterialType" = "Lit"<br/>		"IgnoreProjector" = "True"<br/>	}<br/><br/>	Pass<br/>	{<br/>		Name "ForwardLit"<br/>		Tags { **"LightMode" = "UniversalForward"**}<br/>	}<br/>} | **正    确** |