
using GameDev.Library;

using UnityEngine;

namespace GameDev.MiddleWare
{
    public abstract class GLevel : GSingleton<GLevel>
    {

        //#region Initialize

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //private static void Initialize()
        //{
        //    GLog.Log($"Initialize", GLogName.GameManager);

        //    var gameObject = new GameObject();
        //    gameObject.AddComponent<LevelManager>();
        //    gameObject.name = "LevelManager";
        //    DontDestroyOnLoad(gameObject);
        //}

        //#endregion


        //private GameObject _currentLevel;


        //#region Unity: OnEnable

        //private void OnEnable()
        //{
        //    GLog.Log($"OnEnable", GLogName.LevelManager);

        //    this.AddListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
        //}

        //#endregion

        //#region Unity: OnDisable

        //private void OnDisable()
        //{
        //    GLog.Log($"OnDisable", GLogName.LevelManager);

        //    this.RemoveListener<object>(GEventName.SESSION_PREPARE, OnSessionPrepare);
        //}

        //#endregion


        //#region Level: Create



        //public void CreateLevel()
        //{
        //    if (_currentLevel != null)
        //    {
        //        Destroy(_currentLevel);
        //    }

        //    var levelIndex = GetLevelType;

        //    if (DataStore.IsLevelOrderAdjustable)
        //    {
        //        var levelPrefab = GameChef.instance.LevelOrderSO.levels[levelIndex];
        //        var currentLevel = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
        //        _currentLevel = currentLevel;
        //    }
        //    else
        //    {
        //        string levelNo = (levelIndex + 1).ToString();
        //        var path = "Levels/" + "Level_" + levelNo;
        //        var levelPrefab = Resources.Load(path) as GameObject;
        //        var currentLevel = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
        //        _currentLevel = currentLevel;
        //    }
        //}

        //private int GetLevelType
        //{
        //    get
        //    {
        //        int number = DataStore.LevelNo % DataStore.LevelCount;
        //        if (number == 0)
        //        {
        //            number = DataStore.LevelCount;
        //        }
        //        int value = number - 1;
        //        Debug.Assert(value >= 0, "Index cannot be smaller than 0");
        //        return value;
        //    }
        //}

        //#endregion


        //#region Event: OnSessionPrepare

        //protected virtual void OnSessionPrepare(object sender, GEvent<object> eventData)
        //{
        //    GLog.Log($"OnSessionPrepare", GLogName.LevelManager);

        //    CreateLevel();
        //}

        //#endregion

        //#region Event: OnSessionSuccess

        //protected virtual void OnSessionSuccess(object sender, GEvent<object> eventData)
        //{
        //    GLog.Log($"OnSessionPrepare", GLogName.LevelManager);

        //    DataStore.LevelNo++;

        //}

        //#endregion

    }
}
