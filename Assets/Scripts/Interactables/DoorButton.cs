using Assets.Scripts;
using UnityEngine;

public class DoorButton : MonoBehaviour, IInteractable
{
    private Door _parent;
    private SpriteRenderer _highlight;

    private void Awake()
    {
        _parent = GetComponentInParent<Door>();
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
}
