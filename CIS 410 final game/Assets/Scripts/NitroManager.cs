// Script by Arden Butterfield, last modified Apr. 26

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for Slider
 
using TMPro;



public class NitroManager : MonoBehaviour
{
    public Slider NitroMeter;
    private float nitroLevel;
    private const float maxNitro = 10f;
    public float nitroBurnStep = 0.01f;
    public float startingNitroAmount = 0f;
    public float nitroCoinAmount = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        NitroMeter.maxValue = maxNitro;
        nitroLevel = startingNitroAmount;
        UpdateSlider();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Nitro")) 
        {
            print("colliding");
            CoinBehavior coinScript = other.gameObject.GetComponent<CoinBehavior>();
            print(coinScript);
            coinScript.PickupCoin();
            AddNitro(nitroCoinAmount);
        }
    }

    // Called by the car when the car picks up coins. I guess this is
    // the observer pattern or something ;-)
    void AddNitro(float amount)
    {
        nitroLevel += amount;
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
