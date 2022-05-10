using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject[] ChildrenToHide;
    float lastCollisionTime;
    public float recoveryTime = 5.0f;
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
            print("setting inactive");
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
            print("setting active");
            for (int i = 0; i < ChildrenToHide.Length; i++) {
                ChildrenToHide[i].SetActive(true);
            }
            active = true;
        }
    } 
}
