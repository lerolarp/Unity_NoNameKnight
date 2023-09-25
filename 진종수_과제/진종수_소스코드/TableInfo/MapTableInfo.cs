using Enum;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    static public class MapTableInfo
    {
        static public Dictionary<BattleLineType, List<int>> GetMapSpawner(int mapIndex)
        {
            Dictionary<BattleLineType, List<int>> returnDic = new Dictionary<BattleLineType, List<int>>();
            returnDic.Add(BattleLineType.Front, new List<int>());
            returnDic.Add(BattleLineType.Middle, new List<int>());
            returnDic.Add(BattleLineType.Back, new List<int>());

            switch (mapIndex)
            {
                case 0:
                    {
                        returnDic[BattleLineType.Front].AddRange(new int[]{ 1000 });
                        returnDic[BattleLineType.Middle].AddRange(new int[] { 2000 });
                        returnDic[BattleLineType.Back].AddRange(new int[] { 3000 });
                    }
                    break;
                case 1:
                    {
                        returnDic[BattleLineType.Front].AddRange(new int[] { 3000 });
                        returnDic[BattleLineType.Middle].AddRange(new int[] { 4000 });
                        returnDic[BattleLineType.Back].AddRange(new int[] { 6000 });
                    }
                    break;
                case 2:
                    {
                        returnDic[BattleLineType.Front].AddRange(new int[] { 3000, 1000, 3000 });
                        returnDic[BattleLineType.Middle].AddRange(new int[] { 4000, 1000 });
                        returnDic[BattleLineType.Back].AddRange(new int[] { 6000, 2000, 5000 });
                    }
                    break;
                default:
                    return null;
            }

            return returnDic;
        }

        static public GameObject GetMapPrefab(int mapIndex)
        {
            switch (mapIndex)
            {
                case 0:
                case 1:
                case 2:
                    {
                        return Resources.Load<GameObject>("Prefabs/Map/Map_1");
                    }
            }

            return null;
        }
    }
}
