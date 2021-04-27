using System;
using UnityEditor;
using UnityEngine;

namespace Akari.Editor.Action
{
    /// <summary>
    /// GlobalActionSetView
    /// </summary>
    [Serializable]
    public class GlobalActionSetView : IView
    {
        public override string title => "全局动作设置";
        public override bool useAre => true;

        private Vector2 scrollView = Vector2.zero;

        protected override void OnGUI(Rect rect)
        {
            object obj = win.currentGlobalAction;
            if (null == obj)
            {
                return;
            }

            scrollView = EditorGUILayout.BeginScrollView(scrollView);
            EditorGUILayoutEx.DrawObject(GUIContent.none, obj, obj.GetType());
            EditorGUILayout.EndScrollView();
        }

        public override void OnUpdate()
        {
        }
    }
}
