
using UnityEngine;

namespace GameDev.MiddleWare
{
    public class DataStore
    {

        #region Level: No

        public static int LevelNo
        {
            get => PlayerPrefs.GetInt("LEVEL_NO", 1);

            set => PlayerPrefs.SetInt("LEVEL_NO", value);
        }

        #endregion


    }

}