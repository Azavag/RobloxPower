using TMPro;
using UnityEngine;

public class BotCanvas : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI nameText; 
    [SerializeField]
    private TextMeshProUGUI powerText;
    [SerializeField]
    private TextMeshProUGUI rewardText;

    public void ToggleCanvas(bool toggle)
    {
        GetComponent<Canvas>().enabled = toggle;
    }

    public void ShowName(string botNameText)
    {
        nameText.text = botNameText;
    }
    public void ShowPower(int power)
    {
        powerText.text = PowerValueText.ConvertPowerValueText(power);
    }
    public void ShowReward(int botRewardValue)
    {
        rewardText.text = botRewardValue.ToString();
    }

}
