using Data;
using Enum;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPanelChapterSelect : UIPanel
    {
        [SerializeField] private List<Button> mapSelectButtons;
        [SerializeField] private List<TMP_Text> mapSelectTexts;
        [SerializeField] private Button quitButton;

        public void Awake()
        {
            base.uiPanelType = UIPanelType.UIChapterSelect;

            for (int i = 0; i < mapSelectButtons.Count; ++i)
            {
                int index = i;
                mapSelectButtons[i].onClick.AddListener(() =>
                {
                    OnClickMap(index);
                });
            }

            quitButton.onClick.AddListener(() =>
            {
                QuitPopup();
            });
        }

        public override void Initialize()
        {
            base.Initialize();

            this.gameObject.SetActive(true);
        }

        public void OnEnable()
        {
            for (int i = 0; i < mapSelectButtons.Count; ++i)
            {
                var mapInfo = MapTableInfo.GetMapSpawner(i);
                if (mapInfo == null)
                    break;

                mapSelectTexts[i].text = $"Map Select {i + 1}";
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIPanelSetPosition positionPopup = GetUI().GetPanel(UIPanelType.UISetChapterPosition) as UIPanelSetPosition;
                if (positionPopup.gameObject.activeSelf)
                {
                    positionPopup.gameObject.SetActive(false);
                    return;
                }

                UIPanelPopup popup = GetUI().GetPanel(UIPanelType.UIPopup) as UIPanelPopup;
                if (popup.gameObject.activeSelf)
                    popup.gameObject.SetActive(false);
                else
                    QuitPopup();
            }
        }

        private void OnClickMap(int index)
        {
            var UIwindow = GetUI();

            UIPanelSetPosition uiPanel = UIwindow.GetPanel(UIPanelType.UISetChapterPosition) as UIPanelSetPosition;

            uiPanel.SetMapIndex(index);
            uiPanel.gameObject.SetActive(true);
        }

        private void QuitPopup()
        {
            UIPanelPopup popup = GetUI().GetPanel(UIPanelType.UIPopup) as UIPanelPopup;
            popup.SetPopup("Game Quit?", () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(); 
#endif
            });
        }
    }
}
