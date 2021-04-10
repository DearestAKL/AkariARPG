using System;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// 玩家数据类
    /// </summary>
    [Serializable]
    public class HeroData : TargetableData
    {
        [SerializeField]
        private int m_MaxCP = 0;

        public HeroData(int entityId,int typeId):base(entityId, typeId)
        {

        }

        /// <summary>
        /// 最大体力。
        /// </summary>
        public int MaxCP
        {
            get
            {
                return m_MaxCP;
            }
        }
    }
}
