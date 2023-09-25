using Enum;
using UnityEngine;


namespace Data
{
    public class ProjectileData
    {
        public int projectileId;
        public ProjectileType projectileType;

        public Vector3 offsetPos;
        public Vector3 size;

        public float speed;
        public float lifeTime;

        public int skillDamage;

        public string projectileName;

        public float[] paramsValue;
    }

    static public class ProjectileTableInfo
    {
        static public ProjectileData GetProjectileData(int projectileIndex)
        {
            ProjectileData data = null;

            switch (projectileIndex)
            {

                case 200001:
                    {
                        data = new ProjectileData();

                        data.projectileId = projectileIndex;
                        data.skillDamage = 30;

                        data.projectileType = ProjectileType.FireBall_Large;
                        data.projectileName = "FireBall_Large";

                        data.offsetPos = new Vector3(0, 0, 0);
                        data.size = new Vector2(1.2f, 0.7f);
                        data.speed = 5.0f;
                    }
                    break;

            }

            return data;
        }
    }
}