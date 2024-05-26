using Controller;
using Player;

namespace Interface
{
    public interface ICarriable
    {
        public bool Carry(PlayerController playerController);
        
        public bool Drop(PlayerController playerController);
        
        public bool CanCarry(PlayerController playerController);
        
        public bool CanDrop(PlayerController playerController);
    }
}