using Assets.Scripts;
using SpriteGlow;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    private ShipController _shipController;
    private RoomLight[] _roomLights;
    private List<InteractionSpot> _interactionSpots = new List<InteractionSpot>();
    private List<IInteractable> _interactables = new List<IInteractable>();
    private PowerConduit _powerConduit;
    private bool _conduitFlag;
    private bool _localPower;

    private string roomDesignator;

    public List<Room> connectingRooms = new List<Room>();
    public SpriteGlowEffect[] powerLines;

    public bool hasShuttlePower;
    public bool isAirlock;
    public bool isCorridor;
    public string roomSpecialty;

    public bool testConnection;

    private void Awake()
    {
        _shipController = FindObjectOfType<ShipController>();
        roomDesignator = gameObject.name;
        powerLines = gameObject.transform.Find("PowerLines").GetComponentsInChildren<SpriteGlowEffect>();
        SetPowerLineColor(Color.red);

        _roomLights = gameObject.transform.Find("RoomLights").GetComponentsInChildren<RoomLight>();
        _interactionSpots = gameObject.transform.GetComponentsInChildren<InteractionSpot>().ToList();
        if (_interactionSpots.Count > 0)
            PopulateInteractables();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (testConnection)
        {
            PowerUpRoom("shuttle");
        }

        if (HasAnyPower() && ConduitIsFixedOrAbsent())
        {
            ToggleInteractables(true);
        }
        else
        {
            ToggleInteractables(false);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (_conduitFlag && HasAnyPower())
        {
            if (_shipController.shipPower)
                PowerUpRoom("ship");
            else
                PowerUpRoom("shuttle");

            _conduitFlag = false;
        }

        if (_shipController.shipPower && !_localPower)
            PowerUpRoom("ship");

        else if (!_shipController.shipPower && _localPower)
            PowerDownRoom("ship");
    }

    public void PowerUpRoom(string source)
    {
        if (source == "ship")
            PowerUpFromShip();

        if (source == "shuttle")
            PowerUpFromShuttle();

        _localPower = true;

        if (ConduitIsFixedOrAbsent())
            ToggleInteractables(true);

        Debug.Log($"{roomDesignator} Powered Up by {source}");
    }

    public void PowerDownRoom(string source)
    {
        if (source == "ship")
        {
            PowerDownFromShip();
        }

        if (source == "shuttle")
        {
            PowerDownFromShuttle();
        }

        _localPower = false;

        if (!HasAnyPower())
            ToggleInteractables(false);

        Debug.Log($"{roomDesignator} Powered Down by {source}");
    }

    public bool HasAnyPower()
    {
        return _shipController.shipPower || hasShuttlePower;
    }

    private void SetPowerLineColor(Color color)
    {
        foreach (var line in powerLines)
            line.GlowColor = color;
    }

    private void PopulateInteractables()
    {
        var conduitSpot = _interactionSpots[Random.Range(0, _interactionSpots.Count)];

        _powerConduit = conduitSpot.SpawnConduit();
        _powerConduit.SetParentRoom(this);
        _interactionSpots.Remove(conduitSpot);

        if (_interactionSpots.Count > 0)
        {
            foreach (var interactionSpot in _interactionSpots)
            {
                var interactable = interactionSpot.SpawnInteractable();
                _interactables.Add(interactable);
            }
        }
    }

    private bool ConduitIsFixedOrAbsent()
    {
        return _powerConduit == null || _powerConduit.isFixed;
    }

    private void PowerUpFromShip()
    {
        if (ConduitIsFixedOrAbsent())
        {
            SetPowerLineColor(Color.green);
            foreach (var light in _roomLights)
                light.TogglePower(true);
        }
        else
        {
            SetPowerLineColor(Color.yellow);
            foreach (var light in _roomLights)
                light.TogglePower(true, 0.5f);
        }
    }

    private void PowerUpFromShuttle()
    {
        hasShuttlePower = true;

        if (!_shipController.shipPower)
        {
            if (ConduitIsFixedOrAbsent())
            {
                SetPowerLineColor(Color.cyan);
                foreach (var light in _roomLights)
                    light.TogglePower(true, 0.75f);
            }
            else
            {
                SetPowerLineColor(Color.blue);
                foreach (var light in _roomLights)
                    light.TogglePower(true, 0.25f);
            }
        }
    }

    private void PowerDownFromShip()
    {
        if (hasShuttlePower && ConduitIsFixedOrAbsent())
        {
            SetPowerLineColor(Color.cyan);
            foreach (var light in _roomLights)
                light.TogglePower(true, 0.75f);
        }
        else if (hasShuttlePower)
        {
            SetPowerLineColor(Color.blue);
            foreach (var light in _roomLights)
                light.TogglePower(true, 0.25f);
        }
        else
        {
            SetPowerLineColor(Color.red);
            foreach (var light in _roomLights)
                light.TogglePower(false);
        }
    }

    private void PowerDownFromShuttle()
    {
        hasShuttlePower = false;

        if (!_shipController.shipPower)
        {
            SetPowerLineColor(Color.red);
            foreach (var light in _roomLights)
                light.TogglePower(false);
        }
    }

    public void SetConduitFlag()
    {
        _conduitFlag = true;
    }

    private void ToggleInteractables(bool toggle)
    {
        foreach (var interactable in _interactables)
        {
            interactable.TogglePowered(toggle);
        }
    }
}