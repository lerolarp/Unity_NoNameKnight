using Enum;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class CharacterData
    {
        private CampEnum campType;
        public CampEnum CampType => campType;

        private int uid;
        public int UID => uid;

        private int characterIndex;
        public int CharacterIndex => characterIndex;

        public string CharacterName => CharacterTableInfo.GetCharacterName(characterIndex);

        private int level;
        public int Level => level;

        private int maxHp;
        public int MaxHp => maxHp;

        private int hp;
        public int Hp => hp;

        private Dictionary<int, float> skillCoolTimeDic = new Dictionary<int, float>();
        public Dictionary<int, float> SkillCoolTimeDic => skillCoolTimeDic;

        private int attackPoint;
        public int AttackPoint => attackPoint;


        private int defensePoint;
        public int DefensePoint => defensePoint;

        private Vector3 projectileStartPos;
        public Vector3 ProjectileStartPos => projectileStartPos;

        public BattleLineType defaultLine = BattleLineType.Front;

        public CharacterData(int index, int level, int maxHp, int attackPoint, int defensePoint, Vector3 projectilePos, BattleLineType lineType)
        {
            this.characterIndex = index;

            this.level = level;
            this.maxHp = maxHp;
            this.hp = maxHp;

            this.attackPoint = attackPoint;
            this.defensePoint = defensePoint;

            this.projectileStartPos = projectilePos;

            this.defaultLine = lineType;
        }

        public CharacterData(CharacterData data)
        {
            uid = data.uid;
            campType = data.CampType;

            characterIndex = data.CharacterIndex;

            level = data.Level;
            maxHp = data.MaxHp;
            hp = data.MaxHp;

            attackPoint = data.AttackPoint;
            defensePoint = data.DefensePoint;

            projectileStartPos = data.ProjectileStartPos;

            defaultLine = data.defaultLine;
        }

        public void SetUID(int uid)
        {
            this.uid = uid;

        }

        public void SetHP(int hp)
        {
            this.hp = hp;
          
        }

        public void SetCampType(CampEnum campType)
        {
            this.campType = campType;

        }

        public bool IsDead()
        {
            return hp <= 0;
        }

        public void SetSkillMaxCoolTime()
        {
            int[] skillArray = SkillTableInfo.GetSkillList(characterIndex);
            for (int i = 0; i < skillArray.Length; ++i)
            {
                SkillData skillData = SkillTableInfo.GetSkillData(skillArray[i]);
                if(skillData.skillType == SkillType.Special)
                {
                    SetSkillCoolTime(skillData.skillIndex, skillData.skillCoolTime);
                }
            }
        }

        public void SetSkillCoolTime(int skillIndex, float coolTimeAt)
        {
            if(skillCoolTimeDic.ContainsKey(skillIndex) == false)
                skillCoolTimeDic.Add(skillIndex, coolTimeAt);
            else
                skillCoolTimeDic[skillIndex] = coolTimeAt;
        }

        public float GetSkillCoolTime(int skillIndex)
        {
            if (skillCoolTimeDic.ContainsKey(skillIndex) == false)
                return -1;
            else
                return skillCoolTimeDic[skillIndex];
        }

        public bool IsCoolTime(int skillIndex)
        {
            if (skillCoolTimeDic.ContainsKey(skillIndex))
                return 0 < skillCoolTimeDic[skillIndex];

            return false;
        }

        public bool IsAutoUseSkill(int skillIndex)
        {
            if (IsCoolTime(skillIndex) == true)
                return false;

            if (skillCoolTimeDic.ContainsKey(skillIndex))
                return 0 <= skillCoolTimeDic[skillIndex];

            return true;
        }
    }
}
