﻿using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;


namespace Akari
{
    public class PlayerComponent : GameFrameworkComponent
    {
        [SerializeField]
        private Hero m_Hero;
        [SerializeField]
        private HeroData m_HeroData;

        //---
        [SerializeField]
        private Transform m_MainCameraTransform;
        [SerializeField]
        private Rigidbody m_Rigidbody;
        [SerializeField]
        private Animator m_Animator;


        #region 移动相关数据


        [FoldoutGroup("玩家移动"),SerializeField]
        private Vector2 playerInput;
        [FoldoutGroup("玩家移动"), SerializeField]
        private Vector3 velocity;
        [FoldoutGroup("玩家移动"),SerializeField]
        private Vector3 desiredVelocity;


        [FoldoutGroup("玩家移动")]
        [SerializeField, Range(0f, 100f), Header("最大速度")]
        private float maxSpeed = 10f;

        [FoldoutGroup("玩家移动")]
        [SerializeField, Range(0f, 100f), Header("最大加速度")]
        private float maxAcceleration = 10f;

        [FoldoutGroup("玩家移动")]
        [SerializeField, Range(0f, 100f), Header("最大空中加速度")]
        private float maxAirAcceleration = 1f;

        [FoldoutGroup("玩家移动")]
        [SerializeField, Range(0f, 10f), Header("跳跃高度")]
        private float jumpHight = 2f;

        [FoldoutGroup("玩家移动")]
        [SerializeField, Range(0, 5), Header("空中跳跃")]
        private int maxAirJumps = 0;

        [FoldoutGroup("玩家移动")]
        [SerializeField, Range(0F, 90F), Header("角度")]
        private float maxGroundAngle = 25f;

        [FoldoutGroup("玩家移动")]
        [SerializeField, Range(0F, 90F), Header("旋转速度")]
        private float desiredRotationSpeed = 0.1f;

        bool desiredJump;
        int jumpPhase;
        float minGroundDotProduct;
        int groundContactCount;
        bool OnGround => groundContactCount > 0;
        Vector3 contactNormal;

        #endregion

        #region Input
        public InputAction jumpAction;
        #endregion

        private void Start()
        {
            m_HeroData = new HeroData(1, 1);

            m_MainCameraTransform = GameEntry.Camera.MainCamera.transform;

            jumpAction = new InputAction(binding: "*/<Keyboard>{jKey}");
        }

        private void Update()
        {
            if (m_Hero == null) { return; }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("Space");
            }

            //playerInput.x = Input.GetAxis("Horizontal");
            //playerInput.y = Input.GetAxis("Vertical");
            playerInput.x =0;
            playerInput.y = 0;
            playerInput = Vector2.ClampMagnitude(playerInput, 1f);

            if (m_MainCameraTransform)
            {
                Vector3 forward = m_MainCameraTransform.forward;
                forward.y = 0f;
                forward.Normalize();
                Vector3 right = m_MainCameraTransform.right;
                right.y = 0f;
                right.Normalize();
                desiredVelocity = (forward * playerInput.y + right * playerInput.x) * maxSpeed;
            }
            else
            {
                desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
            }

            //desiredJump |= Input.GetButtonDown("Jump");
            desiredJump |= jumpAction.triggered;
        }

        private void FixedUpdate()
        {
            if (m_Hero == null) { return; }

            UpdateState();
            AdjustVelocity();

            if (desiredJump)
            {
                desiredJump = false;
                Jump();
            }

            m_Rigidbody.velocity = velocity;

            if (playerInput.magnitude > 0.1)
            {
                m_Hero.CachedTransform.rotation = Quaternion.Slerp(m_Hero.CachedTransform.rotation, Quaternion.LookRotation(desiredVelocity), desiredRotationSpeed);
            }

            ClearState();
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        private void UpdateState()
        {
            velocity = m_Rigidbody.velocity;
            if (OnGround)
            {
                jumpPhase = 0;
                if (groundContactCount > 1)
                {
                    contactNormal.Normalize();
                }
            }
            else
            {
                contactNormal = Vector3.up;
            }
        }

        /// <summary>
        /// 清空状态
        /// </summary>
        private void ClearState()
        {
            groundContactCount = 0;
            contactNormal = Vector3.zero;
        }


        /// <summary>
        /// 跳跃
        /// </summary>
        private void Jump()
        {
            if (OnGround || jumpPhase < maxAirJumps)
            {
                jumpPhase += 1;
                float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHight);
                float alignedSpeed = Vector3.Dot(velocity, contactNormal);
                if (velocity.y > 0)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
                }
                velocity += contactNormal * jumpSpeed;
            }
        }

        /// <summary>
        /// 调整速度
        /// </summary>
        /// <returns></returns>
        private void AdjustVelocity()
        {
            //获取映射的 相对斜边的 X 和 Z轴
            //相对于斜边
            Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

            //求在在世界坐标中X轴和Z轴投影长度 也就是相对的X和Z的速度
            //相对于地面
            float currentX = Vector3.Dot(velocity, xAxis);
            float currentZ = Vector3.Dot(velocity, zAxis);

            //最大加速度
            float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
            //最大速度变化
            float maxSpeedChange = acceleration * Time.deltaTime;

            //新的方向速率
            float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

            //新的速率
            velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
        }

        /// <summary>
        /// 求沿斜面 斜边 方向速度
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private Vector3 ProjectOnContactPlane(Vector3 vector)
        {
            return vector - contactNormal * Vector3.Dot(vector, contactNormal);
        }

        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <param name="collision"></param>
        public void EvaluateCollision(Collision collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector3 normal = collision.GetContact(i).normal;
                //onGround |= normal.y >= minGroundDotProduct;
                if (normal.y >= minGroundDotProduct)
                {
                    //onGround = true;
                    groundContactCount += 1;
                    //累积法线
                    contactNormal += normal;
                }
            }
        }

        private void LateUpdate()
        {
            
        }

        #region 外接口
        /// <summary>
        /// 创建英雄
        /// </summary>
        public void CreatHero()
        {
            GameEntry.Entity.ShowEntity(m_HeroData.Id, typeof(Hero), AssetUtility.GetEntityAsset("Hero"), "Hero", 1, m_HeroData);
        }

        /// <summary>
        /// 回收英雄
        /// </summary>
        public void RecycleHero()
        {
            m_Hero = null;
        }

        /// <summary>
        /// 当前场上英雄
        /// </summary>
        public Hero Hero
        {
            get
            {
                return m_Hero;
            }
            set
            {
                m_Hero = value;

                //刷新引用
                m_Rigidbody = m_Hero.CachedRigidbody;
                m_Animator = m_Hero.CachedAnimator;
                minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);

                GameEntry.Camera.SetFreeLookFollowAndLookAt(m_Hero.CachedTransform, m_Hero.CachedLookAtPos);
            }
        }


        /// <summary>
        /// 当前英雄数据
        /// </summary>
        public HeroData HeroData
        {
            get
            {
                return m_HeroData;
            }
        }
        #endregion
    }
}
