using GameFramework;
using GameFramework.Event;

namespace Akari
{
    /// <summary>
    /// 游戏受到伤害 --> UI变化
    /// </summary>
    public class HeroApplyDamageEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HeroApplyDamageEventArgs).GetHashCode();

        public HeroApplyDamageEventArgs()
        {
            FromHPRatio = 0;
            ToHPRatio = 0;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public float FromHPRatio
        {
            get;
            private set;
        }

        public float ToHPRatio
        {
            get;
            private set;
        }

        public static HeroApplyDamageEventArgs Create(float fromHPRatio,float toHPRatio)
        {
            HeroApplyDamageEventArgs changeSceneEventArgs = ReferencePool.Acquire<HeroApplyDamageEventArgs>();
            changeSceneEventArgs.FromHPRatio = fromHPRatio;
            changeSceneEventArgs.ToHPRatio = toHPRatio;

            return changeSceneEventArgs;
        }

        public override void Clear()
        {
            FromHPRatio = 0;
            ToHPRatio = 0;
        }
    }
}
