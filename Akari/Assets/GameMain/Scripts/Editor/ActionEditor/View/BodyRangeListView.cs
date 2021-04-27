using System;
using System.Collections.Generic;
using UnityEngine;

namespace Akari.Editor.Action
{
    /// <summary>
    /// BodyRangeListView
    /// </summary>
    [Serializable]
    public class BodyRangeListView : IDataView
    {
        public override string title => "身体范围";

        public override bool useAre => true;
        private Vector2 scrollPos;

        protected override void OnGUI(Rect rect)
        {
            FrameConfig configs = win.currentFrame;
            if (null == configs)
            {
                return;
            }

            bool lastStay = configs.stayBodyRange;

            GUILayout.BeginVertical(AEStyles.box);
            bool nextStay = EditorGUILayoutEx.DrawObject("保持上一帧", lastStay);
            GUILayout.EndVertical();

            if (nextStay)
            {
                if (!lastStay)
                {
                    configs.bodyRanges.Clear();
                    win.bodyRangeSelectIndex = -1;
                }
            }
            else
            {
                if (lastStay)
                {//从保持到非保持，则拷贝保持的范围到当前
                    win.CopyBodyRangeToCurrentFrameIfStay();
                }
                win.bodyRangeSelectIndex = EditorGUILayoutEx.DrawList(configs.bodyRanges, win.bodyRangeSelectIndex, ref scrollPos, NewRange, ActionEditorUtility.RangeConfigDrawer);
            }
            configs.stayBodyRange = nextStay;//处理完之后再设置，否者CopyBodyRangeToCurrentFrameIfStay不会执行
        }

        private void NewRange(Action<RangeConfig> adder)
        {
            adder(new RangeConfig());
        }

        public override object CopyData()
        {
            return win.currentBodyRanges;
        }

        public override void PasteData(object data)
        {
            if (win.currentBodyRanges != null && data is List<RangeConfig> ranges)
            {
                win.currentBodyRanges.Clear();
                win.currentBodyRanges.AddRange(ranges);
            }
        }

        public override void OnUpdate()
        {
        }
    }
}
