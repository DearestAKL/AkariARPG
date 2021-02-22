using GameFramework;
using GameFramework.Event;

namespace Akari
{
    /// <summary>
    /// 切换场景
    /// </summary>
    public class ChangeSceneEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeSceneEventArgs).GetHashCode();

        public ChangeSceneEventArgs()
        {
            SceneId = 0;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int SceneId
        {
            get;
            private set;
        }

        public static ChangeSceneEventArgs Create(int sceneId)
        {
            ChangeSceneEventArgs changeSceneEventArgs = ReferencePool.Acquire<ChangeSceneEventArgs>();
            changeSceneEventArgs.SceneId = sceneId;
            return changeSceneEventArgs;
        }

        public override void Clear()
        {
            SceneId = 0;
        }
    }
}
