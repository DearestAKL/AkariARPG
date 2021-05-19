using System;
using UnityEngine;
using GameFramework;

namespace Akari
{
    [Serializable]
    public class FollowerData : EntityData
    {
        public Transform Follow
        {
            get;
            private set;
        }

        public Vector3 Offset
        {
            get;
            private set;
        }

        public Vector3 Angles
        {
            get;
            private set;
        }

        public Vector3 Scale
        {
            get;
            private set;
        }

        /// <summary>
        /// 存活时间 -1为永久存活
        /// </summary>
        public float LifeTime
        {
            get;
            private set;
        }

        public FollowerData(int entityId, int typeId, Transform follow, Vector3 offset) : base(entityId, typeId)
        {
            Follow = follow;
            Offset = offset;
            Angles = Vector3.zero;
            Scale = Vector3.one;
            LifeTime = -1f;
        }

        public FollowerData(int entityId, int typeId, Transform follow, Vector3 offset, Vector3 angles) : base(entityId, typeId)
        {
            Follow = follow;
            Offset = offset;
            Angles = angles;
            Scale = Vector3.one;
            LifeTime = -1f;
        }

        public FollowerData(int entityId, int typeId,Transform follow,Vector3 offset,Vector3 angles,Vector3 scale) : base(entityId, typeId)
        {
            Follow = follow;
            Offset = offset;
            Angles = angles;
            Scale = scale;
            LifeTime = -1f;
        }

        public FollowerData(int entityId, int typeId, Transform follow, Vector3 offset, float lifeTime) : base(entityId, typeId)
        {
            Follow = follow;
            Offset = offset;
            Angles = Vector3.zero;
            Scale = Vector3.one;
            LifeTime = lifeTime;
        }

        public FollowerData(int entityId, int typeId, Transform follow, Vector3 offset, Vector3 angles,float lifeTime) : base(entityId, typeId)
        {
            Follow = follow;
            Offset = offset;
            Angles = angles;
            Scale = Vector3.one;
            LifeTime = lifeTime;
        }

        public FollowerData(int entityId, int typeId, Transform follow, Vector3 offset, Vector3 angles, Vector3 scale,float lifeTime) : base(entityId, typeId)
        {
            Follow = follow;
            Offset = offset;
            Angles = angles;
            Scale = scale;
            LifeTime = lifeTime;
        }
    }
}
