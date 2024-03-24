using UnityEngine;
using UnityEngine.UI;

public class PunchbagsShopTrigger : MonoBehaviour
{
    [SerializeField]
    private UINavigation navigation;
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Button closePunchbagsShopButton;

    SoundController soundController;
    private void OnEnable()
    {
        closePunchbagsShopButton.onClick.AddListener(ClosePunchbagsShop);
    }
    private void OnDisable()
    {
        closePunchbagsShopButton.onClick.RemoveListener(ClosePunchbagsShop);
    }

    private void Awake()
    {
        soundController =FindObjectOfType<SoundController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OpenPunchbagsShop();          
        }
    }

    void OpenPunchbagsShop()
    {
        soundController.MakeClickSound();
        PlayerController.IsBusy = true;
        navigation.ToggleUpgradesPunchbagsCanvas(true);
        playerController.BlockPlayersInput(true);
        CursorLocking.LockCursor(false);
    }

    void ClosePunchbagsShop()
    {
        soundController.MakeClickSound();
        PlayerController.IsBusy = false;
        navigation.ToggleUpgradesPunchbagsCanvas(false);
        playerController.BlockPlayersInput(false);
        CursorLocking.LockCursor(true);
    }
 
}
