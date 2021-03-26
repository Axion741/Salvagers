using Assets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment: MonoBehaviour
{
    private List<GameObject> heldEquipmentObjects = new List<GameObject>();

    private List<Equipment> equipmentList;
    public Equipment selectedEquipment;

    private void Awake()
    {
        var heldEquipmentParent = gameObject.transform.Find("HeldItem");

        foreach (Transform transform in heldEquipmentParent.transform)
        {
            heldEquipmentObjects.Add(transform.gameObject);
        }
    }

    private void Start()
    {
        equipmentList = new List<Equipment>();

        AddEquipment(new Equipment { itemType = Equipment.ItemType.Prybar, amount = 1 });
    }

    public void AddEquipment(Equipment equipment)
    {
        equipmentList.Add(equipment);
    }

    public void TryTogglePrybar()
    {
        var equipment = equipmentList.FirstOrDefault(f => f.itemType == Equipment.ItemType.Prybar);

        if (equipment != null)
        {
            if (selectedEquipment != null && selectedEquipment.itemType == Equipment.ItemType.Prybar)
            {
                selectedEquipment = null;
                ToggleHeldItemGameObject();
                Debug.Log("Unequipped Prybar");
            }
            else
            {
                selectedEquipment = equipment;
                ToggleHeldItemGameObject("Prybar");
                Debug.Log("Equipped Prybar");
            }
        }
        else
        {
            Debug.Log("No Prybar in inventory");
            ToggleHeldItemGameObject();
        }
    }

    private void ToggleHeldItemGameObject(string name = "")
    {
        var objects = heldEquipmentObjects;

        foreach(var obj in objects)
        {
            obj.SetActive(false);
        }

        if (HelperFunctions.IsNullOrWhiteSpace(name) != true)
        {
            var target = objects.FirstOrDefault(f => f.name == name);

            if (target != null)
                target.SetActive(true);
        }
    }
}