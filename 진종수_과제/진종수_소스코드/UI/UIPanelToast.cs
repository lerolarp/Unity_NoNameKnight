using Enum;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIPanelToast : UIPanel
    {
        [SerializeField] private TMP_Text message;

        public float lifeTime = 1;

        public override void Initialize()
        {
            base.Initialize();

            base.uiPanelType = UIPanelType.UIToast;

        }

        public void ShowMessage(string message)
        {
            this.message.text = message;
            lifeTime = 3;
        }

        public void SetLifeTime(float lifeTime)
        {
            this.lifeTime = lifeTime;
        }

        public void Update()
        {
            lifeTime -= Time.deltaTime;

            if(lifeTime <= 0)
            {
                this.gameObject.SetActive(false);
                lifeTime = 3;
            }
        }


        static public void ShowMessage(string message, float lifeTime = 1)
        {
            var window = GameManager.Instance.CurrentStage.UiWindow;
            UIPanelToast panel = window.GetPanel(UIPanelType.UIToast) as UIPanelToast;

            panel.ShowMessage(message);
            panel.SetLifeTime(lifeTime);
            panel.gameObject.SetActive(true);
        }
    }
}
