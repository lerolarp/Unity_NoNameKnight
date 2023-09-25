using Enum;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPanelMenu : UIPanel
    {
        [SerializeField] public Button exitButton;
        [SerializeField] public Button reset;
        [SerializeField] public Button chapterSelect;

        private BattleStage battleStage;

        private float originTimeScale = 0;

        public void OnEnable()
        {
            originTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void OnDisable()
        {
            Time.timeScale = originTimeScale;
        }

        public override void Initialize()
        {
            base.Initialize();

            base.uiPanelType = UIPanelType.UIMenu;
            stage = GameManager.Instance.CurrentStage;

            battleStage = stage as BattleStage;

            chapterSelect.onClick.AddListener(() =>
            {
                PoolManager.Instance.Dispose();
                OnChpterSelectMove();
            });

            reset.onClick.AddListener(() =>
            {
                PoolManager.Instance.Dispose();
                GameManager.Instance.ChangeScene("BattleScene");
            });

            exitButton.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });

            this.gameObject.SetActive(false);
        }

        public void OnChpterSelectMove()
        {
            GameManager.Instance.ChangeScene("ChapterScene");
        }
    }
}

