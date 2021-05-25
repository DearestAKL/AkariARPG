using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;

namespace Akari
{
    public enum InputEvents
    {
        None = 0b00000,
        Moving = 0b00001,
        Attack = 0b00010,
        Jump = 0b00100,
        Dash = 0b01000,
        F = 0b10000,
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

        // 禁止输入 角色无法进行如何操作
        [SerializeField]
        private bool m_IsProhibitInput = false;

        private bool m_IsInMainPanel = false;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            m_Input = new GameInput();
            m_Input.Enable();


            m_Input.Player.Alt.started += AltDown;
            m_Input.Player.Alt.performed += AltUp;
        }

        private void Update() 
        {
            if (m_IsProhibitInput)
            {
                return;
            }

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

            if (player.Dash.triggered)
            {
                m_InputEvents |= InputEvents.Dash;
            }

            if (player.F.triggered)
            {
                m_InputEvents |= InputEvents.F;
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

        public bool IsInMainPanel
        {
            set 
            {
                m_IsInMainPanel = value;
                SetProhibitInput(!m_IsInMainPanel);
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


        /// <summary>
        /// 获取有效的Camera下的水平输入 用于移动相关
        /// </summary>
        /// <returns></returns>
        public Vector2 GetEffectiveCameraAxisValue()
        {
            return m_IsProhibitMove ? Vector2.zero : GetCameraAxisValue();
        }

        /// <summary>
        /// 得到Camera下的水平输入
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCameraAxisValue()
        {
            Vector2 desiredAxisValue;
            var input = Vector2.ClampMagnitude(m_AxisValue, 1f);
            var cameraTrans = GameEntry.Camera.MainCamera.transform;
            if (cameraTrans)
            {
                Vector3 forward = cameraTrans.forward;
                forward.y = 0f;
                forward.Normalize();
                Vector3 right = cameraTrans.right;
                right.y = 0f;
                right.Normalize();
                desiredAxisValue = (forward * input.y + right * input.x).ToVector2();
            }
            else
            {
                desiredAxisValue = new Vector2(input.x, input.y);
            }

            return desiredAxisValue;
        }


        #region Event
        private void AltDown(InputAction.CallbackContext callbackContext)
        {
            if (m_IsInMainPanel)
            {
                SetProhibitInput(true);
            }
        }

        private void AltUp(InputAction.CallbackContext callbackContext)
        {
            if (m_IsInMainPanel)
            {
                SetProhibitInput(false);
            }
        }

        /// <summary>
        /// 是否禁止输入
        /// </summary>
        private void SetProhibitInput(bool isProhibit)
        {
            m_IsProhibitInput = isProhibit;
            if (m_IsProhibitInput)
            {
                //输入禁止,则显示鼠标
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameEntry.Camera.SetInputProvider(false);
            }
            else
            {
                //恢复输入,则隐藏鼠标
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                GameEntry.Camera.SetInputProvider(true);
            }
        }
        #endregion
    }
}
