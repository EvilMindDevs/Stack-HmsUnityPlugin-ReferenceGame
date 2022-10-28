
using GameDev.Library;
using GameDev.MiddleWare;

using HuaweiMobileServices.Scan;

using StackGamePlay;

using System;

using UnityEngine;

public class ScanKitManager : MonoBehaviour
{
    private string scanResult;

    private void Awake()
    {
        Singleton();
    }

    private void OnEnable()
    {
        GLog.Log($"OnEnable", GLogName.ScanKitManager);

        this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
        this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);

        HMSScanKitManager.Instance.ScanSuccess += OnScanSuccess;

        DelegateStore.Scan += OnScan;

    }



    private void OnDisable()
    {
        GLog.Log($"OnDisable", GLogName.ScanKitManager);

        this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
        this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);

        HMSScanKitManager.Instance.ScanSuccess -= OnScanSuccess;

        DelegateStore.Scan -= OnScan;

    }

    #region Singleton

    public static ScanKitManager Instance;

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

    private void OnScan()
    {
        Debug.Log($"OnScan");

        HMSScanKitManager.Instance.Scan(HmsScanBase.ALL_SCAN_TYPE);
    }

    #region Events: OnSessionPrepare

    private void OnSessionPrepare(object sender, GEvent<object> eventData)
    {
        GLog.Log($"OnSessionPrepare", GLogName.ScanKitManager);
    }

    #endregion

    #region Events: OnSessionStart

    private void OnSessionStart(object sender, GEvent<object> eventData)
    {
        GLog.Log($"OnSessionStart", GLogName.ScanKitManager);
    }

    #endregion

    #region Events: OnSessionEnd

    private void OnSessionEnd(object sender, GEvent<object> eventData)
    {
        GLog.Log($"OnSessionEnd", GLogName.ScanKitManager);
    }

    #endregion

    #region Callbacks

    private void OnScanSuccess(string text, HmsScan hmsScan)
    {
        Debug.Log($"OnScanSuccess {text}");
        scanResult = text;

        if (text == "StackCode_NoAds")
        {
            Warehouse.RemoveAds = true;
        }
    }

    #endregion


}



