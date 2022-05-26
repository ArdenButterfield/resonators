using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System;
using TMPro;

// A Simple Race Timer :3
// By: Donny Ebel (mechanics manager)
// Scoured mostly Unity manuals
// Last updated; 5/25/22 Donny

public class RaceTimer : MonoBehaviour
{
    private Stopwatch stopWatch = new Stopwatch();
    private string elapsedTime = String.Format("{0:00}:{1:00}.{2:000}", 0, 0, 0);
    private TimeSpan ts = new TimeSpan();
    public bool timerActive = false;
    public TextMeshProUGUI currentTimeText;

    void Update()
    {
        ts = stopWatch.Elapsed;
        elapsedTime = String.Format("{0:00}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds);
        currentTimeText.text = elapsedTime;
    }

    public void StartTimer()
    {
        if (!timerActive)
        {
            stopWatch.Start();
            timerActive = true;
            currentTimeText.color = new Color(0, 255, 0, 255);
        }
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