// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-05-13 16:29:08.821
//------------------------------------------------------------

namespace Akari
{
    public class UIHPBarCanvasSign : UGuiForm
    {
		//---UI---
		protected UnityEngine.GameObject Infos = null;
		
		
		public void InitUIData()
        {

            ReferenceCollector rc = rootGo.GetComponent<ReferenceCollector>();
			
            Infos = rc.Get<UnityEngine.GameObject>("Infos");
			
        }
    }
}
