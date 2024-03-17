using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PunchbagsShop : MonoBehaviour
{
    [SerializeField]
    private PunchbagCardControl[] punchbagsCards;
    [SerializeField]
    private PunchbagScriptableObject[] punchbagsScriptables;

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

    public static Action<int> PunchbagPurchased;
    public static Action<int> PunchbagSelected;
    private void OnValidate()
    {
        for(int i=0; i< punchbagsCards.Length; i++)
        {
            punchbagsCards[i].punchbagScriptable = punchbagsScriptables[i];
        }
    }
    private void OnEnable()
    {       
         upgradePunchbagButton.onClick.AddListener(OnClickBuyButton);
    }

    private void OnDisable()
    {
        upgradePunchbagButton.onClick.RemoveListener(OnClickBuyButton);
    }
    private void Awake()
    {
        Initialization();
    }
    void Start()
    {
        
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
            priceText.text = "Макс";
            upgradePunchbagButton.interactable = false;
        }
    }
    Tween RotateTween(Transform cardTransform, Vector3 rotateVector)
    {
        return cardTransform.DORotate(rotateVector, swapDuration, RotateMode.Fast)
            .SetAutoKill();            
    }

    void Initialization()
    {
        currentPunchbagNumber = Bank.Instance.playerInfo.currentPunchBagNumber;
        foreach(var card in punchbagsCards)
        {
            card.gameObject.SetActive(false);
        }
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
   
    void ConfirmBuy()
    {
        //звук
        SwapCards();
        PunchbagPurchased?.Invoke(-currentPrice);
        PunchbagSelected?.Invoke(currentPunchbagNumber+1);
    }
    void DenyBuy()
    {
        //звук
        buyButtonShakeTween.Rewind();
        buyButtonShakeTween.Play();
    }

}
