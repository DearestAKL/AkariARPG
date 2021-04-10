using GameFramework;
using GameFramework.Event;

namespace Akari
{
    /// <summary>
    /// 英雄恢复生命值
    /// </summary>
    public class HeroRestoreHealthEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HeroRestoreHealthEventArgs).GetHashCode();

        public HeroRestoreHealthEventArgs()
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

        public static HeroRestoreHealthEventArgs Create(float fromHPRatio,float toHPRatio)
        {
            HeroRestoreHealthEventArgs changeSceneEventArgs = ReferencePool.Acquire<HeroRestoreHealthEventArgs>();
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
