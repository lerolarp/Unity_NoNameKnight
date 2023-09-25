using Data;
using Enum;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UICharacterCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image characterImage;
        [SerializeField] private TMP_Text level;
        [SerializeField] private Button selectButton;

        [SerializeField] private GameObject RegistGo;

        private CharacterData data = null;
        public CharacterData Data => data;

        private string pathStr = "CharacterResource/{0}/{0}";

        private Action<int> buttonCallBack;
        private Action<int, BattleLineType> dragCallBack;
        private Action<UICharacterCell/*My Cell*/, UICharacterCell/*Touch Cell*/> swapCallBack;

        private Transform parentTr;
        private Transform dragTr;

        private RectTransform dragCheckRt;
        private UICharacterCell swapCheckCell;
        private Vector3 originPos;
        private bool isDrag;

        private bool isEnableDrag;

        public void Awake()
        {
            selectButton.onClick.AddListener(() =>
            {
                OnClickSelect();
            });

            parentTr = this.transform.parent;
        }

        public void OnDisable()
        {
            this.transform.SetParent(parentTr);
            data = null;
            swapCheckCell = null;
            dragCheckRt = null;

            isDrag = false;
        }

        public void OnDestroy()
        {
            this.transform.SetParent(parentTr);
            data = null;
            swapCheckCell = null;
            dragCheckRt = null;
        }

        public void SetClickCallBack(Action<int> buttonCallBack)
        {
            this.buttonCallBack = buttonCallBack;
        }

        public void SetEndDragCallBack(Action<int, BattleLineType> dragCallBack)
        {
            this.dragCallBack = dragCallBack;
        }

        public void SetSwapDragCallBack(Action<UICharacterCell, UICharacterCell> swapCallBack)
        {
            this.swapCallBack = swapCallBack;
        }

        public void SetDragParent(Transform tr)
        {
            this.dragTr = tr;
        }

        public void SetCharacter(CharacterData characterData, bool isEnableDrag)
        {
            swapCheckCell = null;
            dragCheckRt = null;

            data = characterData;

            this.isEnableDrag = isEnableDrag;

            string characterName = CharacterTableInfo.GetCharacterName(data.CharacterIndex);
            string path = string.Format(pathStr, characterName);

            characterImage.sprite = Resources.Load<Sprite>(path);
            level.text = $"Lv.{data.Level}";
            SetRegist(false);
        }

        public void SetCharacter(int characterIndex, bool isEnableDrag)
        {
            CharacterData data = CharacterTableInfo.GetCharacterData(characterIndex);

            SetCharacter(data, isEnableDrag);
        }

        public void SetRegist(bool isRegist)
        {
            RegistGo.SetActive(isRegist);
        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (isEnableDrag == false)
                return;

            isDrag = true;
            originPos = this.transform.position;

            this.transform.SetParent(dragTr);
        }

        public void OnDrag(PointerEventData data)
        {
            if (isEnableDrag == false)
                return;

            this.transform.position = data.position;
        }

        public void OnEndDrag(PointerEventData data)
        {
            if (isEnableDrag == false)
                return;

            this.transform.SetParent(parentTr);
            this.transform.position = originPos;

            if (swapCallBack != null && swapCheckCell != null)
            {
                swapCallBack.Invoke(this, swapCheckCell);
                isDrag = false;
                return;
            }

            if (dragCallBack != null && dragCheckRt != null)
            {
                BattleLineType type = BattleLineType.Front;

                if (dragCheckRt.name.Contains("Middle"))
                    type = BattleLineType.Middle;

                if (dragCheckRt.name.Contains("Back"))
                    type = BattleLineType.Back;

                dragCallBack(this.data.UID, type);
            }

            isDrag = false;
        }


        private void OnClickSelect()
        {
            if (isDrag)
                return;

            if (buttonCallBack != null)
                buttonCallBack.Invoke(data.UID);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            swapCheckCell = other.gameObject.GetComponent<UICharacterCell>();
            Debug.Log($"OnCollosionBegin2D : {swapCheckCell}");
            if (isDrag == false)
                return;

        }

        void OnCollisionExit2D(Collision2D other)
        {
            swapCheckCell = null;
            Debug.Log($"OnCollisionExit2D : {swapCheckCell}");
            if (isDrag == false)
                return;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (isDrag == false)
                return;

            if (other.name.Contains("Line"))
            {
                dragCheckRt = other.GetComponent<RectTransform>();
            }

        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (isDrag == false)
                return;


            if (other.name.Contains("Line"))
            {
                dragCheckRt = null;
            }
        }
    }
}
