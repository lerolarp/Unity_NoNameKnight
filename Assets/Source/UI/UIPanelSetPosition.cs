using Data;
using Enum;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPanelSetPosition : UIPanel
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text mapText;
        [SerializeField] private List<UICharacterCell> cellList;
        [SerializeField] private Button enterButton;

        [Header("Hero")]
        [SerializeField] private List<UICharacterCell> heroFrontList;
        [SerializeField] private List<UICharacterCell> heroMiddleList;
        [SerializeField] private List<UICharacterCell> heroBackList;

        [Header("Enemy")]
        [SerializeField] private List<UICharacterCell> enemyFrontList;
        [SerializeField] private List<UICharacterCell> enemyMiddleList;
        [SerializeField] private List<UICharacterCell> enemyBackList;

        [Header("Line")]
        [SerializeField] private List<RectTransform> heroLineGo;
        [SerializeField] private List<RectTransform> enemyLineGo;

        [SerializeField] private Transform cellDragTr;

        private int mapIndex = 0;

        private void Awake()
        {
            closeButton.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });

            enterButton.onClick.AddListener(() =>
            {
                OnEnterBattle();
            });

            for (int i = 0; i < cellList.Count; ++i)
            {
                cellList[i].SetClickCallBack(OnClickScrollCell);
               
            }

            for (int i = 0; i < 3; ++i)
            {
                heroFrontList[i].SetClickCallBack(OnClickBattleLineCell);
                heroFrontList[i].SetEndDragCallBack(OnDragCharacterCell);
                heroFrontList[i].SetSwapDragCallBack(OnSwapCharacterCell);
                heroFrontList[i].SetDragParent(cellDragTr);

                heroMiddleList[i].SetClickCallBack(OnClickBattleLineCell);
                heroMiddleList[i].SetEndDragCallBack(OnDragCharacterCell);
                heroMiddleList[i].SetSwapDragCallBack(OnSwapCharacterCell);
                heroMiddleList[i].SetDragParent(cellDragTr);

                heroBackList[i].SetClickCallBack(OnClickBattleLineCell);
                heroBackList[i].SetEndDragCallBack(OnDragCharacterCell);
                heroBackList[i].SetSwapDragCallBack(OnSwapCharacterCell);
                heroBackList[i].SetDragParent(cellDragTr);
            }
        }

        private void OnEnable()
        {
            UIRefresh();
            RefreshLineEnemyCharacter();
        }

        public override void Initialize()
        {
            base.uiPanelType = UIPanelType.UISetChapterPosition;
            this.gameObject.SetActive(false);
        }

        public void SetMapIndex(int index)
        {
            mapIndex = index;
        }

        public void UIRefresh()
        {
            mapText.text = $"Select Map Index : {mapIndex + 1}";
            RefreshCharacterScroll();
            RefreshLineHeroCharacter();
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                this.gameObject.SetActive(false);
            }
        }

        private List<UICharacterCell> GetLineCell(BattleLineType type, bool isHero)
        {
            if(type == BattleLineType.Front)
                return isHero ? heroFrontList : enemyFrontList;

            if (type == BattleLineType.Middle)
                return isHero ? heroMiddleList : enemyMiddleList;

            if (type == BattleLineType.Back)
                return isHero ? heroBackList : enemyBackList;

            return null;
        }

        private void RefreshCharacterScroll()
        {
            List<CharacterData> list = GameManager.Instance.userData.CharacterList;
            List<CharacterData> battleList = GameManager.Instance.userData.GetBattleCharList();
            for (int i = 0; i < list.Count; ++i)
            {
                if (cellList.Count <= i)
                    break;

                cellList[i].SetCharacter(list[i], false);
                var findCharacter = battleList.Find(x => x.UID == list[i].UID);
                cellList[i].SetRegist(findCharacter != null);
            }
        }

        private void RefreshLineHeroCharacter()
        {
            Dictionary<BattleLineType, List<CharacterData>> dic = GameManager.Instance.userData.BattleDic;
            foreach (var battleInfo in dic)
            {
                List<CharacterData> list = battleInfo.Value;

                RectTransform rt = heroLineGo[(int)battleInfo.Key];

                float minY = rt.transform.position.y - (rt.sizeDelta.y / 2);
                float maxY = rt.transform.position.y + (rt.sizeDelta.y / 2);

                float yInterval = (maxY - minY) / (list.Count + 1);

                float xPos = rt.transform.position.x;

                var cellList = GetLineCell(battleInfo.Key, true);
                for (int i = 0; i < cellList.Count; ++i)
                {
                    if (list.Count <= i)
                    {
                        cellList[i].gameObject.SetActive(false);
                        continue;
                    }

                    cellList[i].gameObject.SetActive(true);
                    cellList[i].SetCharacter(list[i], true);

                    cellList[i].transform.position = new Vector2(xPos, maxY - (yInterval * (i + 1)));
                }
            }
        }

        private void RefreshLineEnemyCharacter()
        {
            Dictionary<BattleLineType, List<int>> enemyDic = MapTableInfo.GetMapSpawner(mapIndex);
            foreach (var battleInfo in enemyDic)
            {
                List<int> list = battleInfo.Value;

                RectTransform rt = enemyLineGo[(int)battleInfo.Key];

                float minY = rt.transform.position.y - (rt.sizeDelta.y / 2);
                float maxY = rt.transform.position.y + (rt.sizeDelta.y / 2);

                float yInterval = (maxY - minY) / (list.Count + 1);

                float xPos = rt.transform.position.x;

                var cellList = GetLineCell(battleInfo.Key, false);
                for (int i = 0; i < cellList.Count; ++i)
                {
                    if (list.Count <= i)
                    {
                        cellList[i].gameObject.SetActive(false);
                        continue;
                    }

                    cellList[i].gameObject.SetActive(true);
                    cellList[i].SetCharacter(list[i], false);

                    cellList[i].transform.position = new Vector2(xPos, maxY - (yInterval * (i + 1)));
                }
            }
        }

        private void OnClickBattleLineCell(int uid)
        {
            bool isRegist = GameManager.Instance.userData.IsRegist(uid);
            
            if (isRegist)
                GameManager.Instance.userData.UnRegistCharacter(uid);
            
            UIRefresh();
        }

        private void OnClickScrollCell(int uid)
        {
            bool isRegist = GameManager.Instance.userData.IsRegist(uid);

            if (isRegist)
                GameManager.Instance.userData.UnRegistCharacter(uid);
            else
                GameManager.Instance.userData.RegistCharacter(uid);

            UIRefresh();
        }

        private void OnDragCharacterCell(int uid, BattleLineType type)
        {
            //현재 어디에 있었는지 확인.
            bool isLinePosition = GameManager.Instance.userData.GetBattleLine(uid, out BattleLineType outType);
            if (isLinePosition == false)
                return;

            if (type == outType)
                return;

            var battleDic = GameManager.Instance.userData.BattleDic;
            if (battleDic[type].Count == 3)
                return;

            GameManager.Instance.userData.UnRegistCharacter(uid);
            GameManager.Instance.userData.RegistCharacter(uid, type);

            UIRefresh();
        }

        private void OnSwapCharacterCell(UICharacterCell myCell, UICharacterCell touchCell)
        {
            GameManager.Instance.userData.SwapCharacter(myCell.Data, touchCell.Data);

            UIRefresh();
        }

        private void OnEnterBattle()
        {
            if(GameManager.Instance.userData.GetBattleHeroCount() == 0)
            {
                UIPanelToast.ShowMessage("캐릭터 배치가 필요합니다.");
                return;
            }

            GameManager.Instance.SelectMapIndex(mapIndex);
            GameManager.Instance.ChangeScene("BattleScene");
        }
    }
}
