// by Arden Butterfield, last modified May 4


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonHandler : MonoBehaviour
{
    public int Player1DefaultCarIndex = 0;
    public int Player2DefaultCarIndex = 0;

    void Start()
    {
        // Default indices of cars for the two players.
        PlayerPrefs.SetInt("Player 1 car", Player1DefaultCarIndex);
        PlayerPrefs.SetInt("Player 2 car", Player2DefaultCarIndex);
        PlayerPrefs.Save();
    }
    
    public void LoadScene(string SceneName)
    {
        // Draws on this tutorial: https://www.youtube.com/watch?v=05OfmBIf5os
        SceneManager.LoadScene(SceneName);
    }

    public void CarselectButton(int carNumber)
    {
        // When we enter the scene for selecting a car, we need to know
        // which player we're selecting a car for. We set that number in
        // the player prefs, so that we can fetch in in the other scene.
        PlayerPrefs.SetInt("Selected player", carNumber);
        PlayerPrefs.Save();
    }
}
