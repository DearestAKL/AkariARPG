using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class UISetting : UISettingSign
    {
        private SliderEx m_MusicSlider;
        private SliderEx m_SoundSlider;
        private SliderEx m_UISoundSlider;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitUIData();

            m_MusicSlider = new SliderEx(goMusic.GetComponent<ReferenceCollector>());
            m_SoundSlider = new SliderEx(goSound.GetComponent<ReferenceCollector>());
            m_UISoundSlider = new SliderEx(goUISound.GetComponent<ReferenceCollector>());

            m_MusicSlider.SetVolumeAction(Constant.Setting.MusicVolume);
            m_SoundSlider.SetVolumeAction(Constant.Setting.SoundVolume);
            m_UISoundSlider.SetVolumeAction(Constant.Setting.UISoundVolume);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }
    }
}

