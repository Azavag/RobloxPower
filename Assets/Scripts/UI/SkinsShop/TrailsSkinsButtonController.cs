using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrailSkinButtonsController : MonoBehaviour
{
    private SoundController soundController;
    [SerializeField]
    private SkinCard[] skinCards;
    [SerializeField]
    private GameObject[] leftGloveFXObjects;
    [SerializeField]
    private GameObject[] rightGloveFXObjects;

    [SerializeField]
    private int selectedSkinId;
    private int prevHighlightedSkinId = 0;
    private bool[] skinsBuyState;

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
    private SkinCharacteristics trailSkinStats;

    private void OnEnable()
    {
        TrailSkinCard.TrailCardClicked += OnSkinCardClicked;
        selectButton.onClick.AddListener(OnClickSelectButton);
    }
    private void OnDisable()
    {
        TrailSkinCard.TrailCardClicked -= OnSkinCardClicked;
        selectButton.onClick.RemoveListener(OnClickSelectButton);
    }
  

    private void Awake()
    {
        soundController = FindObjectOfType<SoundController>();
    }

    void Initialization()
    {
        skinsBuyState = Bank.Instance.playerInfo.trailSkinsBuyStates;
        selectedSkinId = Bank.Instance.playerInfo.selectedTrailId;
        trailSkinStats.SetStatsFromSkin(skinCards[selectedSkinId]);
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

    public void OnSkinCardClicked(SkinCard skinEntity)
    {
        soundController.Play("CardClick");
        clickedSkinCard = skinEntity;
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
      
        foreach (var entity in skinCards)
        {
            entity.Unselect();
        }
        soundController.Play("SelectSkin");
        clickedSkinCard.Select();
        ShowCurrentModelView(clickedSkinCard);
        selectedSkinId = clickedSkinCard.GetSkinIdNumber();
        ShowSkinObject(selectedSkinId);
        trailSkinStats.SetStatsFromSkin(skinCards[selectedSkinId]);
        Bank.Instance.playerInfo.selectedTrailId = selectedSkinId;       //Сохранение
        YandexSDK.Save();
    }
    public void SaveStates(int id)
    {
        skinsBuyState[id] = true;
        Bank.Instance.playerInfo.trailSkinsBuyStates = skinsBuyState;
        YandexSDK.Save();
    }
    void ShowSkinObject(int id)
    {
        leftGloveFXObjects[prevHighlightedSkinId].SetActive(false);
        rightGloveFXObjects[prevHighlightedSkinId].SetActive(false);
        leftGloveFXObjects[id].SetActive(true);
        rightGloveFXObjects[id].SetActive(true);

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
    public void ShowCurrentSkinStats()
    {
        statsPanel.UpdateStatsText(skinCards[selectedSkinId].skinScriptable.skinStats);
    }
}
