using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        UpdateGold(GoldManager.instance.CurrentGold);
        GoldManager.instance.OnGoldChanged += UpdateGold;
    }

    private void UpdateGold(int amount)
    {
        goldText.text = $"Gold: {amount}";
    }

    private void OnDestroy()
    {
        if (GoldManager.instance != null)
            GoldManager.instance.OnGoldChanged -= UpdateGold;
    }
}
