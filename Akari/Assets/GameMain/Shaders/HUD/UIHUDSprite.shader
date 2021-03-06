// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "HUD/HUDSprite"
{
	Properties
	{
		_MainTex ("Alpha (A)", 2D) = "white" {}
		_MainAlpha("MainAlpha (A)", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_ReverseY("ReverseY", Float) = 1.0

		[Header(ZTest)]
		[Enum(UnityEngine.Rendering.CompareFunction)]_ZTest("LEqual or Disable", Float) = 4 //0 = disable
	}
	SubShader
	{
		Tags
		{
            "RenderPipeline"="UniversalRenderPipeline"
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True"
		}

		HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		struct appdata_t
		{
			float4 vertex : POSITION;
			float4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
		};

		struct v2f
		{
			float4 vertex : POSITION;
			float4 color : COLOR;
			float2 texcoord : TEXCOORD0;
		};

		CBUFFER_START(UnityPerMaterial) 
		float4 _MainTex_ST;
		float4 _MainAlpha_ST;
		float4 _Color;
		float  _ReverseY;
		CBUFFER_END
			
		TEXTURE2D(_MainTex);
		SAMPLER(sampler_MainTex);
		TEXTURE2D(_MainAlpha);
		SAMPLER(sampler_MainAlpha);
			
		ENDHLSL 

		Pass
		{	
			Cull Front
			Lighting Off
			ZWrite Off
			//ZTest Off
			ZTest[_ZTest]
			Fog { Mode Off }
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile MAIN_ALPHA_OFF  MAIN_ALPHA_ON

			v2f vert (appdata_t v)
			{
				v2f o;

				float fScale = min(12.80 / _ScreenParams.x, 7.2 / _ScreenParams.y);
				float2  uvOffset = v.uv2;
				uvOffset.x *= fScale;
				uvOffset.y *= fScale;

				float3  right = UNITY_MATRIX_IT_MV[0].xyz;
				float3  up = UNITY_MATRIX_IT_MV[1].xyz;
				float3  vPos = v.vertex.xyz + uvOffset.x * right + uvOffset.y * up;
				o.vertex = TransformObjectToHClip(vPos);

				o.color = v.color * _Color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);

				return o;
			}

			float4 frag (v2f i) : SV_Target
			{
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.texcoord);
#if defined(MAIN_ALPHA_ON)
				float4 alpha = SAMPLE_TEXTURE2D(_MainAlpha,sampler_MainAlpha,i.texcoord);
				col.a = alpha.g;
#endif
				return col * i.color;
			}
			ENDHLSL 
		}
	}	
}
