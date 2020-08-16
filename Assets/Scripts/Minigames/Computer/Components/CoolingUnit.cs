using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CoolingUnit : MonoBehaviour, IPointerClickHandler
{
    private List<Screw> _screws = new List<Screw>();

    // Start is called before the first frame update
    void Start()
    {
        _screws.AddRange(gameObject.GetComponentsInChildren<Screw>());
    }

    // Update is called once per frame
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AtLeastOneScrew())
        {
            return;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
    }

    private bool AtLeastOneScrew()
    {
        bool screwExists = false;

        foreach (var screw in _screws)
        {
            if (screw != null)
            {
                screwExists = true;
                break;
            }
        }

        return screwExists;
    }
}
