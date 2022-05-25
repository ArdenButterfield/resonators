// Script by Arden Butterfield, last modified Apr. 26
// Modified 5/9/22 by Donny Ebel
// Modified 5/9/22 by Donny Ebel

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for Slider
 
using TMPro;



public class NitroManager : MonoBehaviour
{
    public Slider NitroMeter;
    private float nitroLevel;
    private const float maxNitro = 12f;
    private float nitroBurnStep = 3f;
    private float startingNitroAmount = 0f;
    private float nitroCoinAmount = 2.0f;


    public SoundManager soundManager;
    public int carNumber;

    // Start is called before the first frame update
    void Start()
    {
        NitroMeter.maxValue = maxNitro;
        nitroLevel = startingNitroAmount;
        UpdateSlider();
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Nitro")) 
        {
            //print("colliding");
            CoinBehavior coinScript = other.gameObject.GetComponent<CoinBehavior>();
            if (coinScript.PickupCoin()) {
                AddNitro(nitroCoinAmount);
                //soundManager.playPickup(carNumber);
            }
        }
    }

    // Called by the car when the car picks up coins.
    void AddNitro(float amount)
    {
        nitroLevel += amount;
        if (nitroLevel > maxNitro)
        {
            nitroLevel = maxNitro;
        }
        UpdateSlider();
    }

    public bool EnoughNitro()
    {
        bool enoughFuel = false;
        if (nitroLevel >= nitroBurnStep)
        {
            enoughFuel = true;
            BurnNitro();
        }

        return enoughFuel;
    }

    void BurnNitro()
    {
        nitroLevel -= nitroBurnStep;

        UpdateSlider();
        //soundManager.playBoost(carNumber);

    }

    void UpdateSlider()
    {
        NitroMeter.value = nitroLevel;
    }
}
