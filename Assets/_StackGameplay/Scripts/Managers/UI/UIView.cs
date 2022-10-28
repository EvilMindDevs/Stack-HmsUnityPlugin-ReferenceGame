using GameDev.Library;
using GameDev.MiddleWare;

using System;
using System.Collections;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StackGamePlay
{
    public class UIView : MonoBehaviour
    {
        #region Childs

        [SerializeField] private GameObject[] _MenuGameObjects;
        [SerializeField] private GameObject[] _SessionGameObjects;
        [SerializeField] private TextMeshProUGUI Txt_Score;
        [SerializeField] private TextMeshProUGUI Txt_TapToStart;
        [SerializeField] private Button _BtnTapToPlay;
        [SerializeField] private Button _BtnRemoveAds;
        [SerializeField] private Button _ShowLeaderboard;
        [SerializeField] private Button _ShowAchievements;
        [SerializeField] private Button _BtnShare;
        [SerializeField] private Button Btn_ShowScoreTablePopup;
        [SerializeField] private Button Btn_ScanKit;

        [SerializeField] private GameObject G_RemoveAds;

        [SerializeField] private TextMeshProUGUI TxtLocation;


        public Button BtnTapToPlay
        {
            get => _BtnTapToPlay;
            set => _BtnTapToPlay = value;
        }

        #endregion

        #region Singleton

        public static UIView Instance;
        public string tapToStartText = "Tap To PLAY";


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

        #region Monobehaviour
        private void Awake()
        {
            Singleton();
        }
        private void OnEnable()
        {
            //GLog.Log($"OnEnable", GLogName.AnalyticsManager);
            this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);

            DelegateStore.TapToPlay += delegate
            {
                this.DispatchEvent(new GEvent<object>(GEventName.SESSION_STARTED, this));
            };

            _BtnShare.onClick.AddListener(delegate () { DelegateStore.Share?.Invoke(); });

            Btn_ShowScoreTablePopup.onClick.AddListener(delegate ()
            {
                DelegateStore.ShowPopup?.Invoke(PopupType.ScoreTable);
            });

            DelegateStore.SetScore += OnSetScoreText;
            DelegateStore.SetAdsState += OnSetInAppPurchaseButton;
            DelegateStore.ShowLocation += ShowLocation;

            Btn_ScanKit.onClick.AddListener(delegate ()
            {
                DelegateStore.Scan?.Invoke();
            });

            DelegateStore.UISessionEnd += delegate
            {
                Txt_TapToStart.text = tapToStartText;
                CloseAllUI();
                Txt_TapToStart.gameObject.SetActive(true);
                BtnTapToPlay.gameObject.SetActive(true);
                BtnTapToPlay.onClick.RemoveAllListeners();
                BtnTapToPlay.onClick.AddListener(delegate { SceneManager.LoadScene("GameScene"); });

            };
        }

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.UIManager);
            this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);

            DelegateStore.TapToPlay = null;

            _BtnShare.onClick.RemoveListener(delegate () { DelegateStore.Share?.Invoke(); });
            DelegateStore.SetScore -= OnSetScoreText;
            DelegateStore.SetAdsState -= OnSetInAppPurchaseButton;
            DelegateStore.ShowLocation -= ShowLocation;

            Btn_ScanKit.onClick.RemoveListener(delegate ()
            {
                DelegateStore.Scan?.Invoke();
            });

            DelegateStore.UISessionEnd = null;
        }

        private async void Start()
        {
            bool isAdsActive = !Warehouse.RemoveAds;
            //Debug.Log($"isAdsActive {isAdsActive}");
            G_RemoveAds.SetActive(isAdsActive);
            StartCoroutine(SetAchievementAndLeaderboardButtons());



            var _lat = LocationKitManager.Instance.Latitude;
            var _long = LocationKitManager.Instance.Longitude;

            if (_lat <= 42 && _lat >= 36)
            {
                if (_long <= 45 && _long >= 26)
                {

                    TxtLocation.text = "KEYİFLİ OYUNLAR !!!";
                    await Task.Delay(2000);

                    TxtLocation.gameObject.SetActive(false);
                }
            }



            //var location = LocationKitManager.Instance.Location;

            //var latitude = location.GetLatitude();
            //var longitude = location.GetLongitude();

            //if (latitude <= 42 && latitude >= 36)
            //{
            //    if (longitude <= 45 && longitude >= 26)
            //    {
            //        TxtLocation.text = "KEYİFLİ OYUNLAR !!!";

            //        //TxtLocation.gameObject.SetActive(false);
            //    }
            //}


        }
        private void Update()
        {
            bool isAdsActive = !Warehouse.RemoveAds;
            //Debug.Log($"isAdsActive {isAdsActive}");
            if (isAdsActive)
                return;
            G_RemoveAds.SetActive(isAdsActive);
        }
        #endregion

        IEnumerator SetAchievementAndLeaderboardButtons()
        {
            _ShowLeaderboard.gameObject.SetActive(false);
            _ShowAchievements.gameObject.SetActive(false);
            _BtnRemoveAds.gameObject.SetActive(false);
            yield return new WaitUntil(() => RealTimeDataStore.AccountKitIsReady);
            _ShowLeaderboard.gameObject.SetActive(true);
            _ShowAchievements.gameObject.SetActive(true);
            _BtnRemoveAds.gameObject.SetActive(true);
        }

        #region Events: OnSessionPrepare
        private void OnSessionPrepare(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionPrepare", GLogName.UIManager);
            Txt_TapToStart.text = "Tap To PLAY";
            CloseAllUI();
            OpenMenuUI();
        }
        #endregion
        #region Events: OnSessionStart
        private void OnSessionStart(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionStart", GLogName.UIManager);
            CloseAllUI();
            OpenSessionUI();
            Txt_Score.text = "0";
        }
        #endregion
        #region Events: OnSessionEnd
        private void OnSessionEnd(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionEnd", GLogName.UIManager);

        }

        #endregion

        #region Events: OnSetScoreText
        private void OnSetScoreText(int score)
        {
            Txt_Score.gameObject.SetActive(true);
            Txt_Score.text = score.ToString();
        }

        #endregion

        #region Events: ShowLocation

        private void ShowLocation(double lat, double log)
        {
            Debug.Log("ShowLocation");

            var latitude = String.Format("{0:0.00}", lat);
            var longitude = String.Format("{0:0.00}", log);
            //tapToStartText = "Tap To REPLAY in \n latitude" + latitude + "\n longitude " + longitude;

            TxtLocation.text = "latitude" + latitude + "\n longitude " + longitude;
        }

        #endregion

        #region ButtonClick: BuyProduct
        public void ButtonClick_BuyProduct(string productId)
        {
            Debug.Log("ButtonClick_BuyProduct");

            DelegateStore.BuyProduct?.Invoke(productId);
        }
        #endregion

        #region ButtonClick: ShowLeaderboard
        public void ButtonClick_ShowLeaderboard()
        {
            DelegateStore.ShowLeaderboard?.Invoke();
        }
        #endregion

        #region ButtonClick: ShowLeaderboard
        public void ButtonClick_SignOut()
        {
            DelegateStore.SignOut?.Invoke();
        }
        #endregion

        #region ButtonClick: ShowLeaderboard
        public void ButtonClick_ShowAchievements()
        {
            DelegateStore.ShowAchievements?.Invoke();
        }
        #endregion


        #region ButtonClick: ShowLeaderboard
        public void ButtonClick_ShowNearbyDevicePopup()
        {
            DelegateStore.ShowPopup?.Invoke(PopupType.NearbyDevice);
        }
        #endregion


        #region CoreMethods
        private void CloseAllUI()
        {
            CloseSessionUI();
            CloseMenuUI();
        }

        private void OpenSessionUI()
        {
            for (int i = 0; i < _SessionGameObjects.Length; i++)
            {
                _SessionGameObjects[i].SetActive(true);
            }
        }

        private void CloseSessionUI()
        {

            for (int i = 0; i < _SessionGameObjects.Length; i++)
            {
                _SessionGameObjects[i].SetActive(false);
            }
        }
        private void OpenMenuUI()
        {
            for (int i = 0; i < _MenuGameObjects.Length; i++)
            {
                _MenuGameObjects[i].SetActive(true);
            }
        }
        private void CloseMenuUI()
        {
            for (int i = 0; i < _MenuGameObjects.Length; i++)
            {
                _MenuGameObjects[i].SetActive(false);
            }
        }
        #endregion


        #region Callback: OnSetInAppPurchaseButton
        private void OnSetInAppPurchaseButton(bool state)
        {
            G_RemoveAds.SetActive(state);
        }
        #endregion

        #region Event: TapToPlayClick

        public void ButtonClick_TapToPlay()
        {
            DelegateStore.TapToPlay?.Invoke();
        }

        #endregion
    }
}