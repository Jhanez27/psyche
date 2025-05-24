using System;
using UnityEngine;
using UnityEngine.Networking;

public class SupabaseDownloader : MonoBehaviour
{
    public static SupabaseDownloader Instance { get; private set; }

    [Header("Supabase Settings")]
    [SerializeField] private string supabaseUrl = "https://vrebhfnaijcupcyrcjiw.supabase.co";
    [SerializeField] private string supabaseApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZyZWJoZm5haWpjdXBjeXJjaml3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI4NjE3ODAsImV4cCI6MjA1ODQzNzc4MH0.Fe2GEGppHlBfdqNaQJtShw6P7LjU5dfQp-hM6q8UM_U";
    [SerializeField] private string bucketName = "gameprogress";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Synchronously downloads a JSON file from Supabase.
    /// WARNING: Blocks the main thread. Use only when absolutely necessary.
    /// </summary>
    /// <param name="fileName">The file name (e.g., "user123.json")</param>
    /// <returns>The JSON content as a string, or null on failure.</returns>
    public string DownloadJSONSync(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogError("SupabaseDownloader: Filename is null or empty.");
            return null;
        }
        string userFilePath = $"{AuthManager.userId}/{fileName}";
        string fileUrl = $"{supabaseUrl}/storage/v1/object/{bucketName}/{userFilePath}";
        string result = null;

        using (UnityWebRequest request = UnityWebRequest.Get(fileUrl))
        {
            request.SetRequestHeader("apikey", supabaseApiKey);
            request.SetRequestHeader("Authorization", "Bearer " + AuthManager.jwtToken);

            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                // Blocking loop – use only in editor or with caution
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                result = request.downloadHandler.text;
            }
            else
            {
                Debug.LogError($"SupabaseDownloader: Failed to download {fileName}: {request.error}");
            }
        }

        return result;
    }
}
