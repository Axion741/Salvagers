using Assets.Scripts;
using System.Runtime.InteropServices;
using UnityEngine;

public class PowerConduit : MonoBehaviour, IInteractable
{
    private SpriteRenderer _highlight;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();

        _spriteRenderer = gameObject.transform.Find("Image").GetComponent<SpriteRenderer>();
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public void UseObject()
    {
        
        _spriteRenderer.sprite = Resources.Load("Sprites/Interactables/ConduitOn", typeof(Sprite)) as Sprite;
        gameObject.tag = "Environment";

        HighlightObject(false);
    }
}
