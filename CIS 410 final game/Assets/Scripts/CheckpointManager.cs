using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public int required_laps = 2;
    public int num_checkpoints_on_track = 5;

    // Car lap information. A lap only counts if all checkpoints exist in that car's list
    private List<int> laps;
    private List<int> lastClearedCheckpoint;

    public GameObject WinPanel;
    public TextMeshProUGUI WinPanelText;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("In CheckpointManager Start()");
        laps = new List<int>() {0,0};
        lastClearedCheckpoint = new List<int>() {0,0};
        WinPanel.SetActive(false);
    }

    public void UpdateCheckpoints(int carnum, int checkpointnum)
    {
        int nextExpectedCheckpoint = lastClearedCheckpoint[carnum - 1] + 1;
        nextExpectedCheckpoint %= num_checkpoints_on_track;

        if (checkpointnum == nextExpectedCheckpoint) {
            //print("Checkpoint cleared");
            lastClearedCheckpoint[carnum - 1] = checkpointnum;
            if (checkpointnum == 0) {
                //print("lap completed");
                laps[carnum-1] += 1;
                if (laps[carnum-1] == required_laps) {
                    EndRace(carnum);
                }
            }
        }
        /*
        // If the car crosses the finish line...
        if (checkpointnum == 0)
        {
            // ...end race if someone won!
            if (laps[carnum-1] == required_laps)
            {
                EndRace(carnum);
            }

            // Otherwise, update lap if all checkpoints were triggered.
            else if (LapWasCompleted(carnum))
            {
                laps[carnum-1]++; 
                Debug.Log("Car " + carnum + "on lap " + laps[carnum-1]);
            }
        }
        */
    }
    /*
    private bool LapWasCompleted(int carnum)
    {
        Debug.Log("In LapWasCompleted()");
        // Assume failure
        bool tickLap = false;
        int checkpointCount = 0;

        // Increment counter if checkpoint was in the list
        for (int i = 1; i <= num_checkpoints_on_track; i++)
            if (clearedCheckpoints[carnum].Contains(i))
                checkpointCount++;

        // If all checkpoints cleared, update return bool
        if (checkpointCount == num_checkpoints_on_track)
            tickLap = true;

        return tickLap;
    }
    */
    // Ends the race; declare the winner
    private void EndRace(int winner)
    {
        //Debug.Log("In EndRace()");
        if (winner == 1)
            WinPanelText.text = ("Player 1 wins!");
        else if (winner == 2)
            WinPanelText.text = ("Player 2 wins!");

        WinPanel.SetActive(true);
    }

    public void LoadTitle()
    {
        //Debug.Log("In LoadTitle()");
        // Draws on this tutorial: https://www.youtube.com/watch?v=05OfmBIf5os
        SceneManager.LoadScene("title screen");
    }
}
