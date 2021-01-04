using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CoolingUnit : MonoBehaviour, IPointerClickHandler, IComputerComponent
{
    private ComputerComponentMinigame _parent;
    private ShipSceneUIController _shipSceneUIController;
    private List<Screw> _screws = new List<Screw>();

    private bool _interactionDisabled;

    // Start is called before the first frame update
    void Start()
    {
        _shipSceneUIController = FindObjectOfType<ShipSceneUIController>();
        _screws.AddRange(gameObject.GetComponentsInChildren<Screw>());

        if (_interactionDisabled)
        {
            DisableScrews();
        }
    }

    // Update is called once per frame
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AtLeastOneScrew() || _interactionDisabled || _shipSceneUIController.escapeMenuIsOpen)
        {
            return;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = false;
            StartCoroutine(_parent.ReceiveSuccess());
        }
    }

    public void SetParent(ComputerComponentMinigame parent)
    {
        _parent = parent;
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

    private void DisableScrews()
    {
        foreach (var screw in _screws)
        {
            screw.interactionDisabled = true;
        }
    }

    public void DisableComponentInteraction()
    {
        _interactionDisabled = true;
    }
}
