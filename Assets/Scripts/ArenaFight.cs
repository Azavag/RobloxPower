using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArenaFight : MonoBehaviour
{
    [SerializeField]
    private bool isArenaFight;

    private bool isDead = false;
    private float deathInterval = 60f;
    private float deathTimer = 0f; 
    
    private float saveInterval = 2f;
    private float saveTimer = 0f;


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
    [SerializeField]
    private Button hitButton;

    SoundController soundController;

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

    private float cameraTransitionDuration = 1f;
    private float cameraStartTransitionDuration;

    public static event Action<int> EnemyLost;
    public static event Action<ArenaFight, float> SavedTimer;

    private void OnEnable()
    {
        hitButton.onClick.AddListener(OnClickHitButton);
    }
    private void OnDisable()
    {
        hitButton.onClick.RemoveListener(OnClickHitButton);
    }
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
        soundController =FindObjectOfType<SoundController>();
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
        if(isDead)
            DeathTimer();       
    }

    public void SavesInitializationTimer(float savedTimer)
    {
        currentArenaEnemy.Death();
        currentArenaEnemy.SetFightState(true);
        StartDeathDelay(savedTimer);
    }

    public void StartFight()
    {        
        StartCoroutine(StartFightRoutine());
        CursorLocking.LockCursor(false);
        PlayerController.IsBusy = true;
    }

    IEnumerator StartFightRoutine()
    {
        fadeScreen.StartInFadeScreenTween();
        isFightState = true;
        playerController.BlockPlayersInput(isFightState);
        yield return new WaitForSeconds(fadeScreen.GetInFadeDuration());
        soundController.Play("ReadyFight");
        sensivityControl.ResetCameraPosition();
        canPlayerAttack = true;
        arenaCamera.enabled = true;
        uINavigation.ToggleArenaFightCanvas(true);
        uINavigation.ToggleJoystickCanvas(false);
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
            soundController.Play("Knockout");
            currentArenaEnemy.WinAnimation();
            currentArenaEnemy.CanAttack(false);
            canPlayerAttack = false;
            playerAnimatorController.DeathAnimation(true);
            StartCoroutine(PlayerLoseFight());
        }
        soundController.MakeRandomPunchSound();
    }

    public void OnEnemyLost()
    {
        canPlayerAttack = false;
        playerAnimatorController.WinAnimation();
        StartCoroutine(PlayerWinFight());
        soundController.Play("Knockout");
    }
    public void EndFight()
    {
        isFightState = false;
        arenaCamera.enabled = false;
        trigger.ToggleTriggerFX(true);        
        MovePlayerToExit();
        playerController.SwapToFightMode(isFightState);
        CursorLocking.LockCursor(true);

    }
    IEnumerator PlayerWinFight()
    {
        //Ожидание анимации победы
        yield return new WaitForSeconds(endDelay);
        uINavigation.ToggleArenaFightCanvas(false);
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
           
        EnemyLost?.Invoke(currentArenaEnemy.GetEnemyReward());
        if (isArenaFight)
        {
            currentArenaEnemy.SetFightState(isFightState);
            arenaFightOrder.ChangeEnemies();
        }
        else
            StartDeathDelay(deathInterval);
        EndFight();
        yield return new WaitForSeconds(cinemachineBrain.m_DefaultBlend.m_Time*1.1f);
        //Возвращение управления к игроку   
        cinemachineBrain.m_DefaultBlend.m_Time = cameraStartTransitionDuration;
        playerController.BlockPlayersInput(false);
        uINavigation.ToggleJoystickCanvas(true);
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
  
    IEnumerator PlayerLoseFight()
    {
        yield return new WaitForSeconds(endDelay);
        fadeScreen.StartInFadeScreenTween();
        yield return new WaitForSeconds(fadeScreen.GetInFadeDuration());
        uINavigation.ToggleArenaFightCanvas(false);
        playerAnimatorController.DeathAnimation(false);
        EndFight();
        yield return new WaitForSeconds(fadeScreen.GetOutFadeDuration());
        currentArenaEnemy.SetFightState(isFightState);
        playerController.BlockPlayersInput(false);
        uINavigation.ToggleJoystickCanvas(true);
        PlayerController.IsBusy = false;
    }

    void OnClickHitButton()
    {   //Поменять на нажатие кнопки
        if(canPlayerAttack)
        {
            soundController.MakeRandomPunchSound();
            currentArenaEnemy.GetHit(playerDamage);
            playerAnimatorController.FistAttackAnimation();            
        }
    }
    void MovePlayerToExit()
    {
        playerController.transform.position = exitPoint.position;
        playerController.transform.rotation = exitPoint.rotation;
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

    void StartDeathDelay(float timeInterval)
    {
        isDead = true;
        deathTimer = timeInterval;
        currentArenaEnemy.StartDeathTimer(timeInterval);
        trigger.gameObject.SetActive(false);
    }
    void DeathTimer()
    {
        deathTimer -= Time.deltaTime;
        saveTimer -= Time.deltaTime;
        if(saveTimer <= 0)
        {
            saveTimer = saveInterval;
            SavedTimer?.Invoke(this, deathTimer);
        }
        if (deathTimer <= 0)
        {
            isDead = false;
            trigger.gameObject.SetActive(true);
            deathTimer = 0;
            SavedTimer?.Invoke(this, deathTimer);
        }
    } 

    public float GetCurrentDeathTimer()
    {
        return deathTimer;
    }
}
