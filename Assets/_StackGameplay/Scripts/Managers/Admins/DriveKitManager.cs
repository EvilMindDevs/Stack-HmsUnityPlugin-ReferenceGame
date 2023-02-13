
using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using HuaweiMobileServices.Drive;
using HuaweiMobileServices.Id;

using StackGamePlay;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DriveKitManager : MonoBehaviour
{
    private string scanResult;

    private void Awake()
    {
        Singleton();
    }



    private void Start()
    {
    }

    private void OnEnable()
    {
        GLog.Log($"OnEnable", GLogName.DriveKitManager);

        this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
        this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);
    }



    private void OnDisable()
    {
        GLog.Log($"OnDisable", GLogName.DriveKitManager);

        this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
        this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);
    }

    #region Singleton

    public static DriveKitManager Instance;

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





    #region Events: OnSessionPrepare

    private void OnSessionPrepare(object sender, GEvent<object> eventData)
    {
        GLog.Log($"OnSessionPrepare", GLogName.DriveKitManager);
    }

    #endregion

    #region Events: OnSessionStart

    private void OnSessionStart(object sender, GEvent<object> eventData)
    {
        GLog.Log($"OnSessionStart", GLogName.DriveKitManager);
    }

    #endregion

    #region Events: OnSessionEnd

    private void OnSessionEnd(object sender, GEvent<object> eventData)
    {
        GLog.Log($"OnSessionEnd", GLogName.DriveKitManager);

        DelegateStore.UISessionEnd?.Invoke();


        return;

        int number = PlayerPrefs.GetInt("SSNo", 1);
        number++;
        PlayerPrefs.SetInt("SSNo", number);

        string filePath = $"{Application.persistentDataPath}/";
        string fileName = $"screenshot{number}.png";

        GLog.Log($"filePath {filePath}", GLogName.DriveKitManager);
        GLog.Log($"Application.dataPath {Application.dataPath}", GLogName.DriveKitManager);

        StartCoroutine(captureScreenshot(filePath, fileName));



    }

    IEnumerator TakeScreenShot(string filePath)
    {

        ScreenCapture.CaptureScreenshot(filePath);

        yield return new WaitForEndOfFrame();

        ScreenCapture.CaptureScreenshot(filePath);

        //SaveCameraView(Camera.main, filePath);

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(3f);

        //HMSDriveKitManager.Instance.CreateFiles(MimeType.MimeTypeFromSuffix(".png"), filePath);


    }

    IEnumerator captureScreenshot(string filePath, string fileName)
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Save image to file
        System.IO.File.WriteAllBytes(filePath + fileName, imageBytes);


        yield return new WaitForEndOfFrame();

        var file = HMSDriveKitManager.Instance.CreateFiles(MimeType.MimeTypeFromSuffix(".png"), filePath, fileName);

        bool state = file == null;
        Debug.Log($"state {state}");

        yield return new WaitForEndOfFrame();


    }

    void SaveCameraView(Camera cam, string filePath)
    {
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
        cam.targetTexture = screenTexture;
        RenderTexture.active = screenTexture;
        cam.Render();
        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        RenderTexture.active = null;
        byte[] byteArray = renderedTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, byteArray);
    }

    #endregion


}



