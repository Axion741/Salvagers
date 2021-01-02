using UnityEngine;

public class AudioController : MonoBehaviour
{
    private SaveController _saveController;

    public float musicVolume = 0.5f;
    public float sfxVolume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _saveController = FindObjectOfType<SaveController>();
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
        musicVolume = _saveController.saveModel.musicVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    void GetSfxVolume()
    {
        sfxVolume = _saveController.saveModel.sfxVolume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
    }
}