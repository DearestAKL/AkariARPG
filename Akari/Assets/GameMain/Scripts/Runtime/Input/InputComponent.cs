using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;

namespace Akari
{
    public enum InputEvents
    {
        None = 0b0000,
        Moving = 0b0001,
        Attack = 0b0010,
        Jump = 0b0100,
        Jumping = 0b1000,
    }

    public class InputComponent : GameFrameworkComponent
    {
        private InputEvents m_InputEvents = InputEvents.None;

        private Vector2 m_AxisValue = Vector2.zero;

        private GameInput m_Input;

        #region 禁用

        // 禁止移动 但可以旋转
        [SerializeField]
        private bool m_IsProhibitMove = false;

        #endregion

        protected override void Awake()
        {
            base.Awake();

            m_Input = new GameInput();
            m_Input.Enable();
        }

        private void Update() 
        {
            var player = m_Input.Player;
            m_AxisValue = player.Move.ReadValue<Vector2>();

            if (player.Move.phase == InputActionPhase.Started)
            {
                m_InputEvents |= InputEvents.Moving;
            }

            if (player.Attack.triggered)
            {
                m_InputEvents |= InputEvents.Attack;
                m_IsProhibitMove = true;
            }

            if (player.Jump.triggered)
            {
                m_InputEvents |= InputEvents.Jump;
            }
        }

        #region 外接口

        public bool HasEvent(InputEvents e,bool fullMatch = false)
        {
            return fullMatch? ((m_InputEvents & e) == m_InputEvents) : ((m_InputEvents & e) != 0);
        }

        public InputEvents InputEvents
        {
            get { return m_InputEvents; }
            //private set { m_InputEvent = value; }
        }

        public Vector2 AxisValue
        {
            get 
            { 
                return m_IsProhibitMove? Vector2.zero: m_AxisValue; 
            }
            //private set { m_AxisValue = value; }
        }

        public Vector2 AxisValueDirection
        {
            get
            {
                return m_AxisValue.normalized;
            }
        }

        public void Clear()
        {
            m_InputEvents = InputEvents.None;
            m_AxisValue = Vector2.zero;
        }

        /// <summary>
        /// 禁止移动 但可以旋转
        /// </summary>
        public bool IsProhibitMove
        {
            get
            {
                return m_IsProhibitMove;
            }
            set
            {
                m_IsProhibitMove = value;
            }
        }
        #endregion
    }
}
