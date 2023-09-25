using UnityEngine;

namespace Data
{
    public class SummonData
    {
        public int summonId;

        public Vector3 offsetPos;
        public Vector3 size;

        public string summonName;

        public float[] paramsValue;
    }

    static public class SummonTableInfo
    {
        static public SummonData GetSummonData(int summonIndex)
        {
            SummonData data = null;

            switch (summonIndex)
            {

                case 400011:
                    {
                        data = new SummonData();

                        data.summonId = summonIndex;
                        data.summonName = "DevilArm";

                        data.offsetPos = new Vector3(0, 0, 0);
                        data.size = new Vector2(1.2f, 0.7f);
                    }
                    break;

            }

            return data;
        }
    }
}