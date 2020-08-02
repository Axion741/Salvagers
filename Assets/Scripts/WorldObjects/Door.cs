using Assets.Scripts.WorldObjects;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Door : MonoBehaviour, IDoor
{
    private Animator _anim;
    private SpriteRenderer _wDoorPanel;
    private SpriteRenderer _eDoorPanel;
    private Light2D _wDoorLight;
    private Light2D _eDoorLight;
    private Sprite _openPanel;
    private Sprite _closedPanel;

    private bool _doorState;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _doorState = _anim.GetBool("Open");
    }

    private void Start()
    {
        var wPanel = gameObject.transform.Find("WDoorPanel");
        var ePanel = gameObject.transform.Find("EDoorPanel");

        _wDoorPanel = wPanel.GetComponent<SpriteRenderer>();
        _eDoorPanel = ePanel.GetComponent<SpriteRenderer>();
        _wDoorLight = wPanel.transform.Find("WDoorLight").GetComponent<Light2D>();
        _eDoorLight = ePanel.transform.Find("EDoorLight").GetComponent<Light2D>();

        Sprite[] _atlas = Resources.LoadAll<Sprite>("Sprites/Interactables/doorButtons");
        _openPanel = _atlas[0];
        _closedPanel = _atlas[1];
        _wDoorLight.color = Color.red;
        _eDoorLight.color = Color.red;
    }

    private void OpenDoor()
    {
        _anim.SetBool("Open", true);
        _doorState = true;

        SetPanelSprites();
    }

    private void CloseDoor()
    {
        _anim.SetBool("Open", false);
        _doorState = false;

        SetPanelSprites();
    }

    public void ToggleDoor()
    {
        if (_doorState == false)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void SetPanelSprites()
    {
        if (_doorState)
        {
            _wDoorPanel.sprite = _openPanel;
            _wDoorLight.color = Color.green;
            _eDoorPanel.sprite = _openPanel;
            _eDoorLight.color = Color.green;

        }
        else
        {
            _wDoorPanel.sprite = _closedPanel;
            _wDoorLight.color = Color.red;
            _eDoorPanel.sprite = _closedPanel;
            _eDoorLight.color = Color.red;
        }
    }

}
