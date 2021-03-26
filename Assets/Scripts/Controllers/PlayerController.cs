using System;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SaveController _saveController;

    public int credits;
    public int experience;
    public string playerName;

    // Start is called before the first frame update
    void Start()
    {
        SetupSingleton();

        _saveController = FindObjectOfType<SaveController>();

        credits = _saveController.saveModel.credits;
        experience = _saveController.saveModel.experience;
        playerName = _saveController.saveModel.playerName;
    }

    void SetupSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddCredits(int amount)
    {
        credits += amount;
    }

    public void RemoveCredits(int amount)
    {
        credits -= amount;
    }

    public void AddExperience(int amount)
    {
        experience += amount;
    }

    public void RemoveExperience(int amount)
    {
        experience -= amount;
    }

    public void SetPlayerName(string input)
    {
        playerName = input;
    }
}