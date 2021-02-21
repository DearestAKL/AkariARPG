// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020-12-24 23:59:13.618
//------------------------------------------------------------

namespace Akari
{
    public class UIMainGameFormSign : UGuiFormEx
    {
		//---UI---
		public TMPro.TextMeshProUGUI txtHp = null;
		public TMPro.TextMeshProUGUI txtMp = null;
		public UnityEngine.UI.Image imgHpTransition = null;
		public UnityEngine.UI.Image imgHpProgress = null;
		public UnityEngine.UI.Image imgMpTransition = null;
		public UnityEngine.UI.Image imgMpProgress = null;
		
		
		public void InitUIData()
        {

            ReferenceCollector rc = rootGo.GetComponent<ReferenceCollector>();
			
            txtHp = rc.Get<TMPro.TextMeshProUGUI>("txtHp");
			txtMp = rc.Get<TMPro.TextMeshProUGUI>("txtMp");
			imgHpTransition = rc.Get<UnityEngine.UI.Image>("imgHpTransition");
			imgHpProgress = rc.Get<UnityEngine.UI.Image>("imgHpProgress");
			imgMpTransition = rc.Get<UnityEngine.UI.Image>("imgMpTransition");
			imgMpProgress = rc.Get<UnityEngine.UI.Image>("imgMpProgress");
			
        }
    }
}
