using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Text loginStatusText;
    public Button loginButton;
    public Button registerButton;

    private string loginUrl = "http://localhost:3000/child-login"; // Change to your server IP address

    void Start()
    {
        loginButton.onClick.AddListener(LoginUser);
        registerButton.onClick.AddListener(GoToRegisterScene);
    }

    public void LoginUser()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        // Create a form to send data to the server
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        StartCoroutine(LoginUserCoroutine(form));
    }

    IEnumerator LoginUserCoroutine(WWWForm form)
    {
        using (var www = UnityWebRequest.Post(loginUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.responseCode == 200)
            {
                loginStatusText.text = "Login Successful";

                PlayerData playerData = ExtractPlayerDataFromResponse(www.downloadHandler.text);

                // Example: Adding a player response
                playerData.AddPlayerResponse("Example Response");

                // Save the skin name in PlayerPrefs
                PlayerPrefs.SetString("SkinName", playerData.skinName);
                PlayerPrefs.Save();

                SceneManager.LoadScene("MainScreen");
            }
            else
            {
                loginStatusText.text = "Error: " + www.error;
            }
        }
    }

    private void GoToRegisterScene()
    {
        SceneManager.LoadScene("Register");
    }

    private PlayerData ExtractPlayerDataFromResponse(string jsonResponse)
    {
        var json = JsonUtility.FromJson<LoginResponse>(jsonResponse);
        return json.playerData;
    }

    [Serializable]
    private class LoginResponse
    {
        public string _id;
        public string username;
        public string password;
        public string parent;
        public PlayerData playerData;
        public int __v;
    }
}
