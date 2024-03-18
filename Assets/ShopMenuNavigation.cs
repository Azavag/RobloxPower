using UnityEngine;
using UnityEngine.UI;

public class ShopMenuNavigation : MonoBehaviour
{
    [SerializeField]
    private Button[] navButtons;

    [SerializeField]
    private GameObject[] navPages;

    [SerializeField]
    private GameObject buttonContent;

    [SerializeField]
    private Button backButton;

    private GameObject prevPage;
    private GameObject currentPage;

    private SoundController soundController;
    private void OnEnable()
    {
        for (int i = 0; i < navButtons.Length; i++)
        {
            int copy = i;
            navButtons[copy].onClick.AddListener(() => { OpenPage(copy); });
            navPages[copy].SetActive(false);
        }

        backButton.onClick.AddListener(() => { OpenPrevPage(); });
    }
    private void OnDisable()
    {
        for (int i = 0; i < navButtons.Length; i++)
        {
            int copy = i;
            navButtons[copy].onClick.RemoveListener(() => { OpenPage(copy); });
            navPages[copy].SetActive(false);
        }
        backButton.onClick.RemoveListener(() => { OpenPrevPage(); });

        buttonContent.SetActive(true);
    }
    private void Awake()
    {
        soundController = FindObjectOfType<SoundController>();
       
    }
    void Start()
    {
        prevPage = buttonContent;
        currentPage = buttonContent;
    }

    void OpenPage(int index)
    {
        navPages[index].SetActive(true);
        currentPage = navPages[index];
        buttonContent.SetActive(false);
        soundController.MakeClickSound();
    }

    void OpenPrevPage()
    {
        currentPage.SetActive(false);
        prevPage.SetActive(true);
        currentPage = prevPage;
        soundController.MakeClickSound();
    }
   

    
    void Update()
    {
        
    }
}
