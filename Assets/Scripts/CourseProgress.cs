using System;
using UnityEngine;

public class CourseProgress : MonoBehaviour
{
    public static event Action<int, Transform> EnteringCourse;
    public static event Action< Vector3> RunningCourse;
    public static event Action ExitingCourse;

    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform finishPoint;
    int distanceLenght;

    // Start is called before the first frame update
    void Start()
    {
        distanceLenght = (int)Vector3.Distance(startPoint.position, finishPoint.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EnteringCourse?.Invoke(distanceLenght, other.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RunningCourse?.Invoke(other.transform.position);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExitingCourse?.Invoke();
        }
    }


}
