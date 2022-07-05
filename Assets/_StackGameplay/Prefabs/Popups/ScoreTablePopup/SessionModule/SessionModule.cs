using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HmsPlugin;
using GameDev.Library;

public class SessionModule : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI Txt_SessionNumber;

    [SerializeField]
    private TextMeshProUGUI Txt_Score;

    public void SetSessionModule(GameSessions gameSessions)
    {
        GLog.Log($"SetSessionModule", GLogName.SessionModule);

        var sessionNumber = gameSessions.SessionNumber;
        var score = gameSessions.Score;

        Txt_SessionNumber.text = sessionNumber.ToString();
        Txt_Score.text = score.ToString();
    }

}
