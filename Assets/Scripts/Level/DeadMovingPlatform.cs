using DG.Tweening;
using UnityEngine;

public class DeadMovingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject[] platformObject;
    [SerializeField]
    private GameObject[] endPositions;

    [SerializeField]
    private float moveAnimTime;
    int currentPlatformNumber = -1;

    [SerializeField]
    private float breakInterval;
    private float breakTimer;
    bool isPlatformMoving;

    [SerializeField]
    private bool isPlatformSphere;

    void Start()
    {       
        ResetTimer();
        for (int i = 0; i < platformObject.Length; i++)
        {
            platformObject[i].GetComponent<MeshRenderer>().enabled = false;
            platformObject[i].GetComponent<Collider>().enabled = false;
        }       
    }

    private void OnDestroy()
    {
        foreach (GameObject t in platformObject)
        {
            t.transform.DOPause();
            t.transform.DOKill();
        }
    }

    void Update()
    {
        if (isPlatformMoving)
            return;

        breakTimer -= Time.deltaTime;
        if(breakTimer <= 0)
        {
            ChooseNextPlatform();
            StartMovePlatfrom(currentPlatformNumber);
        }
    }

    void ChooseNextPlatform()
    {       
        currentPlatformNumber++;
        if(currentPlatformNumber == platformObject.Length)
            currentPlatformNumber = 0;
    }
    void StartMovePlatfrom(int number)
    {
       
        Tween tempTween = MovingTween(platformObject[number], endPositions[number]);
        tempTween.OnComplete(
            delegate { DeactivatePlatform(platformObject[number]); });

        isPlatformMoving = true;
        ActivatePlatform(platformObject[number]);
        tempTween.Play();
    }
    Tween MovingTween(GameObject platfromObject, GameObject endPosition)
    {
        Tween moveTween = platfromObject.transform.DOMove(endPosition.transform.position, moveAnimTime);
        moveTween.SetEase(Ease.Linear);
        return moveTween;
    }
    void DeactivatePlatform(GameObject platformObject)
    {
        platformObject.GetComponent<MeshRenderer>().enabled = false;
        platformObject.GetComponent<Collider>().enabled = false;
        platformObject.transform.DORewind();
        platformObject.transform.DOKill();
        isPlatformMoving = false;
        ResetTimer();
    }
    void ActivatePlatform(GameObject platformObject)
    {
        platformObject.GetComponent<MeshRenderer>().enabled = true;
        platformObject.GetComponent<Collider>().enabled = true;
        if (isPlatformSphere)
        {
            platformObject.transform.parent.GetComponent<RollSphereAnimation>().StartRoll();
        }
    }
   
    void ResetTimer()
    {
        breakTimer = breakInterval;
    }
}
