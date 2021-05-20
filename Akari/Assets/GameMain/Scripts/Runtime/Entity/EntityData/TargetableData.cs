using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Akari
{
    [Serializable]
    public abstract class TargetableData : EntityData
    {

        [FoldoutGroup("当前属性"), SerializeField]
        private int m_HP = 0;

        [FoldoutGroup("基础属性"),Header("生命值上限"),SerializeField]
        private int m_MaxHP = 0;
        [FoldoutGroup("基础属性"), Header("攻击力"), SerializeField]
        private int m_Attack = 0;
        [FoldoutGroup("基础属性"), Header("防御力"), SerializeField]
        private int m_Defense = 0;
        [FoldoutGroup("基础属性"), Header("元素精通"), SerializeField]
        private int m_ElementalMastery = 0;
        [FoldoutGroup("基础属性"), Header("体力上限"), SerializeField]
        private int m_Stamina = 0;

        [FoldoutGroup("进阶属性"), Header("暴击率"), SerializeField]
        private int m_CriticalRate = 0;
        [FoldoutGroup("进阶属性"), Header("暴击伤害"), SerializeField]
        private int m_CriticalDamage = 0;
        [FoldoutGroup("进阶属性"), Header("治疗加成"), SerializeField]
        private int m_HealingBonus = 0;
        [FoldoutGroup("进阶属性"), Header("受治疗加成"), SerializeField]
        private int m_IncomingHealingBonus = 0;
        [FoldoutGroup("进阶属性"), Header("元素充能效率"), SerializeField]
        private int m_EnergyRecharge = 0;
        [FoldoutGroup("进阶属性"), Header("冷却缩减"), SerializeField]
        private int m_CooldownReduction = 0;
        [FoldoutGroup("进阶属性"), Header("护盾强度"), SerializeField]
        private int m_ShieldStrength = 0;

        [FoldoutGroup("元素属性"), Header("火元素伤害加成"), SerializeField]
        private int m_FireElementalDamageBonus = 0;
        [FoldoutGroup("元素属性"), Header("火元素抗性"), SerializeField]
        private int m_FireElementalResistance = 0;
        [FoldoutGroup("元素属性"), Header("水元素伤害加成"), SerializeField]
        private int m_WaterElementalDamageBonus = 0;
        [FoldoutGroup("元素属性"), Header("水元素抗性"), SerializeField]
        private int m_WaterElementalResistance = 0;
        [FoldoutGroup("元素属性"), Header("雷元素伤害加成"), SerializeField]
        private int m_ThundeElementalDamageBonus = 0;
        [FoldoutGroup("元素属性"), Header("雷元素抗性"), SerializeField]
        private int m_ThundeElementalResistance = 0;

        public TargetableData(int entityId, int typeId) : base(entityId, typeId)
        {
            m_MaxHP = 100;
            m_HP = 100;
        }

        #region 基础属性

        /// <summary>
        /// 最大生命。
        /// </summary>
        public int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
            //临时
            set
            {
                m_MaxHP = value;
            }
        }

        /// <summary>
        /// 当前生命。
        /// </summary>
        public int HP
        {
            get
            {
                return m_HP;
            }
            set
            {
                m_HP = Math.Min(value, m_MaxHP);
            }
        }

        /// <summary>
        /// 生命百分比。
        /// </summary>
        public float HPRatio
        {
            get
            {
                return MaxHP > 0 ? (float)HP / MaxHP : 0f;
            }
        }

        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        /// <summary>
        /// 防御力
        /// </summary>
        public int Defense
        {
            get
            {
                return m_Defense;
            }
        }

        /// <summary>
        /// 元素精通
        /// </summary>
        public int ElementalMastery
        {
            get
            {
                return m_ElementalMastery;
            }
        }

        /// <summary>
        /// 体力上限
        /// </summary>
        public int Stamina
        {
            get
            {
                return m_Stamina;
            }
        }

        #endregion

        #region 进阶属性
        /// <summary>
        /// 暴击率
        /// </summary>
        public int CriticalRate
        {
            get
            {
                return m_CriticalRate;
            }
        }

        /// <summary>
        /// 暴击伤害
        /// </summary>
        public int CriticalDamage
        {
            get
            {
                return m_CriticalDamage;
            }
        }

        /// <summary>
        /// 治疗加成
        /// </summary>
        public int HealingBonus
        {
            get
            {
                return m_HealingBonus;
            }
        }

        /// <summary>
        /// 受治疗加成
        /// </summary>
        public int IncomingHealingBonus
        {
            get
            {
                return m_IncomingHealingBonus;
            }
        }

        /// <summary>
        /// 元素充能效率
        /// </summary>
        public int EnergyRecharge
        {
            get
            {
                return m_EnergyRecharge;
            }
        }

        /// <summary>
        /// 冷却缩减
        /// </summary>
        public int CooldownReduction
        {
            get
            {
                return m_CooldownReduction;
            }
        }

        /// <summary>
        /// 护盾强度
        /// </summary>
        public int ShieldStrength
        {
            get
            {
                return m_ShieldStrength;
            }
        }
        #endregion

        #region 元素属性
        /// <summary>
        /// 火元素伤害加成
        /// </summary>
        public int FireElementalDamageBonus
        {
            get
            {
                return m_FireElementalDamageBonus;
            }
        }

        /// <summary>
        /// 火元素抗性
        /// </summary>
        public int FireElementalResistance
        {
            get
            {
                return m_FireElementalResistance;
            }
        }

        /// <summary>
        /// 水元素伤害加成
        /// </summary>
        public int WaterElementalDamageBonus
        {
            get
            {
                return m_WaterElementalDamageBonus;
            }
        }

        /// <summary>
        /// 水元素抗性
        /// </summary>
        public int WaterElementalResistance
        {
            get
            {
                return m_WaterElementalResistance;
            }
        }

        /// <summary>
        /// 雷元素伤害加成
        /// </summary>
        public int ThundeElementalDamageBonus
        {
            get
            {
                return m_ThundeElementalDamageBonus;
            }
        }

        /// <summary>
        /// 雷元素抗性
        /// </summary>
        public int ThundeElementalResistance
        {
            get
            {
                return m_ThundeElementalResistance;
            }
        }
        #endregion
    }

}
