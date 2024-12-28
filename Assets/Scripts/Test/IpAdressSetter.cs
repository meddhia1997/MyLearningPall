using UnityEngine;
using UnityEngine.UI;

public class IpAddressSetter : MonoBehaviour
{
    public InputField ipAddressInputField;
    public Button setIPAddressButton;

    private void Start()
    {
        // Add listener to the button
        setIPAddressButton.onClick.AddListener(SetIPAddress);
    }

    private void SetIPAddress()
    {
        // Get the text from the input field
        string ipAddress = ipAddressInputField.text;

        // Set the IP address to PlayerPrefs
        PlayerPrefs.SetString("IPAddress", ipAddress);
        PlayerPrefs.Save();

        Debug.Log("IP Address set to: " + ipAddress);
    }
}
