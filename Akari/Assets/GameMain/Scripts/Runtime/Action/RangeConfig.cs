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
        [RangeTypes]
        [SerializeReference]
        public IRangeItem value;

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

    public interface IRangeItem
    {
        IRangeItem Clone();
    }

    [Serializable]
    public class RectItem : IRangeItem
    {
        public Vector2 offset = Vector2.up;
        public Vector2 size = Vector2.one;

        public IRangeItem Clone()
        {
            return new RectItem()
            {
                offset = this.offset,
                size = this.size,
            };
        }
    }

    [Serializable]
    public class CircleItem : IRangeItem
    {
        public Vector2 offset = Vector2.up;
        public Single radius = 1;

        public IRangeItem Clone()
        {
            return new CircleItem()
            {
                offset = this.offset,
                radius = this.radius
            };
        }
    }

    [Serializable]
    public class BoxItem : IRangeItem
    {
        public Vector3 offset = Vector3.up;
        public Vector3 size = Vector3.one;

        public IRangeItem Clone()
        {
            return new BoxItem()
            {
                offset = this.offset,
                size = this.size
            };
        }
    }

    [Serializable]
    public class SphereItem : IRangeItem
    {
        public Vector3 offset = Vector3.up;
        public Single radius = 1;

        public IRangeItem Clone()
        {
            return new SphereItem()
            {
                offset = this.offset,
                radius = this.radius
            };
        }
    }

    public class RangeTypesAttribute : ObjectTypesAttribute
    {
        public override Type baseType => typeof(IRangeItem);
    }
    #endregion data
}
