﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerUnit : MonoBehaviour, IPointerClickHandler, IComputerComponent
{
    private ComputerComponentMinigame _parent;
    private ShipSceneUIController _shipSceneUIController;

    private List<Image> _offLights = new List<Image>();
    private List<PowerSwitch> _powerSwitches = new List<PowerSwitch>();
    private GameObject _leftKeeper;
    private GameObject _rightKeeper;

    private bool _interactionDisabled;
    private bool _keepersPopped;

    // Start is called before the first frame update
    void Start()
    {
        _shipSceneUIController = FindObjectOfType<ShipSceneUIController>();
        _offLights.AddRange(gameObject.transform.Find("Lights").GetComponentsInChildren<Image>());
        _powerSwitches.AddRange(gameObject.GetComponentsInChildren<PowerSwitch>());
        _leftKeeper = gameObject.transform.Find("Keepers/KeeperLeft").gameObject;
        _rightKeeper = gameObject.transform.Find("Keepers/KeeperRight").gameObject;

        for (int i = 0; i < _powerSwitches.Count; i++)
        {
            _powerSwitches[i].SetPairedLight(_offLights[i]);
        }

        if (_interactionDisabled)
        {
            DisableSwitches();
        }
    }

    private void Update()
    {
        if (_interactionDisabled || _shipSceneUIController.escapeMenuIsOpen)
        {
            return;
        }

        if (AllLightsOff() && !_keepersPopped)
        {
            PopKeepers();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_keepersPopped && !_shipSceneUIController.escapeMenuIsOpen)
        {
            gameObject.GetComponent<Image>().enabled = false;
            foreach (var light in _offLights)
            {
                light.enabled = false;
            }
            foreach (var powerSwitch in _powerSwitches)
            {
                powerSwitch.switchEnabled = false;
            }
            StartCoroutine(_parent.ReceiveSuccess());
        }
    }

    public void SetParent(ComputerComponentMinigame parent)
    {
        _parent = parent;
    }

    private bool AllLightsOff()
    {
        bool lightsOff = true;

        foreach (var light in _offLights)
        {
            if (light.enabled != true)
            {
                lightsOff = false;
                break;
            }
        }

        return lightsOff;
    }

    private void PopKeepers()
    {
        _leftKeeper.transform.localPosition = new Vector3(_leftKeeper.transform.localPosition.x - 25f, _leftKeeper.transform.localPosition.y, _leftKeeper.transform.localPosition.z);
        _rightKeeper.transform.localPosition = new Vector3(_rightKeeper.transform.localPosition.x + 25f, _rightKeeper.transform.localPosition.y, _rightKeeper.transform.localPosition.z);
        _keepersPopped = true;
    }

    private void DisableSwitches()
    {
        foreach (var pswitch in _powerSwitches)
        {
            pswitch.interactionDisabled = true;
        }
    }

    public void DisableComponentInteraction()
    {
        _interactionDisabled = true;
    }
}
