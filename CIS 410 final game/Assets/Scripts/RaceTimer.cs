using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System;
using TMPro;

public class RaceTimer : MonoBehaviour
{
    public Stopwatch stopWatch = new Stopwatch();
    private string elapsedTime = String.Format("{0:00}:{1:00}.{2:000}", 0, 0, 0);
    public bool timerActive = false;
    private TimeSpan ts = new TimeSpan();
    public TextMeshProUGUI currentTimeText;

    void Update()
    {
        ts = stopWatch.Elapsed;
        elapsedTime = String.Format("{0:00}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds);
        currentTimeText.text = elapsedTime;
    }

    public void StartTimer()
    {
        stopWatch.Start();
        timerActive = true;
        currentTimeText.color = new Color(0, 255, 0, 255);
    }

    public void StopTimer()
    {
        if (timerActive)
        {
            stopWatch.Stop();
            timerActive = false;
            currentTimeText.color = new Color(0, 200, 255, 255);
        }
    }
}