namespace Akari
{
    /// <summary>
    /// 元素反应
    /// </summary>
    public enum ElementalReactions
    {
        None = 0,

        /// <summary>
        /// 结晶 Crystallize 岩和火雷冰水
        /// </summary>
        Crystallize = 1,

        /// <summary>
        /// 漩涡(扩散) Swirl 风和火雷冰水
        /// </summary>
        Swirl = 2,

        /// <summary>
        /// 气化 Vaporize 火水
        /// </summary>
        Vaporize = 3,

        /// <summary>
        /// 融化 Melt 火冰
        /// </summary>
        Melt = 4,

        /// <summary>
        /// 过载 Overload 火雷
        /// </summary>
        Overload = 5,

        /// <summary>
        /// 超导 Superconduct 雷冰
        /// </summary>
        Superconduct = 6,

        /// <summary>
        /// 感电 ElectroCharged 水雷
        /// </summary>
        ElectroCharged = 7,

        /// <summary>
        /// 冻结 Frozen 水冰 冰碎掉才会有伤害,也就是Shattered
        /// </summary>
        Frozen = 8,
    }
}
