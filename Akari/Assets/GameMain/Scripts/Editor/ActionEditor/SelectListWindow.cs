﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Akari.Editor.Action
{
    /// <summary>
    /// SelectListWindow
    /// </summary>
    public class SelectListWindow : EditorWindow
    {
        #region Extension

        public static void ShowTypeWithAttr<T>(Action<Type> callback) where T : Attribute
        {
            List<Type> types = TypeCache.GetTypesWithAttribute<T>().ToList();//AssemblyUtility.FindAllTypeWithAttr<T>();
            List<string> typeNames = types.Select(t => $"{t.GetSimpleName()} | {t.Namespace}").ToList();
            Show(typeNames, t => callback(types[t]));
        }

        #endregion Extension

        public static void Show(List<string> lists, Action<int> callback)
        {
            var win = GetWindow<SelectListWindow>(true, "Select", true);

            win._list.AddRange(lists);
            win._onCallback = callback;
            //win.ShowPopup();
            win.ShowAuxWindow();
        }

        private List<string> _list = new List<string>();
        private Action<int> _onCallback;
        private static string _search = "";
        private Vector2 _scroll = Vector2.zero;
        private GUIStyle _btnStyle;

        private void OnEnable()
        {
        }

        private void OnGUI()
        {
            DrawTool();
            DrawList();
        }

        private void DrawTool()
        {
            using (var v = new GUILayout.VerticalScope("HelpBox"))
            {
                using (var h = new GUILayout.HorizontalScope())
                {
                    _search = EditorGUILayout.TextField("", _search, "SearchTextField");

                    if (string.IsNullOrEmpty(_search))
                    {
                        GUILayout.Label("", "SearchCancelButtonEmpty");
                    }
                    else
                    {
                        if (GUILayout.Button("", "SearchCancelButton"))
                        {
                            GUI.FocusControl(null);
                            _search = "";
                        }
                    }
                }
            }
        }

        private void DrawList()
        {
            if (_btnStyle == null)
            {
                _btnStyle = new GUIStyle("toolbarbutton") { alignment = TextAnchor.MiddleLeft };
            }
            using (var v = new GUILayout.VerticalScope("HelpBox"))
            {
                using (var sv = new GUILayout.ScrollViewScope(_scroll))
                {
                    string searchLow = _search.ToLower();

                    for (int i = 0; i < _list.Count; i++)
                    {
                        string str = _list[i];

                        if (!string.IsNullOrEmpty(searchLow))
                        {
                            string strLow = str.ToLower();

                            if (!strLow.Contains(searchLow))
                            {
                                continue;
                            }
                        }

                        if (GUILayout.Button(str, _btnStyle))
                        {
                            GUI.FocusControl(null);
                            Selected(i);
                        }
                        GUILayout.Space(2);
                    }

                    _scroll = sv.scrollPosition;
                }
            }
        }

        private void Selected(int i)
        {
            if (_onCallback != null)
            {
                _onCallback(i);
            }

            Close();
        }
    }
}
