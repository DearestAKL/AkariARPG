using GameFramework;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using System.Collections;

namespace Akari
{
    public class SliderEx
    {
        private TextMeshProUGUI txtValue;
        private Slider slider;

        private string SettingName;
        private string SoundGroupName;
        public SliderEx(ReferenceCollector rc)
        {
            txtValue = rc.Get<TextMeshProUGUI>("txtValue");
            slider = rc.Get<Slider>("slider");
        }

        /// <summary>
        /// 添加 全局音量 控制
        /// </summary>
        /// <param name="settingName"></param>
        public void SetVolumeAction(string settingName,string soundGroupName)
        {
            SettingName = settingName;
            SoundGroupName = soundGroupName;

            var value = GameEntry.Setting.GetFloat(SettingName) * 100;
            slider.value = value;
            txtValue.text = Utility.Text.Format("{0}%", value);

            slider.onValueChanged.AddListener(OnVolumeChanged);
        }

        private void OnVolumeChanged(float value)
        {
            txtValue.text = Utility.Text.Format("{0}%", value);
            GameEntry.Setting.SetFloat(SettingName, value / 100);
            GameEntry.Sound.SetVolume(SoundGroupName, GameEntry.Setting.GetFloat(Constant.Setting.MusicVolume));
        }
    }

    public class TopLeftInfoEx
    {
        private TextMeshProUGUI txtTitle;
        private Image imgTitle;

        public TopLeftInfoEx(ReferenceCollector rc)
        {
            txtTitle = rc.Get<TextMeshProUGUI>("txtTitle");
            imgTitle = rc.Get<Image>("imgTitle");
        }

        public void UpdateTitle(string name, string imgName)
        {
            imgTitle.sprite = GameEntry.AtlasHelper.GetCommonIcon(imgName);
            txtTitle.text = name;
        }

        public void UpdateTitle(string name)
        {
            txtTitle.text = name;
        }
    }

    public class InfoBarEx
    {
        private TextMeshProUGUI txtProgress;
        private Image imgProgress;
        private Image imgTransition;

        private int m_CurValue;
        private int m_MaxValue;

        public InfoBarEx(ReferenceCollector rc)
        {
            txtProgress = rc.Get<TextMeshProUGUI>("txtProgress");
            imgProgress = rc.Get<Image>("imgProgress");
            imgTransition = rc.Get<Image>("imgTransition");
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="curValue"></param>
        /// <param name="maxValue"></param>
        public void InitData(int curValue,int maxValue)
        {
            m_CurValue = curValue;
            m_MaxValue = maxValue;

            float percentage = (float)m_CurValue / m_MaxValue;
            imgProgress.fillAmount = percentage;
            imgTransition.fillAmount = percentage;

            UpdateText();
        }

        /// <summary>
        /// 更新文本
        /// </summary>
        private void UpdateText()
        {
            txtProgress.text = Utility.Text.Format("{0}/{1}", m_CurValue, m_MaxValue);
        }

        /// <summary>
        /// 减 过渡
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public IEnumerator MinusTransition(float fromHPRatio,float toHPRatio, float duration)
        {
            m_CurValue = (int)(toHPRatio * m_MaxValue);
            UpdateText();

            imgProgress.fillAmount = toHPRatio;

            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                imgTransition.fillAmount = Mathf.Lerp(fromHPRatio, toHPRatio, time / duration);
                yield return new WaitForEndOfFrame();
            }

            imgTransition.fillAmount = toHPRatio;
        }

        /// <summary>
        /// 加 过渡
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public IEnumerator AddTransition(float fromHPRatio, float toHPRatio, float duration)
        {
            m_CurValue = (int)(toHPRatio * m_MaxValue);
            UpdateText();

            imgTransition.fillAmount = toHPRatio;

            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                imgProgress.fillAmount = Mathf.Lerp(fromHPRatio, toHPRatio, time / duration);
                yield return new WaitForEndOfFrame();
            }

            imgProgress.fillAmount = toHPRatio;
        }
    }
}
