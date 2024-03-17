using System;
using TMPro;
using UnityEngine;

public class UpgradesShopFields : MonoBehaviour
{
    [Header("Price Texts")]
    [SerializeField]
    private TextMeshProUGUI[] passivePriceTexts;
    [SerializeField]
    private TextMeshProUGUI[] activePriceTexts;

    [Header("Value Texts")]
    [SerializeField]
    private TextMeshProUGUI[] passiveValueTexts;
    [SerializeField]
    private TextMeshProUGUI[] activeValueTexts;

    int[] activeUpgradesNumbers;
    int[] passiveUpgradesNumbers;
    private UpgradesShop upgradesShop;  
    private UpgradesData upgradesData;
    string secInterText, clickInterText;

    private void Awake()
    {
        upgradesData = FindObjectOfType<UpgradesData>();
        upgradesShop = FindObjectOfType<UpgradesShop>();
        SetInternationalText();
    }
    private void OnEnable()
    {
        activeUpgradesNumbers = upgradesShop.GetActiveNumbers();
        passiveUpgradesNumbers = upgradesShop.GetPassiveNumbers();

        Initialize();
    }

    public void Initialize()
    {
        if (activeUpgradesNumbers.Length != activePriceTexts.Length)
            throw new ArgumentException(
           "Active arrays size are not equal.", nameof(activeUpgradesNumbers.Length));

        if (passiveUpgradesNumbers.Length != passivePriceTexts.Length)
            throw new ArgumentException(
           "Passive arrays size are not equal.", nameof(passiveUpgradesNumbers.Length));

        for (int i = 0; i < passivePriceTexts.Length; i++)
        {
            passivePriceTexts[i].text = upgradesData.GetPassiveUpgradePrice(
                passiveUpgradesNumbers[i]).ToString();

            passiveValueTexts[i].text = 
                $"+{upgradesData.GetPassiveUpgradeValue(passiveUpgradesNumbers[i])}" +
                $"/{secInterText}";
        }

        for (int i = 0; i < activePriceTexts.Length; i++)
        {
            activePriceTexts[i].text = upgradesData.GetActiveUpgradePrice(
                activeUpgradesNumbers[i]).ToString();

            activeValueTexts[i].text = 
                $"+{upgradesData.GetActiveUpgradeValue(activeUpgradesNumbers[i])}" +
                $"/{clickInterText}";
        }


    }

    public void SetNumberArrays(int[] activeNumbers, int[] passiveNumbers)
    {
        activeUpgradesNumbers = activeNumbers;
        passiveUpgradesNumbers = passiveNumbers;
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
