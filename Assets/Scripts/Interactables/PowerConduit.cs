using Assets.Scripts;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PowerConduit : MonoBehaviour, IInteractable
{
    private SpriteRenderer _highlight;
    private SpriteRenderer _spriteRenderer;
    private Light2D _light;

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();

        _spriteRenderer = gameObject.transform.Find("Image").GetComponent<SpriteRenderer>();
        _light = gameObject.transform.Find("Light").GetComponent<Light2D>();
        _light.color = Color.red;
    }

    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public void UseObject()
    {
        _spriteRenderer.sprite = Resources.Load("Sprites/Interactables/ConduitOn", typeof(Sprite)) as Sprite;
        _light.color = Color.green;
        gameObject.tag = "Environment";

        HighlightObject(false);
    }
}
