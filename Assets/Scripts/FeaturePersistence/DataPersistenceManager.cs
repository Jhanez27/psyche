using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    public GameData gameData;
    private List<IDataPersistence> dataPersistenceList;
    private FileDataHandler fileHandler;

    protected override void Awake()
    {
        base.Awake();

        fileHandler = new FileDataHandler();
    }
    private void Start()
    {
        // Load the game data when the game starts
        LoadGame();
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // Functions for Game Persistence Management
    public void NewGame()
    {
        this.gameData = new GameData(); //Creates a new GameData
    }
    public void LoadGame()
    {
        // Load any saved data from a file using the data handler
        this.gameData = fileHandler.LoadGameData(gameData, fileName);
        if (this.gameData != null) {
            Debug.Log(gameData.ToString());
        }

        // if no data is found, initialize to a new game
        if (this.gameData == null)
        {
            Debug.Log("No game data was found. Creating new game data with default values");
            NewGame();
        }

        this.dataPersistenceList = FindAllDataPersistenceObjects();

        // push the loaded data to scripts that use the data

        foreach (IDataPersistence dataPersistenceObj in this.dataPersistenceList)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }
    public void SaveGame()
    {

        this.dataPersistenceList = FindAllDataPersistenceObjects();

        //pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in this.dataPersistenceList)
        {
            Debug.Log($"Saving data from {dataPersistenceObj.GetType().Name}");
            dataPersistenceObj.SaveData(ref gameData);
        }

        // Save the data to a file using the data handler
        fileHandler.SaveGameData(gameData, fileName);
    }

    // Functions for Referencing Persisting Data
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    public string GetSceneName()
    {
        return (gameData.isOnApocalypticWorld) ? gameData.apocalypticWorldData.sceneName : gameData.janeWorldData.sceneName;
    }
    public void SwitchWorld()
    {
        if (gameData.isOnApocalypticWorld)
        {
            gameData.isOnApocalypticWorld = false;
        }
        else
        {
            gameData.isOnApocalypticWorld = true;
        }
    }
}
