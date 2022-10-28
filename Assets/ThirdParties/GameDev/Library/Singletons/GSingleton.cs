using System.Threading;

using UnityEngine;

namespace GameDev.Library
{
    /*
     *  Thread-safe class.
        When you synchronize thread access to a shared resource, lock on a dedicated object instance (for example, 
        private readonly object balanceLock = new object();) or 
        another instance that is unlikely to be used as a lock object by unrelated parts of the code.
    */

    // This object's lifetime depends on the related Scene. When the scene destroyed, this object is destroyed.
    public class GSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _isDestroyed;

        public static T instance
        {
            get
            {
                if (_isDestroyed)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                    //return null;
                }
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (FindObjectsOfType(typeof(T)).Length <= 0)
                    {
                        Debug.Log(typeof(T) + " singleton object is not instantiated yet.");
                    }

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong - there should never be more then 1 singletion!" +
                                " Reopening the scene might fix it. Thread: " + Thread.CurrentThread.Name);
                    }
                }
                return _instance;
            }
        }

        private void OnDestroy()
        {
            //_isDestroyed = true;
        }
    }
}

/*
If a critical section spans an entire method, the locking facility can be achieved by placing the System.
Runtime.CompilerServices.MethodImplAttribute on the method, and specifying the Synchronized value in the constructor of 
System.Runtime.CompilerServices.MethodImplAttribute. When you use this attribute, the Enter and Exit method calls are not needed. 
The following code fragment illustrates this pattern:

C#

Copy
[MethodImplAttribute(MethodImplOptions.Synchronized)]
void MethodToLock()
{
   // Method implementation.
} 

Note that the attribute causes the current thread to hold the lock until the method returns; if the lock can be released sooner, 
use the Monitor class, the C# lock statement, or the Visual Basic SyncLock statement inside of the method instead of the attribute.

While it is possible for the Enter and Exit statements that lock and release a given object to cross member or class boundaries or both, 
this practice is not recommended.
*/
