using Assets.Scripts.WorldObjects;
using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AirlockDoor : MonoBehaviour, IDoor
{
    private AirlockDoor _dockedDoor;

    private Animator _anim;
    private SpriteRenderer _doorPanel;
    private Sprite _openPanel;
    private Sprite _closedPanel;
    private Light2D _panelLight;

    public bool doorSeal;
    private bool _doorState;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _doorState = _anim.GetBool("Open");
    }

    private void Start()
    {
        var doorPanelObject = gameObject.transform.Find("DoorPanel");
        _doorPanel = doorPanelObject.GetComponent<SpriteRenderer>();
        _panelLight = doorPanelObject.transform.Find("PanelLight").GetComponent<Light2D>();

        Sprite[] _atlas = Resources.LoadAll<Sprite>("Sprites/Interactables/doorButtons");
        _openPanel = _atlas[0];
        _closedPanel = _atlas[1];
        _panelLight.color = Color.red;
    }

    private void Update()
    {
        if (_dockedDoor)
        {
            if (!_dockedDoor.doorSeal)
            {
                doorSeal = false;
                CloseDoor();
                _dockedDoor = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Airlock")
        {
            doorSeal = true;
            _dockedDoor = collision.gameObject.GetComponent<AirlockDoor>();
        }
    }

    public void OpenDoor()
    {
        _anim.SetBool("Open", true);
        _doorState = true;

        SetPanelSprites();
    }

    public void CloseDoor()
    {
        _anim.SetBool("Open", false);
        _doorState = false;

        SetPanelSprites();
    }

    public void ToggleDoor()
    {
        if (_doorState == false && doorSeal == true)
        {
            _dockedDoor.OpenDoor();
            OpenDoor();
        }
        else if (_doorState == true && doorSeal == true)
        {
            _dockedDoor.CloseDoor();
            CloseDoor();
        }
        else
        {
            Debug.LogError("No Airlock Seal");
        }
    }

    public void SetPanelSprites()
    {
        if (_doorState)
        {
            _doorPanel.sprite = _openPanel;
            _panelLight.color = Color.green;
        }
        else
        {
            _doorPanel.sprite = _closedPanel;
            _panelLight.color = Color.red;
        }
    }

    public void ToggleDoorLights(bool toggle)
    {
        return;
    }
}
