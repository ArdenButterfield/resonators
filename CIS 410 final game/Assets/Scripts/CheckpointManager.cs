using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public int required_laps = 3;
    public int num_checkpoints_on_track = 5;

    // Car lap information. A lap only counts if all checkpoints exist in that car's list
    private List<int> laps;
    private List<List<int>> clearedCheckpoints;

    public GameObject WinPanel;
    public TextMeshProUGUI WinPanelText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("In CheckpointManager Start()");
        laps[1] = 1;
        laps[2] = 1;
    }

    public void UpdateCheckpoints(int carnum, int checkpointnum)
    {
        // Add checkpoints to the car's as race continues.
        clearedCheckpoints[carnum].Add(checkpointnum);

        // If the car crosses the finish line...
        if (checkpointnum == 0)
        {
            // ...end race if someone won!
            if (laps[carnum] == required_laps)
                EndRace(carnum);

            // Otherwise, update lap if all checkpoints were triggered.
            else if (LapWasCompleted(carnum))
                    laps[carnum]++; Debug.Log("Car " + carnum + "on lap " + laps[carnum]);
        }
    }

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

    // Ends the race; declare the winner
    private void EndRace(int winner)
    {
        Debug.Log("In EndRace()");
        if (winner == 1)
            WinPanelText.text = ("Player 1 wins!");
        else if (winner == 2)
            WinPanelText.text = ("Player 2 wins!");

        WinPanel.SetActive(true);
    }

    public void LoadTitle()
    {
        Debug.Log("In LoadTitle()");
        // Draws on this tutorial: https://www.youtube.com/watch?v=05OfmBIf5os
        SceneManager.LoadScene("title screen");
    }
}
