using System.Collections.Generic;

namespace Table
{
    public class PlaceableTable : Singleton<PlaceableTable>
    {
        private Dictionary<int, PlaceableData> data = new Dictionary<int, PlaceableData>();

        public PlaceableTable()
        {
            data[1] = new PlaceableData(null, "placeable_1", 3, 2);
            data[2] = new PlaceableData(null, "cashier", 2, 2);
            data[3] = new PlaceableData(null, "goods_shelf", 3, 1);
        }

        public PlaceableData GetPlaceableData(int placeableID)
        {
            return data[placeableID];
        }
        
    }

    public class PlaceableData {
        public string className { private set; get; }
        public string prefabName { private set; get; }
        public int length { private set; get; }
        public int width { private set; get; }
        

        public PlaceableData(string className, string prefabName, int length, int width)
        {
            this.className = className;
            this.prefabName = prefabName;
            this.length = length;
            this.width = width;
        }
    }
}