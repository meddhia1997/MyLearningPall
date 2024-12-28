using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance
    {
        get { return instance; }
    }

    public AudioClip musicClip; // Drag your music clip into this field in the Inspector
    private AudioSource musicSource;

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Initialize AudioSource for music playback
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.loop = true; // Loop the music

        // Start playing the music
        PlayMusic();
    }

    void Start()
    {
        // Subscribe to scene loading events
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Stop music if currently in "Level1" scene
        CheckAndStopMusic();
    }

    void OnDestroy()
    {
        // Unsubscribe from scene loading events
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Method to play music
    void PlayMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    // Method to stop music playback
    void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    // Method to check if the current scene is "Level1" and stop music
    void CheckAndStopMusic()
    {
        if (SceneManager.GetActiveScene().name.Equals("Level1"))
        {
            StopMusic();
        }
    }

    // Event handler for scene loaded event
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        // Check if the newly loaded scene is "Level1" and stop music
        if (scene.name.Equals("Level1"))
        {
            StopMusic();
        }
        else
        {
            PlayMusic(); // Resume music for other scenes
        }
    }
}
