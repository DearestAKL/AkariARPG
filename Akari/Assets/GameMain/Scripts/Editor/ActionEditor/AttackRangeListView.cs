﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// AttackRangeListView
    /// </summary>
    [Serializable]
    public class AttackRangeListView : IDataView
    {
        public override string title => "攻击范围";

        public override bool useAre => true;
        private Vector2 scrollPos;

        protected override void OnGUI(Rect rect)
        {
            FrameConfig configs = win.currentFrame;
            if (null == configs)
            {
                return;
            }

            bool lastStay = configs.stayAttackRange;

            GUILayout.BeginVertical(AEStyles.box);
            bool nextStay = EditorGUILayoutEx.DrawObject("保持上一帧", lastStay);
            GUILayout.EndVertical();

            if (nextStay)
            {
                if (!lastStay)
                {
                    configs.attackRanges.Clear();
                    win.attackRangeSelectIndex = -1;
                }
            }
            else
            {
                if (lastStay)
                {//从保持到非保持，则拷贝保持的范围到当前
                    win.CopyAttackRangeToCurrentFrameIfStay();
                }
                win.attackRangeSelectIndex = EditorGUILayoutEx.DrawList(configs.attackRanges, win.attackRangeSelectIndex, ref scrollPos, NewRange, ActionEditorUtility.RangeConfigDrawer);
            }
            configs.stayAttackRange = nextStay;//处理完之后再设置，否者CopyAttackRangeToCurrentFrameIfStay不会执行

        }

        private void NewRange(Action<RangeConfig> adder)
        {
            adder(new RangeConfig());
        }

        public override object CopyData()
        {
            return win.currentAttackRanges;
        }

        public override void PasteData(object data)
        {
            if (win.currentAttackRanges != null && data is List<RangeConfig> ranges)
            {
                win.currentAttackRanges.Clear();
                win.currentAttackRanges.AddRange(ranges);
            }
        }

        public override void OnUpdate()
        {
        }
    }
}
