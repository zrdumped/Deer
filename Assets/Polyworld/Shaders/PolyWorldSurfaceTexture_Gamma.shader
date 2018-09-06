Shader "QuantumTheory/PolyWorld/PolyWorld Surface Texture Gamma" {
	Properties{
		_MainTex("Diffuse Texture", 2D) = "white" {}
		_Smoothness("Smoothness", Range(0,1)) = 1
		_Metallic("Metalness",Range(0,1)) = 0
		_Color("Emissive Color", Color) = (1,1,1,0)
		_EMISSION("Emission Scale", Float) = 0
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 150

		CGPROGRAM

#pragma surface surf Standard vertex:vert addshadow
#pragma target 3.0
#pragma glsl
#pragma multi_compile_fog

#include "UnityCG.cginc"
	
	sampler2D _MainTex;
	float _Smoothness;
	float _Metallic;
	float _EMISSION;
	float4 _Color;

	struct Input {
		half4 vertexColor;
		float2 uv_MainTex;
	};

	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.vertexColor = v.color;
	}

	void surf(Input IN, inout SurfaceOutputStandard o) {

		o.Albedo = IN.vertexColor * tex2D(_MainTex, IN.uv_MainTex).rgb;
		o.Smoothness = _Smoothness;
		o.Metallic = _Metallic;
		o.Emission = (_EMISSION*_Color)*IN.vertexColor.a;
	}

	ENDCG
	}

		Fallback "Diffuse"
}