using Data;
using UnityEngine;

public class PoolEntity : MonoBehaviour
{
    public enum PoolType
    {
        AttackBox,
        Projectile,
        Channeling,
        Summon,
        UIFloatingDamage,
    }


    protected PoolType type;
    public PoolType Type => type;

    protected float lifeTime = 0.0f;
    public float LifeTime => lifeTime;

    public SkillData data;
    public SkillEffectData effectData;
    public BoxCollider2D boxCollider;

    virtual public bool Initialize(PoolType type, Vector3 pos)
    {
        this.type = type;

        transform.localPosition = pos;
        transform.localScale = Vector3.one;

        gameObject.SetActive(true);

        return false;
    }

    virtual public bool Dispose()
    {
        data = null;
        effectData = null;

        return false;
    }

    virtual public void Update()
    {
        lifeTime -= Time.deltaTime;

        if(lifeTime < 0)
        {
            PoolManager.Instance.Push(this);
        }
    }
}
