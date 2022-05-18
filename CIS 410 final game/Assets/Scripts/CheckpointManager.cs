using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private int car1points;
    private int car2points;

    public int FinishLineCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        car1points = 0;
        car2points = 0;
    }

    public void UpdateCheckpoints(int carnum, int checkpointnum)
    {
        if ((carnum == 1) && (checkpointnum == car1points + 1)) {
            car1points = checkpointnum;
        } else if ((carnum == 2) && (checkpointnum == car2points + 1)) {
            car2points = checkpointnum;
        }
        if (car1points == FinishLineCheckpoint) {
            print("Car 1 wins!");
        }
        if (car2points == FinishLineCheckpoint) {
            print("Car 2 wins!");
        }
    }
}
