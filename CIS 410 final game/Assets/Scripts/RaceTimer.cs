using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class RaceTimer : MonoBehaviour
{
    public Stopwatch p1StopWatch = new Stopwatch();
    public Stopwatch p2StopWatch = new Stopwatch();
    private float p1Time;
    private float p2Time;
    private bool p1TimerActive = false;
    private bool p2TimerActive = false;
    public Text p1CurrentTimeText;
    public Text p2CurrentTimeText;

    void Start()
    {
        StartTimers();
    }

    void Update()
    {
        p1CurrentTimeText.text = p1StopWatch.ToString();
        p2CurrentTimeText.text = p2StopWatch.ToString();
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