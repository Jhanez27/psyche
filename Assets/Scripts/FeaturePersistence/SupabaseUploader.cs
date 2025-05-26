using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;

public class SupabaseUploader : MonoBehaviour
{
    public static SupabaseUploader Instance { get; private set; }

    [Header("Supabase Settings")]
    [SerializeField] private string supabaseUrl = "https://vrebhfnaijcupcyrcjiw.supabase.co";
  [SerializeField] private string supabaseApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZyZWJoZm5haWpjdXBjeXJjaml3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI4NjE3ODAsImV4cCI6MjA1ODQzNzc4MH0.Fe2GEGppHlBfdqNaQJtShw6P7LjU5dfQp-hM6q8UM_U";
    [SerializeField] private string bucketName = "gameprogress";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void UploadJSON(string jsonData, string fileName)
    {
        StartCoroutine(ReplaceFile(jsonData, fileName));
    }

    private IEnumerator ReplaceFile(string jsonData, string fileName)
    {
        if (string.IsNullOrEmpty(AuthManager.jwtToken) || string.IsNullOrEmpty(AuthManager.userId))
        {
            Debug.LogWarning("❌ Upload aborted: User not authenticated.");
            yield break;
        }

        string userFilePath = $"{AuthManager.userId}/{fileName}";
        string fileUrl = $"{supabaseUrl}/storage/v1/object/{bucketName}/{userFilePath}";

        // Step 1: Check if the file exists using a HEAD request
        UnityWebRequest headRequest = UnityWebRequest.Head(fileUrl);
        headRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.jwtToken);
        headRequest.SetRequestHeader("apikey", supabaseApiKey);

        yield return headRequest.SendWebRequest();

        bool fileExists = headRequest.responseCode == 200;

        // Step 2: If file exists, delete it
        if (fileExists)
        {
            UnityWebRequest deleteRequest = UnityWebRequest.Delete(fileUrl);
            deleteRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.jwtToken);
            deleteRequest.SetRequestHeader("apikey", supabaseApiKey);

            yield return deleteRequest.SendWebRequest();

            if (deleteRequest.result != UnityWebRequest.Result.Success)
            {
                string errorText = deleteRequest.downloadHandler != null ? deleteRequest.downloadHandler.text : "No response data";
                Debug.LogWarning($"❌ File deletion failed: {deleteRequest.error}\n{errorText}");
                yield break;
            }

            Debug.Log($"🗑️ Deleted existing file: {userFilePath}");
        }
        else
        {
            Debug.Log($"📁 File not found, continuing to upload: {userFilePath}");
        }

        // Step 3: Upload the new file using multipart/form-data
        byte[] fileData = Encoding.UTF8.GetBytes(jsonData);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, fileName, "application/json");

        string uploadUrl = $"{supabaseUrl}/storage/v1/object/{bucketName}/{userFilePath}?upsert=true";

        UnityWebRequest uploadRequest = UnityWebRequest.Post(uploadUrl, form);
        uploadRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.jwtToken);
        uploadRequest.SetRequestHeader("apikey", supabaseApiKey);

        yield return uploadRequest.SendWebRequest();

        if (uploadRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"✅ Upload successful: {userFilePath}");
        }
        else
        {
            string errorText = uploadRequest.downloadHandler != null ? uploadRequest.downloadHandler.text : "No response data";
            Debug.LogWarning($"❌ Upload failed: {uploadRequest.error}\n{errorText}");
        }
    }
}
