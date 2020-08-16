using Assets.Scripts.Minigames;
using UnityEngine;

public class ComputerComponentMinigame : MonoBehaviour, IMinigame
{
    private Computer _parent;

    private bool _success = false;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    public void SetParent(Computer parent)
    {
        _parent = parent;
    }

    public void CloseWindow()
    {
        Time.timeScale = 1;

        _parent.MinigameResult(_success);
        Destroy(gameObject);
    }

}
