using UnityEngine;
using UnityEngine.UI;

public class ShipSceneUIController : MonoBehaviour
{
    private PlayerController _playerController;

    private bool _characterMenuIsOpen;

    public GameObject characterMenuPanel;
    public Text characterNameText;
    public Text characterCreditText;
    public Text characterExperienceText;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();

        characterNameText.text = _playerController.playerName;
        characterCreditText.text = _playerController.credits.ToString();
        characterExperienceText.text = _playerController.experience.ToString();
    }

    private void Update()
    {
        if (Time.timeScale > 0 && Input.GetKeyDown(KeyCode.C))
            ToggleCharacterMenu();
    }

    public void ToggleCharacterMenu()
    {
        if (_characterMenuIsOpen)
            characterMenuPanel.SetActive(false);

        else
            characterMenuPanel.SetActive(true);

        _characterMenuIsOpen = characterMenuPanel.activeSelf;
    }
}
