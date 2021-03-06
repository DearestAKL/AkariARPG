﻿using GameFramework;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;


namespace Akari
{
    public class PlayerComponent : GameFrameworkComponent
    {
        //private enum State
        //{
        //    Locomotion,//地面
        //    Water,//水中
        //    Air,//空中
        //}

        //---

        #region 动画参数

        /// 全局
        private float logicTimer = 0f;
        private const float actionFrameRate = GameUtility.ActionFrameRate;
        /// -----------

        private IActionMachine m_ActionMachine;

        [SerializeField]
        private string configName = null;

        private float animatorTimer = 0;


        #endregion

        #region 英雄数据

        [SerializeField]
        private Hero m_Hero;
        [SerializeField]
        private HeroData m_HeroData;

        private Rigidbody m_HeroRigidbody;
        private Animator m_HeroAnimator;
        #endregion 英雄数据

        protected override void Awake()
        {
            base.Awake();

            //----------------------------------------
            Physics.autoSimulation = false;

            //GameEntry.ObjectPool.CreateMultiSpawnObjectPool
            //----------------------------------------

            m_HeroData = new HeroData(GameEntry.Entity.GenerateSerialId(), (int)EnumEntity.Hero);

            m_ActionMachine = new ActionMachine();
        }


        private void Update()
        {
            if(m_Hero == null) 
            {
                return;
            }

            UpdateAnimation();

            //帧更新
            LogicUpdate();
        }

        private void UpdateAnimation()
        {
            if(animatorTimer <= 0)
            {
                return;
            }

            float deltaTime = Time.deltaTime;
            if(deltaTime < animatorTimer)
            {
                animatorTimer -= deltaTime;
            }
            else
            {
                deltaTime = animatorTimer;
                animatorTimer = 0;
            }

            if(m_HeroAnimator != null)
            {
                //m_HeroAnimator.Update(deltaTime);
            }
        }

        #region 帧更新 一秒30帧
        private void LogicUpdate()
        {
            logicTimer += Time.deltaTime;
            if (logicTimer >= actionFrameRate)
            {
                logicTimer -= actionFrameRate;

                //更新状态
                m_ActionMachine.LogicUpdate(actionFrameRate);
                //更新动画
                UpdateLogicAnimation(actionFrameRate);
                //检测地面
                m_Hero.CheckGround();

                //检测攻击对象
                CheckAttack();

                //更新物理
                Physics.Simulate(actionFrameRate);
                //清理输入
                GameEntry.Input.Clear();
            }
        }

        private void CheckAttack()
        {
            var attackRanges = m_ActionMachine.GetAttackRanges();
            if (attackRanges.Count <= 0) {
                m_Hero.CheckRangeBox(null);
                return;         
            }

            var attackRange = attackRanges[0];

            //检测范围盒
            m_Hero.CheckRangeBox(attackRange);
        }

        private void UpdateLogicAnimation(float deltaTime)
        {
            ActionMachineEvent eventTypes = m_ActionMachine.eventTypes;

            if ((eventTypes & ActionMachineEvent.FrameChanged) != 0)
            {
                animatorTimer += deltaTime;
            }

            if ((eventTypes & ActionMachineEvent.StateChanged) != 0)
            {
                Debug.Log($"StateChanged：{m_ActionMachine.stateName}");
            }

            if (m_HeroAnimator != null && (eventTypes & ActionMachineEvent.AnimChanged) != 0)
            {
                StateConfig config = m_ActionMachine.GetStateConfig();

                float fixedTimeOffset = m_ActionMachine.animStartTime;
                float fadeTime = config.fadeTime;
                string animName = m_ActionMachine.GetAnimName();

                if ((eventTypes & ActionMachineEvent.HoldAnimDuration) != 0)
                {
                    fixedTimeOffset = m_HeroAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                }

                m_HeroAnimator.CrossFadeInFixedTime(animName, fadeTime, 0, fixedTimeOffset);
                m_HeroAnimator.Update(0);
            }
        }

        private void InitAnimation()
        {
            if (m_HeroAnimator == null)
            {
                return;
            }

            string animName = m_ActionMachine.GetAnimName();
            m_HeroAnimator.Play(animName, 0, 0);
            m_HeroAnimator.Update(0);
        }
        #endregion

        #region  英雄
        /// <summary>
        /// 创建英雄
        /// </summary>
        public void CreatHero()
        {
            GameEntry.Entity.ShowEntity<Hero>("Hero", Constant.AssetPriority.PlayerAsset, m_HeroData);

            var carData = new CarData(GameEntry.Entity.GenerateSerialId(), (int)EnumEntity.Motorcycle,30f);
            carData.Position = Vector3.forward;
            GameEntry.Entity.ShowEntity<Car>("Car", Constant.AssetPriority.CarAsset, carData);
        }

        /// <summary>
        /// 回收英雄
        /// </summary>
        public void RecycleHero()
        {
            m_Hero = null;
        }

        /// <summary>
        /// 当前场上英雄
        /// </summary>
        public Hero Hero
        {
            get
            {
                return m_Hero;
            }
            set
            {
                m_Hero = value;

                m_ActionMachine.Initialize(configName, m_Hero);

                //刷新引用
                m_HeroRigidbody = m_Hero.CachedRigidbody;
                m_HeroAnimator = m_Hero.CachedAnimator;

                InitAnimation();

                GameEntry.Camera.SetFreeLookFollowAndLookAt(m_Hero.CachedTransform, m_Hero.CachedLookAtPos);
            }
        }


        /// <summary>
        /// 当前英雄数据
        /// </summary>
        public HeroData HeroData
        {
            get
            {
                return m_HeroData;
            }
        }

        #endregion 英雄
    }
}
