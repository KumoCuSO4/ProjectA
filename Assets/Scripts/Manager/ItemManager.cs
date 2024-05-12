using System.Collections.Generic;
using Controller.Item;
using Table;
using System;
using Controller.Scene;
using JetBrains.Annotations;
using Manager;
using UnityEngine;
using Object = UnityEngine.Object;

public class ItemManager : Singleton<ItemManager>
{
    private List<int> _itemIDs = new List<int>();

    public ItemManager()
    {
        
    }

    [CanBeNull]
    public BaseItem CreateItem(int itemID)
    {
        _itemIDs.Add(itemID);
        BaseScene curScene = SceneManager.Get().GetCurScene();
        return curScene?.CreateItem(itemID);
    }
}