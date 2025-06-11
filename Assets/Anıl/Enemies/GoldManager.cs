using UnityEngine;
using System;

public class GoldManager : MonoBehaviour
{
    public static GoldManager instance;

    [SerializeField] private int startingGold = 0;
    public int CurrentGold { get; private set; }

    public event Action<int> OnGoldChanged;

    private const string GoldKey = "TotalGold";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadGold();
    }

    #region Public API

    public void AddGold(int amount)
    {
        if (amount <= 0) return;

        CurrentGold += amount;
        SaveGold();
        OnGoldChanged?.Invoke(CurrentGold);
    }

    public bool SpendGold(int amount)
    {
        if (amount > CurrentGold)
            return false;

        CurrentGold -= amount;
        SaveGold();
        OnGoldChanged?.Invoke(CurrentGold);
        return true;
    }

    public void SetGold(int amount)
    {
        CurrentGold = Mathf.Max(0, amount);
        SaveGold();
        OnGoldChanged?.Invoke(CurrentGold);
    }

    public void ResetGold(bool save = true)
    {
        CurrentGold = 0;

        if (save)
            PlayerPrefs.DeleteKey(GoldKey);

        OnGoldChanged?.Invoke(CurrentGold);
    }

    #endregion

    #region Persistence

    private void SaveGold()
    {
        PlayerPrefs.SetInt(GoldKey, CurrentGold);
        PlayerPrefs.Save();
    }

    public void LoseHalfGold()
{
    int lostGold = Mathf.FloorToInt(CurrentGold * 0.5f);
    CurrentGold -= lostGold;
    SaveGold();
    OnGoldChanged?.Invoke(CurrentGold);

    Debug.Log($"☠️ Lost {lostGold} gold on death. Remaining: {CurrentGold}");
}


    private void LoadGold()
    {
        CurrentGold = PlayerPrefs.GetInt(GoldKey, startingGold);
        OnGoldChanged?.Invoke(CurrentGold);
    }

    #endregion
}
