Shader "URP/Toon"
{
	Properties
	{
		_MainColor("Main Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}	
		// 环境光均匀地施加到对象的所有表面上
		[HDR]_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		[HDR]_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		// 控制镜面反射的大小
		_Glossiness("Glossiness", Float) = 32
		[HDR]_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		//控制边缘在接近表面的未照明部分时的融合程度。
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

		//阴影所需属性
		[HideInInspector]_BaseMap("Base Map", 2D) = "white" {}	
		[HideInInspector]_BaseColor("Base Color", Color) = (1, 1, 1, 1)
		[HideInInspector]_Cutoff("Cutoff", Float) = 1
	}
	SubShader
	{
        Tags 
		{ 
            "RenderPipeline"="UniversalPipeline"
		}

		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

		CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_ST;
        float4 _MainColor;
        float4 _AmbientColor;
        float4 _SpecularColor;
        float _Glossiness;
        float4 _RimColor;
        float _RimAmount;
        float _RimThreshold;

		// 提供给 SurfeceInput和ShadowCasterPass的参数 用于产生阴影
		float4 _BaseMap_ST;
		float4 _BaseColor;
		float _Cutoff;
        CBUFFER_END

		TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

		struct a2v
        {
            float4 positionOS : POSITION;
            float2 texcoord : TEXCOORD;
			float3 normalOS : NORMAL;
        };

		struct v2f
        {
            float4 positionCS : SV_POSITION;
            float2 texcoord : TEXCOORD0;
			float3 normalWS : NORMAL;
			float3 positionWS : TEXCOORD1;
        };
        ENDHLSL

		Pass
		{
			Tags
			{
				"LightMode" = "UniversalForward"
				"PassFlags" = "OnlyDirectional"//只接收直线光
			}

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag	
			//关键字编译多个变体shader 计算阴影所需要
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT//柔化阴影，得到软阴影
			
			v2f vert (a2v i)
            {
                v2f o;

                o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
                o.texcoord = TRANSFORM_TEX(i.texcoord,_MainTex);
                o.normalWS = TransformObjectToWorldNormal(i.normalOS.xyz);
                o.positionWS = TransformObjectToWorld(i.positionOS.xyz);
                return o;
            }
			
			half4 frag (v2f i) : SV_Target
            {
                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.texcoord);

				Light mainLight = GetMainLight(TransformWorldToShadowCoord(i.positionWS));
				float3 WS_L = normalize(mainLight.direction);//光照方向
				float3 WS_N = normalize(i.normalWS);//法线
				float3 WS_V =  normalize(_WorldSpaceCameraPos-i.positionWS);//观察方向
				float3 WS_H = normalize(WS_V+WS_L);//半程向量

				//漫反射
                float NdotL = dot(WS_L,WS_N);
				float lightIntensity = smoothstep(0,0.01,NdotL);
				float4 light = lightIntensity*float4(mainLight.color,1)*mainLight.shadowAttenuation;

				//镜面 高光
				float NdotH = dot(WS_N,WS_H);
				float specularIntensity = pow(abs(NdotH * lightIntensity) ,_Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005,0.01,specularIntensity);//005
				float4 specular = specularIntensitySmooth * _SpecularColor * mainLight.shadowAttenuation;

				//外描边
				float rimDot = 1 - dot(WS_V,WS_N);				
				//我们只希望边缘出现在表面的光亮面，
				//因此，将其乘以NdotL，并提升为平滑融合的能力。 
				float rimIntensity = rimDot * pow(abs(NdotL),_RimThreshold);
				rimIntensity = smoothstep(_RimAmount - 0.01,_RimAmount + 0.01,rimIntensity);
				float4 rim = rimIntensity * _RimColor;

                return mainTex * _MainColor * (light+_AmbientColor + rim + specular);
            }
            ENDHLSL
		}

		//阴影
		Pass {
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }
 
			ZWrite On
			ZTest LEqual
 
			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x gles
			//#pragma target 4.5
 
			// Material Keywords
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
 
			// GPU Instancing
			#pragma multi_compile_instancing
			#pragma multi_compile _ DOTS_INSTANCING_ON
             
			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment
     
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
 
			ENDHLSL
		}
	}
}