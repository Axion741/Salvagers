using Assets.Scripts;
using Assets.Scripts.Minigames;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    private SpriteRenderer _highlight;
    private SpriteRenderer _sprite;
    private GameObject[] _appropriateMinigames;
    private GameObject _selectedMinigame;

    private void Awake()
    {
        _highlight = gameObject.transform.Find("InteractionHalo").GetComponent<SpriteRenderer>();
        _sprite = gameObject.transform.Find("Image").GetComponent<SpriteRenderer>();
        _appropriateMinigames = Resources.LoadAll<GameObject>("Prefabs/Minigames/Computer");
        _selectedMinigame = _appropriateMinigames[Random.Range(0, _appropriateMinigames.Length)];
    }
    public void HighlightObject(bool enabled)
    {
        _highlight.enabled = enabled;
    }

    public void UseObject()
    {
        var minigame = Instantiate(_selectedMinigame);
        var minigameScript = minigame.GetComponent<IMinigame>();
        minigameScript.SetParent(this);
    }

    public void MinigameResult(bool result)
    {
        if(result == true)
        {
            _sprite.sprite = Resources.Load("Sprites/Interactables/ComputerOff", typeof(Sprite)) as Sprite;
            gameObject.tag = "Environment";

            HighlightObject(false);
        }
        else
        {
            return;
        }
    }
}
