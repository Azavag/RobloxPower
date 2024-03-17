using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using SimpleJSON;

public class YandexSDK : MonoBehaviour
{ 
    //Сохранение данных
    [DllImport("__Internal")]
    private static extern void SaveExtern(string date);
    //Загрузка данных
    [DllImport("__Internal")]
    private static extern void LoadExtern();
    //----Таблица лидеров----
    [DllImport("__Internal")]
    //Вывести строку с записями
    private static extern void ShowLeaderBoard();
    //Добавить запись в таблицу
    [DllImport("__Internal")]
    private static extern void SetToLeaderboard(uint value);
    //Межстраничная реклама
    [DllImport("__Internal")]
    private static extern void ShowIntersitialAdvExtern();
    //Реклама с наградой
    [DllImport("__Internal")]
    private static extern void ShowRewardedAdvExtern();
    ////Получение языка
    [DllImport("__Internal")]
    private static extern string GetLang();
    //Получение типа устройства
    [DllImport("__Internal")]
    private static extern string GetDevice();
    //Оценка игры
    //[DllImport("__Internal")]
    //private static extern string RateGameExtern();


    public event Action<string> LeaderBoardReady;
    LeaderboardController leaderboard;
    string jsonEntries;
    bool isDataGetting;
    string deviceType;
    [HideInInspector]
    public static bool dataIsLoaded;
    string domainType;
    private void OnEnable()
    {
        LeaderBoardReady += SetJSONEntries;
    }
    private void OnDisable()
    {
        LeaderBoardReady -= SetJSONEntries;
    }
    private void Awake()
    {    
        transform.SetParent(null);
        DontDestroyOnLoad(this);
    }

    void Start()
    {
              
    }
    //Вызывается месте сохранения Save -> SaveExtern в jslib
    static public void Save()
    {
        string jsonString = JsonUtility.ToJson(Bank.Instance.playerInfo);
#if !UNITY_EDITOR
        SaveExtern(jsonString);
#endif
    }
    //Вызывается в месте загрузки Load -> LoadExtern -> SetPlayerInfo
    public void Load()
    {
#if !UNITY_EDITOR
        Debug.Log("Load");
        LoadExtern();     
#endif
    }
    //Вызывается в jslib
    public void SetPlayerInfo(string value)
    {
        Bank.Instance.playerInfo = JsonUtility.FromJson<PlayerInfo>(value);
        dataIsLoaded = true;
    }

    static public void ShowRewardedADV()
    {

#if !UNITY_EDITOR
        ShowRewardedAdvExtern();
#else
        //Debug.Log("Реклама с наградой");
#endif
    }
    static public void ShowADV()
    {
#if !UNITY_EDITOR
        ShowIntersitialAdvExtern();
#else
       // Debug.Log("Реклама");
#endif
    }
//    static public void OpenAuthorization()
//    {
//#if !UNITY_EDITOR
//        Auth();
//#else
//    Debug.Log("Авторизация");
//#endif
//    }

    public static void SetNewLeaderboardValue(int value)
    {
#if !UNITY_EDITOR
        SetToLeaderboard(value); 
#endif
    }

    //    public void CheckAuthorization()
    //    {
    //#if !UNITY_EDITOR
    //        CheckAuth();
    //#endif
    //    }
    public void SetLeaderboardObject(LeaderboardController leaderboard)
    {
        this.leaderboard = leaderboard;
    }
    //ShowLeaderBoard() -> BoardEntriesReady -> SetJSONEntries
    public void GetLeaderboardEntries()
    {
#if !UNITY_EDITOR
        ShowLeaderBoard();
#endif
    }
    public void BoardEntriesReady(string _data)
    {
        LeaderBoardReady?.Invoke(_data);
    }
    public void SetJSONEntries(string json)
    {
        jsonEntries = json;
        leaderboard.OpenEntries();
    }
    public string GetJSONEntries()
    {
        return jsonEntries;
    }

    //public bool GetDataCheck()
    //{
    //    return isDataGetting;
    //}
    //public void SetDataCheck(bool state)
    //{
    //    isDataGetting = state;
    //}

    public void LoadDeviceInfo()
    {
#if !UNITY_EDITOR
        GetDevice();
#endif
    }
    //Вызывается в jslib
    public void SetDeviceInfo(string deviceString)
    {
        deviceType = deviceString;
    }
    public string GetDeviceType()
    {
        return deviceType;
    }


    //    static public void RateGame()
    //    {
    //#if !UNITY_EDITOR
    //        RateGameExtern();
    //#else
    //        Debug.Log("Оценка игры");
    //#endif

    //    }
}
