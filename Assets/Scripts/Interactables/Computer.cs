using Assets.Scripts;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    private SpriteRenderer _highlight;

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();
    }
    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public void UseObject()
    {
        Debug.Log("Computer Used");
        gameObject.tag = "Environment";
        HighlightObject(false);
    }
}
