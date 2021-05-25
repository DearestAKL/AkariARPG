using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Akari
{
    public class Car : Entity
    {
        private CarData m_CarData;
        private bool m_IsActive = false;
        private Rigidbody m_Rigidbody = null;

        [SerializeField, Range(0F, 180F), Header("旋转速度")]
        private float m_RotationSpeed = 45f;

        public CarType carType = CarType.Motorcycle;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Rigidbody = CachedTransform.GetComponent<Rigidbody>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_CarData = userData as CarData;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (!m_IsActive)
            {
                return;
            }

            #region 移动
            if (!GameEntry.Input.IsProhibitMove)
            {
                var velocity = m_Rigidbody.velocity;
                if (GameEntry.Input.HasEvent(InputEvents.Moving))
                {
                    Vector2 desiredVelocity = GameEntry.Input.GetEffectiveCameraAxisValue() * m_CarData.Speed;

                    velocity.x = desiredVelocity.x;
                    velocity.z = desiredVelocity.y;
                }
                else
                {
                    velocity.x = 0;
                    velocity.z = 0;
                }
                m_Rigidbody.velocity = velocity;
            }
            #endregion

            #region 旋转
            Vector3 direction;
            if (GameEntry.Input.IsProhibitMove)
            {
                direction = GameEntry.Input.GetCameraAxisValue().ToVector3();
            }
            else
            {
                direction = m_Rigidbody.velocity;
                direction.y = 0f;
            }

            if (direction.magnitude > 0.01f)
            {
                var rotation = CachedTransform.rotation;
                CachedTransform.rotation = Quaternion.Slerp(rotation, Quaternion.LookRotation(direction), m_RotationSpeed * Time.deltaTime);
            }
            #endregion
        }

        public CarData CarData
        {
            get { return m_CarData; }
        }

        public bool IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }

        /// <summary>
        /// 缓存的Rigidbody
        /// </summary>
        public Rigidbody CachedRigidbody
        {
            get { return m_Rigidbody; }
        }
    }
}
