using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameDev.Library
{
    public class GLogger
    {
        private readonly List<GLogConfig> _logData = new List<GLogConfig>();

        private const int MAX_LOG = 10000;
        private List<string> _logs = new List<string>(MAX_LOG);
        private int _counter = 0;

        #region (Log)

        public void Log(string log, string name)
        {
            if (!AllowLog(name))
            {
                return;
            }

            string result = GetLogText(log, name);
            Debug.Log(result);

            AddLog(log);
        }

        #endregion

        #region (Warning)

        public void Warning(string log, string name)
        {
            if (!AllowLog(name))
            {
                return;
            }

            string result = GetLogText(log, name);
            Debug.LogWarning(result);

            AddLog(log);
        }

        #endregion

        #region (Error)

        public void Error(string log, string name)
        {
            if (!AllowLog(name))
            {
                return;
            }

            string result = GetLogText(log, name);
            Debug.LogError(result);

            AddLog(log);
        }

        #endregion


        #region (Log) GetText

        private string GetLogText(string log, string name)
        {
            GLogConfig logConfig = GetLog(name);

            string colorName = ColorUtility.ToHtmlStringRGBA(logConfig.Color);
            //string colorLog = ColorUtility.ToHtmlStringRGBA(GColors.White);

            string result = $"<color=#{colorName}>{logConfig.Name}: </color>";
            result += log;
            //result +=$"<color=#{colorLog}>{log}</color>";

            return result;
        }

        #endregion

        #region (Log) Prefix

        private string GetPrefix()
        {
            return $"[{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToShortTimeString()}]: ";
        }

        #endregion

        #region (Log) Add

        private void AddLog(string log)
        {
            if (_counter == MAX_LOG)
            {
                _counter = 0;
            }

            string row = GetPrefix() + log;

            _logs.Insert(_counter, row);
            _counter++;
        }

        #endregion

        #region (Log) GetLogs

        public string GetLogs()
        {
            var result = String.Join("\n", _logs);
            return result;
        }

        #endregion


        #region (LogConfig) Get

        private GLogConfig GetLog(string name)
        {
            GLogConfig logConfig = _logData.Find(x => x.Name == name);
            return logConfig;
        }

        #endregion

        #region (LogConfig) Allow

        private bool AllowLog(string name)
        {
            GLogConfig logConfig = _logData.Find(x => x.Name == name);
            if (logConfig == null)
            {
                return false;
            }

            if (logConfig.State == GLogState.Disable)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region (LogConfig) Register

        public void RegisterLog(string name, GLogState state)
        {
            RegisterLog(new GLogConfig()
            {
                Name = name,
                State = state
            });
        }

        public void RegisterLog(GLogConfig logConfig)
        {
            _logData.Add(logConfig);
        }

        #endregion

        #region (LogConfig) UnRegister

        public void UnRegisterLog(string name)
        {
            int index = _logData.FindIndex(x => x.Name == name);
            if (index != -1)
            {
                _logData.RemoveAt(index);
            }
        }

        #endregion
    }
}