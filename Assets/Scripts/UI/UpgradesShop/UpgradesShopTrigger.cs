using UnityEngine;

public class UpgradesShopTrigger : MonoBehaviour
{
    [SerializeField]
    private UpgradesShop upgradesShop;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            upgradesShop.OpenUpgradeShop();
        }
    }
}
