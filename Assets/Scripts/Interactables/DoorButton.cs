﻿using Assets.Scripts;
using Assets.Scripts.WorldObjects;
using UnityEngine;

public class DoorButton : MonoBehaviour, IInteractable
{
    private IDoor _parent;
    private SpriteRenderer _highlight;
    private string _interactionPrompt = "Press Door Button";

    private void Awake()
    {
        this.gameObject.transform.parent.gameObject.TryGetComponent(out _parent);
        _highlight = gameObject.transform.Find("DoorPanelHighlight").GetComponent<SpriteRenderer>();
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public void UseObject()
    {
        _parent.ToggleDoor();
    } 

    public string GetInteractionPrompt()
    {
        return _interactionPrompt;
    }

    public void TogglePowered(bool toggle)
    {
        if (toggle)
        {
            gameObject.tag = "Interactable";
            _parent.ToggleDoorLights(toggle);
        }
        else
        {
            gameObject.tag = "Environment";
            _parent.ToggleDoorLights(toggle);
        }
    }
}
