//NOTE: 边缘光只能应用于Loypoly模型，即每个点提供 Color

Shader "MyShaders/RimLightVertexColor"
{
	Properties
	{
		_RimColor("Rim Light Color", Color) = (1,1,1,1)
		_ReflectionRate("Reflection Rate", Range(0,20)) = 0.3
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }

		Pass
		{

			Tags{"LightMode" = "ForwardBase"}

			CGPROGRAM

			#pragma multi_compile_fwdbase
			
			#pragma vertex vert
			#pragma fragment frag

			

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 color : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				SHADOW_COORDS(3)
			};

			fixed4 _RimColor;
			fixed _ReflectionRate;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;

				TRANSFER_SHADOW(o);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 worldPos = normalize(i.worldPos);
				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
				fixed3 worldNormal = normalize(i.worldNormal);


				fixed3 albedo = i.color;
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				fixed3 diffuse = _LightColor0.rgb * albedo * saturate(dot(worldNormal, lightDir));

				UNITY_LIGHT_ATTENUATION(atten, i, worldPos);

				// 计算 rim light
				float adotNV = abs(dot(worldNormal, viewDir));
				float Fr = pow(1-adotNV, _ReflectionRate);

				fixed3 rim = saturate(_RimColor.rgb * Fr);

				return fixed4(ambient + diffuse*atten + rim, 1.0);
			}
			ENDCG
		}

		Pass
		{

			Tags{"LightMode" = "ForwardAdd"}

			Blend One One 

			CGPROGRAM

			#pragma multi_compile_fwdadd_fullshadows

			#pragma vertex vert
			#pragma fragment frag

			

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 color : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				SHADOW_COORDS(3)
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;

				TRANSFER_SHADOW(o);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 worldPos = normalize(i.worldPos);
				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
				fixed3 worldNormal = normalize(i.worldNormal);


				fixed3 albedo = i.color;
				fixed3 diffuse = _LightColor0.rgb * albedo * saturate(dot(worldNormal, lightDir));

				UNITY_LIGHT_ATTENUATION(atten, i, worldPos);

				return fixed4(diffuse*atten, 1.0);
			}
			ENDCG
		}
	}
	Fallback "QuantumTheory/VertexColors/VertexLit"
}
