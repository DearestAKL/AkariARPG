using UnityEditor;
using UnityEngine;

namespace Akari.Editor.Action
{
    /// <summary>
    /// TestActionMachineEditor
    /// </summary>
    [CustomEditor(typeof(ActionMachineTest))]
    public class ActionMachineTestEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ActionMachineTest actionMachine = (ActionMachineTest)target;

            base.OnInspectorGUI();

            if (GUILayout.Button("打开编辑器"))
            {
                ActionEditorWindow.ShowEditor(actionMachine.gameObject, actionMachine.config);
            }
        }
    }
}
