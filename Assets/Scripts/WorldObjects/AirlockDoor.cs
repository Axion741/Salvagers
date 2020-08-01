using Assets.Scripts.WorldObjects;
using System;
using UnityEngine;

public class AirlockDoor : MonoBehaviour, IDoor
{
    private AirlockDoor _dockedDoor;

    private Animator _anim;
    private SpriteRenderer _doorPanel;
    private Sprite _openPanel;
    private Sprite _closedPanel;

    public bool doorSeal;
    private bool _doorState;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _doorState = _anim.GetBool("Open");
    }

    private void Start()
    {
        _doorPanel = gameObject.transform.Find("DoorPanel").GetComponent<SpriteRenderer>();
        Sprite[] _atlas = Resources.LoadAll<Sprite>("Sprites/Interactables/doorButtons");
        _openPanel = _atlas[0];
        _closedPanel = _atlas[1];
    }

    private void Update()
    {
        if (_dockedDoor)
        {
            if (!_dockedDoor.doorSeal)
            {
                Debug.Log("Door Seal Broken");
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
            Debug.Log("Airlock Trigger");
            Debug.Log(collision);
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
        }
        else
        {
            _doorPanel.sprite = _closedPanel;
        }
    }
}
