namespace Data
{
    static public class PrafabPathInfo
    {
        static public string GetPrefabPath(string prafabName)
        {
            switch (prafabName)
            {
                //Handler
                case "Character_Hendler":
                    return "Prefabs/Character/CharacterBase";

                //Character
                case "Knight":
                    return "Prefabs/Character/Knight";
                case "FireWizard":
                    return "Prefabs/Character/FireWizard";
                case "Assassin":
                    return "Prefabs/Character/Assassin";
                case "Bringer":
                    return "Prefabs/Character/Bringer";
                case "Skeleton":
                    return "Prefabs/Character/Skeleton";
                case "Samurai":
                    return "Prefabs/Character/Samurai";


                //Battle
                case "AttackBox":
                    return "Prefabs/PoolPrefab/AttackBox";
                case "Projectile":
                    return "Prefabs/PoolPrefab/Projectile_Base";
                case "Channeling":
                    return "Prefabs/PoolPrefab/Channeling";
                case "DevilArm":
                    return "Prefabs/Summon/DevilArm";


                case "Summon":
                    return "Prefabs/PoolPrefab/Summon_Base";


                //UI
                case "UIFloatingDamage":
                    return "Prefabs/PoolPrefab/UIFloatingDamage";
                case "HPSlider":
                    return "Prefabs/UI/HPSlider";


                //Projectile
                case "FireBall_Large":
                    return "Prefabs/Projectile/FireBall_Large";

            }

            return null;
        }


    }
}
