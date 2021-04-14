// 圆形范围
Shader "URP/Custom/Indicator"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0.17,0.36,0.81,0.0)
        _Angle ("Angle", Range(0, 360)) = 60
        _Gradient ("Gradient", Range(0, 1)) = 0
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

        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_ST;
        float4 _Color;
        float _Angle;
        float _Gradient;
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
        };

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        ENDHLSL

        Pass
        {        
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(a2v i)
            {
                v2f o;
                o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
                o.texcoord=TRANSFORM_TEX(i.texcoord,_MainTex);
                return o;
            }

            float4 frag(v2f i):SV_Target
            {
                half4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);                

                // 离中心的距离
                float distance = sqrt(pow(i.texcoord.x - 0.5,2)+pow(i.texcoord.y - 0.5,2));
                // 在圆外
                if(distance > 0.5f){
                    discard;
                }
                // 根据距离计算透明度渐变
                float grediant = (1 - distance - 0.5 * _Gradient)/0.5;
                // 正常显示的结果
                half4 result = mainTex * _Color * half4(1,1,1,grediant);
                float x = i.texcoord.x;
                float y = i.texcoord.y;
                float deg2rad = 0.017453; // 角度转弧度
                // 根据角度剔除掉不需要显示的部分
                // 大于180度
                if(_Angle > 180)
                {
                    if(y > 0.5 && abs(0.5 - y) >= abs(0.5 - x) / tan((180 - _Angle / 2) * deg2rad))
                        discard;// 剔除
                }
                else    // 180度以内
                {
                    if(y > 0.5 || abs(0.5 -y) < abs(0.5 - x) / tan(_Angle / 2 * deg2rad))
                        discard;
                }
                return result;
            }
            ENDHLSL
        }
    }
}
