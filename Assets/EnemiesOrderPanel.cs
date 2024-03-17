using TMPro;
using UnityEngine;

public class EnemiesOrderPanel : MonoBehaviour
{
    [SerializeField]
    private EnemyIcon[] enemiesIcons;
    [SerializeField]
    private TextMeshProUGUI killedCountText;
    private ArenaEnemyScriptable[] templates;

   
    private void Awake()
    {
        enemiesIcons = GetComponentsInChildren<EnemyIcon>();
    }

    public void CrossNewIcon(int enemyNumber)
    {
        enemiesIcons[enemyNumber].AnimateCross();
    }
    public void UpdateKilledCountText()
    {
        killedCountText.text =
            $"{ArenaFightOrder.GetCurrentEnemyNumber()}/{ArenaFightOrder.GetMaxEnemiesCount()}";
    }
    public void InitializeEnemyData(ArenaEnemyScriptable[] data)
    {
        for (int i = 0; i < enemiesIcons.Length; i++)
        {
            enemiesIcons[i].InitializeIcon(data[i].enemyName, data[i].faceSprite, data[i].blackWhiteFaceSprite);
        }
    }
    public void InitializeCrossEnemyIcons()
    {
        for(int i = 0; i < enemiesIcons.Length; i++)
        {
            if(i < Bank.Instance.playerInfo.currentEnemyNumber)
                enemiesIcons[i].SetClosed(true);
            else enemiesIcons[i].SetClosed(false);
        }
        UpdateKilledCountText();
    }

   

}
