using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class ArenaFight : MonoBehaviour
{
    [SerializeField]
    private bool isArenaFight;

    [Header("Refs")]
    [SerializeField]
    private UINavigation uINavigation;
    [SerializeField]
    private PlayerController playerController;
    private PlayerAnimatorController playerAnimatorController;
    [SerializeField]
    private PowerControl powerControl;
    [SerializeField]
    FadeScreen fadeScreen;
    private ArenaEnemyBehavior currentArenaEnemy;
    ArenaFightTrigger trigger;
    HealthBars healthBars;
    ArenaFightOrder arenaFightOrder;

    [Header("Cameras")]
    [SerializeField]
    private CinemachineVirtualCamera arenaCamera;
    [SerializeField]
    private CinemachineVirtualCamera fightDeskCamera;
    [SerializeField]
    private CinemachineVirtualCamera doorCamera;

    private bool isFightState;   
    private float endDelay = 3f;

    private int playerMaxHealth;
    private int playerHealth;
    private int playerDamage;
    private bool canPlayerAttack;

    [SerializeField]
    private Transform exitPoint;
    private float enemiesOrderPanelAnimDuration = 2f;

    Tween shakeCameraTween;

    public static event Action<int> EnemyLost;

    private void OnDestroy()
    {
        shakeCameraTween.Kill();
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
        SetupShakeCameraTween();
        arenaCamera.enabled = false;
        fightDeskCamera.enabled = false;
        doorCamera.enabled = false;
    }

    void Update()
    {
        if (isFightState)
            CheckClick();
    }

    public void StartFight()
    {
        StartCoroutine(StartFightRoutine());
        PlayerController.IsBusy = true;
    }

    IEnumerator StartFightRoutine()
    {
        fadeScreen.StartInFadeScreenTween();
        isFightState = true;
        playerController.BlockPlayersInput(isFightState);
        yield return new WaitForSeconds(fadeScreen.GetInFadeDuration());
        canPlayerAttack = true;
        arenaCamera.enabled = true;
        uINavigation.ToggleArenaFightCanvas(true);
        currentArenaEnemy.SetFightState(isFightState);
        currentArenaEnemy.CanAttack(true);
        ResetPlayerStats();
        healthBars.ResetFightHealthBar();
        playerController.SwapToFightMode(true);
    }

    public void EndFight()
    {
        isFightState = false;
        fadeScreen.StartInFadeScreenTween();
        arenaCamera.enabled = false;        
        uINavigation.ToggleArenaFightCanvas(false);      
        trigger.ToggleTriggerFX(true);
    }
    public void OnEnemyAttacking(int enemyDamage)
    {
        playerHealth -= enemyDamage;
        shakeCameraTween.Restart();
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

    public void OnEnemyLost()
    {
        canPlayerAttack = false;
        playerAnimatorController.WinAnimation();
        StartCoroutine(PlayerWinFight());
    }

    IEnumerator PlayerWinFight()
    {
        //Ожидание анимации победы
        yield return new WaitForSeconds(endDelay);
        uINavigation.ToggleArenaFightCanvas(false);
        if (isArenaFight && !arenaFightOrder.areAllEnemiesDefeated)
        {
            yield return StartCoroutine(ProgressCinematic());
        }
        //Возвращение управления к игроку
        fadeScreen.StartInFadeScreenTween();
        yield return new WaitForSeconds(fadeScreen.GetInFadeDuration());
        arenaCamera.enabled = false;
        fightDeskCamera.enabled = false;
        isFightState = false;        
        trigger.ToggleTriggerFX(true);
        currentArenaEnemy.SetFightState(isFightState);
        EnemyLost?.Invoke(currentArenaEnemy.GetEnemyReward());
        MovePlayerToExit();
        playerController.SwapToFightMode(false);
        yield return new WaitForSeconds(fadeScreen.GetOutFadeDuration());
        playerController.BlockPlayersInput(false);
        PlayerController.IsBusy = false;
    }

    IEnumerator  ProgressCinematic()
    {
        //Переход к стенду     
        fightDeskCamera.enabled = true;
        yield return new WaitForSeconds(0.75f);
        //Анимация обновления 
        arenaFightOrder.SwapNextEnemy();
        yield return new WaitForSeconds(enemiesOrderPanelAnimDuration * 1.25f);
        if (arenaFightOrder.areAllEnemiesDefeated)
        {
            yield return StartCoroutine(DoorOpenCinematic());
        }
    }
    IEnumerator DoorOpenCinematic()
    {
        doorCamera.enabled = true;
        yield return new WaitForSeconds(0.5f);
        arenaFightOrder.OpenDoorAnimation();
        yield return new WaitForSeconds(1.25f);
        doorCamera.enabled = false;
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
        PlayerController.IsBusy = false;
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
        playerMaxHealth = powerControl.GetPlayerHealth();
        playerHealth = playerMaxHealth;
        playerDamage = powerControl.GetPlayerDamage();
    }

    void SetupShakeCameraTween()
    {
        shakeCameraTween = arenaCamera.transform.DOShakePosition(0.4f, randomnessMode:ShakeRandomnessMode.Harmonic);
    }
}
