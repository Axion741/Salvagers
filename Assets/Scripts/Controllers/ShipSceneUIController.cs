using Assets;
using Assets.Scripts.Minigames;
using Assets.Scripts.Models;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSceneUIController : MonoBehaviour
{
    private PlayerController _playerController;
    private SceneController _sceneController;
    private AudioController _audioController;
    private PlayerMovement _playerMovement;

    private bool _characterMenuIsOpen;

    public bool escapeMenuIsOpen;

    public GameObject escapeMenuPanel;
    public GameObject characterMenuPanel;
    public Text characterNameText;
    public Text characterCreditText;
    public Text characterExperienceText;
    public TextMeshProUGUI onscreenPrompt;
    public GameObject interactionPanel;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _sceneController = FindObjectOfType<SceneController>();
        _audioController = FindObjectOfType<AudioController>();
        _playerMovement = FindObjectOfType<PlayerMovement>();

        characterNameText.text = _playerController.playerName;
        characterCreditText.text = _playerController.credits.ToString();
        characterExperienceText.text = _playerController.experience.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && _playerMovement.playerMovementEnabled)
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
                _playerMovement.playerMovementEnabled = true;
                
            Time.timeScale = 1;
        }
            
        else
        {
            escapeMenuPanel.SetActive(true);
            _playerMovement.playerMovementEnabled = false;
            Time.timeScale = 0;
        }

        escapeMenuIsOpen = escapeMenuPanel.activeSelf;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        _sceneController.FadeToScene(1);
        _audioController.FadeTitleMusic(2);
        _audioController.FadeBackgroundMusic(2, null, 0);
    }

    public void SetInteractionPrompt(InteractionPrompt interactionPrompt)
    {
        if (HelperFunctions.IsNullOrWhiteSpace(interactionPrompt.Key))
            onscreenPrompt.text = $"{interactionPrompt.Prompt}";
        else
            onscreenPrompt.text = $"{interactionPrompt.Key} - {interactionPrompt.Prompt}";
    }

    public void ClearInteractionPrompt()
    {
        onscreenPrompt.text = "";
    }

    public void ToggleInteractionPanelVisibility(bool toggle)
    {
        interactionPanel.SetActive(toggle);
    }
}
