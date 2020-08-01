using Assets.Scripts.WorldObjects;
using UnityEngine;

public class AirlockDoor : MonoBehaviour, IDoor
{
    private Animator _anim;
    private SpriteRenderer _doorPanel;
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
        _doorPanel = gameObject.transform.Find("DoorPanel").GetComponent<SpriteRenderer>();
        Sprite[] _atlas = Resources.LoadAll<Sprite>("Sprites/Interactables/doorButtons");
        _openPanel = _atlas[0];
        _closedPanel = _atlas[1];
    }

    public void ToggleDoor()
    {
        if (_doorState == false)
        {
            _anim.SetBool("Open", true);
            _doorState = true;
        }
        else
        {
            _anim.SetBool("Open", false);
            _doorState = false;
        }

        SetPanelSprites();
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
