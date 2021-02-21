using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class UIMainGameForm : UIMainGameFormSign
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitUIData();
        }
    }
}