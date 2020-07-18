using Assets.Scripts;
using UnityEngine;

public class VerticalDoorButton : MonoBehaviour, IInteractable
{
    private VerticalDoor _parent;

    private void Awake()
    {
        _parent = GetComponentInParent<VerticalDoor>();
    }

    public void UseObject()
    {
        _parent.ToggleDoor();
    } 
}
