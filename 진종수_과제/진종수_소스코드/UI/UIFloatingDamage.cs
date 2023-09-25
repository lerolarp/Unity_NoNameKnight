using TMPro;
using UnityEngine;

public class UIFloatingDamage : PoolEntity
{
    [SerializeField] public TMP_Text text;

    public int damage;

    private float speed = 100f;

    override public bool Initialize(PoolType type, Vector3 pos)
    {
        base.type = type;

        transform.position = pos;
        transform.localScale = Vector3.one;

        gameObject.SetActive(true);

        return false;
    }

    public void SetAttribute(int damage, Color damageColor)
    {
        text.text = damage.ToString();
        text.color = damageColor;

        transform.localPosition += new Vector3(0, 150);

        this.lifeTime = 0.3f;
    }

    override public void Update()
    {
        base.Update();
        transform.Translate(new Vector3(0, speed * Time.deltaTime));
    }
}
