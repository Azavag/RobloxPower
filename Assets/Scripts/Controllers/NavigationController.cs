//using UnityEngine;

//public class NavigationController : MonoBehaviour
//{
//    [Header("Канвасы")]
//    [SerializeField] GameObject ingameCanvas;
//    [SerializeField] GameObject startCanvas;
//    [SerializeField] GameObject settingsCanvas;
//    [Header("Панели в главном меню")]
//    [SerializeField] GameObject mainPanel;
//    [SerializeField] GameObject gamemmodesPanel;
//    [Header("Панели в игре")]
//    [SerializeField] GameObject pauseMenu;
//    [SerializeField] GameObject deathMenu;
//    [SerializeField] GameObject endGamePanel;
//    [Header("Объекты на сцене")]
//    [SerializeField] Camera mainCamera;
//    [SerializeField] GameObject playerObject;
//    [SerializeField] GameObject pauseButton;
//    [Header("Контроллеры")]
//    [SerializeField] SpawnManager spawnManager;
//    //[SerializeField] AdvManager advManager;
//    //[SerializeField] InputGame inputGame;
//    //[SerializeField] SoundController soundController;
//    //[SerializeField] SwapCharacterModel swapCharacter;
//    //[SerializeField] SpeedRunLevelController speedRunLevelController;
//    //ChoosingGamemode choosingGamemode;
//    //SceneLoadingAnimator levelLoadAnimator;
//    bool isPause;
//    bool isSettings;
//    bool isGame;

//    private void Awake()
//    {
//        soundController = FindObjectOfType<SoundController>();
//        settingsCanvas = soundController.transform.GetChild(0).gameObject;
//        choosingGamemode = GetComponent<ChoosingGamemode>();
//        levelLoadAnimator = FindObjectOfType<SceneLoadingAnimator>();
//    }
//    void Start()
//    {
//        startCanvas.SetActive(true);
//        ingameCanvas.SetActive(false);
//        settingsCanvas.SetActive(false);
//        EnableCharacterControl(isGame);
//        deathMenu.SetActive(isGame);
//        pauseMenu.SetActive(isPause);
//        pauseButton.SetActive(!isPause);
//        endGamePanel.SetActive(false);
//        gamemmodesPanel.SetActive(false);
//    }

    
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Tab) && isGame)
//        {
//            ShowPauseMenu();
//        }
//    }

//    public void StartNormalGamemode()
//    {
//        ToggleMenu_Ingame();
//        spawnManager.SetGamemodeType(Gamemode.normal);
//        speedRunLevelController.SetupSpeedRun(false);
//        spawnManager.ResetSpeedrunSpawnpoints();       
//        spawnManager.TransferPlayer();
//    }
//    public void StartSpeedrunGamemode()
//    {
//        ToggleMenu_Ingame();
//        spawnManager.SetGamemodeType(Gamemode.speedrun);
//        spawnManager.ResetLastSpawnPoint();
//        spawnManager.ResetSpeedrunSpawnpoints();
//        speedRunLevelController.SetupSpeedRun(true);
//        spawnManager.TransferPlayer();
//    }

//    public void ToggleMenu_Ingame()
//    {
//        isGame = !isGame;
//        Time.timeScale = 1;
//        if (!isGame)
//            speedRunLevelController.CancelSpeedRun();
//        inputGame.ShowCursorState(!isGame);
//        swapCharacter.MakeCurrentCharacterModelActive();  
//        EnableCharacterControl(isGame);
//        isPause = false;
//        pauseMenu.SetActive(isPause);
//        pauseButton.SetActive(isGame);
//        deathMenu.SetActive(false);
//        startCanvas.SetActive(!isGame);
//        ingameCanvas.SetActive(isGame);
//        soundController.MakeClickSound();
//    }  
//    public void SwapChooseGamemodeToMainPanel()
//    {
//        soundController.MakeClickSound();
//        gamemmodesPanel.SetActive(false);
//        mainPanel.SetActive(true);
//    }
    
//    public void SwapMainToChooseGamemode()
//    {
//        soundController.MakeClickSound();
//        mainPanel.SetActive(false);
//        gamemmodesPanel.SetActive(true);
//        choosingGamemode.ChangeStartGameText();
//        choosingGamemode.ChangeBestTimeText();
//    }
//    public void ShowPauseMenu()
//    {
//        if (deathMenu.activeSelf)
//        {
//            return;
//        }
//        soundController.MakeClickSound();
//        isPause = !isPause;       
//        Time.timeScale = isPause ? 0 : 1;
//        pauseMenu.SetActive(isPause);
//        pauseButton.SetActive(!isPause);
//        speedRunLevelController.ToggleSpeedRunTimer(isPause);
//        EnableCharacterControl(!isPause);
//        inputGame.ShowCursorState(isPause); 
//    }


//    public void EnableCharacterControl(bool state)
//    {
//        mainCamera.GetComponent<OrbitingCamera>().enabled = state;
//        playerObject.GetComponent<SimpleCharacterController>().enabled = state;
//    }

//    public void ShowCharactersSwapScene()
//    {
//        soundController.MakeClickSound();
//        levelLoadAnimator.LoadNewScene("SwapHeroesScene");
//    }

//    public void ShowSettingMenu()
//    {
//        soundController.MakeClickSound();
//        isSettings = !isSettings;
//        settingsCanvas.SetActive(isSettings);      
//    }

//    public void ShowEndGamePanel(bool state)
//    {
        
//        endGamePanel.SetActive(state);
//        EnableCharacterControl(!state);
//        inputGame.ShowCursorState(state);
//    }
//}
