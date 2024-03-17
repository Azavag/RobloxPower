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

    private void OnEnable()
    {
        closePunchbagsShopButton.onClick.AddListener(ClosePunchbagsShop);
    }
    private void OnDisable()
    {
        closePunchbagsShopButton.onClick.RemoveListener(ClosePunchbagsShop);
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
        PlayerController.IsBusy = true;
        navigation.ToggleUpgradesPunchbagsCanvas(true);
        playerController.BlockPlayersInput(true);
        CursorLocking.LockCursor(false);
    }

    void ClosePunchbagsShop()
    {
        PlayerController.IsBusy = false;
        navigation.ToggleUpgradesPunchbagsCanvas(false);
        playerController.BlockPlayersInput(false);
        CursorLocking.LockCursor(true);
    }
 
}
