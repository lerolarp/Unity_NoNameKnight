using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHpSlider : MonoBehaviour
    {
        [SerializeField] public Slider slider;
        [SerializeField] public Image hpColor;

        private CharacterHandler character = null;


        public void Initialize(CharacterHandler handler)
        {
            character = handler;
            this.gameObject.SetActive(true);

            hpColor.color = handler.Data.CampType == Enum.CampEnum.Hero ? Color.green : Color.red;
        }

        private void Update()
        {
            if (character == null)
                return;

            Vector3 uiPosition = Camera.main.WorldToScreenPoint(character.transform.position);
            this.transform.position = uiPosition;
            this.transform.localPosition += new Vector3(0, 150);

            slider.value = (float)character.Data.Hp / character.Data.MaxHp;
        }
    }
}
