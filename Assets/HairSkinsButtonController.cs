using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HairSkinsButtonController : MonoBehaviour
{
    private SoundController soundController;
    [SerializeField]
    private HairSkinCard[] skinCards;

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
    private HatSkinButtonsController hatSkinButtonController;

    private void OnEnable()
    {
        HairSkinCard.HairCardClicked += OnSkinCardClicked;
        HatSkinCard.HatCardClicked += HideSkin;
        selectButton.onClick.AddListener(OnClickSelectButton);
    }
    private void OnDisable()
    {
        HairSkinCard.HairCardClicked -= OnSkinCardClicked;
        HatSkinCard.HatCardClicked -= HideSkin;

        selectButton.onClick.RemoveListener(OnClickSelectButton);
    }
    private void Awake()
    {
        soundController = FindObjectOfType<SoundController>();
    }
    void Initialization()
    {
        skinsBuyState = Bank.Instance.playerInfo.hairSkinsBuyStates;
        selectedSkinId = Bank.Instance.playerInfo.selectedHairId;
        skinsBuyState = new bool[skinCards.Length];
        skinsBuyState[0] = true;
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
            if (!skinCard.isSelected)
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
        hatSkinButtonController.DropSkin();
        ShowSkinObject(selectedSkinId);
        Bank.Instance.playerInfo.selectedHairId = selectedSkinId;       //Сохранение
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
        Bank.Instance.playerInfo.selectedHairId = selectedSkinId;
    }
    public void SaveStates(int id)
    {
        skinsBuyState[id] = true;
        Bank.Instance.playerInfo.hairSkinsBuyStates[id] = true;
        YandexSDK.Save();
    }
}
