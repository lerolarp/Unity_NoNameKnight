using UnityEngine;
using System;
using UI;
using Data;

public class CharacterHandler : MonoBehaviour
{
    //Component
    [SerializeField] public AnimatorComponent animComponent;
    [SerializeField] public MoveComponent moveComponent;

    //Render & Collision
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public BoxCollider2D boxCollider;

    [SerializeField] public AIComponent aiComponent;

    [SerializeField] public UIHpSlider slider;

    private CharacterData data = null;
    public CharacterData Data => data;
    public int CharacterIndex => data != null ? data.CharacterIndex : 0;

    bool ishit = false;
    float hitDelay = 0;

    public Action<SkillData, SkillEffectData, CharacterHandler> OnHitCallBack;

    private void OnDestroy()
    {
        OnHitCallBack = null;
    }

    private void Update()
    {
        TestCode();

        BattleStage stage = GameManager.Instance.CurrentStage as BattleStage;

        int index = stage.GetCharacterList().FindIndex(x => x == this);

        spriteRenderer.sortingOrder = index;


        if (ishit)
        {
            spriteRenderer.color = Color.red;

            hitDelay += Time.deltaTime;

            if (hitDelay > 0.1f)
            {
                spriteRenderer.color = Color.white;
                ishit = false;
            }
        }
    }

    public void Initialize(CharacterData data, Vector3 postion, Vector3 dir, UIHpSlider slider)
    {
        this.data = data;

        GameObject obj = Resources.Load<GameObject>(PrafabPathInfo.GetPrefabPath(CharacterTableInfo.GetCharacterName(data.CharacterIndex)));
        GameObject character = Instantiate(obj, this.transform);

        character.gameObject.tag = this.gameObject.tag;
        this.gameObject.name = $"{data.CampType}_{data.CharacterName}";

        boxCollider = character.GetComponent<BoxCollider2D>();
        spriteRenderer = character.GetComponent<SpriteRenderer>();

        animComponent = this.GetComponent<AnimatorComponent>();
        animComponent.Initialize(character.GetComponent<Animator>());

        moveComponent = this.GetComponent<MoveComponent>();
        moveComponent.Initialize(this);

        aiComponent = this.GetComponent<AIComponent>();
        aiComponent.Initialize(this);

        this.slider = slider;

        this.transform.position = postion;
        moveComponent.LookUp(dir);
        Stop();

        this.gameObject.SetActive(true);
    }

    public void Move(Vector3 dir, float speed = 2f)
    {
        if (data.IsDead())
            return;

        animComponent.SetBool("IsRun", true);
        moveComponent.Move(dir, speed);
    }

    public void Move(Vector3 dir, Vector3 endPos, float speed = 2f)
    {
        if (data.IsDead())
            return;

        animComponent.SetBool("IsRun", true);
        moveComponent.Move(dir, endPos, speed);
    }

    public void KnockBack(Vector3 endPos, float speed)
    {
        if (data.IsDead())
            return;

        moveComponent.KnockBack(endPos, speed);
    }

    public void Stop()
    {
        if (data.IsDead())
            return;

        moveComponent.Stop();
        animComponent.SetBool("IsRun", false);
    }

    public void Attack(int skillIndex, bool isSkillButton = false)
    {
        if (data.IsDead())
            return;

        var skillData = SkillTableInfo.GetSkillData(skillIndex);
        if (skillData == null)
            return;

        if (animComponent.GetBool("IsSkillButton"))
        {
            animComponent.SetBool("IsAttack", true);
            return;
        }
            

        if (aiComponent.CheckSkillRange(skillData, out Vector3 endPos) == false)
        {
            UIPanelToast.ShowMessage("사거리가 부족합니다.");
            return;
        }

        if (data.IsCoolTime(skillData.skillIndex) == true)
        {
            UIPanelToast.ShowMessage("쿨타임이 돌아오지 않았습니다.");
            return;
        }

        Stop();

        data.SetSkillCoolTime(skillIndex, skillData.skillCoolTime);

        animComponent.SetBool("IsAttack", true);
        animComponent.SetBool("IsSkillButton", isSkillButton);
        animComponent.SetInt("Attack", skillIndex);
        animComponent.SetBool("IsRun", false);
    }

    public void OnIdle()
    {
        if (data.IsDead())
            return;

        if (animComponent.animator.GetBool("IsAttack") == true)
            return;

        Stop();

        animComponent.SetBool("IsAttack", false);
        animComponent.SetInt("Attack", 0);
        animComponent.SetBool("IsRun", false);
    }


    public void OnHit()
    {
        animComponent.SetBool("Hit", true);

        ishit = true;
        hitDelay = 0;
    }

    public void OnDie()
    {
        boxCollider.enabled = false;
        this.slider.gameObject.SetActive(false);

        Stop();

        animComponent.SetBool("Die", true);
    }


