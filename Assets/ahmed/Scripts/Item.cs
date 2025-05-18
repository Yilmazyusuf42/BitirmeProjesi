using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite sprite;
    [TextArea] [SerializeField] private string itemDescription;

    private InventoryManager inventoryManager;
    public ItemType itemType;
    void Start()
    {
      inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        
    }

  private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            inventoryManager.AddItem(itemName,quantity,sprite,itemDescription, itemType, gameObject);
            
        }
        
    }      

    public enum ShopItemType
    {
    ArmorNone,
    Armor_1,
    Armor_2,
    HelmetNone,
    Helmet,
    HealthPotion,
    Sword_1,
    Sword_2
    }

    public static int GetCost(ShopItemType shopItemType)
    {
        switch (shopItemType){
            default:
    case ShopItemType.ArmorNone: return 0;
    case ShopItemType.Armor_1: return 30;
    case ShopItemType.Armor_2: return 100;
    case ShopItemType.HelmetNone: return 0;
    case ShopItemType.Helmet: return 90;
    case ShopItemType.HealthPotion: return 30;
    case ShopItemType.Sword_1: return 0;
    case ShopItemType.Sword_2: return 150;

        }
    }

    public static Sprite GetSprite(ShopItemType shopItemType)
    {
        switch (shopItemType){
            default:
            case ShopItemType.ArmorNone: return GameAssets.i.s_Armor_None;
            case ShopItemType.Armor_1: return GameAssets.i.s_Armor_1;
            case ShopItemType.Armor_2: return GameAssets.i.s_Armor_2;
            case ShopItemType.HelmetNone: return GameAssets.i.s_Helmet_None;
            case ShopItemType.Helmet: return GameAssets.i.s_Helmet_1;
            case ShopItemType.HealthPotion: return GameAssets.i.s_Health_Potion;
            case ShopItemType.Sword_1: return GameAssets.i.s_Sword_1;
            case ShopItemType.Sword_2: return GameAssets.i.s_Sword_2;
        }
    }

}
