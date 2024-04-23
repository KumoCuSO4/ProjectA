using UnityEngine;
using Utils;

namespace Controller.Item
{
    public abstract class BaseItem : BaseController, IInteractable
    {
        public BaseItem(Transform transform) : base(transform)
        {
        }

        public virtual bool Interact(PlayerController playerController)
        {
            return false;
        }
    }
}