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
        private InfoBarEx mpBar;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitUIData();

            //设置HP MP
            hpBar = new InfoBarEx(goHP.GetComponent<ReferenceCollector>());
            mpBar = new InfoBarEx(goMP.GetComponent<ReferenceCollector>());
            hpBar.InitData(20, 30);
            mpBar.InitData(15, 20);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(HeroApplyDamageEventArgs.EventId, ApplyDamage);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(HeroApplyDamageEventArgs.EventId, ApplyDamage);
        }

        #region Msg

        #endregion

        #region Event
        private void ApplyDamage(object sender, GameEventArgs e)
        {
            HeroApplyDamageEventArgs ne = (HeroApplyDamageEventArgs)e;
            if (ne == null)
                return;

            StartCoroutine(hpBar.MinusTransition(ne.FromHPRatio, ne.ToHPRatio, 0.5f));
        }

        #endregion
    }
}
