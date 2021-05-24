using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Akari.Editor
{
    public static class ColorUtility
    {

        ///A greyscale color
        public static Color Grey(float value)
        {
            return new Color(value, value, value);
        }

        ///The color, with alpha
        public static Color WithAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }

    public class Cutscene : MonoBehaviour
    {
        //版本号
        public const float VERSION_NUMBER = 0.01f;

        public float length = 20;
        public float viewTimeMin = -1;
        public float viewTimeMax = 21;
        public float currentTime = -1;

        public string name = "测试";

        public bool isActive { get; private set; }
        public List<IDirectable> directables { get; private set; }
    }

    public class CutsceneEditor : EditorWindow
    {
        struct GuideLine
        {
            public float time;
            public Color color;
            public GuideLine(float time, Color color)
            {
                this.time = time;
                this.color = color;
            }
        }

        private Cutscene cutscene = new Cutscene();

        public static CutsceneEditor current;
        public List<CutsceneTrack> tracks = new List<CutsceneTrack>();

        private static float LEFT_MARGIN
        {
            //caps for consistency. margin on the left side. The width of the group/tracks list.
            //保持一致性的上限。 左边距。 组/曲目列表的宽度。
            get { return Prefs.trackListLeftMargin; }
            set { Prefs.trackListLeftMargin = Mathf.Clamp(value, 230, 400); }
        }
        private const float RIGHT_MARGIN = 16; //margin on the right side 右边距
        private const float TOOLBAR_HEIGHT = 20; //the height of the toolbar 工具栏高度
        private const float TOP_MARGIN = 40; //top margin AFTER the toolbar 工具栏之后的上边距
        private const float GROUP_HEIGHT = 21; //height of group headers 组标题的高度
        private const float TRACK_MARGINS = 4;  //margin between tracks of same group (top/bottom) 同一组轨道之间的边距（顶部/底部）
        private const float GROUP_RIGHT_MARGIN = 4;  //margin at the right side of groups 组右边距
        private const float TRACK_RIGHT_MARGIN = 4;  //margin at the right side of tracks 轨道右侧的边距
        private const float FIRST_GROUP_TOP_MARGIN = 22; //initial top margin 初始组上边距

        private static readonly Color LIST_SELECTION_COLOR = new Color(0.5f, 0.5f, 1, 0.3f);

        private float MAGNET_SNAP_INTERVAL { get { return viewTime * 0.01f; } }

        //Layout Rects
        private Rect topLeftRect;   //for playback controls 播放控制
        private Rect topMiddleRect; //for time info 时间信息
        private Rect leftRect;      //for group/track list 组与轨道列表
        private Rect centerRect;    //for timeline 时间线

        //[System.NonSerialized] private ActionClipWrapper interactingClip;
        [System.NonSerialized] private bool isMovingScrubCarret;
        [System.NonSerialized] private bool isMovingEndCarret;
        [System.NonSerialized] private bool isMouseButton2Down;
        [System.NonSerialized] private Vector2 scrollPos;
        [System.NonSerialized] private float totalHeight;

        [System.NonSerialized] private Vector2 mousePosition;
        [System.NonSerialized] private bool willRepaint;
        [System.NonSerialized] private bool isResizingLeftMargin;
        [System.NonSerialized] private float[] magnetSnapTimesCache;
        [System.NonSerialized] private List<GuideLine> pendingGuides;

        [System.NonSerialized] private CutsceneTrack pickedTrack;

        [System.NonSerialized] private float timeInfoStart;
        [System.NonSerialized] private float timeInfoEnd;
        [System.NonSerialized] private float timeInfoInterval;
        [System.NonSerialized] private float timeInfoHighMod;

        [MenuItem("Tools/Line编辑器")]
        public static void ShowEditor()
        {
            CutsceneEditor.ShowWindow();
        }

        public static void ShowWindow() 
        {
            var window = EditorWindow.GetWindow(typeof(CutsceneEditor)) as CutsceneEditor;
            window.InitializeAll();
            window.Show();
        }

        void InitializeAll()
        {
            tracks.Add(new CutsceneTrack());
            tracks.Add(new CutsceneTrack());
        }

        public void OnGUI()
        {
            GUI.skin.label.richText = true;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            EditorStyles.label.richText = true;
            EditorStyles.textField.wordWrap = true;
            EditorStyles.foldout.richText = true;
            var e = Event.current;
            mousePosition = e.mousePosition;
            current = this;

            //remove notifications quickly
            if (e.type == EventType.MouseDown) { RemoveNotification(); }

            //button 2 seems buggy
            if (e.button == 2 && e.type == EventType.MouseDown) { isMouseButton2Down = true; }
            if (e.button == 2 && e.rawType == EventType.MouseUp) { isMouseButton2Down = false; }


            topLeftRect = new Rect(0, TOOLBAR_HEIGHT, LEFT_MARGIN, TOP_MARGIN);
            topMiddleRect = new Rect(LEFT_MARGIN, TOOLBAR_HEIGHT, screenWidth - LEFT_MARGIN - RIGHT_MARGIN, TOP_MARGIN);
            leftRect = new Rect(0, TOOLBAR_HEIGHT + TOP_MARGIN, LEFT_MARGIN, screenHeight - TOOLBAR_HEIGHT - TOP_MARGIN + scrollPos.y);
            centerRect = new Rect(LEFT_MARGIN, TOP_MARGIN + TOOLBAR_HEIGHT, screenWidth - LEFT_MARGIN - RIGHT_MARGIN, screenHeight - TOOLBAR_HEIGHT - TOP_MARGIN + scrollPos.y);

            //...
            ShowTimeInfo(topMiddleRect);
            ShowToolbar();
            DoScrubControls();

            //Timelines
            var scrollRect1 = Rect.MinMaxRect(0, centerRect.yMin, screenWidth, screenHeight - 5);
            var scrollRect2 = Rect.MinMaxRect(0, centerRect.yMin, screenWidth, totalHeight + 150);
            scrollPos = GUI.BeginScrollView(scrollRect1, scrollPos, scrollRect2);
            ShowTracksList(leftRect);
            ShowTimeLines(centerRect);
            GUI.EndScrollView();

            DrawGuides();
        }

        //left - the groups and tracks info and option per group/track
        void ShowTracksList(Rect leftRect)
        {

            var e = Event.current;

            //allow resize list width
            var scaleRect = new Rect(leftRect.xMax - 4, leftRect.yMin, 4, leftRect.height);
            AddCursorRect(scaleRect, MouseCursor.ResizeHorizontal);
            if (e.type == EventType.MouseDown && e.button == 0 && scaleRect.Contains(e.mousePosition)) { isResizingLeftMargin = true; e.Use(); }
            if (isResizingLeftMargin) { LEFT_MARGIN = e.mousePosition.x + 2; }
            if (e.rawType == EventType.MouseUp) { isResizingLeftMargin = false; }

            GUI.enabled = cutscene.currentTime <= 0;

            //starting height && search.
            var nextYPos = FIRST_GROUP_TOP_MARGIN;

            //TRACKS
            for (int t = 0; t < tracks.Count; t++)
            {
                var track = tracks[t];
                var yPos = nextYPos;

                var trackRect = new Rect(10, yPos, leftRect.width - TRACK_RIGHT_MARGIN - 10, track.finalHeight);
                nextYPos += track.finalHeight + TRACK_MARGINS;

                //GRAPHICS
                GUI.color = ColorUtility.Grey((track.isActive ? 0.25f : 0.2f));
                GUI.DrawTexture(trackRect, Styles.whiteTexture);
                GUI.color = Color.white.WithAlpha(0.25f);
                GUI.Box(trackRect, string.Empty, (GUIStyle)"flow node 0");

                if (track == pickedTrack)
                {
                    GUI.color = LIST_SELECTION_COLOR;
                    GUI.DrawTexture(trackRect, Styles.whiteTexture);
                }

                //custom color indicator
                if (track.isActive && track.color != Color.white && track.color.a > 0.2f)
                {
                    GUI.color = track.color;
                    var colorRect = new Rect(trackRect.xMax + 1, trackRect.yMin, 2, track.finalHeight);
                    GUI.DrawTexture(colorRect, Styles.whiteTexture);
                }
                GUI.color = Color.white;
                //

                ///
                GUI.BeginGroup(trackRect);
                GUI.DrawTexture(trackRect, Styles.whiteTexture);
                //track.OnTrackInfoGUI(trackRect);
                GUI.EndGroup();
                ///

                AddCursorRect(trackRect, pickedTrack == null ? MouseCursor.Link : MouseCursor.MoveArrow);

                //CONTEXT
                //if (e.type == EventType.ContextClick && trackRect.Contains(e.mousePosition))
                //{
                //    var menu = new GenericMenu();
                //    menu.AddItem(new GUIContent("Disable Track"), !track.isActive, () => { track.isActive = !track.isActive; });
                //    menu.AddItem(new GUIContent("Lock Track"), track.isLocked, () => { track.isLocked = !track.isLocked; });
                //    menu.AddItem(new GUIContent("Copy"), false, () => { copyTrack = track; });
                //    if (track.GetType().RTGetAttribute<UniqueElementAttribute>(true) == null)
                //    {
                //        menu.AddItem(new GUIContent("Duplicate"), false, () =>
                //        {
                //            group.DuplicateTrack(track);
                //            InitClipWrappers();
                //        });
                //    }
                //    else
                //    {
                //        menu.AddDisabledItem(new GUIContent("Duplicate"));
                //    }
                //    menu.AddSeparator("/");
                //    menu.AddItem(new GUIContent("Delete Track"), false, () =>
                //    {
                //        if (EditorUtility.DisplayDialog("Delete Track", "Are you sure?", "YES", "NO!"))
                //        {
                //            group.DeleteTrack(track);
                //            InitClipWrappers();
                //        }
                //    });
                //    menu.ShowAsContext();
                //    e.Use();
                //}

                //REORDERING
                if (e.type == EventType.MouseDown && e.button == 0 && trackRect.Contains(e.mousePosition))
                {
                    //CutsceneUtility.selectedObject = track;
                    pickedTrack = track;
                    e.Use();
                }

                if (pickedTrack != null && pickedTrack != track)
                {
                    if (trackRect.Contains(e.mousePosition))
                    {
                        var markRect = new Rect(trackRect.x, (tracks.IndexOf(pickedTrack) < t) ? trackRect.yMax - 2 : trackRect.y, trackRect.width, 2);
                        GUI.color = Color.grey;
                        GUI.DrawTexture(markRect, Styles.whiteTexture);
                        GUI.color = Color.white;
                    }

                    if (e.rawType == EventType.MouseUp && e.button == 0 && trackRect.Contains(e.mousePosition))
                    {
                        tracks.Remove(pickedTrack);
                        tracks.Insert(t, pickedTrack);
                        //cutscene.Validate();
                        pickedTrack = null;
                        e.Use();
                    }
                }
            }
            GUI.enabled = true;
            GUI.color = Color.white;
        }


        //top mid - viewTime selection and time info
        void ShowTimeInfo(Rect topMiddleRect)
        {

            GUI.color = Color.white.WithAlpha(0.2f);
            GUI.Box(topMiddleRect, string.Empty, EditorStyles.toolbarButton);
            GUI.color = Color.black.WithAlpha(0.2f);
            GUI.Box(topMiddleRect, string.Empty, Styles.timeBoxStyle);
            GUI.color = Color.white;

            timeInfoInterval = 1000000f;
            timeInfoHighMod = timeInfoInterval;
            var lowMod = 0.01f;
            var modulos = new float[] { 0.1f, 0.5f, 1, 5, 10, 50, 100, 500, 1000, 5000, 10000, 50000, 100000, 250000, 500000 }; //... O.o
            for (var i = 0; i < modulos.Length; i++)
            {
                var count = viewTime / modulos[i];
                if (centerRect.width / count > 50)
                { //50 is approx width of label
                    timeInfoInterval = modulos[i];
                    lowMod = i > 0 ? modulos[i - 1] : lowMod;
                    timeInfoHighMod = i < modulos.Length - 1 ? modulos[i + 1] : timeInfoHighMod;
                    break;
                }
            }

            var doFrames = Prefs.timeStepMode == Prefs.TimeStepMode.Frames;
            var timeStep = doFrames ? (1f / Prefs.frameRate) : lowMod;

            timeInfoStart = (float)Mathf.FloorToInt(viewTimeMin / timeInfoInterval) * timeInfoInterval;
            timeInfoEnd = (float)Mathf.CeilToInt(viewTimeMax / timeInfoInterval) * timeInfoInterval;
            timeInfoStart = Mathf.Round(timeInfoStart * 10) / 10;
            timeInfoEnd = Mathf.Round(timeInfoEnd * 10) / 10;

            GUI.BeginGroup(topMiddleRect);
            {
                //the minMax slider
                var _timeMin = viewTimeMin;
                var _timeMax = viewTimeMax;
                var sliderRect = new Rect(5, 0, topMiddleRect.width - 10, 18);
                EditorGUI.MinMaxSlider(sliderRect, ref _timeMin, ref _timeMax, 0, maxTime);
                viewTimeMin = _timeMin;
                viewTimeMax = _timeMax;
                if (sliderRect.Contains(Event.current.mousePosition) && Event.current.clickCount == 2)
                {
                    viewTimeMin = 0;
                    viewTimeMax = length;
                }

                GUI.color = Color.white.WithAlpha(0.1f);
                GUI.DrawTexture(Rect.MinMaxRect(0, TOP_MARGIN - 1, topMiddleRect.xMax, TOP_MARGIN), Styles.whiteTexture);
                GUI.color = Color.white;

                //the step interval
                if (centerRect.width / (viewTime / timeStep) > 6)
                {
                    for (var i = timeInfoStart; i <= timeInfoEnd; i += timeStep)
                    {
                        var posX = TimeToPos(i);
                        var frameRect = Rect.MinMaxRect(posX - 1, TOP_MARGIN - 2, posX + 1, TOP_MARGIN - 1);
                        GUI.color = Color.white;
                        GUI.DrawTexture(frameRect, Styles.whiteTexture);
                        GUI.color = Color.white;
                    }
                }

                //the time interval
                for (var i = timeInfoStart; i <= timeInfoEnd; i += timeInfoInterval)
                {

                    var posX = TimeToPos(i);
                    var rounded = Mathf.Round(i * 10) / 10;

                    GUI.color = Color.white;
                    var markRect = Rect.MinMaxRect(posX - 2, TOP_MARGIN - 3, posX + 2, TOP_MARGIN - 1);
                    GUI.DrawTexture(markRect, Styles.whiteTexture);
                    GUI.color = Color.white;

                    var text = doFrames ? (rounded * Prefs.frameRate).ToString("0") : rounded.ToString("0.00");
                    var size = GUI.skin.GetStyle("label").CalcSize(new GUIContent(text));
                    var stampRect = new Rect(0, 0, size.x, size.y);
                    stampRect.center = new Vector2(posX, TOP_MARGIN - size.y + 4);
                    GUI.color = rounded % timeInfoHighMod == 0 ? Color.white : Color.white.WithAlpha(0.5f);
                    GUI.Box(stampRect, text, (GUIStyle)"label");
                    GUI.color = Color.white;
                }

                //the number showing current time when scubing
                if (cutscene.currentTime > 0)
                {
                    var label = doFrames ? (cutscene.currentTime * Prefs.frameRate).ToString("0") : cutscene.currentTime.ToString("0.00");
                    var text = "<b><size=17>" + label + "</size></b>";
                    var size = Styles.headerBoxStyle.CalcSize(new GUIContent(text));
                    var posX = TimeToPos(cutscene.currentTime);
                    var stampRect = new Rect(0, 0, size.x, size.y);
                    stampRect.center = new Vector2(posX, TOP_MARGIN - size.y / 2);

                    GUI.backgroundColor = Color.black.WithAlpha(0.4f);
                    // TODO:
                    //GUI.color = scruberColor;
                    GUI.color = Color.yellow;
                    GUI.Box(stampRect, text, Styles.headerBoxStyle);
                }

                //the length position carret texture and pre-exit length indication
                var lengthPos = TimeToPos(length);
                var lengthRect = new Rect(0, 0, 16, 16);
                lengthRect.center = new Vector2(lengthPos, TOP_MARGIN - 2);
                GUI.color = Color.white;
                GUI.DrawTexture(lengthRect, Styles.carretIcon);
                GUI.color = Color.white;
            }
            GUI.EndGroup();
        }

        void ShowToolbar()
        {
            GUI.enabled = cutscene.currentTime <= 0;
            var e = Event.current;

            GUI.backgroundColor = Color.white;
            GUI.color = Color.white;
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button(string.Format("[{0}]", cutscene.name), EditorStyles.toolbarDropDown, GUILayout.Width(100)))
            {
                GenericMenu.MenuFunction2 SelectCutscene = (object cut) =>
                {
                    Selection.activeObject = (Cutscene)cut;
                    EditorGUIUtility.PingObject((Cutscene)cut);
                };

                var cutscenes = FindObjectsOfType<Cutscene>();
                var menu = new GenericMenu();
                foreach (Cutscene cut in cutscenes)
                {
                    menu.AddItem(new GUIContent(string.Format("[{0}]", cut.name)), cut == cutscene, SelectCutscene, cut);
                }
                menu.ShowAsContext();
            }

            if (GUILayout.Button("Snap: " + Prefs.snapInterval.ToString(), EditorStyles.toolbarDropDown, GUILayout.Width(90)))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("0.001"), false, () => { Prefs.timeStepMode = Prefs.TimeStepMode.Seconds; Prefs.frameRate = 1000; });
                menu.AddItem(new GUIContent("0.01"), false, () => { Prefs.timeStepMode = Prefs.TimeStepMode.Seconds; Prefs.frameRate = 100; });
                menu.AddItem(new GUIContent("0.1"), false, () => { Prefs.timeStepMode = Prefs.TimeStepMode.Seconds; Prefs.frameRate = 10; });
                menu.AddItem(new GUIContent("30 FPS"), false, () => { Prefs.timeStepMode = Prefs.TimeStepMode.Frames; Prefs.frameRate = 30; });
                menu.AddItem(new GUIContent("60 FPS"), false, () => { Prefs.timeStepMode = Prefs.TimeStepMode.Frames; Prefs.frameRate = 60; });
                menu.ShowAsContext();
            }

            GUILayout.Space(10);

            GUI.color = Color.white.WithAlpha(0.3f);
            GUILayout.Label(string.Format("<size=9>Version {0}</size>", Cutscene.VERSION_NUMBER.ToString("0.00")));
            GUI.color = Color.white;

            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;

            GUI.enabled = true;
            GUI.contentColor = Color.white;
        }

        void DoScrubControls()
        {
            if (cutscene.isActive)
            { //no scrubbing if playing in runtime
                return;
            }

            ///
            var e = Event.current;
            if (e.type == EventType.MouseDown && topMiddleRect.Contains(mousePosition))
            {
                var carretPos = TimeToPos(length) + leftRect.width;
                var isEndCarret = Mathf.Abs(mousePosition.x - carretPos) < 10 || e.control;
                if (isEndCarret) { CacheMagnetSnapTimes(); }

                if (e.button == 0)
                {
                    isMovingEndCarret = isEndCarret;
                    isMovingScrubCarret = !isMovingEndCarret;
                    //Pause();
                }

                if (e.button == 1 && isEndCarret && cutscene.directables != null)
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Set To Last Clip Time"), false, () =>
                    {
                        var lastClip = cutscene.directables.Where(d => d is ActionClip).OrderBy(d => d.endTime).LastOrDefault();
                        if (lastClip != null)
                        {
                            length = lastClip.endTime;
                        }
                    });
                    menu.ShowAsContext();
                }

                e.Use();
            }

            if (e.button == 0 && e.rawType == EventType.MouseUp)
            {
                isMovingScrubCarret = false;
                isMovingEndCarret = false;
            }

            var pointerTime = PosToTime(mousePosition.x);
            if (isMovingScrubCarret)
            {
                cutscene.currentTime = SnapTime(pointerTime);
                cutscene.currentTime = Mathf.Clamp(cutscene.currentTime, Mathf.Max(viewTimeMin, 0) + float.Epsilon, viewTimeMax - float.Epsilon);
            }

            if (isMovingEndCarret)
            {
                length = SnapTime(pointerTime);
                var magnetSnap = MagnetSnapTime(length, magnetSnapTimesCache);
                length = magnetSnap != null ? magnetSnap.Value : length;
                length = Mathf.Clamp(length, viewTimeMin + float.Epsilon, viewTimeMax - float.Epsilon);
            }
        }

        //Cache an array of snap times for clip (clip times are excluded)
        //Saved in property .magnetSnapTimesCache
        void CacheMagnetSnapTimes(ActionClip clip = null)
        {
            var result = new List<float>();
            result.Add(0);
            result.Add(length);
            result.Add(cutscene.currentTime);
            //if (cutscene.directorGroup != null)
            //{
            //    result.AddRange(cutscene.directorGroup.sections.Select(s => s.time));
            //}
            //foreach (var cw in clipWrappers)
            //{
            //    var action = cw.Value.action;
            //    //exclude the target clip and only include the same group
            //    if (clip == null || (action != clip && action.parent.parent == clip.parent.parent))
            //    {
            //        result.Add(action.startTime);
            //        result.Add(action.endTime);
            //    }
            //}
            magnetSnapTimesCache = result.Distinct().ToArray();
        }

        //Find best snap time (closest)
        float? MagnetSnapTime(float time, float[] snapTimes)
        {
            if (snapTimes == null) { return null; }
            var bestDistance = float.PositiveInfinity;
            var bestTime = float.PositiveInfinity;
            for (var i = 0; i < snapTimes.Length; i++)
            {
                var snapTime = snapTimes[i];
                var distance = Mathf.Abs(snapTime - time);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestTime = snapTime;
                }
            }
            if (Mathf.Abs(bestTime - time) <= MAGNET_SNAP_INTERVAL)
            {
                return bestTime;
            }
            return null;
        }

        //middle - the actual timeline tracks
        void ShowTimeLines(Rect centerRect)
        {
            var e = Event.current;

            //bg graphic
            var bgRect = Rect.MinMaxRect(centerRect.xMin, TOP_MARGIN + TOOLBAR_HEIGHT + scrollPos.y, centerRect.xMax, screenHeight - TOOLBAR_HEIGHT + scrollPos.y);
            GUI.color = Color.black.WithAlpha(0.1f);
            GUI.DrawTexture(bgRect, Styles.whiteTexture);
            GUI.color = Color.black.WithAlpha(0.03f);
            GUI.DrawTextureWithTexCoords(bgRect, Styles.stripes, new Rect(0, 0, bgRect.width / -7, bgRect.height / -7));
            GUI.color = Color.white;

            // draw guides based on time info stored
            for (var _i = timeInfoStart; _i <= timeInfoEnd; _i += timeInfoInterval)
            {
                var i = Mathf.Round(_i * 10) / 10;
                DrawGuideLine(i, Color.black.WithAlpha(0.1f));
                if (i % timeInfoHighMod == 0)
                {
                    DrawGuideLine(i, Color.black.WithAlpha(0.1f));
                }
            }
        }

        void DrawGuides()
        {
            //draw a vertical line at 0 time
            DrawGuideLine(0,Color.white);

            //draw a vertical line at length time
            DrawGuideLine(length,Color.white);

            //draw a vertical line at current time
            if (cutscene.currentTime > 0)
            {
                DrawGuideLine(cutscene.currentTime, Color.yellow);
            }

        }



        void DrawGuideLine(float time,Color color)
        {
            if (time >= viewTimeMin && time <= viewTimeMax)
            {
                var xPos = TimeToPos(time);
                var guideRect = new Rect(xPos + centerRect.x - 1, centerRect.y, 2, centerRect.height);

                // 时间线
                GUI.color = color;
                GUI.DrawTexture(guideRect, Styles.whiteTexture);
                GUI.color = Color.white;
            }
        }

        //Add a cursor type at rect
        void AddCursorRect(Rect rect, MouseCursor type)
        {
            EditorGUIUtility.AddCursorRect(rect, type);
            willRepaint = true;
        }

        //Convert time to position
        float TimeToPos(float time)
        {
            return (time - viewTimeMin) / viewTime * centerRect.width;
        }

        //Convert position to time
        float PosToTime(float pos)
        {
            return (pos - LEFT_MARGIN) / centerRect.width * viewTime + viewTimeMin;
        }

        //Round time to nearest working snap interval
        float SnapTime(float time)
        {
            //holding control for precision (ignore snap intervals)
            if (Event.current.control) { return time; }
            return (Mathf.Round(time / Prefs.snapInterval) * Prefs.snapInterval);
        }

        //动画长度
        public float length
        {
            get { return cutscene.length; }
            set { cutscene.length = value; }
        }

        //最小查看时间
        public float viewTimeMin
        {
            get { return cutscene.viewTimeMin; }
            set { cutscene.viewTimeMin = value; }
        }

        //最大查看时间
        public float viewTimeMax
        {
            get { return cutscene.viewTimeMax; }
            set { cutscene.viewTimeMax = value; }
        }

        //当前查看的最长时间
        public float maxTime
        {
            get { return Mathf.Max(viewTimeMax, length); }
        }

        //当前查看时间的“长度”
        public float viewTime
        {
            get { return viewTimeMax - viewTimeMin; }
        }

        //Screen Width. Hanldes retina.
        private static float screenWidth
        {
            get { return Screen.width / EditorGUIUtility.pixelsPerPoint; }
        }

        //Screen Height. Hanldes retina.
        private static float screenHeight
        {
            get { return Screen.height / EditorGUIUtility.pixelsPerPoint; }
        }

        ///----------------------------------------------------------------------------------------------


        //A wrapper of an ActionClip placed in cutscene
        //class ActionClipWrapper
        //{

        //    const float CLIP_DOPESHEET_HEIGHT = 13f;
        //    const float SCALE_RECT_WIDTH = 5;

        //    public ActionClip action;
        //    public bool isDragging;
        //    public bool isScalingStart;
        //    public bool isScalingEnd;
        //    public bool isControlingBlendIn;
        //    public bool isControlingBlendOut;

        //    public Dictionary<AnimationCurve, Keyframe[]> preScaleKeys;
        //    public float preScaleStartTime;
        //    public float preScaleEndTime;
        //    public float preScaleSubclipOffset;
        //    public float preScaleSubclipSpeed;

        //    public ActionClip previousClip;
        //    public ActionClip nextClip;

        //    private Event e;
        //    private int windowID;
        //    private bool isWaitingMouseDrag;
        //    private float overlapIn;
        //    private float overlapOut;
        //    private float blendInPosX;
        //    private float blendOutPosX;
        //    private bool hasActiveParameters;
        //    private bool hasParameters;
        //    private float pointerTime;
        //    private float snapedPointerTime;
        //    private bool allowScale;

        //    private Rect dragRect;
        //    private Rect controlRectIn;
        //    private Rect controlRectOut;

        //    private CutsceneEditor editor
        //    {
        //        get { return CutsceneEditor.current; }
        //    }

        //    private List<ActionClipWrapper> multiSelection
        //    {
        //        get { return editor.multiSelection; }
        //        set { editor.multiSelection = value; }
        //    }

        //    private Rect _rect;
        //    public Rect rect
        //    {
        //        get { return action.isCollapsed ? default(Rect) : _rect; }
        //        set { _rect = value; }
        //    }

        //    public ActionClipWrapper(ActionClip action)
        //    {
        //        this.action = action;
        //    }

        //    public void ResetInteraction()
        //    {
        //        isWaitingMouseDrag = false;
        //        isDragging = false;
        //        isControlingBlendIn = false;
        //        isControlingBlendOut = false;
        //        isScalingStart = false;
        //        isScalingEnd = false;
        //    }

        //    public void OnClipGUI(int windowID)
        //    {
        //        this.windowID = windowID;
        //        e = Event.current;

        //        overlapIn = previousClip != null ? Mathf.Max(previousClip.endTime - action.startTime, 0) : 0;
        //        overlapOut = nextClip != null ? Mathf.Max(action.endTime - nextClip.startTime, 0) : 0;
        //        blendInPosX = (action.blendIn / action.length) * rect.width;
        //        blendOutPosX = ((action.length - action.blendOut) / action.length) * rect.width;
        //        hasParameters = action.hasParameters;
        //        hasActiveParameters = action.hasActiveParameters;

        //        pointerTime = editor.PosToTime(editor.mousePosition.x);
        //        snapedPointerTime = editor.SnapTime(pointerTime);

        //        allowScale = action.CanScale() && action.length > 0 && rect.width > SCALE_RECT_WIDTH * 2;
        //        dragRect = new Rect(0, 0, rect.width, rect.height - (hasActiveParameters ? CLIP_DOPESHEET_HEIGHT : 0)).ExpandBy(allowScale ? -SCALE_RECT_WIDTH : 0, 0);
        //        controlRectIn = new Rect(0, 0, SCALE_RECT_WIDTH, rect.height - (hasActiveParameters ? CLIP_DOPESHEET_HEIGHT : 0));
        //        controlRectOut = new Rect(rect.width - SCALE_RECT_WIDTH, 0, SCALE_RECT_WIDTH, rect.height - (hasActiveParameters ? CLIP_DOPESHEET_HEIGHT : 0));

        //        editor.AddCursorRect(dragRect, MouseCursor.Link);
        //        if (allowScale)
        //        {
        //            editor.AddCursorRect(controlRectIn, MouseCursor.ResizeHorizontal);
        //            editor.AddCursorRect(controlRectOut, MouseCursor.ResizeHorizontal);
        //        }

        //        //...
        //        var wholeRect = new Rect(0, 0, rect.width, rect.height);
        //        if (action.isLocked && e.isMouse && wholeRect.Contains(e.mousePosition)) { e.Use(); }
        //        action.ShowClipGUI(wholeRect);
        //        if (hasActiveParameters && action.length > 0)
        //        {
        //            ShowClipDopesheet(wholeRect);
        //        }
        //        //...


        //        //set crossblend overlap properties. Do this when no clip is interacting or no clip is dragging
        //        //this way avoid issue when moving clip on the other side of another, but keep overlap interactive when scaling a clip at least.
        //        if (editor.interactingClip == null || !editor.interactingClip.isDragging)
        //        {
        //            var overlap = previousClip != null ? Mathf.Max(previousClip.endTime - action.startTime, 0) : 0;
        //            if (overlap > 0)
        //            {
        //                action.blendIn = overlap;
        //                previousClip.blendOut = overlap;
        //            }
        //        }


        //        if (e.type == EventType.MouseDown)
        //        {

        //            if (e.button == 0)
        //            {
        //                if (dragRect.Contains(e.mousePosition))
        //                {
        //                    isWaitingMouseDrag = true;
        //                }
        //                editor.interactingClip = this;
        //                editor.CacheMagnetSnapTimes(action);
        //            }

        //            if (e.control && dragRect.Contains(e.mousePosition))
        //            {
        //                if (multiSelection == null)
        //                {
        //                    multiSelection = new List<ActionClipWrapper>() { this };
        //                }
        //                if (multiSelection.Contains(this))
        //                {
        //                    multiSelection.Remove(this);
        //                }
        //                else
        //                {
        //                    multiSelection.Add(this);
        //                }
        //            }
        //            else
        //            {
        //                CutsceneUtility.selectedObject = action;
        //                if (multiSelection != null && !multiSelection.Select(cw => cw.action).Contains(action))
        //                {
        //                    multiSelection = null;
        //                }
        //            }

        //            if (e.clickCount == 2)
        //            {
        //                //do this with reflection to get the declaring actor in case action has 'new' declaration. This is only done in Shot right now.
        //                Selection.activeObject = action.GetType().GetProperty("actor").GetValue(action, null) as Object;
        //            }
        //        }

        //        if (e.type == EventType.MouseDrag && isWaitingMouseDrag)
        //        {
        //            isDragging = true;
        //        }

        //        if (e.rawType == EventType.ContextClick)
        //        {
        //            DoClipContextMenu();
        //        }


        //        DrawBlendGraphics();
        //        DoEdgeControls();


        //        if (e.rawType == EventType.MouseUp)
        //        {
        //            if (editor.interactingClip != null)
        //            {
        //                editor.interactingClip.EndClipAdjust();
        //                editor.interactingClip.ResetInteraction();
        //                editor.interactingClip = null;
        //            }
        //        }

        //        if (e.button == 0)
        //        {
        //            GUI.DragWindow(dragRect);
        //        }

        //        //Draw info text if big enough
        //        if (rect.width > 20)
        //        {
        //            var r = new Rect(0, 0, rect.width, rect.height);
        //            if (overlapIn > 0) { r.xMin = blendInPosX; }
        //            if (overlapOut > 0) { r.xMax = blendOutPosX; }
        //            var label = string.Format("<size=10>{0}</size>", action.info);
        //            GUI.color = Color.black;
        //            GUI.Label(r, label);
        //            GUI.color = Color.white;
        //        }
        //    }

        //    //blend graphics
        //    void DrawBlendGraphics()
        //    {
        //        if (action.blendIn > 0)
        //        {
        //            Handles.color = Color.black.WithAlpha(0.5f);
        //            Handles.DrawAAPolyLine(2, new Vector2(0, rect.height), new Vector2(blendInPosX, 0));
        //            Handles.color = Color.black.WithAlpha(0.3f);
        //            Handles.DrawAAConvexPolygon(new Vector3(0, 0), new Vector3(0, rect.height), new Vector3(blendInPosX, 0));
        //        }

        //        if (action.blendOut > 0 && overlapOut == 0)
        //        {
        //            Handles.color = Color.black.WithAlpha(0.5f);
        //            Handles.DrawAAPolyLine(2, new Vector2(blendOutPosX, 0), new Vector2(rect.width, rect.height));
        //            Handles.color = Color.black.WithAlpha(0.3f);
        //            Handles.DrawAAConvexPolygon(new Vector3(rect.width, 0), new Vector2(blendOutPosX, 0), new Vector2(rect.width, rect.height));
        //        }

        //        if (overlapIn > 0)
        //        {
        //            Handles.color = Color.black;
        //            Handles.DrawAAPolyLine(2, new Vector2(blendInPosX, 0), new Vector2(blendInPosX, rect.height));
        //        }
        //        Handles.color = Color.white;
        //    }

        //    //clip scale/blend in/out controls
        //    void DoEdgeControls()
        //    {

        //        var canBlendIn = action.CanBlendIn() && action.length > 0;
        //        var canBlendOut = action.CanBlendOut() && action.length > 0;
        //        if (!isScalingStart && !isScalingEnd && !isControlingBlendIn && !isControlingBlendOut)
        //        {
        //            if (allowScale || canBlendIn)
        //            {
        //                if (controlRectIn.Contains(e.mousePosition))
        //                {
        //                    GUI.BringWindowToFront(windowID);
        //                    GUI.DrawTexture(controlRectIn.ExpandBy(0, -2), whiteTexture);
        //                    if (e.type == EventType.MouseDown && e.button == 0)
        //                    {
        //                        if (allowScale && !e.control) { isScalingStart = true; }
        //                        if (canBlendIn && e.control) { isControlingBlendIn = true; }
        //                        BeginClipAdjust();
        //                        e.Use();
        //                    }
        //                }
        //            }

        //            if (allowScale || canBlendOut)
        //            {
        //                if (controlRectOut.Contains(e.mousePosition))
        //                {
        //                    GUI.BringWindowToFront(windowID);
        //                    GUI.DrawTexture(controlRectOut.ExpandBy(0, -2), whiteTexture);
        //                    if (e.type == EventType.MouseDown && e.button == 0)
        //                    {
        //                        if (allowScale && !e.control) { isScalingEnd = true; }
        //                        if (canBlendOut && e.control) { isControlingBlendOut = true; }
        //                        BeginClipAdjust();
        //                        e.Use();
        //                    }
        //                }
        //            }
        //        }

        //        if (isControlingBlendIn) { action.blendIn = Mathf.Clamp(pointerTime - action.startTime, 0, action.length - action.blendOut); }
        //        if (isControlingBlendOut) { action.blendOut = Mathf.Clamp(action.endTime - pointerTime, 0, action.length - action.blendIn); }

        //        if (isScalingStart)
        //        {
        //            var prevTime = previousClip != null ? previousClip.endTime : 0;
        //            //magnet snap
        //            if (Prefs.magnetSnapping && !e.control)
        //            {
        //                var snapStart = editor.MagnetSnapTime(snapedPointerTime, editor.magnetSnapTimesCache);
        //                if (snapStart != null)
        //                {
        //                    snapedPointerTime = snapStart.Value;
        //                    editor.pendingGuides.Add(new GuideLine(snapedPointerTime, Color.white));
        //                }
        //            }

        //            if (action.CanCrossBlend(previousClip)) { prevTime -= Mathf.Min(action.length / 2, previousClip.length / 2); }

        //            action.startTime = snapedPointerTime;
        //            action.startTime = Mathf.Clamp(action.startTime, prevTime, preScaleEndTime);
        //            action.endTime = preScaleEndTime;

        //            UpdateClipAdjustContents();
        //        }

        //        if (isScalingEnd)
        //        {
        //            var nextTime = nextClip != null ? nextClip.startTime : editor.maxTime;
        //            //magnet snap
        //            if (Prefs.magnetSnapping && !e.control)
        //            {
        //                var snapEnd = editor.MagnetSnapTime(snapedPointerTime, editor.magnetSnapTimesCache);
        //                if (snapEnd != null)
        //                {
        //                    snapedPointerTime = snapEnd.Value;
        //                    editor.pendingGuides.Add(new GuideLine(snapedPointerTime, Color.white));
        //                }
        //            }

        //            if (action.CanCrossBlend(nextClip)) { nextTime += Mathf.Min(action.length / 2, nextClip.length / 2); }

        //            action.endTime = snapedPointerTime;
        //            action.endTime = Mathf.Clamp(action.endTime, 0, nextTime);

        //            UpdateClipAdjustContents();
        //        }
        //    }


        //    //store pre adjust values
        //    public void BeginClipAdjust()
        //    {
        //        preScaleStartTime = action.startTime;
        //        preScaleEndTime = action.endTime;
        //        preScaleKeys = action.GetCurvesAll().ToDictionary(k => k, k => k.keys);
        //        if (action is ISubClipContainable)
        //        {
        //            preScaleSubclipOffset = (action as ISubClipContainable).subClipOffset;
        //            preScaleSubclipSpeed = (action as ISubClipContainable).subClipSpeed;
        //        }
        //        editor.CacheMagnetSnapTimes(action);
        //    }

        //    //retime keys lerp between start/end time.
        //    public void UpdateClipAdjustContents()
        //    {

        //        if (preScaleKeys == null) { return; }

        //        var retime = Event.current.control;
        //        var trim = !Event.current.shift && !retime;

        //        foreach (var curve in action.GetCurvesAll())
        //        {
        //            for (var i = 0; i < curve.keys.Length; i++)
        //            {
        //                var preKey = preScaleKeys[curve][i];

        //                if (retime)
        //                {
        //                    var preLength = preScaleEndTime - preScaleStartTime;
        //                    var newTime = Mathf.LerpUnclamped(0, action.length, preKey.time / preLength);
        //                    preKey.time = newTime;
        //                }

        //                if (trim)
        //                {
        //                    preKey.time -= action.startTime - preScaleStartTime;
        //                }

        //                curve.MoveKey(i, preKey);
        //            }

        //            curve.UpdateTangentsFromMode();
        //        }

        //        CutsceneUtility.RefreshAllAnimationEditorsOf(action.animationData);

        //        if (action is ISubClipContainable)
        //        {
        //            if (trim)
        //            {
        //                var subClip = (ISubClipContainable)action;
        //                var delta = preScaleStartTime - action.startTime;
        //                var newOffset = preScaleSubclipOffset + delta;
        //                subClip.subClipOffset = newOffset;
        //            }
        //        }
        //    }

        //    //flush pre adjust values
        //    public void EndClipAdjust()
        //    {
        //        preScaleKeys = null;
        //        if (Prefs.autoCleanKeysOffRange)
        //        {
        //            CleanKeysOffRange();
        //        }
        //    }



        //    ///Split the clip in two, at specified local time
        //    public ActionClip Split(float time)
        //    {

        //        if (!action.IsTimeWithinClip(time))
        //        {
        //            return null;
        //        }

        //        if (hasParameters)
        //        {
        //            foreach (var param in action.animationData.animatedParameters)
        //            {
        //                if (param.HasAnyKey()) { param.TryKeyIdentity(action.ToLocalTime(time)); }
        //            }
        //        }

        //        CutsceneUtility.CopyClip(action);
        //        var copy = CutsceneUtility.PasteClip((CutsceneTrack)action.parent, time);
        //        copy.startTime = time;
        //        copy.endTime = action.endTime;
        //        action.endTime = time;
        //        copy.blendIn = 0;
        //        action.blendOut = 0;
        //        CutsceneUtility.selectedObject = null;
        //        CutsceneUtility.FlushCopy();

        //        var delta = action.length;
        //        if (hasParameters)
        //        {
        //            foreach (var curve in copy.GetCurvesAll())
        //            {
        //                curve.OffsetCurveTime(-delta);
        //                curve.RemoveNegativeKeys();
        //            }
        //            CutsceneUtility.RefreshAllAnimationEditorsOf(action.animationData);
        //        }

        //        if (copy is ISubClipContainable)
        //        {
        //            (copy as ISubClipContainable).subClipOffset -= delta;
        //        }

        //        return copy;
        //    }

        //    ///Scale clip to fit previous and next
        //    public void StretchFit()
        //    {
        //        var wasStartTime = action.startTime;
        //        var wasEndTime = action.endTime;
        //        var targetStart = previousClip != null ? previousClip.endTime : action.parent.startTime;
        //        var targetEnd = nextClip != null ? nextClip.startTime : action.parent.endTime;
        //        if (previousClip == null || previousClip.endTime < action.startTime)
        //        {
        //            action.startTime = targetStart;
        //            action.endTime = wasEndTime;
        //        }
        //        if (nextClip == null || nextClip.startTime > action.endTime)
        //        {
        //            action.endTime = targetEnd;
        //        }

        //        var delta = wasStartTime - action.startTime;
        //        if (hasParameters)
        //        {
        //            foreach (var curve in action.GetCurvesAll())
        //            {
        //                curve.OffsetCurveTime(delta);
        //            }
        //            CutsceneUtility.RefreshAllAnimationEditorsOf(action.animationData);
        //        }

        //        if (action is ISubClipContainable)
        //        {
        //            (action as ISubClipContainable).subClipOffset += delta;
        //        }
        //    }

        //    ///Clean keys off clip range after adding a key at 0 and length if there is any key outside that range
        //    public void CleanKeysOffRange()
        //    {
        //        if (hasParameters)
        //        {
        //            foreach (var param in action.animationData.animatedParameters)
        //            {
        //                if (param.HasAnyKey())
        //                {
        //                    if (param.GetKeyPrevious(0) < 0)
        //                    {
        //                        param.TryKeyIdentity(0);
        //                    }
        //                    if (param.GetKeyNext(action.length) > action.length)
        //                    {
        //                        param.TryKeyIdentity(action.length);
        //                    }
        //                }
        //            }
        //            foreach (var curve in action.GetCurvesAll())
        //            {
        //                curve.RemoveKeysOffRange(0, action.length);
        //                curve.UpdateTangentsFromMode();
        //            }
        //            CutsceneUtility.RefreshAllAnimationEditorsOf(action.animationData);
        //        }
        //    }

        //    //Show the clip dopesheet
        //    void ShowClipDopesheet(Rect rect)
        //    {
        //        var dopeRect = new Rect(0, rect.height - CLIP_DOPESHEET_HEIGHT, rect.width, CLIP_DOPESHEET_HEIGHT);
        //        GUI.color = isProSkin ? new Color(0, 0.2f, 0.2f, 0.5f) : new Color(0, 0.8f, 0.8f, 0.5f);
        //        GUI.Box(dopeRect, string.Empty, Slate.Styles.clipBoxHorizontalStyle);
        //        GUI.color = Color.white;
        //        DopeSheetEditor.DrawDopeSheet(action.animationData, action, dopeRect, 0, action.length, false);
        //    }

        //    //CONTEXT
        //    void DoClipContextMenu()
        //    {
        //        var menu = new GenericMenu();
        //        if (multiSelection != null && multiSelection.Contains(this))
        //        {
        //            menu.AddItem(new GUIContent("Delete Clips"), false, () =>
        //            {
        //                editor.SafeDoAction(() =>
        //                {
        //                    foreach (var act in multiSelection.Select(b => b.action).ToArray())
        //                    {
        //                        (act.parent as CutsceneTrack).DeleteAction(act);
        //                    }
        //                    editor.InitClipWrappers();
        //                    multiSelection = null;
        //                });
        //            });

        //            menu.ShowAsContext();
        //            e.Use();
        //            return;
        //        }

        //        menu.AddItem(new GUIContent("Copy Clip"), false, () => { CutsceneUtility.CopyClip(action); });
        //        menu.AddItem(new GUIContent("Cut Clip"), false, () => { CutsceneUtility.CutClip(action); });

        //        if (allowScale)
        //        {
        //            menu.AddItem(new GUIContent("Fit Clip (F)"), false, () => { StretchFit(); });
        //            if (action.length > 0)
        //            {
        //                menu.AddItem(new GUIContent("Split At Cursor"), false, () => { Split(snapedPointerTime); });
        //                menu.AddItem(new GUIContent("Split At Scrubber (S)"), false, () => { Split(editor.cutscene.currentTime); });
        //            }
        //        }

        //        if (hasParameters)
        //        {
        //            menu.AddItem(new GUIContent("Key At Cursor"), false, () => { action.TryAddIdentityKey(action.ToLocalTime(snapedPointerTime)); });
        //            menu.AddItem(new GUIContent("Key At Scrubber (K)"), false, () => { action.TryAddIdentityKey(action.RootTimeToLocalTime()); });
        //        }

        //        menu.AddSeparator("/");

        //        if (hasActiveParameters)
        //        {
        //            menu.AddItem(new GUIContent("Clean Keys Off-Range (C)"), false, () => { CleanKeysOffRange(); });
        //            menu.AddItem(new GUIContent("Remove Animation"), false, () =>
        //            {
        //                if (EditorUtility.DisplayDialog("Remove Animation", "All Animation Curve keys of all animated parameters for this clip will be removed.\nAre you sure?", "Yes", "No"))
        //                {
        //                    editor.SafeDoAction(() => { action.ResetAnimatedParameters(); });
        //                }
        //            });
        //        }

        //        menu.AddItem(new GUIContent("Delete Clip"), false, () =>
        //        {
        //            editor.SafeDoAction(() =>
        //            {
        //                (action.parent as CutsceneTrack).DeleteAction(action);
        //                editor.InitClipWrappers();
        //            });
        //        });

        //        menu.ShowAsContext();
        //        e.Use();
        //    }
        //}
    }
}
