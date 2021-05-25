using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Akari
{
    public class SpawnPoint : MonoBehaviour
    {
        private enum SpawnType
        {
            /// <summary>
            /// 线
            /// </summary>
            Line = 0,

            /// <summary>
            /// 扇形
            /// </summary>
            Sector = 1,
        }

        [SerializeField]
        [EnumPaging]
        private SpawnType spawnType = SpawnType.Sector;

        [SerializeField]
        [PropertyRange(0,360)]
        private float rotation = 0f;

        [SerializeField]
        [Range(1, 20)]
        private int num = 3;

        [SerializeField]
        private Vector2 targetSize = new Vector2(0.1F, 0.1F);

        #region 线 属性

        [SerializeField]
        [ShowIf("spawnType", SpawnType.Line), PropertyRange(1, 100)]
        private float lineDistance = 1f;

        [SerializeField]
        [ShowIf("spawnType", SpawnType.Line), PropertyRange(1, 100)]
        private float lineLength = 1f;

        #endregion

        #region 扇形 属性

        [SerializeField]
        [ShowIf("spawnType",SpawnType.Sector), PropertyRange(1, 100)]
        private float sectorRadius = 1f;

        [SerializeField]
        [ShowIf("spawnType", SpawnType.Sector), PropertyRange(0, 360)]
        private float sectorAngle = 60f;

        #endregion

        public void Awake()
        {

        }

        public void OnDrawGizmos()
        {
            if(spawnType == SpawnType.Line)
            {
                DrawLineGizmos();
            }
            else if(spawnType == SpawnType.Sector)
            {
                DrawSectorGizmos();
            }
        }

        private void DrawLineGizmos()
        {
            var start1 = transform.position;

            float x = start1.x + lineDistance * Mathf.Sin(rotation * 3.14f / 180f);
            float z = start1.z + lineDistance * Mathf.Cos(rotation * 3.14f / 180f);

            var end1 = new Vector3(x, start1.y, z);

            DrawUtility.G.PushColor(Color.green);
            DrawUtility.G.DrawLine(start1, end1);
            DrawUtility.G.PopColor();

            // 取左侧
            var lineAngle = rotation - 90f;
            // X轴方向系数
            var lineCoefficientX = Mathf.Sin(lineAngle * 3.14f / 180f);
            // Z轴方向系数
            var lineCoefficientZ = Mathf.Cos(lineAngle * 3.14f / 180f);

            var lineLineLength = lineLength / 2;
            var lineStartX = x + lineLineLength * lineCoefficientX;
            var lineStartZ = z + lineLineLength * lineCoefficientZ;
            var start2 = new Vector3(lineStartX, start1.y, lineStartZ);

            var lineEndX = x + (lineLength / 2) * -lineCoefficientX;
            var lineEndZ = z + (lineLength / 2) * -lineCoefficientZ;
            var end2 = new Vector3(lineEndX, end1.y, lineEndZ);

            DrawUtility.G.PushColor(Color.red);
            DrawUtility.G.DrawLine(start2, end2);
            DrawUtility.G.PopColor();

            DrawUtility.G.PushColor(Color.blue);
            if (num == 1)
            {
                DrawUtility.G.DrawRect(targetSize, GetHorizontalMatrix(end1));
            }
            else
            {
                var unitLength = lineLength / (num - 1);
                Vector3 curV3 = start2;
                for (int i = 0; i < num; i++)
                {
                    curV3.x = lineStartX + unitLength * i * -lineCoefficientX;
                    curV3.z = lineStartZ + unitLength * i * -lineCoefficientZ;

                    DrawUtility.G.DrawRect(targetSize, GetHorizontalMatrix(curV3));
                }
            }
            DrawUtility.G.PopColor();

            DrawUtility.G.ClearColor();
        }

        private void DrawSectorGizmos()
        {
            DrawUtility.G.PushColor(Color.red);
            DrawUtility.G.DrawArc(sectorRadius, sectorAngle, rotation, GetHorizontalMatrix(transform.position));
            DrawUtility.G.PopColor();


            DrawUtility.G.PushColor(Color.blue);

            var contentPos = transform.position;
            Vector3 curV3 = new Vector3(0, contentPos.y, 0); ;
            if (num == 1)
            {
                curV3.x = contentPos.x + sectorRadius * Mathf.Sin(rotation * 3.14f / 180f);
                curV3.z = contentPos.z + sectorRadius * Mathf.Cos(rotation * 3.14f / 180f);
                DrawUtility.G.DrawRect(targetSize, GetHorizontalMatrix(curV3));
            }
            else
            {
                //平均角度
                float averageAngle = sectorAngle / (num - 1);
                float startAngle = rotation - (sectorAngle / 2);
                for (int i = 0; i < num; i++)
                {
                    float radians = (startAngle + i * averageAngle) * 3.14f / 180f;
                    curV3.x = contentPos.x + sectorRadius * Mathf.Sin(radians);
                    curV3.z = contentPos.z + sectorRadius * Mathf.Cos(radians);

                    DrawUtility.G.DrawRect(targetSize, GetHorizontalMatrix(curV3));
                }
            }

            DrawUtility.G.PopColor();

            DrawUtility.G.ClearColor();
        }

        private Matrix4x4 GetHorizontalMatrix(Vector3 vector3)
        {
            return Matrix4x4.Translate(vector3) * Matrix4x4.Rotate(Quaternion.Euler(-90.0f, 0.0f, -90.0f));
        }

        [Button("Generate Monster")]
        private void Generate()
        {
            Vector3[] positions = new Vector3[num];

            if (spawnType == SpawnType.Line)
            {
                positions = MonsterSpawn.LineSpawn(transform, rotation, num, lineDistance, lineLength);
            }
            else if (spawnType == SpawnType.Sector)
            {
                positions = MonsterSpawn.SectorSpawn(transform, rotation, num, sectorRadius, sectorAngle);
            }

            for (int i = 0; i < positions.Length; i++)
            {
                var m_MonsterData = new MonsterData(GameEntry.Entity.GenerateSerialId(), (int)EnumEntity.Monster);
                m_MonsterData.MaxHP = 100;
                m_MonsterData.HP = 100;
                m_MonsterData.Position = positions[i];
                GameEntry.Entity.ShowEntity<Monster>("Hero", Constant.AssetPriority.MonsterAsset, m_MonsterData);
            }
        }
    }
}
