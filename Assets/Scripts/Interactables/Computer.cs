using Assets.Scripts;
using Assets.Scripts.Minigames;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Computer : MonoBehaviour, IInteractable
{
    private PlayerInteraction _playerInteraction;
    private ShipSceneUIController _shipSceneUIController;
    private PlayerMovement _playerMovement;

    private SpriteRenderer _highlight;
    private SpriteRenderer _sprite;
    private Light2D _monitorLight;
    private Light2D _powerLight;
    private GameObject[] _appropriateMinigames;
    private GameObject _selectedMinigame;
    private InteractionPrompt _interactionPrompt = new InteractionPrompt();
    
    private bool _componentMinigameSelected;
    private bool _completed;
    
    public System.Type targetComponentType;

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();
        _monitorLight = gameObject.transform.Find("MonitorLight").GetComponent<Light2D>();
        _powerLight = gameObject.transform.Find("PowerLight").GetComponent<Light2D>();
        _sprite = gameObject.transform.Find("Image").GetComponent<SpriteRenderer>();
        _appropriateMinigames = Resources.LoadAll<GameObject>("Prefabs/Minigames/Computer");
        _selectedMinigame = _appropriateMinigames[Random.Range(0, _appropriateMinigames.Length)];

        _componentMinigameSelected = _selectedMinigame.GetComponent<ComputerComponentMinigame>() != null;

        if (_componentMinigameSelected)
        {
            _powerLight.color = Color.red;
            _monitorLight.color = Color.red;
            _interactionPrompt.Prompt = "Extract Terminal Components";
        }
        else
        {
            _powerLight.color = Color.green;
            _interactionPrompt.Prompt = "Use Terminal";
        }
    }

    private void Start()
    {
        _playerInteraction = FindObjectOfType<PlayerInteraction>();
        _shipSceneUIController = FindObjectOfType<ShipSceneUIController>();
        _playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public void UseObject()
    {
        _shipSceneUIController.ToggleInteractionPanelVisibility(false);
        var minigame = Instantiate(_selectedMinigame);
        var minigameScript = minigame.GetComponent<IMinigame>();
        minigameScript.SetParent(this);
        _playerMovement.playerMovementEnabled = false;
    }

    public void MinigameResult(bool result)
    {
        _shipSceneUIController.ToggleInteractionPanelVisibility(true);
        _playerMovement.playerMovementEnabled = true;

        if (result == true)
        {
            _sprite.sprite = Resources.Load("Sprites/Interactables/ComputerOff", typeof(Sprite)) as Sprite;
            _monitorLight.enabled = false;
            _powerLight.enabled = false;

            gameObject.tag = "Environment";
            _completed = true;

            HighlightObject(false);
            _playerInteraction.ClearCurrentTargetAndInteraction();
        }
        else
        {
            return;
        }
    }

    public InteractionPrompt GetInteractionPrompt()
    {
        return _interactionPrompt;
    }

    public void TogglePowered(bool toggle)
    {
        if (_completed)
            return;

        if (toggle)
        {
            _powerLight.enabled = true;
            _monitorLight.enabled = true;
            gameObject.tag = "Interactable";               
        }
        else 
        {
            _powerLight.enabled = false;
            _monitorLight.enabled = false;
            gameObject.tag = "Environment";
        }
    }
}
