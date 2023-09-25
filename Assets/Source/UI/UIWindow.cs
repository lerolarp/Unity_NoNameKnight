using Enum;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIWindow : MonoBehaviour
    {
        [SerializeField] public List<UIPanel> uIPanels = new List<UIPanel>();
        [SerializeField] public GameObject poolObjectGo = null;

        public void Initialize()
        {
            for(int i = 0; i < uIPanels.Count; ++i)
            {
               uIPanels[i].Initialize();
            }
        }

        public UIPanel GetPanel(UIPanelType type)
        {
            UIPanel panel = uIPanels.Find(x => x.UiPanelType == type);
            return panel;
        }
    }
}