namespace Utils
{
    public abstract class Singleton<T> where T:new()
    {
        public static T instance = new T();
        public static T Get()
        {
            return instance;
        }
    }
}