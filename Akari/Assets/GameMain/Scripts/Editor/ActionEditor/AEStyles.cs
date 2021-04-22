using UnityEngine;

namespace Akari
{
    /// <summary>
    /// AEStyles
    /// </summary>
    public static class AEStyles
    {
        public static GUIStyle list_tool_bg = "IN Footer";
        public static GUIStyle view_head = "HeaderButton";
        public static GUIStyle view_bg = "hostview";
        public static GUIStyle text_bg = "WhiteBackground";
        public static GUIStyle item_head_normal = "MeTransitionSelect";
        public static GUIStyle item_head_select = "MeTransitionSelectHead";
        public static GUIStyle item_body_normal = new GUIStyle("ScrollViewAlt") { alignment = TextAnchor.MiddleLeft };
        public static GUIStyle item_body_select = new GUIStyle("TE ElementBackground") { alignment = TextAnchor.MiddleLeft };
        public static GUIStyle frame_label = "CN StatusWarn";
        public static GUIStyle box = "HelpBox";
    }
}
