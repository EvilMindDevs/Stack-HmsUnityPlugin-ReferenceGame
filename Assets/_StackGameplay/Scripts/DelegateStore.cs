
using HmsPlugin;

using System;
using System.Collections.Generic;

namespace StackGamePlay
{
    public static class DelegateStore
    {

        public static Action Touch;
        public static Action TapToPlay;

        public static Action ShowLeaderboard;
        public static Action ShowAchievements;
        public static Action<int> SetScore;
        public static Action<string> BuyProduct;
        public static Action<bool> SetAdsState;
        public static Action SignOut;
        public static Action Share;
        public static Action<float> AdvanceProgressBar;

        public static Func<List<GameSessions>> ObtainSessions;

        public static Action<PopupType> ShowPopup;

        public static Action<string> ShowNearbyDeviceStatus;
        public static Action<NearbyDeviceType> SetNearbyDeviceType;

       public static Action<double,double> ShowLocation;

    }
}