using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using HuaweiMobileServices.Nearby;
using HuaweiMobileServices.Nearby.Discovery;
using HuaweiMobileServices.Nearby.Transfer;

using StackGamePlay;

using System;

using UnityEngine;

public class NearbyServer : GNearbyBehaviour
{

    #region Variables

    public Action<string, ConnectInfo> OnEstablish { get; set; }
    public NearbyManagerConnectListener nearbyManagerListener;
    private string clientEndPointID;
    public Action<string, ConnectResult> OnResult { get; set; }
    public Action<string> OnDisconnected { get; set; }
    private NearbyManagerDataListener dataCallBack;
    private string serverEndPointID;

    #endregion

    #region Singleton

    private static NearbyServer instance;

    public static NearbyServer Instance { get => instance; set => instance = value; }

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
        GLog.Log($"Awake", GLogName.NearbyServer);

        Singleton();
    }

    private void OnEnable()
    {
        GLog.Log($"OnEnable", GLogName.NearbyServer);

        DelegateStore.Touch += OnTouch;
        DelegateStore.TapToPlay += OnTapToPlay;

    }

    private void OnDisable()
    {
        GLog.Log($"OnDisable", GLogName.NearbyServer);

        DelegateStore.Touch -= OnTouch;
        DelegateStore.TapToPlay -= OnTapToPlay;

    }

    void Start()
    {
        GLog.Log($"Start", GLogName.NearbyServer);

        nearbyManagerListener = new NearbyManagerConnectListener(this);

    }

    #endregion

    protected override void SendData()
    {
        GLog.Log($"SendData", GLogName.NearbyServer);
    }


    public void AcceptConnectionRequest(string endpointId, ConnectInfo connectInfo)
    {
        dataCallBack = new NearbyManagerDataListener(this);
        HMSNearbyServiceManager.Instance.AcceptConnectionRequest(endpointId, connectInfo, dataCallBack);
    }

    private void ServerConnected()
    {
        Debug.Log("[HMS] NearbyManager ServerConnected");
    }

    public class NearbyManagerConnectListener : IConnectCallBack
    {
        private readonly NearbyServer nearbyManagerObject;
        private static string receivedMessage = "Receive Success";
        public NearbyManagerConnectListener(NearbyServer nearbyManager)
        {
            Debug.Log("[HMS] NearbyManager NearbyManagerListener");

            nearbyManagerObject = nearbyManager;
        }

        public void onEstablish(string endpointId, ConnectInfo connectionInfo)
        {
            Debug.Log("[HMS] NearbyManager onEstablish");
            // Authenticating the Connection
            nearbyManagerObject.AcceptConnectionRequest(endpointId, connectionInfo);
            nearbyManagerObject.OnEstablish?.Invoke(endpointId, connectionInfo);
            Debug.Log("[HMS] NearbyManager onEstablish. Client accept connection from " + endpointId);
        }

        public void onResult(string endpointId, ConnectResult resultObject)
        {
            Debug.Log("[HMS] NearbyManager onResult");

            if (resultObject.Status.StatusCode == StatusCode.STATUS_SUCCESS)
            {
                /* The connection was established successfully, we can exchange data. */
                Debug.Log("[HMS] NearbyManager Connection Established. STATUS SUCCESS");
                HMSNearbyServiceManager.Instance.StopScanning();
                nearbyManagerObject.ServerConnected();
            }
            else if (resultObject.Status.StatusCode == StatusCode.STATUS_CONNECT_REJECTED)
            {
                Debug.Log("[HMS] NearbyManager Connection Established. STATUS REJECTED");
            }
            else
            {
                Debug.Log("[HMS] NearbyManager Connection Established.  status code" + resultObject.Status.StatusCode);
            }
            nearbyManagerObject.OnResult?.Invoke(endpointId, resultObject);
            nearbyManagerObject.clientEndPointID = endpointId;
        }

        public void onDisconnected(string p0)
        {
            Debug.Log("[HMS] NearbyManager onDisconnected");
            nearbyManagerObject.OnDisconnected?.Invoke(p0);
        }
    }

    public class NearbyManagerDataListener : IDataCallback
    {
        private readonly NearbyServer nearbyManagerObject;
        private static string receivedMessage = "Receive Success";

        public NearbyManagerDataListener(NearbyServer nearbyManager)
        {
            Debug.Log("[HMS] NearbyManager NearbyManagerListener");

            nearbyManagerObject = nearbyManager;
        }

        public void onReceived(string endpointId, Data dataReceived)
        {
            if (dataReceived.DataType == Data.Type.BYTES)
            {
                string msg = System.Text.Encoding.UTF8.GetString(dataReceived.AsBytes);
                if (msg.Equals(receivedMessage))
                {
                    Debug.Log("[HMS] NearbyManager Received ACK. Send next.");
                }
            }
        }

        public void onTransferUpdate(string endpointId, TransferStateUpdate update)
        {
            Debug.Log("[HMS] NearbyManager onTransferUpdate");
        }
    }

    #region Callback: Delegate

    private void OnTouch()
    {
        GLog.Log($"OnTouch", GLogName.NearbyServer);

        if (!GameManager.Instance.SessionIsPlaying)
        {
            return;
        }

        if (NearbyDeviceManager.Instance.IsSender)
        {
            HMSNearbyServiceManager.Instance.SendData(clientEndPointID, "gameclick");
            Debug.Log("SENDYES TOUCH");
        }

    }

    private void OnTapToPlay()
    {
        GLog.Log($"OnTapToPlay", GLogName.NearbyServer);

        if (GameManager.Instance.SessionIsPlaying)
        {
            return;
        }

        if (NearbyDeviceManager.Instance.IsSender)
        {
            HMSNearbyServiceManager.Instance.SendData(clientEndPointID, "taptostart");
            Debug.Log("SENDYES TAPTOPLAY");
        }

    }

    #endregion

}
