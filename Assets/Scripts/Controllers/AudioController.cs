using UnityEngine;

public class AudioController : MonoBehaviour
{
    public float musicVolume;
    public float sfxVolume;

    // Start is called before the first frame update
    void Start()
    {
        SetupSingleton();
        GetMusicVolume();
        GetSfxVolume();
    }

    void SetupSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void GetMusicVolume()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);
        musicVolume = volume;
    }

    void GetSfxVolume()
    {
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
    }

    public void SetSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat("sfxVolume", volume);
        sfxVolume = volume;
    }
}