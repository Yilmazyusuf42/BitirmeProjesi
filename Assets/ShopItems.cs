using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItems : MonoBehaviour
{
    public Item[] items;
    public EquipmentSlot prefab;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in items)
        {
            var slot = Instantiate(prefab.gameObject, transform.position, transform.rotation, transform).GetComponent<EquipmentSlot>();
            slot.AddItem(item.name, item.quantity, item.sprite, item.itemDescription, item.itemType);
            slot.item = item;
            slot.itemImage.sprite = slot.item.sprite != null ? slot.item.sprite : slot.itemImage.sprite;
            slot.GetComponent<Button>().onClick.AddListener(() => InventoryManager.inventoryManager.PressedItem(slot));
        }

    }


    // Update is called once per frame
    void Update()
    {

    }
}
