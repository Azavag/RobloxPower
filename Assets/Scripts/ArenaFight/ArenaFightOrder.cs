using System.Collections.Generic;
using UnityEngine;

public class ArenaFightOrder : MonoBehaviour
{
    [SerializeField]
    private bool isArenaFight;
    [SerializeField]
    private int enemyNumber;

    [SerializeField]
    private ArenaEnemyScriptable[] arenaEnemiesScriptables;
    [SerializeField]
    private EnemiesOrderPanel enemiesOrderPanel;
    private ArenaEnemyBehavior enemyBehavior;
    private SceneSwapper sceneSwapper;
    private ArenaFightTrigger trigger;

    private static int maxEnemies;

    private static int currentEnemyNumber = 0;

    public bool areAllEnemiesDefeated;

    private void Awake()
    {
        enemyBehavior = GetComponentInChildren<ArenaEnemyBehavior>();
        sceneSwapper = FindObjectOfType<SceneSwapper>();
        trigger = GetComponentInChildren<ArenaFightTrigger>();
    }
    private void OnValidate()
    {
        //InitializeEnemies();

    }
    void Start()
    {
        currentEnemyNumber = Bank.Instance.playerInfo.currentEnemyNumber;
        InitializeEnemies();
    }

    void InitializeEnemies()
    {
        if (!isArenaFight)
        {
            enemyBehavior.SetArenaEnemy(arenaEnemiesScriptables[enemyNumber]);
            return;
        }
        enemyBehavior.SetArenaEnemy(arenaEnemiesScriptables[currentEnemyNumber]);
        enemiesOrderPanel.InitializeEnemyData(arenaEnemiesScriptables);
        enemiesOrderPanel.InitializeCrossEnemyIcons();
        maxEnemies = arenaEnemiesScriptables.Length;
    }

    //После победы свапать врагов
    public void DeskAnimation()
    {
        enemiesOrderPanel.CrossNewIcon(currentEnemyNumber);
        IncreaseDefeatedEnemiesCount();                   
    }
    public void ChangeEnemies()
    {
        enemyBehavior.SetArenaEnemy(arenaEnemiesScriptables[currentEnemyNumber]);
        if (areAllEnemiesDefeated)
        {
            enemyBehavior.gameObject.SetActive(false);
            trigger.gameObject.SetActive(false);
        }
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
            currentEnemyNumber = maxEnemies - 1;
        }
    }

    public void OpenDoorAnimation()
    {
        sceneSwapper.UnlockLevel();
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
