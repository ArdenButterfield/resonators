using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public int required_laps;
    public int num_checkpoints_on_track;

    // Car lap information.
    private List<int> laps;
    private List<int> lastClearedCheckpoint;

    // Keeps a list of all checkpoints and updates respawnPoint with the most recently cleared valid checkpoint
    public Transform p1RespawnPoint;
    public Transform p2RespawnPoint;
    public List<Transform> respawnPoints = new List<Transform>();

    public GameObject WinPanel;
    public TextMeshProUGUI WinPanelText;

    // Start is called before the first frame update
    void Start()
    {
        laps = new List<int>() {1,1};
        lastClearedCheckpoint = new List<int>() {0,0};
        p1RespawnPoint = respawnPoints[0];
        p2RespawnPoint = respawnPoints[0];
        WinPanel.SetActive(false);

        // Race Timer

    }

    public void UpdateCheckpoints(int carnum, int checkpointnum)
    {
        int nextExpectedCheckpoint = lastClearedCheckpoint[carnum - 1] + 1;
        nextExpectedCheckpoint %= num_checkpoints_on_track;

        if (checkpointnum == nextExpectedCheckpoint)
        {
            lastClearedCheckpoint[carnum - 1] = checkpointnum;

            if (carnum == 1)
                p1RespawnPoint = respawnPoints[checkpointnum];
            else if (carnum == 2)
                p2RespawnPoint = respawnPoints[checkpointnum];

            if (checkpointnum == 0)
            {
                if (laps[carnum - 1] == required_laps)
                    EndRace(carnum);
                else
                    laps[carnum - 1] += 1;
            }
        }
    }

    // Ends the race; declare the winner
    private void EndRace(int winner)
    {
        if (winner == 1)
            WinPanelText.text = ("Player 1 wins!");
        else if (winner == 2)
            WinPanelText.text = ("Player 2 wins!");

        WinPanel.SetActive(true);
    }

    public void LoadTitle()
    {
        // Draws on this tutorial: https://www.youtube.com/watch?v=05OfmBIf5os
        SceneManager.LoadScene("title screen");
    }
}
