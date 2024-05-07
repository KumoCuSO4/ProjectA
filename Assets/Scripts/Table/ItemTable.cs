using System.Collections.Generic;
using Utils;

namespace Table
{
    public class ItemTable : Singleton<ItemTable>
    {
        private Dictionary<int, ItemData> data = new Dictionary<int, ItemData>();

        public ItemTable()
        {
            data[1] = new ItemData(null, "item_1", 10);
        }

        public ItemData GetItemData(int itemID)
        {
            return data[itemID];
        }
        
    }

    public class ItemData
    {
        public string className { private set; get; }
        public string prefabName { private set; get; }
        public int value { private set; get; }

        public ItemData(string className, string prefabName, int value)
        {
            this.className = className;
            this.prefabName = prefabName;
            this.value = value;
        } 
    }
}