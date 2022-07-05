
using GameDev.Library;
using GameDev.MiddleWare;

using HmsPlugin;

using HuaweiMobileServices.Push;
using HuaweiMobileServices.Utils;

using System;
using System.Collections;

using UnityEngine;

namespace StackGamePlay
{
    public class CommunicationManager : MonoBehaviour
    {

        #region Singleton

        private static CommunicationManager instance;

        public static CommunicationManager Instance { get => instance; set => instance = value; }

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
        }
        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.AdsManager);
            this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);
        }
        private void Start()
        {
            Debug.Log("[HMS] Push Start");
            StartCoroutine(LateStart(0f));
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

        IEnumerator LateStart(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            HMSPushKitManager.Instance.OnTokenSuccess = OnNewToken;
            HMSPushKitManager.Instance.OnTokenFailure = OnTokenError;
            HMSPushKitManager.Instance.OnTokenBundleSuccess = OnNewToken;
            HMSPushKitManager.Instance.OnTokenBundleFailure = OnTokenError;
            HMSPushKitManager.Instance.OnMessageSentSuccess = OnMessageSent;
            HMSPushKitManager.Instance.OnSendFailure = OnSendError;
            HMSPushKitManager.Instance.OnMessageDeliveredSuccess = OnMessageDelivered;
            HMSPushKitManager.Instance.OnMessageReceivedSuccess = OnMessageReceived;
            HMSPushKitManager.Instance.OnNotificationMessage = OnNotificationMessage;
            HMSPushKitManager.Instance.NotificationMessageOnStart = NotificationMessageOnStart;
            HMSPushKitManager.Instance.Init();
        }
        private void OnNotificationMessage(NotificationData data)
        {
            Debug.Log("[HMSPushDemo] CmdType: " + data.CmdType);
            Debug.Log("[HMSPushDemo] MsgId: " + data.MsgId);
            Debug.Log("[HMSPushDemo] NotifyId: " + data.NotifyId);
            Debug.Log("[HMSPushDemo] KeyValueJSON: " + data.KeyValueJSON);
        }
        private void NotificationMessageOnStart(NotificationData data)
        {
            Debug.Log("[HMSPushDemo] CmdType: " + data.CmdType);
            Debug.Log("[HMSPushDemo] MsgId: " + data.MsgId);
            Debug.Log("[HMSPushDemo] NotifyId: " + data.NotifyId);
            Debug.Log("[HMSPushDemo] KeyValueJSON: " + data.KeyValueJSON);

            Debug.Log("TODO");
        }
        public void OnNewToken(string token)
        {
            Debug.Log($"[HMS] Push token from OnNewToken is {token}");
        }
        public void OnTokenError(Exception e)
        {
            Debug.Log("Error asking for Push token");
            Debug.Log(e.StackTrace);
        }
        public void OnMessageReceived(RemoteMessage remoteMessage)
        {

        }

        public void OnNewToken(string token, Bundle bundle)
        {
            Debug.Log($"[HMS] Push token from OnNewToken is {token}");
        }

        public void OnTokenError(Exception exception, Bundle bundle)
        {
            Debug.Log("Error asking for Push token");
            Debug.Log(exception.StackTrace);
        }

        public void OnMessageSent(string msgId)
        {
            Debug.Log(msgId);
        }
        public void OnMessageDelivered(string msgId, Exception exception)
        {
            Debug.Log("Message Delivered");
            Debug.Log(exception.StackTrace + " , Message ID:" + msgId);
        }
        public void OnSendError(string msgId, Exception exception)
        {
            Debug.Log(exception.StackTrace + " , Message ID:" + msgId);
        }
    }
}