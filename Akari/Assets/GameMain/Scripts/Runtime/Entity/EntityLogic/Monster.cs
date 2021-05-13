using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework;
using TMPro;
using UnityGameFramework.Runtime;
using Sirenix.OdinInspector;

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

        private Camera m_MainCamera;
        private InfoBarEx m_HPBar = null;
        private Vector2 originOff;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_MainCamera = GameEntry.Camera.MainCamera;
            //var uiHPBarCanvas = GameEntry.UI.GetUIForm(UIFormId.UIHPBarCanvas)?.GetComponent<UIHPBarCanvas>();
            //m_HPBar = GameEntry.UI.GetUIForm(UIFormId.UIHPBarCanvas)?.GetComponent<UIHPBarCanvas>()?.GetHpBar();

            if (ui != null)
            {
                m_HPBar = new InfoBarEx(ui.GetComponent<ReferenceCollector>());
            }

            originOff = new Vector2(-Screen.width / 2, -Screen.height / 2);
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

            //txtInfo.text = Utility.Text.Format("生命值：{0}/{1}",m_MonsterData.HP, m_MonsterData.MaxHP);
            //CreateFsm();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_HPBar == null)
            {
                return;
            }

            //var v2 =  RectTransformUtility.WorldToScreenPoint(m_MainCamera, CachedTransform.position + Vector3.up * 2.5f) + originOff;
            //var position = new Vector3(v2.x, v2.y, CachedTransform.position.z);
            m_HPBar.CachedTransform.position = CachedTransform.position + Vector3.up * 2.5f;
            m_HPBar.CachedTransform.LookAt(m_MainCamera.transform);

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

        [Button("UpdateUI")]
        private void UpdateUI()
        {
            if (ui != null)
            {
                m_HPBar = new InfoBarEx(ui.GetComponent<ReferenceCollector>());
            }
        }
    }
}