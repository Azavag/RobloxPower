using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class ArenaFight : MonoBehaviour
{
    [SerializeField]
    private bool isArenaFight;

    private bool isDead = false;
    private float deathInterval = 60f;
    private float deathTimer = 0f;

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
    private CinemachineBrain cinemachineBrain;
    private CameraSensivityControl sensivityControl;

    public static bool isFightState;   
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

    private float cameraTransitionDuration = 1f;
    private float cameraStartTransitionDuration;

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
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        sensivityControl = FindObjectOfType<CameraSensivityControl>();
        cameraStartTransitionDuration = cinemachineBrain.m_DefaultBlend.m_Time;
    }
    void Start()
    {
        SetupShakeCameraTween();
        arenaCamera.enabled = false;
        fightDeskCamera.enabled = false;
        doorCamera.enabled = false;

        isFightState = false;       
    }

    void Update()
    {
        if (isFightState)
            CheckClick();

        if(isDead)
            DeathTimer();
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
        sensivityControl.ResetCameraPosition();
        canPlayerAttack = true;
        arenaCamera.enabled = true;
        uINavigation.ToggleArenaFightCanvas(true);
        currentArenaEnemy.SetFightState(isFightState);
        currentArenaEnemy.CanAttack(true);
        ResetPlayerStats();
        healthBars.ResetFightHealthBar();
        playerController.SwapToFightMode(true);       
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
    public void EndFight()
    {
        isFightState = false;
        arenaCamera.enabled = false;
        trigger.ToggleTriggerFX(true);
        if (isArenaFight)
            currentArenaEnemy.SetFightState(isFightState);
        else
            StartDeathDelay();
        MovePlayerToExit();
        playerController.SwapToFightMode(isFightState);
        uINavigation.ToggleArenaFightCanvas(false);
        cinemachineBrain.m_DefaultBlend.m_Time = cameraStartTransitionDuration;
    }
    IEnumerator PlayerWinFight()
    {
        //Ожидание анимации победы
        yield return new WaitForSeconds(endDelay);
        if (isArenaFight && !arenaFightOrder.areAllEnemiesDefeated)
        {
            yield return StartCoroutine(ProgressCinematic());
            cinemachineBrain.m_DefaultBlend.m_Time = cameraTransitionDuration * 2f;
        }
        else
        {
            fadeScreen.StartInFadeScreenTween();
            yield return new WaitForSeconds(fadeScreen.GetInFadeDuration());
            cinemachineBrain.m_DefaultBlend.m_Time = cameraStartTransitionDuration;
        }
        //Возвращение управления к игроку      
        EnemyLost?.Invoke(currentArenaEnemy.GetEnemyReward());
        if(isArenaFight)
            arenaFightOrder.ChangeEnemies();
        EndFight();
        yield return new WaitForSeconds(cinemachineBrain.m_DefaultBlend.m_Time*1.1f);
        playerController.BlockPlayersInput(false);
        PlayerController.IsBusy = false;       
    }

    IEnumerator  ProgressCinematic()
    {
        //Переход к стенду     
        cinemachineBrain.m_DefaultBlend.m_Time = cameraTransitionDuration;
        fightDeskCamera.enabled = true;
        yield return new WaitForSeconds(cameraTransitionDuration);
        //Анимация обновления 
        arenaFightOrder.DeskAnimation();
        yield return new WaitForSeconds(enemiesOrderPanelAnimDuration * 1.25f);
        fightDeskCamera.enabled = false;
        if (arenaFightOrder.areAllEnemiesDefeated)
        {
            yield return StartCoroutine(DoorOpenCinematic());
        }
        
    }
    IEnumerator DoorOpenCinematic()
    {
        doorCamera.enabled = true;
        yield return new WaitForSeconds(cameraStartTransitionDuration);
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
        playerAnimatorController.DeathAnimation(false);
        EndFight();
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

    void StartDeathDelay()
    {
        isDead = true;
        ResetDeathTimer();
        currentArenaEnemy.StartDeathTimer(deathInterval);
        trigger.gameObject.SetActive(false);
    }
    void DeathTimer()
    {
        deathTimer -= Time.deltaTime;
        if(deathTimer <= 0)
        {
            isDead = false;
            trigger.gameObject.SetActive(true);
        }
    }

    void ResetDeathTimer()
    {
        deathTimer = deathInterval;
    }
}
