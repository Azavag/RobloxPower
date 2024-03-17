using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesShop : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private PowerControl jumpControl;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private UINavigation uiNavigation;
    [SerializeField]
    private UpgradesShopFields shopFields;
    private SoundController soundController;

    [Header("Upgrades numbers")]
    [SerializeField]
    private int[] passiveUpgradesNumbers;
    [SerializeField]
    private int[] activeUpgradesNumbers;
    [Header("Buttons")]
    [SerializeField]
    private Button[] passiveUpgradeButtons;
    [SerializeField]
    private Button[] activeUpgradeButtons;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private UpgradesData upgradesData;

    Dictionary<Button, Tween> shakeTweenDict = new Dictionary<Button, Tween>();

    public static event Action<int> PurchaseMade;
    private void OnEnable()
    {
        for (int i = 0; i < passiveUpgradeButtons.Length; i++)
        {
            int copy = i;
            passiveUpgradeButtons[copy].onClick.AddListener(
                () => OnClickBuyPassiveUpgrade(passiveUpgradesNumbers[copy], passiveUpgradeButtons[copy]));
            
            Tween shakeTween = ButtonAnimation.ShakeAnimation(passiveUpgradeButtons[copy].transform);
            shakeTweenDict.Add(passiveUpgradeButtons[copy], shakeTween);
        }
        for (int i = 0; i < activeUpgradeButtons.Length; i++)
        {
            int copy = i;
            activeUpgradeButtons[copy].onClick.AddListener(
                ()=>OnClickBuyActiveUpgrade(activeUpgradesNumbers[copy], activeUpgradeButtons[copy]));

            Tween shakeTween = ButtonAnimation.ShakeAnimation(activeUpgradeButtons[copy].transform);
            shakeTweenDict.Add(activeUpgradeButtons[copy], shakeTween);
        }

        closeButton.onClick.AddListener(CloseUpgradeShop);
    }
    private void OnDisable()
    {
        for (int i = 0; i < passiveUpgradeButtons.Length; i++)
        {
            passiveUpgradeButtons[i].onClick.RemoveAllListeners();
            shakeTweenDict[passiveUpgradeButtons[i]].Kill();
        }
        for (int i = 0; i < activeUpgradeButtons.Length; i++)
        {
            activeUpgradeButtons[i].onClick.RemoveAllListeners();
            shakeTweenDict[activeUpgradeButtons[i]].Kill();
        }

        closeButton.onClick.RemoveListener(CloseUpgradeShop);
        shakeTweenDict.Clear();
    }
    private void Start()
    {
        soundController = FindObjectOfType<SoundController>();
    }
    public void OpenUpgradeShop()
    {
        PlayerController.IsBusy = true;
        soundController.MakeClickSound();
        CursorLocking.LockCursor(false);
        playerController.BlockPlayersInput(true);
        uiNavigation.ToggleShopUpgradeCanvas(true);
        uiNavigation.ToggleJoystickCanvas(false);
    }

    void CloseUpgradeShop()
    {
        PlayerController.IsBusy = false;
        soundController.MakeClickSound();
        CursorLocking.LockCursor(true);
        uiNavigation.ToggleShopUpgradeCanvas(false);
        uiNavigation.ToggleJoystickCanvas(true);
        playerController.BlockPlayersInput(false);
    }
    void OnClickBuyPassiveUpgrade(int upgradeNumber, Button clickedButton)
    {
        int price = upgradesData.GetPassiveUpgradePrice(upgradeNumber);
        if (price > Bank.Instance.playerInfo.coins)
        {
            DenyBuy(clickedButton);
            return;
        }
        ConfirmBuy(price);
        PassiveUpgrade(upgradesData.GetPassiveUpgradeValue(upgradeNumber));
    }

    void OnClickBuyActiveUpgrade(int upgradeNumber, Button clickedButton)
    {
        int price = upgradesData.GetActiveUpgradePrice(upgradeNumber);
        if(price > Bank.Instance.playerInfo.coins)
        {
            DenyBuy(clickedButton);
            return;
        }
        ConfirmBuy(price);
        ActiveUpgrade(upgradesData.GetActiveUpgradeValue(upgradeNumber));
    }

    void DenyBuy(Button clickedButton)
    {
        soundController.Play("DeclineBuy");
        shakeTweenDict[clickedButton].Pause();
        shakeTweenDict[clickedButton].Rewind();
        shakeTweenDict[clickedButton].Play();
    }
    
    void ConfirmBuy(int price)
    {
        soundController.Play("ConfirmBuy");
        PurchaseMade?.Invoke(-price);      
    }
    void PassiveUpgrade(int value)
    {
        jumpControl.ChangeUpgradesPassivePowerIncrease(value);
    }
    void ActiveUpgrade(int value)
    {
        jumpControl.ChangeUpgradesActivePowerIncrease(value);
    }

    public int[] GetActiveNumbers()
    {
        return activeUpgradesNumbers;
    }
    public int[] GetPassiveNumbers()
    {
        return passiveUpgradesNumbers;
    }
}
