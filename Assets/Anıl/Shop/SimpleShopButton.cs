using UnityEngine;
using UnityEngine.UI;

public class SimpleShopButton : MonoBehaviour
{
    public int cost = 50;

    [Header("Stat Bonuses")]
    public int bonusStrength = 0;
    public int bonusAgility = 0;
    public int bonusVitality = 0;

    private Button myButton;

    private void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(BuyItem);
    }

    private void BuyItem()
    {
        if (GoldManager.instance == null || PlayerManager.instance == null)
        {
            Debug.LogWarning("GoldManager or PlayerManager missing!");
            return;
        }

        if (GoldManager.instance.SpendGold(cost))
        {
            var stats = PlayerManager.instance.player.GetComponent<CharacterStats>();
            stats.strength.AddModifier(bonusStrength);
            stats.agility.AddModifier(bonusAgility);
            stats.vitality.AddModifier(bonusVitality);

            Debug.Log($"Bought item for {cost} gold. Stats applied.");
            myButton.interactable = false;
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
}
