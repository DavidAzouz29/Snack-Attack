Shader "Custom/OutlineShader" {
		Properties
	{
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0.0, 0.03)) = .005
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		_GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
		[Enum(Metallic Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0
		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}
	    [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0
		_BumpScale("Scale", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}
	    _Parallax("Height Scale", Range(0.005, 0.08)) = 0.02
		_ParallaxMap("Height Map", 2D) = "black" {}
	    _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}
	    _EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}
	    _DetailMask("Detail Mask", 2D) = "white" {}
	    _DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
	    _DetailNormalMapScale("Scale", Float) = 1.0
		_DetailNormalMap("Normal Map", 2D) = "bump" {}
		[Enum(UV0,0,UV1,1)] _UVSec("UV Set for secondary textures", Float) = 0

		// Blending state
		[HideInInspector] _Mode("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend("__src", Float) = 1.0
		[HideInInspector] _DstBlend("__dst", Float) = 0.0
		[HideInInspector] _ZWrite("__zw", Float) = 1.0
	}

		CGINCLUDE
#include "UnityCG.cginc"
#define UNITY_SETUP_BRDF_INPUT MetallicSetup

			struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f {
			float4 pos : POSITION;
			float4 color : COLOR;
		};

		uniform float _Outline;
		uniform float4 _OutlineColor;

		v2f vert(appdata v) {
			// just make a copy of incoming vertex data but scaled according to normal direction
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

			float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
			float2 offset = TransformViewToProjection(norm.xy);

			o.pos.xy += offset * o.pos.z * _Outline;
			o.color = _OutlineColor;
			return o;
		}
		ENDCG

		SubShader
	{
		Tags{ "RenderType" = "Opaque" "PerformanceChecks" = "False" }
		Tags{ "Queue" = "Transparent" }
		LOD 300


		// ------------------------------------------------------------------
		//  Base forward pass (directional light, emission, lightmaps, ...)
		Pass
		{
		Name "FORWARD"
		Tags{ "LightMode" = "ForwardBase" }

		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]

		CGPROGRAM
#pragma target 3.0

		// -------------------------------------

#pragma shader_feature _NORMALMAP
#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
#pragma shader_feature _EMISSION
#pragma shader_feature _METALLICGLOSSMAP
#pragma shader_feature ___ _DETAIL_MULX2
#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
#pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
#pragma shader_feature _ _GLOSSYREFLECTIONS_OFF
#pragma shader_feature _PARALLAXMAP

#pragma multi_compile_fwdbase
#pragma multi_compile_fog

#pragma vertex vertBase
#pragma fragment fragBase
#include "UnityStandardCoreForward.cginc"

		ENDCG
	}
		// ------------------------------------------------------------------
		//  Additive forward pass (one light per pass)
		Pass
		{
		Name "FORWARD_DELTA"
		Tags{ "LightMode" = "ForwardAdd" }
		Blend[_SrcBlend] One
		Fog{ Color(0,0,0,0) } // in additive pass fog should be black
		ZWrite Off
		ZTest LEqual

		CGPROGRAM
#pragma target 3.0

		// -------------------------------------


#pragma shader_feature _NORMALMAP
#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
#pragma shader_feature _METALLICGLOSSMAP
#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
#pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
#pragma shader_feature ___ _DETAIL_MULX2
#pragma shader_feature _PARALLAXMAP

#pragma multi_compile_fwdadd_fullshadows
#pragma multi_compile_fog


#pragma vertex vertAdd
#pragma fragment fragAdd
#include "UnityStandardCoreForward.cginc"

		ENDCG
	}
		// ------------------------------------------------------------------
		//  Shadow rendering pass
		Pass
		{
		Name "ShadowCaster"
		Tags{ "LightMode" = "ShadowCaster" }

		ZWrite On ZTest LEqual

		CGPROGRAM
#pragma target 3.0

		// -------------------------------------


#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
#pragma multi_compile_shadowcaster

#pragma vertex vertShadowCaster
#pragma fragment fragShadowCaster

#include "UnityStandardShadow.cginc"

		ENDCG
	}
		// ------------------------------------------------------------------
		//  Deferred pass
		Pass
		{
		Name "DEFERRED"
		Tags{ "LightMode" = "Deferred" }

		CGPROGRAM
#pragma target 3.0
#pragma exclude_renderers nomrt


		// -------------------------------------

#pragma shader_feature _NORMALMAP
#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
#pragma shader_feature _EMISSION
#pragma shader_feature _METALLICGLOSSMAP
#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
#pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
#pragma shader_feature ___ _DETAIL_MULX2
#pragma shader_feature _PARALLAXMAP

#pragma multi_compile ___ UNITY_HDR_ON
#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
#pragma multi_compile ___ DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON

#pragma vertex vertDeferred
#pragma fragment fragDeferred

#include "UnityStandardCore.cginc"

		ENDCG
	}

		// ------------------------------------------------------------------
		// Extracts information for lightmapping, GI (emission, albedo, ...)
		// This pass it not used during regular rendering.
		Pass
		{
		Name "META"
		Tags{ "LightMode" = "Meta" }

		Cull Off

		CGPROGRAM
#pragma vertex vert_meta
#pragma fragment frag_meta

#pragma shader_feature _EMISSION
#pragma shader_feature _METALLICGLOSSMAP
#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
#pragma shader_feature ___ _DETAIL_MULX2

#include "UnityStandardMeta.cginc"
		ENDCG
	}

		Pass
		{
			Name "BASE"
			Cull Back
			Blend Zero One

			// uncomment this to hide inner details:
			//Offset -8, -8

			SetTexture[_OutlineColor]{
			ConstantColor(0,0,0,0)
			Combine constant
		}
		}

		// note that a vertex shader is specified here but its using the one above
		Pass
		{
			Name "OUTLINE"
			Tags{ "LightMode" = "Always" }
			Cull Front

			// you can choose what kind of blending mode you want for the outline
			//Blend SrcAlpha OneMinusSrcAlpha // Normal
			//Blend One One // Additive
			Blend One OneMinusDstColor // Soft Additive
									   //Blend DstColor Zero // Multiplicative
									   //Blend DstColor SrcColor // 2x Multiplicative

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag

			half4 frag(v2f i) :COLOR{
			return i.color;
		}
			ENDCG
		}
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" "PerformanceChecks" = "False" }
		Tags{ "Queue" = "Transparent" }

		LOD 150

		// ------------------------------------------------------------------
		//  Base forward pass (directional light, emission, lightmaps, ...)
		Pass
		{
		Name "FORWARD"
		Tags{ "LightMode" = "ForwardBase" }

		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]

		CGPROGRAM
