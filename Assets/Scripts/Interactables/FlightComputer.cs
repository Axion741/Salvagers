using Assets.Scripts;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightComputer : MonoBehaviour, IInteractable
{
    private PlayerMovement _playerMovement;
    private Shuttle _shuttle;
    private InteractionPrompt _interactionPrompt = new InteractionPrompt();

    private SpriteRenderer _highlight;

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _shuttle = FindObjectOfType<Shuttle>();
    }

    public InteractionPrompt GetInteractionPrompt()
    {
        if (_shuttle.shuttleMovementEnabled)
        {
            _interactionPrompt.Prompt = "Relinquish Controls";
            _interactionPrompt.Key = "R";
        }

        else
        {
            _interactionPrompt.Prompt = "Control Shuttle";
            _interactionPrompt.Key = "E";
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
        if (!_shuttle.shuttleMovementEnabled)
        {
            _shuttle.shuttleMovementEnabled = true;
            _playerMovement.playerMovementEnabled = false;
            _playerMovement.ChildPlayerToShuttle(true);
        }
    }
}