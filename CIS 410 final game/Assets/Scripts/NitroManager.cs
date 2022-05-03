// Script by Arden Butterfield, last modified Apr. 26

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for Slider


public class NitroManager : MonoBehaviour
{
    public Slider NitroMeter;
    private float nitroLevel;
    private const float maxNitro = 10f;
    public float nitroAddStep = 0.5f;
    public float nitroBurnStep = 0.01f;
    public float startingNitroAmount = 10f;

    // Start is called before the first frame update
    void Start()
    {
        NitroMeter.maxValue = maxNitro;
        nitroLevel = startingNitroAmount;
        UpdateSlider();
    }

    // Called by the car when the car picks up coins. I guess this is
    // the observer pattern or something ;-)
    void AddNitro()
    {
        nitroLevel += nitroAddStep;
        if (nitroLevel > maxNitro)
        {
            nitroLevel = maxNitro;
        }
        UpdateSlider();
    }

    public bool BurnNitro()
    {
        // Attempts to burn nitro, returns if successful.
        bool enoughFuel;
        if (nitroLevel >= nitroBurnStep)
        {
            nitroLevel -= nitroBurnStep;
            enoughFuel = true;
        } else {
            nitroLevel = 0f;
            enoughFuel = false;
        }
        UpdateSlider();
        return enoughFuel;
    }

    void UpdateSlider()
    {
        NitroMeter.value = nitroLevel;
    }
}
