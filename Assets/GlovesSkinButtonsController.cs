using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlovesSkinButtonsController : MonoBehaviour
{
    private SoundController soundController;
    [SerializeField]
    private GlovesSkinCard[] skinCards;
    [SerializeField]
    private Color[] glovesColors;
    [SerializeField]
    private int selectedSkinId;
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
    private Material glovesMaterial;

    private void OnEnable()
    {
        GlovesSkinCard.GlovesCardClicked += OnSkinCardClicked;
        selectButton.onClick.AddListener(OnClickSelectButton);
    }
    private void OnDisable()
    {
        GlovesSkinCard.GlovesCardClicked -= OnSkinCardClicked;
        selectButton.onClick.RemoveListener(OnClickSelectButton);
    }
    private void OnValidate()
    {
        Initialization();
    }
    private void Awake()
    {
        soundController = FindObjectOfType<SoundController>();
    }

    void Initialization()
    {
        for (int i = 0; i < skinCards.Length; i++)
        {
            skinCards[i].ChangeImageColor(glovesColors[i]);
        }

    }
    void Start()
    {
        skinsBuyState = Bank.Instance.playerInfo.glovesSkinsBuyStates;
        selectedSkinId = Bank.Instance.playerInfo.selectedGlovesId;
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
        ShowSkinObject(selectedSkinId);
        Bank.Instance.playerInfo.selectedGlovesId = selectedSkinId;       //Сохранение
        YandexSDK.Save();
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

    void ShowSkinObject(int id)
    {
        glovesMaterial.color = glovesColors[id];
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
    public void SaveStates(int id)
    {
        skinsBuyState[id] = true;
        Bank.Instance.playerInfo.glovesSkinsBuyStates[id] = true;
        YandexSDK.Save();
    }
}
