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
        /// <param name="rotation">朝向</param>
        /// <param name="num">怪物数量</param>
        /// <param name="radius">圆圈半径</param>
        /// <param name="sectorAngle">扇形角度</param>
        public static Vector3[] SectorSpawn(Transform content, float rotation, int num,float radius,float sectorAngle)
        {
            var positions = new Vector3[num];

            var contentPos = content.position;
            Vector3 curPos = new Vector3(0, contentPos.y, 0);

            // 数量为1 直接在角度方向生成
            if (num == 1)
            {
                curPos.x = contentPos.x + radius * Mathf.Sin(rotation * 3.14f / 180f);
                curPos.z = contentPos.z + radius * Mathf.Cos(rotation * 3.14f / 180f);
                positions[0] = curPos;
            }
            else 
            { 

                //平均角度
                float averageAngle = sectorAngle / (num-1);
                float startAngle = rotation - (sectorAngle / 2);

                for (int i = 0; i < num; i++)
                {
                    float radians = (startAngle + (i * averageAngle)) * 3.14f / 180f;
                    // 根据原点,角度,半径获取物体的位置.  
                    // x = 原点x + 半径 * 邻边除以斜边的比例,   邻边除以斜边的比例 = cos(弧度) , 弧度 = 角度 *3.14f / 180f;
                    curPos.x = contentPos.x + radius * Mathf.Sin(radians);
                    curPos.z = contentPos.z + radius * Mathf.Cos(radians);

                    positions[i] = curPos;
                }
            }

            return positions;
        }

        /// <summary>
        /// 线形怪物生成
        /// </summary>
        /// <param name="content">中心</param>
        /// <param name="rotation">朝向</param>
        /// <param name="num">怪物数量</param>
        /// <param name="lineDistance">线距离</param>
        /// <param name="lineLength">线长度</param>
        public static Vector3[] LineSpawn(Transform content, float rotation, int num, float lineDistance, float lineLength)
        {
            var positions = new Vector3[num];

            var contentPos = content.position;
            Vector3 curPos = new Vector3(0, contentPos.y, 0);

            // 数量为1 直接在角度方向生成
            if (num == 1)
            {
                curPos.x = contentPos.x + lineDistance * Mathf.Sin(rotation * 3.14f / 180f);
                curPos.z = contentPos.z + lineDistance * Mathf.Cos(rotation * 3.14f / 180f);
                positions[0] = curPos;
            }
            else
            {

                // 取左侧
                var lineAngle = rotation - 90f;
                // X轴方向系数
                var lineCoefficientX = Mathf.Sin(lineAngle * 3.14f / 180f);
                // Z轴方向系数
                var lineCoefficientZ = Mathf.Cos(lineAngle * 3.14f / 180f);

                // 取线左边为起点
                var lineStartX = contentPos.x + lineDistance * Mathf.Sin(rotation * 3.14f / 180f) + lineLength / 2 * lineCoefficientX;
                var lineStartZ = contentPos.z + lineDistance * Mathf.Cos(rotation * 3.14f / 180f) + lineLength / 2 * lineCoefficientZ;

                var unitLength = lineLength / (num - 1);

                for (int i = 0; i < num; i++)
                {
                    curPos.x = lineStartX + unitLength * i * -lineCoefficientX;
                    curPos.z = lineStartZ + unitLength * i * -lineCoefficientZ;
                    positions[i] = curPos;
                }
            }

            return positions;
        }
    }
}
