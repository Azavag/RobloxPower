using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvAlert : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private UINavigation uiNavigation;
    private AdvManager advManager;
    [Header("UI Elements")]
    [SerializeField]
    private Transform advAlertPanel;
    [SerializeField]
    private TextMeshProUGUI counterText;
    [SerializeField]
    private Image counterCircle;

    int timeCounter = 3;
    float timer;
    bool isTimerGoing;


    private void Awake()
    {
        advManager = GetComponent<AdvManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Y)) 
        {
            ShowAdvAlertPanel();
        }
        if(isTimerGoing)
        {
            timer -= Time.deltaTime;
            UpdateCounterOnPanel();
            if(timer < 0)
            {
                //Реклама
                HideAdvAlertPanel();
                advManager.ShowAdv();
#if UNITY_EDITOR
                advManager.ResetTimer();
#endif
            }
        }
    }

    public void ShowAdvAlertPanel()
    {
        uiNavigation.ToggleAdvAlertCanvas(true);
        advAlertPanel.GetComponent<Animator>().SetTrigger("isShow");
        ResetCounterOnPanel();
        isTimerGoing = true;
    }
    void HideAdvAlertPanel()
    {
        advAlertPanel.GetComponent<Animator>().SetTrigger("isHide");
        uiNavigation.ToggleAdvAlertCanvas(false);        
        isTimerGoing = false;
    }

    void ResetCounterOnPanel()
    {
        timer = timeCounter;
        counterText.text = timeCounter.ToString();
        counterCircle.fillAmount = 1;
    }

    void UpdateCounterOnPanel()
    {
        counterText.text = timer.ToString("0");
        counterCircle.fillAmount = timer / timeCounter;
    }
}
