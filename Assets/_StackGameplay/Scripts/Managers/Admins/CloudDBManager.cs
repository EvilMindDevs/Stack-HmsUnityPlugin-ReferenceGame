
using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using HuaweiMobileServices.AuthService;
using HuaweiMobileServices.CloudDB;
using HuaweiMobileServices.Common;
using HuaweiMobileServices.IAP;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.Utils;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

namespace StackGamePlay
{
    public class CloudDBManager : MonoBehaviour
    {

        private string TAG = "CloudDBDemo";
        private HMSAuthServiceManager authServiceManager = null;
        private AGConnectUser user = null;
        private Text loggedInUser;

        private const string NOT_LOGGED_IN = "No user logged in";
        private const string LOGGED_IN = "{0} is logged in";
        private const string LOGGED_IN_ANONYMOUSLY = "Anonymously Logged In";
        private const string LOGIN_ERROR = "Error or cancelled login";

        private HMSCloudDBManager cloudDBManager = null;
        private readonly string cloudDBZoneName = "Main";
        private readonly string GameSessionsClass = "com.refapp.stack.huawei.GameSessions";
        private readonly string ObjectTypeInfoHelper = "com.refapp.stack.huawei.ObjectTypeInfoHelper";


        List<GameSessions> gameSessionsList = new List<GameSessions>();
        public List<GameSessions> GameSessionsList { get => gameSessionsList; set => gameSessionsList = value; }


        private bool queryIsOK = false;
        public bool QueryIsOK { get => queryIsOK; set => queryIsOK = value; }



        public static int SessionNumber
        {
            get
            {
                return PlayerPrefs.GetInt("SessionNumber", 0);
            }
            set
            {
                PlayerPrefs.SetInt("SessionNumber", value);
            }
        }


        #region Singleton
        private static CloudDBManager instance;

        public static CloudDBManager Instance { get => instance; set => instance = value; }

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

            DontDestroyOnLoad(gameObject);

        }
        #endregion

        #region Monobehaviour

        private void Awake()
        {
            Singleton();
        }

