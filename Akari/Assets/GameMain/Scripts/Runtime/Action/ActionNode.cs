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
        public IActionHandler handler;
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

        public void InvokeEnter()
        {
            updateCnt = 0;
            isUpdating = true;
            handler.Enter(this);
        }

        public void InvokeExit()
        {
            handler.Exit(this);
            isUpdating = false;
        }

        public void InvokeUpdate(float deltaTime)
        {
            if (!isUpdating)
            {
                return;
            }

            handler.Update(this, deltaTime);
            updateCnt++;
        }
    }
}
