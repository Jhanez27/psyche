using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Playgame()
    {
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync("IntroCutscene");

    }

    public void Loadgame()
    {
        SceneManagement.Instance.SetLastLoadType(LoadType.LoadGame);
        SceneManagement.Instance.SetTransitionName(DataPersistenceManager.Instance.gameData.apocalypticWorldData.sceneTransitionName);
        SceneManager.LoadSceneAsync(DataPersistenceManager.Instance.GetSceneName());
    }

    public void QuitGame()
    {
        DataPersistenceManager.Instance.SaveGame();
        Application.Quit();
    }
}