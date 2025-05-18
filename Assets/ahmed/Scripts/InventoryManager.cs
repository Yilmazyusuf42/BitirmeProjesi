using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    public GameObject EquipmentMenu;
    
    public ItemSlot[] itemSlot;
    public EquipmentSlot[] equipmentSlot;
    // Start is called before the first frame update
    
    void Start()
    {
        itemSlot.ToList().ForEach(x => x.GetComponent<Button>().onClick.AddListener(() => { DeActivateOthers(x.GetComponent<Button>()); }));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("InventoryMenu"))
           Inventory();
        if(Input.GetButtonDown("EquipmentMenu"))
           Equipment();
     
    }
    void Inventory()
    {
        if (InventoryMenu.activeSelf)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            EquipmentMenu.SetActive(false);
            
        }
        else 
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            EquipmentMenu.SetActive(false);
            
        }
    }

     void Equipment()
    {
        if (EquipmentMenu.activeSelf)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            EquipmentMenu.SetActive(false);
            
        }
        else 
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(false);
            EquipmentMenu.SetActive(true);
            
        }
    }


    public void AddItem(string itemName, int quantity, Sprite itemSprite,string itemDescription, ItemType itemType, GameObject ItemgameObject)
    {
        Destroy(ItemgameObject);
        
        if(itemType == ItemType.consumable || itemType == ItemType.crafting || itemType == ItemType.collectible)
        {

            for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription, itemType);
                return;
            }
        }
        }
        else
        {
            for (int i = 0; i < equipmentSlot.Length; i++)
        {
            if (equipmentSlot[i].isFull == false)
            {         
                 

                equipmentSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription, itemType);
                return;
            }
        }
        }

       
    }

    public void DeselectAllSlot()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;

        }
    }


    public void DeActivateOthers(Button button)
    {
        itemSlot.ToList().ForEach(x =>
        {
            if (x.GetComponent<Button>() == button)
            {
                x.gameObject.GetComponent<Image>().color = Color.white;
            }
            else
            {
                x.gameObject.GetComponent<Image>().color = Color.gray;
            }
        });
    }
}

public enum ItemType
{
    consumable,
    crafting,
    collectible,
    head,
    cloak,
    legs,
    rightHand,
    leftHand,
    relic,
    feet,
    none,
    

};
