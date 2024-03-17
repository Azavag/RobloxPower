using Cinemachine;
using UnityEngine;

public class MobileController : MonoBehaviour
{
    YandexSDK yandexSDK;
    public string deviceType;
    [SerializeField]
    private CinemachineFreeLook freeLookCamera;
    [SerializeField]
    private Joystick cameraJoystick;

    public static MobileController instance;
    private void Awake()
    {
        yandexSDK = FindObjectOfType<YandexSDK>();
        yandexSDK.LoadDeviceInfo();

    }
    private void OnEnable()
    {

#if !UNITY_EDITOR
        deviceType = yandexSDK.GetDeviceType();
#endif
        if (deviceType == "mobile" || deviceType == "tablet")
        {
            IsMobileController.IsMobile = true;
            ResetCameraAxisName();
        }
        else
        {
            IsMobileController.IsMobile = false;
        }
    }

    void Start()
    {

    }
    private void Update()
    {
        if (!IsMobileController.IsMobile)
            return;
        freeLookCamera.m_XAxis.m_InputAxisValue = cameraJoystick.input.x/3;
        freeLookCamera.m_YAxis.m_InputAxisValue = cameraJoystick.input.y/5;
    }

    void ResetCameraAxisName()
    {
        freeLookCamera.m_XAxis.m_InputAxisName = "";
        freeLookCamera.m_YAxis.m_InputAxisName = "";
    }

}
