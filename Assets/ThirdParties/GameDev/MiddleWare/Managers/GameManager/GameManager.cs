
using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using HuaweiMobileServices.AuthService;
using HuaweiMobileServices.Drive;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.InAppComment;
using HuaweiMobileServices.RemoteConfig;
using HuaweiMobileServices.Utils;

using StackGamePlay;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    private HMSAuthServiceManager authServiceManager = null;

    private AGConnectUser user = null;
    private string uid;

    public string Uid { get => U_ID; }


    private bool sessionIsPlaying = false;
    public bool SessionIsPlaying { get => sessionIsPlaying; set => sessionIsPlaying = value; }


    public static string U_ID
    {
        get => PlayerPrefs.GetString("U_ID", "");

        set => PlayerPrefs.SetString("U_ID", value);
    }


    #region Singleton

    private static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }

    private void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    #region Unity: OnEnable
    private void OnEnable()
    {
        GLog.Log($"OnEnable", GLogName.GameManager);

        this.AddListener<object>(GEventName.ACCOUNT_KIT_IS_READY, OnAccountKitIsReady);
        this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
        this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);
        //DelegateStore.SignOut += OnSignOut;

        HMSAccountKitManager.Instance.OnSignInSuccess += OnAccountKitLoginSuccess;
    }

    #endregion

    #region Unity: OnDisable

    private void OnDisable()
    {
        GLog.Log($"OnDisable", GLogName.GameManager);

        this.RemoveListener<object>(GEventName.ACCOUNT_KIT_IS_READY, OnAccountKitIsReady);
        this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
        this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);
        //DelegateStore.SignOut -= OnSignOut;

        HMSAccountKitManager.Instance.OnSignInSuccess -= OnAccountKitLoginSuccess;
    }

    #endregion

    #region Unity: Awake

    private void Awake()
    {
        Singleton();
        DontDestroyOnLoad(gameObject);

        authServiceManager = HMSAuthServiceManager.Instance;
    }
    #endregion

    #region Unity: Start
    private void Start()
    {
        GLog.Log($"Start", GLogName.GameManager);

        LeanTween.value(0f, 0.25f, 0.5f).setOnUpdate((float val) =>
        {
            DelegateStore.AdvanceProgressBar?.Invoke(val);
        })
.setOnComplete(delegate ()
{
    SignInWithHuaweiAccount();
});

    }
    #endregion


    #region HMS: Crash
    public void SetCrash()
    {
        HMSCrashManager.Instance.EnableCrashCollection(true);

        //HMSCrashManager.Instance.customReport();
        //await Task.Delay(4000);
        //Debug.LogError("!!!!!   CRASH !!!!!");
        //HMSCrashManager.Instance.TestCrash();

        RealTimeDataStore.CrashIsOk = true;

    }
    #endregion

    #region HMS: RemoteConfig
    private void RemoteConfig()
    {
        void OnFecthSuccess(ConfigValues config)
        {
            HMSRemoteConfigManager.Instance.Apply(config);
            var _blockMoveTimeCoeff = HMSRemoteConfigManager.Instance.GetValueAsDouble("BlockMoveTimeCoeff");
            var _backgroundColorIndex = HMSRemoteConfigManager.Instance.GetValueAsLong("BGColor");
            RealTimeDataStore.BackgroundColorIndex = int.Parse(_backgroundColorIndex.ToString());
            RealTimeDataStore.BlockMoveTimeCoeff = (float)_blockMoveTimeCoeff;
            RealTimeDataStore.RemoteConfigIsOk = true;
        }
        void OnFecthFailure(HMSException exception)
        {
            Debug.Log($" fetch() Failed Error Code => {exception.ErrorCode} Message => {exception.WrappedExceptionMessage}");
        }
        HMSRemoteConfigManager.Instance.OnFecthSuccess = OnFecthSuccess;
        HMSRemoteConfigManager.Instance.OnFecthFailure = OnFecthFailure;
        HMSRemoteConfigManager.Instance.Fetch(5);
    }
    #endregion

    #region ONGUI

    //private GUIStyle fontSize;

    //private void OnGUI()
    //{
    //    fontSize = new GUIStyle();

    //    fontSize.fontSize = 65;

    //    GUI.Box(new Rect(10, 10, 100, 100), "Teleportation Booths", fontSize);

    //    Debug.Log("OnGUI");

    //    var gdfgdf = string.Format(" {0} ", crashIsOk.ToString());

    //    var log = $" CrashIsOk {CrashIsOk} \n  RemoteConfigIsOk {RemoteConfigIsOk} \n IapIsOk {IapIsOk}";

    //    GUI.Label(new Rect(10, 200, 1000, 190), log, fontSize);
    //    return;
    //}

    #endregion


    #region ServiceManage

    private bool allServicesAreReady = false;

    void ServiceManage()
    {
        StartCoroutine(Advancement2());
    }

    IEnumerator Advancement2()
    {

        LeanTween.value(0.25f, 0.95f, 1.85f).setOnUpdate((float val) =>
        {
            DelegateStore.AdvanceProgressBar?.Invoke(val);
        }).setOnComplete(delegate ()
        {
            StartCoroutine(Advancement());
        });
        yield return new WaitUntil(() => RealTimeDataStore.CrashIsOk && RealTimeDataStore.RemoteConfigIsOk && RealTimeDataStore.IapIsOk);
        allServicesAreReady = true;
    }

    IEnumerator Advancement()
    {
        yield return new WaitUntil(() => allServicesAreReady);

        LeanTween.value(0.95f, 1f, 0.15f).setOnUpdate((float val) =>
        {
            DelegateStore.AdvanceProgressBar?.Invoke(val);
        }).setOnComplete(delegate ()
        {
            SceneManager.LoadScene("GameScene");
        });
    }



    #endregion

    public AccountAuthParams GetAccountAuthParams()
    {
        List<Scope> scopeList = new List<Scope>
        {
            new Scope(Drive.DriveScopes.SCOPE_DRIVE_FILE),
            new Scope(Drive.DriveScopes.SCOPE_DRIVE_APPDATA)
        };

        AccountAuthParams authParams = new AccountAuthParamsHelper(AccountAuthParams.DEFAULT_AUTH_REQUEST_PARAM)
            .SetAccessToken()
            .SetIdToken()
            .SetScopeList(scopeList)
            .CreateParams();

        string log = "authParams is null -> " + (authParams == null).ToString() + "\n";

        return authParams;
    }

    #region Login

    #region Account

    public void SignInWithHuaweiAccount()
    {
        HMSAccountKitManager.Instance.OnSignInSuccess = OnAccountKitLoginSuccess;
        HMSAccountKitManager.Instance.OnSignInFailed = OnAuthSericeSignInFailed;
        //HMSAccountKitManager.Instance.SignIn();


        HMSAccountKitManager.Instance.SignInDrive(AccountAuthManager.GetService(GetAccountAuthParams()));


    }

    private void OnAccountKitLoginSuccess(AuthAccount authHuaweiId)
    {
        authServiceManager.SignOut();
        RealTimeDataStore.AccountKitIsReady = true;

        AGConnectAuthCredential credential = HwIdAuthProvider.CredentialWithToken(authHuaweiId.AccessToken);

        authServiceManager.OnSignInSuccess = OnAuthSericeSignInSuccess;
        authServiceManager.OnSignInFailed = OnAuthSericeSignInFailed;

        authServiceManager.SignIn(credential);

        this.DispatchEvent(new GEvent<object>(GEventName.ACCOUNT_KIT_IS_READY, this));

        CredentialManager.GetInstance().InitDrive(authHuaweiId);

        ServiceManage();

    }

    #endregion

    #region Auth

    private void OnAuthSericeSignInFailed(HMSException error)
    {

    }

    private void OnAuthSericeSignInSuccess(SignInResult signInResult)
    {
        user = signInResult.GetUser();
        //loggedInUser.text = user.IsAnonymous() ? LOGGED_IN_ANONYMOUSLY : string.Format(LOGGED_IN, user.DisplayName);
        uid = user.Uid;
        U_ID = uid;
    }

    #endregion

    #endregion




    #region Unity: OnApplicationQuit
    //#if UNITY_EDITOR
    //        async void OnApplicationQuit()
    //        {
    //            await Task.Delay(300);
    //            return;
    //            var assembly = Assembly.GetAssembly(typeof(SceneView));
    //            var type = assembly.GetType("UnityEditor.LogEntries");
    //            var method = type.GetMethod("Clear");
    //            method.Invoke(new object(), null);
    //            PlayerPrefs.DeleteAll();
    //        }
    //#endif
    #endregion

    #region Events: OnAppInitialize
    private void OnAccountKitIsReady(object sender, GEvent<object> eventData)
    {
        RemoteConfig();
        SetCrash();
    }
    #endregion

    #region Events: OnSessionPrepare
    private void OnSessionPrepare(object sender, GEvent<object> eventData)
    {
        GLog.Log($"OnSessionPrepare", GLogName.GameManager);

        sessionIsPlaying = false;

        BlockManager.Instance.StartMethod();
    }
    #endregion
    #region Events: OnSessionStart
    private void OnSessionStart(object sender, GEvent<object> eventData)
    {
        GLog.Log($"OnSessionStart", GLogName.GameManager);

        sessionIsPlaying = true;
    }
    #endregion
    #region Events: OnSessionEnd
    private void OnSessionEnd(object sender, GEvent<object> eventData)
    {
        GLog.Log($"OnSessionEnd", GLogName.GameManager);
        sessionIsPlaying = false;

        InAppComment.ShowInAppComment();

    }
    #endregion



    //#region Event: OnSignOut
    //private void OnSignOut()
    //{
    //    GLog.Log($"OnSignOut", GLogName.GameManager);
    //    authServiceManager.SignOut();
    //    var agConnectUser = authServiceManager.GetCurrentUser();
    //    Debug.Log("DisplayName" + agConnectUser.DisplayName);
    //}
    //#endregion






}


