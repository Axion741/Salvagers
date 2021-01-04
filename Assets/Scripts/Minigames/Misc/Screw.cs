using UnityEngine;
using UnityEngine.EventSystems;

public class Screw : MonoBehaviour, IPointerClickHandler
{
    private ShipSceneUIController _shipSceneUIController;

    public bool interactionDisabled;

    private void Start()
    {
        _shipSceneUIController = FindObjectOfType<ShipSceneUIController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactionDisabled || _shipSceneUIController.escapeMenuIsOpen)
            return;

        Destroy(gameObject);
    }
}
