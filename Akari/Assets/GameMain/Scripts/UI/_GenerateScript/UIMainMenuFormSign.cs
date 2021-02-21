// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-12-23 23:20:35.656
//------------------------------------------------------------

namespace Akari
{
    public class UIMainMenuFormSign : UGuiFormEx
    {
		//---UI---
		public UnityEngine.UI.Button btnAbout = null;
		public UnityEngine.UI.Button btnSetting = null;
		public UnityEngine.UI.Button btnContinue = null;
		public UnityEngine.UI.Button btnStart = null;
		
		
		public void InitUIData()
        {

            ReferenceCollector rc = rootGo.GetComponent<ReferenceCollector>();
			
            btnAbout = rc.Get<UnityEngine.UI.Button>("btnAbout");
			btnSetting = rc.Get<UnityEngine.UI.Button>("btnSetting");
			btnContinue = rc.Get<UnityEngine.UI.Button>("btnContinue");
			btnStart = rc.Get<UnityEngine.UI.Button>("btnStart");
			
        }
    }
}
