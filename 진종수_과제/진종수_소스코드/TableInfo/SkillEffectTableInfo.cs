using Enum;
using UnityEngine;

namespace Data
{
    public class SkillEffectData
    {
        public int effectId;
        public SkillEffectType effectType;

        public Vector3 offsetPos;
        public Vector3 size;
        public float lifeTime;

        public int skillDamage;

        public float[] paramsValue;
    }

    static public class SkillEffectTableInfo
    {
        static public SkillEffectData GetSkillEffectData(int skillEffectIndex)
        {
            SkillEffectData data;

            switch (skillEffectIndex)
            {
                case 100011:
                    {
                        data = new SkillEffectData();
                        data.effectId = 100011;
                        data.skillDamage = 30;
                        data.effectType = SkillEffectType.Target;
                    }
                    break;
                case 100012:
                    {
                        data = new SkillEffectData();
                        data.effectId = 100012;
                        data.skillDamage = 30;
                        data.effectType = SkillEffectType.KnockBack;
                        data.paramsValue = new float[] { 1f, 10f };
                    }
                    break;

                case 200001:
                    {
                        data = new SkillEffectData();
                        data.effectId = 200001;
                        data.skillDamage = 30;
                        data.lifeTime = 5.0f;
                        data.effectType = SkillEffectType.Projectile;
                    }
                    break;

                case 200011:
                    {
                        data = new SkillEffectData();
                        data.effectId = 200011;
                        data.skillDamage = 5;
                        data.lifeTime = 0.5f;
                        data.effectType = SkillEffectType.Channeling;

                        data.offsetPos = new Vector3(1.5f, 1f, 0);
                        data.size = new Vector2(4f, 0.7f);
                        data.paramsValue = new float[] { 0.2f };
                    }
                    break;

                case 300001:
                    {
                        data = new SkillEffectData();
                        data.effectId = 300001;
                        data.skillDamage = 10;
                        data.effectType = SkillEffectType.Target;
                    }
                    break;

                case 300011:
                    {
                        data = new SkillEffectData();
                        data.effectId = 300011;
                        data.effectType = SkillEffectType.TargetTracking;
                    }
                    break;

                case 300012:
                    {
                        data = new SkillEffectData();
                        data.effectId = 300012;
                        data.skillDamage = 20;
                        data.effectType = SkillEffectType.Target;
                    }
                    break;

                case 400001:
                    {
                        data = new SkillEffectData();
                        data.effectId = 400001;
                        data.skillDamage = 30;
                        data.effectType = SkillEffectType.Target;
                    }
                    break;

                case 400011:
                    {
                        data = new SkillEffectData();
                        data.effectId = 400011;
                        data.skillDamage = 10;
                        data.lifeTime = 1.5f;
                        data.effectType = SkillEffectType.Summon;

                        data.offsetPos = new Vector3(1.5f, 1f, 0);
                        data.size = new Vector2(4f, 0.7f);
                        data.paramsValue = new float[] { 0.2f };
                    }
                    break;

                case 500011:
                    {
                        data = new SkillEffectData();
                        data.effectId = 500011;
                        data.skillDamage = 30;
                        data.effectType = SkillEffectType.Dash;
                        data.paramsValue = new float[] { 10f };
                    }
                    break;

                case 500012:
                    {
                        data = new SkillEffectData();
                        data.effectId = 500012;
                        data.lifeTime = 0.2f;
                        data.skillDamage = 30;
                        data.effectType = SkillEffectType.Target;
                    }
                    break;

                case 1000001:
                    {
                        data = new SkillEffectData();
                        data.effectId = 1000001;

                        data.skillDamage = 10;
                        data.effectType = SkillEffectType.Target;
                    }
                    break;

                case 600001:
                    {
                        data = new SkillEffectData();
                        data.effectId = 600001;
                        data.skillDamage = 30;
                        data.effectType = SkillEffectType.Target;
                    }
                    break;

                case 600011:
                    {
                        data = new SkillEffectData();
                        data.effectId = 600012;
                        data.skillDamage = 30;
                        data.effectType = SkillEffectType.KnockBack;
                        data.paramsValue = new float[] { 2f, 10f };
                    }
                    break;

                default:
                    {
                        data = new SkillEffectData();
                        //data.effectId = skillEffectIndex;

                        data.skillDamage = 10;
                        data.effectType = SkillEffectType.Target;

                        //광역스킬용
                        data.lifeTime = 0.2f;
                        data.offsetPos = new Vector3(1.5f, 1.5f, 0);
                        data.size = new Vector2(1f, 1f);
                    }
                    break;
            }

            return data;
        }
    }
}