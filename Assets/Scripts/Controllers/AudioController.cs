using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private SaveController _saveController;

    public AudioSource backgroundMusicSource;

    private Coroutine _backgroundMusicCoroutine;
    public float musicVolume;
    public float sfxVolume;

    // Start is called before the first frame update
    void Start()
    {
        SetupSingleton();

        _saveController = FindObjectOfType<SaveController>();

        musicVolume = _saveController.saveModel.musicVolume;
        sfxVolume = _saveController.saveModel.sfxVolume;

        backgroundMusicSource.volume = musicVolume;
        backgroundMusicSource.Play();
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
        backgroundMusicSource.volume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
    }

    public void FadeTitleMusic(float duration, float targetVolume)
    {
        if (_backgroundMusicCoroutine != null)
            StopCoroutine(_backgroundMusicCoroutine);

        _backgroundMusicCoroutine = StartCoroutine(FadeAudioSource(backgroundMusicSource, duration, targetVolume, true));
    }

    public void FadeMusic(AudioSource audioSource, float duration, float targetVolume, bool stopAudioWhenVolumeZero)
    {
        if (_backgroundMusicCoroutine != null)
            StopCoroutine(_backgroundMusicCoroutine);

        _backgroundMusicCoroutine = StartCoroutine(FadeAudioSource(audioSource, duration, targetVolume, stopAudioWhenVolumeZero));
    }

    private IEnumerator FadeAudioSource(AudioSource audioSource, float duration, float targetVolume, bool stopAudioWhenVolumeZero)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        if (start == 0 && audioSource.isPlaying == false) 
        {
            audioSource.Play();
        }

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);

            if (stopAudioWhenVolumeZero && audioSource.volume == 0 && targetVolume == 0) //Check target vol to prevent accidental stops when the track has just started fading in
            {
                audioSource.Stop();
            }

            yield return null;
        }
        yield break;
    }
}