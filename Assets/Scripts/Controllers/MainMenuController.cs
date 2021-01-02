using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private SceneController _sceneController;
    private AudioController _audioController;
    private SaveController _saveController;

    public GameObject mainMenuControls;
    public GameObject optionsMenuControls;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        _sceneController = FindObjectOfType<SceneController>();
        _audioController = FindObjectOfType<AudioController>();
        _saveController = FindObjectOfType<SaveController>();
        musicSlider.value = _audioController.musicVolume;
        sfxSlider.value = _audioController.sfxVolume;
    }

    private void Update()
    {
        if (musicSlider.value != _audioController.musicVolume)
            _audioController.SetMusicVolume(musicSlider.value);

        if (sfxSlider.value != _audioController.sfxVolume)
            _audioController.SetSfxVolume(sfxSlider.value);
    }

    public void LoadShipScene()
    {
        _sceneController.FadeToScene(2);
    }

    public void OpenOptions()
    {
        mainMenuControls.SetActive(false);
        optionsMenuControls.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenuControls.SetActive(true);
        optionsMenuControls.SetActive(false);
        _saveController.SaveGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}