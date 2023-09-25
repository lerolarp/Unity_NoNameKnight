using Enum;

namespace Data
{
    public class SkillData
    {
        public SkillType skillType;
        public int skillIndex;

        public float skillRange;
        public float skillCoolTime;
    }

    static public class SkillTableInfo
    {
        static public SkillData GetSkillData(int skillIndex)
        {
            SkillData data = null;

            switch (skillIndex)
            {
                case 10000:
                    {
                        data = new SkillData();

                        data.skillType = SkillType.Normal;
                        data.skillRange = 2f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 0.0f;
                    }
                    break;

                case 10001:
                    {
                        data = new SkillData();
                        data.skillType = SkillType.Special;
                        data.skillRange = 2f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 3.0f;
                    }
                    break;

                case 20000:
                    {
                        data = new SkillData();

                        data.skillType = SkillType.Normal;
                        data.skillRange = 4f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 0.0f;
                    }
                    break;

                case 20001:
                    {
                        data = new SkillData();
                        data.skillType = SkillType.Special;
                        data.skillRange = 4f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 5.0f;
                    }
                    break;

                case 30000:
                    {
                        data = new SkillData();

                        data.skillType = SkillType.Normal;
                        data.skillRange = 2f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 0.0f;
                    }
                    break;

                case 30001:
                    {
                        data = new SkillData();
                        data.skillType = SkillType.Special;
                        data.skillRange = 999f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 4.0f;
                    }
                    break;

                case 40000:
                    {
                        data = new SkillData();

                        data.skillType = SkillType.Normal;
                        data.skillRange = 2f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 0.0f;
                    }
                    break;

                case 40001:
                    {
                        data = new SkillData();
                        data.skillType = SkillType.Special;
                        data.skillRange = 999f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 4.0f;
                    }
                    break;

                case 50000:
                    {
                        data = new SkillData();

                        data.skillType = SkillType.Normal;
                        data.skillRange = 2f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 0.0f;
                    }
                    break;

                case 50001:
                    {
                        data = new SkillData();
                        data.skillType = SkillType.Special;
                        data.skillRange = 3f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 5.0f;
                    }
                    break;

                case 60000:
                    {
                        data = new SkillData();

                        data.skillType = SkillType.Normal;
                        data.skillRange = 2f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 0.0f;
                    }
                    break;

                case 60001:
                    {
                        data = new SkillData();
                        data.skillType = SkillType.Special;
                        data.skillRange = 2f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 5.0f;
                    }
                    break;


                case 100000:
                    {
                        data = new SkillData();

                        data.skillType = SkillType.Normal;
                        data.skillRange = 2f;
                        data.skillIndex = skillIndex;
                        data.skillCoolTime = 0.0f;
                    }
                    break;
            }

            return data;
        }

        static public int[] GetSkillList(int characterID)
        {
            switch (characterID)
            {
                case 1000:
                    return new int[] { 10000, 10001 };

                case 2000:
                    return new int[] { 20000, 20001 };

                case 3000:
                    return new int[] { 30000, 30001 };

                case 4000:
                    return new int[] { 40000, 40001 };

                case 5000:
                    return new int[] { 50000, 50001 };

                case 6000:
                    return new int[] { 60000, 60001 };
            }

            return null;
        }
    }
}
