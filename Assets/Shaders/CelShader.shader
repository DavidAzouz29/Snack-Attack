Shader "MyShader/Cel" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "blue" {}
	_Ramp("Toon Ramp (RGB)", 2D) = "blue" {}
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf ToonRamp

		sampler2D _Ramp;

	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif

		half d = max(0, dot(lightDir, s.Normal));
		if (d > 0.05f)
			d = 1.0f;
		else d = .6f;


		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

		half4 c;

		c.rgb = s.Albedo * _LightColor0.rgb * ramp * atten;

		c.a = 1;
		return c;
	}

	sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex : TEXCOORD0;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG

	}

		Fallback "Diffuse"
}
