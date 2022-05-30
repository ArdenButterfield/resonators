using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class SingleplayerSelect : MonoBehaviour
{
    public Toggle singleplayerToggle;

    public void setSinglePlayer() {
        // True for singleplayer, false for 2 player.

        // We can't set bools in playerprefs, so we need to convert to ints.
        if (singleplayerToggle.isOn) {
            print("On");
            PlayerPrefs.SetInt("singleplayer", 1);
        } else {
            print("Off");
            PlayerPrefs.SetInt("singleplayer", 0);
        }
    }
}
