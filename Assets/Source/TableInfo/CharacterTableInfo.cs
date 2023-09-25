using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Data
{
    static public class CharacterTableInfo
    {
        static public string GetCharacterName(int characterIndex)
        {
            switch (characterIndex)
            {
                case 1000: return "Knight";
                case 2000: return "FireWizard";
                case 3000: return "Assassin";
                case 4000: return "Bringer";
                case 5000: return "Skeleton";
                case 6000: return "Samurai";
            }

            return null;
        }

        static public CharacterData GetCharacterData(int characterIndex)
        {
            switch (characterIndex)
            {
                case 1000: return new CharacterData(characterIndex, 1, 300, 10, 1, Vector3.zero, Enum.BattleLineType.Front);
                case 2000: return new CharacterData(characterIndex, 1, 300, 10, 1, new Vector3(0, 1.28f, 0), Enum.BattleLineType.Back);
                case 3000: return new CharacterData(characterIndex, 1, 300, 10, 1, Vector3.zero, Enum.BattleLineType.Middle);
                case 4000: return new CharacterData(characterIndex, 1, 300, 10, 1, Vector3.zero, Enum.BattleLineType.Back);
                case 5000: return new CharacterData(characterIndex, 1, 300, 10, 1, Vector3.zero, Enum.BattleLineType.Front);
                case 6000: return new CharacterData(characterIndex, 1, 300, 10, 1, Vector3.zero, Enum.BattleLineType.Middle);
            }

            return null;
        }

       

    }
}
