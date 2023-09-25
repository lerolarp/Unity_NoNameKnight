using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIToggle : UIComponent
    {
        [SerializeField] public Toggle toggle;
        [SerializeField] public Image backImage;

        public Action<bool> toggleCallBack = null;

        public void Start()
        {
            toggle.onValueChanged.AddListener(delegate { ChangeToggle(); });
        }

        public void OnDestroy()
        {
            toggleCallBack = null;
        }

        public void ChangeBackImage(bool isOn)
        {
            if (isOn)
            {
                backImage.color = Color.green;
            }
            else
            {
                backImage.color = Color.white;
            }
        }

        private void ChangeToggle()
        {
            ChangeBackImage(toggle.isOn);

            if (toggleCallBack != null)
                toggleCallBack.Invoke(toggle.isOn);
        }
    }
}
