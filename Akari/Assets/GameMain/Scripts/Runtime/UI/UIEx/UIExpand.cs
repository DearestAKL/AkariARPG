using GameFramework;
using TMPro;
using UnityEngine.UI;

namespace Akari
{
    public class SliderEx
    {
        private TextMeshProUGUI txtValue;
        private Slider slider;

        private string SettingName;
        public SliderEx(ReferenceCollector rc,float value = 0)
        {
            txtValue = rc.Get<TextMeshProUGUI>("txtValue");
            slider = rc.Get<Slider>("slider");
        }

        /// <summary>
        /// 添加 全局音量 控制
        /// </summary>
        /// <param name="settingName"></param>
        public void SetVolumeAction(string settingName)
        {
            SettingName = settingName;

            var value = GameEntry.Setting.GetFloat(SettingName) * 100;
            slider.value = value;
            txtValue.text = Utility.Text.Format("{0}%", value);

            slider.onValueChanged.AddListener(OnVolumeChanged);
        }

        private void OnVolumeChanged(float value)
        {
            txtValue.text = Utility.Text.Format("{0}%", value);
            GameEntry.Setting.SetFloat(SettingName, value / 100);

            if(SettingName == Constant.Setting.MusicVolume)
            {
                GameEntry.Sound.SetVolume("Music", GameEntry.Setting.GetFloat(Constant.Setting.MusicVolume, 0.3f));
            }
            else if (SettingName == Constant.Setting.MusicVolume)
            {
                GameEntry.Sound.SetVolume("Sound", GameEntry.Setting.GetFloat(Constant.Setting.SoundVolume, 1f));
            }
            else
            {
                GameEntry.Sound.SetVolume("UISound", GameEntry.Setting.GetFloat(Constant.Setting.UISoundVolume, 1f));
            }
        }
    }
}
