using Assets.Scripts;
using UnityEngine;

public class EngineeringConsole : MonoBehaviour, IInteractable
{
    private SpriteRenderer _highlight;
    private Room _parentRoom;
    private bool _shipPower;
    private string _interactionPrompt;
    private string _shutdownString = "Shutdown Ship Reactor";
    private string _startupString = "Startup Ship Reactor";

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();
        _parentRoom = gameObject.GetComponentInParent<Room>();
        _shipPower = _parentRoom.hasShipPower;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_shipPower)
        {
            _interactionPrompt = _shutdownString;        
        }
        else
        {
            _interactionPrompt = _startupString;
        }
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
            _interactionPrompt = _startupString;
        }
        else
        {
            _parentRoom.PowerUpRoom("ship", 100, 0, true);
            _interactionPrompt = _shutdownString;
        }

        _shipPower = _parentRoom.hasShipPower;
    }

    public string GetInteractionPrompt()
    {
        return _interactionPrompt;
    }
}
