﻿using System;
using UnityEditor;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// ActionSetView
    /// </summary>
    [Serializable]
    public class ActionSetView : IView
    {
        public override string title => "动作设置";

        public override bool useAre => true;

        private Vector2 scrollView = Vector2.zero;

        protected override void OnGUI(Rect rect)
        {
            object obj = win.currentAction;
            if (null == obj)
            {
                return;
            }

            scrollView = EditorGUILayout.BeginScrollView(scrollView);
            EditorGUILayoutEx.DrawObject(GUIContent.none, obj, obj.GetType());
            EditorGUILayout.EndScrollView();

            /*
            Type type = obj.GetType();
            FieldInfo[] fieldInfos = type.GetFields();
            foreach (var fieldInfo in fieldInfos)
            {
                DrawField(obj, fieldInfo);
            }*/
        }

        /*
         private void DrawField(object obj, FieldInfo fieldInfo)
         {
             EditorGUILayoutEx.DrawField(obj, fieldInfo);
         }
         */

        public override void OnUpdate()
        {
        }
    }
}
