using UnityEngine;
using UnityGameFramework.Runtime;

namespace Akari
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static DataComponent Data
        {
            get;
            private set;
        }

        /// <summary>
        /// 这里注册自定义的组件
        /// </summary>
        private static void InitCustomComponents()
        {
            Data = UnityGameFramework.Runtime.GameEntry.GetComponent<DataComponent>();
        }

        /// <summary>
        /// 这里注册自定义的调试器
        /// </summary>
        private static void InitCustomDebuggers()
        {

        }
    }
}
