using System;
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

    [Header("HairContent")]
    [SerializeField]
    private Button[] hairOptionsButtons; 
    [SerializeField]
    private GameObject[] hairOptions;


    [SerializeField]
    private Button backButton;
    private GameObject prevPage;
    private GameObject currentPage;

    private SoundController soundController;

    public static event Action<int> TabSelected;
    private void OnEnable()
    {
        for (int i = 0; i < navButtons.Length; i++)
        {
            int copy = i;
            navButtons[copy].onClick.AddListener(() => { OpenPage(copy); });
            navPages[copy].SetActive(false);
        }
        for (int i = 0;i < hairOptionsButtons.Length;i++)
        {
            int copy = i;
            hairOptionsButtons[copy].onClick.AddListener( () => { OpenHairOption(copy); });
            hairOptions[copy].SetActive(false);
        }
        backButton.onClick.AddListener(() => { OpenPrevPage(); });

        prevPage = buttonContent;
        currentPage = buttonContent;
        buttonContent.SetActive(true);
        ToggleBackButton(false);
    }
    private void OnDisable()
    {
        for (int i = 0; i < navButtons.Length; i++)
        {
            int copy = i;
            navButtons[copy].onClick.RemoveAllListeners();
            navPages[copy].SetActive(false);
        }
        for (int i = 0; i < hairOptionsButtons.Length; i++)
        {
            int copy = i;
            hairOptionsButtons[copy].onClick.RemoveAllListeners();
            hairOptions[copy].SetActive(false);
        }
        backButton.onClick.RemoveAllListeners();
        
    }
    private void Awake()
    {
        soundController = FindObjectOfType<SoundController>();
       
    }
    void Start()
    {
        TabSelected?.Invoke(-1);
    }

    void OpenPage(int index)
    {
        ToggleBackButton(true);
        prevPage.SetActive(false);
        navPages[index].SetActive(true);
        currentPage = navPages[index];

        soundController.MakeClickSound();
        TabSelected?.Invoke(index);
    }

    void OpenPrevPage()
    {
        soundController.MakeClickSound();
        currentPage.SetActive(false);
        prevPage.SetActive(true);
        if (prevPage == buttonContent)
        {
            ToggleBackButton(false);
            currentPage = prevPage;
            TabSelected?.Invoke(-1);
            return;
        }
        if (currentPage == hairOptions[0] || currentPage == hairOptions[1])
        {
            currentPage = navPages[1];
            prevPage = buttonContent;
            TabSelected?.Invoke(-2);
        }
     
    }
    void OpenHairOption(int index)
    {
        prevPage = navPages[1];
        prevPage.SetActive(false);
        hairOptions[index].SetActive(true);
        currentPage = hairOptions[index];
        soundController.MakeClickSound();
        TabSelected?.Invoke(-3);
    }
   
    void ToggleBackButton(bool toggle)
    {
        backButton.gameObject.SetActive(toggle);
    }
}
