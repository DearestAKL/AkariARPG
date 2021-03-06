﻿using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework;
using TMPro;
using UnityGameFramework.Runtime;
using Sirenix.OdinInspector;
using Akari.HUD;

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

        [SerializeField]
        private GameObject ui;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            //stateList = new List<FsmState<Monster>>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_MonsterData = userData as MonsterData;
            if (m_MonsterData == null)
            {
                Log.Error("Monster data is invalid.");
                return;
            }

            UpdateTitle();
            //txtInfo.text = Utility.Text.Format("生命值：{0}/{1}",m_MonsterData.HP, m_MonsterData.MaxHP);
            //CreateFsm();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
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
            HUDNumberRender.Instance.AddHudNumber(CachedTransform, HUDNumberRenderType.HUD_SHOW_COMMON_ATTACK, damageHP, true, false, false);
            hudTitleInfo.SetBloodPos(m_MonsterData.HPRatio);

            if (m_MonsterData.HP <= 0)
            {
                OnDead(attacker);
                return;
            }

            //txtInfo.text = Utility.Text.Format("生命值：{0}/{1}", m_MonsterData.HP, m_MonsterData.MaxHP);
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);

            //txtInfo.text = Utility.Text.Format("死亡：0/{0}", m_MonsterData.MaxHP);
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

        #region HUD
        private HUDTitleInfo hudTitleInfo;
        private int m_TitleIns = 0;
        private bool m_Main;
        private HUDBloodType m_nBloodType = HUDBloodType.Blood_Red;
        private void UpdateTitle()
        {
            if (m_TitleIns == 0)
            {
                m_TitleIns = HUDTitleInfo.HUDTitleRender.Instance.RegisterTitle(transform, 1.8f, m_Main);
            }

            hudTitleInfo = HUDTitleInfo.HUDTitleRender.Instance.GetTitle(m_TitleIns);
            hudTitleInfo.Clear();

            hudTitleInfo.SetOffsetY(1.5f);
            hudTitleInfo.ShowTitle(true);
            // 血条
            HUDBloodType nBloodType = m_nBloodType;
            if (nBloodType != HUDBloodType.Blood_None)
            {
                hudTitleInfo.BeginTitle();
                hudTitleInfo.PushBlood(nBloodType, m_MonsterData.HPRatio);
                hudTitleInfo.EndTitle();
            }

            hudTitleInfo.BeginTitle();
            hudTitleInfo.PushTitle("Lv.1", HUDTilteType.PlayerName, 0);
            hudTitleInfo.EndTitle();
        }
        #endregion
    }
}