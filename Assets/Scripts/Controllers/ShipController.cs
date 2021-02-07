using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private List<Room> _rooms = new List<Room>();

    public GameObject environment;

    public string shipClass;
    public float integrity;

    // Start is called before the first frame update
    void Start()
    {
        GetRooms();

        shipClass = "TestShip";
        integrity = 100f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void GetRooms()
    {
        foreach(Transform child in environment.transform)
        {
            var childRoom = child.transform.GetComponent<Room>();
            if (childRoom != null)
                _rooms.Add(childRoom);
        }
    }
}