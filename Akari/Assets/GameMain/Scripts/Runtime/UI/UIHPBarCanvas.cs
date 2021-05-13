using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class UIHPBarCanvas : UIHPBarCanvasSign
    {
        List<InfoBarEx> infoBarIces = new List<InfoBarEx>();

        List<int> hasIndex = new List<int>();
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitUIData();

            var res = Infos.GetComponentsInChildren<ReferenceCollector>(true);
            for (int i = 0; i < res.Length; i++)
            {
                infoBarIces.Add(new InfoBarEx(res[i]));
            }
        }

        public InfoBarEx GetHpBar()
        {
            for (int i = 0; i < infoBarIces.Count; i++)
            {
                if (hasIndex.Contains(i))
                {
                    continue;
                }

                hasIndex.Add(i);
                infoBarIces[i].CachedTransform.gameObject.SetActive(true);
                return infoBarIces[i];
            }

            return null;
        }
    }
}
