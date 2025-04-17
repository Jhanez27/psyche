using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;

[System.Serializable] 
public class SupabaseError2
{
    public string msg;
}

public class AuthManager2 : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField passwordInput2;
    //public TMP_Text responseText;

    private string supabaseUrl = "https://vrebhfnaijcupcyrcjiw.supabase.co";
    private string supabaseApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZyZWJoZm5haWpjdXBjeXJjaml3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI4NjE3ODAsImV4cCI6MjA1ODQzNzc4MH0.Fe2GEGppHlBfdqNaQJtShw6P7LjU5dfQp-hM6q8UM_U";
    public void SignUp()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();
        string confirmPassword = passwordInput2.text.Trim();

        if (password != confirmPassword)
        {
            // responseText.text = "⚠️ Passwords do not match.";
            Debug.Log(emailInput.text + passwordInput.text);

            return;
        }

        StartCoroutine(SignUpWithSupabase(email, password));
    }

    private IEnumerator SignUpWithSupabase(string email, string password)
    {
        string url = supabaseUrl + "/auth/v1/signup";

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
          //  responseText.text = "✅ Signup successful! Check your email to confirm.";
            Debug.Log("Signup success: " + request.downloadHandler.text);
        }
        else
        {
            string raw = request.downloadHandler.text;
            SupabaseError error = JsonUtility.FromJson<SupabaseError>(raw);
       // responseText.text = "❌ " + (error?.msg ?? "Signup failed.");
            Debug.Log("Signup error: " + raw);
        }
    }
}
