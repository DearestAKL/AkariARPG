using UnityEngine;
using UnityGameFramework.Runtime;

namespace Akari
{
    public class PlayerComponent : GameFrameworkComponent
    {
        private PlayerInput m_PlayerInput;
        [SerializeField]
        private Hero m_Hero;
        [SerializeField]
        private HeroData m_HeroData;


        private void Start()
        {
            m_PlayerInput = new PlayerInput();
            m_HeroData = new HeroData(1, 1);
        }

        private void OnDestroy()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameEntry.Entity.ShowEntity(m_HeroData.Id, typeof(Hero), AssetUtility.GetEntityAsset("Hero"), "Hero", 1, m_HeroData);
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                var hero = GameEntry.Entity.GetEntity(m_HeroData.Id);
                m_Hero = hero.GetComponent<Hero>();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                m_Hero?.ApplyDamage(null, 5);
            }
        }
    }
}
