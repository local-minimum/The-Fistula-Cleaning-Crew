using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDTimeAndDepth : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeTextUI;
    [SerializeField] TextMeshProUGUI depthTextUI;
    Canvas canvas;

    float timerStart;
    float timerDuration;
    bool timerRunning = false;
    private static HUDTimeAndDepth _instance { get; set; }

    public static float Depth
    {
        set
        {
            _instance.SetDepth(value);
        }
    }

    public static void StartTimer()
    {
        _instance.InitTimer();
    }

    public static void StopTimer()
    {
        _instance.timerRunning = false;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            canvas = GetComponentInChildren<Canvas>();
            canvas.enabled = false;
        } else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (timerRunning)
        {
            timerDuration = Time.timeSinceLevelLoad - timerStart;
            timeTextUI.text = string.Format("{0:00.00}", timerDuration);
        }
    }

    private void OnDestroy()
    {
        if (_instance = this)
        {
            _instance = null;
        }
    }

    void SetDepth(float depth)
    {
        depth = Mathf.Clamp01(depth);
        depthTextUI.text = string.Format("{0:00%}", depth);
        if (!canvas.enabled) canvas.enabled = true;
    }

    void InitTimer()
    {
        timerStart = Time.timeSinceLevelLoad;
        timerRunning = true;
        if (!canvas.enabled) canvas.enabled = true;
    }
}
