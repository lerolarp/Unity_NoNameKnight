using Data;
using Enum;
using System.Collections;
using UnityEngine;

public class AIComponent : Component
{
    private BattleStage stage = null;
    private CharacterHandler myHandler = null;
    private Coroutine coroutineAI = null;

    [SerializeField] private CharacterHandler targetHandler = null;
    public CharacterHandler TargetHandler => targetHandler;

    [SerializeField] private CharacterHandler specialtargetHandler = null;
    public CharacterHandler SpecialTargetHandler => specialtargetHandler;

    public void OnDestroy()
    {
        if(coroutineAI != null)
        {
            StopCoroutine(coroutineAI);
            coroutineAI = null;
        }
    }

    public void Initialize(CharacterHandler data)
    {
        stage = GameManager.Instance.CurrentStage as BattleStage;
        myHandler = data;

        //if(myHandler.Data.CampType == CampEnum.Hero)
        coroutineAI = StartCoroutine("AiCoroutine");
    }

    public bool CheckSkillRange(SkillData data, out Vector3 endDir)
    {
        endDir = Vector3.zero;

        if (targetHandler == null)
            return false;

        Vector3 targetPos = targetHandler.transform.position + new Vector3(0, 30);
        Vector3 myPos = myHandler.transform.position + new Vector3(0, 30);

        Vector3 dir = (targetPos - myPos).normalized;
        float dist = (myPos - targetPos).sqrMagnitude;

        Vector3 lookUpVector = myHandler.moveComponent.GetLookDir();
        Vector3 skillCenter = myPos + new Vector3((data.skillRange / 2) * dir.x, 0, 0);

        float minXPos;
        float maxXPos;
        float minYPos = myPos.y - (data.skillRange / 2);
        float maxYPos = myPos.y + (data.skillRange / 2);

        if (dir.x > 0)
        {
            minXPos = myPos.x;
            maxXPos = skillCenter.x + Vector3.right.x * (data.skillRange / 2);
        }
        else
        {
            minXPos = skillCenter.x + Vector3.left.x * (data.skillRange / 2); 
            maxXPos = myPos.x;
        }

        endDir = dir;

        if (minXPos <= targetPos.x && targetPos.x <= maxXPos && minYPos <= targetPos.y && targetPos.y <= maxYPos)
        {
            return true;
        }
        else
        {
            endDir = dir;
            if (minXPos <= targetPos.x && targetPos.x <= maxXPos)
                endDir.x = 0;

            return false;
        }
    }


    public IEnumerator AiCoroutine()
    {
        while (true)
        {
            if (myHandler.Data.IsDead())
                break;

            BattleStage battleStage = GameManager.Instance.CurrentStage as BattleStage;
            if (battleStage != null && battleStage.IsStart == true)
            {
                //적 탐색
                if (specialtargetHandler != null && specialtargetHandler.Data.IsDead() == false)
                    targetHandler = specialtargetHandler;
                else
                {
                    specialtargetHandler = null;
                    targetHandler = SearchEnemy();
                }

                if (targetHandler != null)
                {
                    //사용가능한 스킬 선택 (쿨타임, 타겟범위 긴놈)
                    SkillData skillData = SelectSkill();
                    if (skillData == null)
                    {
                        myHandler.OnIdle();
                    }
                    else
                    {
                        bool isCheck = CheckSkillRange(skillData, out Vector3 dir);

                        Vector3 lookUpVector = myHandler.moveComponent.GetLookDir();
                        if (Mathf.Approximately(dir.x, 0) == false)
                        {
                            if (dir.x > 0 && lookUpVector.x < 0 || dir.x < 0 && lookUpVector.x > 0)
                                myHandler.moveComponent.LookUp(dir);
                        }

                        //충족하면 스킬 사용, 멀다면 그 스킬로 도착위치 설정
                        if (myHandler.animComponent.animator.GetBool("IsAttack") == false)
                        {
                            if (isCheck == true)
                            {
                                myHandler.Attack(skillData.skillIndex);
                            }
                            else
                            {
                                myHandler.Move(dir);

                                yield return new WaitForSeconds(0.1f);
                            }
                        }
                    }
                }
                else
                {
                    myHandler.OnIdle();
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public SkillData SelectSkill()
    {
        int characterIndex = myHandler.CharacterIndex;

        int[] skillArray = SkillTableInfo.GetSkillList(characterIndex);
        for (int i = 0; i < skillArray.Length - 1; ++i)
        {
            for (int j = i + 1; j < skillArray.Length; ++j)
            {
                SkillData skillDataFirst = SkillTableInfo.GetSkillData(skillArray[i]);
                SkillData skillDataSecond = SkillTableInfo.GetSkillData(skillArray[j]);

                if (skillDataFirst.skillType == SkillType.Normal && skillDataSecond.skillType == SkillType.Special)
                {
                    int tempSkillIndex = skillArray[i];

                    skillArray[i] = skillArray[j];
                    skillArray[j] = tempSkillIndex;
                }
                else if (skillDataFirst.skillType == skillDataSecond.skillType && skillDataFirst.skillRange < skillDataSecond.skillRange)
                {
                    int tempSkillIndex = skillArray[i];

                    skillArray[i] = skillArray[j];
                    skillArray[j] = tempSkillIndex;
                }
            }
        }

        for(int i = 0; i < skillArray.Length; ++i)
        {
            if (myHandler.Data.IsAutoUseSkill(skillArray[i]) == false)
                continue;

            SkillData skillData = SkillTableInfo.GetSkillData(skillArray[i]);
            if (myHandler.Data.CampType == CampEnum.Hero && skillData.skillType == SkillType.Special && GameManager.Instance.isAutoSkill == false)
                continue;

            return SkillTableInfo.GetSkillData(skillArray[i]);
        }

        return null;
    }



    public CharacterHandler SearchEnemy(bool isNear = true)
    {
        if (stage.IsResult)
            return null;

        //한번 잡은 타겟은 변경하지 않는다? 생각을 해볼 필요가 있음.
        //if (isNear == true && targetHandler != null && targetHandler.Data.IsDead() == false)
        //    return targetHandler;

        //if (myHandler.animComponent.animator.GetBool("IsAttack") == true)
        //    return null;

        CharacterHandler searchCharacter = null;

        CampEnum findCamp = myHandler.Data.CampType == CampEnum.Hero ? CampEnum.Enemy : CampEnum.Hero;

        var list = stage.GetCharacterList(findCamp);

        float findDist = isNear ? float.MaxValue : float.MinValue;
        Vector3 myPos = transform.position;

        for(int i = 0; i < list.Count; ++i)
        {
            if (list[i].Data.IsDead())
                continue;

            Vector3 targetPos = list[i].transform.position;

            float dist = (myPos - targetPos).sqrMagnitude;

            bool isFindTarget = isNear ? findDist > dist : findDist < dist;

            if (isFindTarget)
            {
                findDist = dist;
                searchCharacter = list[i];
            }
        }

        return searchCharacter;
    }

    public void SetSpecialTarget(CharacterHandler handler)
    {
        specialtargetHandler = handler;
    }
}
