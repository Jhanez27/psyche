using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{

    public GameData LoadGameData(GameData gameData, string dataFileName)
{
    string fullPath = Path.Combine(Application.persistentDataPath, dataFileName);
    GameData loadedData = null;

    try
    {
        if (SupabaseDownloader.Instance == null)
        {
        new GameObject("SupabaseDownloader").AddComponent<SupabaseDownloader>();
        }

        string downloadedJson = SupabaseDownloader.Instance.DownloadJSONSync(dataFileName);

        if (!string.IsNullOrEmpty(downloadedJson))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, downloadedJson);
        }
        else
        {
            Debug.LogWarning("Downloaded JSON is empty or null, falling back to local file if exists.");
        }
    }
    catch (Exception ex)
    {
        Debug.LogException(ex);
        Debug.LogWarning("Failed to download from Supabase, will attempt to load local save file.");
    }

    if (File.Exists(fullPath))
    {
        try
        {
            string dataToLoad = File.ReadAllText(fullPath);
            loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    else
    {
        Debug.LogWarning($"Save file not found at path: {fullPath}");
    }

    return loadedData;
}
    public void SaveGameData(GameData gameData, string dataFileName)
{
    string fullPath = Path.Combine(Application.persistentDataPath, dataFileName);

    try
    {
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        string jsonGameData = JsonUtility.ToJson(gameData, true);

        File.WriteAllText(fullPath, jsonGameData); // Save locally first

        // Ensure SupabaseUploader exists
        if (SupabaseUploader.Instance == null)
        {
            new GameObject("SupabaseUploader").AddComponent<SupabaseUploader>();
        }
        SupabaseUploader.Instance.UploadJSON(jsonGameData, dataFileName);
    }
    catch (Exception ex)
    {
        Debug.LogException(ex);
    }
}

}
