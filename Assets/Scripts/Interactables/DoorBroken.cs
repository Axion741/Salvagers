using Assets.Scripts;
using Assets.Scripts.Models;
using UnityEngine;

public class DoorBroken : MonoBehaviour, IInteractable
{
    private Animator _anim;
    private SpriteRenderer _highlight;
    private PlayerEquipment _playerEquipment;
    private PlayerInteraction _playerInteraction;

    private InteractionPrompt _interactionPrompt = new InteractionPrompt();
    private bool _doorState;

    private void Awake()
    {
        _playerInteraction = FindObjectOfType<PlayerInteraction>();
        _playerEquipment = FindObjectOfType<PlayerEquipment>();
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
        if (_playerEquipment.selectedEquipment != null && _playerEquipment.selectedEquipment.itemType == Equipment.ItemType.Prybar)
            OpenDoor();
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public InteractionPrompt GetInteractionPrompt()
    {
        if (_playerEquipment.selectedEquipment != null && _playerEquipment.selectedEquipment.itemType == Equipment.ItemType.Prybar)
        {
            _interactionPrompt.Prompt = "Pry Door Open";
            _interactionPrompt.Key = "E";
        }
        else
        {
            _interactionPrompt.Prompt = "Prybar Needed";
            _interactionPrompt.Key = "";
        }

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