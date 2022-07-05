
using GameDev.Library;

namespace GameDev.MiddleWare
{
    public class AnalyticsManager : GAnalytics
    {

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

    }
}

