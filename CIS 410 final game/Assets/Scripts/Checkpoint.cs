using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber;
    public CheckpointManager Manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player1")) {
            //Debug.LogFormat("P1 passed {0}", other);
            Manager.UpdateCheckpoints(1, checkpointNumber);
            
        } else if (other.gameObject.CompareTag("player2")) {
            //Debug.LogFormat("P2 passed {0}", checkpointNumber);
            Manager.UpdateCheckpoints(2, checkpointNumber);
        }
    }
}
