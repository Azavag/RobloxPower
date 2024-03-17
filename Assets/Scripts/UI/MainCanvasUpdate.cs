using TMPro;
using UnityEngine;

public class MainCanvasUpdate : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI currentJumpText;
    [SerializeField]
    private TextMeshProUGUI passiveJumpText;
    [SerializeField]
    private TextMeshProUGUI activeJumpText;
    [SerializeField]
    private TextMeshProUGUI coinsText;


    private void OnEnable()
    {
        PowerControl.PowerIncreasesChanged += OnStatsValueChanged;
        PowerControl.CurrentPowerChanged += OnCurrentSpeedValueChanged;
        Wallet.CoinsChanged += OnCoinsValueChanged;
    }
    private void OnDisable()
    {
        PowerControl.PowerIncreasesChanged -= OnStatsValueChanged;
        PowerControl.CurrentPowerChanged -= OnCurrentSpeedValueChanged;
        Wallet.CoinsChanged -= OnCoinsValueChanged;
    }

    void OnCurrentSpeedValueChanged()
    {
        currentJumpText.text = PowerValueText.ConvertPowerValueText(Bank.Instance.playerInfo.currentPower);
    }
   
    void OnStatsValueChanged() 
    {
        passiveJumpText.text =
             PowerValueText.ConvertPowerValueText(
                 Bank.Instance.playerInfo.upgradePassivePowerIncrease + 
                 Bank.Instance.playerInfo.skinsPassivePowerIncrease);
        activeJumpText.text =
             PowerValueText.ConvertPowerValueText(
                 Bank.Instance.playerInfo.upgradeActivePowerIncrease +
                 Bank.Instance.playerInfo.skinsActivePowerIncrease);
    }

    void OnCoinsValueChanged()
    {
        coinsText.text = Bank.Instance.playerInfo.coins.ToString();
    }
}
