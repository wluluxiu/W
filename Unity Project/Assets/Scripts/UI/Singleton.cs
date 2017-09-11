/// <summary>
/// 单例
/// by TT
/// 2016-06-17
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : class, new()
{
    private static T instance;
    private static readonly object sysLock = new object();

    public static T Instance()
    {
        if (instance == null)
        {
            lock (sysLock)
            {
                if (instance == null)
                {
                    instance = new T();
                }
            }
        }
        return instance;
    }

    protected void OnQuit()
    {
        instance = null;
    }
}