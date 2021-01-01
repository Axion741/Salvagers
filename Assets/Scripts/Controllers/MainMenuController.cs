using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private SceneController _sceneController;
    private AudioController _audioController;

    public GameObject _mainMenuControls;
    public GameObject _optionsMenuControls;

    // Start is called before the first frame update
    void Start()
    {
        _sceneController = FindObjectOfType<SceneController>();
        _audioController = FindObjectOfType<AudioController>();
    }

    public void LoadShipScene()
    {
        _sceneController.FadeToScene(2);
    }

    public void OpenOptions()
    {
        _mainMenuControls.SetActive(false);
        _optionsMenuControls.SetActive(true);
    }

    public void BackToMainMenu()
    {
        _mainMenuControls.SetActive(true);
        _optionsMenuControls.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}