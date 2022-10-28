
using UnityEngine;

namespace StackGamePlay
{
    public class Warehouse
    {
       public static bool RemoveAds
        {
            get
            {
                return PlayerPrefs.GetString("RemoveAds", "False") == "True";
            }
           set
            {
                PlayerPrefs.SetString("RemoveAds", value.ToString());
                DelegateStore.SetAdsState?.Invoke(!value);
           }
        }


    }
}