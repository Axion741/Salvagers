using Assets.Scripts;
using Assets.Scripts.Models;
using UnityEngine;

public class EquipmentBench : MonoBehaviour, IInteractable
{
    private PlayerEquipment _playerEquipment;
    private PlayerInteraction _playerInteraction;

    private SpriteRenderer _highlight;
    private InteractionPrompt _interactionPrompt = new InteractionPrompt();

    public Equipment.ItemType _itemType;
    public int _itemAmount;

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _playerInteraction = FindObjectOfType<PlayerInteraction>();
        _playerEquipment = FindObjectOfType<PlayerEquipment>();
        _interactionPrompt.Prompt = $"Take {_itemType}";
    }

    public InteractionPrompt GetInteractionPrompt()
    {
        return _interactionPrompt;
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public void TogglePowered(bool toggle)
    {
        return;
    }

    public void UseObject()
    {
        _playerEquipment.AddEquipment(new Equipment { itemType = _itemType, amount = _itemAmount });
        HighlightObject(false);
        gameObject.tag = "Environment";
        _playerInteraction.ClearCurrentTargetAndInteraction();
    }
}