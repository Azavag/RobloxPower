using TMPro;
using UnityEngine;

public class UpgradesStatsShow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI passiveUpgradeStatsText;
    [SerializeField]
    private TextMeshProUGUI activeUpgradeStatsText;

    string secInterText, clickInterText;
    private void OnEnable()
    {
        SetInternationalText();
        PowerControl.PowerIncreasesChanged += OnStatsValueChanged;
    }
    private void OnDisable()
    {
        PowerControl.PowerIncreasesChanged -= OnStatsValueChanged;
    }
    public void UpdateUpgradeText(TextMeshProUGUI _textField, string _text)
    {
        _textField.text = _text;
    }
    void OnStatsValueChanged()
    {
        string _text = $"+{Bank.Instance.playerInfo.upgradePassivePowerIncrease}/{secInterText}";
        UpdateUpgradeText(passiveUpgradeStatsText, _text);
        _text = $"+{Bank.Instance.playerInfo.upgradeActivePowerIncrease}/{clickInterText}";
        UpdateUpgradeText(activeUpgradeStatsText, _text);
    }

    void SetInternationalText()
    {
        if (Language.Instance.languageName == LanguageName.Rus)
        {
            secInterText = "сек";
            clickInterText = "клик";
        }
        else
        {
            secInterText = "sec";
            clickInterText = "click";
        }
    }
}
