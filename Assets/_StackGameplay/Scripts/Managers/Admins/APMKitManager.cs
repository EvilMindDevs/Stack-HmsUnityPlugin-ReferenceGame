
using GameDev.Library;
using GameDev.MiddleWare;

using HuaweiMobileServices.APM;

using System.Threading.Tasks;

using UnityEngine;

public class APMKitManager : MonoBehaviour
{
    private string scanResult;

    private void Awake()
    {
        Singleton();
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

    private async void Start()
    {

        HMSAPMManager.Instance.EnableCollection(true);
        HMSAPMManager.Instance.EnableAnrMonitor(true);

        await Task.Delay(50);

        Debug.Log("APM TESTING !!!");


        //var customTrace = APMS.GetInstance().CreateCustomTrace("CustomEvent1");
        //customTrace.Start();

        //customTrace.PutMeasure("ProcessingTimes", 0);

        //for (int i = 0; i < 5; i++)
        //{
        //    customTrace.IncrementMeasure("ProcessingTimes", 1);
        //}

        //customTrace.PutProperty("ProcessingResult", "Success");
        //customTrace.PutProperty("Status", "Normal");

        //customTrace.Stop();

        //await Task.Delay(1500);

        //while (true)
        //{
        //    Debug.Log("this is ANR !!!");
        //}

        //await Task.Delay(1500);

        //UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.AccessViolation);
    }

    #region Singleton

    public static APMKitManager Instance;

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
    }

    #endregion


}



