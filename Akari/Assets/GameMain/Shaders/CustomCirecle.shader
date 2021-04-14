// 圆形范围
Shader "URP/Custom/Circle"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {} 
        _BaseColor("Base Color",Color)=(1,1,1,1)
        _RoundWidth("Round Width",float) = 0.03;
    }
    SubShader
    {
        Tags 
        { 
            "RenderPipeline"="UniversalRenderPipeline"
            "RenderType"="Opaque" 
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex;
        half4 _BaseColor;
        float _RoundWidth;
        CBUFFER_END

        struct a2v
        {
            float4 positionOS : POSITION;
            float2 texcoord : TEXCOORD;
        };

        struct v2f
        {
            float4 positionCS : SV_POSITION;
            float2 texcoord : TEXCOORD;
            float4 positionOS : TEXCOORD1;
        };

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        ENDHLSL

        Pass
        {        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(a2v i)
            {
                v2f o;
                o.positionOS = i.positionOS;
                o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
                o.texcoord=TRANSFORM_TEX(i.texcoord,_MainTex);
                return o;
            }

            float4 frag(v2f i):SV_Target
            {
                float dis = sqrt(i.positionOS.x * i.positionOS.x + i.positionOS.y * i.positionOS.y)
                float maxDistance = 0.05;

                half4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);                
                return mainTex * _BaseColor;
            }
            ENDHLSL
        }
    }
}
