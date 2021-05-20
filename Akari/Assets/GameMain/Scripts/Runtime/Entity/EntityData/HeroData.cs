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
        public HeroData(int entityId,int typeId):base(entityId, typeId)
        {

        }

        #region 加成属性
        // 技能 武器 圣遗物 食物


        /// <summary>
        /// 生命值上限百分比加成
        /// </summary>
        public float HPBonus
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 生命值上限固定加成
        /// </summary>
        public int HPFlatBonus
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 攻击力百分比加成
        /// </summary>
        public float ATKBonus
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 攻击力固定加成
        /// </summary>
        public int ATKFlatBonus
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 防御力百分比加成
        /// </summary>
        public float DEFBonus
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 防御力固定加成
        /// </summary>
        public int DEFFlatBonus
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 伤害百分比加成
        /// </summary>
        public float DamageBonus
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 物理伤害百分比加成
        /// </summary>
        public float PhysicalDamage
        {
            get
            {
                return 0;
            }
        }
        #endregion


    }
}
