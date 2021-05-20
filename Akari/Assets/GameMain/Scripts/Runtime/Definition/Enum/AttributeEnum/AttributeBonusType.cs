using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    /// <summary>
    /// 属性加成类型
    /// </summary>
    public enum AttributeBonusType
    {
        None = 0,

        /// <summary>
        /// 生命值上限百分比加成
        /// </summary>
        HPBonus = 1,

        /// <summary>
        /// 生命值上限固定加成
        /// </summary>
        HPFlatBonus = 2,

        /// <summary>
        /// 攻击力百分比加成
        /// </summary>
        ATKBonus = 3,

        /// <summary>
        /// 攻击力固定加成
        /// </summary>
        ATKFlatBonus = 4,

        /// <summary>
        /// 防御力百分比加成
        /// </summary>
        DEFBonus = 5,

        /// <summary>
        /// 防御力固定加成
        /// </summary>
        DEFFlatBonus = 6,

        /// <summary>
        /// 伤害百分比加成
        /// </summary>
        DamageBonus = 7,

        /// <summary>
        /// 物理伤害百分比加成
        /// </summary>
        PhysicalDamageBonus = 8,

        /// <summary>
        /// 移动速度加成
        /// </summary>
        MoveSpeedBonus = 9,
    }
}
