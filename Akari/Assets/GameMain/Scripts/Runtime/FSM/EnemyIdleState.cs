using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using ProcedureOwner = GameFramework.Fsm.IFsm<Akari.Monster>;

namespace Akari
{
    public class EnemyIdleState : FsmState<Monster>, IReference
    {
        private Monster owner;
        private Hero m_TargetHero;

        public EnemyIdleState()
        {

        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            owner = procedureOwner.Owner;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        public static EnemyIdleState Create()
        {
            EnemyIdleState state = ReferencePool.Acquire<EnemyIdleState>();
            return state;
        }

        public void Clear()
        {
            owner = null;
            m_TargetHero = null;
        }

    }
}
