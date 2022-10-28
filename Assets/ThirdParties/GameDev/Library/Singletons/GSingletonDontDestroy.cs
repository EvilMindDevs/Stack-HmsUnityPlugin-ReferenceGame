using System.Threading;

using UnityEngine;

namespace GameDev.Library
{

    // This object wont be destroyed accross the scenes. Lifetime is same as App's
    public class GSingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _isDestroyed;

        public static T instance
        {
            get
            {
                if (_isDestroyed)
                {
                    //Debug.LogError("<color=#ff0000ff>[SingletonDontDestroy] Instance '" + typeof(T) +
                    //"' cannot be DESTROYED!. Something is very wrong.</color>");
                    return _instance;
                }
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                " - there should never be more then 1 singletion!" +
                                " Reopening the scene might fix it. Thread: " + Thread.CurrentThread.Name);
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        singleton.AddComponent<T>();

                        //singleton.name = "(singleton) " + typeof(T).ToString();
                        singleton.name = typeof(T).Name.ToString();

                        // Debug.LogFormat("[Singleton] An instance of '{0}' is initialized via code. Thread: {1}", typeof(T),
                        //Thread.CurrentThread.ManagedThreadId);

                        //DontDestroyOnLoad(singleton);
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                // Debug.LogFormat("[SingletonDontDestroy::Awake] Creating '{0}'", this);
                _instance = GetComponent<T>();
                DontDestroyOnLoad(gameObject);
                onAwake();
            }
            else
            {
                Debug.LogFormat("[SingletonDontDestroy::Awake] '{0}' already created! Destroying newly created one", this);
                DestroyImmediate(this.gameObject);
            }
        }

        virtual public void onAwake() { }


        private void OnDestroy()
        {
            _isDestroyed = true;
        }
    }
}
