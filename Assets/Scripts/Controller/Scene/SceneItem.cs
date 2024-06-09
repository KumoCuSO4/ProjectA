using System;
using System.Collections.Generic;
using Controller.Item;
using Table;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controller.Scene
{
    public class SceneItem : BaseController
    {
        private List<BaseItem> _items = new List<BaseItem>();
        public SceneItem(GameObject gameObject) : base(gameObject)
        {
        }
        
        public BaseItem CreateItem(int itemID)
        {
            ItemData itemData = ItemTable.Get().GetItemData(itemID);
            string className = itemData.className;
            if (className == null)
            {
                className = "BaseItem";
            }
            Type type = Type.GetType("Controller.Item." + className);
            if (type == null)
            {
                LogManager.LogError("物品类不存在", className);
                return null;
            }
        
            GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item/" + itemData.prefabName);
            GameObject obj = Utils.AddChild(gameObject, itemPrefab);
            obj.transform.localPosition = new Vector3(0, 2, 0);
            object[] constructorArgs = {obj, itemID};
            BaseItem item = Activator.CreateInstance(type, constructorArgs) as BaseItem;
            _items.Add(item);
            return item;
        }

        public override void Dispose()
        {
            foreach (var item in _items)
            {
                item.Dispose();
            }
            base.Dispose();
        }
    }
}