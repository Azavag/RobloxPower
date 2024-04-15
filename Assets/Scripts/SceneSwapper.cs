using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private int nextSceneNumber;
    [SerializeField]
    private TextMeshProUGUI nextSceneNumberText;

    [SerializeField]
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
        if (isLevelUnlock)
            RotateDoors(75f);

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
        Bank.Instance.playerInfo.currentPunchBagNumber = 0;
        Bank.Instance.playerInfo.currentEnemyNumber = 0;
        for (int i = 0; i < Bank.Instance.playerInfo.levelEnemiesTimers.Length; i++)
            Bank.Instance.playerInfo.levelEnemiesTimers[i] = 0;
        Bank.Instance.playerInfo.currentLevelNumber = nextSceneNumber;
        YandexSDK.Save();
        fadeScreen.ExitLevelFadeIn(() => SceneManager.LoadScene(nextSceneNumber)); 
        
    }

    public void UnlockLevel()
    {
        isLevelUnlock = true;
        RotateDoors(75f);
        Bank.Instance.playerInfo.areLevelsUnlock[nextSceneNumber-1] = isLevelUnlock;
       
    }

    void RotateDoors(float angle)
    {
        leftDoor.DOLocalRotate(new Vector3(0, -angle, 0), 1f)
            .SetAutoKill()
            .Play();
        rightDoor.DOLocalRotate(new Vector3(0, angle, 0), 1f)
            .SetAutoKill()
            .Play();
    }

}
