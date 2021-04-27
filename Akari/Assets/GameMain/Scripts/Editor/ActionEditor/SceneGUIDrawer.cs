using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Akari.Editor.Action
{
    /// <summary>
    /// SceneGUIDrawer
    /// </summary>
    public class SceneGUIDrawer
    {
        public ActionEditorWindow win { get; set; }

        public void OnSceneGUI(SceneView sceneView)
        {
            Matrix4x4 localToWorld = (Matrix4x4)win.actionMachine.localToWorldMatrix;

            FrameConfig config = win.currentFrame;
            if (config == null)
            {
                return;
            }

            if (config.stayAttackRange)
            {//保持范围的时候，不可以设置
                List<RangeConfig> attackRanges = win.FindStayAttackRangeStartWith(win.frameSelectIndex);
                if (attackRanges != null)
                {
                    ProcessRanges(attackRanges, localToWorld, false, -1, new Color(1, 0, 0, 0.25f));
                }
            }
            else
            {
                if (config.attackRanges != null)
                {
                    ProcessRanges(config.attackRanges, localToWorld, true, win.attackRangeSelectIndex, new Color(1, 0, 0, 0.25f));
                }
            }

            if (config.stayBodyRange)
            {//保持范围的时候，不可以设置
                List<RangeConfig> bodyRanges = win.FindStayBodyRangeStartWith(win.frameSelectIndex);
                if (bodyRanges != null)
                {
                    ProcessRanges(bodyRanges, localToWorld, false, -1, new Color(0, 0, 1, 0.25f));
                }
            }
            else
            {
                if (config.bodyRanges != null)
                {
                    ProcessRanges(config.bodyRanges, localToWorld, true, win.bodyRangeSelectIndex, new Color(0, 0, 1, 0.25f));
                }
            }
        }

        private void ProcessRanges(List<RangeConfig> ranges, Matrix4x4 localToWorld, bool enableControl, int selectIndex, Color color)
        {
            Matrix4x4 localToWorldNoScale = Matrix4x4.TRS(localToWorld.MultiplyPoint3x4(Vector3.zero), localToWorld.rotation, Vector3.one);

            Matrix4x4 oldMat = Handles.matrix;
            Handles.matrix = localToWorldNoScale;

            int length = ranges.Count;
            for (int i = 0; i < length; i++)
            {
                RangeConfig config = ranges[i];
                DrawRange(i, config, color);

                if (enableControl && (win.setting.enableAllControl || (selectIndex == i)))
                {
                    ControlRange(i, config.value);
                }
            }

            Handles.matrix = oldMat;
        }

        private void DrawRange(int index, RangeConfig config, Color color)
        {
            HandlesDrawer.H.PushColor(color);
            HandlesDrawer.H.fillPolygon = true;
            switch (config.value)
            {
                case RectItem v:
                    HandlesDrawer.H.DrawRect(v.size, Matrix4x4.Translate((Vector3)v.offset));
                    break;

                case CircleItem v:
                    HandlesDrawer.H.DrawCircle(v.radius, Matrix4x4.Translate((Vector3)v.offset));
                    break;

                case BoxItem v:
                    HandlesDrawer.H.DrawBox(v.size, Matrix4x4.Translate((Vector3)v.offset));
                    break;

                case SphereItem v:
                    HandlesDrawer.H.DrawSphere(v.radius, Matrix4x4.Translate((Vector3)v.offset));
                    break;
            }
            HandlesDrawer.H.fillPolygon = false;
            HandlesDrawer.H.PopColor();
        }

        private float FixFloat(float v)
        {
            return (float)Math.Round(v, 3);
        }

        private BoxBoundsHandle boxHandle = new BoxBoundsHandle();
        private SphereBoundsHandle sphereHandle = new SphereBoundsHandle();

        private void ControlRange(int index, IRangeItem config)
        {
            Vector3 offset = Vector3.zero;
            Vector3 size = Vector3.zero;

            switch (config)
            {
                case RectItem v:
                    offset = (Vector2)v.offset;
                    size = (Vector2)v.size;
                    break;

                case CircleItem v:
                    offset = (Vector2)v.offset;
                    size = new Vector3(v.radius, 0, 0);
                    break;

                case BoxItem v:
                    offset = v.offset;
                    size = v.size;
                    break;

                case SphereItem v:
                    offset = v.offset;
                    size = new Vector2(v.radius, 0);
                    break;

                default:
                    return;
            }

            //draw handle =========================================

            float handleSize = HandleUtility.GetHandleSize(offset);

            switch (Tools.current)
            {
                case Tool.View:
                    break;

                case Tool.Move:
                    offset = (Vector3)Handles.DoPositionHandle(offset, Quaternion.identity);
                    break;

                case Tool.Scale:
                    size = (Vector3)Handles.DoScaleHandle(size, offset, Quaternion.identity, handleSize);
                    break;

                case Tool.Transform:
                    UnityEngine.Vector3 _offset = offset;
                    UnityEngine.Vector3 _size = size;
                    Handles.TransformHandle(ref _offset, UnityEngine.Quaternion.identity, ref _size);
                    offset = (Vector3)_offset;
                    size = (Vector3)_size;
                    break;

                case Tool.Rect:

                    switch (config)
                    {
                        case RectItem v:
                            {
                                boxHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;
                                boxHandle.center = offset;
                                boxHandle.size = size;
                                boxHandle.DrawHandle();
                                offset = (Vector3)boxHandle.center;
                                size = (Vector3)boxHandle.size;
                                break;
                            }

                        case CircleItem v:
                            {
                                sphereHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;
                                sphereHandle.center = offset;
                                sphereHandle.radius = size.x;
                                sphereHandle.DrawHandle();
                                offset = (Vector3)sphereHandle.center;
                                size.x = (float)sphereHandle.radius;
                                break;
                            }
                        case BoxItem v:
                            {
                                boxHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y | PrimitiveBoundsHandle.Axes.Z;
                                boxHandle.center = offset;
                                boxHandle.size = size;
                                boxHandle.DrawHandle();
                                offset = (Vector3)boxHandle.center;
                                size = (Vector3)boxHandle.size;
                                break;
                            }
                        case SphereItem v:
                            {
                                sphereHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y | PrimitiveBoundsHandle.Axes.Z;
                                sphereHandle.center = offset;
                                sphereHandle.radius = size.x;
                                sphereHandle.DrawHandle();
                                offset = (Vector3)sphereHandle.center;
                                size.x = (float)sphereHandle.radius;
                                break;
                            }
                    }

                    break;
            }

            //===============================================

            Func<Vector3> getOffset = () => new Vector3(FixFloat(offset.x), FixFloat(offset.y), FixFloat(offset.z));
            Func<Vector3> getSize = () => new Vector3(FixFloat(size.x), FixFloat(size.y), FixFloat(size.z));
            Func<float> getRadius = () => FixFloat(size.magnitude);

            switch (config)
            {
                case RectItem v:
                    v.offset = getOffset();
                    v.size = getSize();
                    break;

                case CircleItem v:
                    v.offset = getOffset();
                    v.radius = getRadius();
                    break;

                case BoxItem v:
                    v.offset = getOffset();
                    v.size = getSize();
                    break;

                case SphereItem v:
                    v.offset = getOffset();
                    v.radius = getRadius();
                    break;
            }
        }
    }
}
