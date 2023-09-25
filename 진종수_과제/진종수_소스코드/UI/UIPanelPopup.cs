using Enum;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPanelPopup : UIPanel
    {
        [SerializeField] public Button okButton;
        [SerializeField] public Button noButton;

        [SerializeField] public TMP_Text message;

        private Action okCallback = null;

        public void OnEnable()
        {
            noButton.onClick.AddListener(() => 
            {
                this.gameObject.SetActive(false);
            });

            okButton.onClick.AddListener(() =>
            {
                if (okCallback != null)
                    okCallback.Invoke();

                this.gameObject.SetActive(false);
            });
        }

        public override void Initialize()
        {
            base.uiPanelType = UIPanelType.UIPopup;
            this.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            okCallback = null;
        }

        public void SetPopup(string message, Action okCallBack)
        {
            this.message.text = message;
            this.okCallback = okCallBack;
            this.gameObject.SetActive(true);
        }
    }
}
