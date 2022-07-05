
using UnityEngine;

namespace GameDev.Library
{
    public static class GLog
    {
        private static GLogger _logger = new GLogger();


        #region (Log)

        public static void Log(string log, GLogName name)
        {
            _logger.Log(log, name.ToString());
        }

        #endregion

        #region (Warning)

        public static void Warning(string log, GLogName name)
        {
            _logger.Warning(log, name.ToString());
        }

        #endregion

        #region (Error)

        public static void Error(string log, GLogName name)
        {
            _logger.Error(log, name.ToString());
        }

        #endregion


        #region (LogConfig) Register

        public static void Register(GLogName name, GLogState state, Color color)
        {
            Register(new GLogConfig()
            {
                Name = name.ToString(),
                State = state,
                Color = color
            });
        }

        private static void Register(GLogConfig logConfig)
        {
            _logger.RegisterLog(logConfig);
        }

        #endregion

        #region (LogConfig) UnRegister

        public static void UnRegister(GLogName name)
        {
            _logger.UnRegisterLog(name.ToString());
        }

        #endregion


        #region (GetLogs)

        public static string GetLogs()
        {
            return _logger.GetLogs();
        }

        #endregion
    }
}