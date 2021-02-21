using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    public abstract class GameBase
    {
        /// <summary>
        /// 游戏模式
        /// </summary>
        public abstract GameMode GameMode
        {
            get;
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public bool GameOver
        {
            get;
            protected set;
        }

        /// <summary>
        /// 游戏数据初始化
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// 关闭游戏
        /// </summary>
        public virtual void Shutdown()
        {

        }

        /// <summary>
        /// 保存游戏
        /// </summary>
        public virtual void SaveGame()
        {

        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {

        }
    }
}
