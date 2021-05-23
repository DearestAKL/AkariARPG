// For more information, visit -> https://github.com/ColinLeung-NiloCat/UnityURPToonLitShaderExample

#ifndef Include_NiloZOffset
#define Include_NiloZOffset

// Push an imaginary vertex towards camera in view space (linear, view space unit), 
// then only overwrite original positionCS.z using imaginary vertex's result positionCS.z value
// Will only affect ZTest ZWrite's depth value of vertex shader
//在视图空间（线性，视图空间单位）中向相机推动一个虚构顶点，
//然后仅使用虚拟顶点的结果positionCS.z值覆盖原始positionCS.z
//仅会影响ZTest ZWrite顶点着色器的深度值

// Useful for:
// -Hide ugly outline on face/eye
// -Make eyebrow render on top of hair
// -Solve ZFighting issue without moving geometry
// 用于：
//-将丑陋的轮廓隐藏在脸部/眼睛上
//-使眉毛在头发上呈现
//-解决ZFighting问题而不移动几何
float4 NiloGetNewClipPosWithZOffset(float4 originalPositionCS, float viewSpaceZOffsetAmount)
{
    if(unity_OrthoParams.w == 0)
    {
        ////////////////////////////////
        //Perspective camera case
        ////////////////////////////////
        float2 ProjM_ZRow_ZW = UNITY_MATRIX_P[2].zw;
        float modifiedPositionVS_Z = -originalPositionCS.w + -viewSpaceZOffsetAmount; // push imaginary vertex
        float modifiedPositionCS_Z = modifiedPositionVS_Z * ProjM_ZRow_ZW[0] + ProjM_ZRow_ZW[1];
        originalPositionCS.z = modifiedPositionCS_Z * originalPositionCS.w / (-modifiedPositionVS_Z); // overwrite positionCS.z
        return originalPositionCS;    
    }
    else
    {
        ////////////////////////////////
        //Orthographic camera case
        ////////////////////////////////
        originalPositionCS.z += -viewSpaceZOffsetAmount / _ProjectionParams.z; // push imaginary vertex and overwrite positionCS.z
        return originalPositionCS;
    }
}

#endif

