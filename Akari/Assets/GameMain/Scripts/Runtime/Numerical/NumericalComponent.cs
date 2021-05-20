using System;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Akari
{

    public class NumericalComponent : GameFrameworkComponent
    {

    }

    public class NumericalUtility
    {
        #region 计算基础属性
        /// <summary>
        /// 计算生命值上限
        /// </summary>
        /// <param name="BaseHP">基础生命值</param>
        /// <param name="HPBonus">生命值百分比加成(千分比)</param>
        /// <param name="HPFlatBonus">生命值数值加成</param>
        /// <returns></returns>
        public static int CalculateMaxHP(int BaseHP, float HPBonus, int HPFlatBonus)
        {
            var MaxHP = BaseHP * (1 + HPBonus) + HPFlatBonus;
            return (int)MaxHP;
        }

        /// <summary>
        /// 计算攻击力
        /// </summary>
        /// <param name="ATKCharacter">角色基础攻击力</param>
        /// <param name="ATKWeapon">武器攻击力</param>
        /// <param name="ATKBonus">攻击力百分比加成(千分比)</param>
        /// <param name="ATKFlatBonus">攻击力数值加成</param>
        /// <returns></returns>
        public static int CalculateAttack(int ATKCharacter, int ATKWeapon, float ATKBonus, int ATKFlatBonus)
        {
            var ATK = (ATKCharacter + ATKWeapon) * (1 + ATKBonus) + ATKFlatBonus;
            return (int)ATK;
        }


        /// <summary>
        /// 计算防御力
        /// </summary>
        /// <param name="DEFCharacter">角色基础防御力</param>
        /// <param name="DEFWeapon">武器防御力</param>
        /// <param name="DEFBonus">防御力百分比加成(千分比)</param>
        /// <param name="DEFFlatBonus">防御力数值加成</param>
        public static int CalculateDefense(int DEFCharacter, int DEFWeapon, float DEFBonus, int DEFFlatBonus)
        {
            var DEF = (DEFCharacter + DEFWeapon) * (1 + DEFBonus) + DEFFlatBonus;
            return (int)DEF;

            //DMGReduction = DEF/(DEF + 5*LevelAttacker + 50);//伤害降低公式
        }
        #endregion

        #region 计算物理伤害与治疗
        /// <summary>
        /// 计算造成物理攻击伤害
        /// </summary>
        /// <returns></returns>
        //public static int CalculateCausePhysicalDamage(int DEF)
        //{
        //    //var DMGReduction = DEF / (DEF + 5 * LevelAttacker + 50);
        //    //return (int)DMGReduction;
        //}

        /// <summary>
        /// 计算受到物理攻击伤害
        /// </summary>
        /// <param name="DEF"></param>
        /// <param name="LevelAttacker"></param>
        /// <returns></returns>
        public static int CalculateSufferPhysicalDamage(int DEF, int LevelAttacker)
        {
            var DMGReduction = DEF / (DEF + 5 * LevelAttacker + 50);
            return (int)DMGReduction;
        }
        #endregion

        #region 计算元素

        // 元素反应 Elemental Reactions
        // 结晶 Crystallize 岩和火雷冰水
        // 漩涡(扩散) Swirl 风和火雷冰水
        // 气化 Vaporize 火水
        // 融化 Melt 火冰
        // 过载 Overload 火雷
        // 超导 Superconduct 雷冰
        // 感电 ElectroCharged 水雷
        // 冻结 Frozen 水冰 攻击冰冻敌人会触发破碎,也就是Shattered


        /// <summary>
        /// 获取元素反应伤害系数
        /// </summary>
        /// <param name="type">元素反应类型</param>
        /// <param name="EM">元素精通值</param>
        /// <returns></returns>
        public static float GetElementalReactionsCoefficient(ElementalReactions type, int EM)
        {
            float coefficient = 0.0f;
            switch (type)
            {
                case ElementalReactions.Vaporize:
                case ElementalReactions.Melt:
                    coefficient = CoefficientX;
                    break;

                case ElementalReactions.Swirl:
                case ElementalReactions.Overload:
                case ElementalReactions.Superconduct:
                case ElementalReactions.ElectroCharged:
                case ElementalReactions.Frozen:
                    coefficient = CoefficientY;
                    break;

                case ElementalReactions.Crystallize:
                    coefficient = CoefficientZ;
                    break;
            }

            return coefficient * (EM / (EM + 1400));
        }

        /// <summary>
        /// Vaporize and Melt 
        /// 气化(火水) 和 融化(火冰)
        /// </summary>
        public const float CoefficientX = 2.78f;

        /// <summary>
        /// Overload, Superconduct, ElectroCharged,  Shattered, and Swirl
        /// 过载(火雷)，超导(雷冰)，感电(水雷)，破碎(结冰状态击碎)和漩涡(也就是扩散，风和火雷冰水)
        /// </summary>
        public const float CoefficientY = 6.66f;

        /// <summary>
        /// Crystallize 
        /// 结晶(岩和火雷冰水)
        /// </summary>
        public const float CoefficientZ = 4.44f;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Damage">伤害值 攻击力*伤害倍数</param>
        /// <returns></returns>
        public static int CalculateCauseElementalDamage(int Damage, ElementType elementTtpe)
        {
            switch (elementTtpe)
            {
                case ElementType.Fire:
                    break;
                case ElementType.Water:
                    break;
                case ElementType.Thunder:
                    break;
                default:
                    break;
            }


            return 0;
        }


        //public static AttributeType GetElementalDamageBonus(ElementType elementType)
        //{

        //}
        #endregion
    }
}
