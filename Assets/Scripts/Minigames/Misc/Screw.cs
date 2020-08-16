using UnityEngine;
using UnityEngine.EventSystems;

public class Screw : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
