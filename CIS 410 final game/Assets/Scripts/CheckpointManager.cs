using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Checkpoint Manager
// By: Arden Butterfield, Donny Ebel
// Arden wrote the first version
// Last updated: 5/25/22 Donny - rewrote most of UpdateCheckpoints()

public class CheckpointManager : MonoBehaviour
{
    public int required_laps;
    public int num_checkpoints_on_track;

    // Car lap information.
    int nextExpectedCheckpoint = 0;
    private List<int> laps;
    private List<int> lastClearedCheckpoint;
    public RaceTimer p1RaceTimer;
    public RaceTimer p2RaceTimer;
    private int winner;     // 0 if no one won yet; set to carnum who won

    // Keeps a list of all checkpoints and updates respawnPoint with the most recently cleared valid checkpoint
    public Transform p1RespawnPoint;
    public Transform p2RespawnPoint;
    public List<Transform> respawnPoints = new List<Transform>();

    // UI Objects
    public GameObject WinPanel;
    public TextMeshProUGUI WinPanelText;
    public TextMeshProUGUI p1LapCounter;
    public TextMeshProUGUI p2LapCounter;

    void Start()
    {
        laps = new List<int>() {-1, 0, 0};                      // 1-indexing; laps[1] is p1, laps[2] is p2
        lastClearedCheckpoint = new List<int>() { -1, 0, 0 };   // same here; this is for code readability!
        p1LapCounter.text = new string("1/" + required_laps);
        p2LapCounter.text = new string("1/" + required_laps);   // update both players' lap counters to 1 at start
        p1RespawnPoint = respawnPoints[0];
        p2RespawnPoint = respawnPoints[0];                      // first checkpoint is the finish line!
        WinPanel.SetActive(false);

        // Start the timers. Eventually move these when implement countdown.
        p1RaceTimer.StartTimer();
        p2RaceTimer.StartTimer();
    }

    public void UpdateCheckpoints(int carnum, int checkpointnum)    // carnum is always 1 or 2
    {
        // Update expected checkpoints
        nextExpectedCheckpoint = lastClearedCheckpoint[carnum] + 1;
        nextExpectedCheckpoint %= num_checkpoints_on_track;

        // If we're at the next expected checkpoint (or at the finish line on the first lap)...
        // The latter logic is required so that the laps will update appropriately at the beginning of the race.
        if ((checkpointnum == nextExpectedCheckpoint) || (checkpointnum == 0 && laps[carnum] == 0))
        {
            // Update the last cleared checkpoint.
            lastClearedCheckpoint[carnum] = checkpointnum;

            // Update respawn points for appropriate player
            if (carnum == 1)
                p1RespawnPoint = respawnPoints[checkpointnum];
            else if (carnum == 2)
                p2RespawnPoint = respawnPoints[checkpointnum];

            // Handle crossing the finish line
            if (checkpointnum == 0)
            {
                // If race shouldn't be over, update lap counters and UI
                if (laps[carnum] < required_laps)
                {
                    laps[carnum]++;
                    if (carnum == 1)
                        p1LapCounter.text = new string(laps[carnum] + "/" + required_laps);
                    else if (carnum == 2)
                        p2LapCounter.text = new string(laps[carnum] + "/" + required_laps);
                }
                // Otherwise, at least one player is done. Stop the timer and update winner if needed.
                else
                {
                    if (carnum == 1)
                        p1RaceTimer.StopTimer();
                    else if (carnum == 2)
                        p2RaceTimer.StopTimer();

                    // Winner is initialized to 0, this only updates the winner once
                    if (winner == 0)
                    {
                        winner = carnum;
                    }
                }

                // If both timers stopped and a winner was chosen, end the race!
                if (!p1RaceTimer.timerActive && !p2RaceTimer.timerActive && winner != 0)
                    EndRace(winner);
            }
        }
    }

    // End the race and declare the winner
    private void EndRace(int whoWon)
    {
        if (whoWon == 1)
            WinPanelText.text = ("Player 1 Wins!");
        else if (whoWon == 2)
            WinPanelText.text = ("Player 2 Wins!");
        else
            WinPanelText.text = ("This should not happen!");

        WinPanel.SetActive(true);
    }

    public void LoadTitle()
    {
        // Draws on this tutorial: https://www.youtube.com/watch?v=05OfmBIf5os
        SceneManager.LoadScene("title screen");
    }
}
