
using System;
using System.Collections.Generic;
using System.Linq;

using GameDev.Library;

using UnityEngine;

namespace GameDev.MiddleWare
{
    // generic class to manage poolable game objects
    // types and prefabs must be specified in the editor
    public class GStateDrivenCamera : GSingleton<GStateDrivenCamera>
    {
        public Animator CinemachineAnimator => GetComponent<Animator>();

        private const string START_SESSION = "StartSession";

        #region Unity: OnEnable

        private void OnEnable()
        {
            GLog.Log($"OnEnable", GLogName.GStateDrivenCamera);

            this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        }

        #endregion

        #region Unity: OnDisable

        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.GStateDrivenCamera);

            this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
        }

        #endregion


        #region Events: OnSessionStart

        private void OnSessionStart(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionStart", GLogName.GStateDrivenCamera);

            CinemachineAnimator.SetTrigger(START_SESSION);
        }

        #endregion

    }

}
