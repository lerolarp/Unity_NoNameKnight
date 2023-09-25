using Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPanelBattleResult : UIPanel
    {
        [SerializeField] public Button chapterSelect;
        [SerializeField] public TMP_Text resultText;
        [SerializeField] public TMP_Text mvpText;

        public override void Initialize()
        {
            base.Initialize();

            base.uiPanelType = UIPanelType.UIBattleResult;

            stage = GameManager.Instance.CurrentStage;

            chapterSelect.onClick.AddListener(()=> 
            {
                OnChpterSelectMove();
            });

            this.gameObject.SetActive(false);
        }

        public void SetResult(string resultText, string mvpText)
        {
            this.resultText.text = resultText;
            this.mvpText.text = mvpText;

            this.gameObject.SetActive(true);
        }

        private void OnChpterSelectMove()
        {
            GameManager.Instance.ChangeScene("ChapterScene");
        }
    }
}
