using Assets.Scripts;
using UnityEngine;

public class VerticalDoorButton : MonoBehaviour, IInteractable
{
    private VerticalDoor _parent;
    private SpriteRenderer _highlight;

    private void Awake()
    {
        _parent = GetComponentInParent<VerticalDoor>();
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
