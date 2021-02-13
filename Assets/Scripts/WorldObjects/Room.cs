using SpriteGlow;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Room : MonoBehaviour
{
    public List<Room> connectingRooms = new List<Room>();
    public SpriteGlowEffect[] powerLines;
    private RoomLight[] _roomLights;

    private string roomDesignator;

    public bool hasShuttlePower;
    public bool hasShipPower;
    public bool isAirlock;
    public bool isCorridor;

    public bool testConnection;

    private void Awake()
    {
        roomDesignator = gameObject.name;
        powerLines = gameObject.transform.Find("PowerLines").GetComponentsInChildren<SpriteGlowEffect>();
        SetPowerLineColor(Color.red);

        _roomLights = gameObject.transform.Find("RoomLights").GetComponentsInChildren<RoomLight>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (testConnection)
        {
            PowerUpRoom("shuttle", 100, 20, true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PowerUpRoom(string source, int successPercentage, int successDegredation = 0, bool perpetuate = false)
    {
        if (successPercentage > 100)
            successPercentage = 100;

        if (successPercentage == 0)
            return;

        var random = Random.Range(0, 101);

        if (random > successPercentage)
            return;

        if (source == "ship")
        {
            hasShipPower = true;
            SetPowerLineColor(Color.green);
        }

        if (source == "shuttle" && !hasShipPower)
        {
            hasShuttlePower = true;
            SetPowerLineColor(Color.blue);
        }

        foreach (var light in _roomLights)
            light.TogglePower(true);

        Debug.Log($"{roomDesignator} Powered Up by {source}");

        if (perpetuate)
        {
            foreach (var room in connectingRooms)
            {
                if ((!room.hasShuttlePower && source == "shuttle") || (!room.hasShipPower && source == "ship"))
                {
                    room.PowerUpRoom(source, successPercentage - successDegredation, successDegredation, perpetuate);
                }
            }
        }
    }

    public void PowerDownRoom(string source, bool perpetuate = false)
    {
        if (source == "ship")
        {
            hasShipPower = false;

            if (hasShuttlePower)
            {
                SetPowerLineColor(Color.blue);
            }
            else
            {
                SetPowerLineColor(Color.red)
;            }
        }

        if (source == "shuttle")
        {
            hasShuttlePower = false;

            if (!hasShipPower)
            {
                SetPowerLineColor(Color.red);
            }
        }

        Debug.Log($"{roomDesignator} Powered Down by {source}");

        foreach (var light in _roomLights)
            light.TogglePower(false);

        if (perpetuate)
        {
            foreach (var room in connectingRooms)
            {
                if ((room.hasShuttlePower && source == "shuttle") || (room.hasShipPower && source == "ship"))
                {
                    room.PowerDownRoom(source, perpetuate);
                }
            }
        }
    }

    public bool HasAnyPower()
    {
        return hasShipPower || hasShuttlePower;
    }

    private void SetPowerLineColor(Color color)
    {
        foreach (var line in powerLines)
            line.GlowColor = color;
    }
}
