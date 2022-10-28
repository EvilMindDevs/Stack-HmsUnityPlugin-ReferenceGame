using GameDev.Library;

using UnityEngine;

namespace GameDev.MiddleWare
{
    public class LogManager : GSingleton<LogManager>
    {

        #region Initialize

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<LogManager>();
            gameObject.name = "LogManager";
            DontDestroyOnLoad(gameObject);
        }

        #endregion


        #region Unity: Awake

        private void Awake()
        {
            Register();
        }

        #endregion


        #region Register

        private void Register()
        {
            GLog.Log($"LogRegister", GLogName.LogManager);

            //GLog.Register(GLogName.LogManager, GLogState.Enable, GColors.YellowPantone);
            //GLog.Register(GLogName.GameManager, GLogState.Enable, GColors.MagentaProcess);
            //GLog.Register(GLogName.InputManager, GLogState.Enable, GColors.AeroBlue);
            //GLog.Register(GLogName.AnalyticsManager, GLogState.Enable, GColors.OldGold);
            //GLog.Register(GLogName.LevelManager, GLogState.Enable, GColors.MediumSpringGreen);
            //GLog.Register(GLogName.Player, GLogState.Enable, GColors.AmaranthPurple);
            //GLog.Register(GLogName.GameManager, GLogState.Enable, GColors.AmaranthPurple);
            //GLog.Register(GLogName.IAPManager, GLogState.Enable, GColors.White);

            //GLog.Register(GLogName.BlockManager, GLogState.Enable, GColors.MagentaProcess);
            //GLog.Register(GLogName.CloudDBManager, GLogState.Enable, GColors.AeroBlue);
            //GLog.Register(GLogName.ScoreManager, GLogState.Enable, GColors.OldGold);
            //GLog.Register(GLogName.ScoreTablePopup, GLogState.Enable, GColors.MediumSpringGreen);
            //GLog.Register(GLogName.SessionModule, GLogState.Enable, GColors.AmaranthPurple);
            //GLog.Register(GLogName.NearbyServer, GLogState.Enable, GColors.AmaranthPurple);

            GLog.Register(GLogName.DriveKitManager, GLogState.Enable, GColors.AmaranthPurple);

        }

        #endregion

    }

}


