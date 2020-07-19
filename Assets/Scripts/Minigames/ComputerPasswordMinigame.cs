using Assets.Scripts.Minigames;
using UnityEngine;
using UnityEngine.UI;

public class ComputerPasswordMinigame : MonoBehaviour, IMinigame
{
    private InputField _input;
    private Computer _parent;

    private string _correctPassword = "PASSWORD";

    private void Awake()
    {
        _input = GameObject.Find("InputField").GetComponent<InputField>();
        Time.timeScale = 0;
    }

    public void SetParent(Computer parent)
    {
        _parent = parent;
    }

    public void CheckPassword()
    {
        var inputString = _input.text;

        if(inputString == _correctPassword)
        {
            Time.timeScale = 1;
            _parent.MinigameResult(true);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Incorrect Password");
        }
    }

    public void CloseWindow()
    {
        Time.timeScale = 1;

        _parent.MinigameResult(false);
        Destroy(gameObject);
    }
}
