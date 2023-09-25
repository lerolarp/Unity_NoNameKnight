using Data;
using UnityEngine;

public class Summon : PoolEntity
{
    public CharacterHandler caster;
    public CharacterHandler target;
    public Vector3 dir;

    public void Awake()
    {
        this.tag = "Summon";
    }

    public void SetAttribute(SkillData data, SkillEffectData effectData, CharacterHandler caster, CharacterHandler target, Vector3 dir)
    {
        this.tag = "Summon";
        this.caster = caster;
        this.target = target;

        base.data = data;
        base.effectData = effectData;
        this.lifeTime = effectData.lifeTime;

        this.transform.position = target.transform.position + (Vector3.down / 10.0f);

        SummonData summonDataData = SummonTableInfo.GetSummonData(effectData.effectId);

        GameObject obj = Resources.Load<GameObject>(PrafabPathInfo.GetPrefabPath(summonDataData.summonName));
        Instantiate(obj, this.transform);

        this.boxCollider.offset = new Vector2(effectData.offsetPos.x, effectData.offsetPos.y);
        this.boxCollider.size = effectData.size;
    }

    public override bool Dispose()
    {
        base.Dispose();
        target = null;
        return true;
    }

    public override void Update()
    {
        base.Update();
    }

    public void EventTargetAttack(int skillDataIndex, int skillEffectIndex)
    {
        if (target != null)
        {
            SkillData skillData = SkillTableInfo.GetSkillData(skillDataIndex);
            SkillEffectData skillEffectData = SkillEffectTableInfo.GetSkillEffectData(skillEffectIndex);

            if(caster != null && caster.Data.IsDead() == false)
                caster.OnHitCallBack.Invoke(skillData, skillEffectData, this.target);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }
}

