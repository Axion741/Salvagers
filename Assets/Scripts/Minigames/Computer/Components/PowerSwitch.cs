using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerSwitch : MonoBehaviour, IPointerClickHandler
{
    private Image _offLight;
    private Image _switch;
    private Sprite _onSwitch;
    private Sprite _offSwitch;

    public bool interactionDisabled;
    public bool switchEnabled = true;    

    private void Awake()
    {
        _switch = gameObject.GetComponent<Image>();
        _onSwitch = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/Power/Switch_ON")[0];
        _offSwitch = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/Power/Switch_OFF")[0];
    }

    public void SetPairedLight(Image offLight)
    {
        _offLight = offLight;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactionDisabled) 
        {
            return;
        }

        if (switchEnabled == true)
        {
            _offLight.enabled = !_offLight.enabled;

            if (_switch.sprite == _onSwitch)
            {
                _switch.sprite = _offSwitch;
                _switch.gameObject.transform.position = _switch.transform.position + new Vector3(0, -4.5f, 0);
            }
            else
            {
                _switch.sprite = _onSwitch;
                _switch.gameObject.transform.position = _switch.transform.position + new Vector3(0, 4.5f, 0);
            }
        }
    }
}
