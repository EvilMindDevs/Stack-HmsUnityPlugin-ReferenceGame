
using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using HuaweiMobileServices.IAP;
using HuaweiMobileServices.Utils;

using System;
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
            GLog.Log($"OnEnable", GLogName.IAPManager);
            this.AddListener<object>(GEventName.ACCOUNT_KIT_IS_READY, OnAccountKitIsReady);
            HMSIAPManager.Instance.OnBuyProductSuccess += OnBuyProductSuccess;
            HMSIAPManager.Instance.OnInitializeIAPSuccess += OnInitializeIAPSuccess;
            HMSIAPManager.Instance.OnInitializeIAPFailure += OnInitializeIAPFailure;
            DelegateStore.BuyProduct += OnBuyProduct;
            HMSIAPManager.Instance.OnObtainOwnedPurchasesSuccess += OnObtainOwnedPurchasesSuccess;
        }

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.IAPManager);
            this.RemoveListener<object>(GEventName.ACCOUNT_KIT_IS_READY, OnAccountKitIsReady);
            HMSIAPManager.Instance.OnBuyProductSuccess -= OnBuyProductSuccess;
            HMSIAPManager.Instance.OnInitializeIAPSuccess -= OnInitializeIAPSuccess;
            HMSIAPManager.Instance.OnInitializeIAPFailure -= OnInitializeIAPFailure;
            DelegateStore.BuyProduct -= OnBuyProduct;
            HMSIAPManager.Instance.OnObtainOwnedPurchasesSuccess -= OnObtainOwnedPurchasesSuccess;
        }

        #endregion

        #region Events: CheckIapAvailability
        private void OnCheckIapAvailabilityFailure(HMSException obj)
        {

        }
        private void OnCheckIapAvailabilitySuccess()
        {

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
            HMSIAPManager.Instance.InitializeIAP();

            yield return new WaitUntil(() => RealTimeDataStore.IapIsOk);

            HMSIAPManager.Instance.RestorePurchaseRecords((restoredProducts) =>
            {
                foreach (var item in restoredProducts.InAppPurchaseDataList)
                {
                    if ((IAPProductType)item.Kind == IAPProductType.Consumable)
                    {
                        Debug.Log($"Consumable: ProductId {item.ProductId} , SubValid {item.SubValid} , PurchaseToken {item.PurchaseToken} , OrderID  {item.OrderID}");
                    }
                }
            });

            HMSIAPManager.Instance.RestoreOwnedPurchases((restoredProducts) =>
            {
                foreach (var item in restoredProducts.InAppPurchaseDataList)
                {
                    if ((IAPProductType)item.Kind == IAPProductType.Subscription)
                    {
                        Debug.Log($"Subscription: ProductId {item.ProductId} , ExpirationDate {item.ExpirationDate} , AutoRenewing {item.AutoRenewing} , PurchaseToken {item.PurchaseToken} , OrderID {item.OrderID}");
                    }

                    else if ((IAPProductType)item.Kind == IAPProductType.NonConsumable)
                    {
                        Debug.Log($"NonConsumable: ProductId {item.ProductId} , DaysLasted {item.DaysLasted} , SubValid {item.SubValid} , PurchaseToken {item.PurchaseToken} ,OrderID {item.OrderID}");
                    }
                }
            });

        }

        #endregion

        #region Events: OnBuyProduct
        private void OnBuyProduct(string productID)
        {
            HMSIAPManager.Instance.PurchaseProduct(productID);
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
        #endregion

        #region Events: OnInitializeIAPFailure
        private void OnInitializeIAPFailure(HMSException obj)
        {
            GLog.Log($"OnInitializeIAPFailure", GLogName.IAPManager);
            RealTimeDataStore.IapIsOk = false;
        }
        #endregion

        #region Events: OnInitializeIAPSuccess
        private void OnInitializeIAPSuccess()
        {
            GLog.Log($"OnInitializeIAPSuccess", GLogName.IAPManager);
            RealTimeDataStore.IapIsOk = true;
        }
        #endregion

    }





}



