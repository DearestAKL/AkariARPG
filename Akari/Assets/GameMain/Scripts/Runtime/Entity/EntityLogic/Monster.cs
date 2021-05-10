using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework;
using TMPro;

namespace Akari
{
    /// <summary>
    /// 怪物类
    /// </summary>
    public class Monster : TargetableObject
    {
        [SerializeField]
        private MonsterData m_MonsterData = null;

        protected IFsm<Monster> fsm;
        protected List<FsmState<Monster>> stateList;

        private TextMeshPro txtInfo;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            txtInfo = CachedTransform.Find("txtInfo").GetComponent<TextMeshPro>();

            CachedTransform.position = new Vector3(10, 0, 0);
            //stateList = new List<FsmState<Monster>>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            txtInfo.text = Utility.Text.Format("生命值：{0}/{1}",m_MonsterData.HP, m_MonsterData.MaxHP);
            //CreateFsm();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            //DestroyFsm();
        }

        protected override void OnAttachTo(UnityGameFramework.Runtime.EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }

        protected override void OnDetached(UnityGameFramework.Runtime.EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        public override void ApplyDamage(Entity attacker, int damageHP)
        {
            base.ApplyDamage(attacker, damageHP);

            m_MonsterData.HP -= damageHP;

            if (m_MonsterData.HP <= 0)
            {
                OnDead(attacker);
                return;
            }

            txtInfo.text = Utility.Text.Format("生命值：{0}/{1}", m_MonsterData.HP, m_MonsterData.MaxHP);
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);

            txtInfo.text = Utility.Text.Format("死亡：0/{0}", m_MonsterData.MaxHP);
        }

        #region FSM
        /// <summary>
        /// 创建FSM
        /// </summary>
        private void CreateFsm()
        {
            AddFsmState();
            fsm = GameEntry.Fsm.CreateFsm<Monster>(gameObject.name, this, stateList);
            StartFsm();
        }

        /// <summary>
        /// 回收FSM
        /// </summary>
        private void DestroyFsm()
        {
            GameEntry.Fsm.DestroyFsm(fsm);
            foreach (var item in stateList)
            {
                ReferencePool.Release((IReference)item);
            }

            stateList.Clear();
            fsm = null;
        }

        /// <summary>
        /// 添加FsmState 到 stateList
        /// </summary>
        protected virtual void AddFsmState()
        {
            stateList.Add(EnemyIdleState.Create());
        }

        /// <summary>
        /// 启动状态机(指定初始状态)
        /// </summary>
        protected virtual void StartFsm()
        {
            fsm.Start<EnemyIdleState>();
        }
        #endregion
    }
}