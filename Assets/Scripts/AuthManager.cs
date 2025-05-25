using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;

[System.Serializable]
public class SupabaseError
{
    public string msg;
}

[System.Serializable]
public class SupabaseAuthResponse
{
    public string access_token;
    public string token_type;
    public string refresh_token;
    public int expires_in;
    public SupabaseUser user;
}

[System.Serializable]
public class SupabaseUser
{
    public string id; // unique user ID from Supabase
}

public class AuthManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text responseText;
    public GameObject responseBox;
    public GameObject accountManage;
    public GameObject main_menu;

    private string supabaseUrl = "https://vrebhfnaijcupcyrcjiw.supabase.co";
    private string supabaseApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZyZWJoZm5haWpjdXBjeXJjaml3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI4NjE3ODAsImV4cCI6MjA1ODQzNzc4MH0.Fe2GEGppHlBfdqNaQJtShw6P7LjU5dfQp-hM6q8UM_U"; 

    // 🔐 Store the current user's credentials
    public static string jwtToken;
    public static string userId;

    public void SignIn()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();
        StartCoroutine(LoginWithSupabase(email, password));
    }

    private IEnumerator LoginWithSupabase(string email, string password)
    {
        string url = supabaseUrl + "/auth/v1/token?grant_type=password";
        string jsonBody = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseApiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            SupabaseAuthResponse authResponse = JsonUtility.FromJson<SupabaseAuthResponse>(response);

            jwtToken = authResponse.access_token;
            userId = authResponse.user?.id;

            if (string.IsNullOrEmpty(userId))
            {
                Debug.LogError("❌ Login failed: No user ID returned.");
                responseText.text = "Login failed: Invalid user response.";
                yield break;
            }

            Debug.Log($"✅ Login success.\nAccess token: {jwtToken}\nUser ID: {userId}");

            responseBox.SetActive(true);
            emailInput.text = "";
            passwordInput.text = "";

            accountManage.SetActive(false);
            main_menu.SetActive(false);
        }
        else
        {
            string raw = request.downloadHandler.text;
            SupabaseError error = JsonUtility.FromJson<SupabaseError>(raw);
            responseBox.SetActive(true);
            responseText.text = error?.msg ?? "Login failed. Please try again.";
        }
    }
}