        private void OnEnable()
        {
            GLog.Log($"OnEnable", GLogName.CloudDBManager);

        }

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.CloudDBManager);

        }


        public void Start()
        {
            GLog.Log($"Start", GLogName.CloudDBManager);

            cloudDBManager = HMSCloudDBManager.Instance;
            cloudDBManager.Initialize();
            cloudDBManager.GetInstance(AGConnectInstance.GetInstance(), AGConnectAuth.GetInstance());
            cloudDBManager.OnExecuteQuerySuccess = OnExecuteQuerySuccess;
            cloudDBManager.OnExecuteQueryFailed = OnExecuteQueryFailed;

            CreateObjectType();

            OpenCloudDBZone();



            //_ = TestFlow();

        }

        //private async Task TestFlow()
        //{
        //    await Task.Delay(3000);
        //    AddSession(77);
        //    //await Task.Delay(1000);
        //    //AddSession(77);
        //    //await Task.Delay(1000);
        //    //AddSession(77);
        //    //await Task.Delay(1000);
        //    //AddSession(77);

        //}

        #endregion


        public void QuerySessions()
        {
            GLog.Log($"QuerySessions", GLogName.CloudDBManager);

            queryIsOK = false;
            gameSessionsList.Clear();

            CloudDBZoneQuery mCloudQuery = CloudDBZoneQuery.Where(new AndroidJavaClass(GameSessionsClass));

            mCloudQuery.EqualTo("huaweiIdMail", GameManager.Instance.Uid);



            //            return CallAsWrapper<CloudDBZoneQuery>("in", new object[2]
            //{
            //                fieldName,
            //                value
            //});


            cloudDBManager.ExecuteQuery(mCloudQuery, CloudDBZoneQuery.CloudDBZoneQueryPolicy.CLOUDDBZONE_CLOUD_CACHE);
        }



        public void AddSession(int score)
        {
            GLog.Log($"AddSession", GLogName.CloudDBManager);

            var gameSession = new GameSessions();

            var token = Guid.NewGuid().ToString();

            var sessionNumber = ++SessionNumber;

            //Debug.Log($"------ AddSession 1");

            //Debug.Log($"Email {huaweiID.Email}");
            //Debug.Log($"DisplayName {huaweiID.DisplayName}");
            ////Debug.Log($"DisplayName {huaweiID.EmailVerified}");

            //Debug.Log($"------ AddSession 2");

            //Debug.Log($"Uid {huaweiID.Uid}");
            //Debug.Log($"ProviderId {huaweiID.ProviderId}");
            //Debug.Log($"FamilyName {huaweiID.Phone}");

            //Debug.Log($"------ AddSession 3");

            gameSession.HuaweiIdMail = GameManager.Instance.Uid;
            gameSession.Id = token;
            gameSession.SessionNumber = sessionNumber;
            gameSession.Score = score;

            cloudDBManager.ExecuteUpsert(gameSession);
        }



        public void OpenCloudDBZone()
        {
            GLog.Log($"OpenCloudDBZone", GLogName.CloudDBManager);

            cloudDBManager.OpenCloudDBZone(cloudDBZoneName, CloudDBZoneConfig.CloudDBZoneSyncProperty.CLOUDDBZONE_CLOUD_CACHE, CloudDBZoneConfig.CloudDBZoneAccessProperty.CLOUDDBZONE_PUBLIC);
        }


        public void ExecuteSumQuery()
        {
            GLog.Log($"ExecuteSumQuery", GLogName.CloudDBManager);

            CloudDBZoneQuery mCloudQuery = CloudDBZoneQuery.Where(new AndroidJavaClass(GameSessionsClass));
            cloudDBManager.ExecuteSumQuery(mCloudQuery, "price", CloudDBZoneQuery.CloudDBZoneQueryPolicy.CLOUDDBZONE_LOCAL_ONLY);
        }

        public void ExecuteCountQuery()
        {
            CloudDBZoneQuery mCloudQuery = CloudDBZoneQuery.Where(new AndroidJavaClass(GameSessionsClass));
            cloudDBManager.ExecuteCountQuery(mCloudQuery, "price", CloudDBZoneQuery.CloudDBZoneQueryPolicy.CLOUDDBZONE_LOCAL_ONLY);
        }


        public void CreateObjectType()
        {
            cloudDBManager.CreateObjectType(ObjectTypeInfoHelper);
        }


        #region Callback: OnExecuteQuerySuccess

        private void OnExecuteQuerySuccess(CloudDBZoneSnapshot<GameSessions> snapshot) => ProcessQueryResult(snapshot);


        private void ProcessQueryResult(CloudDBZoneSnapshot<GameSessions> snapshot)
        {
            GLog.Log($"ProcessQueryResult", GLogName.CloudDBManager);

            CloudDBZoneObjectList<GameSessions> bookInfoCursor = snapshot.GetSnapshotObjects();
            //gameSessionsList = new List<GameSessions>();
            try
            {
                while (bookInfoCursor.HasNext())
                {
                    GameSessions bookInfo = bookInfoCursor.Next();
                    gameSessionsList.Add(bookInfo);
                    Debug.Log($"{TAG} bookInfoCursor.HasNext() {bookInfo.Id}  {bookInfo.Score}");
                }

                QueryIsOK = true;
            }
            catch (Exception e)
            {
                Debug.Log($"{TAG} processQueryResult:  Exception => " + e.Message);
            }
            finally
            {
                snapshot.Release();
            }
        }

        #endregion

        #region Callback: OnExecuteQueryFailed

        private void OnExecuteQueryFailed(HMSException error) => Debug.Log($"{TAG} OnExecuteQueryFailed(HMSException error) => {error.WrappedExceptionMessage}");

        #endregion






        //public void Start()
        //{

        //    authServiceManager = HMSAuthServiceManager.Instance;
        //    //authServiceManager.OnSignInSuccess = OnAuthSericeSignInSuccess;
        //    //authServiceManager.OnSignInFailed = OnAuthSericeSignInFailed;

        //    //if (authServiceManager.GetCurrentUser() != null)
        //    //{
        //    //    user = authServiceManager.GetCurrentUser();
        //    //    loggedInUser.text = user.IsAnonymous() ? LOGGED_IN_ANONYMOUSLY : string.Format(LOGGED_IN, user.DisplayName);
        //    //}
        //    //else
        //    //{
        //    //    SignInWithHuaweiAccount();
        //    //}


        //}

        //private void OnAccountKitLoginSuccess(AuthAccount authHuaweiId)
        //{
        //    AGConnectAuthCredential credential = HwIdAuthProvider.CredentialWithToken(authHuaweiId.AccessToken);
        //    authServiceManager.SignIn(credential);
        //}

        //public void SignInWithHuaweiAccount()
        //{
        //    HMSAccountKitManager.Instance.OnSignInSuccess = OnAccountKitLoginSuccess;
        //    HMSAccountKitManager.Instance.OnSignInFailed = OnAuthSericeSignInFailed;
        //    HMSAccountKitManager.Instance.SignIn();
        //}

        //private void OnAuthSericeSignInFailed(HMSException error)
        //{
        //    loggedInUser.text = LOGIN_ERROR;
        //}

        //private void OnAuthSericeSignInSuccess(SignInResult signInResult)
        //{
        //    user = signInResult.GetUser();
        //    loggedInUser.text = user.IsAnonymous() ? LOGGED_IN_ANONYMOUSLY : string.Format(LOGGED_IN, user.DisplayName);
        //}



        //public void GetCloudDBZoneConfigs()
        //{
        //    IList<CloudDBZoneConfig> CloudDBZoneConfigs = cloudDBManager.GetCloudDBZoneConfigs();
        //    Debug.Log($"{TAG} " + CloudDBZoneConfigs.Count);
        //}



        //public void OpenCloudDBZone2()
        //{
        //    cloudDBManager.OpenCloudDBZone2(cloudDBZoneName, CloudDBZoneConfig.CloudDBZoneSyncProperty.CLOUDDBZONE_CLOUD_CACHE, CloudDBZoneConfig.CloudDBZoneAccessProperty.CLOUDDBZONE_PUBLIC);
        //}

        //public void EnableNetwork() => cloudDBManager.EnableNetwork(cloudDBZoneName);
        //public void DisableNetwork() => cloudDBManager.DisableNetwork(cloudDBZoneName);

        //public void AddBookInfo()
        //{
        //    BookInfo bookInfo = new BookInfo();
        //    bookInfo.Id = 1;
        //    bookInfo.BookName = "bookName";
        //    bookInfo.Author = "Author 1";
        //    cloudDBManager.ExecuteUpsert(bookInfo);
        //}

        //public void AddBookInfoList()
        //{
        //    IList<AndroidJavaObject> bookInfoList = new List<AndroidJavaObject>();

        //    BookInfo bookInfo1 = new BookInfo();
        //    bookInfo1.Id = 2;
        //    bookInfo1.Author = "Author 2";
        //    bookInfoList.Add(bookInfo1.GetObj());

        //    BookInfo bookInfo2 = new BookInfo();
        //    bookInfo2.Id = 3;
        //    bookInfo2.Author = "Author 3";
        //    bookInfoList.Add(bookInfo2.GetObj());

        //    cloudDBManager.ExecuteUpsert(bookInfoList);
        //}

        //public void UpdateBookInfo()
        //{
        //    BookInfo bookInfo = new BookInfo();
        //    bookInfo.Id = 1;
        //    bookInfo.BookName = "bookName";
        //    bookInfo.Author = "Author 1";
        //    bookInfo.Price = 300;
        //    cloudDBManager.ExecuteUpsert(bookInfo);
        //}

        //public void DeleteBookInfo()
        //{
        //    BookInfo bookInfo = new BookInfo();
        //    bookInfo.Id = 1;
        //    cloudDBManager.ExecuteDelete(bookInfo);
        //}

        //public void DeleteBookInfoList()
        //{
        //    IList<AndroidJavaObject> bookInfoList = new List<AndroidJavaObject>();

        //    BookInfo bookInfo1 = new BookInfo();
        //    bookInfo1.Id = 2;
        //    bookInfo1.Author = "Author 2";
        //    bookInfoList.Add(bookInfo1.GetObj());

        //    BookInfo bookInfo2 = new BookInfo();
        //    bookInfo2.Id = 3;
        //    bookInfo2.Author = "Author 3";
        //    bookInfoList.Add(bookInfo2.GetObj());

        //    cloudDBManager.ExecuteDelete(bookInfoList);
        //}

        //public void GetBookInfo()
        //{
        //    CloudDBZoneQuery mCloudQuery = CloudDBZoneQuery.Where(new AndroidJavaClass(BookInfoClass));
        //    cloudDBManager.ExecuteQuery(mCloudQuery, CloudDBZoneQuery.CloudDBZoneQueryPolicy.CLOUDDBZONE_LOCAL_ONLY);
        //}












    }

}







//Debug.Log($"Email {huaweiID.Email}");
//Debug.Log($"AccessToken {huaweiID.AccessToken}");
//Debug.Log($"DisplayName {huaweiID.DisplayName}");
//Debug.Log($"CarrierId {huaweiID.CarrierId}");
//Debug.Log($"AvatarUriString {huaweiID.AvatarUriString}");
//Debug.Log($"FamilyName {huaweiID.FamilyName}");
//Debug.Log($"Gender {huaweiID.Gender}");
//Debug.Log($"GivenName {huaweiID.GivenName}");
//Debug.Log($"HuaweiAccount {huaweiID.HuaweiAccount}");
//Debug.Log($"IdToken {huaweiID.IdToken}");
//Debug.Log($"OpenId {huaweiID.OpenId}");
//Debug.Log($"RequestedScopes {huaweiID.RequestedScopes}");
//Debug.Log($"Uid {huaweiID.Uid}");
//Debug.Log($"UnionId {huaweiID.UnionId}");