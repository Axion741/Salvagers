﻿using Assets.Scripts;
using Assets.Scripts.Models;
using UnityEngine;

public class EngineeringConsole : MonoBehaviour, IInteractable
{
    private ShipController _shipController;
    private SpriteRenderer _highlight;
    private Room _parentRoom;
    private InteractionPrompt _interactionPrompt = new InteractionPrompt();
    private string _shutdownString = "Shut Down Ship Reactor";
    private string _startupString = "Start Up Ship Reactor";

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();
        _parentRoom = gameObject.GetComponentInParent<Room>();
        _shipController = FindObjectOfType<ShipController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_shipController.shipPower)
        {
            _interactionPrompt.Prompt = _shutdownString;        
        }
        else
        {
            _interactionPrompt.Prompt = _startupString;
        }
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public void UseObject()
    {
        if (_shipController.shipPower)
        {
            _shipController.shipPower = !_shipController.shipPower;
            _parentRoom.PowerDownRoom("ship");
            _interactionPrompt.Prompt = _startupString;
        }
        else
        {
            _shipController.shipPower = !_shipController.shipPower;
            _parentRoom.PowerUpRoom("ship");
            _interactionPrompt.Prompt = _shutdownString;
        }
    }

    public InteractionPrompt GetInteractionPrompt()
    {
        return _interactionPrompt;
    }

    public void TogglePowered(bool toggle)
    {
        return;
    }
}
