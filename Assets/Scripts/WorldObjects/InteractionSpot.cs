using UnityEngine;

public class InteractionSpot : MonoBehaviour
{
    private GameObject[] _interactablePrefabs;

    private void Awake()
    {
        _interactablePrefabs = Resources.LoadAll<GameObject>("Prefabs/Interactables");

        SpawnInteractable();

        Destroy(gameObject);
    }

    private void SpawnInteractable()
    {
        var i = Random.Range(0, _interactablePrefabs.Length);
        var selectedInteractable = _interactablePrefabs[i];

        Instantiate(selectedInteractable, transform.position, transform.rotation);
    }
}
