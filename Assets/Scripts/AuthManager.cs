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
            responseBox.SetActive(true);
            emailInput.text = "";
            passwordInput.text = "";
            Debug.Log("Login success: " + response);
            accountManage.SetActive(false);
            main_menu.SetActive(false);
        }
        else
        {
            string raw = request.downloadHandler.text;
            SupabaseError error = JsonUtility.FromJson<SupabaseError>(raw);
            responseBox.SetActive(true);
            responseText.text = "" + (error?.msg ?? "Login failed. Please try again.");
        }
    }
}
