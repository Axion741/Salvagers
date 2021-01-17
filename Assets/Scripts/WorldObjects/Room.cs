using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Room> connectingRooms = new List<Room>();

    private string roomDesignator;
    private bool isPowered;

    public bool isAirlock;
    public bool isCorridor;

    public bool testConnection;

    private void Awake()
    {
        roomDesignator = gameObject.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (testConnection)
        {
            PowerUpRoom(100, 15, true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PowerUpRoom(int successPercentage, int successDegredation = 0, bool perpetuate = false)
    {
        if (successPercentage > 100)
            successPercentage = 100;

        if (successPercentage == 0)
            return;

        var random = Random.Range(0, 101);

        if (random > successPercentage)
            return;

        isPowered = true;

        Debug.Log($"{roomDesignator} Powered Up");

        if (perpetuate)
        {
            foreach (var room in connectingRooms)
            {
                if (!room.isPowered)
                {
                    room.PowerUpRoom(successPercentage - successDegredation, successDegredation, perpetuate);
                }
            }
        }
    }
}
