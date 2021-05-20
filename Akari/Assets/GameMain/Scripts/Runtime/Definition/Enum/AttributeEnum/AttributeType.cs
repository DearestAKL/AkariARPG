namespace Akari
{
    /// <summary>
    /// 属性类型
    /// </summary>
    public enum AttributeType
    {
        None = 0,

        #region Basic Stats 基础属性
        /// <summary>
        /// 生命值上限
        /// </summary>
        MaxHP = 1,

        /// <summary>
        /// 攻击力
        /// </summary>
        Attack = 2,

        /// <summary>
        /// 防御力
        /// </summary>
        Defense = 3,

        /// <summary>
        /// 元素精通
        /// </summary>
        ElementalMastery = 4,

        /// <summary>
        /// 体力上限
        /// </summary>
        Stamina = 5,
        #endregion

        #region Advanced Stats 进阶属性
        /// <summary>
        /// 暴击率
        /// </summary>
        CriticalRate = 6,

        /// <summary>
        /// 暴击伤害
        /// </summary>
        CriticalDamage = 7,

        /// <summary>
        /// 治疗加成
        /// </summary>
        HealingBonus = 8,

        /// <summary>
        /// 受治疗加成
        /// </summary>
        IncomingHealingBonus = 9,

        /// <summary>
        /// 元素充能效率
        /// </summary>
        EnergyRecharge = 10,

        /// <summary>
        /// 冷却缩减
        /// </summary>
        CooldownReduction = 11,

        /// <summary>
        /// 护盾强度
        /// </summary>
        ShieldStrength = 12,
        #endregion

        #region Elemental Stats 元素属性
        /// <summary>
        /// 火元素伤害加成
        /// </summary>
        FireElementalDamageBonus = 15,

        /// <summary>
        /// 火元素抗性
        /// </summary>
        FireElementalResistance = 16,

        /// <summary>
        /// 水元素伤害加成
        /// </summary>
        WaterElementalDamageBonus = 17,

        /// <summary>
        /// 水元素抗性
        /// </summary>
        WaterElementalResistance = 18,

        /// <summary>
        /// 雷元素伤害加成
        /// </summary>
        ThundeElementalDamageBonus = 19,

        /// <summary>
        /// 雷元素抗性
        /// </summary>
        ThundeElementalResistance = 20,
        #endregion

        #region Hide Stats 隐藏属性
        /// <summary>
        /// 移动速度
        /// </summary>
        MovementSpeed = 101,

        /// <summary>
        /// 攻击速度
        /// </summary>
        AttackSpeed = 102,

        /// <summary>
        /// 体力消耗
        /// </summary>
        StaminaConsumption = 103,

        /// <summary>
        /// 耐干扰性
        /// </summary>
        InterruptionResistance = 104,
        #endregion
    }
}
