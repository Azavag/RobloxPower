using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraSensivityControl : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook freeLookCamera;
    [SerializeField]
    private Slider sensivitySlider;

    float sensMultiplier = 0.5f;
    float startCameraXAxisSpeed;
    float startCameraYAxisSpeed;
    float newCameraXAxisSpeed;
    float newCameraYAxisSpeed;

    private void OnEnable()
    {
        sensivitySlider.onValueChanged.AddListener(OnSliderValueChanged);
        startCameraXAxisSpeed = freeLookCamera.m_XAxis.m_MaxSpeed;
        startCameraYAxisSpeed = freeLookCamera.m_YAxis.m_MaxSpeed;
    }
    private void OnDisable()
    {
        sensivitySlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void Awake()
    {
        sensivitySlider.minValue = 0.01f;
        sensivitySlider.maxValue = 2.0f;
    }
    private void Start()
    {        
        sensMultiplier = Bank.Instance.playerInfo.sensivityValue;
        sensivitySlider.value = sensMultiplier;
        EnableCamera();
    }
    public void OnSliderValueChanged(float newValue)
    {
        sensMultiplier = newValue;
        Bank.Instance.playerInfo.sensivityValue = sensMultiplier;
        ChangeCameraSensivity();
    }

    void ChangeCameraSensivity()
    {
        newCameraXAxisSpeed = startCameraXAxisSpeed * sensMultiplier;
        newCameraYAxisSpeed = startCameraYAxisSpeed * sensMultiplier;
    }

    public void DisableCamera()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0;
    }
    public void EnableCamera()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = newCameraXAxisSpeed;
        freeLookCamera.m_YAxis.m_MaxSpeed = newCameraYAxisSpeed;
    }
}
