using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;
    public Transform itemListParent;
    public GameObject shopItemButtonPrefab;
    public ShopItem[] availableItems;

    private bool isShopOpen = false;

    private void Start()
    {
        PopulateShop();
        shopPanel.SetActive(false); // Hide shop initially
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleShop();
        }
    }

    private void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        shopPanel.SetActive(isShopOpen);
    }

private void PopulateShop()
{
    foreach (ShopItem item in availableItems)
    {
        GameObject go = Instantiate(shopItemButtonPrefab, itemListParent);
        Button button = go.GetComponent<Button>();

        go.transform.Find("ItemNameText").GetComponent<TextMeshProUGUI>().text = item.itemName;
        go.transform.Find("ItemCostText").GetComponent<TextMeshProUGUI>().text = $" {item.goldCost} G";
        go.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.itemIcon;

        button.onClick.AddListener(() => TryPurchase(item, button));
    }
}


private void TryPurchase(ShopItem item, Button button)
{
    if (item.isPurchased)
    {
        Debug.Log("Item already purchased.");
        return;
    }

    if (GoldManager.instance.SpendGold(item.goldCost))
    {
        Debug.Log($"Purchased {item.itemName}");

        CharacterStats stats = PlayerManager.instance.player.GetComponent<CharacterStats>();
        stats.strength.AddModifier(item.bonusStrength);
        stats.agility.AddModifier(item.bonusAgility);
        stats.vitality.AddModifier(item.bonusVitality);

        item.isPurchased = true;
        button.interactable = false; // disable button
    }
    else
    {
        Debug.Log("Not enough gold!");
    }
}


}
