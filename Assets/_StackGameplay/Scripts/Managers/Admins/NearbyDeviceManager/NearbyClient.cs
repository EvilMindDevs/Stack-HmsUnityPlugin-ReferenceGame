using GameDev.Library;

using HmsPlugin;

using HuaweiMobileServices.Nearby;
using HuaweiMobileServices.Nearby.Discovery;
using HuaweiMobileServices.Nearby.Transfer;

using StackGamePlay;

using System;

using UnityEngine;

public class NearbyClient : GNearbyBehaviour
{

    #region Singleton

    private static NearbyClient instance;

    public static NearbyClient Instance { get => instance; set => instance = value; }

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

    #region Variables

    public NearbyManagerListener nearbyManagerListener;
    public Action<string, ScanEndpointInfo> OnFound { get; set; }
    public Action<string, ConnectInfo> OnEstablish { get; set; }
    public Action<string, ConnectResult> OnResult { get; set; }

    private NearbyManagerDataListener dataCallBack;
    private NearbyManagerConnectListener connectCallBack;
    private string clientEndPointID;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GLog.Log($"Awake", GLogName.NearbyClient);

        Singleton();
    }

    private void OnEnable()
    {
        GLog.Log($"OnEnable", GLogName.NearbyClient);

    }

    private void OnDisable()
    {
        GLog.Log($"OnDisable", GLogName.NearbyClient);

    }

    void Start()
    {
        nearbyManagerListener = new NearbyManagerListener(this);
    }

    #endregion


    public void AcceptConnectionRequest(string endpointId, ConnectInfo connectInfo)
    {
        Debug.Log("[HMS] NearbyManager AcceptConnectionRequest");

        dataCallBack = new NearbyManagerDataListener(this);
        HMSNearbyServiceManager.Instance.AcceptConnectionRequest(endpointId, connectInfo, dataCallBack);
    }
    public void InitiateConnection(string endpointId, ScanEndpointInfo scanEndpointInfo)
    {
        Debug.Log("[HMS] NearbyManager InitiateConnection");

        connectCallBack = new NearbyManagerConnectListener(this);
        HMSNearbyServiceManager.Instance.InitiateConnection(endpointId, scanEndpointInfo, connectCallBack);
    }
    public class NearbyManagerListener : IScanEndpointCallback
    {
        private readonly NearbyClient nearbyManagerObject;

        public NearbyManagerListener(NearbyClient nearbyManager)
        {
            Debug.Log("[HMS] NearbyManager NearbyManagerListener");

            nearbyManagerObject = nearbyManager;
        }

        public void onFound(string endpointId, ScanEndpointInfo discoveryEndpointInfo)
        {
            Debug.Log("[HMS] NearbyManager onFound");

            nearbyManagerObject.InitiateConnection(endpointId, discoveryEndpointInfo);
            Debug.Log("[HMS] NearbyManager onFound2");

            nearbyManagerObject.OnFound?.Invoke(endpointId, discoveryEndpointInfo);
            Debug.Log("[HMS] NearbyManager onFound3");

        }

        public void onLost(string endpointId)
        {
            Debug.Log("[HMS] NearbyManager onLost");
            throw new System.NotImplementedException();
        }
    }
    public class NearbyManagerConnectListener : IConnectCallBack
    {
        private readonly NearbyClient nearbyManagerObject;
        public NearbyManagerConnectListener(NearbyClient nearbyManager)
        {
            Debug.Log("[HMS] NearbyManager NearbyManagerListener");

            nearbyManagerObject = nearbyManager;
        }
        public void onEstablish(string endpointId, ConnectInfo connectionInfo)
        {
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
        }

        public void onDisconnected(string p0)
        {
            Debug.Log($"[HMS] onDisconnected {p0}");
        }

    }
    public class NearbyManagerDataListener : IDataCallback
    {
        private readonly NearbyClient nearbyManagerObject;
        private static string receivedMessage = "Receive Success";

        public NearbyManagerDataListener(NearbyClient nearbyManager)
        {
            Debug.Log("[HMS] NearbyManager NearbyManagerDataListener");

            nearbyManagerObject = nearbyManager;
        }

        public void onReceived(string endpointId, Data dataReceived)
        {
            Debug.Log($"onReceived{endpointId}");

            Debug.Log($"DataType {dataReceived.DataType}");

            if (dataReceived.DataType == Data.Type.BYTES)
            {

                string msg = System.Text.Encoding.UTF8.GetString(dataReceived.AsBytes);
                Debug.Log($"onReceived2 {msg}");

                if (msg.Equals("taptostart"))
                {
                    Debug.Log("TAPTOSTART");

                    DelegateStore.TapToPlay?.Invoke();
                }
                else if (msg.Equals("gameclick"))
                {
                    Debug.Log("GAMECLICK");

                    DelegateStore.Touch?.Invoke();

                }
            }
        }

        public void onTransferUpdate(string endpointId, TransferStateUpdate update)
        {
            Debug.Log("[HMS] NearbyManager onTransferUpdate");
        }
    }

    private void ServerConnected()
    {
        Debug.Log("[HMS] NearbyManager ServerConnected ");
    }

    protected override void SendData()
    {
    }

}
