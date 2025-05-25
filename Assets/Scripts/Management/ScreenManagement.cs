using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>, IDataPersistence
{
    public string SceneTransitionName { get; private set; }
    public bool IsInApocalytpicWorld { get; private set; } = true;
    public LoadType LastLoadType { get; private set;} = LoadType.NewGame;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log($"Transition Name: {SceneTransitionName}");
    }

    // IDataPersistence interface implementation
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

        IsInApocalytpicWorld = data.isOnApocalypticWorld;
    }
    public void SaveData(ref GameData data)
    {
        Debug.Log("Saving Scene Data");
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

    //Attribute Setters
    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
        Debug.Log($"Transition Name: {sceneTransitionName}");
    }
    public void SetIsInApocalypticWorld(bool isInApocalypticWorld)
    {
        this.IsInApocalytpicWorld = isInApocalypticWorld;
    }
    public void SetLastLoadType(LoadType loadType)
    {
        this.LastLoadType = loadType;
        Debug.Log($"Last Load Type: {loadType}");
    }
}

public enum LoadType
{
    SceneTransition, // Load a new scene
    LoadGame, // Load a saved game state
    NewGame // Start a new game from the beginning
}
