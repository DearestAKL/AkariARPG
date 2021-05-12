using GameFramework.DataTable;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Akari
{
    public static class MonsterSpawn
    {
        /// <summary>
        /// 扇形怪物生成
        /// </summary>
        /// <param name="content">中心</param>
        /// <param name="radius">圆圈半径</param>
        /// <param name="num">怪物数量</param>
        /// <param name="sectorAngle">扇形角度</param>
        /// <param name="initialAngle">扇形朝向</param>
        public static void SectorSpawn(Transform content, int radius, int num,float sectorAngle,float initialAngle)
        {
            var contentPos = content.position;
            //平均角度
            float averageAngle = sectorAngle / num;

            for (float angle = initialAngle; angle < 360; angle += 30)
            {
                // 根据原点,角度,半径获取物体的位置.  
                // x = 原点x + 半径 * 邻边除以斜边的比例,   邻边除以斜边的比例 = cos(弧度) , 弧度 = 角度 *3.14f / 180f;
                float x = contentPos.x + radius * Mathf.Sin(angle * 3.14f / 180f);
                float z = contentPos.z + radius * Mathf.Cos(angle * 3.14f / 180f);
            }
        }
    }
}
