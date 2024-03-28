using UnityEngine;

public class AdvManager : MonoBehaviour
{
    private float advTimer;
    [SerializeField]
    float advBreak = 600f;
    AdvAlert advAlert;
    bool isCounterToAdv;

    PlayerController playerController;

    public static bool isAdvOpen;
    private void Awake()
    {
        transform.SetParent(null);
        advAlert = GetComponent<AdvAlert>();
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Start()
    {
        ResetTimer();
#if !UNITY_EDITOR
        AdvPauseGame(true);
        ShowAdv();
#endif
    }
    private void Update()
    {
        advTimer -= Time.deltaTime;
        if(advTimer <= 0 && !isCounterToAdv && !PlayerController.IsBusy)
        {
            isCounterToAdv = true;
            AdvPauseGame(true);
            advAlert.ShowAdvAlertPanel();
        }
    }
    public void ShowAdv()
    {        
        YandexSDK.ShowADV();
    }

    public void ShowRewardedAdv()
    {
        AdvPauseGame(true);
        YandexSDK.ShowRewardedADV();
    }
    public void AdvPauseGame(bool pause)
    {
        isAdvOpen = pause;
        playerController.BlockPlayersInput(pause);
        CursorLocking.LockCursor(!pause);
    }
    //Â Jslib
    public void AdvContinueGame()
    {
        isAdvOpen = false;
        if (PlayerController.IsBusy || UINavigation.isSettingsOpen)
            return;
        playerController.BlockPlayersInput(false);
        CursorLocking.LockCursor(true);
    }


    public void ResetTimer()
    {
        isAdvOpen = false;
        advTimer = advBreak;
        isCounterToAdv = false;
    }
}

public static class AdvZoneCheck
{
    public static bool notAdvZone;
}

