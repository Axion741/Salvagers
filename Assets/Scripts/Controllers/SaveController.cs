using UnityEngine;

public class SaveController : MonoBehaviour
{
    private AudioController _audioController;
    private PlayerController _playerController;

    public SaveModel saveModel;
    private string saveFileName = "/SalvagersSave.json";

    private void Awake()
    {
        if (DoesSaveFileExist() == true)
        {
            saveModel = LoadSaveModel();
        }
        else
        {
            saveModel = CreateSaveModel();
            SaveGame(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupSingleton();

        _audioController = FindObjectOfType<AudioController>();
        _playerController = FindObjectOfType<PlayerController>();
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
        //PersistantDataPath is AppData\LocalLow\Axion Softworks\Salvagers
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

        if (_playerController != null)
        {
            saveModel.credits = _playerController.credits;
            saveModel.experience = _playerController.experience;
            saveModel.playerName = _playerController.playerName;
        }
    }

    private bool DoesSaveFileExist()
    {
        return System.IO.File.Exists(Application.persistentDataPath + saveFileName);
    }

    public void SaveGame(bool isFirstTimeSaving = false)
    {
        //Initial load skips checking values and uses defaults only
        if (!isFirstTimeSaving)
            UpdateSaveModel();

        var json = JsonUtility.ToJson(saveModel);

        System.IO.File.WriteAllText(Application.persistentDataPath + saveFileName, json);
    }
}
