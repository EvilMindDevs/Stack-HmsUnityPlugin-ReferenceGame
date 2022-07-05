
using GameDev.Library;

using UnityEngine;

namespace GameDev.MiddleWare
{
    public abstract class GAnalytics : GSingleton<GAnalytics>
    {

        //#region Initialize

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //private static void Initialize()
        //{
        //    GLog.Log($"Initialize", GLogName.GameManager);

        //    var gameObject = new GameObject();
        //    gameObject.AddComponent<AnalyticsManager>();
        //    gameObject.name = "AnalyticsManager";
        //    DontDestroyOnLoad(gameObject);
        //}

        //#endregion


        #region Unity: OnEnable

        private void OnEnable()
        {
            GLog.Log($"OnEnable", GLogName.AnalyticsManager);

            this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);
        }

        #endregion

        #region Unity: OnDisable

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.AnalyticsManager);

            this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);
        }

        #endregion


        #region Events: OnSessionStart

        protected virtual void OnSessionStart(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionStart", GLogName.AnalyticsManager);
        }

        #endregion

        #region Events: OnSessionEnd

        protected virtual void OnSessionEnd(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionEnd", GLogName.AnalyticsManager);
        }

        #endregion

    }
}
