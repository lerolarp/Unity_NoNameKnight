using Enum;
using System.Collections.Generic;

namespace Data
{
    public class UserData
    {
        //private string userName = null;
        private List<CharacterData> characterList = new List<CharacterData>();
        public List<CharacterData> CharacterList => characterList;

        private Dictionary<BattleLineType, List<CharacterData>> battleDic = new Dictionary<BattleLineType, List<CharacterData>>();
        public Dictionary<BattleLineType, List<CharacterData>> BattleDic => battleDic;

        public void Intialize(int[] characterArray)
        {
            characterList.Clear();

            for(int i = 0; i < characterArray.Length; ++i)
            {
                if (characterArray[i] == 0)
                    continue;

                CharacterData data = new CharacterData(CharacterTableInfo.GetCharacterData(characterArray[i]));
                data.SetCampType(CampEnum.Hero);
                data.SetUID(1000 + i);

                characterList.Add(data);
            }

            battleDic.Add(BattleLineType.Front, new List<CharacterData>());
            battleDic.Add(BattleLineType.Middle, new List<CharacterData>());
            battleDic.Add(BattleLineType.Back, new List<CharacterData>());
        }

        public void SetBattleLine(BattleLineType battleLine, int[] array)
        {
            BattleDic[battleLine].Clear();

            for(int i = 0; i < array.Length; ++i)
            {
                battleDic[battleLine].Add(characterList.Find(x => x.CharacterIndex == array[i]));
            }
        }

        public void ResetHeroHP()
        {
            for(int i = 0; i < characterList.Count; ++i)
            {
                characterList[i].SetHP(characterList[i].MaxHp);
            }
        }

        public void Dispose()
        {
            characterList.Clear();

            foreach(var lineInfo in battleDic)
            {
                for (int i = 0; i < lineInfo.Value.Count; ++i)
                    lineInfo.Value[i] = null;

                lineInfo.Value.Clear();
            }

            battleDic.Clear();
        }

        public List<CharacterData> GetBattleCharList()
        {
            List<CharacterData> battleList = new List<CharacterData>();
            foreach (var battleData in battleDic)
            {
                battleList.AddRange(battleData.Value);
            }

            return battleList;
        }

        public bool IsRegist(int characterUid)
        {
            foreach (var line in battleDic)
            {
                if (line.Value.Find(x => x.UID == characterUid) != null)
                    return true;
            }

            return false;
        }

        public bool GetBattleLine(int characterUid, out BattleLineType outLine)
        {
            outLine = BattleLineType.Front;

            foreach (var line in battleDic)
            {
                if (line.Value.Find(x => x.UID == characterUid) != null)
                {
                    outLine = line.Key;
                    return true;
                }
            }

            return false;
        }

        public void UnRegistCharacter(int characterUid)
        {
            CharacterData data = CharacterList.Find(x => x.UID == characterUid);

            foreach (var line in battleDic)
            {
                if (line.Value.Remove(data))
                    break;
            }
        }

        public void RegistCharacter(int characterUid)
        {
            CharacterData data = CharacterList.Find(x => x.UID == characterUid);
            
            BattleLineType line = data.defaultLine;
            if(battleDic[line].Count < 3)
                battleDic[line].Add(data);
            else
            {
                foreach(var typeList in battleDic)
                {
                    if (typeList.Value.Count < 3)
                    {
                        battleDic[typeList.Key].Add(data);
                        break;
                    }
                }
            }
        }

        public void SwapCharacter(CharacterData a, CharacterData b)
        {
            BattleLineType aLine = BattleLineType.None;
            int aIndex = -1;

            BattleLineType bLine = BattleLineType.None;
            int bIndex = -1;

            foreach (var lineInfo in battleDic)
            {
                for(int i = 0; i < lineInfo.Value.Count; ++i)
                {
                    if(lineInfo.Value[i].UID == a.UID)
                    {
                        aLine = lineInfo.Key;
                        aIndex = i;
                    }

                    if (lineInfo.Value[i].UID == b.UID)
                    {
                        bLine = lineInfo.Key;
                        bIndex = i;
                    }
                }
            }

            CharacterData temp = battleDic[aLine][aIndex];
            battleDic[aLine][aIndex] = battleDic[bLine][bIndex];
            battleDic[bLine][bIndex] = temp;

        }

        public void RegistCharacter(int characterUid, BattleLineType type)
        {
            CharacterData data = CharacterList.Find(x => x.UID == characterUid);

            battleDic[type].Add(data);
        }

        public int GetBattleHeroCount()
        {
            int count = 0;
            foreach (var line in battleDic)
            {
                count += line.Value.Count;
            }

            return count;
        }
    }
}