#pragma target 2.0

#pragma shader_feature _NORMALMAP
#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
#pragma shader_feature _EMISSION 
#pragma shader_feature _METALLICGLOSSMAP 
#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
#pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
#pragma shader_feature _ _GLOSSYREFLECTIONS_OFF
		// SM2.0: NOT SUPPORTED shader_feature ___ _DETAIL_MULX2
		// SM2.0: NOT SUPPORTED shader_feature _PARALLAXMAP

#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE

#pragma multi_compile_fwdbase
#pragma multi_compile_fog

#pragma vertex vertBase
#pragma fragment fragBase
#include "UnityStandardCoreForward.cginc"

		ENDCG
	}
		// ------------------------------------------------------------------
		//  Additive forward pass (one light per pass)
		Pass
		{
		Name "FORWARD_DELTA"
		Tags{ "LightMode" = "ForwardAdd" }
		Blend[_SrcBlend] One
		Fog{ Color(0,0,0,0) } // in additive pass fog should be black
		ZWrite Off
		ZTest LEqual

		CGPROGRAM
#pragma target 2.0

#pragma shader_feature _NORMALMAP
#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
#pragma shader_feature _METALLICGLOSSMAP
#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
#pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
#pragma shader_feature ___ _DETAIL_MULX2
		// SM2.0: NOT SUPPORTED shader_feature _PARALLAXMAP
#pragma skip_variants SHADOWS_SOFT

#pragma multi_compile_fwdadd_fullshadows
#pragma multi_compile_fog

#pragma vertex vertAdd
#pragma fragment fragAdd
#include "UnityStandardCoreForward.cginc"

		ENDCG
	}
		// ------------------------------------------------------------------
		//  Shadow rendering pass
		Pass
		{
		Name "ShadowCaster"
		Tags{ "LightMode" = "ShadowCaster" }

		ZWrite On ZTest LEqual

		CGPROGRAM
#pragma target 2.0

#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
#pragma skip_variants SHADOWS_SOFT
#pragma multi_compile_shadowcaster

#pragma vertex vertShadowCaster
#pragma fragment fragShadowCaster

#include "UnityStandardShadow.cginc"

		ENDCG
	}

		// ------------------------------------------------------------------
		// Extracts information for lightmapping, GI (emission, albedo, ...)
		// This pass it not used during regular rendering.
		Pass
		{
		Name "META"
		Tags{ "LightMode" = "Meta" }

		Cull Off

		CGPROGRAM
#pragma vertex vert_meta
#pragma fragment frag_meta

#pragma shader_feature _EMISSION
#pragma shader_feature _METALLICGLOSSMAP
#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
#pragma shader_feature ___ _DETAIL_MULX2

#include "UnityStandardMeta.cginc"
		ENDCG
	}

		Pass
		{
			Name "BASE"
			Cull Back
			Blend Zero One

			// uncomment this to hide inner details:
			//Offset -8, -8

			SetTexture[_OutlineColor]{
			ConstantColor(0,0,0,0)
			Combine constant
		}
		}

		// note that a vertex shader is specified here but its using the one above
		Pass
		{
			Name "OUTLINE"
			Tags{ "LightMode" = "Always" }
			Cull Front

			// you can choose what kind of blending mode you want for the outline
			//Blend SrcAlpha OneMinusSrcAlpha // Normal
			//Blend One One // Additive
			Blend One OneMinusDstColor // Soft Additive
									   //Blend DstColor Zero // Multiplicative
									   //Blend DstColor SrcColor // 2x Multiplicative

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag

			half4 frag(v2f i) :COLOR{
			return i.color;
		}
			ENDCG
		}
	}

		FallBack "VertexLit"
		Fallback "Diffuse"
		CustomEditor "StandardShaderGUI"
}
