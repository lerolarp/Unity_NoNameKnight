using Data;
using UnityEngine;

public class HitBox : PoolEntity
{
    public void SetAttribute(SkillData data, SkillEffectData effectData)
    {
        this.tag = "AttackBox";

        this.data = data;
        this.effectData = effectData;
        this.lifeTime = effectData.lifeTime;

        this.boxCollider.offset = new Vector2(effectData.offsetPos.x, effectData.offsetPos.y);
        this.boxCollider.size = effectData.size;
    }

    public override bool Dispose()
    {
        base.Dispose();
        return true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.tag;

        int layer = TagLayerTableInfo.GetTagLayerData(tag);
        if(layer != this.gameObject.layer)
        {
            CharacterHandler handler = other.gameObject.GetComponentInParent<CharacterHandler>();
            if (handler != null && handler.OnHitCallBack != null)
            {
                handler.OnHitCallBack.Invoke(data, effectData, handler);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        Gizmos.DrawCube(this.transform.position, collider.size);
    }
}
