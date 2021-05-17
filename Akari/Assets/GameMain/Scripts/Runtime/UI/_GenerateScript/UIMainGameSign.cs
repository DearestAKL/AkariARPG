// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-05-16 12:05:33.212
//------------------------------------------------------------

namespace Akari
{
    public class UIMainGameSign : UGuiForm
    {
		//---UI---
		protected UnityEngine.GameObject goHP = null;
		
		
		public void InitUIData()
        {

            ReferenceCollector rc = rootGo.GetComponent<ReferenceCollector>();
			
            goHP = rc.Get<UnityEngine.GameObject>("goHP");
			
        }
    }
}
