
<!-- # ![mg-builder](/img~/mg-builder.png) -->

# Reference Game with Huawei Mobile Services

Stack is one of those simple, silly, addictive games that
you can lose an entire afternoon to if you're not careful.
It became hit in hypercasual game industry in 2016. This is a clone of Stack.The goal of the game is to stack the moving blocks over
each other as high as you can.

<table>

  <tr>
    <td><img src="https://raw.githubusercontent.com/EvilMindDevs/Stack-HmsUnityPlugin-ReferenceGame/Master/Assets/Images/scene.jpg" width=250 height=535></td>
    <td><img src="https://raw.githubusercontent.com/EvilMindDevs/Stack-HmsUnityPlugin-ReferenceGame/Master/Assets/Images/pic3.jpg" width=250 height=535></td>
    <td><img src="https://raw.githubusercontent.com/EvilMindDevs/Stack-HmsUnityPlugin-ReferenceGame/Master/Assets/Images/pic5.jpg" width=250  height=535></td>

  </tr>
 </table>

In this project, we explain the use of [Unity Plugin](https://github.com/EvilMindDevs/hms-unity-plugin) of Huawei Mobile Services on this game.

# Huawei Mobile Services Plugin

The HMS Unity plugin helps you integrate all the power of Huawei Mobile Services in your Unity game.

[Requirements](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/getting-started/what-you-will-need)

[Quick Start](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/getting-started/quick-start)



In this project we are using those kits:

* [Account](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/account-kit)
* [IAP](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/in-app-purchases)
* [Ads](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/ads-kit)
* [Push](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/push-kit)
* [Game Service](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/game-service)
* [Analytics](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/analytics-kit)
* [Crash Service](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/crash-service)
* [Remote Configuration](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/remote-configuration)
* [Auth Service](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/auth-service)
* [App Linking](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/app-linking)
* [In App Comment](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/in-app-comments)
* [App Messaging](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/app-messaging)
* [Nearby Service](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/nearby-service)
* [Cloud DB](https://evilminddevs.gitbook.io/hms-unity-plugin-beta/kits-and-services/cloud-db)

## Account Kit

`Purpose In Project` :  Sign in automatically.


 `Use In Project` : By calling this method `HMSAccountKitManager.Instance.SignIn();` at GameManager.cs 121

   ```csharp

        #region Unity: Start

        private void Start()
        {
            GLog.Log($"Start", GLogName.GameManager);

            HMSAccountKitManager.Instance.SignIn();
        }

        #endregion

  ```



## IAP

`Purpose In Project` :  Selling the product of "Remove Ads".


 `Use In Project` : 
 
 1. By clicking "Remove Ads" button in game menu.This button calls this method  `HMSAccountKitManager.Instance.SignIn();` at UIView.cs 231

   ```csharp

    #region ButtonClick: BuyProduct

    public void ButtonClick_BuyProduct(string productId)
    {
        DelegateStore.BuyProduct?.Invoke(productId);
    }

    #endregion

  ```

2. `DelegateStore.BuyProduct?.Invoke(productId);` command invokes `OnBuyProduct(string productID)` at IAPManager.cs 154


```csharp


    #region Events: OnBuyProduct

    private void OnBuyProduct(string productID)
    {
        HMSIAPManager.Instance.BuyProduct(productID);
    }

    #endregion


```

  3. If purchase request return success `OnBuyProductSuccess(PurchaseResultInfo obj)` callback method will be called automatically.In this method we handle the having "Remove Ads" item.


   ```csharp

    #region Events: BuyProductSuccess

    private void OnBuyProductSuccess(PurchaseResultInfo obj)
    {
        if (obj.InAppPurchaseData.ProductId == "1006")
        {
            GLog.Log($"OnBuyProductSuccess {obj.InAppPurchaseData.ProductName}", GLogName.IAPManager);

            Warehouse.RemoveAds = true;
        }
    }

    #endregion


  ```




## Ads Kit

`Purpose In Project` :  Monetizing with Interstial Ads and Banner Ads.


`Use In Project` : 
 
 1. Banner Ads : By calling this method `ShowBanner()` at AdsManager.cs 102.We are calling this method at first,and you can hide banner ads by calling `HideBanner()` at AdsManager.cs 112.

   ```csharp
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
  ```


2. Interstial Ads : By calling this method `ShowInterstitial()` at AdsManager.cs 145.We are calling this method at the end of sessions.

```csharp
    #region Ads: Interstitial

    public void ShowInterstitial()
    {
        if (!IsAdsActive)
            return;

        GLog.Log($"ShowInterstitial", GLogName.AdsManager);

        HMSAdsKitManager.Instance.ShowInterstitialAd();
    }

    #endregion
```



## Push Kit

`Purpose In Project` :  Send notificatons to users when we want.

 `Use In Project` : We can send notificaton at [App Gallery Connect]( https://developer.huawei.com/consumer/en/service/josp/agc/index.html#/).


## Game Service

`Purpose In Project` :  Handle the success of users.

`Use In Project` : 
 
 1. Leaderboard : We can see leaderboard by clicking leaderboard button in game menu ui.Button click calls `OnShowLeaderboard()` at ScoreManager.cs 292 and we can handle score by calling `SendScore(int score)` at ScoreManager.cs 277.

  ```csharp
    #region Event: ShowLeaderboard

    private void OnShowLeaderboard()
    {
        Debug.Log("OnShowLeaderboard");
        HMSLeaderboardManager.Instance.ShowLeaderboards();
    }

    #endregion
  ```

2. Achievement : By calling this method `SendScore(int score)` at ScoreManager.cs 226.We can handle the passing 10 points achievement.

  ```csharp
    public void SendScore(int score)
    {
        HMSLeaderboardManager.Instance.SubmitScore(LeaderBoardId, score);

        if (score >= 10)
        {
            HMSAchievementsManager.Instance.UnlockAchievement(AchievementId);
        }
    }

  ```




## Analytics Kit

`Purpose In Project` :  Analyse the session starts and ends count.

 `Use In Project` :  With those callbacks `OnSessionStart() , OnSessionEnd()` at AnalyticsManager.cs 15,26.

   ```csharp

        #region Events: OnSessionStart

        protected override void OnSessionStart(object sender, GEvent<object> eventData)
        {
            base.OnSessionStart(sender, eventData);

            HMSAnalyticsKitManager.Instance.SendEventWithBundle("Session_Start", "Session_Start", "OK");
        }

        #endregion

        #region Events: OnSessionEnd

        protected override void OnSessionEnd(object sender, GEvent<object> eventData)
        {
            base.OnSessionStart(sender, eventData);

            HMSAnalyticsKitManager.Instance.SendEventWithBundle("Session_End", "Session_End", "OK");
        }

        #endregion

  ```


## Crash Service

`Purpose In Project` : Learn the cause of unexpected exits of application.

 `Use In Project` : We can read the crash reports from [App Gallery Connect]( https://developer.huawei.com/consumer/en/service/josp/agc/index.html#/).



## Remote Configuration

`Purpose In Project` :  Modifying the game without deploying a new version.

 `Use In Project` :  We are fetching the values at the beginning of application.Then we are using that values in game mechanic.(GameManager.cs 147)

   ```csharp

        #region HMS: RemoteConfig

        private void RemoteConfig()
        {
            void OnFecthSuccess(ConfigValues config)
            {

                HMSRemoteConfigManager.Instance.Apply(config);

                var _blockMoveTimeCoeff = HMSRemoteConfigManager.Instance.GetValueAsDouble("BlockMoveTimeCoeff");
                var _backgroundColorIndex = HMSRemoteConfigManager.Instance.GetValueAsLong("BGColor");

                backgroundColorIndex = int.Parse(_backgroundColorIndex.ToString());
                blockMoveTimeCoeff = (float)_blockMoveTimeCoeff;

                Debug.Log($"coeff {blockMoveTimeCoeff}");
                Debug.Log($"BG Color {_backgroundColorIndex}");

                remoteConfigIsOk = true;
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


  ```

## Auth Service

`Purpose In Project` :  Potential need of login with

* WeChat
* QQ
* Weibo
* Apple
* Google
* Facebook
* Twitter

## App Linking

`Purpose In Project` :  Provide users to share game application with their friends.

 `Use In Project` :  By clicking sharing button in game menu,this calls the `ShareLongLink()` at AppLinkingManager.cs 177.

```csharp
    public void ShareLongLink()
    {
        Debug.Log("ShareLongLink");
        AGConnectAppLinking.ShareLink(longLink);
    }
```

## In App Comment

`Purpose In Project` : Providing users to submit ratings and make comments for app without leaving the application.

 `Use In Project` : Automatically running at the session end callback  `OnSessionEnd` at GameManager.cs 245.

```csharp
    #region Events: OnSessionEnd

    private void OnSessionEnd(object sender, GEvent<object> eventData)
    {
        GLog.Log($"OnSessionEnd", GLogName.GameManager);
        InAppComment.ShowInAppComment();
    }

    #endregion
```

## App Messaging

`Purpose In Project` : Remind the user of "Remove Ads" IAP product.

 `Use In Project` : Automatically running with `Session_End` analytic event.You can arrange this from AGC.

<table>

  <tr>
    <td><img src="https://raw.githubusercontent.com/EvilMindDevs/Stack-HmsUnityPlugin-ReferenceGame/Master/Assets/Images/appMessaging.png" width=543 height=323></td>

  </tr>
 </table>

## Nearby Service

`Purpose In Project` : Broadcasting the game as if it is being played live on another device by transferring input

 `Use In Project` : In "Pre" unity-scene , there is a NearbyDeviceManager gameobject.This includes `NearbyDeviceManager`, `NearbyServer` and `NearbyClient` scripts.We are transferring input to other device by these scripts.These scripts subscribes Touch and TapToPlay events at `DelegateStore`.

 
## CloudDB

`Purpose In Project` : Save all game session scores with session number (sequential) and display this at popup.

 `Use In Project` : We are saving session at `CloudDBManager` 157, AddSession method.

```csharp

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

```
We are showing ScoreTable popup by clicking ScoreTable Button at menu.This button calls following command  `UIView` 88.

```csharp

        Btn_ShowScoreTablePopup.onClick.AddListener(delegate ()
        {
             DelegateStore.ShowPopup?.Invoke(PopupType.ScoreTable);
        });

```

This command query sessions from CloudDB by calling QuerySessions method at `CloudDBManager` 141.


```csharp

        public void QuerySessions()
        {
            GLog.Log($"QuerySessions", GLogName.CloudDBManager);

            queryIsOK = false;
            gameSessionsList.Clear();

            CloudDBZoneQuery mCloudQuery = 
            CloudDBZoneQuery.Where(
              new AndroidJavaClass(GameSessionsClass));

            mCloudQuery.EqualTo("huaweiIdMail", GameManager.Instance.Uid);

            cloudDBManager.ExecuteQuery
            (
              mCloudQuery,
              CloudDBZoneQueryCloudDBZoneQueryPolicy.CLOUDDBZONE_CLOUD_CACHE
            );
        }

```
