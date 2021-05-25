// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-05-25 15:58:39.649
//------------------------------------------------------------

namespace Akari
{
    public class UIMainGameSign : UGuiForm
    {
		//---UI---
		protected UnityEngine.GameObject goHP = null;
		protected UnityEngine.UI.Button Btn_Player = null;
		
		
		public void InitUIData()
        {

            ReferenceCollector rc = rootGo.GetComponent<ReferenceCollector>();
			
            goHP = rc.Get<UnityEngine.GameObject>("goHP");
			Btn_Player = rc.Get<UnityEngine.UI.Button>("Btn_Player");
			
        }
    }
}
