namespace Data
{
    static public class TagLayerTableInfo
    {
        static public int GetTagLayerData(string tagType)
        {
            switch (tagType)
            {
                case "Hero": return 6;
                case "Enemy": return 7;
            }

            return -1;
        }
    }
}
