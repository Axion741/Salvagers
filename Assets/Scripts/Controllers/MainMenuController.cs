using UnityEngine;
using UnityEngine.UI;
using Assets;

public class MainMenuController : MonoBehaviour
{
    private SceneController _sceneController;
    private AudioController _audioController;
    private SaveController _saveController;
    private PlayerController _playerController;

    public GameObject mainMenuControls;
    public GameObject optionsMenuControls;
    public GameObject namePrompt;
    public Text namePromptText;
    public InputField nameInput;
    public Button nameConfirmButton;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        _sceneController = FindObjectOfType<SceneController>();
        _audioController = FindObjectOfType<AudioController>();
        _saveController = FindObjectOfType<SaveController>();
        _playerController = FindObjectOfType<PlayerController>();

        musicSlider.value = _audioController.musicVolume;
        sfxSlider.value = _audioController.sfxVolume;

        if (_saveController.saveModel.playerName == "")
        {
            DisplayNamePrompt();
        }
    }

    private void Update()
    {
        if (namePrompt.activeSelf && HelperFunctions.IsNullOrWhiteSpace(nameInput.text))
        {
            nameConfirmButton.interactable = false;
        }
        else
        {
            nameConfirmButton.interactable = true;
        }

        if (musicSlider.value != _audioController.musicVolume)
            _audioController.SetMusicVolume(musicSlider.value);

        if (sfxSlider.value != _audioController.sfxVolume)
            _audioController.SetSfxVolume(sfxSlider.value);
    }

    public void LoadShipScene()
    {
        _sceneController.FadeToScene(2);
        _audioController.FadeTitleMusic(10, 0);
        _audioController.FadeBackgroundMusic(10, "audio_mangler_science_fiction_ship_power_room_8_055");
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

    private void DisplayNamePrompt()
    {
        namePromptText.text = $"Greetings Salvager #{Random.Range(0, 100000)}.\nPlease Input your preferred designation.";
        namePrompt.SetActive(true);
    }

    public void ConfirmPlayerName()
    {
        var nameInputValue = nameInput.text;

        _playerController.playerName = nameInputValue;
        _saveController.SaveGame();

        namePrompt.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}