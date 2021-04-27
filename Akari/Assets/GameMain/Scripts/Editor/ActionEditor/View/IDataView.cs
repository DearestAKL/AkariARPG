using UnityEngine;

namespace Akari.Editor.Action
{
    /// <summary>
    /// IDataView
    /// </summary>
    public abstract class IDataView : IView
    {
        public abstract object CopyData();

        public abstract void PasteData(object data);

        protected override void OnHeaderDraw()
        {
            base.OnHeaderDraw();

            if (GUILayout.Button("C", AEStyles.view_head, GUILayout.Width(20)))
            {
                GUI.FocusControl(null);
                win.copyBuffer = CopyData();
            }

            if (GUILayout.Button("P", AEStyles.view_head, GUILayout.Width(20)))
            {
                GUI.FocusControl(null);
                object data = win.copyBuffer;
                if (data != null)
                {
                    PasteData(data);
                }
            }
        }
    }
}
