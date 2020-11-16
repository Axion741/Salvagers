using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AIUnit : MonoBehaviour, IPointerClickHandler
{
    private GameObject _rightKeeper;
    private GameObject _bottomKeeper;
    private AIButton _button;
    private Image _aiFace;
    private Sprite _happyFace;
    private Sprite _mediumFace;
    private Sprite _angryFace;
    private Image _progressBar;

    private bool _keepersPopped;
    private int _progress = 0;

    // Start is called before the first frame update
    void Start()
    {
        _rightKeeper = gameObject.transform.Find("Keepers/KeeperRight").gameObject;
        _bottomKeeper = gameObject.transform.Find("Keepers/KeeperBottom").gameObject;
        _button = gameObject.transform.Find("AIButton").gameObject.GetComponent<AIButton>();
        _aiFace = gameObject.transform.Find("Screen/AIFace").gameObject.GetComponent<Image>();
        _happyFace = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/AI/Happy_Face")[0];
        _mediumFace = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/AI/Medium_Face")[0];
        _angryFace = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/AI/Angry_Face")[0];
        _progressBar = gameObject.transform.Find("BarBackground").gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_keepersPopped)
        {
            return;
        }
        else if (_progress >= 100 && !_keepersPopped)
        {
            PopKeepers();
        }
        else if (_button.pressed)
        {
            ProgressGame();
        }
        else if (_progress > 0)
        {
            RegressGame();
        }

        SetProgressBarColor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_keepersPopped)
        {
            gameObject.GetComponent<Image>().enabled = false;
            _button.GetComponent<Image>().enabled = false;
            _progressBar.GetComponent<Image>().enabled = false;
            gameObject.transform.Find("Bar").GetComponent<Image>().enabled = false;
            gameObject.transform.Find("ScreenBackground").GetComponent<Image>().enabled = false;
            gameObject.transform.Find("Screen").GetComponent<Image>().enabled = false;
        }
    }

    private void PopKeepers()
    {
        _rightKeeper.transform.localPosition = new Vector3(_rightKeeper.transform.localPosition.x + 25f, _rightKeeper.transform.localPosition.y, _rightKeeper.transform.localPosition.z);
        _bottomKeeper.transform.localPosition = new Vector3(_bottomKeeper.transform.localPosition.x, _bottomKeeper.transform.localPosition.y - 25f, _bottomKeeper.transform.localPosition.z);
        _keepersPopped = true;
    }

    private void ProgressGame()
    {
        _progress++;

        SetAIFace();
    }

    private void RegressGame()
    {
        _progress--;

        SetAIFace();
    }

    private void SetAIFace()
    {
        if (_progress <= 33)
            _aiFace.sprite = _happyFace;

        if (_progress > 33 && _progress <= 66)
            _aiFace.sprite = _mediumFace;

        if (_progress > 66 && _progress < 100)
            _aiFace.sprite = _angryFace;

        if (_progress > 99)
            _aiFace.gameObject.SetActive(false);
    }

    private void SetProgressBarColor()
    {
        if (_progress < 50)
        {
            _progressBar.color = Color.Lerp(Color.green, Color.yellow, _progress * Time.fixedDeltaTime);
        }
        else if (_progress >= 50 && _progress < 100)
        {
            _progressBar.color = Color.Lerp(Color.yellow, Color.red, (_progress - 50) * Time.fixedDeltaTime);
        }
        else
        {
            _progressBar.color = Color.black;
        }
    }
}