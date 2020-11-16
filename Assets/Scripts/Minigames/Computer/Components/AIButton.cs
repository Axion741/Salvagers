using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image _button;
    private Sprite _buttonUp;
    private Sprite _buttonDown;

    public bool pressed = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _button = gameObject.GetComponent<Image>();
        _buttonUp = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/AI/Button_UP")[0];
        _buttonDown = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/AI/Button_DOWN")[0];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _button.sprite = _buttonDown;
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _button.sprite = _buttonUp;
        pressed = false;
    }
}
