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
        string interName;
        for (int i = 0; i < enemiesIcons.Length; i++)
        {
            if (Language.Instance.languageName == LanguageName.Rus)
                interName = data[i].enemyRusName;
            else interName = data[i].enemyEngName;
            enemiesIcons[i].InitializeIcon(interName, data[i].faceSprite, data[i].blackWhiteFaceSprite);
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
