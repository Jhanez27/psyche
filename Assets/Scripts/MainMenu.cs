using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Playgame()
    {
        SceneManager.LoadSceneAsync("IntroCutscene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
