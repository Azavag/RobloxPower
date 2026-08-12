using DG.Tweening;
using ECM.Controllers;
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
    HairStyles,
    Accessories,
    Bags,
    HairColors,
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

    private HatSkinButtonsController hatSkinButtonsController;
    private PetSkinButtonsController petSkinButtonsController;
    private TrailSkinButtonsController trailSkinButtonsControllers;
    private ShirtSkinButtonsController shirtSkinButtonsController;
    private PantsSkinButtonsController pantsSkinButtonsController;
    private GlovesSkinButtonsController glovesSkinButtonController;
    private AccessoriesSkinButtonsController accessoriesSkinButtonsController;
    private HairSkinsButtonController hairSkinButtonsController;
    private BagSkinButtonsController bagSkinButtonController;
    private HairColorsSkinButtonsController hairColorsSkinButtonsController;

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

        hatSkinButtonsController = GetComponentInChildren<HatSkinButtonsController>();
        petSkinButtonsController = GetComponentInChildren<PetSkinButtonsController>();
        trailSkinButtonsControllers = GetComponentInChildren<TrailSkinButtonsController>();
        shirtSkinButtonsController = GetComponentInChildren<ShirtSkinButtonsController>();
        pantsSkinButtonsController = GetComponentInChildren<PantsSkinButtonsController>();
        glovesSkinButtonController = GetComponentInChildren<GlovesSkinButtonsController>();
        accessoriesSkinButtonsController = GetComponentInChildren<AccessoriesSkinButtonsController>();
        hairSkinButtonsController = GetComponentInChildren<HairSkinsButtonController>();
        bagSkinButtonController = GetComponentInChildren<BagSkinButtonsController>();
        hairColorsSkinButtonsController = GetComponentInChildren<HairColorsSkinButtonsController>();
        
    }
    private void OnEnable()
    {
        ShopMenuNavigation.TabSelected += OnTabSelected;
        closeButton.onClick.AddListener(CloseSkinShop);
        
    }
    private void OnDisable()
    {
        ShopMenuNavigation.TabSelected -= OnTabSelected;
        closeButton.onClick.RemoveListener(CloseSkinShop);
    }

    public void OpenSkinShop()
    {
        soundController.MakeClickSound();
        PlayerController.IsBusy = true;
        CursorLocking.LockCursor(false);
        playerController.BlockPlayersInput(true);
        YandexSDK.StopGameplayProcess();
        SyncAllUnlockedSkins();
        uiNavigation.ToggleSkinShopCanvas(true);
        uiNavigation.ToggleJoystickCanvas(false);
        
    }

    void SyncAllUnlockedSkins()
    {
        hatSkinButtonsController.SyncUnlockedSkins();
        petSkinButtonsController.SyncUnlockedSkins();
        trailSkinButtonsControllers.SyncUnlockedSkins();
        shirtSkinButtonsController.SyncUnlockedSkins();
        pantsSkinButtonsController.SyncUnlockedSkins();
        glovesSkinButtonController.SyncUnlockedSkins();
        accessoriesSkinButtonsController.SyncUnlockedSkins();
        hairSkinButtonsController.SyncUnlockedSkins();
        bagSkinButtonController.SyncUnlockedSkins();
        hairColorsSkinButtonsController.SyncUnlockedSkins();
    }

    public void CloseSkinShop()
    {
        soundController.MakeClickSound();
        PlayerController.IsBusy = false;
        trigger.ToggleSkinShopView(false);
        CursorLocking.LockCursor(true);
        playerController.BlockPlayersInput(false);
        ResetPages();
        trigger.SetBagsView(false);
        trigger.ResetRotationState();
        uiNavigation.ToggleSkinShopCanvas(false);
        uiNavigation.ToggleJoystickCanvas(true);
        YandexSDK.StartGameplayProcess();
    }

    public void ResetPages()
    {
        switch (lastOpenedPage)
        { 
            case 0:
                ResetHatSkin();
                ResetHairSkin();
                break;
            case 1:
                ResetHairSkin();
                ResetHatSkin();
                break;
            case 2:
                ResetAccessoriesSkin();
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
                trigger.SetBagsView(false);
                ResetBagSkin();
                break;
            case 7:
                ResetPantsSkin();
                break;
            case 8:
                ResetTrailSkinAndStats();
                break;
        }

    }
    //�� ����� ������� �������������
    void OnTabSelected(int index)
    {
        soundController.MakeClickSound();      
        ToggleStatWindow(false);
        
        if (lastOpenedPage == 6 && index != 6 && index >= 0)
            trigger.SetBagsView(false);
        else if (index == 6)
            trigger.SetBagsView(true);

        switch (index)
        {
            //�� �������� ������ ����� �����
            case -2:
                ToggleBuyWindow(false);
                lastOpenedPage = 1;
                return;
            case -1:
                ToggleBuyWindow(false);
                ResetPages();
                return;
            case 0:
                break;
            case 3:
                ResetPetSkinAndStats();
                ToggleStatWindow(true);
                break;
            case 8:
                ToggleStatWindow(true);
                ResetTrailSkinAndStats();
                break;
            default:
                
                break;
        }
        ToggleBuyWindow(true);
        lastOpenedPage = index;
    }

    void ResetPetSkinAndStats()
    {
        petSkinButtonsController.ResetSkin();
        petSkinButtonsController.ShowCurrentSkinStats();
    }
    void ResetHatSkin()
    {
        hatSkinButtonsController.ResetSkin();       
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

    void ResetAccessoriesSkin()
    {
        accessoriesSkinButtonsController.ResetSkin();
    }

    void ResetHairSkin()
    {
        hairSkinButtonsController.ResetSkin();
        hairColorsSkinButtonsController.ResetSkin();
    }
    void ResetBagSkin()
    {
        bagSkinButtonController.ResetSkin();
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
