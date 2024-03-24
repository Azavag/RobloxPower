using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BuySkinButton : MonoBehaviour
{
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Button adsButton;

    [Header("Refs")]
    private SoundController soundController;
    private static SkinCard selectedSkinCard;
    private AdvManager advManager;
    private HatSkinButtonsController hatCardsController;
    private PetSkinButtonsController petCardsController;
    private TrailSkinButtonsController trailCardsController;
    private ShirtSkinButtonsController shirtCardsController;
    private PantsSkinButtonsController pantsCardsController;
    private GlovesSkinButtonsController glovesCardsController;
    private AccessoriesSkinButtonsController accessoriesCardsController;
    private HairSkinsButtonController hairSkinButtonsController;
    private BagSkinButtonsController bagSkinButtonController;
    private HairColorsSkinButtonsController hairColorsSkinButtonsController;

    Tween shakeTween;

    bool isAdsRewarded;
    public static event Action<int> SkinPurchaseMade;

    private void OnEnable()
    {
        HatSkinCard.HatCardClicked += SetSelectedSkinCardType;
        PetSkinCard.PetCardClicked += SetSelectedSkinCardType;
        TrailSkinCard.TrailCardClicked += SetSelectedSkinCardType;
        ShirtSkinCard.ShirtCardClicked += SetSelectedSkinCardType;
        PantsSkinCard.PantsCardClicked += SetSelectedSkinCardType;
        GlovesSkinCard.GlovesCardClicked += SetSelectedSkinCardType;
        AccessoriesSkinCard.ÀccessoriesCardClicked += SetSelectedSkinCardType;
        HairSkinCard.HairCardClicked += SetSelectedSkinCardType;
        BagSkinCard.BagCardClicked += SetSelectedSkinCardType;
        HairSkinCard.HairCardClicked += SetSelectedSkinCardType;
        HairColorSkinCard.HairColorCardClicked += SetSelectedSkinCardType;

        adsButton.onClick.AddListener(OnClickAdsButton);
        buyButton.onClick.AddListener(OnClickBuyButton);
    }
    private void OnDisable()
    {
        HatSkinCard.HatCardClicked -= SetSelectedSkinCardType;
        PetSkinCard.PetCardClicked -= SetSelectedSkinCardType;
        TrailSkinCard.TrailCardClicked -= SetSelectedSkinCardType;
        ShirtSkinCard.ShirtCardClicked -= SetSelectedSkinCardType;
        PantsSkinCard.PantsCardClicked -= SetSelectedSkinCardType;
        GlovesSkinCard.GlovesCardClicked -= SetSelectedSkinCardType;
        AccessoriesSkinCard.ÀccessoriesCardClicked -= SetSelectedSkinCardType;
        HairSkinCard.HairCardClicked -= SetSelectedSkinCardType;
        BagSkinCard.BagCardClicked -= SetSelectedSkinCardType;
        HairSkinCard.HairCardClicked -= SetSelectedSkinCardType;
        HairColorSkinCard.HairColorCardClicked -= SetSelectedSkinCardType;

        adsButton.onClick.RemoveListener(OnClickAdsButton);
        buyButton.onClick.RemoveListener(OnClickBuyButton);
    }
    private void OnDestroy()
    {
        shakeTween.Kill();
    }

    private void Awake()
    {
        soundController = FindObjectOfType<SoundController>();
        advManager = FindObjectOfType<AdvManager>();

        hatCardsController = GetComponentInChildren<HatSkinButtonsController>();
        petCardsController = GetComponentInChildren<PetSkinButtonsController>();
        trailCardsController = GetComponentInChildren<TrailSkinButtonsController>();
        shirtCardsController = GetComponentInChildren<ShirtSkinButtonsController>();
        pantsCardsController = GetComponentInChildren<PantsSkinButtonsController>();
        glovesCardsController = GetComponentInChildren<GlovesSkinButtonsController>();
        accessoriesCardsController = GetComponentInChildren<AccessoriesSkinButtonsController>();
        hairSkinButtonsController = GetComponentInChildren<HairSkinsButtonController>();
        bagSkinButtonController = GetComponentInChildren<BagSkinButtonsController>();
        hairColorsSkinButtonsController = GetComponentInChildren<HairColorsSkinButtonsController>();

    }
    private void Start()
    {
        shakeTween = ButtonAnimation.ShakeAnimation(buyButton.transform);
    }
    void SetSelectedSkinCardType(SkinCard skinCard)
    {
        selectedSkinCard = skinCard;
    }

    public static SkinType GetSelectedSkinCardType()
    {
        return selectedSkinCard.GetSkinType();
    }

    void OnClickBuyButton()
    {       
        if (Bank.Instance.playerInfo.coins >= selectedSkinCard.GetSkinPrice())
        {         
            ConfirmBuy();
            SkinPurchaseMade?.Invoke(-selectedSkinCard.GetSkinPrice());
            return;
        }
        DenyBuy();
    }
    void OnClickAdsButton()
    {
#if UNITY_EDITOR
        ConfirmBuy();
#endif
        advManager.ShowRewardedAdv();       
    }
    void DenyBuy()
    {
        soundController.Play("DeclineBuy");
        shakeTween.Pause();
        shakeTween.Rewind();
        shakeTween.Play();
    }
    void ConfirmBuy()
    {
        soundController.Play("ConfirmBuy");
        selectedSkinCard.Unclock();
        switch (selectedSkinCard.GetSkinType())
        {
            case SkinType.Hat:
                hatCardsController.ShowCurrentModelView(selectedSkinCard);
                hatCardsController.SaveStates(selectedSkinCard.GetSkinIdNumber());
                break;
            case SkinType.Pet:
                petCardsController.ShowCurrentModelView(selectedSkinCard);
                petCardsController.SaveStates(selectedSkinCard.GetSkinIdNumber());
                break;
            case SkinType.Trail:
                trailCardsController.ShowCurrentModelView(selectedSkinCard);
                trailCardsController.SaveStates(selectedSkinCard.GetSkinIdNumber());
                break;
            case SkinType.Shirt:
                shirtCardsController.ShowCurrentModelView(selectedSkinCard);
                shirtCardsController.SaveStates(selectedSkinCard.GetSkinIdNumber());
                break;
            case SkinType.Pants:
                pantsCardsController.ShowCurrentModelView(selectedSkinCard);
                pantsCardsController.SaveStates(selectedSkinCard.GetSkinIdNumber());
                break;
            case SkinType.Gloves:
                glovesCardsController.ShowCurrentModelView(selectedSkinCard);
                glovesCardsController.SaveStates(selectedSkinCard.GetSkinIdNumber());
                break;
            case SkinType.HairStyles:
                hairSkinButtonsController.ShowCurrentModelView(selectedSkinCard);
                hairSkinButtonsController.SaveStates(selectedSkinCard.GetSkinIdNumber());
                break;
            case SkinType.HairColors:
                hairColorsSkinButtonsController.ShowCurrentModelView(selectedSkinCard);
                hairColorsSkinButtonsController.SaveStates(selectedSkinCard.GetSkinIdNumber());
                break;
            case SkinType.Accessories:
                accessoriesCardsController.ShowCurrentModelView(selectedSkinCard);
                accessoriesCardsController.SaveStates(selectedSkinCard.GetSkinIdNumber());
                break;
            case SkinType.Bags:
                bagSkinButtonController.ShowCurrentModelView(selectedSkinCard);
                bagSkinButtonController.SaveStates(selectedSkinCard.GetSkinIdNumber());
                break;

        }
             
    }
    //Â jslib
    public void SetRewardingState()
    {
        isAdsRewarded = true;
    }
    //Â jslib
    public void UnlockRewardSkin()
    {
        if(isAdsRewarded)
            ConfirmBuy();
        isAdsRewarded = false;
    }
}
