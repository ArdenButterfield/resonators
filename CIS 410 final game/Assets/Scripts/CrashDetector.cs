using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We want a sound effect to play every time a car crashes. But what is a crash? This script uses acceleration
// data to determine when a car crashes.

// Script inspired by snippets here: https://answers.unity.com/questions/1489115/how-do-i-get-the-acceleration-of-an-object-accurat.html
// The velocity and acceleration calculations are all quite rough, since they're dependent on how often
// fixed update is called. That said, we don't need very precise information for detecting a crash, so 
// it didn't seem worth doing the extra divisions to get the real accelerations and velocities.


public class CrashDetector : MonoBehaviour
{
    Vector3 position;
    Vector3 prevPosition;
    Vector3 velocity;
    Vector3 prevVelocity;
    Vector3 acceleration;

    public SoundManager manager;

    public int carNumber;

    public float crashThreshold = 0.25f;
    float lastCrash;

    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position;
        prevPosition = position;
        velocity = Vector3.zero;
        prevVelocity = Vector3.zero;
        acceleration = Vector3.zero;
        lastCrash = Time.time;
        
    }

    bool CrashConditionsMet(Vector3 v, Vector3 a) {
        // A crash has a high acceleration. However, boosting has a high acceleration as well, and is not
        // a crash. Since boosting propells the car forward in the direction it's already going, we can
        // check that the crash is accelerating the car in a different direction than it's velocity using
        // a dot product. We calculate the squared magnitude instead of the magnitude, to limit unecessary
        // and costly square root applications.
        return (Vector3.Dot(v, a) < 0.9) && (a.sqrMagnitude > crashThreshold) && (Time.time > lastCrash + 0.25f);
    }

    void FixedUpdate()
    {
        position = gameObject.transform.position;
        
        velocity = position - prevPosition;
        acceleration = velocity - prevVelocity;

        if (CrashConditionsMet(velocity, acceleration)) {
            lastCrash = Time.time;
            float volume = acceleration.sqrMagnitude;
            //print(volume);
            if (volume > 1f) {
                volume = 1f;
            }
            //manager.playCrash(carNumber, volume);
        }

        prevPosition = position;
        prevVelocity = velocity;
    }
}
