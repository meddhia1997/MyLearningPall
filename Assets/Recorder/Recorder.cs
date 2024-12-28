using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace Recorder
{
    [RequireComponent(typeof(AudioSource), typeof(EventTrigger))]
    public class Recorder : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        private AudioSource audioSource;
        private float[] samplesData;
        private const int HEADER_SIZE = 44;
    public GameObject objectToDestroy; // The object to destroy upon saving
 public PhraseGenerator phraseGenrator;
    private float customDeltaTime;

        public static bool isRecording = false;
        private float recordingTime = 0f;
        private int minute = 0, second = 0;

        public KeyCode keyCode;
        public TMP_Text ConsoleText;
        public TMP_Text RecordingTimeText;
        public Button RecordButton;
        public Image RecordImage, SaveImage;
        public int timeToRecord = 30;
        public bool holdToRecord = false;

        void Start()
        {
            // Request iOS Microphone permission
            Application.RequestUserAuthorization(UserAuthorization.Microphone);
         Time.timeScale=0;
        customDeltaTime = Time.unscaledDeltaTime;
            // Check iOS Microphone permission
            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                Debug.Log("Microphone not found");
            }

            // Get the AudioSource component
            audioSource = GetComponent<AudioSource>();
            isRecording = false;
            ConsoleText.text = "";

            if (RecordButton == null)
            {
                Debug.LogError("RecordButton is not set");
                return;
            }
        }
         void Awake()
{
                PlayerPrefs.SetInt("flag", 0);

}

        private void Update()
        {
            customDeltaTime = Time.unscaledDeltaTime;
            if (Input.GetKeyDown(keyCode) && !holdToRecord)
            {            customDeltaTime = Time.unscaledDeltaTime;

                if (isRecording)
                {
                    SaveRecording();
                }
                else
                {
                    StartRecording();
                }
            }

            if (recordingTime >= timeToRecord)
            {
                SaveRecording();
            }

            if (isRecording)
            {
                ConsoleText.text = "";
                recordingTime += Time.unscaledDeltaTime;

                minute = (int)(recordingTime / 60);
                second = (int)(recordingTime % 60);

                RecordingTimeText.text = $"{minute:D2}:{second:D2}";

                RecordImage.gameObject.SetActive(false);
                SaveImage.gameObject.SetActive(true);
            }
            else
            {
                RecordingTimeText.text = "00:00";

                RecordImage.gameObject.SetActive(true);
                SaveImage.gameObject.SetActive(false);
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!holdToRecord)
            {
                if (isRecording)
                {
                    SaveRecording();
                }
                else
                {
                    StartRecording();
                }
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (holdToRecord)
                StartRecording();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (holdToRecord)
                SaveRecording();
        }

        IEnumerator ScaleOverTime(GameObject button, float scaleFactor)
        {
            Vector3 originalScale = button.transform.localScale;
            Vector3 destinationScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            float currentTime = 0.0f;

            do
            {
                button.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / 0.5f);
                currentTime += Time.deltaTime;
                yield return null;
            }
            while (currentTime <= 1f);
        }

        public void StartRecording()
        {
            if (Microphone.devices.Length == 0)
            {
                Debug.LogError("No microphone devices found");
                ConsoleText.text = "No microphone devices found";
                return;
            }

            recordingTime = 0f;
            isRecording = true;

            StartCoroutine(ScaleOverTime(RecordButton.gameObject, 1.2f));

            Microphone.End(null); // Stop the microphone if it is already running
            audioSource.clip = Microphone.Start(null, false, timeToRecord, 44100);
        }

        public void SaveRecording(string fileName = "Audio")
        {
            if (isRecording)
            {
                StartCoroutine(ScaleOverTime(RecordButton.gameObject, 1f));

                while (!(Microphone.GetPosition(null) > 0)) { }
                samplesData = new float[audioSource.clip.samples * audioSource.clip.channels];
                audioSource.clip.GetData(samplesData, 0);

                var samples = samplesData.ToList();
                int recordedSamples = (int)(samplesData.Length * (recordingTime / (float)timeToRecord));

                if (recordedSamples < samplesData.Length)
                {
                    samples.RemoveRange(recordedSamples, samplesData.Length - recordedSamples);
                    samplesData = samples.ToArray();
                }

                AudioClip audioClip = AudioClip.Create(fileName, samplesData.Length, audioSource.clip.channels, 44100, false);
                audioClip.SetData(samplesData, 0);

              

                string filePath = Path.Combine(Application.persistentDataPath, fileName + " " + DateTime.UtcNow.ToString("yyyy_MM_dd HH_mm_ss_ffff") + ".wav");

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                try
                {
                    WriteWAVFile(audioClip, filePath);
                    ConsoleText.text = "Audio Saved at: " + filePath;
                    Debug.Log("File Saved Successfully at " + filePath);
                    StartCoroutine(SendAudioFile(filePath));
                                    PlayerPrefs.SetInt("flag", 1);

                    DestroyObjectInScene();
                }
                catch (DirectoryNotFoundException)
                {
                    Debug.LogError("Persistent Data Path not found!");
                }

                isRecording = false;
                Microphone.End(null);
            }
        }

