using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPressKeyManager : MonoBehaviour
{
    public KeyCode key;
    public Button button;
    private ColorBlock buttonColor;

    void Awake()
    {
        button = GetComponent<Button>();
        buttonColor = GetComponent<Button>().colors;
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            button.onClick.Invoke();
        }
    }

    //void OnClick()
    //{
        //buttonColor.pressedColor = Color.red;
        //button.colors = buttonColor;
    //}

    void ChangeColor(bool enabled)
    {
        buttonColor.pressedColor = Color.red;
        button.colors = buttonColor;
    }
}
