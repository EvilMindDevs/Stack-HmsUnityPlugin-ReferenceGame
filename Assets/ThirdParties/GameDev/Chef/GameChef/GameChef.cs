using GameDev.Library;

using System;
using System.Collections;
using System.Threading.Tasks;

using UnityEngine;

namespace GameDev.MiddleWare
{
    public class GameChef : GSingleton<GameChef>
    {


        #region Unity: OnEnable

        private void OnEnable()
        {
            GLog.Log($"OnEnable", GLogName.GameChef);

            this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);
        }

        #endregion

        #region Unity: OnDisable

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.GameChef);

            this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);
        }

        #endregion

        #region Unity: Start

        private async void Start()
        {

            StartCoroutine(PrepareSession());

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
            Physics.autoSyncTransforms = false;
            Physics.reuseCollisionCallbacks = true;

            await Task.Delay(50);
        }

        IEnumerator PrepareSession()
        {
            yield return new WaitUntil(() => RealTimeDataStore.RemoteConfigIsOk);
            yield return new WaitUntil(() => RealTimeDataStore.CrashIsOk);
            this.DispatchEvent(new GEvent<object>(GEventName.SESSION_PREPARE, this));
        }

        #endregion

        #region Unity: Update

        private void Update()
        {
        }

        #endregion



        #region Events: OnSessionPrepare

        private void OnSessionPrepare(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionPrepare", GLogName.GameChef);


        }

        #endregion

        #region Events: OnSessionStart

        private void OnSessionStart(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionStart", GLogName.GameChef);

        }

        #endregion

        #region Events: OnSessionEnd

        private void OnSessionEnd(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionEnd", GLogName.GameChef);

        }

        #endregion

    }
}

