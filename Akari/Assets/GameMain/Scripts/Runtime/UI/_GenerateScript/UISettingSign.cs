// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-02-28 06:05:59.779
//------------------------------------------------------------

namespace Akari
{
    public class UISettingSign : UGuiForm
    {
		//---UI---
		protected TMPro.TMP_Dropdown LanguageDropdown = null;
		protected UnityEngine.GameObject goMusic = null;
		protected UnityEngine.GameObject goSound = null;
		protected UnityEngine.GameObject goUISound = null;
		protected UnityEngine.GameObject goTopLeftInfo = null;
		protected UnityEngine.UI.Button btnClose = null;
		
		
		public void InitUIData()
        {

            ReferenceCollector rc = rootGo.GetComponent<ReferenceCollector>();
			
            LanguageDropdown = rc.Get<TMPro.TMP_Dropdown>("LanguageDropdown");
			goMusic = rc.Get<UnityEngine.GameObject>("goMusic");
			goSound = rc.Get<UnityEngine.GameObject>("goSound");
			goUISound = rc.Get<UnityEngine.GameObject>("goUISound");
			goTopLeftInfo = rc.Get<UnityEngine.GameObject>("goTopLeftInfo");
			btnClose = rc.Get<UnityEngine.UI.Button>("btnClose");
			
        }
    }
}
