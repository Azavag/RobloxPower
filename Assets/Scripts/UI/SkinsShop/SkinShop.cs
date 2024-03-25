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
    private AccessoriesSkinButtonsController àccessoriesSkinButtonController;
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
        àccessoriesSkinButtonController = GetComponentInChildren<AccessoriesSkinButtonsController>();
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
                ResetHatSkin();
                ResetHairSkin();
                break;
            case 1:
                ResetHairSkin();
                ResetHatSkin();
                break;
            case 2:
                ResetÀccessoriesSkin();
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
                RotateAroundPlayer();
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
    //Íà êàêóþ âêëàäêó ïåðåêëþ÷èëèñü
    void OnTabSelected(int index)
    {
        soundController.MakeClickSound();      
        ToggleStatWindow(false);
        
        switch (index)
        {
            //Íà ñòðàíèöó âûáîðà îïöèé âîëîñ
            case -2:
                ToggleBuyWindow(false);
                lastOpenedPage = 1;
                return;
            case -1:
                ToggleBuyWindow(false);
                ResetPages();
                return;
            case 6:
                RotateAroundPlayer();
                break;
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

    void ResetÀccessoriesSkin()
    {
        àccessoriesSkinButtonController.ResetSkin();
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

    public void RotateAroundPlayer()
    {
        trigger.RotatePlayer();

        //Vector3 targetRotation = playerController.gameObject.transform.rotation.eulerAngles;
        //targetRotation = new Vector3(targetRotation.x, targetRotation.y - 180, targetRotation.z);

        //playerController.gameObject.transform.DORotate(targetRotation, 0.25f)
        //    .SetAutoKill()
        //    .Play();
    }
}
