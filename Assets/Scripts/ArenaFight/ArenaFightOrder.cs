using System;
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

    private static int maxEnemies = 5;

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
        
        enemiesOrderPanel.InitializeEnemyData(arenaEnemiesScriptables);
        enemiesOrderPanel.InitializeCrossEnemyIcons();
        maxEnemies = arenaEnemiesScriptables.Length;
        if (currentEnemyNumber >= maxEnemies)
        {
            areAllEnemiesDefeated = true;
        }
        ChangeEnemies();
    }

    //После победы свапать врагов
    public void DeskAnimation()
    {
        enemiesOrderPanel.CrossNewIcon(currentEnemyNumber);
        IncreaseDefeatedEnemiesCount();                   
    }
    public void ChangeEnemies()
    {
        if (areAllEnemiesDefeated)
        {
            enemyBehavior.gameObject.SetActive(false);
            trigger.gameObject.SetActive(false);
            return;
        }
        enemyBehavior.SetArenaEnemy(arenaEnemiesScriptables[currentEnemyNumber]);
    }
    //После победы
    public void IncreaseDefeatedEnemiesCount()
    {
        if (areAllEnemiesDefeated)
            return;
        currentEnemyNumber++;
        Bank.Instance.playerInfo.currentEnemyNumber = currentEnemyNumber;
        YandexSDK.Save();
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
