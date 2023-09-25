using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Channeling : PoolEntity
{
    private Dictionary<int, float> channelingHitList = new Dictionary<int, float>();

    private float hitInterval = 3.0f;

    public void SetAttribute(SkillData data, SkillEffectData effectData)
    {
        this.tag = "Channeling";

        this.data = data;
        this.effectData = effectData;
        this.lifeTime = effectData.lifeTime;
        this.hitInterval = effectData.paramsValue[0];

        this.boxCollider.offset = new Vector2(effectData.offsetPos.x, effectData.offsetPos.y);
        this.boxCollider.size = effectData.size;
        channelingHitList.Clear();

    }

    public override bool Dispose()
    {

        channelingHitList.Clear();

        return true;
    }

    public override void Update()
    {
        base.Update();

        var list = channelingHitList.Select(x => x.Key).ToArray();
        for(int i = 0; i < list.Length; ++i)
        {
            int uid = list[i];

            if (channelingHitList[uid] <= 0)
                continue;

            channelingHitList[uid] = Mathf.Clamp(channelingHitList[uid] - Time.deltaTime, 0, float.MaxValue);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.tag;

        int layer = TagLayerTableInfo.GetTagLayerData(tag);
        if (layer != this.gameObject.layer)
        {
            CharacterHandler handler = other.gameObject.GetComponentInParent<CharacterHandler>();
            if (handler != null && handler.OnHitCallBack != null)
            {
                handler.OnHitCallBack.Invoke(data, effectData, handler);
            }
        }
    }

    private bool IsCheckHitAble(CharacterHandler handler)
    {
        if (channelingHitList.ContainsKey(handler.Data.UID))
            return 0 >= channelingHitList[handler.Data.UID];

        return true;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        string tag = other.transform.tag;

        int layer = TagLayerTableInfo.GetTagLayerData(tag);
        if (layer != -1 && layer != this.gameObject.layer)
        {
            CharacterHandler handler = other.gameObject.GetComponentInParent<CharacterHandler>();

            if (IsCheckHitAble(handler) == false)
                return;

            if (handler != null && handler.OnHitCallBack != null)
            {
                //Debug.Log("Hit!");

                handler.OnHitCallBack.Invoke(data, effectData, handler);

                if(channelingHitList.ContainsKey(handler.Data.UID) == false)
                    channelingHitList.Add(handler.Data.UID, hitInterval);

                channelingHitList[handler.Data.UID] = hitInterval;
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
