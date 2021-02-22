//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace Akari
{
    /// <summary>
    /// 界面编号。
    /// </summary>
    public enum UIFormId : byte
    {
        Undefined = 0,

        /// <summary>
        /// 主菜单。
        /// </summary>
        UIMainMenu = 100,

        /// <summary>
        /// 设置。
        /// </summary>
        UISetting = 101,

        /// <summary>
        /// 关于。
        /// </summary>
        UIAbout = 102,

        /// <summary>
        /// 加载界面。
        /// </summary>
        UILoading = 103,

        /// <summary>
        /// 弹出框。
        /// </summary>
        UIDialogForm = 104,

        /// <summary>
        /// 主游戏界面。
        /// </summary>
        UIMainGame = 105,
    }
}
