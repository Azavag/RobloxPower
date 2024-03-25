using ECM.Controllers;
using System;
using UnityEngine;

public class PowerControl : MonoBehaviour
{
    [Header("POWER")]
    [SerializeField]
    private bool isPowerIncrease;

    private float powerIncreaseTimer = 0f;
    private float powerIncreaseInterval = 1f;

    [SerializeField]
    private BaseCharacterController characterController;
    [SerializeField]
    private PlayerAnimatorController playerAnimatorController;

    int upgradesPassivePowerIncrease;
    int upgradesActivePowerIncrease;
    int skinsPassivePowerIncrease;
    int skinsActivePowerIncrease;
    int passivePowerIncrease;
    int activePowerIncrease;
    int currentPower;

    public static event Action PowerIncreasesChanged;
    public static event Action CurrentPowerChanged;
    [SerializeField]
    private SkinCharacteristics[] skinsCharacteristics;

    float saveTimer;
    float saveInterval = 2;
    private void Awake()
    {
        characterController = FindObjectOfType<BaseCharacterController>();
        playerAnimatorController = FindObjectOfType<PlayerAnimatorController>();
        
    }
    private void OnEnable()
    {
        CourseFinish.CourseFinished += ResetPowerToMinLevel;
        SkinCharacteristics.CharacteristicsChanged += OnSkinsCharacteristicsChanged;
    }
    private void OnDisable()
    {
        CourseFinish.CourseFinished -= ResetPowerToMinLevel;
        SkinCharacteristics.CharacteristicsChanged -= OnSkinsCharacteristicsChanged;
    }

    void Start()
    {
        Initialization();
        ResetTimer();
        saveTimer = saveInterval;
    }
    void Initialization()
    {
        currentPower = Bank.Instance.playerInfo.currentPower;
        skinsPassivePowerIncrease = Bank.Instance.playerInfo.skinsPassivePowerIncrease;
        skinsActivePowerIncrease = Bank.Instance.playerInfo.skinsActivePowerIncrease;
        upgradesPassivePowerIncrease = Bank.Instance.playerInfo.upgradePassivePowerIncrease;   
        upgradesActivePowerIncrease = Bank.Instance.playerInfo.upgradeActivePowerIncrease;

        passivePowerIncrease = upgradesPassivePowerIncrease + skinsPassivePowerIncrease;
        activePowerIncrease = upgradesActivePowerIncrease + skinsActivePowerIncrease;
        ChangePlayerCurrentPower();
        PowerIncreasesChanged?.Invoke();
    }
  
    void Update()
    {
        if (!isPowerIncrease)
            return;
        SaveTimer();
        if (isTimeToIncrease())
        {
            PassiveIncreaseCurrentPower();
            ResetTimer();
        }
    }
    //По ивенту
    void OnSkinsCharacteristicsChanged()
    {
        skinsPassivePowerIncrease = 0;
        skinsActivePowerIncrease = 0;
        foreach (var characteristic in skinsCharacteristics)
        {
            skinsPassivePowerIncrease += characteristic.GetPassiveStats();
            skinsActivePowerIncrease += characteristic.GetActiveStats();
        }

        Bank.Instance.playerInfo.skinsActivePowerIncrease = skinsActivePowerIncrease;
        Bank.Instance.playerInfo.skinsPassivePowerIncrease = skinsPassivePowerIncrease;
        YandexSDK.Save();
        ChangePowerIncreases();
    }

    void PassiveIncreaseCurrentPower()
    {
        currentPower += passivePowerIncrease;
        if (currentPower < 0)
        {
            currentPower = int.MaxValue;
            isPowerIncrease = false;
        }
        //Подумать про максимальное значение
        Bank.Instance.playerInfo.overallPower += passivePowerIncrease;
        ChangePlayerCurrentPower();
    }
    public void ActiveIncreaseCurrentPower(int streakMultiply)
    {
        currentPower += (activePowerIncrease * streakMultiply);
        if (currentPower < 0)
        {
            currentPower = int.MaxValue;
            isPowerIncrease = false;
        }
        Bank.Instance.playerInfo.overallPower += (activePowerIncrease * streakMultiply);
        ChangePlayerCurrentPower();
    }

    public void ChangeUpgradesPassivePowerIncrease(int upgradesDiff)
    {
        upgradesPassivePowerIncrease += (int)upgradesDiff;
        Bank.Instance.playerInfo.upgradePassivePowerIncrease = upgradesPassivePowerIncrease;
        YandexSDK.Save();
        ChangePowerIncreases();
    }
    public void ChangeUpgradesActivePowerIncrease(int upgradesDiff)
    {
        upgradesActivePowerIncrease += upgradesDiff;
        Bank.Instance.playerInfo.upgradeActivePowerIncrease = upgradesActivePowerIncrease;
        YandexSDK.Save();
        ChangePowerIncreases();
    }
    void ChangePowerIncreases()
    {       
        passivePowerIncrease = skinsPassivePowerIncrease + upgradesPassivePowerIncrease;
        activePowerIncrease = skinsActivePowerIncrease + upgradesActivePowerIncrease;
        PowerIncreasesChanged?.Invoke();
    }

    void ChangePlayerCurrentPower()
    {
        Bank.Instance.playerInfo.currentPower = currentPower;
        CurrentPowerChanged?.Invoke();
        YandexSDK.SetNewLeaderboardValue(Bank.Instance.playerInfo.overallPower);
    }

    //void UpdateSpeedAnimation()
    //{
    //    float speedAnimationMultiplier = (float)currentPower / minLevelJump / 5;
    //    speedAnimationMultiplier = Mathf.Clamp(speedAnimationMultiplier, 1f, 3f);
    //    playerAnimatorController.SetSpeedMultiplier(speedAnimationMultiplier);
    //}
    public void ResetPowerToMinLevel(int diff)
    {
        //currentPower = minLevelJump;
        ChangePlayerCurrentPower();
    }

    bool isTimeToIncrease()
    {
        powerIncreaseTimer -= Time.deltaTime;
        return powerIncreaseTimer <= 0;
    }
    void ResetTimer()
    {
        powerIncreaseTimer = powerIncreaseInterval;
    }

    void SaveTimer()
    {
        saveTimer -= Time.deltaTime;
        if (saveTimer <= 0)
        {
            YandexSDK.Save();
            saveTimer = saveInterval;
        }
    }
    public int GetPlayerHealth()
    {
        return currentPower;
    }
    public int GetPlayerDamage()
    {
        return Mathf.Max(currentPower / 21, 1);
    }
    public int GetLevelKoeficient()
    {
        return 1;
    }


}
