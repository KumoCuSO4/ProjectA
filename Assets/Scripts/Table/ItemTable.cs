using System.Collections.Generic;
using Utils;

namespace Table
{
    public class ItemTable : Singleton<ItemManager>
    {
        private Dictionary<int, ItemData> data = new Dictionary<int, ItemData>();

        public ItemTable()
        {
            data[1] = new ItemData("Item1", 10);
        }

        public ItemData GetItemData(int itemID)
        {
            return data[itemID];
        }
    }

    public class ItemData
    {
        public string name { private set; get; }
        public int value { private set; get; }

        public ItemData(string name, int value)
        {
            this.name = name;
            this.value = value;
        } 
    }
}