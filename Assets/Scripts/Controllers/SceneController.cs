using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private Animator _anim;

    private int _sceneIndexToLoad;

    void Awake()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        SetupSingleton();

        Invoke("LoadMainMenu", 5);
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _anim.SetTrigger("FadeIn");
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

    void LoadMainMenu()
    {
        FadeToScene(1);
    }

    public void FadeToScene(int index)
    {
        _sceneIndexToLoad = index;

        _anim.SetTrigger("FadeOut");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneIndexToLoad);
    }

    public void ResetGameScene()
    {
        FadeToScene(SceneManager.GetActiveScene().buildIndex);
    }
}
