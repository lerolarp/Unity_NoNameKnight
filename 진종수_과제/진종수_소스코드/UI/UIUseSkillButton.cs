using Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIUseSkillButton : UIComponent
    {
        [SerializeField] public Button skillUseButton;
        [SerializeField] public Image characterImage;
        [SerializeField] public Image coolTimeImage;
        [SerializeField] public TMP_Text coolTimeText;
        [SerializeField] public GameObject deadGo;

        private CharacterData data = null;
        private int skillId = 0;

        public Action<CharacterData, int> useSkillCallBack = null;
        private string pathStr = "CharacterResource/{0}/{0}";

        override public void Awake()
        {
            skillUseButton.onClick.AddListener(()=>
            {
                if (useSkillCallBack != null)
                    useSkillCallBack.Invoke(data, skillId);
            });

            deadGo.gameObject.SetActive(false);
            coolTimeImage.fillAmount = 0;
            coolTimeText.gameObject.SetActive(true);
        }

        public void Update()
        {
            if (data == null)
                return;

            if(data.IsDead())
            {
                if (deadGo.gameObject.activeSelf == false)
                    deadGo.gameObject.SetActive(true);

                coolTimeImage.fillAmount = 0;
               coolTimeText.gameObject.SetActive(false);

                return;
            }

            SkillData skillData = SkillTableInfo.GetSkillData(skillId);
            if (skillData == null)
                return;

            float currentCoolTme = data.GetSkillCoolTime(skillId);
            coolTimeImage.fillAmount = currentCoolTme / skillData.skillCoolTime;

            if (currentCoolTme <= 0)
            {
                coolTimeText.gameObject.SetActive(false);
            }
            else
            {
                coolTimeText.gameObject.SetActive(true);
                coolTimeText.text = ((int)currentCoolTme).ToString();
            }
        }

        public void OnDestroy()
        {
            useSkillCallBack = null;
        }

        public void Initialize(CharacterData characterData)
        {
            data = characterData;

            string characterName = CharacterTableInfo.GetCharacterName(characterData.CharacterIndex);

            string path = string.Format(pathStr, characterName);
            characterImage.sprite = Resources.Load<Sprite>(path);

            int[] skillArray = SkillTableInfo.GetSkillList(data.CharacterIndex);
            if (skillArray != null)
            {
                for (int i = 0; i < skillArray.Length; ++i)
                {
                    SkillData skillData = SkillTableInfo.GetSkillData(skillArray[i]);

                    if (skillData.skillType == Enum.SkillType.Normal)
                        continue;

                    skillId = skillArray[i];
                }
            }
        }
    }
}
