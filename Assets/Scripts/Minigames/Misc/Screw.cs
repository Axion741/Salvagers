using UnityEngine;
using UnityEngine.EventSystems;

public class Screw : MonoBehaviour, IPointerClickHandler
{
    public bool interactionDisabled;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactionDisabled)
            Destroy(gameObject);
    }
}
