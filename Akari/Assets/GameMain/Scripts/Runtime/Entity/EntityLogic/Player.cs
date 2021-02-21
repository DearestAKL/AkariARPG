using DG.Tweening;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Akari
{
    public class Player : Entity
    {
        [SerializeField]
        private PlayerData m_PlayerData = null;

        public PlayerData PlayerData => m_PlayerData;

        /// <summary>
        /// 移动到 目标点
        /// </summary>
        public void MoveToTargetPoint(Vector3[] path)
        {
            this.transform.DOPath(path, 0.5f, PathType.Linear, PathMode.Sidescroller2D);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_PlayerData = userData as PlayerData;
            if (m_PlayerData == null)
            {
                Log.Error("Player data is invalid.");
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}
