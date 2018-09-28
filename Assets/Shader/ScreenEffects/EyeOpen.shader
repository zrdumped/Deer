Shader "MyShaders/Eye Open"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}
		_Threshod ("Threshod of eye control", Float) = 0.5
		_Delta("Threshod extent", Float) = 10.0
		_BlurSize("Blur Size", Float) = 1.0
	}
	SubShader
	{
		CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half4 _MainTex_TexelSize;
		sampler2D _MaskTex;
		float _Threshod;
		float _Delta;
		float _BlurSize;

		struct v2f{
			float4 pos : SV_POSITION;
			half2 uv[5] : TEXCOORD0;
		};


		v2f vertBlurVertical(appdata_img v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			
			half2 uv = v.texcoord;
			
			// _BlurSize 控制采样间隔，当为 1 时，相邻点采样；
			// 当为 2 时，每隔一个点采样，依此类推
			o.uv[0] = uv;
			o.uv[1] = uv + float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
			o.uv[2] = uv - float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
			o.uv[3] = uv + float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
			o.uv[4] = uv - float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
					 
			return o;
		}
		
		v2f vertBlurHorizontal(appdata_img v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			
			half2 uv = v.texcoord;
			
			o.uv[0] = uv;
			o.uv[1] = uv + float2(_MainTex_TexelSize.x * 1.0, 0.0) * _BlurSize;
			o.uv[2] = uv - float2(_MainTex_TexelSize.x * 1.0, 0.0) * _BlurSize;
			o.uv[3] = uv + float2(_MainTex_TexelSize.x * 2.0, 0.0) * _BlurSize;
			o.uv[4] = uv - float2(_MainTex_TexelSize.x * 2.0, 0.0) * _BlurSize;
					 
			return o;
		}

		v2f vert(appdata_img v){
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);

			half2 uv = v.texcoord;

			// _BlurSize 控制采样间隔，当为 1 时，相邻点采样；
			// 当为 2 时，每隔一个点采样，依此类推
			o.uv[0] = uv;
			o.uv[1] = uv + float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
			o.uv[2] = uv - float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
			o.uv[3] = uv + float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
			o.uv[4] = uv - float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;

			return o;

		}


		fixed4 fragBlur(v2f i) : SV_Target {
			float weight[3] = {0.4026, 0.2442, 0.0545};
			
			fixed3 sum = tex2D(_MainTex, i.uv[0]).rgb * weight[0];
			
			for (int it = 1; it < 3; it++) {
				sum += tex2D(_MainTex, i.uv[it*2-1]).rgb * weight[it];
				sum += tex2D(_MainTex, i.uv[it*2]).rgb * weight[it];
			}
			
			return fixed4(sum, 1.0);
		}

		fixed4 frag(v2f i) : SV_Target{
			fixed maskVal = tex2D(_MaskTex, i.uv[0]).g;
			

			fixed3 renderTexColor = tex2D(_MainTex, i.uv[0]).rgb;
			
			fixed3 color = fixed3(0,0,0);

			
			//fixed3 color = (maskVal > _Threshod) ? renderTexColor : (renderTexColor * maskVal);

			if(maskVal >= _Threshod + _Delta){
				color = renderTexColor;
			}
			else if(maskVal >= _Threshod - _Delta && _Threshod >= 0){
				float interColor = (maskVal-(_Threshod - _Delta))/(2*_Delta);
				color = renderTexColor * interColor;
			}
			else{
				color = fixed3(0,0,0);
			}
			return fixed4(color, 1.0);

		}


		ENDCG


		ZTest Always Cull Off ZWrite Off 


		Pass {
			NAME "GAUSSIAN_BLUR_VERTICAL"
			
			CGPROGRAM
			  
			#pragma vertex vertBlurVertical  
			#pragma fragment fragBlur
			  
			ENDCG  
		}
		
		Pass {  
			NAME "GAUSSIAN_BLUR_HORIZONTAL"
			
			CGPROGRAM  
			
			#pragma vertex vertBlurHorizontal  
			#pragma fragment fragBlur
			
			ENDCG
		}


		Pass
		{
			NAME "EYE_OPEN"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			
			ENDCG
		}
	}
	FallBack "Diffuse"
}
