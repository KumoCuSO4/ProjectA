using Controller;

namespace Interface
{
    public interface IInteractable
    {
        public bool Interact(PlayerController playerController);
        
        public bool CanInteract(PlayerController playerController);
    }
}