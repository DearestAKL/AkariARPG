using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class UILoadingForm : UILoadingFormSign
    {
        private float loadingSpeed = 0.01f;
        private float curProgress = 0f;
        private int curProgressInt = 0;
        private int loadingSpeedInt = 1;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitUIData();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            imgProgress.fillAmount = 0f;
            txtLoading.text = "0%";
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            curProgress += loadingSpeed;
            imgProgress.fillAmount = curProgress;

            curProgressInt += loadingSpeedInt;
            txtLoading.text = $"{curProgressInt}%";

            if(curProgress >= 1.0f)
            {
                //GameEntry.UI.CloseUIForm(this);
                Close(true);
            }
        }
    }
}