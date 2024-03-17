using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CourseProgressTracking : MonoBehaviour
{
    [SerializeField]
    private UINavigation uiNavigation;
    [SerializeField]
    private PowerControl jumpControl;

    [SerializeField]
    private Slider progressSlider;
    [SerializeField]
    private TextMeshProUGUI progressText;

    private Vector3 playerStartPosition;
    private int distanceLenght;

    string metrInterText;

    private void OnEnable()
    {
        CourseProgress.EnteringCourse += OnEnteringCourse;
        CourseProgress.ExitingCourse += OnExitingCourse;
        CourseProgress.RunningCourse += OnRunningCourse;
    }

    private void OnDisable()
    {
        CourseProgress.EnteringCourse -= OnEnteringCourse;
        CourseProgress.ExitingCourse -= OnExitingCourse;
        CourseProgress.RunningCourse -= OnRunningCourse;
    }

    private void Start()
    {
        if (Language.Instance.languageName == LanguageName.Rus)
            metrInterText = "ì";
        else metrInterText = "m";
    }
    private void OnEnteringCourse(int distance, Transform transform)
    {
        playerStartPosition = transform.position;
        playerStartPosition.y = 0;
        uiNavigation.ToggleCourseProgressCanvas(true);
        distanceLenght = distance;
        UpdateProgressText(0);
    }
    private void OnRunningCourse(Vector3 currentPosition)
    {
        int currentDistance = (int)Vector3.Distance(currentPosition, playerStartPosition);
        UpdateSliderValue(currentDistance);
        UpdateProgressText(currentDistance);
    }
    private void OnExitingCourse()
    {
        uiNavigation.ToggleCourseProgressCanvas(false);
    }

    void UpdateSliderValue(int currentValue)
    {
        progressSlider.value = (float)currentValue / distanceLenght;

    }

    private void UpdateProgressText(int currentValue)
    {
        progressText.text = $"{currentValue * jumpControl.GetLevelKoeficient() / 10} {metrInterText}";
    }

}



