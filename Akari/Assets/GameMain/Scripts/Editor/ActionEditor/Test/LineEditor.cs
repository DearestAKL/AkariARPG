using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Akari.Editor
{
    public class LineEditor : EditorWindow
    {
        [MenuItem("Tools/Line编辑器")]
        public static void ShowEditor()
        {
            EditorWindow.GetWindow<LineEditor>();
        }

        public void OnGUI()
        {
            Handles.color = new Color(0.15f, 0.15f, 0.15f);

            Handles.DrawLine(Vector2.zero, new Vector2(100, 100));
        }
    }
}
