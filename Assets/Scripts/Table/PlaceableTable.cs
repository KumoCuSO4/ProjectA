using System.Collections.Generic;

namespace Table
{
    public class PlaceableTable : Singleton<PlaceableTable>
    {
        private Dictionary<int, PlaceableData> data = new Dictionary<int, PlaceableData>();

        public PlaceableTable()
        {
            data[1] = new PlaceableData(null, "placeable_1", "测试", 3, 2);
            data[2] = new PlaceableData(null, "cashier", "收银台", 2, 2);
            data[3] = new PlaceableData(null, "goods_shelf","商品货架", 3, 1);
        }

        public PlaceableData GetPlaceableData(int placeableID)
        {
            return data[placeableID];
        }

        public Dictionary<int, PlaceableData> GetAllData()
        {
            return data;
        }
    }

    public record PlaceableData(string className, string prefabName, string name, int length, int width);
}