
using GameDev.Library;
using GameDev.MiddleWare;

using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace StackGamePlay
{
    public class MechanicManager : MonoBehaviour
    {

        #region Child: GameData

        [SerializeField]
        private GameSettingData gameData;

        public GameSettingData GameData { get => gameData; set => gameData = value; }

        #endregion

        #region Variables

        private bool isGameOver = false;
        public bool IsGameOver { get => isGameOver; set => isGameOver = value; }

        private bool isGameStarted = false;
        public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }


        #endregion

        #region Singleton

        private static MechanicManager instance;
        public static MechanicManager Instance { get => instance; set => instance = value; }


        private void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion

        #region Monobehaviour
        private void Awake()
        {
            Singleton();
        }
        private void OnEnable()
        {
            GLog.Log($"OnEnable", GLogName.MechanicManager);
            this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.AddListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.AddListener<object>(GEventName.SESSION_END, OnSessionEnd);
        }
        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.MechanicManager);
            this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
            this.RemoveListener<object>(GEventName.SESSION_STARTED, OnSessionStart);
            this.RemoveListener<object>(GEventName.SESSION_END, OnSessionEnd);
        }
        private void Start()
        {
            StartCoroutine(FSM());
        }
        #endregion
        IEnumerator FSM()
        {
            yield return OnWaitForStart();
            yield return OnUpdate();
            this.DispatchEvent(new GEvent<object>(GEventName.SESSION_END, this));
        }
        IEnumerator OnWaitForStart()
        {
            while (!IsGameStarted) yield return null;
            yield return null;
        }

        IEnumerator OnUpdate()
        {
            while (!IsGameOver)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DelegateStore.Touch?.Invoke();
                }
                yield return null;
            }
        }

        public void Replay()
        {
            SceneManager.LoadScene("GameScene");
        }


        #region Events: OnSessionPrepare
        private void OnSessionPrepare(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionPrepare", GLogName.GameChef);
            IsGameStarted = false;
        }
        #endregion
        #region Events: OnSessionStart
        private void OnSessionStart(object sender, GEvent<object> eventData)
        {
            GLog.Log($"OnSessionStart", GLogName.GameChef);
            IsGameStarted = true;

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