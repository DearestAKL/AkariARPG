using System;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// 范围配置
    /// </summary>
    [Serializable]
    public class RangeConfig
    {
        [SerializeReference]
        public IItem value;

        public Type GetValueType() => value?.GetType() ?? null;

        public RangeConfig()
        {
        }

        public RangeConfig(RangeConfig config)
        {
            this.value = config.value.Clone();
        }
    }

    #region data

    public interface IItem
    {
        IItem Clone();
    }

    [Serializable]
    public class RectItem : IItem
    {
        public Vector2 offset = Vector2.up;
        public Vector2 size = Vector2.one;

        public IItem Clone()
        {
            return new RectItem()
            {
                offset = this.offset,
                size = this.size,
            };
        }
    }

    [Serializable]
    public class CircleItem : IItem
    {
        public Vector2 offset = Vector2.up;
        public Single radius = 1;

        public IItem Clone()
        {
            return new CircleItem()
            {
                offset = this.offset,
                radius = this.radius
            };
        }
    }

    [Serializable]
    public class BoxItem : IItem
    {
        public Vector3 offset = Vector3.up;
        public Vector3 size = Vector3.one;

        public IItem Clone()
        {
            return new BoxItem()
            {
                offset = this.offset,
                size = this.size
            };
        }
    }

    [Serializable]
    public class SphereItem : IItem
    {
        public Vector3 offset = Vector3.up;
        public Single radius = 1;

        public IItem Clone()
        {
            return new SphereItem()
            {
                offset = this.offset,
                radius = this.radius
            };
        }
    }
    #endregion data
}
