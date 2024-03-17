using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField]
    private TextMeshProUGUI passiveStatsText;
    [SerializeField]
    private TextMeshProUGUI activeStatsText;

    string secInterText, clickInterText;
    private void OnEnable()
    {
        HatSkinCard.HatCardClicked += OnSkinCardClicked;
        PetSkinCard.PetCardClicked += OnSkinCardClicked;
        TrailSkinCard.TrailCardClicked += OnSkinCardClicked;
        SetInternationalText();
    }
    private void OnDisable()
    {
        HatSkinCard.HatCardClicked -= OnSkinCardClicked;
        PetSkinCard.PetCardClicked -= OnSkinCardClicked;
        TrailSkinCard.TrailCardClicked -= OnSkinCardClicked;
    }

    private void Start()
    {
       
    }
    void SetInternationalText()
    {
        if (Language.Instance.languageName == LanguageName.Rus)
        {
            secInterText = "сек";
            clickInterText = "клик";
        }
        else
        {
            secInterText = "sec";
            clickInterText = "click";
        }
    }
    void OnSkinCardClicked(SkinCard skinCard)
    {
        UpdateStatsText(skinCard.skinScriptable.skinStats);
    }

    public void UpdateStatsText(SkinStats skinStats)
    {
       
        passiveStatsText.text = $"+{skinStats.passiveStats}/{secInterText}";
        activeStatsText.text = $"+{skinStats.activeStats}/{clickInterText}";
    }
}
