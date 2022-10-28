using GameDev.Library;

using HmsPlugin;

using StackGamePlay;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ScoreTablePopup : AbstractPopup
{
    #region Childs

    [SerializeField]
    private GameObject prefab_sessionModule;

    [SerializeField]
    private Transform parent_sessionModule;

    #endregion

    #region Variables


    #endregion

    #region Monobehaviour

    private void OnEnable()
    {
        GLog.Log($"OnEnable", GLogName.ScoreTablePopup);

        CleanSessionModules();
        CreateSessionModules();

    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        GLog.Log($"Start", GLogName.ScoreTablePopup);


    }

    #endregion

    #region CreateSessionModules

    private void CreateSessionModules()
    {
        CloudDBManager.Instance.QuerySessions();
        StartCoroutine(QuerySessionsIEnumerator());
    }

    IEnumerator QuerySessionsIEnumerator()
    {
        GLog.Log($"QuerySessionsIEnumerator", GLogName.ScoreTablePopup);

        GLog.Log($"phase1", GLogName.ScoreTablePopup);

        yield return new WaitUntil(() => CloudDBManager.Instance.QueryIsOK);

        GLog.Log($"phase2", GLogName.ScoreTablePopup);

        foreach (var gameSession in CloudDBManager.Instance.GameSessionsList)
        {
            var gameObject = Instantiate(prefab_sessionModule, parent_sessionModule);
            var sesssionModule = gameObject.GetComponent<SessionModule>();
            sesssionModule.SetSessionModule(gameSession);
        }

        GLog.Log($"phase3", GLogName.ScoreTablePopup);

    }

    #endregion

    #region CleanSessionModules
    private void CleanSessionModules()
    {
        var sessionModules = parent_sessionModule.GetComponentsInChildren<SessionModule>();

        List<GameObject> listOfGameobjects = new List<GameObject>();

        for (int i = 0; i < sessionModules.Length; i++)
        {
            listOfGameobjects.Add(sessionModules[i].gameObject);
        }

        foreach (var item in listOfGameobjects)
        {
            Destroy(item);
        }
    }
    #endregion

    #region Implementation

    public override void Activate(Action action)
    {
    }

    public override void Deactivate(Action action)
    {
    }

    #endregion

}
