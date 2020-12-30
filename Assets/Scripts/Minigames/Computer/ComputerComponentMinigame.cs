using Assets.Scripts.Minigames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ComputerComponentMinigame : MonoBehaviour, IMinigame
{
    private Computer _parent;
    private AIUnit _aiUnit;
    private CoolingUnit _coolingUnit;
    private HardDrive _hardDrive;
    private PowerUnit _powerUnit;
    private Image _hintImage;
    private Sprite _aiHint;
    private Sprite _coolingHint;
    private Sprite _hdHint;
    private Sprite _powerHint;
    private IComputerComponent selectedComponent;

    private bool _success = false;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void Start()
    {
        _aiUnit = gameObject.GetComponentsInChildren<AIUnit>().SingleOrDefault();
        _aiUnit.SetParent(this);
        _coolingUnit = gameObject.GetComponentsInChildren<CoolingUnit>().SingleOrDefault();
        _coolingUnit.SetParent(this);
        _hardDrive = gameObject.GetComponentsInChildren<HardDrive>().SingleOrDefault();
        _hardDrive.SetParent(this);
        _powerUnit = gameObject.GetComponentsInChildren<PowerUnit>().SingleOrDefault();
        _powerUnit.SetParent(this);
        _hintImage = gameObject.transform.Find("StickyNote/HintImage").GetComponent<Image>();
        _aiHint = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/Crude/AI_Core")[0];
        _coolingHint = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/Crude/Cooling_Unit")[0];
        _hdHint = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/Crude/Hard_Drive")[0];
        _powerHint = Resources.LoadAll<Sprite>("Sprites/Minigames/Computer/ComputerComponents/Crude/Power_core")[0];

        SetRequiredComponent();
        DisableOtherComponents();
    }

    public void SetParent(Computer parent)
    {
        _parent = parent;
    }

    private void SetRequiredComponent()
    {
        var components = new IComputerComponent[] { _aiUnit, _coolingUnit, _hardDrive, _powerUnit };

        if (_parent.targetComponentType != null)
        {
            selectedComponent = components.Single(w => w.GetType() == _parent.targetComponentType);
        }
        else
        {
            selectedComponent = components[Random.Range(0, components.Length)];
            _parent.targetComponentType = selectedComponent.GetType();
        }
        
        switch (selectedComponent)
        {
            case AIUnit a:
                _hintImage.sprite = _aiHint;
                break;

            case CoolingUnit c:
                _hintImage.sprite = _coolingHint;
                break;

            case HardDrive h:
                _hintImage.sprite = _hdHint;
                break;

            case PowerUnit p:
                _hintImage.sprite = _powerHint;
                break;

            default:
                break;
        }
    }

    private void DisableOtherComponents()
    {
        var components = new List<IComputerComponent>() { _aiUnit, _coolingUnit, _hardDrive, _powerUnit };
        components.Remove(selectedComponent);

        foreach (var component in components)
        {
            component.DisableComponentInteraction();
        }
    }

    public IEnumerator ReceiveSuccess()
    {
        _success = true;
        yield return new WaitForSecondsRealtime(1);

        CloseWindow();
    }

    public void CloseWindow()
    {
        Time.timeScale = 1;

        _parent.MinigameResult(_success);
        Destroy(gameObject);
    }

}
