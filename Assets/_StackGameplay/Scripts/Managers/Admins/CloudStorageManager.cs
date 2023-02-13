using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using HuaweiMobileServices.CloudStorage;

using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

using UnityEngine;

public class CloudStorageManager : MonoBehaviour
{

    #region Monobehaviour

    private void Awake()
    {
        Singleton();
    }

    private void OnEnable()
    {
        this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
        this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);
    }

    private void OnDisable()
    {
        this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
        this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);
    }

    private void Start()
    {

    }

    #endregion

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

    public GameObject gameOverCanvas;

    public IEnumerator GetAssetBundle()
    {
        Debug.Log("PHASE_1");

        var fullPathFile1 = Application.persistentDataPath + "/gameovercanvas";
        var fullPathFile2 = Application.persistentDataPath + "/gameovercanvas.manifest";

        Debug.Log($"PHASE_1.1 ,fullPathFile1: {fullPathFile1},fullPathFile2 {fullPathFile2}");

        string fileName = "gameovercanvas";

        bool state1 = File.Exists(fullPathFile1);
        bool state2 = File.Exists(fullPathFile2);

        DownloadTask task1;
        DownloadTask task2;

        if (!state1)
            task1 = HMSCloudStorageManager.Instance.DownloadFile("/gameovercanvas", "", "");

        if (!state2)
            task2 = HMSCloudStorageManager.Instance.DownloadFile("/gameovercanvas.manifest", "", "");

        while (true)
        {
            yield return new WaitForSeconds(0.3f);

            if (File.Exists(fullPathFile1))
            {
                break;
            }
        }

        while (true)
        {
            yield return new WaitForSeconds(0.3f);

            if (File.Exists(fullPathFile2))
            {
                break;
            }
        }

        if (gameOverCanvas == null)
        {
            AssetBundle orjinalAssetBundle = AssetBundle.LoadFromFile(fullPathFile1);
            var effectPrefab = (GameObject)orjinalAssetBundle.LoadAsset(fileName);
            var gameoverCanvas = Instantiate(effectPrefab);

            gameoverCanvas.SetActive(false);

            gameOverCanvas = gameoverCanvas;
            DontDestroyOnLoad(gameOverCanvas);
        }
    }

    private async void OnSessionPrepare(object sender, GEvent<object> eventData)
    {
        HMSCloudStorageManager.CheckRequestUserPermissionForCloudStorage();

        await Task.Delay(2000);

        StartCoroutine(GetAssetBundle());
    }

    private void OnSessionStart(object sender, GEvent<object> eventData)
    {

    }

    private async void OnSessionEnd(object sender, GEvent<object> eventData)
    {
        gameOverCanvas.SetActive(true);
        await Task.Delay(1000);
        gameOverCanvas.SetActive(false);
    }

}
