using UnityEngine;
using System.Collections.Generic;
using System;

namespace Akari.HUD
{

    // 功能：精灵图元的属性显示
    [ExecuteInEditMode]
    public class MyUISpritePreView : MonoBehaviour
    {
        public string spritename;  // 精灵名字

        public bool isValid { get { return !string.IsNullOrEmpty(spritename); } }
        public new string name { get { return spritename; } set { spritename = value; } }
    }
}
