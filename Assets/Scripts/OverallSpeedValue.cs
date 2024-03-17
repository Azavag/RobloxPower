using TMPro;
using UnityEngine;

public class OverallSpeedValue : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI overallSpeedText;


    private void OnEnable()
    {
        PowerControl.CurrentPowerChanged += OnCurrentSpeedChange;
    }
    private void OnDisable()
    {
        PowerControl.CurrentPowerChanged -= OnCurrentSpeedChange;
    }
    void OnCurrentSpeedChange()
    {
        overallSpeedText.text = Bank.Instance.playerInfo.overallPower.ToString();
        YandexSDK.SetNewLeaderboardValue(Bank.Instance.playerInfo.overallPower);
    }
}
