using Interface;
using Player;
using Table;
using UnityEditor.UI;
using UnityEngine;

namespace Controller.Item
{
    public class BaseItem : BaseController, ICarriable, IInteractable
    {
        enum ItemState
        {
            DEFAULT,
            CARRY,
            DISABLED,
        }

        private ItemState _itemState = ItemState.DEFAULT;
        private Collider _collider;
        public int itemID { get; private set; }
        public ItemData itemData { get; private set; }
        public int leftGoodNum { get; private set; }
        private PlayerController _carryPlayer;
        
        public BaseItem(GameObject gameObject, int itemID) : base(gameObject)
        {
            _collider = base.transform.GetComponent<Collider>();
            this.itemID = itemID;
            this.itemData = ItemTable.Get().GetItemData(itemID);
            leftGoodNum = itemData.goodNum;
        }
        
        public virtual bool Carry(PlayerController playerController)
        {
            if (CanCarry(playerController))
            {
                _itemState = ItemState.CARRY;
                _collider.enabled = false;
                _carryPlayer = playerController;
                return true;
            }

            return false;
        }

        public virtual bool Drop(PlayerController playerController)
        {
            if (CanDrop(playerController))
            {
                _itemState = ItemState.DEFAULT;
                _collider.enabled = true;
                return true;
            }
            return false;
        }

        public virtual bool CanCarry(PlayerController playerController)
        {
            if (!Utils.IsNull(playerController.GetCarryItem()))
            {
                return false;
            }
            if (_itemState == ItemState.DEFAULT)
            {
                return true;
            }

            return false;
        }

        public virtual bool CanDrop(PlayerController playerController)
        {
            if (playerController != _carryPlayer)
            {
                return false;
            }
            if (_itemState == ItemState.CARRY)
            {
                return true;
            }

            return false;
        }

        public void SubGoodNum(int num)
        {
            leftGoodNum -= num;
            LogManager.Log("消耗", num, "剩余", leftGoodNum);
            if (leftGoodNum <= 0)
            {
                Dispose();
            }
        }

        public bool Interact(PlayerController playerController)
        {
            if (_itemState == ItemState.DEFAULT)
            {
                return Carry(playerController);
            }
            if (_itemState == ItemState.CARRY)
            {
                return Drop(playerController);
            }

            return false;
        }

        public bool CanInteract(PlayerController playerController)
        {
            if (_itemState == ItemState.DEFAULT)
            {
                return CanCarry(playerController);
            }
            if (_itemState == ItemState.CARRY)
            {
                return CanDrop(playerController);
            }

            return false;
        }
    }
}