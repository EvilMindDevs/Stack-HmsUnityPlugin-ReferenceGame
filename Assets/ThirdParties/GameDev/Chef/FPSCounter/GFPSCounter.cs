
using StackGamePlay;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GFPSCounter : MonoBehaviour
{

    const float fpsMeasurePeriod = 0.5f;
    private int m_FpsAccumulator = 0;
    private float m_FpsNextPeriod = 0;
    private int m_CurrentFps;
    const string display = "{0}";
    private GUIStyle fontSize;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
    }
    private void Update()
    {
        // measure average frames per second
        m_FpsAccumulator++;
        if (Time.realtimeSinceStartup > m_FpsNextPeriod)
        {
            m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
            m_FpsAccumulator = 0;
            m_FpsNextPeriod += fpsMeasurePeriod;

            //m_Text.text = string.Format(display, m_CurrentFps);

        }
    }

    private void OnGUI()
    {
        fontSize = new GUIStyle();

        fontSize.fontSize = 55;

        //GUI.Box(new Rect(10, 10, 100, 100), "FPS", fontSize);

        GUI.Label(new Rect(10, 20, 1000, 190), string.Format(" {0} ", string.Format(display, Warehouse.RemoveAds).ToString()), fontSize);
        //GUI.Label(new Rect(10, 20, 1000, 190), string.Format(" {0} ", string.Format(display, m_CurrentFps).ToString()), fontSize);
        return;
    }
}