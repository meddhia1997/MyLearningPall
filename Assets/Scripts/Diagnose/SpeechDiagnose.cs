using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class SpeechDiagnose : MonoBehaviour
{
    private bool isRecording = false;
    private string audioBase64;

    public string serverURL = "http://localhost:3000/sendAudio/";
    public GameObject objectToDestroy; // The object to destroy upon saving
 public PhraseGenerator phraseGenrator;

    public Button recordButton;

    private AudioSource audioSource;
    private AudioClip recordedClip;
    private int recordingTime = 10; // Recording time in seconds

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Assuming the AudioSource is attached to the same GameObject
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is not found!");
        }

        if (recordButton != null)
        {
            recordButton.onClick.AddListener(ToggleRecording);
        }
        else
        {
            Debug.LogError("Record button is not assigned!");
        }
    }


    private void ToggleRecording()
    {
        if (!isRecording)
        {
            StartRecording();
        }
        else
        {
            StopRecording();
        }
    }

    private void StartRecording()
    {
        isRecording = true;
        recordedClip = Microphone.Start(null, false, recordingTime, AudioSettings.outputSampleRate);
        if (recordedClip == null)
        {
            Debug.LogError("Microphone is not available or not working.");
            isRecording = false;
        }
    }

    private void StopRecording()
    {
        isRecording = false;
        Microphone.End(null);

        if (recordedClip != null)
        {
            audioBase64 = AudioClipToBase64(recordedClip);
            StartCoroutine(SendAudioToServer(audioBase64));
 
         DestroyObjectInScene();
         PlayerPrefs.SetInt("flag", 1);



        }
        else
        {
            Debug.LogError("Recording failed, AudioClip is null.");
        }
    }

    private string AudioClipToBase64(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("AudioClip is null.");
            return null;
        }

        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        byte[] byteArray = new byte[samples.Length * 2];
        Buffer.BlockCopy(samples, 0, byteArray, 0, byteArray.Length);

        return Convert.ToBase64String(byteArray);
    }

    IEnumerator SendAudioToServer(string audioData)
    {
        if (string.IsNullOrEmpty(audioData))
        {
            Debug.LogError("Audio data is empty.");
            yield break;
        }
    string currentPhrase = phraseGenrator.GetCurrentPhrase();


        WWWForm form = new WWWForm();
        form.AddField("audio", audioData);
        form.AddField("currentText", currentPhrase); // Assuming 'currentText' is the parameter name expected by your server

        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to send audio: " + www.error);
            }
            else
            {
                Debug.Log("Audio sent successfully!");
            }
        }
    }
        void DestroyObjectInScene()
    {
        // Destroy the assigned object in the scene upon saving
        if (objectToDestroy != null)
        {
        PlayerPrefs.SetInt("flag", 1);

            Destroy(objectToDestroy);
            Debug.Log("Object destroyed from the scene upon saving.");
        }
        else
        {
            Debug.LogWarning("No object assigned to destroy upon saving.");
        }
    }
}
