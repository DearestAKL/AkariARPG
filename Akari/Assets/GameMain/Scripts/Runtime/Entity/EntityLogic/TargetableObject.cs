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
    /// 可作为目标的实体类
    /// </summary>
    public abstract class TargetableObject : Entity
    {
        [SerializeField]
        private TargetableData m_TargetableData = null;
        [SerializeField]
        private Animator m_Animator = null;
        [SerializeField]
        private Rigidbody m_Rigidbody = null;
        [SerializeField]
        private bool m_IsGround = true;
        [SerializeField]
        private LayerMask m_GoundMask;


        public bool IsDead
        {
            get
            {
                return m_TargetableData.HP <= 0;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            //查找组件
            m_Rigidbody = CachedTransform.GetComponent<Rigidbody>();
            m_Animator = CachedTransform.Find("HeroModel").GetComponent<Animator>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_TargetableData = userData as TargetableData;
            if (m_TargetableData == null)
            {
                Log.Error("Creature object data is invalid.");
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();
            if (entity == null)
            {
                return;
            }

            if (entity is TargetableObject && entity.Id >= Id)
            {
                // 碰撞事件由 Id 小的一方处理，避免重复处理
                return;
            }

            //AIUtility.PerformCollision(this, entity);
        }

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

        public bool IsGround
        {
            get
            {
                return m_IsGround && Mathf.Approximately(m_Rigidbody.velocity.y, 0);
            }
        }

        #endregion

        #region 行为接口

        public void CheckGround()
        {
            float length = 0.02f;
            m_IsGround = m_Rigidbody.velocity.y > 0 ? false : Physics.Raycast(CachedTransform.position + length * Vector3.up, Vector3.down, length * 2, m_GoundMask);
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="damageHP">伤害值</param>
        public virtual void ApplyDamage(Entity attacker, int damageHP)
        {

        }

        /// <summary>
        /// 恢复生命值
        /// </summary>
        /// <param name="curer">治疗者</param>
        /// <param name="restoreHP">恢复值</param>
        public virtual void RestoreHealth(Entity curer, int restoreHP)
        {

        }

        /// <summary>
        /// 死亡
        /// </summary>
        /// <param name="attacker">攻击者</param>
        protected virtual void OnDead(Entity attacker)
        {

        }
        #endregion
    }
}