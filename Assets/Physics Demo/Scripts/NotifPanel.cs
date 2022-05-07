using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifPanel : MonoBehaviour
{
    Text text;
    void Awake()
    {
        text = GetComponentInChildren<Text>();
    }
    void Update()
    {
        if(CameraController.cursorLocked)
        {
            GetComponent<Image>().enabled = false;
            text.enabled = false;
        }
        else
        {
            GetComponent<Image>().enabled = true;
            text.enabled = true;
        }
    }
}
