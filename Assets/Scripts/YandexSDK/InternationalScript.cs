using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InternationalScript : MonoBehaviour
{
    [TextArea]
    [SerializeField] string _ru;
    [TextArea]
    [SerializeField] string _en;

    private void Start()
    {
        if(Language.Instance.languageName == LanguageName.Rus)
            GetComponent<TextMeshProUGUI>().text = _ru;
        else 
            GetComponent<TextMeshProUGUI>().text = _en;        
    }
}
