using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class RaceTimer : MonoBehaviour
{
    public Stopwatch stopWatch = new Stopwatch();


    void Start()
    {
        stopWatch.Start();
    }

    void Update()
    {
        
    }

}