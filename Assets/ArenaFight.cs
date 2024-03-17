using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class ArenaFight : MonoBehaviour
{
    [SerializeField]
    private UINavigation uINavigation;
    [SerializeField]
    private CinemachineVirtualCamera arenaCamera;
    [SerializeField]
    private CinemachineVirtualCamera fightDeskCamera;
    private bool isFightState;

    private ArenaEnemyBehavior currentArenaEnemy;
    [SerializeField]
    private PlayerController playerController;
    private PlayerAnimatorController playerAnimatorController;
    [SerializeField]
    private PowerControl powerControl;

    [SerializeField]
    FadeScreen fadeScreen;
    private float endDelay = 3f;

    private int playerMaxHealth;
    private int playerHealth;
    private int playerDamage;
    private bool canPlayerAttack;

    ArenaFightTrigger trigger;
    HealthBars healthBars;
    ArenaFightOrder arenaFightOrder;

    [SerializeField]
    private Transform exitPoint;
    private float enemiesOrderPanelAnimDuration = 2f;

    public static event Action<int> EnemyLost;

    private void OnEnable()
    {
        ArenaEnemyBehavior.EnemyLost += OnEnemyLost;
        ArenaEnemyBehavior.EnemyAttacking += OnEnemyAttacking;
    } 
    private void OnDisable()
    {
        ArenaEnemyBehavior.EnemyLost -= OnEnemyLost;
        ArenaEnemyBehavior.EnemyAttacking -= OnEnemyAttacking;
    }
    private void Awake()
    {
        trigger = GetComponentInChildren<ArenaFightTrigger>();
        healthBars = GetComponentInChildren<HealthBars>();
        playerAnimatorController = playerController.GetComponent<PlayerAnimatorController>();
        currentArenaEnemy = GetComponentInChildren<ArenaEnemyBehavior>();
        arenaFightOrder = GetComponent<ArenaFightOrder>();
    }
    void Start()
    {
        arenaCamera.enabled = false;
        fightDeskCamera.enabled = false;
    }

    void Update()
    {
        if (isFightState)
            CheckClick();
    }

    public void StartFight()
    {
        fadeScreen.StartInFadeScreenTween();
        isFightState = true;
        canPlayerAttack = true;
        arenaCamera.enabled = true;
        uINavigation.ToggleArenaFightCanvas(true);
        currentArenaEnemy.SetFightState(isFightState);
        currentArenaEnemy.CanAttack(true);
        ResetPlayerStats();
        healthBars.ResetFightHealthBar();
        playerController.SwapToFightMode(true);
        playerController.BlockPlayersInput(true);
    }

    public void EndFight()
    {
        isFightState = false;
        fadeScreen.StartInFadeScreenTween();
        arenaCamera.enabled = false;        
        uINavigation.ToggleArenaFightCanvas(false);      
        trigger.ToggleTriggerFX(true);
    }
    private void OnEnemyAttacking(int enemyDamage)
    {
        playerHealth -= enemyDamage;
        
        healthBars.UpdatePlayerHealthBar((float)playerHealth / playerMaxHealth);
        if (playerHealth <= 0)
        {
            currentArenaEnemy.WinAnimation();
            currentArenaEnemy.CanAttack(false);
            canPlayerAttack = false;
            playerAnimatorController.DeathAnimation(true);
            StartCoroutine(PlayerLoseFight());
        }
    }

    void OnEnemyLost()
    {
        canPlayerAttack = false;
        playerAnimatorController.WinAnimation();
        StartCoroutine(PlayerWinFight());
    }

    IEnumerator PlayerWinFight()
    {
        yield return new WaitForSeconds(endDelay);
        //Ожидание анимации победы
        //fadeScreen.StartInFadeScreenTween();
        //yield return new WaitForSeconds(fadeScreen.GetInFadeDuration() + fadeScreen.GetOutFadeDuration());
        //Переход к стенду
        uINavigation.ToggleArenaFightCanvas(false);
        arenaCamera.enabled = false;
        fightDeskCamera.enabled = true;
        yield return new WaitForSeconds(1f);
        //Анимация обновления 
        UpdateEnemiesDesk();
        yield return new WaitForSeconds(enemiesOrderPanelAnimDuration * 1.5f);
        //Возвращение управления к игроку
        fadeScreen.StartInFadeScreenTween();
        yield return new WaitForSeconds(fadeScreen.GetInFadeDuration());       
        fightDeskCamera.enabled = false;
        isFightState = false;        
        trigger.ToggleTriggerFX(true);
        currentArenaEnemy.SetFightState(isFightState);
        EnemyLost?.Invoke(currentArenaEnemy.GetEnemyReward());
        MovePlayerToExit();
        playerController.SwapToFightMode(false);
        yield return new WaitForSeconds(fadeScreen.GetOutFadeDuration());
        playerController.BlockPlayersInput(false);
    }
    void UpdateEnemiesDesk()
    {        
        arenaFightOrder.SwapNextEnemy();
    }
    void MovePlayerToExit()
    {
        playerController.transform.position = exitPoint.position;
        playerController.transform.rotation = exitPoint.rotation;
    }
    IEnumerator PlayerLoseFight()
    {
        yield return new WaitForSeconds(endDelay);
        fadeScreen.StartInFadeScreenTween();
        yield return new WaitForSeconds(fadeScreen.GetInFadeDuration());
        isFightState = false;
        currentArenaEnemy.SetFightState(isFightState);
        playerAnimatorController.DeathAnimation(false);
        uINavigation.ToggleArenaFightCanvas(false);
        trigger.ToggleTriggerFX(true);
        MovePlayerToExit();
        playerController.SwapToFightMode(isFightState);
        arenaCamera.enabled = false;
        yield return new WaitForSeconds(fadeScreen.GetOutFadeDuration());
        playerController.BlockPlayersInput(false);
    }

    void CheckClick()
    {   //Поменять на нажатие кнопки
        if(Input.GetMouseButtonDown(0) && canPlayerAttack)
        {
            //звук
            currentArenaEnemy.GetHit(playerDamage);
            playerAnimatorController.FistAttackAnimation();
            

        }
    }

    void ResetPlayerStats()
    {        
        playerHealth = powerControl.GetPlayerHealth();
        playerDamage = powerControl.GetPlayerDamage();
        Debug.Log("current Hp " + playerHealth);
    }
}
