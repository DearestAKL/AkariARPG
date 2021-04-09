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
    /// 玩家类
    /// </summary>
    public class Hero : TargetableObject
    {
        [SerializeField]
        private HeroData m_HeroData = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
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

        public override void ApplyDamage(Entity attacker, int damageHP)
        {
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

        public void RestoreHealth(Entity curer,int restoreHP)
        {
            float fromHPRatio = m_HeroData.HPRatio;
            m_HeroData.HP += restoreHP;
            float toHPRatio = m_HeroData.HPRatio;
        }
    }
}