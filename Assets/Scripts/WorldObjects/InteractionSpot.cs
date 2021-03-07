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

    public void SpawnInteractable(bool isConduit = false)
    {
        GameObject selectedInteractable;

        if (isConduit)
        {
            selectedInteractable = _conduitPrefab;
        }
        else
        {
            var i = Random.Range(0, _interactablePrefabs.Count);
            selectedInteractable = _interactablePrefabs[i];
        }

        Instantiate(selectedInteractable, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}