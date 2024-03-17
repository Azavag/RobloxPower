using EasyUI.Tabs;
using UnityEngine;
using UnityEngine.UI;

public enum SkinType
{
    Hat,
    Pet,
    Trail,
}
public class SkinShop : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private UINavigation uiNavigation;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private SkinShopTrigger trigger;
    [SerializeField]
    private HatSkinButtonsController hatSkinButtonsController;
    [SerializeField]
    private PetSkinButtonsController petSkinButtonsController;
    [SerializeField]
    private TrailSkinButtonsController trailSkinButtonsControllers;
    private SoundController soundController;


    [Header("Shop UI")]
    [SerializeField]
    private Button closeButton;

    int lastOpenedPage;

    private void Awake()
    {
        trigger = GetComponentInChildren<SkinShopTrigger>();
        soundController = FindObjectOfType<SoundController>();
    }
    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseSkinShop);
        TabsUI.TabSwapped += OnTabsChange;

    }
    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(CloseSkinShop);
        TabsUI.TabSwapped -= OnTabsChange;
    }

    public void OpenSkinShop()
    {
        soundController.MakeClickSound();
        PlayerController.IsBusy = true;
        CursorLocking.LockCursor(false);
        playerController.BlockPlayersInput(true);
        uiNavigation.ToggleSkinShopCanvas(true);
        uiNavigation.ToggleJoystickCanvas(false);
        ResetPages();
    }

    public void CloseSkinShop()
    {
        soundController.MakeClickSound();
        PlayerController.IsBusy = false;
        trigger.ToggleSkinShopView(false);
        CursorLocking.LockCursor(true);
        playerController.BlockPlayersInput(false);
        ResetPages();
        uiNavigation.ToggleSkinShopCanvas(false);
        uiNavigation.ToggleJoystickCanvas(true);
    }

    public void ResetPages()
    {   
        switch (lastOpenedPage)
        { 
            case 0:
                ResetHatSkinAndStats();
                break;
            case 1:
                ResetPetSkinAndStats();
                break;
            case 2:
                ResetTrailSkinAndStats();
                break;
        }

    }
    //На какую вкладку переключились
    void OnTabsChange(int index)
    {
        soundController.MakeClickSound();
        ResetPages();
        switch (index)
        {
            case 0:
                lastOpenedPage = 0;
                break;
            case 1:
                lastOpenedPage = 1;
                break;
            case 2:
                lastOpenedPage = 2;
                break;
        }
        
    }

    void ResetPetSkinAndStats()
    {
        petSkinButtonsController.ResetSkin();
        petSkinButtonsController.ShowCurrentSkinStats();
    }
    void ResetHatSkinAndStats()
    {
        hatSkinButtonsController.ResetSkin();
        hatSkinButtonsController.ShowCurrentSkinStats();
    }

    void ResetTrailSkinAndStats()
    {
        trailSkinButtonsControllers.ResetSkin();
        trailSkinButtonsControllers.ShowCurrentSkinStats();
    }
}
