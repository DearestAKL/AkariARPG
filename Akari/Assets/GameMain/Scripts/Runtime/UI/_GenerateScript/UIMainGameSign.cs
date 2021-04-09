// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-02-28 07:23:23.190
//------------------------------------------------------------

namespace Akari
{
    public class UIMainGameSign : UGuiForm
    {
		//---UI---
		protected UnityEngine.GameObject goHP = null;
		protected UnityEngine.GameObject goMP = null;
		
		
		public void InitUIData()
        {

            ReferenceCollector rc = rootGo.GetComponent<ReferenceCollector>();
			
            goHP = rc.Get<UnityEngine.GameObject>("goHP");
			goMP = rc.Get<UnityEngine.GameObject>("goMP");
			
        }
    }
}
