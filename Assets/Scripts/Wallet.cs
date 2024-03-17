using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private int coins;
    public static event Action CoinsChanged; 
    private void OnEnable()
    {
        CourseFinish.CourseFinished += OnCoinsValueChanged;
        UpgradesShop.PurchaseMade += OnCoinsValueChanged;
        BuySkinButton.SkinPurchaseMade += OnCoinsValueChanged;
        BuyLevel.LevelUnlocked += OnCoinsValueChanged;
        PunchbagsShop.PunchbagPurchased += OnCoinsValueChanged;
        ArenaFight.EnemyLost += OnCoinsValueChanged;
    }
    private void OnDisable()
    {
        CourseFinish.CourseFinished -= OnCoinsValueChanged;
        UpgradesShop.PurchaseMade -= OnCoinsValueChanged;
        BuySkinButton.SkinPurchaseMade -= OnCoinsValueChanged;
        BuyLevel.LevelUnlocked -= OnCoinsValueChanged;
        PunchbagsShop.PunchbagPurchased -= OnCoinsValueChanged;
        ArenaFight.EnemyLost -= OnCoinsValueChanged;
    }

    void Start()
    {
        coins = Bank.Instance.playerInfo.coins;
        CoinsChanged?.Invoke();
    }
    void OnCoinsValueChanged(int diff)
    {
        coins += diff;
        Bank.Instance.playerInfo.coins = coins;
        YandexSDK.Save();
        CoinsChanged?.Invoke();
    }

}
