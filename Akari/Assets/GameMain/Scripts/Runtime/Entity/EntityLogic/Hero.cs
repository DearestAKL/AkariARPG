using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Akari
{
    /// <summary>
    /// 英雄类
    /// </summary>
    public class Hero : TargetableObject
    {
        [SerializeField]
        private HeroData m_HeroData = null;

        private Transform m_LookAtPos = null;

        private GameObject m_Weapon = null;

        private PlayerComponent m_Player = null;

        private bool m_InDriving = false;

        [SerializeField, Range(0F, 180F), Header("旋转速度")]
        private float m_RotationSpeed = 90f;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            //查找组件
            m_LookAtPos = CachedTransform.Find("LookAt").GetComponent<Transform>();
            m_Weapon = CachedTransform.Find("Weapon").gameObject;

            m_Player = GameEntry.Player;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_HeroData = userData as HeroData;
            if (m_HeroData == null)
            {
                Log.Error("Hero data is invalid.");
                return;
            }

            //英雄创建成功 赋值到PlayerCommpont
            m_Player.Hero = this;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnAttachTo(UnityGameFramework.Runtime.EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }

        protected override void OnDetached(UnityGameFramework.Runtime.EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (InDriving)
            {
                return;
            }

            Vector3 velocity;
            if (GameEntry.Input.IsProhibitMove)
            {
                velocity = GameEntry.Input.GetCameraAxisValue().ToVector3();
            }
            else
            {
                velocity = CachedRigidbody.velocity;
                velocity.y = 0f;
            }

            if (velocity.magnitude > 0.01f)
            {
                var rotation = CachedTransform.rotation;
                CachedTransform.rotation = Quaternion.Slerp(rotation, Quaternion.LookRotation(velocity), m_RotationSpeed * Time.deltaTime);
            }
        }

        #region 外部行为接口

        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="damageHP">伤害值</param>
        public override void ApplyDamage(Entity attacker, int damageHP)
        {
            base.ApplyDamage(attacker, damageHP);

            float fromHPRatio = m_HeroData.HPRatio;
            m_HeroData.HP -= damageHP;
            float toHPRatio = m_HeroData.HPRatio;
            if (fromHPRatio > toHPRatio)
            {
                GameEntry.Event.Fire(HeroApplyDamageEventArgs.EventId, HeroApplyDamageEventArgs.Create(fromHPRatio, toHPRatio));
            }

            if (m_HeroData.HP <= 0)
            {
                OnDead(attacker);
            }
        }

        /// <summary>
        /// 恢复生命值
        /// </summary>
        /// <param name="curer">治疗者</param>
        /// <param name="restoreHP">恢复值</param>
        public override void RestoreHealth(Entity curer, int restoreHP)
        {
            if (m_HeroData.HP == m_HeroData.MaxHP)
            {
                //todo:已满血
                return;
            }

            float fromHPRatio = m_HeroData.HPRatio;
            m_HeroData.HP += restoreHP;
            float toHPRatio = m_HeroData.HPRatio;
            if (toHPRatio > fromHPRatio)
            {
                GameEntry.Event.Fire(HeroRestoreHealthEventArgs.EventId, HeroRestoreHealthEventArgs.Create(fromHPRatio, toHPRatio));
            }
        }
       
        #endregion

        #region 外部引用接口
        public Transform CachedLookAtPos
        {
            get { return m_LookAtPos; }
        }

        public GameObject Weapon
        {
            get { return m_Weapon; }
        }
        #endregion

        #region 碰撞检测
        public bool canDrive = false;
        public Car car = null;
        private void OnTriggerEnter(Collider other)
        {

            Entity entity = other.gameObject.GetComponent<Entity>();
            if (entity == null)
            {
                return;
            }

            if (entity is Car)
            {
                car = entity as Car;

                canDrive = true;
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();
            if (entity == null)
            {
                return;
            }

            if (entity is Car)
            {
                car = null;

                canDrive = false;
                return;

            }
        }
        #endregion

        public bool InDriving
        {
            get { return m_InDriving; }
        }

        public void EnterDrive(CarType carType)
        {
            GameEntry.Entity.AttachEntity(Id, car.Id);
            switch (carType)
            {
                case CarType.Motorcycle:
                    CachedTransform.localPosition = new Vector3(0, 0.4f, -0.4f);
                    CachedTransform.localRotation = Quaternion.identity;
                    break;
            }
            car.IsActive = true;
            m_InDriving = true;
            GetComponent<CapsuleCollider>().enabled = false;
            CachedRigidbody.isKinematic = true;
        }

        public void LeaveDrive()
        {
            CachedTransform.localPosition = new Vector3(1f, 0f, 0f);

            car.IsActive = false;
            m_InDriving = false;
            ModelObj.transform.localPosition = Vector3.zero;
            GetComponent<CapsuleCollider>().enabled = true;
            GameEntry.Entity.DetachEntity(Id);
            CachedRigidbody.isKinematic = false;
        }
    }
}