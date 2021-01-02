using UnityEngine;

public class AudioController : MonoBehaviour
{
    private SaveController _saveController;

    public float musicVolume;
    public float sfxVolume;

    // Start is called before the first frame update
    void Start()
    {
        SetupSingleton();

        _saveController = FindObjectOfType<SaveController>();

        musicVolume = _saveController.saveModel.musicVolume;
        sfxVolume = _saveController.saveModel.sfxVolume;
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

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
    }
}