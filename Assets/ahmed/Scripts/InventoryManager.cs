using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager inventoryManager;
    public GameObject EquipmentMenu;

    public ItemSlot[] itemSlot;
    public EquipmentSlot[] equipmentSlot;
    // Start is called before the first frame update
    [Header("Item Details")]
    public Image MainImage;
    public TMP_Text Streng;
    public TMP_Text intelligence;
    public TMP_Text Defence;
    public TMP_Text Agility;
    [Header("Character Details")]
    public TMP_Text MainStreng;
    public TMP_Text MainIntelligence;
    public TMP_Text MainDefence;
    public TMP_Text MainAgility;

    void Start()
    {
        inventoryManager = this;
        itemSlot.ToList().ForEach(x => x.GetComponent<Button>().onClick.AddListener(() => { DeActivateOthers(x.GetComponent<Button>()); }));

    }
    private void Reset()
    {
        MainStreng.text = CharacterStats.characterStats.strength.baseValue.ToString();
        MainIntelligence.text = CharacterStats.characterStats.intelligence.baseValue.ToString();
        MainDefence.text = CharacterStats.characterStats.armor.baseValue.ToString();
        MainAgility.text = CharacterStats.characterStats.agility.baseValue.ToString();

        MainImage.sprite = null;
        Streng.text =      null;
        Agility.text =     null;
        Defence.text =     null;
        intelligence.text= null;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetButtonDown("EquipmentMenu"))
            Equipment();

    }


    void Equipment()
    {
        if (EquipmentMenu.activeSelf)
        {
            Time.timeScale = 1;
            EquipmentMenu.SetActive(false);

        }
        else
        {
            Time.timeScale = 0;
            EquipmentMenu.SetActive(true);
            Reset();

        }
    }
    public void PressedItem(EquipmentSlot item)
    {
        MainImage.sprite = item.itemImage.sprite;
        Streng.text = item.item.Strength.ToString();
        Agility.text = item.item.Agility.ToString();
        Defence.text = item.item.Defence.ToString();
        intelligence.text = item.item.Intelligence.ToString();

        MainStreng.text = (CharacterStats.characterStats.strength.baseValue + item.item.Strength).ToString();
        MainIntelligence.text = (CharacterStats.characterStats.intelligence.baseValue + item.item.Agility).ToString();
        MainDefence.text = (CharacterStats.characterStats.armor.baseValue + item.item.Defence).ToString();
        MainAgility.text = (CharacterStats.characterStats.agility.baseValue + item.item.Intelligence).ToString();


    }


    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType, GameObject ItemgameObject)
    {
        Destroy(ItemgameObject);

        if (itemType == ItemType.consumable || itemType == ItemType.crafting || itemType == ItemType.collectible)
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
