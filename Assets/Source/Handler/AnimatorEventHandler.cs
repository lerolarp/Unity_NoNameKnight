using Data;
using UnityEngine;

public class AnimatorEventHandler : MonoBehaviour
{
    private void SetCharacterAnimEvent(CharacterHandler handler, SkillEffectData skillEffectData, string[] splitStr)
    {
        switch (skillEffectData.effectType)
        {
            case Enum.SkillEffectType.HitBox:
                {
                    handler.EventCreateHitBox(int.Parse(splitStr[0]), int.Parse(splitStr[1]));
                }
                break;

            case Enum.SkillEffectType.Projectile:
                {
                    handler.EventCreateProjectile(int.Parse(splitStr[0]), int.Parse(splitStr[1]));
                }
                break;

            case Enum.SkillEffectType.Channeling:
                {
                    handler.EventCreateChanneling(int.Parse(splitStr[0]), int.Parse(splitStr[1]));
                }
                break;

            case Enum.SkillEffectType.Target:
            case Enum.SkillEffectType.KnockBack:
                {
                    handler.EventTargetAttack(int.Parse(splitStr[0]), int.Parse(splitStr[1]));
                }
                break;

            case Enum.SkillEffectType.Dash:
                {
                    handler.EventTargetDash(int.Parse(splitStr[0]), int.Parse(splitStr[1]));
                }
                break;

            case Enum.SkillEffectType.TargetTracking:
                {
                    handler.EventTargetTracking(int.Parse(splitStr[0]), int.Parse(splitStr[1]));
                }
                break;

            case Enum.SkillEffectType.Summon:
                {
                    handler.EventSummon(int.Parse(splitStr[0]), int.Parse(splitStr[1]));
                }
                break;
        }
    }


    public void SetSkillEffect(string skillString)
    {
        GameObject obj = this.transform.parent.gameObject;

        skillString = skillString.Replace(" ", "");
        string[] splitStr = skillString.Split(',');

        SkillEffectData skillEffectData = SkillEffectTableInfo.GetSkillEffectData(int.Parse(splitStr[1]));

        CharacterHandler handler = obj.GetComponent<CharacterHandler>();
        if (handler != null)
        {
            SetCharacterAnimEvent(handler, skillEffectData, splitStr);
        }

        Summon summon = obj.GetComponent<Summon>();
        if (summon != null)
        {
            SetSummonAnimEvent(summon, skillEffectData, splitStr);
        }
    }

    private void SetSummonAnimEvent(Summon summon, SkillEffectData skillEffectData, string[] splitStr)
    {
        switch (skillEffectData.effectType)
        {
            case Enum.SkillEffectType.Target:
                {
                    summon.EventTargetAttack(int.Parse(splitStr[0]), int.Parse(splitStr[1]));
                }
                break;
        }
    }
}
