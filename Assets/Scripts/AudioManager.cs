using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;
    public AudioClip bossMusic;
    public AudioClip winMusic; 

    private AudioSource audioSource;
    private string currentClipName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic(menuMusic);
                break;
            case "Level01":
            case "Level02":
            case "Level03":
                PlayMusic(levelMusic);
                break;
            case "BossLevel":
                PlayMusic(bossMusic);
                break;
            case "DeathScreen":
                StopMusic();
                break;
            case "WinScreen":
                StopMusic(); // El video ya tiene audio
                break;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || audioSource.clip == clip) return;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMusic() => audioSource.Stop();
    public void PauseMusic() => audioSource.Pause();
    public void ResumeMusic() => audioSource.UnPause();
}