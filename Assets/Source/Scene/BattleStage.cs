using UnityEngine;
using System.Collections.Generic;
using Data;
using Enum;
using UI;
using System.Collections;
using System.Linq;

public class BattleStage : GameStage
{
    private BattlePositionComponent battleComponent = null;

    private int mapIndex => GameManager.Instance.MapIndex;
    private List<CharacterHandler> characters = new List<CharacterHandler>();

    private List<CharacterData> battleCharacterList = new List<CharacterData>();
    private Dictionary<BattleLineType, List<CharacterHandler>> enemyDic = new Dictionary<BattleLineType, List<CharacterHandler>>();
    private Coroutine zOrderCor = null;
    private Coroutine entranceCor = null;

    private float time = 0;
    private int heroTotalHp = 0;
    private int enemyTotalHp = 0;
    
    private bool isResult = false;
    public bool IsResult => isResult;

    private bool isStart = false;
    public bool IsStart => isStart;


    public void OnDestroy()
    {
        InitData();
    }

    private void InitData()
    {
        if (zOrderCor != null)
        {
            StopCoroutine(zOrderCor);
            zOrderCor = null;
        }

        if (entranceCor != null)
        {
            StopCoroutine(entranceCor);
            entranceCor = null;
        }

        characters.Clear();
        battleCharacterList.Clear();

        foreach (var item in enemyDic)
        {
            item.Value.Clear();
        }

        enemyDic.Clear();

        isStart = false;
    }

    public List<CharacterHandler> GetCharacterList(CampEnum camp = CampEnum.None)
    {
        if (camp == CampEnum.None)
            return characters;

        return characters.FindAll(x => x.Data.CampType == camp);
    }

    override public void OnSceneLoaded()
    {
        base.OnSceneLoaded();

        InitData();

        Instantiate(MapTableInfo.GetMapPrefab(mapIndex));

        GameManager.Instance.userData.ResetHeroHP();
        SetupCharacter();

        time = 60.0f;

        Time.timeScale = GameManager.Instance.timeScale;

        zOrderCor = StartCoroutine("ZorderCoroutine");
        entranceCor = StartCoroutine("EntranceCor");
    }


    private void UpdateCoolTime()
    {
        for(int i = 0; i < battleCharacterList.Count; ++i)
        {
            int[] array = SkillTableInfo.GetSkillList(battleCharacterList[i].CharacterIndex);
            if (array == null)
                break;

            for(int j = 0; j < array.Length; ++j)
            {
                float currentCoolTime = battleCharacterList[i].GetSkillCoolTime(array[j]);
                if (currentCoolTime == -1)
                    continue;

                float nextCoolTime = Mathf.Clamp(currentCoolTime - Time.deltaTime, 0, float.MaxValue);

                battleCharacterList[i].SetSkillCoolTime(array[j], nextCoolTime);
            }
        }
    }


    public void Update()
    {
        if (isResult)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIPanelMenu menuPanel = UiWindow.GetPanel(UIPanelType.UIMenu) as UIPanelMenu;
            if (menuPanel.gameObject.activeSelf)
                menuPanel.gameObject.SetActive(false);
            else
                menuPanel.gameObject.SetActive(true);
        }

        if (isStart == false)
            return;

        UpdateCoolTime();

        var heroDic = characters.Where(x => x.Data.CampType == CampEnum.Hero);
        var enemyDic = characters.Where(x => x.Data.CampType == CampEnum.Enemy);

        int heroCurrentHp = 0;
        int enemyCurrentHp = 0;

        foreach(var hero in heroDic)
        {
            heroCurrentHp += hero.Data.Hp;
        }

        foreach (var enemy in enemyDic)
        {
            enemyCurrentHp += enemy.Data.Hp;
        }

        UIBattle battlePanel = UiWindow.GetPanel(UIPanelType.UIBattle) as UIBattle;
        battlePanel.SetTotalHP(heroCurrentHp, heroTotalHp, enemyCurrentHp, enemyTotalHp);

        if (heroCurrentHp <= 0 || enemyCurrentHp <= 0)
        {
            isResult = true;
        }
        else
        {
            time -= Time.deltaTime;
            battlePanel.SetTime(time);

            if (time <= 0)
                isResult = true;
        }

