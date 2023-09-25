using Enum;
using UnityEngine;

namespace UI
{
    public class UIPanel : MonoBehaviour
    {
        [HideInInspector] public GameStage stage = null;
        [HideInInspector] protected UIPanelType uiPanelType = UIPanelType.None;
        [HideInInspector] public UIPanelType UiPanelType => uiPanelType;


        virtual public void Initialize()
        {
            stage = GameManager.Instance.CurrentStage;
        }

        public UIWindow GetUI()
        {
            return stage.UiWindow;
        }
    }
}
