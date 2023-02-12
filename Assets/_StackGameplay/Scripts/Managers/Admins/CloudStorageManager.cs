using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

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

    private void OnEnable()
    {
        this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);

    }



    private void OnDisable()
    {
        this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
    }

    private async void StartStorage()
    {
        //HMSCloudStorageManager.CheckRequestUserPermissionForCloudStorage();
        HMSCloudStorageManager.RequestPermission();
        await Task.Delay(9000);

        //string downloadDirectory = System.IO.Path.Combine(Application.persistentDataPath + "/files", "");

        //var file = new HuaweiMobileServices.Utils.java.io.File(downloadDirectory + "levelendeffect");

        //if (file.Exists()) { return; }

        var task1 = HMSCloudStorageManager.Instance.DownloadFile("/levelendeffect.manifest", "", "");
        var task2 = HMSCloudStorageManager.Instance.DownloadFile("/levelendeffect", "", "");

        //task1.OnPaused();
        //task1.OnCanceled();

    }


    private void OnSessionPrepare(object sender, GEvent<object> eventData)
    {
        StartStorage();
    }


}
