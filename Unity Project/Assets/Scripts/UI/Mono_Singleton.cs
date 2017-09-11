using UnityEngine;

public abstract class Mono_Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly object temp = new object();

    private static T m_instance;

    public static bool m_ApplicationIsQuitting { get; private set; }

    protected virtual bool IsAutoCreateNew
    {
        get { return true; }
    }

    /// <summary>
    ///     get the instance of this type monobehaviour, will create a component attached to DontDestroyOnLoad if instance is
    ///     null
    /// </summary>
    public static T Instance
    {
        get
        {
            var editMode = Application.isEditor && !Application.isPlaying;
            if (!editMode && m_ApplicationIsQuitting)
            {
				Debug.Log("[Singleton] Instance:" + typeof(T) +
                                     " already destroyed on application quit." +
                                     " Won't create again - returning null.");
                return null;
            }

            if (m_instance == null)
                lock (temp)
                {
                    //another thread may wait outside the lock while this thread goes into lock and instanced singleton, so check null again when enter into lock.
                    if (m_instance == null)
                    {
                        var tempList = FindObjectsOfType(typeof(T));
                        if (tempList != null && tempList.Length > 0)
                        {
                            if (tempList.Length > 1)
								Debug.Log("[Singleton] Something went really wrong " +
                                                   " - there should never be more than 1 singleton!" +
                                                   " Reopenning the scene might fix it.");
                            m_instance = (T) tempList[0];
                        }

                        if (m_instance == null)
                        {
                            var main = GameObject.Find("Main");
                            if (main == null)
                                main = new GameObject("Main");

                            m_instance = main.AddComponent<T>();

                            var ins = m_instance as Mono_Singleton<T>;
                            if (ins != null && ins.IsAutoCreateNew)
                            {
								Debug.Log("[Singleton]Add singleton:" + typeof(T) + " in DontDestroyOnLoad.");
                            }
                            else
                            {
                                Destroy(m_instance);
                                m_instance = null;
								Debug.Log("[Singleton]" + typeof(T) +
                                                   " instance is not derived from singleton or Auto add singleton is forbidden, return null.");
                            }
                        }
                        else
                        {
							Debug.Log("[Singleton] Using instance already created:" + typeof(T));
                        }
                    }
                }

            return m_instance;
        }
    }

    public void OnDestroy()
    {
        m_instance = null;
    }

    /// <summary>
    ///     When Unity quits, it destroys objects in a random order.
    ///     In principle, a Singleton is only destroyed when application quits.
    ///     If any script calls Instance after it have been destroyed,
    ///     it will create a buggy ghost object that will stay on the Editor scene
    ///     even after stopping playing the Application. Really bad!
    ///     So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    /// Instance will be destroyed manually in our game.
    public void OnApplicationQuit()
    {
        m_ApplicationIsQuitting = true;
        m_instance = null;
        Destroy(gameObject);
    }
}