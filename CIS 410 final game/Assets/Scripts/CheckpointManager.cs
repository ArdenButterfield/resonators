using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Checkpoint Manager
// By: Arden Butterfield, Donny Ebel
// Arden wrote the first version
// Last updated: 6/2/22 Donny

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
    private bool raceStarted = false;

    // UI Objects
    private float countdownTimer = 3.0f;
    public float startTime;
    public GameObject WinPanel;
    public GameObject CountdownPanel;
    public TextMeshProUGUI CountdownText;
    public TextMeshProUGUI WinPanelText;
    public TextMeshProUGUI p1LapCounter;
    public TextMeshProUGUI p2LapCounter;

    // Keeps a list of all checkpoints and updates respawnPoint with the most recently cleared valid checkpoint
    [System.NonSerialized] public Transform p1RespawnPoint;
    [System.NonSerialized] public Transform p2RespawnPoint;
    public List<Transform> respawnPoints = new List<Transform>();

    public SoundManager soundmanager;


    void Start()
    {
        laps = new List<int>() {-1, 0, 0};                      // 1-indexing; laps[1] is p1, laps[2] is p2
        lastClearedCheckpoint = new List<int>() { -1, 0, 0 };   // same here; this is for code readability!
        p1LapCounter.text = new string("1/" + required_laps);
        p2LapCounter.text = new string("1/" + required_laps);   // update both players' lap counters to 1 at start
        p1RespawnPoint = respawnPoints[0];
        p2RespawnPoint = respawnPoints[0];                      // first checkpoint is the finish line!
        WinPanel.SetActive(false);
        startTime = Time.time;
        soundmanager.playTick();
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
            //Debug.Log("P" + carnum + " cleared checkpoint " + checkpointnum);

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

    private void Update()
    {
        // If race hasn't started, update countdown text.
        if (!raceStarted)
        {
            countdownTimer = startTime + 3f - Time.time;

            if (countdownTimer <= 2.0f && CountdownText.text == "3")
            {
                CountdownText.text = "2";
                soundmanager.playTick();
            }
            if (countdownTimer <= 1.0f && CountdownText.text == "2")
            {
                CountdownText.text = "1";
                soundmanager.playTick();
            }
            // When timer expires, reset it, then start race!
            if (countdownTimer <= 0)
            {
                CountdownText.text = "GO!!";
                countdownTimer = 0.0f;
                StartRace();
            }
        }
        // If we have started, update timer and disable text if needed.
        else if (raceStarted)
        {
            if (countdownTimer < 1.0f)
            {
                countdownTimer += Time.deltaTime;
            }
            else
            {
                CountdownText.enabled = false;
                CountdownPanel.SetActive(false);
            }
        }
    }

    private void StartRace()
    {
        raceStarted = true;
        p1RaceTimer.StartTimer();

        // Only start P2 timer if they're racing!
        // DON'T change this; P1 race needs this to end properly.
        if (PlayerPrefs.GetInt("singleplayer") == 0)
            p2RaceTimer.StartTimer();

        soundmanager.startTheMusic();
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

    public bool getRaceStarted()
    {
        return raceStarted;
    }
}
