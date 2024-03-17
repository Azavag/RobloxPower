using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class SkinCard : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    public SkinScriptableObejct skinScriptable;
    private SkinType skinType;
    [SerializeField]
    private int idNumber;
    public bool isSelected { private set; get; }
    public bool isBought { private set; get; }
    private int price;
    public bool isAdsReward;

    [Header("Card Components")]
    [SerializeField]
    private GameObject lockImage;
    [SerializeField]
    private GameObject selectedImage;
    [SerializeField]
    private GameObject priceObject;
    [SerializeField]
    private GameObject adsObject;
    [SerializeField]
    private Image skinImage;

    [Header("SkinsButtonBackgrounds")]
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Sprite standartBackgroundSprite;
    [SerializeField]
    private Sprite selectedBackgroundSprite;

    private void Awake()
    {
        priceObject.SetActive(true);
        lockImage.SetActive(true);
        selectedImage.SetActive(false);

        idNumber = skinScriptable.idNumber;
        skinType = skinScriptable.skinType;
        price = skinScriptable.price;
        skinImage.sprite = skinScriptable.sprite;
        priceObject.GetComponentInChildren<TextMeshProUGUI>().text = price.ToString();
        isAdsReward = skinScriptable.isAdsReward;
    }
    private void OnValidate()
    {
        idNumber = skinScriptable.idNumber;
        skinType = skinScriptable.skinType;
        price = skinScriptable.price;
        skinImage.sprite = skinScriptable.sprite;
        priceObject.GetComponentInChildren<TextMeshProUGUI>().text = price.ToString();
        isAdsReward = skinScriptable.isAdsReward;
    }
    private void OnEnable()
    {
        if(!isAdsReward)
        {
            adsObject.SetActive(false);
        }
        else priceObject.SetActive(false);
        if (isBought)
        {
            lockImage.SetActive(false);
            priceObject.SetActive(false);
            adsObject.SetActive(false);
        }
        
        if (isSelected)
        {
            selectedImage.SetActive(true);
        }
    }

    public void Unclock()
    {
        isBought = true;
        lockImage.SetActive(false);
        priceObject.SetActive(false);
        adsObject.SetActive(false);
    }
    public void Select()
    {
        if (!isBought)
            return;
        isSelected = true;
        selectedImage.gameObject.SetActive(true);
    }

    public void Unselect()
    {
        isSelected = false;
        selectedImage.gameObject.SetActive(false);
    }

    public void Highlight() => backgroundImage.sprite = selectedBackgroundSprite;
    public void UnHighlight() => backgroundImage.sprite = standartBackgroundSprite;

    public SkinType GetSkinType()
    {
        return skinType;
    }
    public int GetSkinPrice()
    {
        return price;
    }
    public int GetSkinIdNumber()
    {
        return idNumber;
    }
}
