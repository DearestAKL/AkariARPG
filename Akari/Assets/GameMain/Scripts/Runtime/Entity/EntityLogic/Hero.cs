using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Akari
{
    /// <summary>
    /// 英雄类
    /// </summary>
    public class Hero : TargetableObject
    {
        [SerializeField]
        private HeroData m_HeroData = null;


        [SerializeField]
        private Animator m_Animator = null;
        [SerializeField]
        private Rigidbody m_Rigidbody = null;

        private Transform m_LookAtPos = null;

        private PlayerComponent m_Player = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            //查找组件
            m_Rigidbody = CachedTransform.GetComponent<Rigidbody>();
            m_Animator = CachedTransform.Find("HeroModel").GetComponent<Animator>();
            m_LookAtPos = CachedTransform.Find("LookAt").GetComponent<Transform>();

            m_Player = GameEntry.Player;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_HeroData = userData as HeroData;
            if (m_HeroData == null)
            {
                Log.Error("Hero data is invalid.");
                return;
            }

            //英雄创建成功 赋值到PlayerCommpont
            m_Player.Hero = this;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnAttachTo(UnityGameFramework.Runtime.EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }

        protected override void OnDetached(UnityGameFramework.Runtime.EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);
        }


        #region 向PlayerComponent传递信息

        private void OnCollisionEnter(Collision collision)
        {
            m_Player.EvaluateCollision(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            m_Player.EvaluateCollision(collision);
        }

        #endregion

        #region 外部行为接口

        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="damageHP">伤害值</param>
        public override void ApplyDamage(Entity attacker, int damageHP)
        {
            base.ApplyDamage(attacker, damageHP);

            float fromHPRatio = m_HeroData.HPRatio;
            m_HeroData.HP -= damageHP;
            float toHPRatio = m_HeroData.HPRatio;
            if (fromHPRatio > toHPRatio)
            {
                GameEntry.Event.Fire(HeroApplyDamageEventArgs.EventId, HeroApplyDamageEventArgs.Create(fromHPRatio, toHPRatio));
            }

            if (m_HeroData.HP <= 0)
            {
                OnDead(attacker);
            }
        }

        /// <summary>
        /// 恢复生命值
        /// </summary>
        /// <param name="curer">治疗者</param>
        /// <param name="restoreHP">恢复值</param>
        public override void RestoreHealth(Entity curer, int restoreHP)
        {
            if (m_HeroData.HP == m_HeroData.MaxHP)
            {
                //todo:已满血
                return;
            }

            float fromHPRatio = m_HeroData.HPRatio;
            m_HeroData.HP += restoreHP;
            float toHPRatio = m_HeroData.HPRatio;
            if (toHPRatio > fromHPRatio)
            {
                GameEntry.Event.Fire(HeroRestoreHealthEventArgs.EventId, HeroRestoreHealthEventArgs.Create(fromHPRatio, toHPRatio));
            }
        }
        #endregion

        #region 外部引用接口
        /// <summary>
        /// 缓存的Animator
        /// </summary>
        public Animator CachedAnimator
        {
            get { return m_Animator; }
        }

        /// <summary>
        /// 缓存的Rigidbody
        /// </summary>
        public Rigidbody CachedRigidbody
        {
            get { return m_Rigidbody; }
        }

        public Transform CachedLookAtPos
        {
            get { return m_LookAtPos; }
        }
        #endregion
    }
}