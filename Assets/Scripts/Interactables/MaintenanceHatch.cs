using Assets.Scripts;
using Assets.Scripts.Models;
using UnityEngine;

public class MaintenanceHatch : MonoBehaviour, IInteractable
{
    private Animator _anim;
    private SpriteRenderer _highlight;
    private PlayerInteraction _playerInteraction;
    private InteractionPrompt _interactionPrompt = new InteractionPrompt();
    
    private bool _doorState;

    private void Awake()
    {
        _playerInteraction = FindObjectOfType<PlayerInteraction>();
        _anim = gameObject.transform.parent.GetComponent<Animator>();
        _doorState = _anim.GetBool("Open");
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();
    }

    public InteractionPrompt GetInteractionPrompt()
    {
        if (_doorState == false)
        {
            _interactionPrompt.Prompt = "Open Maintenance Hatch";
        }
        else
        {
            _interactionPrompt.Prompt = "Close Maintenance Hatch";
        }

        return _interactionPrompt;
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public void TogglePowered(bool toggle)
    {
        return;
    }

    public void UseObject()
    {
        _doorState = !_doorState;
        _anim.SetBool("Open", _doorState);
        _playerInteraction.UpdateInteractionPrompt();
    }
}