using Assets.Scripts;
using UnityEngine;

public class DoorBroken : MonoBehaviour, IInteractable
{
    private Animator _anim;
    private SpriteRenderer _highlight;
    private PlayerController _playerController;
    private PlayerInteraction _playerInteraction;

    private string _interactionPrompt;
    private bool _doorState;

    private void Awake()
    {
        _playerInteraction = FindObjectOfType<PlayerInteraction>();
        _playerController = FindObjectOfType<PlayerController>();
        _anim = gameObject.transform.parent.GetComponent<Animator>();
        _doorState = _anim.GetBool("Open");
        _highlight = gameObject.transform.parent.Find("InteractionHalo").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (_doorState == true)
            DisableInteraction();
    }

    private void OpenDoor()
    {
        _anim.SetBool("Open", true);
        _doorState = true;
        DisableInteraction();
        _playerInteraction.ClearCurrentTargetAndInteraction();
    }

    public void UseObject()
    {
        if (_playerController.hasPrybar())
            OpenDoor();
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public string GetInteractionPrompt()
    {
        if (_playerController.hasPrybar())
            _interactionPrompt = "Pry Door Open";
        else
            _interactionPrompt = "Prybar Needed";

        return _interactionPrompt;
    }

    public void TogglePowered(bool toggle)
    {
        return;
    }

    private void DisableInteraction()
    {
        this.tag = "Environment";
        HighlightObject(false);
    }
}