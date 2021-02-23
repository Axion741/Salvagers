using Assets.Scripts;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private ShipSceneUIController _shipSceneUIController;

    private IInteractable _currentTarget;

    private void Start()
    {
        _shipSceneUIController = FindObjectOfType<ShipSceneUIController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.timeScale > 0)
        {
            if (_currentTarget != null)
            {
                _currentTarget.UseObject();

                //Set interaction prompt again incase the prompt changes after use
                _shipSceneUIController.SetInteractionPrompt("E", _currentTarget.GetInteractionPrompt());
            }
            else
            {
                Debug.LogWarning("Nothing to interact with");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            if (collision.TryGetComponent<IInteractable>(out _currentTarget))
            {
                _currentTarget.HighlightObject(true);
                _shipSceneUIController.SetInteractionPrompt("E", _currentTarget.GetInteractionPrompt());
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

    public void ClearCurrentTargetAndInteraction()
    {
        _currentTarget = null;
        _shipSceneUIController.ClearInteractionPrompt();
    }
}
