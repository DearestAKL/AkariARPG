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

        public bool IsDead
        {
            get
            {
                return m_TargetableData.HP <= 0;
            }
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="damageHP">伤害值</param>
        public virtual void ApplyDamage(Entity attacker, int damageHP)
        {

        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
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

        /// <summary>
        /// 死亡
        /// </summary>
        /// <param name="attacker">攻击者</param>
        protected virtual void OnDead(Entity attacker)
        {

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
    }
}