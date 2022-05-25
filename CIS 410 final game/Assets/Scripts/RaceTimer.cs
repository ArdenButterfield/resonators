using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System;
using TMPro;

public class RaceTimer : MonoBehaviour
{
    public Stopwatch p1StopWatch = new Stopwatch();
    public Stopwatch p2StopWatch = new Stopwatch();
    private string p1ElapsedTime = String.Format("{0:00}:{1:00}.{2:00}", 0, 0, 0);
    private string p2ElapsedTime = String.Format("{0:00}:{1:00}.{2:00}", 0, 0, 0);
    private bool p1TimerActive = false;
    private bool p2TimerActive = false;
    private TimeSpan ts = new TimeSpan();
    public TextMeshProUGUI p1CurrentTimeText;
    public TextMeshProUGUI p2CurrentTimeText;

    void Start()
    {
        StartTimers();
    }

    void Update()
    {
        ts = p1StopWatch.Elapsed;
        p1ElapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        p2ElapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        p1CurrentTimeText.text = p1ElapsedTime;
        p2CurrentTimeText.text = p2ElapsedTime;
    }

    public void StartTimers()
    {
        p1StopWatch.Start();
        p2StopWatch.Start();
        p1TimerActive = true;
        p2TimerActive = true;
    }

    public void StopTimer(int playernum)
    {
        if (playernum == 1)
        {
            p1StopWatch.Stop();
            p1TimerActive = false;
        }
        else if (playernum ==2)
        {
            p2StopWatch.Stop();
            p2TimerActive = false;
        }
    }

}