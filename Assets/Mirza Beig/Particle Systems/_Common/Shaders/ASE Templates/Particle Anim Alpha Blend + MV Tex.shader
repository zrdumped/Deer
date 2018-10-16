Shader /*ase_name*/ "Particles Anim Alpha Blended + MV Tex" /*end*/
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)

		_MainTex ("Particle Texture", 2D) = "white" {}
		_MotionVectorTex ("Motion Vector Texture", 2D) = "white" {}

		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0

		/*ase_props*/
	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" /*ase_tags*/ }

		
		SubShader
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off 
			Lighting Off 
			ZWrite Off

			/*ase_pass*/

			Pass {
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_particles
				#pragma multi_compile_fog

				/*ase_pragma*/

				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					float2 texcoord2 : TEXCOORD1;
					UNITY_VERTEX_INPUT_INSTANCE_ID

					/*ase_vdata:p=p;uv0=tc0;uv1=tc1;c=c*/
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					float2 texcoord2 : TEXCOORD1;
					UNITY_FOG_COORDS(2)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD4;
					#endif
					UNITY_VERTEX_OUTPUT_STEREO

					/*ase_interp(3,7):sp=sp.xyzw;uv0=tc0;uv1=tc1;c=c*/
				};
				
				uniform sampler2D _MainTex;
				uniform sampler2D _MotionVectorTex;

				uniform fixed4 _TintColor;

				uniform float4 _MainTex_ST;

				uniform sampler2D_float _CameraDepthTexture;
				uniform float _InvFade;

				/*ase_globals*/

				v2f vert ( appdata_t v /*ase_vert_input*/ )
				{
					v2f o;

					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

					/*ase_vert_code:v=appdata_t;o=v2f*/

					v.vertex.xyz += /*ase_vert_out:Offset;Float3*/ float3( 0, 0, 0 ) /*end*/;
					o.vertex = UnityObjectToClipPos(v.vertex);

					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif

					o.color = v.color * _TintColor;
					
					o.texcoord.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex);
					o.texcoord.zw = TRANSFORM_TEX(v.texcoord.zw,_MainTex);

					o.texcoord2.x = v.texcoord2.x;
					o.texcoord2.y = v.texcoord2.y;
															
					UNITY_TRANSFER_FOG(o,o.vertex);

					return o;
				}

				fixed4 frag ( v2f i /*ase_frag_input*/ ) : SV_Target
				{
					#ifdef SOFTPARTICLES_ON

						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;

					#endif

					/*ase_frag_code:i=v2f*/
					
					fixed4 colA = tex2D(_MainTex, i.texcoord.xy);
					fixed4 colB = tex2D(_MainTex, i.texcoord.zw);

					float blend = i.texcoord2.x;

					fixed4 col = /*ase_frag_out:Color;Float4*/2.0f * i.color * lerp(colA, colB, blend)/*end*/;
					
					UNITY_APPLY_FOG(i.fogCoord, col);

					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
}
