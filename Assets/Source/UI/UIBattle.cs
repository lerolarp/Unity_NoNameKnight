using Data;
using Enum;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIBattle : UIPanel
    {
        [SerializeField] private UIToggle speedToggle;
        [SerializeField] private UIToggle autoSkillToggle;
        [SerializeField] private List<UIUseSkillButton> skillButton;

        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text MapId;

        [SerializeField] private Slider heroTotalSlider;
        [SerializeField] private Slider enemyTotalSlider;

        [SerializeField] private Button menuButton;

        private BattleStage battleStage;

        public void OnEnable()
        {
            speedToggle.toggleCallBack = OnToggleSpeed;
            autoSkillToggle.toggleCallBack = OnToggleAutoSkill;

            menuButton.onClick.AddListener(() => 
            {
                OnClickMenu();
            });
        }

        public override void Initialize()
        {
            base.Initialize();

            base.uiPanelType = UIPanelType.UIBattle;

            battleStage = stage as BattleStage;
            if (battleStage == null)
                return;

            var userCharDic = GameManager.Instance.userData.BattleDic;
            for (int i = 0; i < skillButton.Count; ++i)
            {
                skillButton[i].gameObject.SetActive(false);
            }

            int index = 0;
            foreach(var character in userCharDic)
            {
                if (skillButton.Count <= index)
                    break;

                for (int i = 0; i < character.Value.Count; ++i)
                {
                    skillButton[index].gameObject.SetActive(true);
                    skillButton[index].Initialize(character.Value[i]);
                    skillButton[index].useSkillCallBack = OnUseSkill;
                    index++;
                }
            }

            bool isSpeed = GameManager.Instance.timeScale == 1 ? false : true;

            speedToggle.toggle.SetIsOnWithoutNotify(isSpeed);
            speedToggle.ChangeBackImage(isSpeed);

            autoSkillToggle.toggle.SetIsOnWithoutNotify(GameManager.Instance.isAutoSkill);
            autoSkillToggle.ChangeBackImage(GameManager.Instance.isAutoSkill);

            MapId.gameObject.SetActive(false);
        }

        public void SetTotalHP(int heroHP, int heroTotalHP, int enemyHP, int enemyTotalHP)
        {
            heroTotalSlider.value = (float)heroHP / heroTotalHP;
            enemyTotalSlider.value = (float)enemyHP / enemyTotalHP;
        }

        public void SetTime(float time)
        {
            int min = (int)time / 60;
            float second = Mathf.Clamp(time % 60, 0, float.MaxValue);

            timeText.text = string.Format("{0:D2}:{1:D2}", min, (int)second);
        }

        private void OnToggleSpeed(bool isOn)
        {
            GameManager.Instance.SetFastPlaySpeed(isOn);
        }

        private void OnToggleAutoSkill(bool isOn)
        {
            GameManager.Instance.SetAutoSkill(isOn);
        }

        private void OnClickMenu()
        {
            UIPanelMenu menu = battleStage.UiWindow.GetPanel(UIPanelType.UIMenu) as UIPanelMenu;

            menu.gameObject.SetActive(true);
        }

        private void OnUseSkill(CharacterData data, int skillIndex)
        {
            BattleStage battleStage = GameManager.Instance.CurrentStage as BattleStage;
            if (battleStage != null && battleStage.IsStart == true)
            {
                if (data.IsDead())
                {
                    UIPanelToast.ShowMessage("캐릭터가 사망하였습니다.");
                    return;
                }

                if (data.IsCoolTime(skillIndex))
                {
                    UIPanelToast.ShowMessage("쿨타임이 돌아오지 않았습니다.");
                    return;
                }

                CharacterHandler handler = battleStage.GetCharacterList().Find(x => x.Data.UID == data.UID);
                handler.Attack(skillIndex, true);
            }
        }

        public void SetUpStartUI(string text)
        {
            MapId.text = text;
            MapId.gameObject.SetActive(true);
        }
    }
}
