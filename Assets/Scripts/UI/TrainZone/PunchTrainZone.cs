using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunchTrainZone : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Button trainClickButton;
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private Image background;
    [SerializeField]
    private Color[] fillColors;

    [Header("ClickTimer")]
    private float clickInterval = 0.6f;
    private int currentStreakMultiply = 1;
    private float timeAfterClick;
    private int clickStreak = 0;
    private int maxClickStreak = 60;
    private int currentNumberOfMultipliers = 0;
    private Dictionary<int, int> clickStreaksMultipliersDict;
    private bool isClickCount;
    private bool isPlayerTrain;

    float targetValue;

    [Header("Refs")]
    [SerializeField]
    private PlayerController playerController;
    private UINavigation uiNavigation;
    private PowerControl powerControl;
    private ClickStreakAnimation clickStreakAnimation;
    private PunchBagFocus punchBagFocus;
    private PunchBagTrigger trigger;
    private SoundController soundController;


    private void OnEnable()
    {
        quitButton.onClick.AddListener(RemovePlayerFromTrainZone);
        trainClickButton.onClick.AddListener(OnClickTrainButton);      
    }
    private void OnDisable()
    {
        quitButton.onClick.RemoveListener(RemovePlayerFromTrainZone);
        trainClickButton.onClick.AddListener(OnClickTrainButton);
    }

    private void Awake()
    {
        clickStreakAnimation = GetComponent<ClickStreakAnimation>();
        uiNavigation = FindObjectOfType<UINavigation>();
        powerControl = FindObjectOfType<PowerControl>();
        soundController = FindObjectOfType<SoundController>();
        punchBagFocus = GetComponentInChildren<PunchBagFocus>();
        trigger = GetComponentInChildren<PunchBagTrigger>();
    }
    private void Start()
    {
        //UpdateStreakDictionary();
    }
    void FixedUpdate()
    {
        ClickTimer();
    }
    public void OnPunchbagSelected(int maxMultiplier, int maxClickStreak)
    {
        currentNumberOfMultipliers = maxMultiplier-1;
        this.maxClickStreak = maxClickStreak;
        UpdateStreakDictionary();
    }
    public void UpdateStreakDictionary()
    {
        clickStreaksMultipliersDict = new Dictionary<int, int>();

        for (int counter = 0; counter < currentNumberOfMultipliers;)
        {
            int stepClickStreakValue = (counter + 1) * (maxClickStreak / (currentNumberOfMultipliers + 1));
            clickStreaksMultipliersDict.Add(stepClickStreakValue, ++counter + 1);
        }
    }
       
    void OnClickTrainButton()
    {
        if (isPlayerTrain)
        {
            powerControl.ActiveIncreaseCurrentPower(currentStreakMultiply);
            isClickCount = true;
            if (timeAfterClick <= clickInterval)
            {
                clickStreak++;
                ClickStreakCheck();
                timeAfterClick = 0;
                FillImage();
                punchBagFocus.OnClickTrainButton();
            }
        }
    }
    void ClickTimer()
    {      
        if(isClickCount)
        {          
            timeAfterClick += Time.deltaTime;
            if (timeAfterClick >= clickInterval)
            {
                timeAfterClick = 0;
                clickStreak = 0;
                currentStreakMultiply = 1;
                isClickCount = false;
                FillImage();
                clickStreakAnimation.ActivateStreakText(currentStreakMultiply);
                ChangeBackgroundColor();
            }          
        }                
    }

    void FillImage()
    {
        targetValue = ((float)clickStreak / maxClickStreak);
        fillImage.fillAmount = targetValue;
       
    }

    void ChangeBackgroundColor()
    {
        fillImage.color = fillColors[currentStreakMultiply-1];
    }

    void ClickStreakCheck()
    {
        int counter = 0;
        foreach(var element in clickStreaksMultipliersDict)
        {
            if(clickStreak == element.Key)
            {
                currentStreakMultiply = element.Value;                
                soundController.Play("FillUp_1");
                clickStreakAnimation.ActivateStreakText(currentStreakMultiply);
                ChangeBackgroundColor();
            }
            counter++;
        }               
    }

    public void FreezePlayerToTrainZone()
    {
        FillImage();
        PlayerController.IsBusy = true; 
        soundController.MakeClickSound();
        playerController.TrainPowerAnimation(true);            
        uiNavigation.ToggleTrainCanvas(true);
        uiNavigation.ToggleJoystickCanvas(false);
        isPlayerTrain = true;
        CursorLocking.LockCursor(false);

        clickStreakAnimation.ActivateStreakText(currentStreakMultiply);
        ChangeBackgroundColor();
    }

    public void RemovePlayerFromTrainZone()
    {
        PlayerController.IsBusy = false;
        soundController.MakeClickSound();
        isPlayerTrain = false;
        uiNavigation.ToggleTrainCanvas(false);
        uiNavigation.ToggleJoystickCanvas(true);
        playerController.TrainPowerAnimation(false);
        punchBagFocus.EndTrain();
        trigger.ResetColor();
        CursorLocking.LockCursor(true);        
    }
}
