using UnityEngine;
using UnityEngine.Networking;
using TMPro;  // ✅ Add this to use TMP components
using System.Collections;
using System.Text;

public class AuthManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField passwordInput2; // Optional, if you want confirm password
    public TMP_Text responseText;

    private string supabaseUrl = "https://vrebhfnaijcupcyrcjiw.supabase.co";
    private string supabaseApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZyZWJoZm5haWpjdXBjeXJjaml3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI4NjE3ODAsImV4cCI6MjA1ODQzNzc4MH0.Fe2GEGppHlBfdqNaQJtShw6P7LjU5dfQp-hM6q8UM_U";

    public void OnSignUpClick() => StartCoroutine(SignUp(emailInput.text, passwordInput.text));
    public void OnSignInClick() => StartCoroutine(SignIn(emailInput.text, passwordInput.text));

    IEnumerator SignUp(string email, string password)
    {
        string endpoint = $"{supabaseUrl}/auth/v1/signup";
        string jsonBody = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";

        UnityWebRequest request = new UnityWebRequest(endpoint, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseApiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseText.text = "✅ Sign Up Success!";
        }
        else
        {
            responseText.text = "❌ Sign Up Failed: " + request.downloadHandler.text;
        }
    }

    IEnumerator SignIn(string email, string password)
    {
        string endpoint = $"{supabaseUrl}/auth/v1/token?grant_type=password";
        string jsonBody = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";

        UnityWebRequest request = new UnityWebRequest(endpoint, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseApiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseText.text = "✅ Login Success!";
            string tokenJson = request.downloadHandler.text;
            Debug.Log("Token Response: " + tokenJson);
            // Optionally parse and store the access_token here
        }
        else
        {
            Debug.LogError("❌ Sign Up Failed: " + request.downloadHandler.text);
            responseText.text = "❌ Login Failed: " + request.downloadHandler.text;
        }
    }
}
