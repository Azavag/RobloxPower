using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PunchbagsShop : MonoBehaviour
{
    private PunchbagCardControl[] punchbagsCards;
    private PunchbagScriptableObject[] punchbagsScriptables;
    SoundController soundController;

    [SerializeField]
    private Button upgradePunchbagButton;
    [SerializeField]
    private TextMeshProUGUI priceText;
    private int currentPunchbagNumber;
    private int currentPrice;
    [Header("Swap Animaion")]
    [SerializeField]
    private float swapDuration;
    [SerializeField]
    private float frameAnimationDuration;
    Tween buyButtonShakeTween;
    [Header("Localization")]
    private string maxInterText;
    private string maxEngText = "Max";
    private string maxRusText = "Макс";


    public static Action<int> PunchbagPurchased;
    public static Action<int> PunchbagSelected;

    private void OnEnable()
    {       
         upgradePunchbagButton.onClick.AddListener(OnClickBuyButton);
    }

    private void OnDisable()
    {
        upgradePunchbagButton.onClick.RemoveListener(OnClickBuyButton);
    }

    private void Start()
    {
        if (Language.Instance.languageName == LanguageName.Rus)
            maxInterText = maxRusText;
        if(Language.Instance.languageName == LanguageName.Eng)
            maxInterText = maxEngText;
        soundController = FindObjectOfType<SoundController>();

        buyButtonShakeTween = ButtonAnimation.ShakeAnimation(upgradePunchbagButton.transform);
    }

    public void GetCardsData(PunchbagCardControl[] punchCards, PunchbagScriptableObject[] punchData)
    {
        punchbagsCards = punchCards;
        punchbagsScriptables = punchData;
        Initialization();
    }
    void Initialization()
    {
        foreach (var card in punchbagsCards)
        {
            card.gameObject.SetActive(false);
        }
        currentPunchbagNumber = Bank.Instance.playerInfo.currentPunchBagNumber;
        punchbagsCards[currentPunchbagNumber].gameObject.SetActive(true);
        SwapButtonPrice();
    }
    void OnClickBuyButton()
    {
        if (Bank.Instance.playerInfo.coins >= currentPrice)
        {
            ConfirmBuy();
            return;
        }
        else DenyBuy();
    }
    void SwapCards()
    {
        upgradePunchbagButton.interactable = false;
        punchbagsCards[currentPunchbagNumber].CardFrameAnimation(frameAnimationDuration, CardRotationsAnimation);      
    }
    void CardRotationsAnimation()
    {
        Tween hideAnimation = RotateTween(punchbagsCards[currentPunchbagNumber].transform, new Vector3(0, -90, 0));
        hideAnimation.OnComplete(delegate 
        {
            punchbagsCards[currentPunchbagNumber].gameObject.SetActive(false);
            punchbagsCards[++currentPunchbagNumber].gameObject.SetActive(true);
            
            punchbagsCards[currentPunchbagNumber].transform.Rotate(0, 90, 0);
            Tween showAnimation = RotateTween(
                punchbagsCards[currentPunchbagNumber].transform, new Vector3(0, 0, 0));
            showAnimation
            .OnComplete(SwapButtonPrice)
            .Play();
        });
        hideAnimation.Play();      
    }
    void SwapButtonPrice()
    {
        int nextPunchbagNumber = currentPunchbagNumber + 1;
        if (nextPunchbagNumber < punchbagsScriptables.Length)
        {
            currentPrice = punchbagsScriptables[nextPunchbagNumber].punchbagPrice;
            priceText.text = currentPrice.ToString();
            upgradePunchbagButton.interactable = true;
        }
        else
        {
            priceText.text = maxInterText;
            upgradePunchbagButton.interactable = false;
        }
    }
   
    Tween RotateTween(Transform cardTransform, Vector3 rotateVector)
    {
        return cardTransform.DORotate(rotateVector, swapDuration, RotateMode.Fast)
            .SetAutoKill();
    }
    void ConfirmBuy()
    {
        soundController.Play("ConfirmBuy");
        SwapCards();
        PunchbagPurchased?.Invoke(-currentPrice);
        PunchbagSelected?.Invoke(currentPunchbagNumber+1);
    }
    void DenyBuy()
    {
        soundController.Play("DeclineBuy");
        buyButtonShakeTween.Rewind();
        buyButtonShakeTween.Play();
    }

}
