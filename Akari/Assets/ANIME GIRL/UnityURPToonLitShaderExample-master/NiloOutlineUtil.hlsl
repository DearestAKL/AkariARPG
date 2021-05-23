// For more information, visit -> https://github.com/ColinLeung-NiloCat/UnityURPToonLitShaderExample

#ifndef Include_NiloOutlineUtil
#define Include_NiloOutlineUtil

// If your project has a faster way to get camera fov in shader, you can replace this slow function to your method.
// For example, you write cmd.SetGlobalFloat("_CurrentCameraFOV",cameraFOV) using a new RendererFeature in C#.
// For this tutorial shader, we will keep things simple and use this slower but convenient method to get camera fov
//如果您的项目有一个更快的方法来在着色器中获取摄影机，则可以将此慢函数替换为您的方法。
//例如，您使用C＃中的新RendererFeature编写cmd.SetGlobalFloat（“ _ CurrentCameraFOV”，cameraFOV）。
//对于本教程的着色器，我们将使事情保持简单，并使用这种较慢但方便的方法来获取摄像机的视场。
float GetCameraFOV()
{
    //https://answers.unity.com/questions/770838/how-can-i-extract-the-fov-information-from-the-pro.html
    float t = unity_CameraProjection._m11;
    float Rad2Deg = 180 / 3.1415;
    float fov = atan(1.0f / t) * 2.0 * Rad2Deg;
    return fov;
}
float ApplyOutlineDistanceFadeOut(float inputMulFix)
{
    //make outline "fadeout" if character is too small in camera's view
    //如果相机视图中的人物太小，则使轮廓“淡出”
    return saturate(inputMulFix);
}
float GetOutlineCameraFovAndDistanceFixMultiplier(float positionVS_Z)
{
    float cameraMulFix;
    if(unity_OrthoParams.w == 0)
    {
        ////////////////////////////////
        // Perspective camera case
        ////////////////////////////////

        // keep outline similar width on screen accoss all camera distance
        // 在所有相机距离上，使轮廓线在屏幕上的宽度保持相似
        cameraMulFix = abs(positionVS_Z);

        // can replace to a tonemap function if a smooth stop is needed
        // 如果需要平稳停止，可以替换为色调图功能
        cameraMulFix = ApplyOutlineDistanceFadeOut(cameraMulFix);

        // keep outline similar width on screen accoss all camera fov
        // 保持所有相机镜头在屏幕上的轮廓宽度相似
        cameraMulFix *= GetCameraFOV();       
    }
    else
    {
        ////////////////////////////////
        // Orthographic camera case
        ////////////////////////////////
        float orthoSize = abs(unity_OrthoParams.y);
        orthoSize = ApplyOutlineDistanceFadeOut(orthoSize);
        // 50是一个魔术数字，用于匹配透视相机的轮廓宽度
        // 50 is a magic number to match perspective camera's outline width
        cameraMulFix = orthoSize * 50; 
    }

    // mul a const to make return result = default normal expand amount WS
    // 多个const使返回结果=默认正常展开量WS
    return cameraMulFix * 0.00005; 
}
#endif

