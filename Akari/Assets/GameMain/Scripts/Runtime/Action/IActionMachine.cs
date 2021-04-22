using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    [Flags]
    public enum ActionMachineEvent
    {
        None = 1,
        FrameChanged = 2,
        StateChanged = 3,
        AnimChanged = 4,
        HoldAnimDuration = 5,

        All = 6
    }
    public interface IActionMachine
    {
        bool isDebug { get; set; }
        System.Object controller { get; }
        Single animStartTime { get; }

        void Initialize(string config, System.Object controller);

        void LogicUpdate(Single delta);

        void ChangeState(string stateName, int priority = 0, int animIndex = -1, Single animStartTime = default);

        ActionMachineEvent eventTypes { get; }

        int animIndex { get; }

        int frameIndex { get; }
        int stateBeginFrameIndex { get; }

        int waitFrameCnt { get; set; }
        int waitFrameDelay { get; set; }

        string configName { get; }
        string stateName { get; }

        string GetAnimName();

        MachineConfig GetMachineConfig();

        StateConfig GetStateConfig();

        int GetStateFrameIndex();

        FrameConfig GetStateFrame();

        int GetStateLoopCnt();

        List<RangeConfig> GetAttackRanges();

        List<RangeConfig> GetBodyRanges();

        void Destroy();

        void ChangeAnim(int animIndex, bool holdDuration = false);

        void ReplayAnim();
    }
}
