using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BotsCanvases : MonoBehaviour
{
    [SerializeField]
    private BotController[] bots;
    private Canvas[] botsCanvases;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private List<string> rusNames;
    [SerializeField]
    private List<string> engNames;
    private List<string> names;

    private void Start()
    {
        botsCanvases = new Canvas[bots.Length];

        if (Language.Instance.languageName == LanguageName.Rus)
            names = new List<string>(rusNames);
        else names = new List<string>(engNames);
        int counter = 0;
        foreach (var bot in bots)
        {
            botsCanvases[counter] = bot.GetComponentInChildren<Canvas>();
            botsCanvases[counter].transform.GetComponentInChildren<TextMeshProUGUI>().text = RandomName();
            counter++;
        }

    }
    private void LateUpdate()
    {
        RotateCanvas();
    }

    void RotateCanvas()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        foreach (var canvas in botsCanvases)
        {
            canvas.transform.LookAt(cameraPosition);
            canvas.transform.Rotate(0, 180, 0);
        }
    }

    string RandomName()
    {
        int randomNumber = Random.Range(0, names.Count);
        string name = names[randomNumber];
        names.RemoveAt(randomNumber);
        return name;
    }
}
