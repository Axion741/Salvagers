using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerSwitch : MonoBehaviour, IPointerClickHandler
{
    private Image _offLight;
    private Image _switch;
    private Sprite _onSwitch;
    private Sprite _offSwitch;
    

    private void Awake()
    {
        _switch = gameObject.GetComponent<Image>();
        _onSwitch = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/Power/Switch_ON")[0];
        _offSwitch = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/Power/Switch_OFF")[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPairedLight(Image offLight)
    {
        _offLight = offLight;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _offLight.enabled = !_offLight.enabled;

        if (_switch.sprite == _onSwitch)
        {
            _switch.sprite = _offSwitch;
        }
        else
        {
            _switch.sprite = _onSwitch;
        }
    }
}
