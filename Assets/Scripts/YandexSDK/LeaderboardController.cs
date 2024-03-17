using TMPro;
using UnityEngine;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] GameObject[] otherPlayersEntryName;
    [SerializeField] GameObject[] otherPlayersEntryScore;
    private YandexSDK yandexSDK;
    GameObject alertAuth;
    GameObject leaderboardObject; 
    string unknownUserText;

    private void Awake()
    {
        yandexSDK = FindObjectOfType<YandexSDK>();
    }
    void Start()
    {      
        RecieveLeaderBoard();
    }
    //для получения данных, только этот метод
    //По кнопке открытия лидерборда
    public void RecieveLeaderBoard()
    {
        yandexSDK.SetLeaderboardObject(this);
        yandexSDK.GetLeaderboardEntries();
    }

    public void FillLeaderboardData(string jsonData)
    {
        Debug.Log("FillLeaderboardData");
        var json = JSON.Parse(jsonData);
        var userRank = json["userRank"].ToString();
        //Если userScore = 0, То выводить -
        if (userRank == "0")
            userRank = "-";
        var count = Math.Min((int)json["entries"].Count, otherPlayersEntryName.Length);

        for (int i = 0; i < count; i++)
        {
            var score = json["entries"][i]["score"].ToString();
            var name = json["entries"][i]["player"]["publicName"];
            string strName = name.ToString();
            if (string.IsNullOrEmpty(strName))
                strName = unknownUserText;
            strName = strName.Trim(new char[] { '\"', '\'' });

            for (int index = 0; index < strName.Length; index++)
            {
                if (strName[index] == ' ')
                {
                    strName = strName.Substring(0, index + 2) + ".";
                    break;
                }
            }

            otherPlayersEntryName[i].GetComponent<TextMeshProUGUI>().text = strName;        
            otherPlayersEntryScore[i].GetComponent<TextMeshProUGUI>().text = score;
        }
    }

    public void Launch()
    {
        //loadingPanel.SetActive(true);
        //allEntries.SetActive(false);
        //alertAuth.SetActive(true);
    }


    //
    public void OpenAuthAlert()
    {
        //allEntries.SetActive(false);
        alertAuth.SetActive(true);
    }
    //
    public void OpenEntries()
    {
       // alertAuth.SetActive(false);
        //allEntries.SetActive(true);
        FillLeaderboardData(yandexSDK.GetJSONEntries());
    }
    //По кнопке авторизации
    //public void MakeAuth()
    //{
    //    YandexSDK.OpenAuthorization();
    //}

    //В jslib после нажатия на кнопку авторизации
    public void CloseAuthWindow()
    {
        leaderboardObject.SetActive(false);
    }
}

