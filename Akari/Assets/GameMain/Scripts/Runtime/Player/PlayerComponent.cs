using UnityEngine;
using UnityGameFramework.Runtime;

namespace Akari
{
    public class PlayerComponent : GameFrameworkComponent
    {
        //private PlayerInput m_PlayerInput;
        [SerializeField]
        private Hero m_Hero;
        [SerializeField]
        private HeroData m_HeroData;
        [SerializeField]
        private Camera m_Camera;

        private void Start()
        {
            //m_PlayerInput = new PlayerInput();
            m_HeroData = new HeroData(1, 1);
        }

        private void OnDestroy()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                m_Hero?.RestoreHealth(null, 5);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                m_Hero?.ApplyDamage(null, 5);
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
