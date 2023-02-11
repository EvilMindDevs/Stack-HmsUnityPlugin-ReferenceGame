using UnityEngine;

public class CloudStorageManager : MonoBehaviour
{

    private void Awake()
    {
        Singleton();
    }

    #region Singleton

    public static CloudStorageManager Instance;

    private void Singleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion



}
