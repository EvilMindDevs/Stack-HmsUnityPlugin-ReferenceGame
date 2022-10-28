
using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using HuaweiMobileServices.Ads;

using System;

using UnityEngine;

namespace StackGamePlay
{
    public class AdsManager : MonoBehaviour
    {

        #region Childs

        #endregion

        #region Variables

        #endregion

        private static Action RewardedSuccess;
        public bool IsAdsActive => !Warehouse.RemoveAds;

        public static AdsManager Instance { get => instance; set => instance = value; }

        #region Singleton

        private static AdsManager instance;

        private void Singleton()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
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
            HMSAdsKitManager.Instance.OnRewarded += OnRewardedSuccess;
            DelegateStore.SetAdsState += OnSetAdsState;
        }

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.AdsManager);
            this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);
            HMSAdsKitManager.Instance.OnRewarded -= OnRewardedSuccess;
            DelegateStore.SetAdsState -= OnSetAdsState;
        }
        private void Start()
        {
            //ShowBanner();
        }
        #endregion
        #region Ads: Banner
        public void ShowBanner()
        {
            if (!IsAdsActive)
                return;
            GLog.Log($"ShowBanner", GLogName.AdsManager);
            HMSAdsKitManager.Instance.ShowBannerAd();
        }
        public void HideBanner()
        {
            GLog.Log($"HideBanner", GLogName.AdsManager);
            HMSAdsKitManager.Instance.HideBannerAd();
        }
        #endregion
        #region Ads: Rewarded
        public void ShowRewarded(Action onRewardedSuccess)
        {
            GLog.Log($"ShowRewarded", GLogName.AdsManager);
            if (!IsAdsActive)
                return;
            RewardedSuccess = onRewardedSuccess;
            HMSAdsKitManager.Instance.ShowRewardedAd();
        }
        private void OnRewardedSuccess(Reward reward)
        {
            GLog.Log($"OnRewardedSuccess", GLogName.AdsManager);
            RewardedSuccess?.Invoke();
        }
        #endregion
        #region Ads: Interstitial
        public void ShowInterstitial()
        {
            if (!IsAdsActive)
                return;
            GLog.Log($"ShowInterstitial", GLogName.AdsManager);
            HMSAdsKitManager.Instance.ShowInterstitialAd();
        }
        #endregion
        #region Ads: Splash
        public void ShowSplashImage()
        {
            if (!IsAdsActive)
                return;
            GLog.Log($"ShowSplashImage", GLogName.AdsManager);
            HMSAdsKitManager.Instance.LoadSplashAd("testq6zq98hecj", SplashAd.SplashAdOrientation.PORTRAIT);
        }
        public void ShowSplashVideo()
        {
            GLog.Log($"ShowSplashVideo", GLogName.AdsManager);
            if (!IsAdsActive)
                return;
            HMSAdsKitManager.Instance.LoadSplashAd("testd7c5cewoj6", SplashAd.SplashAdOrientation.PORTRAIT);
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

            ShowInterstitial();
        }
        #endregion

        #region Event: OnSetAdsState
        private void OnSetAdsState(bool state)
        {
            if (!state)
            {
                HideBanner();
                return;
            }
        }
    }
    #endregion
}


