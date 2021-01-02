using UnityEngine;

public class SaveController : MonoBehaviour
{
    private AudioController _audioController;

    public SaveModel saveModel;
    private string saveFileName = "/SalvagersSave.json";

    // Start is called before the first frame update
    void Start()
    {
        _audioController = FindObjectOfType<AudioController>();

        SetupSingleton();

        if (DoesSaveFileExist() == true)
        {
            saveModel = LoadSaveModel();
        }
        else
        {
            saveModel = CreateSaveModel();
            SaveGame();
        }
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

    private SaveModel CreateSaveModel()
    {
        var save = new SaveModel();

        return save;
    }

    private SaveModel LoadSaveModel()
    {
        var json = System.IO.File.ReadAllText(Application.persistentDataPath + saveFileName);

        var save = JsonUtility.FromJson<SaveModel>(json);

        return save;
    }

    private void UpdateSaveModel()
    {
        if (_audioController != null)
        {
            saveModel.musicVolume = _audioController.musicVolume;
            saveModel.sfxVolume = _audioController.sfxVolume;
        }
    }

    private bool DoesSaveFileExist()
    {
        return System.IO.File.Exists(Application.persistentDataPath + saveFileName);
    }

    public void SaveGame()
    {
        UpdateSaveModel();

        var json = JsonUtility.ToJson(saveModel);

        System.IO.File.WriteAllText(Application.persistentDataPath + saveFileName, json);
    }
}
