using Assets.Scripts;
using UnityEngine;

public class PowerConduit : MonoBehaviour, IInteractable
{
    private GameObject _highlight;
    private SpriteRenderer _leftCore;
    private SpriteRenderer _rightCore;

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").gameObject;
        _highlight.SetActive(false);

        _leftCore = gameObject.transform.Find("Conduit/LeftCore").GetComponent<SpriteRenderer>();
        _rightCore = gameObject.transform.Find("Conduit/RightCore").GetComponent<SpriteRenderer>();
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.SetActive(enabled);
    }

    public void UseObject()
    {
        Debug.Log("Power Conduit Used");

        _leftCore.color = Color.green;
        _rightCore.color = Color.green;

        gameObject.tag = "Environment";

        HighlightObject(false);
    }
}
