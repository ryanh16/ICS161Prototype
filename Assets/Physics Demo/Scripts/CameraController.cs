using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static bool cursorLocked = false;
    public float overallSens;
    float xSens, ySens;
    float xRotation, yRotation;
    public Transform playerOrientation; //stores orientation of player


    void Start()
    {
        xSens = overallSens * 1000f;
        ySens = overallSens * 800f;
        lockCursor();
    }

    void lockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    void unlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        // Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        cursorLocked = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if(cursorLocked)
            {
                unlockCursor();
            }
            else
            {
                lockCursor();
            }
        }
        if(cursorLocked)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSens;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * -ySens;

            yRotation += mouseX;
            xRotation += mouseY;

            xRotation = Mathf.Clamp(xRotation, -90, 90);

            //rotates cam
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
