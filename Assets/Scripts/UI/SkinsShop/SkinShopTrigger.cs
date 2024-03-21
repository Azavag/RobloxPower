using Cinemachine;
using UnityEngine;

public class SkinShopTrigger : MonoBehaviour
{
    [SerializeField] SkinShop skinShop;
    [SerializeField] Transform viewPoint;
    [SerializeField] CinemachineVirtualCamera shopCamera;
    [SerializeField] GameObject triggerVisual;
    private void Awake()
    {
        shopCamera.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            skinShop.OpenSkinShop();
            ToggleSkinShopView(true);
            MovePlayerToPoint(other.transform);
        }
    }

    void MovePlayerToPoint(Transform playerTransform)
    {
        playerTransform.position = viewPoint.position;
        Vector3 cameraProjectionPos = shopCamera.transform.position;
        cameraProjectionPos.y = 0;
        playerTransform.LookAt(cameraProjectionPos);
        playerTransform.rotation = Quaternion.Euler(0, playerTransform.rotation.eulerAngles.y, 0);
    }

   

    public void ToggleSkinShopView(bool state)
    {
        shopCamera.enabled = state;
        triggerVisual.SetActive(!state);
    }


}
