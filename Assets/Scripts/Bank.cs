using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public float musicVolume = 0.5f;                //++
    public float effectsVolume = 0.5f;              //++
    public float sensivityValue = 1f;               //++

    public int currentPower = 1;                   //++
    public int upgradePassivePowerIncrease = 1;    //++
    public int upgradeActivePowerIncrease = 1;     //++
    public int skinsPassivePowerIncrease = 0;      //++
    public int skinsActivePowerIncrease = 0;       //++
    public int coins = 0;                           //++
    public int currentPunchBagNumber = 0;
    public int overallPower = 0;                  //++
    //Скины
    public int selectedHatId = 0;                   //++
    public int selectedPetId = 0;                   //++
    public int selectedTrailId = 0;                 //++
    public int currentEnemyNumber = 0;

    public bool[] hatSkinsBuyStates = new bool[90];                //++
    public bool[] petSkinsBuyStates = new bool[43];                //++
    public bool[] trailSkinsBuyStates = new bool[20];              //++
    public bool[] areLevelsUnlock = new bool[5];                  //++
}

public class Bank : MonoBehaviour
{
    public static Bank Instance;
    public PlayerInfo playerInfo; 
    private YandexSDK yandexSDK;

    bool firstLaunch = true;
    private void Awake()
    {
        yandexSDK = FindObjectOfType<YandexSDK>();
       

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            yandexSDK.Load();          
            return;
        }
        Destroy(gameObject);
    }
    private void Update()
    {
        if (!firstLaunch)
            return;

        if (YandexSDK.dataIsLoaded)
        {
            Instance.playerInfo.hatSkinsBuyStates[0] = true;
            Instance.playerInfo.petSkinsBuyStates[0] = true;
            Instance.playerInfo.trailSkinsBuyStates[0] = true;
            Instance.playerInfo.areLevelsUnlock[0] = true;
            firstLaunch = false;
        }
    }
}
