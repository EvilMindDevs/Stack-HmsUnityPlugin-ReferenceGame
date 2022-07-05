
using GameDev.Library;
using GameDev.MiddleWare;

using HuaweiMobileServices.AppLinking;

using UnityEngine;

using static HuaweiMobileServices.AppLinking.AGConnectAppLinking;

namespace StackGamePlay
{
    public class AppLinkingManager : MonoBehaviour
    {
        #region Childs

        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private string imageUrl;

        [Header("CampaignInfo")]
        [SerializeField] private string name;
        [SerializeField] private string AGC;
        [SerializeField] private string app;

        #endregion
        #region Variables
        private readonly string TAG = "HMS AppLinking Demo";
        private static string shortLink;
        private static string longLink;
        private readonly string deepLink = "https://github.com/bunyamineymen";
        private readonly string uriPrefix = "https://stackgame.dre.agconnect.link";
        #endregion
        #region Singleton
        public static AppLinkingManager Instance;
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
            GLog.Log($"OnEnable", GLogName.AdsManager);
            this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);
            DelegateStore.Share += OnShare;

        }

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.AdsManager);
            this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);
            DelegateStore.Share -= OnShare;

        }
        void Start()
        {
            Debug.Log($"[{TAG}]:Get instance called for app linking");
            if (agc == null) agc = AGConnectAppLinking.GetInstance();
            Debug.Log($"[{TAG}]: GetInstance() {agc}");
            CreateAppLinking();
        }
        #endregion
        #region Logic

        public void CreateAppLinking()
        {
            AppLinking.Builder builder = new AppLinking.Builder();
            builder.SetUriPrefix(uriPrefix).SetDeepLink(deepLink)
            .SetAndroidLinkInfo(AndroidLinkInfo.NewBuilder()
            .SetAndroidDeepLink(deepLink).SetOpenType(AndroidLinkInfo.AndroidOpenType.AppGallery)
            .Build())
            .SetSocialCardInfo(SocialCardInfo.NewBuilder()
            .SetTitle(title)
            .SetImageUrl(imageUrl)
            .SetDescription(description)
            .Build())
            .SetCampaignInfo(CampaignInfo.NewBuilder()
            .SetName(name)
            .SetSource(AGC)
            .SetMedium(app)
            .Build())
            .SetPreviewType(AppLinking.LinkingPreviewType.AppInfo);
            BuildShortAppLink(builder);
            BuildLongAppLink(builder);
        }
        public void BuildLongAppLink(AppLinking.Builder builder)
        {
            longLink = builder.BuildAppLinking().GetUri();
            Debug.Log($"[{TAG}]:Long Link = {longLink}");
        }
        public void BuildShortAppLink(AppLinking.Builder builder)
        {
            var task = builder.BuildShortAppLinking();
            task.AddOnSuccessListener(it =>
            {
                shortLink = it.GetShortUrl();
                Debug.Log($"[{TAG}]:Short Link = {shortLink}");

            }).AddOnFailureListener(exception =>
            {
                Debug.LogError($"[{TAG}]: Failure on BuildShortAppLinking error { exception.WrappedExceptionMessage} cause : {exception.WrappedCauseMessage}");
            });
        }
        public void ShareShortLink()
        {
            ShareLink(shortLink);
        }
        public void ShareLongLink()
        {
            Debug.Log("ShareLongLink");
            ShareLink(longLink);
        }
        public void GetLink()
        {
            AGConnectAppLinking.GetInstance().GetAppLinking().AddOnSuccessListener(verifyCodeResult =>
            {
            })
                .AddOnFailureListener(exception =>
                {
                    Debug.LogError($"[{TAG}]: Failure on GetAppLinking error " + exception.WrappedExceptionMessage + " cause : " + exception.WrappedCauseMessage);
                });
        }
        public void GetInstance()
        {
        }

        #endregion
        #region Events: OnSessionPrepare
        private void OnSessionPrepare(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionPrepare", GLogName.AdsManager);
        }
        #endregion
        #region Events: OnSessionStart
        private void OnSessionStart(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionStart", GLogName.AdsManager);
        }
        #endregion
        #region Events: OnSessionEnd
        private void OnSessionEnd(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionEnd", GLogName.AdsManager);
        }
        #endregion
        #region Events: OnShare
        private void OnShare()
        {
            ShareLongLink();
        }
    }
    #endregion
}


