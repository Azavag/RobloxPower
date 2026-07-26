using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraSensivityControl : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook freeLookCamera;
    [SerializeField]
    private Slider sensivitySlider;

    float sensMultiplier = 1f;
    float startCameraXAxisSpeed;
    float startCameraYAxisSpeed;
    float newCameraXAxisSpeed;
    float newCameraYAxisSpeed;
    bool isCameraEnabled = true;
    bool baseSpeedCached;

    private void Awake()
    {
        sensivitySlider.minValue = 0.01f;
        sensivitySlider.maxValue = 2.0f;
        CacheBaseSpeed();
    }

    private void OnEnable()
    {
        sensivitySlider.onValueChanged.AddListener(OnSliderValueChanged);
        CacheBaseSpeed();
    }

    private void OnDisable()
    {
        sensivitySlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void Start()
    {
        sensMultiplier = Mathf.Clamp(
            Bank.Instance.playerInfo.sensivityValue,
            sensivitySlider.minValue,
            sensivitySlider.maxValue);

        Bank.Instance.playerInfo.sensivityValue = sensMultiplier;
        ApplySensivity();
        EnableCamera();
        sensivitySlider.SetValueWithoutNotify(sensMultiplier);
        ResetCameraPosition();
    }

    public void OnSliderValueChanged(float newValue)
    {
        sensMultiplier = newValue;
        Bank.Instance.playerInfo.sensivityValue = sensMultiplier;
        ApplySensivity();
    }

    void CacheBaseSpeed()
    {
        if (baseSpeedCached || freeLookCamera == null)
            return;

        float x = freeLookCamera.m_XAxis.m_MaxSpeed;
        float y = freeLookCamera.m_YAxis.m_MaxSpeed;

        // Не кэшируем нули: DisableCamera / FadeScreen уже могли обнулить MaxSpeed.
        if (x <= 0f && y <= 0f)
            return;

        startCameraXAxisSpeed = x;
        startCameraYAxisSpeed = y;
        baseSpeedCached = true;
    }

    void EnsureBaseSpeed()
    {
        CacheBaseSpeed();
        if (baseSpeedCached)
            return;

        // Fallback, если к моменту первого Apply FreeLook уже обнулён.
        startCameraXAxisSpeed = 1000f;
        startCameraYAxisSpeed = 5f;
        baseSpeedCached = true;
    }

    void ApplySensivity()
    {
        EnsureBaseSpeed();
        newCameraXAxisSpeed = startCameraXAxisSpeed * sensMultiplier;
        newCameraYAxisSpeed = startCameraYAxisSpeed * sensMultiplier;
        if (isCameraEnabled)
            ApplyCameraSpeed();
    }

    void ApplyCameraSpeed()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = newCameraXAxisSpeed;
        freeLookCamera.m_YAxis.m_MaxSpeed = newCameraYAxisSpeed;
    }

    public void DisableCamera()
    {
        CacheBaseSpeed();
        isCameraEnabled = false;
        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0;
    }

    public void EnableCamera()
    {
        isCameraEnabled = true;
        ApplySensivity();
    }

    public void ResetCameraPosition()
    {
        freeLookCamera.m_YAxis.Value = 0.5f;
        freeLookCamera.m_XAxis.Value = 0f;
    }
}
