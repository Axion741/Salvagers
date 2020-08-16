using System.Collections.Generic;
using UnityEngine;

public class ComputerBackplate : MonoBehaviour
{
    private List<Screw> _screws = new List<Screw>();

    // Start is called before the first frame update
    void Start()
    {
        _screws.AddRange(gameObject.GetComponentsInChildren<Screw>());
    }

    private void Update()
    {
        if (AtLeastOneScrew())
        {
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private bool AtLeastOneScrew()
    {
        bool screwExists = false;

        foreach(var screw in _screws)
        {
            if(screw != null)
            {
                screwExists = true;
                break;
            }
        }

        return screwExists;
    }
}
