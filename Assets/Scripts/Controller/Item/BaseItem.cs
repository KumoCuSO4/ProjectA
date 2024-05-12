using Interface;
using UnityEngine;

namespace Controller.Item
{
    public class BaseItem : BaseController, ICarriable
    {
        enum ItemState
        {
            DEFAULT,
            CARRY,
            DISABLED,
        }

        private ItemState _itemState = ItemState.DEFAULT;
        private Collider _collider;
        
        public BaseItem(GameObject gameObject) : base(gameObject)
        {
            _collider = base.transform.GetComponent<Collider>();
        }
        
        public virtual bool Carry(PlayerController playerController)
        {
            if (CanCarry(playerController))
            {
                _itemState = ItemState.CARRY;
                _collider.enabled = false;
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
            if (_itemState == ItemState.DEFAULT)
            {
                return true;
            }

            return false;
        }

        public virtual bool CanDrop(PlayerController playerController)
        {
            if (_itemState == ItemState.CARRY)
            {
                return true;
            }

            return false;
        }
    }
}