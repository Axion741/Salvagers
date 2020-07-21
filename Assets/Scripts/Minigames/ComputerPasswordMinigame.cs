using Assets.Scripts.Minigames;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerPasswordMinigame : MonoBehaviour, IMinigame
{
    private InputField _input;
    private Text _loginText;
    private Text _downloadText;

    private Computer _parent;

    private string _correctPassword = "PASSWORD";
    private bool _closing = false;
    private bool _success = false;

    private void Awake()
    {
        _input = GameObject.Find("InputField").GetComponent<InputField>();
        _loginText = GameObject.Find("LoginResult").GetComponent<Text>();
        _downloadText = GameObject.Find("DownloadResult").GetComponent<Text>();

        Time.timeScale = 0;
    }

    private void Start()
    {
        _input.Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ProcessInput();
        }
    }

    public void SetParent(Computer parent)
    {
        _parent = parent;
    }

    public void ProcessInput()
    {
        if (!_closing)
        {
            _input.DeactivateInputField();
            _loginText.color = Color.green;
            _loginText.text = "PROCESSING...";

            var checkPassCor = CheckPassword(3f);
            StartCoroutine(checkPassCor);
        }
    }

    private IEnumerator CheckPassword(float timescale)
    {
        yield return new WaitForSecondsRealtime(timescale);

        var inputString = _input.text;

        if(inputString == _correctPassword)
        {
            _loginText.text = "SUCCESS";
            _loginText.color = Color.green;
            _downloadText.text = $"0%";
            _downloadText.color = Color.green;

            var downloadCor = DownloadFile(1f);
            StartCoroutine(downloadCor);
        }
        else
        {
            if (!_closing)
            {
                _loginText.color = Color.red;
                _loginText.text = "INCORRECT PASSWORD";
                _input.text = string.Empty;
                _input.Select();
                _input.ActivateInputField();
            }
        }
    }

    private IEnumerator DownloadFile(float timescale)
    {
        yield return new WaitForSecondsRealtime(timescale);
        var download = 0;

        while (download < (99 - Random.Range(3, 7)))
        {
            download++;
            _downloadText.text = $"{download}%";
            yield return new WaitForSecondsRealtime(0.05f);
        }

        while (download < 99)
        {
            download++;
            _downloadText.text = $"{download}%";
            yield return new WaitForSecondsRealtime(0.05f * Random.Range(5, 15));
        }

        _downloadText.text = "COMPLETE";
        _success = true;

        var completeCor = CompleteDownload(2f);
        StartCoroutine(completeCor);
    }

    private IEnumerator CompleteDownload(float timescale)
    {
        yield return new WaitForSecondsRealtime(timescale);

        Time.timeScale = 1;
        _parent.MinigameResult(_success);
        Destroy(gameObject);

    }

    public void CloseWindow()
    {
        _closing = true;
        Time.timeScale = 1;

        _parent.MinigameResult(_success);
        Destroy(gameObject);
    }
    
}
