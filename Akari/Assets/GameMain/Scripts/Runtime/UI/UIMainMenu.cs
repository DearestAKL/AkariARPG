using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class UIMainMenu : UIMainMenuSign
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitUIData();

            btnStart.onClick.Add(OnStart);
            btnContinue.onClick.Add(OnContinue);
            btnSetting.onClick.Add(OnSetting);
            btnAbout.onClick.Add(OnAbout);        
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        #region Event
        private void OnStart() 
        {
            GameEntry.Event.Fire(ChangeSceneEventArgs.EventId, ChangeSceneEventArgs.Create((int)SceneId.Main));
        }
        private void OnContinue() 
        {
        
        }
        private void OnSetting() 
        {
            GameEntry.UI.OpenUIForm(EnumUIForm.UISetting);
        }
        private void OnAbout() 
        {

        }
        #endregion
    }
}