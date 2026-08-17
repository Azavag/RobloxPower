using TMPro;
using UnityEngine;
using SimpleJSON;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] GameObject[] otherPlayersEntryName;
    [SerializeField] GameObject[] otherPlayersEntryScore;
    private YandexSDK yandexSDK;
    GameObject alertAuth;
    GameObject leaderboardObject;
    string unknownUserText = "Пользователь скрыт";

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
        if (string.IsNullOrEmpty(jsonData) || otherPlayersEntryName == null)
            return;

        var json = JSON.Parse(jsonData);
        if (json == null || json["entries"] == null)
            return;

        ClearEntries();

        var entries = json["entries"];
        for (int i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            int rank = entry["rank"].AsInt;
            if (rank < 1 || rank > otherPlayersEntryName.Length)
                continue;

            int slot = rank - 1;
            int score = entry["score"].AsInt;
            string strName = entry["player"]["publicName"].Value;
            if (string.IsNullOrWhiteSpace(strName) || strName == "null")
                strName = unknownUserText;

            strName = FormatPlayerName(strName);

            SetEntryText(otherPlayersEntryName, slot, strName);
            SetEntryText(otherPlayersEntryScore, slot, score.ToString());
        }
    }

    void ClearEntries()
    {
        if (otherPlayersEntryName != null)
        {
            for (int i = 0; i < otherPlayersEntryName.Length; i++)
                SetEntryText(otherPlayersEntryName, i, string.Empty);
        }

        if (otherPlayersEntryScore != null)
        {
            for (int i = 0; i < otherPlayersEntryScore.Length; i++)
                SetEntryText(otherPlayersEntryScore, i, string.Empty);
        }
    }

    static void SetEntryText(GameObject[] entries, int index, string value)
    {
        if (entries == null || index < 0 || index >= entries.Length || entries[index] == null)
            return;

        var label = entries[index].GetComponent<TextMeshProUGUI>();
        if (label != null)
            label.text = value;
    }

    string FormatPlayerName(string strName)
    {
        strName = strName.Trim(new char[] { '\"', '\'' });
        for (int index = 0; index < strName.Length; index++)
        {
            if (strName[index] == ' ')
            {
                strName = strName.Substring(0, index + 2) + ".";
                break;
            }
        }

        return strName;
    }

    public void Launch()
    {
        //loadingPanel.SetActive(true);
        //allEntries.SetActive(false);
        //alertAuth.SetActive(true);
    }

    public void OpenAuthAlert()
    {
        //allEntries.SetActive(false);
        alertAuth.SetActive(true);
    }

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
