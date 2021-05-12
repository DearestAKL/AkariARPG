using System;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// 怪物数据类
    /// </summary>
    [Serializable]
    public class MonsterData : TargetableData
    {
        public MonsterData(int entityId, int typeId) : base(entityId, typeId)
        {

        }
    }
}