IEnumerator SendAudioFile(string filePath)
    {
        // Read audio file bytes
        byte[] audioBytes = File.ReadAllBytes(filePath);

        // Get current phrase from PhraseGenerator
        string currentPhrase = phraseGenrator.GetCurrentPhrase(); // Make sure this is correctly initialized

        // Create a new form and add binary data
        WWWForm form = new WWWForm();
        form.AddBinaryData("audioFile", audioBytes, Path.GetFileName(filePath), "audio/wav");
        form.AddField("currentText", currentPhrase); // Pass currentPhrase as a parameter to the server

        // Create UnityWebRequest and assign the form
        UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/uploadaudio/" + currentPhrase, form);

        // Do not manually set the Content-Type header when using WWWForm

        // Send the request and wait for response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Audio file uploaded successfully");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Failed to upload audio file: " + request.error);
        }
    }

        static void WriteWAVFile(AudioClip clip, string filePath)
        {
            float[] clipData = new float[clip.samples];
            clip.GetData(clipData, 0);

            using (FileStream fs = File.Create(filePath))
            {
                int frequency = clip.frequency;
                int numOfChannels = clip.channels;
                int samples = clip.samples;
                fs.Seek(0, SeekOrigin.Begin);

                byte[] riff = Encoding.ASCII.GetBytes("RIFF");
                fs.Write(riff, 0, 4);

                byte[] chunkSize = BitConverter.GetBytes((HEADER_SIZE + clipData.Length * sizeof(short)) - 8);
                fs.Write(chunkSize, 0, 4);

                byte[] wave = Encoding.ASCII.GetBytes("WAVE");
                fs.Write(wave, 0, 4);

                byte[] fmt = Encoding.ASCII.GetBytes("fmt ");
                fs.Write(fmt, 0, 4);

                byte[] subChunk1 = BitConverter.GetBytes(16);
                fs.Write(subChunk1, 0, 4);

                byte[] audioFormat = BitConverter.GetBytes(1);
                fs.Write(audioFormat, 0, 2);

                byte[] numChannels = BitConverter.GetBytes(numOfChannels);
                fs.Write(numChannels, 0, 2);

                byte[] sampleRate = BitConverter.GetBytes(frequency);
                fs.Write(sampleRate, 0, 4);

                byte[] byteRate = BitConverter.GetBytes(frequency * numOfChannels * sizeof(short));
                fs.Write(byteRate, 0, 4);

                ushort blockAlign = (ushort)(numOfChannels * sizeof(short));
                fs.Write(BitConverter.GetBytes(blockAlign), 0, 2);

                ushort bps = 16;
                byte[] bitsPerSample = BitConverter.GetBytes(bps);
                fs.Write(bitsPerSample, 0, 2);

                byte[] datastring = Encoding.ASCII.GetBytes("data");
                fs.Write(datastring, 0, 4);

                byte[] subChunk2 = BitConverter.GetBytes(samples * numOfChannels * sizeof(short));
                fs.Write(subChunk2, 0, 4);

                short[] intData = new short[clipData.Length];
                byte[] bytesData = new byte[clipData.Length * sizeof(short)];

                int conversionFactor = 32767;

                for (int i = 0; i < clipData.Length; i++)
                {
                    intData[i] = (short)(clipData[i] * conversionFactor);
                    byte[] byteArr = BitConverter.GetBytes(intData[i]);
                    Array.Copy(byteArr, 0, bytesData, i * sizeof(short), sizeof(short));
                }

                fs.Write(bytesData, 0, bytesData.Length);
            }
        }
          void DestroyObjectInScene()
    {
        Time.timeScale=1;
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
  
}
