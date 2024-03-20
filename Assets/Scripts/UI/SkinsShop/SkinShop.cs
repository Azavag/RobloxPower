using UnityEngine;
using UnityEngine.UI;

public enum SkinType
{
    Hat,
    Pet,
    Trail,
    Shirt,
    Pants,
    Gloves,
    Hair,
    Accessories,
    Bags,
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
    [SerializeField]
    private ShirtSkinButtonController shirtSkinButtonsController;
    [SerializeField]
    private PantsSkinButtonController pantsSkinButtonsController;
    [SerializeField]
    private GlovesSkinButtonController glovesSkinButtonController;

   private SoundController soundController;


    [Header("Shop UI")]
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject buyWindow;
    [SerializeField]
    private GameObject statWindow;

    int lastOpenedPage = 0;

    private void Awake()
    {
        trigger = GetComponentInChildren<SkinShopTrigger>();
        soundController = FindObjectOfType<SoundController>();
    }
    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseSkinShop);
        ShopMenuNavigation.TabSelected += OnTabSelected;

    }
    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(CloseSkinShop);
        ShopMenuNavigation.TabSelected -= OnTabSelected;
    }

    public void OpenSkinShop()
    {
        soundController.MakeClickSound();
        PlayerController.IsBusy = true;
        CursorLocking.LockCursor(false);
        playerController.BlockPlayersInput(true);
        uiNavigation.ToggleSkinShopCanvas(true);
        uiNavigation.ToggleJoystickCanvas(false);
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
                break;
            case 2:
                break;
            case 3:
                ResetPetSkinAndStats();
                break;
            case 4:
                ResetShirtSkin();
                break;
            case 5:
                ResetGlovesSkin();
                break;
            case 6:
                break;
            case 7:
                ResetPantsSkin();
                break;
            case 8:
                ResetTrailSkinAndStats();
                break;
        }

    }
    //На какую вкладку переключились
    void OnTabSelected(int index)
    {
        soundController.MakeClickSound();

        switch (index)
        { 
            case -1:
                ToggleBuyWindow(false);
                ResetPages();
                return;
            case 0:
            case 3:
            case 8:
                ToggleStatWindow(true);
                break;
            default:
                ToggleStatWindow(false);
                break;
        }
        ToggleBuyWindow(true);
        lastOpenedPage = index;
        ResetPages();
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

    void ResetShirtSkin()
    {
        shirtSkinButtonsController.ResetSkin();
    }
    
    void ResetPantsSkin()
    {
        pantsSkinButtonsController.ResetSkin();
    }

    void ResetGlovesSkin()
    {
        glovesSkinButtonController.ResetSkin();
    }
    void ToggleBuyWindow(bool toggle)
    {
        buyWindow.gameObject.SetActive(toggle);
    }
    void ToggleStatWindow(bool toggle)
    {
        statWindow.gameObject.SetActive(toggle);
    }
}
