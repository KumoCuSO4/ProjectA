namespace Utils
{
    public abstract class Singleton<T> where T:new()
    {
        private static readonly T Instance = new T();
        public static T Get()
        {
            return Instance;
        }
    }
}