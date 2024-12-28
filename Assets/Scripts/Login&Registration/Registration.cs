using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Registration : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    public InputField parentEmailInputField;
    public Text registrationStatusText;
    public Button registerButton;
    public Button loginButton;
    public SkinChooser skinChooser;

    private string registrationUrl = "http://localhost:3000/register"; // Change to your server IP address

    void Start()
    {
        registerButton.onClick.AddListener(Register);
        loginButton.onClick.AddListener(GoToLogin);
    }

    public void Register()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        string parentEmail = parentEmailInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(parentEmail))
        {
            registrationStatusText.text = "Please fill in all fields.";
            return;
        }

        string skinName = skinChooser.skinNames[skinChooser.currentIndex];

        // Create an empty player data object
        PlayerData playerData = new PlayerData
        {
            username = username,
            parentEmail = parentEmail,
            skinName = skinName,
            // Initialize the arrays with empty values
            PlayerResponses = new PlayerAction[0],
            WritingTests = new PlayerAction[0],
            PronunciationAccuracies = new PlayerAction[0]
        };

        // Convert player data object to JSON
        string playerDataJson = JsonUtility.ToJson(playerData);

        // Create a form to send data to the server
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("parentEmail", parentEmail);
        form.AddField("playerData", playerDataJson);

        StartCoroutine(RegisterUser(form));
    }

    IEnumerator RegisterUser(WWWForm form)
    {
        using (var www = new WWW(registrationUrl, form))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                registrationStatusText.text = www.text;
                if (www.text == "Success")
                {
                    SceneManager.LoadScene("Login");
                }
            }
            else
            {
                registrationStatusText.text = "Error: " + www.error;
            }
        }
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("Login");
    }
}
