using System;
using System.Collections.Generic;

namespace Akari
{
    /// <summary>
    /// 帧配置
    /// </summary>
    [Serializable]
    public class FrameConfig 
    {
        public bool stayAttackRange;
        public bool stayBodyRange;
        public List<RangeConfig> attackRanges = new List<RangeConfig>();
        public List<RangeConfig> bodyRanges = new List<RangeConfig>();

        public FrameConfig()
        {
        }

        public FrameConfig(List<RangeConfig> attackRanges, List<RangeConfig> bodyRanges)
        {
            CopyAttackRangeFrom(attackRanges);
            CopyBodyRangeFrom(bodyRanges);
        }

        public List<RangeConfig> CopyAttackRanges()
        {
            return CopyRanges(attackRanges);
        }

        private List<RangeConfig> CopyRanges(List<RangeConfig> ranges)
        {
            List<RangeConfig> copy = new List<RangeConfig>(ranges.Count);
            foreach (var item in ranges)
            {
                copy.Add(new RangeConfig(item));
            }
            return copy;
        }

        public void CopyAttackRangeFrom(List<RangeConfig> ranges)
        {
            attackRanges.Clear();

            if (ranges == null)
            {
                return;
            }

            foreach (var item in ranges)
            {
                attackRanges.Add(new RangeConfig(item));
            }
            stayAttackRange = false;
        }

        public void CopyBodyRangeFrom(List<RangeConfig> ranges)
        {
            bodyRanges.Clear();

            if (ranges == null)
            {
                return;
            }

            foreach (var item in ranges)
            {
                bodyRanges.Add(new RangeConfig(item));
            }
            stayBodyRange = false;
        }
    }
}
