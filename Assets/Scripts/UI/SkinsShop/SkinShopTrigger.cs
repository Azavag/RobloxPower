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

    private bool isShowingBack;

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
        ResetRotationState();
        ApplyFacingRotation(false);
        Vector3 cameraProjectionPos = shopCamera.transform.position;
        cameraProjectionPos.y = 0;
        playerTransform.LookAt(cameraProjectionPos);
        ApplyFacingRotation(false);
    }


    public void ToggleSkinShopView(bool state)
    {
        shopCamera.enabled = state;
        triggerVisual.SetActive(!state);
    }

    public void SetBagsView(bool showBack)
    {
        if (playerTransform == null || isShowingBack == showBack)
            return;

        isShowingBack = showBack;
        ApplyFacingRotation(showBack);
    }

    public void ResetRotationState()
    {
        isShowingBack = false;
    }

    void ApplyFacingRotation(bool showBack)
    {
        Transform point = showBack ? viewPoint_opposite : viewPoint;
        float y = point.localRotation.eulerAngles.y;
        var rotation = Quaternion.Euler(0f, y, 0f);
        playerTransform.GetComponent<Rigidbody>().MoveRotation(rotation);
        playerTransform.rotation = rotation;
    }
}
