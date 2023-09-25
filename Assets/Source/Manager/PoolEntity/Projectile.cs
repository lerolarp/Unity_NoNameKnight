using Data;
using UnityEngine;

public class Projectile : PoolEntity
{
    public CharacterHandler target;

    public float speed;
    public Vector3 dir;

    public void SetAttribute(SkillData data, SkillEffectData effectData, CharacterHandler target, Vector3 dir)
    {
        this.tag = "Projectile";

        base.data = data;
        base.effectData = effectData;
        this.lifeTime = effectData.lifeTime;

        ProjectileData projectileData = ProjectileTableInfo.GetProjectileData(effectData.effectId);

        GameObject obj = Resources.Load<GameObject>(PrafabPathInfo.GetPrefabPath(projectileData.projectileName));
        Instantiate(obj, this.transform);

        this.speed = projectileData.speed;

        this.target = target;
        if (this.target == null)
            this.dir = dir.normalized;
        else
        {
            Vector3 goalPos = this.target.transform.position + (Vector3.up / 2);
            this.dir = (goalPos - this.transform.position).normalized;
        }

        Vector3 baseNormal = Vector3.right;

        //z가 0이 오른쪽
        //x와 y의 각도가 몇이냐에 따라 z의 값이 변해야한다.
        //case : right일 경우 0, up일 경우 90, left의 경우 180, down일 경우 270

        //(1, 0) : 0
        //(0, 1) : 90
        //(-1, 0) : 180
        //(0, -1) : 270

        //기준 벡터와 얼마나 떨어져 있는지 확인
        float dotProduct = Vector3.Dot(baseNormal, this.dir);
        float angleValue = dotProduct > 0 ? 90 - (90 * dotProduct) : 90 + (90 * -1 * dotProduct);

        //벡터가 baseVactor 기준으로 오른쪽인지 왼쪽인지 확인 : z가 양수면 왼쪽, 음수면 오른쪽 
        Vector3 crossProduct = Vector3.Cross(baseNormal, this.dir);
        if (crossProduct.z < 0)
            angleValue = angleValue * - 1;

        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleValue));

        this.boxCollider.offset = projectileData.offsetPos;
        this.boxCollider.size = projectileData.size;
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

        this.transform.position = this.transform.position + (dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.tag;

        int layer = TagLayerTableInfo.GetTagLayerData(tag);
        if (layer != this.gameObject.layer)
        {
            CharacterHandler handler = other.gameObject.GetComponentInParent<CharacterHandler>();
            if (this.target == handler && handler != null && handler.OnHitCallBack != null)
            {
                handler.OnHitCallBack.Invoke(data, effectData, handler);
                PoolManager.Instance.Push(this);
            }
        }
    }
}

