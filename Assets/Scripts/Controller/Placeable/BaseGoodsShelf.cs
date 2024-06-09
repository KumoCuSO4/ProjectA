using System;
using System.Collections.Generic;
using Controller.Item;
using Interface;
using Player;
using UnityEngine;

namespace Controller.Placeable
{
    public class BaseGoodsShelf : BasePlaceable, IInteractable
    {
        private int _leftNum = 0;
        private int _maxNum = 50;
        private int _curGoodType = 0;
        private List<int> _enabledGoodTypes = new List<int>();

        public BaseGoodsShelf(GameObject gameObject, int tableID, PlaceGridController placeGrid) : base(gameObject, tableID, placeGrid)
        {
            _enabledGoodTypes.Add(1);
        }

        public int FillItem(int goodType, int num)
        {
            int emptyNum = _maxNum - _leftNum;
            int fillNum = Math.Min(num, emptyNum);
            _leftNum += fillNum;
            if (_curGoodType == -1 && _leftNum > 0) _curGoodType = goodType;
            return fillNum;
        }

        public bool Interact(PlayerController playerController)
        {
            BaseItem item = playerController.GetCarryItem();
            int num = FillItem(item.itemData.goodType, item.leftGoodNum);
            item.SubGoodNum(num);
            LogManager.Log("补货", num, "现在有", _leftNum);
            return true;
        }

        public bool CanInteract(PlayerController playerController)
        {
            BaseItem item = playerController.GetCarryItem();
            if (Utils.IsNull(item))
            {
                return false;
            }
            int goodType = item.itemData.goodType;
            if (goodType == 0) return false;
            int num = item.leftGoodNum;
            if (num <= 0) return false;
            if (_curGoodType != 0 && _leftNum > 0 && goodType != _curGoodType) return false;
            if (!_enabledGoodTypes.Contains(goodType)) return false;
            if (_leftNum >= _maxNum) return false;
            return true;
        }
    }
}