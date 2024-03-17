using DG.Tweening;
using UnityEngine;

public class OrderedMovingBarriers : MonoBehaviour
{
    [SerializeField]
    private GameObject[] platformObjects;
    [SerializeField]
    private GameObject endPosition;

    [SerializeField]
    private float moveAnimTime;
    private int currentPlatformNumber = -1;

    [SerializeField]
    private float breakInterval;
    private float breakTimer;


    void Start()
    {
        ResetTimer();
        for (int i = 0; i < platformObjects.Length; i++)
        {
            platformObjects[i].GetComponentInChildren<MeshRenderer>().enabled = false;
            platformObjects[i].GetComponentInChildren<Collider>().enabled = false;
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject t in platformObjects)
        {
            t.transform.DOPause();
            t.transform.DOKill();
        }
    }

    void Update()
    {
        breakTimer -= Time.deltaTime;
        if (breakTimer <= 0)
        {
            ChooseNextPlatform();
            StartMovePlatfrom(currentPlatformNumber);
            ResetTimer();
        }
    }

    void ChooseNextPlatform()
    {
        currentPlatformNumber++;
        if (currentPlatformNumber == platformObjects.Length)
            currentPlatformNumber = 0;
    }
    void StartMovePlatfrom(int number)
    {

        Tween tempTween = MovingTween(platformObjects[number], endPosition);
        tempTween.OnComplete(
            delegate { DeactivatePlatform(platformObjects[number]); });

        ActivatePlatform(platformObjects[number]);
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
        platformObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        platformObject.GetComponentInChildren<Collider>().enabled = false;
        platformObject.transform.DORewind();
        platformObject.transform.DOKill();
        
    }
    void ActivatePlatform(GameObject platformObject)
    {
        platformObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        platformObject.GetComponentInChildren<Collider>().enabled = true;    
    }

    void ResetTimer()
    {
        breakTimer = breakInterval;
    }
}
