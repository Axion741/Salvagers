using Assets.Scripts;
using UnityEngine;

public class EngineeringConsole : MonoBehaviour, IInteractable
{
    private SpriteRenderer _highlight;
    private Room _parentRoom;
    private bool _shipPower;

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();
        _parentRoom = gameObject.GetComponentInParent<Room>();
        _shipPower = _parentRoom.hasShipPower;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public void UseObject()
    {
        if (_shipPower)
        {
            _parentRoom.PowerDownRoom("ship", true);
        }
        else
        {
            _parentRoom.PowerUpRoom("ship", 100, 0, true);
        }

        _shipPower = _parentRoom.hasShipPower;
    }
}
