using System;
using UnityEditor;
using UnityEngine;

namespace Akari.Editor.Action
{
    /// <summary>
    /// StateSetView
    /// </summary>
    [Serializable]
    public class StateSetView : IDataView
    {
        public override string title => "状态设置";
        public override bool useAre => true;

        private Vector2 scrollView = Vector2.zero;

        protected override void OnGUI(Rect rect)
        {
            StateConfig config = win.currentState;
            if (null == config)
            {
                return;
            }

            scrollView = EditorGUILayout.BeginScrollView(scrollView);

            EditorGUI.BeginChangeCheck();

            config.stateName = EditorGUILayoutEx.DrawObject("状态名", config.stateName);
            config.dafualtAnimIndex = EditorGUILayoutEx.DrawObject("默认动画序号", config.dafualtAnimIndex);
            config.animNames = EditorGUILayoutEx.DrawObject("动画名", config.animNames);
            config.fadeTime = EditorGUILayoutEx.DrawObject("过度时间", config.fadeTime);

            config.enableLoop = EditorGUILayoutEx.DrawObject("循环", config.enableLoop);
            if (!config.enableLoop)
            {
                config.nextStateName = EditorGUILayoutEx.DrawObject("下一个状态", config.nextStateName);
                config.nextAnimIndex = EditorGUILayoutEx.DrawObject("下一个状态动画序号", config.nextAnimIndex);
            }

            if (EditorGUI.EndChangeCheck())
            {
                win.actionMachineDirty = true;
            }

            EditorGUILayout.EndScrollView();
        }

        public override void OnUpdate()
        {
        }

        public override object CopyData()
        {
            return win.currentState;
        }

        public override void PasteData(object data)
        {
            if (win.currentState != null && data is StateConfig state)
            {
                win.currentStates[win.stateSelectIndex] = state;
            }
        }
    }
}
