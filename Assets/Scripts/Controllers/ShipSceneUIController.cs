using Assets.Scripts.Minigames;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSceneUIController : MonoBehaviour
{
    private PlayerController _playerController;
    private SceneController _sceneController;
    private AudioController _audioController;

    private bool _characterMenuIsOpen;

    public bool escapeMenuIsOpen;

    public GameObject escapeMenuPanel;
    public GameObject characterMenuPanel;
    public Text characterNameText;
    public Text characterCreditText;
    public Text characterExperienceText;
    public TextMeshProUGUI interactionPrompt;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _sceneController = FindObjectOfType<SceneController>();
        _audioController = FindObjectOfType<AudioController>();

        characterNameText.text = _playerController.playerName;
        characterCreditText.text = _playerController.credits.ToString();
        characterExperienceText.text = _playerController.experience.ToString();
    }

    private void Update()
    {
        if (Time.timeScale > 0 && Input.GetKeyDown(KeyCode.C))
            ToggleCharacterMenu();

        if (Input.GetKeyDown(KeyCode.Escape) && escapeMenuIsOpen == false)
            ToggleEscapeMenu();
    }

    public void ToggleCharacterMenu()
    {
        if (_characterMenuIsOpen)
            characterMenuPanel.SetActive(false);

        else
            characterMenuPanel.SetActive(true);

        _characterMenuIsOpen = characterMenuPanel.activeSelf;
    }

    public void ToggleEscapeMenu()
    {
        if (escapeMenuIsOpen)
        {
            escapeMenuPanel.SetActive(false);

            var openMinigame = FindObjectsOfType<MonoBehaviour>().OfType<IMinigame>();

            if (openMinigame == null || openMinigame.Count() <= 0)
                Time.timeScale = 1;
        }
            
        else
        {
            escapeMenuPanel.SetActive(true);
            Time.timeScale = 0;
        }

        escapeMenuIsOpen = escapeMenuPanel.activeSelf;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        _sceneController.FadeToScene(1);
        _audioController.FadeTitleMusic(2, 1);
    }

    public void SetInteractionPrompt(string key, string prompt)
    {
        interactionPrompt.text = $"{key} - {prompt}";
    }

    public void ClearInteractionPrompt()
    {
        interactionPrompt.text = "";
    }
}
