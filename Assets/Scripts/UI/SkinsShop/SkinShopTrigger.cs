using Cinemachine;
using UnityEngine;

public class SkinShopTrigger : MonoBehaviour
{
    [SerializeField]
    private SkinShop skinShop;
    [SerializeField]
    private Transform viewPoint;
    [SerializeField]
    private Transform viewPoint_opposite;

    [SerializeField]
    private CinemachineVirtualCamera shopCamera;
    [SerializeField]
    private GameObject triggerVisual;
    private Transform playerTransform;

    private bool rotateToggle;

    private void Awake()
    {
        shopCamera.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            skinShop.OpenSkinShop();
            ToggleSkinShopView(true);
            playerTransform = other.transform;
            MovePlayerToPoint(playerTransform);
        }
    }

    void MovePlayerToPoint(Transform playerTransform)
    {
        playerTransform.position = viewPoint.position;
        RotatePlayer();
        Vector3 cameraProjectionPos = shopCamera.transform.position;
        cameraProjectionPos.y = 0;
        playerTransform.LookAt(cameraProjectionPos);
        playerTransform.rotation = Quaternion.Euler(0, viewPoint.localRotation.eulerAngles.y, 0);
    }


    public void ToggleSkinShopView(bool state)
    {
        shopCamera.enabled = state;
        triggerVisual.SetActive(!state);
    }

    public void RotatePlayer()
    {
        rotateToggle = !rotateToggle;
        if (rotateToggle)
            playerTransform.GetComponent<Rigidbody>().
                MoveRotation(Quaternion.Euler(viewPoint.localRotation.eulerAngles));
        else playerTransform.GetComponent<Rigidbody>().
                MoveRotation(Quaternion.Euler(viewPoint_opposite.localRotation.eulerAngles));

    }
}
