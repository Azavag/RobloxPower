using DG.Tweening;
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

    private HatSkinButtonsController hatSkinButtonsController;
    private PetSkinButtonsController petSkinButtonsController;
    private TrailSkinButtonsController trailSkinButtonsControllers;
    private ShirtSkinButtonsController shirtSkinButtonsController;
    private PantsSkinButtonsController pantsSkinButtonsController;
    private GlovesSkinButtonsController glovesSkinButtonController;
    private AccessoriesSkinButtonsController �ccessoriesSkinButtonController;
    private HairSkinsButtonController hairSkinButtonsController;
    private BagSkinButtonsController bagSkinButtonController;

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
        �ccessoriesSkinButtonController = GetComponentInChildren<AccessoriesSkinButtonsController>();
        hairSkinButtonsController = GetComponentInChildren<HairSkinsButtonController>();
        bagSkinButtonController = GetComponentInChildren<BagSkinButtonsController>();
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
                ResetHatSkin();
                ResetHairSkin();
                break;
            case 1:
                ResetHairSkin();
                ResetHatSkin();
                break;
            case 2:
                Reset�ccessoriesSkin();
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
    //�� ����� ������� �������������
    void OnTabSelected(int index)
    {
        soundController.MakeClickSound();

        switch (index)
        { 
            case -1:
                ToggleBuyWindow(false);
                ResetPages();
                return;
            case 6:
                RotateAroundPlayer();
                break;
            case 0:
                ToggleStatWindow(false);
                break;
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

    void Reset�ccessoriesSkin()
    {
        �ccessoriesSkinButtonController.ResetSkin();
    }

    void ResetHairSkin()
    {
        hairSkinButtonsController.ResetSkin();
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
        Vector3 targetRotation = playerController.gameObject.transform.rotation.eulerAngles;
        targetRotation = new Vector3(targetRotation.x, targetRotation.y -180, targetRotation.z);
        playerController.gameObject.transform.DOLocalRotate(targetRotation, 0.05f)
            .SetAutoKill()
            .Play();
    }
}
