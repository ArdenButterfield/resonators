using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Singlplayer : MonoBehaviour
{
    public GameObject player1camera;
    public GameObject player2camera;

    public GameObject player1car;
    public GameObject player2car;

    public GameObject canvas;

    public AudioSource car1Audio;

    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("singleplayer") == 1) {
            player2car.SetActive(false);
            player2camera.SetActive(false);

            player1camera.GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);


            GameObject Nitrometer1 = canvas.transform.Find("Nitrometer1").gameObject;
            GameObject Nitrometer2 = canvas.transform.Find("Nitrometer2").gameObject;
            GameObject WinPanel = canvas.transform.Find("win panel").gameObject;
            GameObject WinPanelText = WinPanel.transform.Find("Win panel text").gameObject;
            
            Nitrometer2.SetActive(false);

            RectTransform N1transform = Nitrometer1.GetComponent<RectTransform>();
            Slider N1Slider = Nitrometer1.GetComponent<Slider>();

            N1Slider.direction = Slider.Direction.LeftToRight;
            N1transform.anchorMax = new Vector2(0.5f,0f);
            N1transform.anchorMin = new Vector2(0.5f,0f);
            N1transform.anchoredPosition = new Vector2(0f,10f);
            N1transform.sizeDelta = new Vector2(800f,20f);

            // We just have 1 car, so we want it's sounds coming from the center.
            car1Audio.panStereo = 0f;
        }
        
    }
}
