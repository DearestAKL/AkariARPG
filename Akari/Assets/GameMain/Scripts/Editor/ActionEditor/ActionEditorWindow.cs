using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Akari
{
    /// <summary>
    /// 动作编辑器
    /// </summary>
    public class ActionEditorWindow : EditorWindow
    {
        [MenuItem("Tools/动作编辑器")]
        public static void ShowEditor()
        {
            EditorWindow.GetWindow<ActionEditorWindow>();
        }

        public static void ShowEditor(GameObject target, TextAsset config) 
        { 
        
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                GUILayout.Space(5f);
                if (GUILayout.Button("创建", GUILayout.Width(80)))
                {
                    GUI.FocusControl(null);
                    CreateNew();
                }
            }
            EditorGUILayout.EndVertical();
        }

        void CreateNew()
        {

            var filePath = Utility.Path.GetRegularPath(Path.Combine("Assets/MachineConfig.bytes"));
            var ss = new MachineConfig();
            ss.firstStateName = "测试";
            string data = JsonUtility.ToJson(ss);
            File.WriteAllText(filePath, data);
            AssetDatabase.Refresh();
            Debug.Log($"配置已创建到 : {filePath}");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }
}
