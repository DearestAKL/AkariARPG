﻿using GameFramework;
using GameFramework.Fsm;
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityGameFramework.Runtime;

namespace Akari
{
    /// <summary>
    /// ActionMachine
    /// </summary>
    public class ActionMachine : IActionMachine
    {
        public bool isDebug { get; set; }
        public int waitFrameCnt { get; set; }
        public int waitFrameDelay { get; set; }
        public int frameIndex { get; protected set; }
        public int globalFrameIndex { get; protected set; }
        public int stateBeginFrameIndex { get; protected set; }

        public int animIndex { get; protected set; }
        public float animStartTime { get; protected set; }

        public string configName { get; protected set; }

        public string stateName
        {
            get => cacheStateName;
            protected set
            {
                cacheStateName = value;
                isCacheSConfigDirty = true;
            }
        }

        private IFsm<Hero> fsm;
        protected List<FsmState<Hero>> stateList = new List<FsmState<Hero>>();

        private List<ActionNode> globalActionNodes { get; set; } = new List<ActionNode>();
        private List<ActionNode> actionNodes { get; set; } = new List<ActionNode>();

        public ActionMachineEvent eventTypes { get; protected set; }

        public System.Object controller { get; protected set; }

        public string nextStateName { get; protected set; }
        public int nextAnimIndex { get; protected set; }
        public float nextAnimStartTime { get; protected set; }
        public int nextStatePriority { get; protected set; }

        protected bool isCacheSConfigDirty;
        protected string cacheStateName;
        protected MachineConfig cacheMConfig;
        protected StateConfig cacheSConfig;


        public string GetAnimName() => GetStateConfig().GetAnimName(animIndex);

        public void ChangeAnim(int animIndex, bool holdDuration = false)
        {
            throw new NotImplementedException("功能未测试通过，请勿调用");
            /*
            this.animIndex = animIndex;
            eventTypes |= ActionMachineEvent.AnimChanged;
            eventTypes = holdDuration
            ? eventTypes | ActionMachineEvent.HoldAnimDuration
            : eventTypes & ~ActionMachineEvent.HoldAnimDuration;
            */
        }

        public void ReplayAnim()
        {
            eventTypes |= ActionMachineEvent.AnimChanged;
            eventTypes &= ~ActionMachineEvent.HoldAnimDuration;
        }

        public MachineConfig GetMachineConfig()
        {
            return cacheMConfig ?? (cacheMConfig = ActionMachineHelper.GetMachineConfig(configName));
        }

        public StateConfig GetStateConfig()
        {
            if (isCacheSConfigDirty || cacheSConfig == null)
            {
                cacheSConfig = ActionMachineHelper.GetStateConfig(configName, stateName);
                isCacheSConfigDirty = false;
            }

            return cacheSConfig;
        }

        public int GetStateFrameIndex()
        {
            StateConfig config = GetStateConfig();
            int interval = frameIndex - stateBeginFrameIndex;
            int frameMax = config.frames.Count;
            if (config.enableLoop && interval + 1 > frameMax)
            {
                interval %= frameMax;
            }
            return interval;
        }

        public FrameConfig GetStateFrame()
        {
            StateConfig config = GetStateConfig();
            if (config == null)
            {
                return null;
            }

            int index = GetStateFrameIndex();
            if (index >= 0 && index < config.frames.Count)
            {
                return config.frames[index];
            }

            return null;
        }

        public int GetStateLoopCnt()
        {
            StateConfig config = GetStateConfig();
            int loopCnt = 0;
            if (!config.enableLoop)
            {
                return loopCnt;
            }

            int interval = frameIndex - stateBeginFrameIndex;
            int frameMax = config.frames.Count;
            if (interval + 1 > frameMax)
            {
                loopCnt = Mathf.FloorToInt(interval / frameMax);
            }

            return loopCnt;
        }

        public List<RangeConfig> GetAttackRanges()
        {
            StateConfig sconfig = GetStateConfig();
            List<RangeConfig> ranges = sconfig.GetAttackRanges(GetStateFrameIndex());
            return ranges;
        }

        public List<RangeConfig> GetBodyRanges()
        {
            StateConfig sconfig = GetStateConfig();
            List<RangeConfig> ranges = sconfig.GetBodyRanges(GetStateFrameIndex());
            return ranges;
        }

        public void ChangeState(string stateName, int priority = 0, int animIndex = -1, float animStartTime = default)
        {
            if (!string.IsNullOrEmpty(stateName) && priority < nextStatePriority)
            {
                return;
            }

            nextStateName = stateName;
            nextStatePriority = priority;
            nextAnimIndex = animIndex;
            nextAnimStartTime = animStartTime;

#if UNITY_EDITOR
            if (isDebug)
            {
                Debug.Log($"ChangeState {this.stateName}=>{nextStateName}");
            }
#endif
        }

        public void Initialize(string configName, System.Object controller)
        {
            waitFrameCnt = 0;
            waitFrameDelay = 0;
            frameIndex = -1;
            globalFrameIndex = -1;
            stateBeginFrameIndex = -1;
            animIndex = 0;
            animStartTime = default;
            nextStateName = null;
            nextStatePriority = 0;
            nextAnimIndex = -1;
            nextAnimStartTime = default;
            eventTypes = ActionMachineEvent.None;

            isCacheSConfigDirty = true;
            cacheStateName = null;
            cacheMConfig = null;
            cacheSConfig = null;

            globalActionNodes.Clear();
            actionNodes.Clear();

            this.configName = configName;
            this.controller = controller;

            //初始化全局动作
            MachineConfig mConfig = GetMachineConfig();
            InitializeActions(globalActionNodes, mConfig.globalActions, 0);

            //初始化第一个状态
            stateName = mConfig.firstStateName;
            StateConfig sConfig = GetStateConfig();
            stateBeginFrameIndex = frameIndex + 1;
            animIndex = sConfig.dafualtAnimIndex;
            animStartTime = default;
            InitializeActions(actionNodes, sConfig.actions, 0); //初始化新的动作


        }

