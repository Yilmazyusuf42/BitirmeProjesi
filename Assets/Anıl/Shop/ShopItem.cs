using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop/Shop Item")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int goldCost;

    public int bonusStrength;
    public int bonusAgility;
    public int bonusVitality;

    // Optional flag for logic clarity (not serialized)
    [HideInInspector] public bool isPurchased = false;
}
