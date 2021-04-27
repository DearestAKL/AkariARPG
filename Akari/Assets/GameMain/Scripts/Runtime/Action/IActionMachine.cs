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
        None = 0b0000_0000,
        FrameChanged = 0b0000_0001,
        StateChanged = 0b0000_0010,
        AnimChanged = 0b0000_0100,
        HoldAnimDuration = 0b0000_1000,

        All = 0b1111_1111
    }
    public interface IActionMachine
    {
        bool isDebug { get; set; }
        System.Object controller { get; }
        float animStartTime { get; }

        void Initialize(string config, System.Object controller);

        void LogicUpdate(float delta);

        void ChangeState(string stateName, int priority = 0, int animIndex = -1, float animStartTime = default);

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
