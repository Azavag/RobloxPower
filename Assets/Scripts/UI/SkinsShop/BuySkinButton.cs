using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BuySkinButton : MonoBehaviour
{
    SoundController soundController;
    SkinCard selectedSkinCard;
    AdvManager advManager;
    [SerializeField]
    private HatSkinButtonsController hatCardsController;
    [SerializeField]
    private PetSkinButtonsController petCardsController;
    [SerializeField]
    private TrailSkinButtonsController trailCardsController;
    [SerializeField]
    private ShirtSkinButtonController shirtCardsController;
    [SerializeField]
    private PantsSkinButtonController pantsCardsController;
    [SerializeField]
    private GlovesSkinButtonController glovesCardsController;


    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Button adsButton;
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
    }
    private void Start()
    {
        shakeTween = ButtonAnimation.ShakeAnimation(buyButton.transform);
    }
    void SetSelectedSkinCardType(SkinCard skinCard)
    {
        selectedSkinCard = skinCard;
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
                break;
            case SkinType.Pants:
                pantsCardsController.ShowCurrentModelView(selectedSkinCard);
                break;
            case SkinType.Gloves:
                glovesCardsController.ShowCurrentModelView(selectedSkinCard);
                break;
            case SkinType.Hair: break;
            case SkinType.Accessories: break;
            case SkinType.Bags: break;

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
