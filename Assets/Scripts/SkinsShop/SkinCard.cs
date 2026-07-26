using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinCard : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    public SkinScriptableObject skinScriptable;
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
    protected Image skinImage;

    [Header("SkinsButtonBackgrounds")]
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Sprite standartBackgroundSprite;
    [SerializeField]
    private Sprite selectedBackgroundSprite;

    private void Awake()
    {
        ApplyScriptableData();
        RefreshVisuals();
    }

    private void OnValidate()
    {
        if (skinScriptable == null)
            return;

        ApplyScriptableData();
        if (priceObject != null)
            priceObject.GetComponentInChildren<TextMeshProUGUI>().text = price.ToString();
    }

    private void OnEnable()
    {
        if (Bank.Instance != null)
            Bank.Instance.OnDataReady += ApplyUnlockFromBank;

        ApplyUnlockFromBank();
    }

    private void OnDisable()
    {
        if (Bank.Instance != null)
            Bank.Instance.OnDataReady -= ApplyUnlockFromBank;
    }

    private void Start()
    {
        // Повторно после готовности Bank / порядка Awake-Start.
        ApplyUnlockFromBank();
    }

    void ApplyScriptableData()
    {
        if (skinScriptable == null)
            return;

        idNumber = skinScriptable.idNumber;
        skinType = skinScriptable.skinType;
        price = skinScriptable.price;
        isAdsReward = skinScriptable.isAdsReward;

        if (skinImage != null)
            skinImage.sprite = skinScriptable.sprite;
        if (priceObject != null)
        {
            var priceText = priceObject.GetComponentInChildren<TextMeshProUGUI>();
            if (priceText != null)
                priceText.text = price.ToString();
        }
    }

    /// <summary>
    /// Карточка сама читает прогресс: слот 0 и всё, что открыто в Bank.
    /// </summary>
    public void ApplyUnlockFromBank()
    {
        ApplyScriptableData();

        int slotIndex = ResolveSlotIndex();
        bool unlocked = ShouldBeUnlocked(slotIndex);

        if (unlocked)
            Unclock();
        else
            RefreshVisuals();
    }

    int ResolveSlotIndex()
    {
        // Имена вида Card_0_Blank / Card_12_Hat
        const string prefix = "Card_";
        string objectName = name;
        if (objectName.StartsWith(prefix))
        {
            int start = prefix.Length;
            int end = objectName.IndexOf('_', start);
            string numberPart = end > start
                ? objectName.Substring(start, end - start)
                : objectName.Substring(start);
            if (int.TryParse(numberPart, out int parsed))
                return parsed;
        }

        // Fallback: порядковый номер среди соседних SkinCard
        Transform parent = transform.parent;
        if (parent == null)
            return transform.GetSiblingIndex();

        int index = 0;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.GetComponent<SkinCard>() == null)
                continue;
            if (child == transform)
                return index;
            index++;
        }

        return idNumber;
    }

    bool ShouldBeUnlocked(int slotIndex)
    {
        // Бесплатные blank-скины (price 0).
        if (price <= 0 && !isAdsReward)
            return true;

        // Стартовый бесплатный слот.
        if (slotIndex == 0)
            return true;

        if (Bank.Instance == null || Bank.Instance.playerInfo == null)
            return false;

        bool[] buyStates = GetBuyStates(skinType);
        if (buyStates == null || buyStates.Length == 0)
            return false;

        buyStates[0] = true;

        return slotIndex >= 0 && slotIndex < buyStates.Length && buyStates[slotIndex];
    }

    static bool[] GetBuyStates(SkinType type)
    {
        PlayerInfo info = Bank.Instance.playerInfo;
        switch (type)
        {
            case SkinType.Hat: return info.hatSkinsBuyStates;
            case SkinType.Pet: return info.petSkinsBuyStates;
            case SkinType.Trail: return info.trailSkinsBuyStates;
            case SkinType.Shirt: return info.shirtsSkinsBuyStates;
            case SkinType.Pants: return info.pantsSkinsBuyStates;
            case SkinType.Gloves: return info.glovesSkinsBuyStates;
            case SkinType.HairStyles: return info.hairSkinsBuyStates;
            case SkinType.HairColors: return info.hairColorsBuyStates;
            case SkinType.Accessories: return info.accessoriesSkinsBuyStates;
            case SkinType.Bags: return info.bagsSkinsBuyStates;
            default: return null;
        }
    }

    void RefreshVisuals()
    {
        if (!isAdsReward)
        {
            if (adsObject != null)
                adsObject.SetActive(false);
        }
        else if (priceObject != null)
        {
            priceObject.SetActive(false);
        }

        if (isBought)
        {
            if (lockImage != null)
                lockImage.SetActive(false);
            if (priceObject != null)
                priceObject.SetActive(false);
            if (adsObject != null)
                adsObject.SetActive(false);
        }
        else
        {
            if (lockImage != null)
                lockImage.SetActive(true);
            if (priceObject != null && !isAdsReward)
                priceObject.SetActive(true);
        }

        if (selectedImage != null)
            selectedImage.SetActive(isSelected);
    }

    public void Unclock()
    {
        isBought = true;
        RefreshVisuals();
    }

    public void Select()
    {
        if (!isBought)
            return;
        isSelected = true;
        if (selectedImage != null)
            selectedImage.SetActive(true);
    }

    public void Unselect()
    {
        isSelected = false;
        if (selectedImage != null)
            selectedImage.SetActive(false);
    }

    public void Highlight() => backgroundImage.sprite = selectedBackgroundSprite;
    public void UnHighlight() => backgroundImage.sprite = standartBackgroundSprite;

    public SkinType GetSkinType() => skinType;
    public int GetSkinPrice() => price;
    public int GetSkinIdNumber() => idNumber;
}
