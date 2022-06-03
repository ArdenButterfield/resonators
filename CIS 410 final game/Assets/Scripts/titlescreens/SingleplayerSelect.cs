using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class SingleplayerSelect : MonoBehaviour
{
    public Toggle singleplayerToggle;
    public TextMeshProUGUI controlInstructions;

    public string singlePlayerInstructions = "move with WASD, boost is v (or right shift), drift is spacebar, HOLD r to respawn to checkpoint";
    public string twoPlayerInstructions = "Player 1: move with WASD, boost is v (or right shift), drift is spacebar, HOLD r to respawn to checkpoint\nPlayer 2: move with PL;', boost is Return, drift is right shift, HOLD \\ to respawn to checkpoint";

    void Start() {
        //print(PlayerPrefs.GetInt("singleplayer"));
        if (PlayerPrefs.GetInt("singleplayer") == 1) {
            singleplayerToggle.isOn = true;
            controlInstructions.text = singlePlayerInstructions;
        } else {
            singleplayerToggle.isOn = false;
            controlInstructions.text = twoPlayerInstructions;
        }
    }

    public void setSinglePlayer() {
        // True for singleplayer, false for 2 player.

        // We can't set bools in playerprefs, so we need to convert to ints.
        if (singleplayerToggle.isOn) {
            //print("On");
            PlayerPrefs.SetInt("singleplayer", 1);
            controlInstructions.text = singlePlayerInstructions;
        } else {
            //print("Off");
            PlayerPrefs.SetInt("singleplayer", 0);
            controlInstructions.text = twoPlayerInstructions;
        }
    }
}