        if(isResult)
        {
            UIPanelMenu menuPanel = UiWindow.GetPanel(UIPanelType.UIMenu) as UIPanelMenu;
            if (menuPanel.gameObject.activeSelf)
                menuPanel.gameObject.SetActive(false);

            UIPanelBattleResult panel = uiWindow.GetPanel(UIPanelType.UIBattleResult) as UIPanelBattleResult;

            string resultText = time > 0 && heroCurrentHp > 0 ? "Result : Win" : "Result : Lose";
            string mvpCharacterText = "";

            panel.SetResult(resultText, mvpCharacterText);
        }
    }


    public void OnHit(SkillData data, SkillEffectData effectData, CharacterHandler hitCharacter)
    {
        CharacterData hitData = battleCharacterList.Find(x => x.UID == hitCharacter.Data.UID);
        if(hitData == null)
        {
            Debug.LogError($"Not Found CharacterData {hitCharacter.CharacterIndex}");
            return;
        }

        hitData.SetHP(Mathf.Clamp(hitData.Hp - effectData.skillDamage, 0, hitData.MaxHp));

        //Debug.Log($"{hitData.CharacterName} :: {effectData.skillDamage} Damege! HP : {hitData.Hp}");

        if (0 >= hitData.Hp)
        {
            hitCharacter.OnDie();
        }
        else
        {
            hitCharacter.OnHit();

            if(effectData.effectType == SkillEffectType.KnockBack)
            {
                Vector3 endPos = hitCharacter.transform.position;

                endPos.x += hitCharacter.moveComponent.GetLookDir().x * -1 * effectData.paramsValue[0];
                hitCharacter.KnockBack(endPos, effectData.paramsValue[1]);
            }
        }

        Vector3 uiPosition = Camera.main.WorldToScreenPoint(hitCharacter.transform.position);
        var entity = PoolManager.Instance.Create(PoolEntity.PoolType.UIFloatingDamage, uiPosition, null, uiWindow.poolObjectGo.transform);
        UIFloatingDamage floating = entity as UIFloatingDamage;

        Color color = Color.red;

        if (hitCharacter.Data.CampType == CampEnum.Enemy)
            color = Color.white;

        floating.SetAttribute(effectData.skillDamage, color);
    }


    private void SetupCharacter()
    {
        GameObject positionObj = GameObject.Find("BattlePosition");
        battleComponent = positionObj.GetComponent<BattlePositionComponent>();

        GameObject hpSliderObj = Resources.Load<GameObject>(PrafabPathInfo.GetPrefabPath("HPSlider"));
        GameObject obj = Resources.Load<GameObject>(PrafabPathInfo.GetPrefabPath("Character_Hendler"));

        Dictionary<BattleLineType, List<CharacterData>> dic = GameManager.Instance.userData.BattleDic;
        foreach (var hero in dic)
        {
            for (int i = 0; i < hero.Value.Count; ++i)
            {
                GameObject characterObj = Instantiate(obj);
                UIHpSlider hpSlider = Instantiate(hpSliderObj, uiWindow.poolObjectGo.transform).GetComponent<UIHpSlider>();

                //몇번째 라인이고, 몇개의 캐릭터가 들어가고, 그 캐릭터는 몇번째인지
                var array = hero.Value.Select(x => x.CharacterIndex).ToArray();

                Vector2 pos = battleComponent.GetBattlePos(hero.Key, CampEnum.Hero, array, i, true);

                CharacterHandler handler = characterObj.GetComponent<CharacterHandler>();
                handler.tag = "Hero";
                handler.Initialize(hero.Value[i], pos, Vector3.right, hpSlider);
                handler.OnHitCallBack = OnHit;

                hpSlider.Initialize(handler);

                characters.Add(handler);
                battleCharacterList.Add(hero.Value[i]);
            }
        }

        Dictionary<BattleLineType, List<int>> spawnerInfo = MapTableInfo.GetMapSpawner(mapIndex);
        if (spawnerInfo != null)
        {
            int UidCount = 0;
            foreach (var lineInfo in spawnerInfo)
            {
                for (int i = 0; i < lineInfo.Value.Count; ++i)
                {
                    GameObject characterObj = Instantiate(obj);
                    UIHpSlider hpSlider = Instantiate(hpSliderObj, uiWindow.poolObjectGo.transform).GetComponent<UIHpSlider>();

                    CharacterHandler handler = characterObj.GetComponent<CharacterHandler>();
                    handler.tag = "Enemy";

                    CharacterData data = CharacterTableInfo.GetCharacterData(lineInfo.Value[i]);
                    data.SetCampType(CampEnum.Enemy);
                    data.SetUID(UidCount++);

                    Vector2 pos = battleComponent.GetBattlePos(lineInfo.Key, CampEnum.Enemy, lineInfo.Value.ToArray(), i, true);

                    handler.Initialize(data, pos, Vector3.left, hpSlider);
                    handler.OnHitCallBack = OnHit;

                    hpSlider.Initialize(handler);

                    characters.Add(handler);

                    if (this.enemyDic.ContainsKey(lineInfo.Key) == false)
                        this.enemyDic.Add(lineInfo.Key, new List<CharacterHandler>());

                    this.enemyDic[lineInfo.Key].Add(handler);
                    battleCharacterList.Add(handler.Data);
                }
            }
        }

        var heroDic = characters.Where(x => x.Data.CampType == CampEnum.Hero);
        var enemyDic = characters.Where(x => x.Data.CampType == CampEnum.Enemy);

        heroTotalHp = 0;
        enemyTotalHp = 0;

        foreach (var hero in heroDic)
        {
            heroTotalHp += hero.Data.Hp;
        }

        foreach (var enemy in enemyDic)
        {
            enemyTotalHp += enemy.Data.Hp;
        }

        foreach(var cha in characters)
        {
            cha.Data.SetSkillMaxCoolTime();
        }
    }

    public IEnumerator ZorderCoroutine()
    {
        while (true)
        {
            characters.Sort((CharacterHandler a, CharacterHandler b) =>
            {
                return a.transform.position.y > b.transform.position.y ? -1 : 1 ;
            });

            yield return new WaitForSeconds(0.1f);
        }
    }


    public IEnumerator EntranceCor()
    {
        //최초 시작에서 끝점까지 이동하는 로직
        Dictionary<int, Vector3> endPosCheckDic = new Dictionary<int, Vector3>();

        Dictionary<BattleLineType, List<CharacterData>> dic = GameManager.Instance.userData.BattleDic;
        foreach (var hero in dic)
        {
            for (int i = 0; i < hero.Value.Count; ++i)
            {
                var array = hero.Value.Select(x => x.CharacterIndex).ToArray();
                Vector2 pos = battleComponent.GetBattlePos(hero.Key, CampEnum.Hero, array, i, false);

                CharacterHandler handler = characters.Find(x => x.Data.UID == hero.Value[i].UID);
                handler.Move(Vector3.zero, pos);

                endPosCheckDic.Add(handler.Data.UID, pos);
            }
        }

        foreach (var enemy in enemyDic)
        {
            for (int i = 0; i < enemy.Value.Count; ++i)
            {
                var array = enemy.Value.Select(x => x.CharacterIndex).ToArray();
                Vector2 pos = battleComponent.GetBattlePos(enemy.Key, CampEnum.Enemy, array, i, false);

                CharacterHandler handler = enemy.Value[i];
                handler.Move(Vector3.zero, pos);

                endPosCheckDic.Add(handler.Data.UID, pos);
            }
        }

        int matchMaxCount = endPosCheckDic.Count;
        while (true)
        {
            int matchCurrentCount = 0;

            foreach (var cha in endPosCheckDic)
            {
                CharacterHandler handler = characters.Find(x => x.Data.UID == cha.Key);

                if (handler.transform.position == cha.Value)
                    matchCurrentCount++;
            }

            if (matchCurrentCount == matchMaxCount)
                break;

            yield return null;
        }

        UIBattle battlePanel = UiWindow.GetPanel(UIPanelType.UIBattle) as UIBattle;
        battlePanel.SetUpStartUI($"3");
        yield return new WaitForSeconds(1.0f);

        battlePanel.SetUpStartUI($"2");
        yield return new WaitForSeconds(1.0f);

        battlePanel.SetUpStartUI($"1");
        yield return new WaitForSeconds(1.0f);

        isStart = true;
        battlePanel.SetUpStartUI($"MapId : {mapIndex}");

        yield return null;
    }
}
