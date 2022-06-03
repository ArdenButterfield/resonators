using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlockage : MonoBehaviour
{
    public CameraFollow cameraFollow;
    int numCollisions;

    void Start()
    {
        numCollisions = 0;
    }

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("wall"))
        {
            //print("colliding.");
            cameraFollow.whichCameraPos(true);
            numCollisions += 1;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("wall"))
        {
            //print("colliding.");
            numCollisions -= 1;
            if (numCollisions == 0)
            {
                cameraFollow.whichCameraPos(false);
            }
            
        }
    }
}
