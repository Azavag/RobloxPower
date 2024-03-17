using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PunchbagCardControl : MonoBehaviour
{
    public PunchbagScriptableObject punchbagScriptable;
    private int idNumber;
    [Header("UI Fields")]
    [SerializeField]
    private Image modelImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI statsText;
    [SerializeField]
    private GameObject selectedFrame;
    [Header("Localization")]
    private string clickInterText;
    private string clickEngText = "click";
    private string clickRusText = "клик";
    


    private void OnValidate()
    {
        //CardInitializtion();
    }
    void Start()
    {
        if (Language.Instance.languageName == LanguageName.Rus)
            clickInterText = clickRusText;
        else clickInterText = clickEngText;

        CardInitializtion();       
    }

    void CardInitializtion()
    {
        modelImage.sprite = punchbagScriptable.punchbagSprite;
        idNumber = punchbagScriptable.punchbagIdNumber;
        if(Language.Instance.languageName == LanguageName.Rus)
            nameText.text = $"{punchbagScriptable.rusName}";
        else nameText.text = $"{punchbagScriptable.engName}";
        statsText.text = $"Макс. множитель\nх{punchbagScriptable.punchbagStats.maxStreak}";
        
        selectedFrame.transform.localScale = 
            new Vector3(selectedFrame.transform.localScale.x, 0, selectedFrame.transform.localScale.z);
    }


    public void CardFrameAnimation(float animDuration, TweenCallback callback)
    {
        selectedFrame.transform.DOScaleY(1, animDuration)
            .SetAutoKill()
            .OnComplete(callback)
            .Play();
    }


}
