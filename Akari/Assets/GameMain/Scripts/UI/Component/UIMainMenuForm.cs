namespace Akari
{
    public class UIMainMenuForm: UIMainMenuFormSign
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitUIData();

            btnStart.onClick.AddListener(OnStartGame);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        #region Event
        private void OnStartGame()
        {
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Game")));
        }
        #endregion
    }
}