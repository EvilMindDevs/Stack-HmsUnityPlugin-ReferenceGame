
using GameDev.Library;

using HmsPlugin;

using HuaweiMobileServices.AuthService;
using HuaweiMobileServices.CloudDB;
using HuaweiMobileServices.Common;

using System;
using System.Collections.Generic;

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
        private readonly string GameSessionsClass = "com.refapp.stackpro.huawei.GameSessions";
        private readonly string ObjectTypeInfoHelper = "com.refapp.stackpro.huawei.ObjectTypeInfoHelper";

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

            CreateObjectType();

            OpenCloudDBZone();
        }

        #endregion


        public void QuerySessions()
        {
            GLog.Log($"QuerySessions {GameManager.Instance.Uid}", GLogName.CloudDBManager);

            queryIsOK = false;
            gameSessionsList.Clear();

            CloudDBZoneQuery mCloudQuery = CloudDBZoneQuery.Where(new AndroidJavaClass(GameSessionsClass));

            mCloudQuery.EqualTo("huaweiIdMail", GameManager.Instance.Uid);

            var cloudDBZoneQueryPolicy = CloudDBZoneQuery.CloudDBZoneQueryPolicy.CLOUDDBZONE_CLOUD_CACHE;

            HMSCloudDBManager.Instance.MCloudDBZone.ExecuteQuery<GameSessions>(mCloudQuery, cloudDBZoneQueryPolicy)
                    .AddOnSuccessListener(snapshot =>
                    {
                        Debug.Log($"[{TAG}]: mCloudDBZone.ExecuteQuery AddOnSuccessListener {snapshot}");

                        ProcessQueryResult(snapshot);

                    }).AddOnFailureListener(exception =>
                    {
                        Debug.Log($"[{TAG}]: mCloudDBZone.ExecuteQuery AddOnFailureListener " +
                            exception.WrappedCauseMessage + " - " +
                            exception.WrappedExceptionMessage + " - ");

                        Debug.Log($"{TAG} OnExecuteQueryFailed(HMSException error) => {exception.WrappedExceptionMessage}");

                    });

        }

        public void AddSession(int score)
        {
            GLog.Log($"AddSession", GLogName.CloudDBManager);

            var gameSession = new GameSessions();
            var token = Guid.NewGuid().ToString();
            var sessionNumber = ++SessionNumber;

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

        private void ProcessQueryResult(CloudDBZoneSnapshot<GameSessions> snapshot)
        {
            GLog.Log($"ProcessQueryResult", GLogName.CloudDBManager);

            CloudDBZoneObjectList<GameSessions> sessionInfoCursor = snapshot.GetSnapshotObjects();
            gameSessionsList = new List<GameSessions>();

            try
            {
                while (sessionInfoCursor.HasNext())
                {
                    GameSessions gameSessions = sessionInfoCursor.Next();
                    gameSessionsList.Add(gameSessions);
                    Debug.Log($"{TAG} bookInfoCursor.HasNext() {gameSessions.Id}  {gameSessions.Score}");
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

    }
}
