// by Arden Butterfield, last modified May 4

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class SelectionManager : MonoBehaviour
{
    public Transform[] CarTransforms;
    public string[] CarDescriptions;

    public Transform CameraTransform;

    public Vector3 cameraPositionOffset;

    public TextMeshProUGUI InfoText;

    public Quaternion step;
    public float RotationSpeed = 10f;

    private int selected_item;
    private int selectedPlayer;
    
    
    // Start is called before the first frame update
    void Start()
    {
        selectedPlayer = PlayerPrefs.GetInt("Selected player");
        step.eulerAngles = new Vector3(0, 0.2f, 0);
        if (selectedPlayer == 1)
        {
            selected_item = PlayerPrefs.GetInt("Player 1 car");
        }
        else
        {
            selected_item = PlayerPrefs.GetInt("Player 2 car");
        }
        
        setCameraPosition();
        InfoText.text = CarDescriptions[selected_item];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion rot = Quaternion.identity;
        float angle = (Time.time * RotationSpeed) % 360f;
        rot.eulerAngles = new Vector3(0f, angle, 0f);
        int i;
        for (i = 0; i < CarTransforms.Length; i++)
        {
            CarTransforms[i].rotation = rot;
        }
    }

    public void ChangeSelection(bool forwards)
    {
        if (forwards) {
            selected_item += 1;
        } else {
            selected_item -= 1;
        }
        if (selected_item < 0)
        {
            selected_item = CarTransforms.Length - 1;
        }
        if (selected_item >= CarTransforms.Length)
        {
            selected_item = 0;
        }

        setCameraPosition();
        InfoText.text = CarDescriptions[selected_item];

    }

    void setCameraPosition()
    {
        Vector3 carpos = CarTransforms[selected_item].position;
        CameraTransform.position = carpos + cameraPositionOffset;
    }

    public void ExitSelection()
    {
        if (selectedPlayer == 1)
        {
            PlayerPrefs.SetInt("Player 1 car", selected_item);
        }
        else 
        {
            PlayerPrefs.SetInt("Player 2 car", selected_item);
        }
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("title screen");
    }
}
