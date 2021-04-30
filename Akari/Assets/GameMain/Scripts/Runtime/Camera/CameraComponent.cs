using UnityEngine;
using UnityGameFramework.Runtime;
using Cinemachine;

namespace Akari
{
    /// <summary>
    /// 摄像机管理类
    /// </summary>
    public class CameraComponent : GameFrameworkComponent
    {
        [SerializeField]
        private Camera m_UICamera;
        [SerializeField]
        private Camera m_MainCamera;

        private CinemachineFreeLook m_CMFreeLook;

        protected override void Awake()
        {
            base.Awake();

            if(m_UICamera == null)
            {
                m_UICamera = transform.Find("UI Camera").GetComponent<Camera>();
            }

            if(m_MainCamera == null)
            {
                m_MainCamera = transform.Find("Main Camera").GetComponent<Camera>();
            }

            if (m_CMFreeLook == null)
            {
                m_CMFreeLook = transform.Find("CM FreeLook").GetComponent<CinemachineFreeLook>();
            }

            //m_CMFreeLook.SetGoActive(false);
        }

        #region 外部引用接口
        /// <summary>
        /// UI摄像机
        /// </summary>
        public Camera UICamera
        {
            get { return m_UICamera; }
        }

        /// <summary>
        /// 主摄像机
        /// </summary>
        public Camera MainCamera
        {
            get { return m_MainCamera; }
        }
        #endregion

        #region 外部行为接口

        /// <summary>
        /// 设置 CM FreeLook 的 跟随目标 和 观察目标
        /// </summary>
        /// <param name="follow">跟随目标</param>
        /// <param name="lookAt">观察目标</param>
        public void SetFreeLookFollowAndLookAt(Transform follow,Transform lookAt)
        {
            m_CMFreeLook.Follow = follow;
            m_CMFreeLook.LookAt = lookAt;
            //m_CMFreeLook.SetGoActive(true);
        }
        #endregion
    }
}
