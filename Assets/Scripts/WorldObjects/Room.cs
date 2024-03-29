﻿using Assets.Scripts;
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
    private SpriteGlowEffect[] _powerLines;
    public List<Room> connectingRooms = new List<Room>();

    private bool _conduitFlag;
    private bool _localShipPower;
    private int _shuttlePowerDepth;
    private string roomDesignator;
    public bool hasShuttlePower;
    public bool isAirlock;
    public bool isCorridor;
    public string roomSpecialty;
    public int? shuttlePowerIndex;

    private void Awake()
    {
        roomDesignator = gameObject.name;
        _powerLines = gameObject.transform.Find("PowerLines").GetComponentsInChildren<SpriteGlowEffect>();
        SetPowerLineColor(Color.red);

        _roomLights = gameObject.transform.Find("RoomLights").GetComponentsInChildren<RoomLight>();
        _interactionSpots = gameObject.transform.GetComponentsInChildren<InteractionSpot>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        _shipController = FindObjectOfType<ShipController>();
        _shuttlePowerDepth = FindObjectOfType<Shuttle>().shuttlePowerDepth;

        if (_interactionSpots.Count > 0)
            PopulateInteractables();

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

        if (_shipController.shipPower && !_localShipPower)
            PowerUpRoom("ship");

        else if (!_shipController.shipPower && _localShipPower)
            PowerDownRoom("ship");

        if (!hasShuttlePower)
            CheckShuttlePower();
    }

    public void PowerUpRoom(string source)
    {
        if (source == "ship")
            PowerUpFromShip();

        if (source == "shuttle")
            PowerUpFromShuttle();
   
        if (ConduitIsFixedOrAbsent())
            ToggleInteractables(true);

        //Debug.Log($"{roomDesignator} Powered Up by {source}");
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

        if (!HasAnyPower())
            ToggleInteractables(false);

        //Debug.Log($"{roomDesignator} Powered Down by {source}");
    }

    public bool HasAnyPower()
    {
        return _shipController.shipPower || hasShuttlePower;
    }

    private void SetPowerLineColor(Color color)
    {
        foreach (var line in _powerLines)
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

        _localShipPower = true;
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

        _localShipPower = false;
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

    private void CheckShuttlePower()
    {
        var shuttlePoweredRooms = connectingRooms.Where(room => room.hasShuttlePower && room.shuttlePowerIndex < _shuttlePowerDepth);

        if (shuttlePoweredRooms.Count() > 0)
        {
            var index = shuttlePoweredRooms.OrderBy(room => room.shuttlePowerIndex).First().shuttlePowerIndex;

            if (!isCorridor)
            {
                index++;
            }

            shuttlePowerIndex = index;
            PowerUpRoom("shuttle");
        }
    }
}