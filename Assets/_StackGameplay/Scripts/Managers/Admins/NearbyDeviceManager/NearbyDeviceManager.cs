
using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using HuaweiMobileServices.AuthService;
using HuaweiMobileServices.CloudDB;
using HuaweiMobileServices.Common;
using HuaweiMobileServices.IAP;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.Nearby.Discovery;
using HuaweiMobileServices.Nearby.Transfer;
using HuaweiMobileServices.Utils;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Android;

namespace StackGamePlay
{
    public class NearbyDeviceManager : MonoBehaviour
    {

        #region Variables

        private HMSNearbyServiceManager nearbyManager;

        private NearbyDeviceType nearbyDeviceType;
        private NearbyDeviceStatus nearbyDeviceStatus;

        private static readonly String scanInfo = "testInfo", remoteEndpointId = "RemoteEndpointId", transmittingMessage = "Receive Success",
      myNameStr = "MyNameTest", mEndpointName = "testName", mFileServiceId = "com.refapp.stack.huawei";

        #endregion

        #region Properties

        public bool IsSender => nearbyDeviceType == NearbyDeviceType.Server;

        #endregion

        #region Singleton

        private static NearbyDeviceManager instance;

        public static NearbyDeviceManager Instance { get => instance; set => instance = value; }

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
            GLog.Log($"Awake", GLogName.NearbyDeviceManager);

            Singleton();

            nearbyDeviceStatus = NearbyDeviceStatus.InActive;
        }

        private void OnEnable()
        {
            GLog.Log($"OnEnable", GLogName.NearbyDeviceManager);

            DelegateStore.SetNearbyDeviceType += OnSetNearbyDeviceType;
        }

        public void SetNearbyStatus(NearbyDeviceStatus status)
        {
            nearbyDeviceStatus = status;
        }

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.NearbyDeviceManager);

            DelegateStore.SetNearbyDeviceType -= OnSetNearbyDeviceType;
        }


        public void Start()
        {
            GLog.Log($"Start", GLogName.NearbyDeviceManager);

            nearbyManager = HMSNearbyServiceManager.Instance;

            nearbyManager.scanInfo = scanInfo;
            nearbyManager.remoteEndpointId = remoteEndpointId;
            nearbyManager.transmittingMessage = transmittingMessage;
            nearbyManager.myNameStr = myNameStr;
            nearbyManager.mEndpointName = mEndpointName;
            nearbyManager.mFileServiceId = mFileServiceId;

        }



        #endregion

        public void BeServer()
        {
            HMSNearbyServiceManager.Instance.SendFilesInner(NearbyServer.Instance.nearbyManagerListener);
        }

        public void BeClient()
        {
            HMSNearbyServiceManager.Instance.OnScanResult(NearbyClient.Instance.nearbyManagerListener);
        }

        public void HandlePermissions()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
            {
                Permission.RequestUserPermission(Permission.CoarseLocation);
            }
        }

        #region Callback: Delegate

        private void OnSetNearbyDeviceType(NearbyDeviceType _nearbyDeviceType)
        {
            nearbyDeviceType = _nearbyDeviceType;
            nearbyDeviceStatus = NearbyDeviceStatus.Active_ReadyToConnect;

            if (_nearbyDeviceType == NearbyDeviceType.Client)
            {
                BeClient();
            }
            else if (_nearbyDeviceType == NearbyDeviceType.Server)
            {
                BeServer();
            }

        }

        #endregion


    }

    public enum NearbyDeviceType
    {
        Server = 1,
        Client = 2
    }

    public enum NearbyDeviceStatus
    {
        InActive = 1,
        Active_Connected = 2,
        Active_ReadyToConnect = 3
    }


}




