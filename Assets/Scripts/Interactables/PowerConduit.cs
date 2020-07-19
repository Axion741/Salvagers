using Assets.Scripts;
using UnityEngine;

public class PowerConduit : MonoBehaviour, IInteractable
{
    private GameObject _highlight;

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").gameObject;
        _highlight.SetActive(false);
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.SetActive(enabled);
    }

    public void UseObject()
    {
        Debug.Log("Power Conduit Used");
        gameObject.tag = "Environment";
        HighlightObject(false);
    }
}