        public void Destroy()
        {
            DisposeActions(actionNodes);
            DisposeActions(globalActionNodes);
        }

        public void LogicUpdate(float deltaTime)
        {
            InitializeValue();
            UpdateState();
            UpdateGlobalFrame(deltaTime);
            UpdateState();
            UpdateFrame(deltaTime);
        }

        #region action operations

        private void InitializeActions(List<ActionNode> target, List<object> actions, int index)
        {
            for (int i = 0, count = actions.Count; i < count; i++)
            {
                AddAction(actions[i]);
            }

            return;

            void AddAction(object action)
            {
                ActionNode actionNode = ActionMachineHelper.CreateActionNode();
                actionNode.actionMachine = this;
                actionNode.config = action;
                actionNode.beginFrameIndex = index;
                actionNode.handler = ActionMachineHelper.GetActionHandler(action.GetType());
                target.Add(actionNode);
            }
        }

        private void DisposeActions(List<ActionNode> target)
        {
            for (int i = 0, count = target.Count; i < count; i++)
            {
                RemoveAction(target[i]);
            }
            target.Clear();

            return;

            void RemoveAction(ActionNode action)
            {
                if (action.isUpdating)
                {
                    //action.InvokeExit();
                }
                ActionMachineHelper.RecycleActionNode(action);
            }
        }

        private void UpdateActions(List<ActionNode> target, float deltaTime, int index)
        {
            for (int i = 0, count = target.Count; i < count; i++)
            {
                UpdateAction(target[i]);
            }
            return;

            void UpdateAction(ActionNode action)
            {
                if (action.config is IHoldFrames hold && hold.EnableBeginEnd())
                {
                    if (!hold.EnableLoop() && GetStateLoopCnt() > 0)
                    {//只在第一次循环执行
                        return;
                    }

                    if (hold.GetBeginFrame() == index)
                    {
                        //action.InvokeEnter();
                    }

                    //action.InvokeUpdate(deltaTime);

                    if (hold.GetEndFrame() == index)
                    {
                        //action.InvokeExit();
                    }
                }
                else
                {
                    if (action.updateCnt == 0)
                    {
                        //action.InvokeEnter();
                    }

                    //action.InvokeUpdate(deltaTime);
                }
            }
        }

        #endregion action operations

        private void InitializeValue()
        {
            eventTypes = ActionMachineEvent.None;

            //if (globalFrameIndex < 0)
            //{//初始状态是改变
            //    eventTypes |= (ActionMachineEvent.StateChanged | ActionMachineEvent.AnimChanged);
            //}
        }

        private void UpdateGlobalFrame(float deltaTime)
        {
            //全局帧，不受顿帧影响
            globalFrameIndex++;

            //更新全局动作-不被顿帧影响
            UpdateActions(globalActionNodes, deltaTime, globalFrameIndex);
        }

        private void UpdateFrame(float deltaTime)
        {
            StateConfig sConfig = GetStateConfig();
            if (null == sConfig)
            {
                throw new GameFrameworkException("没有状态配置");
            }

            if (waitFrameDelay > 0)
            {
                waitFrameDelay--;
            }
            else if (waitFrameCnt > 0)
            { //顿帧
                waitFrameCnt--;
                return;
            }

            //帧增加
            frameIndex++;

            int index = GetStateFrameIndex();

            int maxFrameCnt = sConfig.frames.Count;
            if (index >= maxFrameCnt)
            {
                throw new GameFrameworkException($"当前状态 {sConfig.stateName} 帧序号 {index} 超过上限 {sConfig.frames.Count}");
            }

            //更新动作
            UpdateActions(actionNodes, deltaTime, index);

            //更新事件
            eventTypes |= ActionMachineEvent.FrameChanged;

            if (!sConfig.enableLoop && index + 1 == maxFrameCnt && string.IsNullOrEmpty(nextStateName))
            { //最后一帧
                ChangeState(sConfig.nextStateName, animIndex: sConfig.nextAnimIndex);
            }
        }

        private void UpdateState()
        {
            if (string.IsNullOrEmpty(nextStateName))
            {
                return;
            }

#if UNITY_EDITOR
            if (isDebug)
            {
                Log.Info($"UpdateState {this.stateName}=>{nextStateName}");
            }
#endif

            stateName = nextStateName;//设置新的状态
            stateBeginFrameIndex = frameIndex + 1;//状态起始帧

            StateConfig sConfig = GetStateConfig();

            animIndex = nextAnimIndex < 0 ? sConfig.dafualtAnimIndex : nextAnimIndex;
            animStartTime = nextAnimStartTime;

            //释放已有动作
            DisposeActions(actionNodes);
            //初始化新的动作
            InitializeActions(actionNodes, sConfig.actions, frameIndex);

            nextStateName = null;
            nextStatePriority = 0;
            nextAnimIndex = -1;
            nextAnimStartTime = default;

            //状态改变
            eventTypes |= (ActionMachineEvent.StateChanged | ActionMachineEvent.AnimChanged);
        }
    }
}
