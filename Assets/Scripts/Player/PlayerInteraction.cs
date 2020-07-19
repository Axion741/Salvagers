using Assets.Scripts;
using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable _currentTarget;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_currentTarget != null)
            {
                _currentTarget.UseObject();
            }
            else
            {
                Debug.LogError("Nothing to interact with");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            Debug.Log("Enter");
            if (collision.TryGetComponent<IInteractable>(out _currentTarget))
            {
                _currentTarget.HighlightObject(true);
            }
            else
            {
                Debug.LogError($"{collision.name} interaction not implemented.");
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log ("Exit");
        if(collision.tag == "Interactable" && collision.GetComponent<IInteractable>() != null) 
        {
            if(collision.GetComponent<IInteractable>() == _currentTarget)
            {
                _currentTarget.HighlightObject(false);
                _currentTarget = null;
            }
        }
    }
}
