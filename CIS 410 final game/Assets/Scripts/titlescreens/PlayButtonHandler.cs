// by Arden Butterfield, last modified May 1
// Draws on this tutorial: https://www.youtube.com/watch?v=05OfmBIf5os

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonHandler : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
