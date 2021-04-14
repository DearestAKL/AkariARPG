using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework;

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

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            stateList = new List<FsmState<Monster>>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            CreateFsm();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            DestroyFsm();
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