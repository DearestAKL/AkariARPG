using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class UISetting : UISettingSign
    {
        private TopLeftInfoEx m_TopLeftInfo;

        private SliderEx m_MusicSlider;
        private SliderEx m_SoundSlider;
        private SliderEx m_UISoundSlider;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitUIData();

            //设置顶部信息
            m_TopLeftInfo = new TopLeftInfoEx(goTopLeftInfo.GetComponent<ReferenceCollector>());
            m_TopLeftInfo.UpdateTitle(GameEntry.Localization.GetRawString("Setting"));
            btnClose.onClick.Add(OnClose);

            //设置语言选项
            List<string> languages = new List<string>();
            languages.Add(GameEntry.Localization.GetRawString("ChineseSimplified"));
            languages.Add(GameEntry.Localization.GetRawString("ChineseTraditional"));
            languages.Add(GameEntry.Localization.GetRawString("English"));
            LanguageDropdown.AddOptions(languages);

            //设置声音
            m_MusicSlider = new SliderEx(goMusic.GetComponent<ReferenceCollector>());
            m_SoundSlider = new SliderEx(goSound.GetComponent<ReferenceCollector>());
            m_UISoundSlider = new SliderEx(goUISound.GetComponent<ReferenceCollector>());
            m_MusicSlider.SetVolumeAction(Constant.Setting.MusicVolume, "Music");
            m_SoundSlider.SetVolumeAction(Constant.Setting.SoundVolume, "Sound");
            m_UISoundSlider.SetVolumeAction(Constant.Setting.UISoundVolume, "UISound");

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        #region Event
        private void OnClose()
        {
            Close();
        }
        #endregion
    }
}

