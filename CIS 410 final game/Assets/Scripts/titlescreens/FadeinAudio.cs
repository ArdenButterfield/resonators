using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeinAudio : MonoBehaviour
{
    public float secondsTilIn;
    public float finalValue;
    public float startTime;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsed = Time.time - startTime;
        if (elapsed <= secondsTilIn) {
            source.volume = (elapsed / secondsTilIn) * finalValue;
        } else {
            source.volume = finalValue;
        }
    }
}
