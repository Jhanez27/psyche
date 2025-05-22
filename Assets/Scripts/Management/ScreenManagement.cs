using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>, IDataPersistence
{
    public string SceneTransitionName { get; private set; }
    public bool IsInApocalytpicWorld { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Debug.Log($"Transition Name: {SceneTransitionName}");

    }
    public void LoadData(GameData data)
    {
        Debug.Log("Krazy Load Data from Scene");
        if (IsInApocalytpicWorld)
        {
            SceneTransitionName = data.apocalypticWorldData.sceneTransitionName;
        }
        else
        {
            SceneTransitionName = data.janeWorldData.sceneTransitionName;
        }
    }

    public void SaveData(ref GameData data)
    {
        if (IsInApocalytpicWorld)
        {
            data.apocalypticWorldData.sceneName = SceneManager.GetActiveScene().name;
            data.apocalypticWorldData.InitializeApocalypticWorldSceneTransitionData(SceneTransitionName);
        }
        else
        {
            data.janeWorldData.sceneName = SceneManager.GetActiveScene().name;
            data.janeWorldData.InitializeJaneWorldSceneTransitionData(SceneTransitionName);
        }
    }

    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
        Debug.Log($"Transition Name: {sceneTransitionName}");
    }

    public void SetIsInApocalypticWorld(bool isInApocalypticWorld)
    {
        this.IsInApocalytpicWorld = isInApocalypticWorld;
    }

}

