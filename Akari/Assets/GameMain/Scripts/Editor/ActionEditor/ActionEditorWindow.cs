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
    /// 动作编辑器窗口类型
    /// </summary>
    [Flags]
    public enum ViewType
    {
        None = 0,

        GlobalAction = 1,
        State = 2,
        StateSet = 3,
        Action = 4,
        Tool = 5,
        Other = 6,
        Frame = 7,
    }

    /// <summary>
    /// 动作编辑器配置
    /// </summary>
    [Serializable]
    public class ActionEditorSetting
    {
        public int stateSelectIndex = -1;
        public int attackRangeSelectIndex = -1;
        public int bodyRangeSelectIndex = -1;
        public int actionSelectIndex = -1;
        public int globalActionSelectIndex = -1;
        public int frameSelectIndex = -1;
        public bool enableAllControl = false;
        public bool enableQuickKey = false;

        public ViewType showView = ViewType.Action;

        public float frameRate => 0.033f;

        public Vector2 otherViewScrollPos = Vector2.zero;

        public float frameWidth = 40;
        public float frameListViewRectHeight = 200f;
    }


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
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.LogWarning("编辑器不能在运行时打开");
                return;
            }

            var win = EditorWindow.GetWindow<ActionEditorWindow>();

            if (win.configAsset != null)
            {
                if (win.configAsset == config)
                {// 如果当前已打开的窗口相同，则focus,并直接返回
                    win.Focus();
                    return;
                }
                /*else
                {
                    //如果不相同，则创建一个新的窗口
                    win = EditorWindow.CreateWindow<ACActionEditorWindow>();
                    win.Show();
                }*/
            }

            //更新参数
            win.UpdateTarget(target);
            win.UpdateConfig(config);
        }

        [NonSerialized] public readonly ActionListView actionListView;
        [NonSerialized] public readonly ActionSetView actionSetView;
        //[NonSerialized] public readonly MenuView menuView;

        public List<IView> views { get; private set; }

        #region style
        private readonly float space = 3f;
        private readonly float scrollHeight = 13f;
        private readonly float actionListViewRectWidth = 180f;
        private readonly float actionSetViewRectWidth = 300f;
        #endregion style

        #region data

        protected static string settingPath = "Akari.ActionEditorWindow";

        public ActionEditorSetting setting = new ActionEditorSetting();

        public bool actionMachineDirty = false;

        public bool isRunning => EditorApplication.isPlaying;

        public GameObject actionMachineObj = null;
        public ActionMachineTest actionMachine = null;
        public TextAsset configAsset = null;

        [NonSerialized] public MachineConfig config;//SerializeReference 存在bug，先不使用，即无法使用回滚

        public bool isConfigValid => config != null && (isRunning || configAsset != null);
        public bool isActionMachineValid => actionMachine != null;
        //public bool isCurrentAnimationClipValid => null != GetCurrentAnimationClip();

        public int stateSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.stateSelectIndex, currentStates);
                return setting.stateSelectIndex;
            }
            set
            {
                int oldIndex = setting.stateSelectIndex;
                setting.stateSelectIndex = value;
                CheckSelectIndex(ref setting.stateSelectIndex, currentStates);
                if (oldIndex != value && oldIndex != setting.stateSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }

        public int attackRangeSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.attackRangeSelectIndex, currentAttackRanges);
                return setting.attackRangeSelectIndex;
            }
            set
            {
                setting.attackRangeSelectIndex = value;
                CheckSelectIndex(ref setting.attackRangeSelectIndex, currentAttackRanges);
            }
        }

        public int bodyRangeSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.bodyRangeSelectIndex, currentBodyRanges);
                return setting.bodyRangeSelectIndex;
            }
            set
            {
                setting.bodyRangeSelectIndex = value;
                CheckSelectIndex(ref setting.bodyRangeSelectIndex, currentBodyRanges);
            }
        }

        public int actionSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.actionSelectIndex, currentActions);
                return setting.actionSelectIndex;
            }
            set
            {
                setting.actionSelectIndex = value;
                CheckSelectIndex(ref setting.actionSelectIndex, currentActions);
            }
        }

        public int globalActionSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.globalActionSelectIndex, currentGlobalActions);
                return setting.globalActionSelectIndex;
            }
            set
            {
                setting.globalActionSelectIndex = value;
                CheckSelectIndex(ref setting.globalActionSelectIndex, currentGlobalActions);
            }
        }

        public int frameSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.frameSelectIndex, currentFrames);
                return setting.frameSelectIndex;
            }
            set
            {
                int oldIndex = setting.frameSelectIndex;
                setting.frameSelectIndex = value;
                CheckSelectIndex(ref setting.frameSelectIndex, currentFrames);
                if (oldIndex != value && oldIndex != setting.frameSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }

        public List<StateConfig> currentStates => config?.states;
        public List<FrameConfig> currentFrames => currentState?.frames;
        public List<RangeConfig> currentBodyRanges => currentFrame?.bodyRanges;
        public List<RangeConfig> currentAttackRanges => currentFrame?.attackRanges;
        public List<object> currentActions => currentState?.actions;
        public List<object> currentGlobalActions => config?.globalActions;

        public int currentStateCount => currentStates?.Count ?? -1;
        public int currentFrameCount => currentFrames?.Count ?? -1;
        public int currentBodyRangeCount => currentBodyRanges?.Count ?? -1;
        public int currentAttackRangeCount => currentAttackRanges?.Count ?? -1;
        public int currentActionCount => currentActions?.Count ?? -1;
        public int currentGlobalActionCount => currentGlobalActions?.Count ?? -1;

        public StateConfig currentState => GetSelectItem(stateSelectIndex, currentStates);
        public FrameConfig currentFrame => GetSelectItem(frameSelectIndex, currentFrames);
        public RangeConfig currentBodyRange => GetSelectItem(bodyRangeSelectIndex, currentBodyRanges);
        public RangeConfig currentAttackRange => GetSelectItem(attackRangeSelectIndex, currentAttackRanges);
        public object currentAction => GetSelectItem(actionSelectIndex, currentActions);
        public object currentGlobalAction => GetSelectItem(globalActionSelectIndex, currentGlobalActions);


        private int CheckSelectIndex<T>(ref int index, IList<T> list)
        {
            return index = list == null ? -1 : Mathf.Clamp(index, -1, list.Count - 1);
        }

        private T GetSelectItem<T>(int index, IList<T> list) where T : class
        {
            if (index < 0 || null == list || list.Count == 0)
            {
                return null;
            }
            return list[index];
        }

        #endregion


        public ActionEditorWindow()
        {
            views = new List<IView>();

            actionListView = CreateView<ActionListView>();
            actionSetView = CreateView<ActionSetView>();
        }

        private T CreateView<T>() where T : IView, new()
        {
            T obj = new T();
            obj.win = this;
            views.Add(obj);
            return obj;
        }

        private void Awake()
        {

        }

        private void Update()
        {
            foreach (var view in views)
            {
                view.OnUpdate();
            }
        }

        public void OnGUI()
        {

            //EditorGUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
            //{
            //    GUILayout.Space(5f);
            //    if (GUILayout.Button("创建", GUILayout.Width(80)))
            //    {
            //        GUI.FocusControl(null);
            //        CreateNew();
            //    }
            //}
            //EditorGUILayout.EndVertical();

            Check();
            //Undo.RecordObject(this, "ActionEditorWindow");
            Draw();
            UpdateActionMachine();

            EventProcess();

            //quickButtonHandler.OnGUI();

            Repaint();
        }

        private void OnEnable()
        {
            this.titleContent = new GUIContent("动作编辑器");
            //加载配置
            string data = EditorUserSettings.GetConfigValue(settingPath);
            if (!string.IsNullOrEmpty(data))
            {
                EditorJsonUtility.FromJsonOverwrite(data, setting);
            }
            //

            autoRepaintOnSceneChange = true;

            //SceneView.duringSceneGui += OnSceneGUI;
            //EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            //if (config == null && configAsset != null)
            //{
            //    UpdateConfig(configAsset);
            //}
        }

        private void OnDisable()
        {
            //SceneView.duringSceneGui -= OnSceneGUI;
            //EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

            //保存配置
            string data = EditorJsonUtility.ToJson(setting, false);
            EditorUserSettings.SetConfigValue(settingPath, data);
        }

        private void OnDestroy()
        {
            foreach (var view in views)
            {
                view.OnDestroy();
            }
        }

        private void Check()
        {
            if (!isActionMachineValid)
            {
                actionMachineObj = null;
                actionMachine = null;
            }

            if (!isConfigValid)
            {
                configAsset = null;
                config = null;
            }

            //更新标题
            //this.titleContent = new GUIContent(configAsset != null ? $"编辑 {configAsset.name} " : $"动作编辑器");
        }

        private void EventProcess()
        {
            Rect rect = position;

            Event evt = Event.current;
            if (!rect.Contains(Event.current.mousePosition))
            {
                return;
            }
        }

        public void UpdateTarget(GameObject target)
        {
            if (target == null)
            {
                throw new GameFrameworkException("未选择目标。");
            }
            actionMachineObj = target;
            actionMachine = target.GetComponent<ActionMachineTest>();
            if (actionMachine == null)
            {
                actionMachineObj = null;
                throw new GameFrameworkException($"目标不存在{nameof(ActionMachineTest)}脚本");
            }
        }

        public void UpdateConfig(TextAsset config)
        {
            if (config == null)
            {
                throw new GameFrameworkException($"未选择配置资源");
            }

            this.configAsset = config;
            this.config = JsonUtility.FromJson<MachineConfig>(config.text);
            if (this.config == null)
            {
                throw new GameFrameworkException($"配置资源解析失败");
            }
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

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!isConfigValid || actionMachine == null)
            {
                return;
            }

            //Undo.RecordObject(this, "ActionEditorWindow");

            //guiDrawer.OnSceneGUI(sceneView);
            //quickButtonHandler.OnSceneGUI(sceneView);

            sceneView.Repaint();
            Repaint();
        }

        #region Anima
        private void UpdateActionMachine()
        {
            if (!actionMachineDirty || !isActionMachineValid)
            {
                return;
            }
            actionMachineDirty = false;

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            AnimationClip clip = GetCurrentAnimationClip();
            int frameIndex = frameSelectIndex;
            if (clip == null || frameIndex < 0)
            {
                return;
            }

            float time = frameIndex * setting.frameRate;

            var state = currentState;

            Animator animator = GetAnimator();
            clip.SampleAnimation(animator.gameObject, time);
        }

        public Animator GetAnimator()
        {
            if (null == actionMachineObj)
            {
                return null;
            }

            Animator animator = actionMachineObj.GetComponent<Animator>();
            if (null != animator)
            {
                return animator;
            }

            animator = actionMachineObj.GetComponentInChildren<Animator>();

            return animator;
        }

        public AnimationClip GetCurrentAnimationClip()
        {
            Animator animator = GetAnimator();
            var state = currentState;

            if (animator == null || state == null || string.IsNullOrEmpty(state.defaultAnimaName))
            {
                return null;
            }

            return Array.Find(animator.runtimeAnimatorController.animationClips, t => string.Compare(state.defaultAnimaName, t.name) == 0);
        }
        #endregion

        #region Draw view

        private static string copyData;
        private static Type copyDataType;

        public object copyBuffer
        {
            set
            {
                if (value == null)
                {
                    copyDataType = null;
                    copyData = null;
                    return;
                }
                copyDataType = value.GetType();
                // copyData = JsonUtility.ToJson(value, copyDataType);
                copyData = JsonUtility.ToJson(value);
            }
            get
            {
                if (copyDataType == null || copyData == null) { return null; }
                return JsonUtility.FromJson(copyData, copyDataType);
            }
        }

        private void Draw() 
        {
            #region calc size
            Rect rect = this.position;
            rect.position = Vector2.zero;

            float startPosX = rect.x;
            float startPosY = rect.y;
            float height = rect.height;
            float width = rect.width;

            Rect actionListViewRect = Rect.zero;
            Rect actionSetViewRect = Rect.zero;


            float itemHeight = height - scrollHeight;
            float nextPosX = startPosX;
            float nextPosY = startPosY;
            bool hasNextView = false;

            if ((setting.showView & ViewType.Action) != 0)
            {
                if (!actionListView.isPop)
                {
                    actionListViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        actionListViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += actionListViewRectWidth;
                    hasNextView = true;
                }

                if (!actionSetView.isPop)
                {
                    actionSetViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        actionSetViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += actionSetViewRectWidth;
                    hasNextView = true;
                }
            }

            #endregion calc size

            #region draw
            if (hasNextView)
            {
                Rect position = new Rect(startPosX + space, startPosY, width - space, height);
                Rect view = new Rect(startPosX + space, startPosY, nextPosX - startPosX - space, itemHeight);
                setting.otherViewScrollPos = GUI.BeginScrollView(position, setting.otherViewScrollPos, view, true, false);

                if ((setting.showView & ViewType.Action) != 0)
                {
                    if (!actionListView.isPop)
                    {
                        actionListView.Draw(actionListViewRect);
                    }
                    if (!actionSetView.isPop)
                    {
                        actionSetView.Draw(actionSetViewRect);
                    }
                }

                GUI.EndScrollView();
            }
            #endregion draw
        }

        #endregion Draw view
    }


    public class ViewWindow : EditorWindow
    {
        protected IView _view;
        protected ActionEditorWindow _win;
        protected string _viewTypeName;

        public IView view
        {
            get
            {
                if (_view != null && _win != null) { return _view; }

                Type viewType = Type.GetType(_viewTypeName, false);
                if (viewType == null) { return null; }

                if (!HasOpenInstances<ActionEditorWindow>()) { return null; }

                _win = GetWindow<ActionEditorWindow>();
                _view = _win.views.Find(t => t.GetType() == viewType);
                _view.popWindow = this;
                return _view;
            }
            set
            {
                _view = value;
                _win = value.win;
                _viewTypeName = value.GetType().FullName + "," + value.GetType().Assembly.FullName;
                _view.popWindow = this;
            }
        }

        public static ViewWindow Show(IView view, Rect rect)
        {
            var win = EditorWindow.CreateWindow<ViewWindow>(view.title);
            win.position = rect;
            win.view = view;
            win.Show();
            return win;
        }

        private void OnEnable()
        {
            autoRepaintOnSceneChange = true;
        }

        private void OnDisable()
        {
        }

        private void OnDestroy()
        {
            view?.OnPopDestroy();
        }

        private void OnGUI()
        {
            if (view == null)
            {
                return;
            }

            Rect contentRect = new Rect(Vector2.zero, this.position.size);
            view.Draw(contentRect);

            Repaint();
        }
    }
}
