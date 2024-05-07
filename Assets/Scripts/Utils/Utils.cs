using Controller;

namespace Utils
{
    public static class Utils
    {
        public static PlayerController GetLocalPlayer()
        {
            return PlayerManager.Get().GetLocalPlayer();
        }
    }
}