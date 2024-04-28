using System.Collections.Generic;
using Controller.Item;
using Table;
using Utils;
using System;
using Controller.Window;
using UnityEngine;
using Object = UnityEngine.Object;

public class ItemManager : Singleton<ItemManager>
{
    private List<BaseItem> _items = new List<BaseItem>();
    private Transform itemRoot;
    
    public ItemManager()
    {
        itemRoot = new GameObject("item_root").transform;
    }

    public BaseItem CreateItem(int itemID)
    {
        ItemData itemData = ItemTable.Get().GetItemData(itemID);
        string className = itemData.className;
        Type type = Type.GetType("Controller.Item." + className);
        if (type == null)
        {
            LogManager.LogError("物品类不存在", className);
            return null;
        }
        
        GameObject windowPrefab = Resources.Load<GameObject>("Prefabs/Item/" + itemData.prefabName);
        GameObject obj = Object.Instantiate(windowPrefab, itemRoot);
        obj.transform.localPosition = new Vector3(0, 2, 0);
        object[] constructorArgs = { obj };
        BaseItem item = Activator.CreateInstance(type, constructorArgs) as BaseItem;
        _items.Add(item);
        return item;
    }
}