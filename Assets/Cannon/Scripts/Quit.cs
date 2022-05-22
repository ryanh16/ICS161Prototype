using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    public void reloadScene()
    {
        SceneManager.LoadScene(0);
    }
    public void OnQuitClick()
    {
        Application.Quit();
    }

}
