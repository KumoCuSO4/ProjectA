using UnityEngine;

namespace Controller.Item
{
    public class Item1 : BaseItem
    {
        public Item1(Transform transform) : base(transform)
        {
            
        }
        
        public override bool Interact(PlayerController playerController)
        {
            return true;
        }
    }
}