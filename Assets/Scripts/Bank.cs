using System;
using UnityEngine;

[Serializable]
public class PlayerInfo
{
    public const int HatCount = 90;
    public const int HairCount = 62;
    public const int HairColorCount = 42;
    public const int AccessoriesCount = 19;
    public const int PetCount = 43;
    public const int ShirtCount = 42;
    public const int GlovesCount = 42;
    public const int BagsCount = 25;
    public const int PantsCount = 42;
    public const int TrailCount = 20;
    public const int LevelsCount = 5;
    public const int LevelEnemyTimersCount = 3;

    public float musicVolume = 0.5f;
    public float effectsVolume = 0.5f;
    public float sensivityValue = 1f;

    public int currentPower = 1;
    public int upgradePassivePowerIncrease = 1;
    public int upgradeActivePowerIncrease = 1;
    public int skinsPassivePowerIncrease = 0;
    public int skinsActivePowerIncrease = 0;
    public int coins = 0;

    public int overallPower = 0;

    public int selectedHatId = 0;
    public int selectedPetId = 0;
    public int selectedTrailId = 0;
    public int selectedAccessoiresId = 0;
    public int selectedShirtId = 0;
    public int selectedPantsId = 0;
    public int selectedGlovesId = 0;
    public int selectedBagsId = 0;
    public int selectedHairId = 0;
    public int selectedHairColorId = 0;

    public int currentEnemyNumber = 0;
    public int currentPunchBagNumber = 0;

    public int currentLevelNumber = 1;

    public bool[] hatSkinsBuyStates = new bool[HatCount];
    public bool[] hairSkinsBuyStates = new bool[HairCount];
    public bool[] hairColorsBuyStates = new bool[HairColorCount];
    public bool[] accessoriesSkinsBuyStates = new bool[AccessoriesCount];
    public bool[] petSkinsBuyStates = new bool[PetCount];
    public bool[] shirtsSkinsBuyStates = new bool[ShirtCount];
    public bool[] glovesSkinsBuyStates = new bool[GlovesCount];
    public bool[] bagsSkinsBuyStates = new bool[BagsCount];
    public bool[] pantsSkinsBuyStates = new bool[PantsCount];
    public bool[] trailSkinsBuyStates = new bool[TrailCount];
    public bool[] areLevelsUnlock = new bool[LevelsCount];

    public float[] levelEnemiesTimers = new float[LevelEnemyTimersCount];

    public static PlayerInfo CreateDefault()
    {
        var info = new PlayerInfo();
        info.EnsureIntegrity();
        info.EnsureStarterUnlocks();
        return info;
    }

    /// <summary>
    /// 
    /// </summary>
    public void EnsureIntegrity()
    {
        hatSkinsBuyStates = EnsureBoolArray(hatSkinsBuyStates, HatCount);
        hairSkinsBuyStates = EnsureBoolArray(hairSkinsBuyStates, HairCount);
        hairColorsBuyStates = EnsureBoolArray(hairColorsBuyStates, HairColorCount);
        accessoriesSkinsBuyStates = EnsureBoolArray(accessoriesSkinsBuyStates, AccessoriesCount);
        petSkinsBuyStates = EnsureBoolArray(petSkinsBuyStates, PetCount);
        shirtsSkinsBuyStates = EnsureBoolArray(shirtsSkinsBuyStates, ShirtCount);
        glovesSkinsBuyStates = EnsureBoolArray(glovesSkinsBuyStates, GlovesCount);
        bagsSkinsBuyStates = EnsureBoolArray(bagsSkinsBuyStates, BagsCount);
        pantsSkinsBuyStates = EnsureBoolArray(pantsSkinsBuyStates, PantsCount);
        trailSkinsBuyStates = EnsureBoolArray(trailSkinsBuyStates, TrailCount);
        areLevelsUnlock = EnsureBoolArray(areLevelsUnlock, LevelsCount);
        levelEnemiesTimers = EnsureFloatArray(levelEnemiesTimers, LevelEnemyTimersCount);

        if (sensivityValue < 0.01f)
            sensivityValue = 1f;
        if (currentPower < 1)
            currentPower = 1;
        if (currentLevelNumber < 1)
            currentLevelNumber = 1;
        if (upgradePassivePowerIncrease < 0)
            upgradePassivePowerIncrease = 0;
        if (upgradeActivePowerIncrease < 1)
            upgradeActivePowerIncrease = 1;
    }

    /// <summary>
    /// ??????????? ???????????????? ????????? ???? [0] ??? ???? ?????????.
    /// ?????????? true, ???? ???-?? ?????????? ? ????? ?????????.
    /// </summary>
    public bool EnsureStarterUnlocks()
    {
        bool changed = false;
        changed |= UnlockIndex0(hatSkinsBuyStates);
        changed |= UnlockIndex0(hairSkinsBuyStates);
        changed |= UnlockIndex0(hairColorsBuyStates);
        changed |= UnlockIndex0(accessoriesSkinsBuyStates);
        changed |= UnlockIndex0(petSkinsBuyStates);
        changed |= UnlockIndex0(shirtsSkinsBuyStates);
        changed |= UnlockIndex0(glovesSkinsBuyStates);
        changed |= UnlockIndex0(bagsSkinsBuyStates);
        changed |= UnlockIndex0(pantsSkinsBuyStates);
        changed |= UnlockIndex0(trailSkinsBuyStates);
        changed |= UnlockIndex0(areLevelsUnlock);
        return changed;
    }

    static bool UnlockIndex0(bool[] states)
    {
        if (states == null || states.Length == 0)
            return false;
        if (states[0])
            return false;
        states[0] = true;
        return true;
    }

    static bool[] EnsureBoolArray(bool[] source, int length)
    {
        if (source != null && source.Length == length)
            return source;

        var result = new bool[length];
        if (source != null)
            Array.Copy(source, result, Math.Min(source.Length, length));
        return result;
    }

    static float[] EnsureFloatArray(float[] source, int length)
    {
        if (source != null && source.Length == length)
            return source;

        var result = new float[length];
        if (source != null)
            Array.Copy(source, result, Math.Min(source.Length, length));
        return result;
    }
}

public class Bank : MonoBehaviour
{
    public static Bank Instance { get; private set; }

    public PlayerInfo playerInfo;
    public bool IsReady { get; private set; }

    public event Action OnDataReady;

    YandexSDK yandexSDK;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (playerInfo == null)
            playerInfo = PlayerInfo.CreateDefault();
        else
            playerInfo.EnsureIntegrity();

        yandexSDK = FindObjectOfType<YandexSDK>();
        if (yandexSDK == null)
        {
            Debug.LogError("Bank: YandexSDK not found.");
            FinalizeLoadedData(cloudWasEmpty: true);
            return;
        }

        yandexSDK.Load();
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    /// <summary>
    /// ?????????? ?? YandexSDK ????? ?????? ?????? (??? ????? ? ?????????).
    /// </summary>
    public void FinalizeLoadedData(bool cloudWasEmpty)
    {
        if (playerInfo == null)
            playerInfo = PlayerInfo.CreateDefault();

        playerInfo.EnsureIntegrity();
        bool startersChanged = playerInfo.EnsureStarterUnlocks();

        IsReady = true;
        YandexSDK.dataIsLoaded = true;

        // ?????? ???????? ???? ??? ????????????? ????????? ????? ù ????? ????? ? ??????.
        if (cloudWasEmpty || startersChanged)
            Save();

        OnDataReady?.Invoke();
    }

    public void Save()
    {
        if (playerInfo == null)
            return;

        YandexSDK.Save();
    }
}
