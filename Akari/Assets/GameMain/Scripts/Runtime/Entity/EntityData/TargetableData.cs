using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Akari
{
    [Serializable]
    public abstract class TargetableData : EntityData
    {
        [FoldoutGroup("基础属性"),SerializeField]
        private int m_MaxHP = 0;

        [FoldoutGroup("基础属性"), SerializeField]
        private int m_Attack = 0;

        [FoldoutGroup("基础属性"), SerializeField]
        private int m_Defense = 0;

        [FoldoutGroup("基础属性"), SerializeField]
        private int m_HP = 0;

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

        #endregion

        #region 动画控制
        private GameObject CharacterPrefab;

        #endregion
    }

}
