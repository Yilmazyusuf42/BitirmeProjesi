using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;

public class UI_Shop : MonoBehaviour
{
   // private Transform container;
    //private Transform shopItemTemplate;
    private IShopCustomer shopCustomer;
    
[SerializeField] private Transform container;
[SerializeField] private Transform shopItemTemplate;


    private void Awake()
    {

       /* container = transform.Find("Container");
        if (container == null)
        {
            Debug.LogError("UI_Shop: 'Container' not found as a child of UI_Shop GameObject!");
            return;
        }

        shopItemTemplate = container.Find("ShopItemTemplate");
        if (shopItemTemplate == null)
        {
            Debug.LogError("UI_Shop: 'ShopItemTemplate' not found under 'Container'!");
            return;
        }*/

        shopItemTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        CreateItemButton(Item.ShopItemType.Armor_1,Item.GetSprite(Item.ShopItemType.Armor_1), "Armor 1", Item.GetCost(Item.ShopItemType.Armor_1), 0);
        CreateItemButton(Item.ShopItemType.Armor_2,Item.GetSprite(Item.ShopItemType.Armor_2), "Armor 2", Item.GetCost(Item.ShopItemType.Armor_2), 1);
        CreateItemButton(Item.ShopItemType.Helmet,Item.GetSprite(Item.ShopItemType.Helmet), "Helmet", Item.GetCost(Item.ShopItemType.Helmet), 2);
        CreateItemButton(Item.ShopItemType.Sword_2,Item.GetSprite(Item.ShopItemType.Sword_2), "Sword", Item.GetCost(Item.ShopItemType.Sword_2), 3);
        CreateItemButton(Item.ShopItemType.HealthPotion,Item.GetSprite(Item.ShopItemType.HealthPotion), "HealthPotion", Item.GetCost(Item.ShopItemType.HealthPotion), 4);

        Hide();

    }
    private void CreateItemButton(Item.ShopItemType shopItemType,Sprite itemSprite, string itemName, int itemCost, int positionIndex) {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 30f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;

        shopItemTransform.GetComponent<Button_UI>().ClickFunc = () => 
        {
            TryBuyItem(shopItemType);

        };
    }

    private void TryBuyItem(Item.ShopItemType shopItemType)
    {
        shopCustomer.BoughtItem(shopItemType);

    }
    public void Show(IShopCustomer shopCustomer)
    {
        this.shopCustomer = shopCustomer;
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
