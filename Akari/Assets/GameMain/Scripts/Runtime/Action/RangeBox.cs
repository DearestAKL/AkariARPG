using System;
using UnityEngine;

namespace Akari
{
    [RequireComponent(typeof(BoxCollider), typeof(SphereCollider))]
    public class RangeBox:MonoBehaviour
    {
        [SerializeField]
        private BoxCollider boxRange = null;
        [SerializeField]
        private SphereCollider sphereRange = null;
        [SerializeField]
        private Entity parentEntity = null;

        private void Awake()
        {
            boxRange = transform.GetComponent<BoxCollider>();
            sphereRange = transform.GetComponent<SphereCollider>();
        }

        public void Init(Entity entity) 
        {
            parentEntity = entity;

            ////初始化层级
            //if(parentEntity.gameObject.layer == Constant.Layer.HeroLayerId)
            //{
            //    gameObject.layer = Constant.Layer.HeroAttackBoxLayerId;
            //}
            //else if(parentEntity.gameObject.layer == Constant.Layer.MonsterLayerId)
            //{
            //    gameObject.layer = Constant.Layer.MonsterAttackBoxLayerId;
            //}
        }

        public void CheckRangeBox(RangeConfig rangeConfig)
        {
            if (rangeConfig == null)
            {
                boxRange.enabled = false;
                sphereRange.enabled = false;

                return;
            }

            switch (rangeConfig.value)
            {
                case BoxItem value:
                    boxRange.enabled = false;
                    sphereRange.enabled = true;

                    boxRange.center = value.offset;
                    boxRange.size = value.size;
                    break;
                case SphereItem value:
                    boxRange.enabled = true;
                    sphereRange.enabled = false;

                    sphereRange.center = value.offset;
                    sphereRange.radius = value.radius;
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();
            if (entity == null)
            {
                return;
            }

            if (entity is TargetableObject)
            {
                (entity as TargetableObject).ApplyDamage(parentEntity, 10);
            }
        }
    }
}
