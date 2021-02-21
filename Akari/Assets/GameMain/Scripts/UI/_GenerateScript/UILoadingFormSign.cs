// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-12-22 22:50:34.713
//------------------------------------------------------------

namespace Akari
{
    public class UILoadingFormSign : UGuiFormEx
    {
		//---UI---
		public UnityEngine.UI.Image imgProgress = null;
		public TMPro.TextMeshProUGUI txtLoading = null;
		
		
		public void InitUIData()
        {

            ReferenceCollector rc = rootGo.GetComponent<ReferenceCollector>();
			
            imgProgress = rc.Get<UnityEngine.UI.Image>("imgProgress");
			txtLoading = rc.Get<TMPro.TextMeshProUGUI>("txtLoading");
			
        }
    }
}
