using UnityEngine;
using UnityEngine.U2D;

public class Door : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _wDoorPanel;
    private SpriteRenderer _eDoorPanel;
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
        _wDoorPanel = gameObject.transform.Find("WDoorPanel").GetComponent<SpriteRenderer>();
        _eDoorPanel = gameObject.transform.Find("EDoorPanel").GetComponent<SpriteRenderer>();
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

    private void SetPanelSprites()
    {
        if (_doorState)
        {
            _wDoorPanel.sprite = _openPanel;
            _eDoorPanel.sprite = _openPanel;
        }
        else
        {
            _wDoorPanel.sprite = _closedPanel;
            _eDoorPanel.sprite = _closedPanel;
        }
    }

}
