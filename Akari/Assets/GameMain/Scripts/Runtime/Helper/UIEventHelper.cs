using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Akari 
{
    public static class UIEventHelper
    {
        private static bool bClick = true;
        public delegate void ButtonClickBoolAction();
        public delegate void ButtonClickBoolAction<T>(T value);

        //同步执行事件 按钮
        public static void Add(this Button.ButtonClickedEvent buttonClickedEvent, ButtonClickBoolAction action)
        {
            buttonClickedEvent.AddListener(() => {
                if (bClick)
                {
                    bClick = false;
                    action.Invoke();
                    bClick = true;
                }
            });
        }

        //同步带参数
        public static void Add<T>(this Button.ButtonClickedEvent buttonClickedEvent, ButtonClickBoolAction<T> action, T param)
        {
            buttonClickedEvent.AddListener(() =>
            {
                if (bClick)
                {
                    bClick = false;
                    action.Invoke(param);
                    bClick = true;
                }
            });
        }
    }
}