using System.Collections.Generic;
using UnityEngine;

public class ArenaFightOrder : MonoBehaviour
{
    [SerializeField]
    private ArenaEnemyScriptable[] arenaEnemiesScriptables;
    [SerializeField]
    private EnemiesOrderPanel enemiesOrderPanel;
    private ArenaEnemyBehavior enemyBehavior;
    private SceneSwapper sceneSwapper;

   private static int maxEnemies = 0;

    private static int currentEnemyNumber = 0;

    bool areAllEnemiesDefeated;

    private void Awake()
    {
        enemyBehavior = GetComponentInChildren<ArenaEnemyBehavior>();
        sceneSwapper = FindObjectOfType<SceneSwapper>();
    }
    private void OnValidate()
    {
        enemiesOrderPanel.InitializeEnemyData(arenaEnemiesScriptables);

    }
    void Start()
    {
        currentEnemyNumber = Bank.Instance.playerInfo.currentEnemyNumber;
        InitializeEnemies();    
    }

    void InitializeEnemies()
    {
        enemyBehavior.SetArenaEnemy(arenaEnemiesScriptables[currentEnemyNumber]);
        enemiesOrderPanel.InitializeEnemyData(arenaEnemiesScriptables);
        enemiesOrderPanel.InitializeCrossEnemyIcons();
        maxEnemies = arenaEnemiesScriptables.Length;
    }

    //После победы свапать врагов
    public void SwapNextEnemy()
    {
        enemiesOrderPanel.CrossNewIcon(currentEnemyNumber);
        IncreaseDefeatedEnemiesCount();           
        enemyBehavior.SetArenaEnemy(arenaEnemiesScriptables[currentEnemyNumber]);
    }
    //После победы
    public void IncreaseDefeatedEnemiesCount()
    {
        if (areAllEnemiesDefeated)
            return;
        currentEnemyNumber++;
        enemiesOrderPanel.UpdateKilledCountText();
        if (currentEnemyNumber >= maxEnemies)
        {
            areAllEnemiesDefeated = true;
            sceneSwapper.UnlockLevel();
            currentEnemyNumber = maxEnemies - 1;
        }
    }

    public static int GetMaxEnemiesCount()
    {
        return maxEnemies;
    }
    public static int GetCurrentEnemyNumber()
    {
        return currentEnemyNumber;
    }
}
