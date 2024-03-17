using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class UINavigation : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private Canvas trainCanvas;
    [SerializeField]
    private Canvas fadeScreenCanvas;
    [SerializeField]
    private Canvas upgradesShopCanvas;
    [SerializeField]
    private Canvas skinShopCanvas;
    [SerializeField]
    private Canvas openLevelCanvas;
    [SerializeField]
    private Canvas advAlertCanvas;
    [SerializeField]
    private Canvas settingsCanvas;
    [SerializeField]
    private Canvas joystickCanvas;
    [SerializeField]
    private Canvas courseProgressCanvas;
    [SerializeField]
    private Canvas upgradePunchbagsCanvas;
    [SerializeField]
    private Canvas arenaFightCanvas;
    [Header("UI elements")]
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button closeSettingsButton;

    PlayerController playerController;
    SoundController soundController;
    bool isSettingOpen;
    private void OnEnable()
    {
        settingsButton.onClick.AddListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);
    }
    private void OnDisable()
    {
        settingsButton.onClick.RemoveListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isSettingOpen)
            {
                OpenSettings();
                return;
            }
            CloseSettings();
        }
    }
    private void Awake()
    {
        Initialize();
        playerController = FindObjectOfType<PlayerController>();
        soundController = FindObjectOfType<SoundController>();
    }

    void Initialize()
    {
        ToggleCanvas(mainCanvas, true);
        ToggleCanvas(trainCanvas, false);
        ToggleCanvas(fadeScreenCanvas, false);
        ToggleCanvas(upgradesShopCanvas, false);
        ToggleCanvas(skinShopCanvas, false);
        ToggleCanvas(openLevelCanvas, false);
        ToggleCanvas(joystickCanvas, false);
        ToggleCanvas(courseProgressCanvas, true);
        ToggleAdvAlertCanvas(false);
        ToggleSettingsCanvas(false);
        ToggleJoystickCanvas(true);
        ToggleArenaFightCanvas(false);
        ToggleUpgradesPunchbagsCanvas(false);
        CursorLocking.LockCursor(true);
    }

    public void ToggleTrainCanvas(bool state)
    {
        ToggleCanvas(trainCanvas, state);
    }
    public void ToggleFadeScreenCanvas(bool state)
    {
        ToggleCanvas(fadeScreenCanvas, state);
    }
    public void ToggleShopUpgradeCanvas(bool state)
    {
        ToggleCanvas(upgradesShopCanvas, state);
    }
    public void ToggleSkinShopCanvas(bool state)
    {
        ToggleCanvas(skinShopCanvas, state);
    }    
    public void ToggleOpenLevelCanvas(bool state)
    {
        ToggleCanvas(openLevelCanvas, state);
    }
    public void ToggleAdvAlertCanvas(bool state)
    {
        ToggleCanvas(advAlertCanvas, state);
    }
    public void ToggleSettingsCanvas(bool state)
    {
        ToggleCanvas(settingsCanvas, state);
    }
    public void ToggleJoystickCanvas(bool state)
    {
        if(IsMobileController.IsMobile)
            ToggleCanvas(joystickCanvas, state);        
    }
    public void ToggleCourseProgressCanvas(bool state)
    {
        if (state)
            courseProgressCanvas.GetComponent<CourseProgressAnimation>().ShowProgressCourse();
        else courseProgressCanvas.GetComponent<CourseProgressAnimation>().HideProgressCourse();
    }
    public void ToggleUpgradesPunchbagsCanvas(bool state)
    {
        ToggleCanvas(upgradePunchbagsCanvas, state);
    }
    public void ToggleArenaFightCanvas(bool state)
    {
        ToggleCanvas(arenaFightCanvas, state);
    }
    
    void OpenSettings()
    {
        if (AdvZoneCheck.notAdvZone || AdvManager.isAdvOpen)
            return;
        isSettingOpen = true;
        soundController.MakeClickSound();
        ToggleSettingsCanvas(true);
        if (PlayerController.IsBusy)
            return;
        CursorLocking.LockCursor(false);
        playerController.BlockPlayersInput(true);
    }

    void CloseSettings()
    {
        soundController.MakeClickSound();
        ToggleSettingsCanvas(false);
        isSettingOpen = false;
        SoundController.SaveVolumeSetting();
        if (PlayerController.IsBusy)
            return;
        CursorLocking.LockCursor(true);
        playerController.BlockPlayersInput(false);
    }
    void ToggleCanvas(Canvas canvas, bool state)
    {
        canvas.gameObject.SetActive(state);
    }



}
