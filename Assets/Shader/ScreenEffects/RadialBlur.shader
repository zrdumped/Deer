// NOTE : 屏幕后处理特效：径向模糊


Shader "MyShaders/Radial Blur"
{
	Properties
	{
		_SampleDist ("Sample distance", Float) = 1.0
		_SampleStrength ("Sample strength", Float) = 1.0
		_BlurTex("Blur texture", 2D) = "black"{}
		_MainTex("Base(RGB)", 2D) = "white"{}
	}
	SubShader
	{
		CGINCLUDE

		#include "UnityCG.cginc"

		float _SampleStrength;
		float _SampleDist;
		sampler2D _MainTex;
		sampler2D _BlurTex;

		struct v2f{
			float4 pos:SV_POSITION;
			half2 texcoord:TEXCOORD0;
		};

		v2f vert(appdata_img v){
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.texcoord = v.texcoord;

			return o;
		}

		fixed4 fragRadialBlur(v2f i):SV_Target{
			fixed2 dir = 0.5-i.texcoord;

			fixed dist = length(dir);
			dir /= dist;
			dir *= _SampleDist;

			fixed4 sum = tex2D(_MainTex, i.texcoord - dir*0.01);  
    		sum += tex2D(_MainTex, i.texcoord - dir*0.02);  
    		sum += tex2D(_MainTex, i.texcoord - dir*0.03);  
    		sum += tex2D(_MainTex, i.texcoord - dir*0.05);  
    		sum += tex2D(_MainTex, i.texcoord - dir*0.08);  
    		sum += tex2D(_MainTex, i.texcoord + dir*0.01);  
    		sum += tex2D(_MainTex, i.texcoord + dir*0.02);  
    		sum += tex2D(_MainTex, i.texcoord + dir*0.03);  
    		sum += tex2D(_MainTex, i.texcoord + dir*0.05);  
    		sum += tex2D(_MainTex, i.texcoord + dir*0.08);  
    		sum *= 0.1;  
      
    		return sum;  
		}

		fixed4 fragCombine(v2f i):SV_Target{
			fixed dist = length(0.5 - i.texcoord);
			fixed4 col = tex2D(_MainTex, i.texcoord);
			fixed4 blur = tex2D(_BlurTex, i.texcoord);
			col = lerp(col, blur, saturate(_SampleStrength*dist));
			return col;
		}

		ENDCG

		ZTest Always Cull Off ZWrite Off

		Pass{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment fragRadialBlur

			ENDCG
		}

		Pass{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment fragCombine

			ENDCG
		}
	} FallBack Off
}
