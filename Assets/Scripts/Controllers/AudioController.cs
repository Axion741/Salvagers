using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private SaveController _saveController;

    public AudioSource titleMusicSource;
    public AudioSource backgroundMusicSource;

    private Coroutine _titleMusicCoroutine;
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

        titleMusicSource.volume = musicVolume;
        titleMusicSource.time = 5;
        titleMusicSource.Play();
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
        titleMusicSource.volume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
    }

    public void FadeTitleMusic(float duration, float targetVolume = -1)
    {
        if (_titleMusicCoroutine != null)
            StopCoroutine(_titleMusicCoroutine);

        if (targetVolume == -1)
            targetVolume = musicVolume;

        _titleMusicCoroutine = StartCoroutine(FadeAudioSource(titleMusicSource, duration, targetVolume, true));
    }

    public void FadeBackgroundMusic(float duration, string track = null, float targetVolume = -1)
    {
        if (_backgroundMusicCoroutine != null)
            StopCoroutine(_backgroundMusicCoroutine);

        if (targetVolume == -1)
            targetVolume = musicVolume/4;

        if (track != null)
            backgroundMusicSource.clip = Resources.Load($"Audio/Music/{track}") as AudioClip;

        _backgroundMusicCoroutine = StartCoroutine(FadeAudioSource(backgroundMusicSource, duration, targetVolume, true));
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