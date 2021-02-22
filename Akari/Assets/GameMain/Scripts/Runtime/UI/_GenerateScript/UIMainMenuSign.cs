// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-02-22 10:47:20.021
//------------------------------------------------------------

namespace Akari
{
    public class UIMainMenuSign : UGuiForm
    {
		//---UI---
		protected UnityEngine.UI.Button btnAbout = null;
		protected UnityEngine.UI.Button btnSetting = null;
		protected UnityEngine.UI.Button btnContinue = null;
		protected UnityEngine.UI.Button btnStart = null;
		
		
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
