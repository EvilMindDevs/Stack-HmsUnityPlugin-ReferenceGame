
using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using HuaweiMobileServices.IAP;
using HuaweiMobileServices.Utils;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace StackGamePlay
{
    public class IapManager : MonoBehaviour
    {


        #region Singleton
        private static IapManager instance;

        public static IapManager Instance { get => instance; set => instance = value; }

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
            GLog.Log($"OnEnable", GLogName.IAPManager);
            this.AddListener<object>(GEventName.ACCOUNT_KIT_IS_READY, OnAccountKitIsReady);
            HMSIAPManager.Instance.OnBuyProductSuccess += OnBuyProductSuccess;
            HMSIAPManager.Instance.OnCheckIapAvailabilitySuccess += OnCheckIapAvailabilitySuccess;
            HMSIAPManager.Instance.OnCheckIapAvailabilityFailure += OnCheckIapAvailabilityFailure;
            DelegateStore.BuyProduct += OnBuyProduct;
            HMSIAPManager.Instance.OnObtainOwnedPurchasesSuccess += OnObtainOwnedPurchasesSuccess;
        }

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.IAPManager);
            this.RemoveListener<object>(GEventName.ACCOUNT_KIT_IS_READY, OnAccountKitIsReady);
            HMSIAPManager.Instance.OnBuyProductSuccess -= OnBuyProductSuccess;
            HMSIAPManager.Instance.OnCheckIapAvailabilitySuccess -= OnCheckIapAvailabilitySuccess;
            HMSIAPManager.Instance.OnCheckIapAvailabilityFailure -= OnCheckIapAvailabilityFailure;
            DelegateStore.BuyProduct -= OnBuyProduct;
            HMSIAPManager.Instance.OnObtainOwnedPurchasesSuccess -= OnObtainOwnedPurchasesSuccess;
        }
        #endregion

        #region Events: CheckIapAvailability
        private void OnCheckIapAvailabilityFailure(HMSException obj)
        {
            GLog.Log($"OnCheckIapAvailabilityFailure", GLogName.IAPManager);
            RealTimeDataStore.IapIsOk = false;
        }
        private void OnCheckIapAvailabilitySuccess()
        {
            GLog.Log($"OnCheckIapAvailabilitySuccess", GLogName.IAPManager);
            RealTimeDataStore.IapIsOk = true;
        }
        #endregion
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

        #region Events: OnSessionPrepare

        private void OnAccountKitIsReady(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnAppInitialize", GLogName.IAPManager);
            StartCoroutine(ControlIAP());
        }

        IEnumerator ControlIAP()
        {
            HMSIAPManager.Instance.CheckIapAvailability();
            yield return new WaitUntil(() => RealTimeDataStore.IapIsOk);
            HMSIAPManager.Instance.RestorePurchases((restoredProducts) =>
             {
                 var productPurchasedList = new List<InAppPurchaseData>(restoredProducts.InAppPurchaseDataList);
                 foreach (var item in productPurchasedList)
                 {
                     GLog.Log($" ProductId {item.ProductId}", GLogName.IAPManager);
                 }
             });
        }

        #endregion

        #region Events: OnBuyProduct
        private void OnBuyProduct(string productID)
        {
            HMSIAPManager.Instance.BuyProduct(productID);
        }
        #endregion
        #region Events: OnObtainOwnedPurchasesSuccess
        private void OnObtainOwnedPurchasesSuccess(OwnedPurchasesResult obj)
        {
            GLog.Log($"OnObtainOwnedPurchasesSuccess {obj.InAppPurchaseDataList.Count}", GLogName.IAPManager);
            foreach (var item in obj.InAppPurchaseDataList)
            {
                GLog.Log($"Purchased items {item.ProductId}", GLogName.IAPManager);
            }
        }
    }
    #endregion

}



