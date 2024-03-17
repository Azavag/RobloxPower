using System.Runtime.InteropServices;
using UnityEngine;

public enum LanguageName
{ 
    Rus, 
    Eng
}


public class Language : MonoBehaviour
{
    public string currentLanguage;
    public static Language Instance;
    public LanguageName languageName;

    [DllImport("__Internal")]
    private static extern string GetLang();
  
    private void Awake()
    {
        if (Instance == null)
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            Instance = this;
#if !UNITY_EDITOR
            currentLanguage = GetLang(); 
#endif

            switch (currentLanguage)
            {
                case "ru":
                    languageName = LanguageName.Rus;
                    break;
                case "en":
                    languageName = LanguageName.Eng;
                    break;
            }

            return;
        }
        Destroy(gameObject);
    }
}
