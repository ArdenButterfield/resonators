using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Make sure coin doesn't spawn if something's in its space!

public class CoinBehavior : MonoBehaviour
{
    public GameObject[] ChildrenToHide;
    float lastCollisionTime;
    public float recoveryTime = 1.5f;
    bool active;

    void Start()
    {
        gameObject.tag = "Nitro";
        lastCollisionTime = Time.time - recoveryTime;
    }

    public bool PickupCoin()
    {
        bool wasActiveBefore = active;
        if (active) {
            for (int i = 0; i < ChildrenToHide.Length; i++) {
                ChildrenToHide[i].SetActive(false);
            }
            lastCollisionTime = Time.time;
            active = false;
        }
        return wasActiveBefore;
    }

    void Update()
    {
        if ((!active) && (Time.time - lastCollisionTime >= recoveryTime)) {
            for (int i = 0; i < ChildrenToHide.Length; i++) {
                ChildrenToHide[i].SetActive(true);
            }
            active = true;
        }
    } 
}
