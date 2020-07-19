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
        if(collision.tag == "Interactable")
        {
            Debug.Log("Enter");
            _currentTarget = collision.GetComponent<IInteractable>();
            _currentTarget.HighlightObject(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
        if(collision.tag == "Interactable")
        {
            if(collision.GetComponent<IInteractable>() == _currentTarget)
            {
                _currentTarget.HighlightObject(false);
                _currentTarget = null;
            }
        }
    }
}
