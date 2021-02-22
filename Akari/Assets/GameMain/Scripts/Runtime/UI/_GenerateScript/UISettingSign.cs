// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-02-22 16:42:55.028
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
		
		
		public void InitUIData()
        {

            ReferenceCollector rc = rootGo.GetComponent<ReferenceCollector>();
			
            LanguageDropdown = rc.Get<TMPro.TMP_Dropdown>("LanguageDropdown");
			goMusic = rc.Get<UnityEngine.GameObject>("goMusic");
			goSound = rc.Get<UnityEngine.GameObject>("goSound");
			goUISound = rc.Get<UnityEngine.GameObject>("goUISound");
			
        }
    }
}
