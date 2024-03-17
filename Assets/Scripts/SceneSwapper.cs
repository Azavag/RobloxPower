using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwapper : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private int nextSceneNumber;
    [SerializeField]
    private TextMeshProUGUI nextSceneNumberText;

    private bool isLevelUnlock;
    [Header("Refs")]
    [SerializeField]
    private BuyLevel buyLevel;
    [SerializeField]
    private FadeScreen fadeScreen;
    private SoundController soundController;

    string levelInterText;


    [SerializeField]
    private Transform leftDoor;
    [SerializeField]
    private Transform rightDoor;
    private void Awake()
    {
        soundController = FindObjectOfType<SoundController>();

        if (Language.Instance.languageName == LanguageName.Rus)
            levelInterText = "Уровень";
        else levelInterText = "Level";
    }
    private void Start()
    {
        isLevelUnlock = Bank.Instance.playerInfo.areLevelsUnlock[nextSceneNumber-1];
        nextSceneNumberText.text = $"{levelInterText} {nextSceneNumber}";
        RotateDoors(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!isLevelUnlock)
        {
            buyLevel.OpenSceneSwapAlertPanel();
        }
        if(other.CompareTag("Player") && isLevelUnlock)
        {
            SwapScene();
        }
    }

    public void SwapScene()
    {
        soundController.Play("SceneSwap");
        fadeScreen.ExitLevelFadeIn(() => SceneManager.LoadScene(nextSceneNumber));      
    }

    public void UnlockLevel()
    {
        isLevelUnlock = true;
        RotateDoors(25);
        Bank.Instance.playerInfo.areLevelsUnlock[nextSceneNumber-1] = isLevelUnlock;
    }

    void RotateDoors(float angle)
    {
        leftDoor.localRotation = Quaternion.Euler(0, -angle, 0);
        rightDoor.localRotation = Quaternion.Euler(0, angle, 0);
    }

}
