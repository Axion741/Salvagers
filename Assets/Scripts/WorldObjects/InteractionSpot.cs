using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionSpot : MonoBehaviour
{
    private List<GameObject> _interactablePrefabs;
    private GameObject _conduitPrefab;

    private void Awake()
    {
        _interactablePrefabs = Resources.LoadAll<GameObject>("Prefabs/Interactables").ToList();
        _conduitPrefab = _interactablePrefabs.Single(prefab => prefab.GetComponent<PowerConduit>() != null);
        _interactablePrefabs.Remove(_conduitPrefab);
    }

    public PowerConduit SpawnConduit()
    {
        var newConduit = Instantiate(_conduitPrefab, transform.position, transform.rotation);

        Destroy(gameObject);

        return newConduit.GetComponent<PowerConduit>();
    }

    public void SpawnInteractable()
    {
        var i = Random.Range(0, _interactablePrefabs.Count);
        var selectedInteractable = _interactablePrefabs[i];
        
        Instantiate(selectedInteractable, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}