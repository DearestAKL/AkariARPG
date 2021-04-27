using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    /// <summary>
    /// ActionNode
    /// </summary>
    public class ActionNode
    {
        public IActionMachine actionMachine;
        public int beginFrameIndex;
        public object config;
        public FsmState<Hero> handler;
        public object data;
        public bool isUpdating { get; private set; } = false;
        public int updateCnt { get; private set; } = 0;

        public override string ToString()
        {
            return $"动作节点：{actionMachine.configName}-{actionMachine.GetStateConfig().stateName}-{config.GetType().Name}-{actionMachine.GetStateFrameIndex()}";
        }

        public void Reset()
        {
            actionMachine = null;
            beginFrameIndex = -1;
            config = null;
            handler = null;
            isUpdating = false;
            updateCnt = 0;
            data = null;
        }

        //public void InvokeEnter()
        //{
        //    updateCnt = 0;
        //    isUpdating = true;
        //    // TODO：进入状态
        //    //handler.OnEnter(this);
        //}

        //public void InvokeExit()
        //{
        //    //handler.Onex(this);
        //    // TODO：退出状态
        //    isUpdating = false;
        //}

        //public void InvokeUpdate(float deltaTime)
        //{
        //    if (!isUpdating)
        //    {
        //        return;
        //    }

        //    // TODO：在状态中
        //    //handler.Update(this, deltaTime);
        //    updateCnt++;
        //}
    }
}
