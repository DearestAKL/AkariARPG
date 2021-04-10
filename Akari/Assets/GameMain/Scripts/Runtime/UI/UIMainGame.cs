using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class UIMainGame : UIMainGameSign
    {
        private InfoBarEx hpBar;
        private InfoBarEx cpBar;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitUIData();

            //设置HP 和 CP
            hpBar = new InfoBarEx(goHP.GetComponent<ReferenceCollector>());
            cpBar = new InfoBarEx(goMP.GetComponent<ReferenceCollector>());

            InitData();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(HeroApplyDamageEventArgs.EventId, ApplyDamage);
            GameEntry.Event.Subscribe(HeroRestoreHealthEventArgs.EventId, RestoreHealth);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(HeroApplyDamageEventArgs.EventId, ApplyDamage);
            GameEntry.Event.Unsubscribe(HeroRestoreHealthEventArgs.EventId, RestoreHealth);
        }

        #region Data
        private void InitData()
        {
            var heroData = GameEntry.Player.HeroData;
            hpBar.InitData(heroData.HP, heroData.MaxHP);
        }
        #endregion

        #region Click

        #endregion

        #region Event
        private void ApplyDamage(object sender, GameEventArgs e)
        {
            HeroApplyDamageEventArgs ne = (HeroApplyDamageEventArgs)e;
            if (ne == null)
                return;

            StartCoroutine(hpBar.MinusTransition(ne.FromHPRatio, ne.ToHPRatio, 0.5f));
        }

        private void RestoreHealth(object sender, GameEventArgs e)
        {
            HeroRestoreHealthEventArgs ne = (HeroRestoreHealthEventArgs)e;
            if (ne == null)
                return;

            StartCoroutine(hpBar.AddTransition(ne.FromHPRatio, ne.ToHPRatio, 0.5f));
        }

        #endregion
    }
}
