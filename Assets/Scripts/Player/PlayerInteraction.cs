using Assets.Scripts;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private ShipSceneUIController _shipSceneUIController;
    private PlayerEquipment _playerEquipment;

    private IInteractable _currentTarget;

    private void Start()
    {
        _playerEquipment = FindObjectOfType<PlayerEquipment>();
        _shipSceneUIController = FindObjectOfType<ShipSceneUIController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.timeScale > 0)
        {
            if (_currentTarget != null)
            {
                _currentTarget.UseObject();

                //Second null check incase target cleared by use action
                //Set interaction prompt again incase the prompt changes after use
                if (_currentTarget != null)
                    UpdateInteractionPrompt();
            }
            else
            {
                Debug.LogWarning("Nothing to interact with");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _playerEquipment.TryTogglePrybar();
            UpdateInteractionPrompt();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            if (collision.TryGetComponent<IInteractable>(out _currentTarget))
            {
                _currentTarget.HighlightObject(true);
                UpdateInteractionPrompt();
            }
            else
            {
                Debug.LogError($"{collision.name} interaction not implemented.");
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        if(_currentTarget != null && collision.GetComponent<IInteractable>() == _currentTarget)
        {
            _currentTarget.HighlightObject(false);
            _currentTarget = null;
            _shipSceneUIController.ClearInteractionPrompt();
        }
    }

    public void UpdateInteractionPrompt()
    {
        if (_currentTarget != null)
            _shipSceneUIController.SetInteractionPrompt(_currentTarget.GetInteractionPrompt());
    }

    public void ClearCurrentTargetAndInteraction()
    {
        _currentTarget = null;
        _shipSceneUIController.ClearInteractionPrompt();
    }
}