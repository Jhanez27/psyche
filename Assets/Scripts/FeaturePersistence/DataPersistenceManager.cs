using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceList;
    private FileDataHandler fileHandler;

    protected override void Awake()
    {
        base.Awake();

        this.dataPersistenceList = FindAllDataPersistenceObjects();
        fileHandler = new FileDataHandler();
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

            // push the loaded data to scripts that use the data

        foreach (IDataPersistence dataPersistenceObj in this.dataPersistenceList)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        //pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in this.dataPersistenceList)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // Save the data to a file using the data handler
        fileHandler.SaveGameData(gameData, fileName);

        print(gameData);
    }

    // Functions for Referencing Persisting Data
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
