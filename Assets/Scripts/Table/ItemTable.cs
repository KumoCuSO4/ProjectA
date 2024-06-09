using System.Collections.Generic;

namespace Table
{
    public class ItemTable : Singleton<ItemTable>
    {
        private Dictionary<int, ItemData> data = new Dictionary<int, ItemData>();

        public ItemTable()
        {
            data[1] = new ItemData(null, "item_1", 10, 1, 20);
        }

        public ItemData GetItemData(int itemID)
        {
            return data[itemID];
        }
        
    }

    public record ItemData(string className, string prefabName, int value, int goodType = 0, int goodNum = 0);
}