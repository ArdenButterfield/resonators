using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    private int car1points;
    private int car2points;
    private bool gameover;

    public int FinishLineCheckpoint;
    public GameObject WinPanel;
    public TextMeshProUGUI WinPanelText;


    // Start is called before the first frame update
    void Start()
    {
        car1points = 0;
        car2points = 0;
        WinPanel.SetActive(false);
        gameover = false;
    }

    public void UpdateCheckpoints(int carnum, int checkpointnum)
    {
        if (gameover) {
            return;
        }

        if ((carnum == 1) && (checkpointnum == car1points + 1)) {
            car1points = checkpointnum;
        } else if ((carnum == 2) && (checkpointnum == car2points + 1)) {
            car2points = checkpointnum;
        }
        if (car1points == FinishLineCheckpoint) {
            WinPanelText.text = ("Player 1 wins!");
            gameover = true;
            WinPanel.SetActive(true);
        }
        if (car2points == FinishLineCheckpoint) {
            WinPanelText.text = ("Player 2 wins!");
            gameover = true;
            WinPanel.SetActive(true);
        }
    }

    public void LoadTitle()
    {
        // Draws on this tutorial: https://www.youtube.com/watch?v=05OfmBIf5os
        SceneManager.LoadScene("title screen");
    }
}
