using System;
using UnityEngine;
using GameFramework;

namespace Akari
{
    [Serializable]
    public class CarData : EntityData
    {
        public float Speed
        {
            get;
            private set;
        }

        public CarData(int entityId, int typeId, float speed) : base(entityId, typeId)
        {
            Speed = speed;
        }
    }
}
