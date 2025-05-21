using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    public GameData LoadGameData(GameData gameData, string dataFileName)
    {
        string fullPath = Path.Combine(Application.persistentDataPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        return loadedData;
    }
    public void SaveGameData(GameData gameData, string dataFileName)
    {
        string fullPath = Path.Combine(Application.persistentDataPath,dataFileName);
        try
        {
            // Create Directory if it does not exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the C# game data object into JSON
            string jsonGameData = JsonUtility.ToJson(gameData, true);

            // Write the serialized data to the directory
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(jsonGameData);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        
    }
}
