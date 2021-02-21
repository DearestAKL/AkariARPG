namespace Akari
{
    public class ${ClassName} : UIBasePanel
    {
		//---UI---
		${MemberVariables}
		
		public void InitUIData()
        {

            ReferenceCollector rc = rootGo.GetComponent<ReferenceCollector>();
			
            ${Init}
        }
    }
}