    public void EventCreateHitBox(int skillDataIndex, int skillEffectIndex)
    {
        SkillEffectData skillEffectData = SkillEffectTableInfo.GetSkillEffectData(skillEffectIndex);
        SkillData skillData = SkillTableInfo.GetSkillData(skillDataIndex);

        var entity = PoolManager.Instance.Create(PoolEntity.PoolType.AttackBox, Vector3.zero, this.gameObject.tag, this.transform);

        HitBox box = entity as HitBox;
        box.SetAttribute(skillData, skillEffectData);
    }

    public void EventTargetAttack(int skillDataIndex, int skillEffectIndex)
    {
        if (this.aiComponent.TargetHandler != null)
        {
            SkillData skillData = SkillTableInfo.GetSkillData(skillDataIndex);
            SkillEffectData skillEffectData = SkillEffectTableInfo.GetSkillEffectData(skillEffectIndex);

            OnHitCallBack.Invoke(skillData, skillEffectData, this.aiComponent.TargetHandler);
        }
    }

    public void EventTargetDash(int skillDataIndex, int skillEffectIndex)
    {
        if (this.aiComponent.TargetHandler != null)
        {
            SkillData skillData = SkillTableInfo.GetSkillData(skillDataIndex);
            SkillEffectData skillEffectData = SkillEffectTableInfo.GetSkillEffectData(skillEffectIndex);

            Vector3 dir = (this.aiComponent.TargetHandler.transform.position - this.transform.position).normalized;
            
            moveComponent.Dash(this.aiComponent.TargetHandler.transform.position + (-dir), skillEffectData.paramsValue[0]);
        }
    }

    public void EventTargetTracking(int skillDataIndex, int skillEffectIndex)
    {
        if (this.aiComponent.TargetHandler != null)
        {
            CharacterHandler handler = 
                aiComponent.SpecialTargetHandler != null && aiComponent.SpecialTargetHandler.data.IsDead() == false ? 
                aiComponent.SpecialTargetHandler : aiComponent.SearchEnemy(false);

            this.aiComponent.SetSpecialTarget(handler);

            //바라보는 반대방향으로 이동
            Vector3 moveDir = (handler.transform.position - this.transform.position).normalized;
            Vector3 endPos = handler.transform.position + (handler.moveComponent.GetLookDir() * -1);
            endPos.y = handler.transform.position.y;

            Vector3 lookDir = handler.moveComponent.GetLookDir().x > 0 ? Vector3.right : Vector3.left;

            moveComponent.SetPos(endPos, moveDir);
            moveComponent.LookUp(lookDir);
        }
    }

    public void EventSummon(int skillDataIndex, int skillEffectIndex)
    {
        if (this.aiComponent.TargetHandler != null)
        {
            SkillData skillData = SkillTableInfo.GetSkillData(skillDataIndex);
            SkillEffectData skillEffectData = SkillEffectTableInfo.GetSkillEffectData(skillEffectIndex);

            var entity = PoolManager.Instance.Create(PoolEntity.PoolType.Summon, skillEffectData.offsetPos, this.gameObject.tag);

            Summon summon = entity as Summon;
            summon.SetAttribute(skillData, skillEffectData, this, this.aiComponent.TargetHandler, Vector3.zero);
        }
    }

    public void EventCreateProjectile(int skillDataIndex, int skillEffectIndex)
    {
        SkillData skillData = SkillTableInfo.GetSkillData(skillDataIndex);
        SkillEffectData skillEffectData = SkillEffectTableInfo.GetSkillEffectData(skillEffectIndex);

        var entity = PoolManager.Instance.Create(PoolEntity.PoolType.Projectile, this.transform.position + data.ProjectileStartPos, this.gameObject.tag);

        Projectile projectile = entity as Projectile;
        projectile.SetAttribute(skillData, skillEffectData, this.aiComponent.TargetHandler, moveComponent.GetLookDir());
    }

    public void EventCreateChanneling(int skillDataIndex, int skillEffectIndex)
    {
        SkillData skillData = SkillTableInfo.GetSkillData(skillDataIndex);
        SkillEffectData skillEffectData = SkillEffectTableInfo.GetSkillEffectData(skillEffectIndex);

        var entity = PoolManager.Instance.Create(PoolEntity.PoolType.Channeling, Vector3.zero, this.gameObject.tag, this.transform);

        Channeling channeling = entity as Channeling;
        channeling.SetAttribute(skillData, skillEffectData);
    }


    private void TestCode()
    {
#if UNITY_EDITOR
        if (this.data.CampType == Enum.CampEnum.Enemy)
            return;

        if (Input.GetKeyDown(KeyCode.A))
            Move(Vector3.zero, Vector3.zero);

        if (Input.GetKeyDown(KeyCode.D))
            Move(Vector3.right);

        if (Input.GetKeyDown(KeyCode.F))
            Stop();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            animComponent.SetBool("IsAttack", true);
            animComponent.SetInt("Attack", 60000);
            animComponent.SetBool("IsRun", false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animComponent.SetBool("IsAttack", true);
            animComponent.SetInt("Attack", 60001);
            animComponent.SetBool("IsRun", false);
        }
#endif
    }
}
