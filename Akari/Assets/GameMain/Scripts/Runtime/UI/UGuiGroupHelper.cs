﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Akari
{
    /// <summary>
    /// uGUI 界面组辅助器。
    /// </summary>
    public class UGuiGroupHelper : UIGroupHelperBase
    {
        public const int DepthFactor = 10000;

        private int m_Depth = 0;
        private Canvas m_CachedCanvas = null;

        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public override void SetDepth(int depth)
        {
            m_Depth = depth;
            m_CachedCanvas.overrideSorting = true;
            m_CachedCanvas.sortingOrder = DepthFactor * depth;
        }

        public override void SetUICamera(Camera uiCamera)
        {
            m_CachedCanvas.worldCamera = uiCamera;
        }

        private void Awake()
        {
            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            m_CachedCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            gameObject.GetOrAddComponent<GraphicRaycaster>();

            Vector2 screenn = new Vector2(1920, 1080);
            CanvasScaler m_CanvasScale = gameObject.GetOrAddComponent<CanvasScaler>();
            m_CanvasScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            m_CanvasScale.referenceResolution = screenn;
            m_CanvasScale.matchWidthOrHeight = 1;
        }

        private void Start()
        {
            m_CachedCanvas.overrideSorting = true;
            m_CachedCanvas.sortingOrder = DepthFactor * m_Depth;

            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
        }
    }
}