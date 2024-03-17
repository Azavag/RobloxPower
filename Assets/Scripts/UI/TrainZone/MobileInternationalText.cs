using TMPro;
using UnityEngine;

public class MobileInternationalText : MonoBehaviour
{
    [TextArea]
    [SerializeField] string _ru;
    [TextArea]
    [SerializeField] string _en;

    private void Start()
    {
        if(IsMobileController.IsMobile)
        {
            if (Language.Instance.languageName == LanguageName.Rus)
                GetComponent<TextMeshProUGUI>().text = _ru;
            else
                GetComponent<TextMeshProUGUI>().text = _en;
        }
        
    }
}
