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

    [Header("ClickTimer")]
    private float clickInterval = 0.6f;
    private int currentStreakMultiply = 1;
    private float timeAfterClick;
    private int clickStreak = 0;
    private int maxClickStreak = 60;
    private int currentNumberOfMultipliers = 2;
    private Dictionary<int, int> clickStreaksMultipliersDict;
    private int currentStreakLineNumber = 0;
    private bool isClickCount;
    private bool isPlayerTrain;

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
        PunchbagsShop.PunchbagSelected += OnPunchbagSelected;

        
    }
    private void OnDisable()
    {
        quitButton.onClick.RemoveListener(RemovePlayerFromTrainZone);
        trainClickButton.onClick.AddListener(OnClickTrainButton);
        PunchbagsShop.PunchbagSelected -= OnPunchbagSelected;
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
        UpdateStreakDictionary();
    }
    void FixedUpdate()
    {
        ClickTimer();
    }
    private void OnPunchbagSelected(int number)
    {
        currentStreakLineNumber = number;
        currentNumberOfMultipliers++;
        maxClickStreak += 10;
        UpdateStreakDictionary();
    }
  
    public void UpdateStreakDictionary()
    {
        clickStreaksMultipliersDict = new Dictionary<int, int>();
        for (int counter = 0; counter < currentNumberOfMultipliers;)
        {
            int stepClickStreakValue = (counter + 1) * (maxClickStreak / (currentNumberOfMultipliers+1));
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
            }          
        }                
    }

    void FillImage()
    {
        float targetValue = 1 - ((float)clickStreak / maxClickStreak);
        fillImage.fillAmount = targetValue;

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
