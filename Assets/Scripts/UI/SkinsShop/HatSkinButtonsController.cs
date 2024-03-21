using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HatSkinButtonsController : SkinButtonController
{
    private SoundController soundController;
    [SerializeField]
    private SkinCard[] skinCards;

    [SerializeField]
    private GameObject[] skinObjects;
    [SerializeField]
    private int selectedSkinId;
    private int prevHighlightedSkinId = 0;
    private bool[] skinsBuyState;

    [SerializeField]
    private SkinType skinType;

    [Header("Model Buttons")]
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private Button adsButton;
    [SerializeField]
    private GameObject selectedText;

    SkinCard clickedSkinCard;
    [SerializeField]
    private StatsPanel statsPanel;
    [SerializeField]
    private SkinCharacteristics hatSkinStats;
    [SerializeField]
    private HairSkinsButtonController hairSkinButtonController;

    private void OnEnable()
    {
        HatSkinCard.HatCardClicked += OnSkinCardClicked;
        HairSkinCard.HairCardClicked += HideSkin;


        selectButton.onClick.AddListener(OnClickSelectButton);        
    }
    private void OnDisable()
    {
        HatSkinCard.HatCardClicked -= OnSkinCardClicked;
        HairSkinCard.HairCardClicked -= HideSkin;

        selectButton.onClick.RemoveListener(OnClickSelectButton);
    }
    private void Awake()
    {
        soundController = FindObjectOfType<SoundController>();
    }
    void Initialization()
    {
        skinsBuyState = Bank.Instance.playerInfo.hatSkinsBuyStates;
        selectedSkinId = Bank.Instance.playerInfo.selectedHatId;
        hatSkinStats.SetStatsFromSkin(skinCards[selectedSkinId]);
    }
    void Start()
    {
        Initialization();
        for (int i = 0; i < skinCards.Length; i++)
        {
            if (skinsBuyState[i])
                skinCards[i].Unclock();
        }
        ShowSkinObject(selectedSkinId);

        clickedSkinCard = skinCards[selectedSkinId];
        clickedSkinCard.Select();
        clickedSkinCard.Highlight();
    }

    public void OnSkinCardClicked(SkinCard skinCard)
    {
        soundController.Play("CardClick");
        clickedSkinCard = skinCard;
        foreach (var entity in skinCards) 
        { 
            entity.UnHighlight();
        }
        clickedSkinCard.Highlight();
        ShowCurrentModelView(clickedSkinCard);
        ShowSkinObject(clickedSkinCard.GetSkinIdNumber());
    }

    public void ShowCurrentModelView(SkinCard skinCard)
    {
        buyButton.gameObject.SetActive(false);
        selectButton.gameObject.SetActive(false);
        selectedText.gameObject.SetActive(false);
        adsButton.gameObject.SetActive(false);

        if (skinCard.isAdsReward && !skinCard.isBought)
            adsButton.gameObject.SetActive(true);

        if (!skinCard.isBought)
        {
            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text =
                skinCard.GetSkinPrice().ToString();   
        }
        else
        {
            if(!skinCard.isSelected)
                selectButton.gameObject.SetActive(true);
            else selectedText.gameObject.SetActive(true);
        }
    }

    void OnClickSelectButton()
    {
        if (BuySkinButton.GetSelectedSkinCardType() != skinType)
            return;

        foreach (var entity in skinCards)
        {
            entity.Unselect();
        }
        soundController.Play("SelectSkin");
        clickedSkinCard.Select();
        ShowCurrentModelView(clickedSkinCard);
        selectedSkinId = clickedSkinCard.GetSkinIdNumber();
        hairSkinButtonController.DropSkin();
        ShowSkinObject(selectedSkinId);
        Bank.Instance.playerInfo.selectedHatId = selectedSkinId;       //Сохранение
        YandexSDK.Save();
    }
    public void SaveStates(int id)
    {
        skinsBuyState[id] = true;
        Bank.Instance.playerInfo.hatSkinsBuyStates[id] = true;
        YandexSDK.Save();
    }
    void ShowSkinObject(int id)
    {
        skinObjects[prevHighlightedSkinId].SetActive(false);
        skinObjects[id].SetActive(true);
        prevHighlightedSkinId = id;
    }
    public void ResetSkin()
    {
        foreach (var card in skinCards)
        {
            card.UnHighlight();
        }
        skinCards[selectedSkinId].Highlight();
        clickedSkinCard = skinCards[selectedSkinId];
        ShowSkinObject(selectedSkinId);
        ShowCurrentModelView(skinCards[selectedSkinId]);
    }
    //public void ShowCurrentSkinStats()
    //{
    //    statsPanel.UpdateStatsText(skinCards[selectedSkinId].skinScriptable.skinStats);
    //}

    void HideSkin(SkinCard card)
    {       
        ShowSkinObject(0);
    }
    public void DropSkin()
    {
        clickedSkinCard.Unselect();
        selectedSkinId = 0;
        clickedSkinCard = skinCards[selectedSkinId];
        clickedSkinCard.Select();
        ResetSkin();      
        Bank.Instance.playerInfo.selectedHatId = selectedSkinId;
    }
}
