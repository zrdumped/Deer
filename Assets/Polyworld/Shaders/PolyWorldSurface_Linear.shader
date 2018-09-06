Shader "QuantumTheory/PolyWorld/PolyWorld Surface Linear" {
	Properties{

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

		float _Smoothness;
	float _Metallic;
	float _EMISSION;
	float4 _Color;

	struct Input {
		half4 vertexColor;
	};

	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.vertexColor = v.color;
	}

	void surf(Input IN, inout SurfaceOutputStandard o) {

		o.Albedo = pow(IN.vertexColor, 2.2f);
		o.Smoothness = _Smoothness;
		o.Metallic = _Metallic;
		o.Emission = (_EMISSION*_Color)*(pow(IN.vertexColor.a, 2.2f));
	}

	ENDCG
	}

		Fallback "Diffuse"
}