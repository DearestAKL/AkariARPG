using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// 状态配置
    /// </summary>
    [Serializable]
    public class StateConfig
    {
        public string stateName = "New State";
        public List<string> animNames;
        public int dafualtAnimIndex = 0;
        public float fadeTime = 1 / 20f;
        public bool enableLoop = false;
        public string nextStateName = "";
        public int nextAnimIndex = -1;

        public List<FrameConfig> frames = new List<FrameConfig>();

        [SerializeReference]
        public List<object> actions = new List<object>();

        public override string ToString() => stateName;

        public string defaultAnimaName => GetAnimName(dafualtAnimIndex);

        public string GetAnimName(int index)
        {
            return animNames?.Count > index ? animNames[index] : string.Empty;
        }

        public FrameConfig GetBodyRangesFrame(int frameIndex)
        {
            if (frames.Count == 0 || frameIndex < 0)
            {
                return null;
            }

            frameIndex %= frames.Count;
            FrameConfig config = frames[frameIndex];

            while (config.stayBodyRange)
            {
                --frameIndex;
                if (frameIndex < 0)
                {
                    return null;
                }
                config = frames[frameIndex];
            }

            return config;
        }

        public List<RangeConfig> GetBodyRanges(int frameIndex)
        {
            return GetBodyRangesFrame(frameIndex)?.bodyRanges;
        }

        public FrameConfig GetAttackRangesFrame(int frameIndex)
        {
            if (frames.Count == 0 || frameIndex < 0)
            {
                return null;
            }

            frameIndex %= frames.Count;
            FrameConfig config = frames[frameIndex];

            while (config.stayAttackRange)
            {
                --frameIndex;
                if (frameIndex < 0)
                {
                    return null;
                }
                config = frames[frameIndex];
            }

            return config;
        }

        public List<RangeConfig> GetAttackRanges(int frameIndex)
        {
            return GetAttackRangesFrame(frameIndex)?.attackRanges;
        }
    }
}
