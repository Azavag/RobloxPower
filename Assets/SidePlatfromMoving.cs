using DG.Tweening;
using UnityEngine;

public class SidePlatformMoving : MonoBehaviour
{
    [Header("Объект")]
    [SerializeField]
    private Transform objectTransform;

    [Header("Анимация")]
    [SerializeField]
    private Transform targetPoint;
    private Transform startPoint;
    [SerializeField]
    private float moveForwardAnimDuration;
    [SerializeField]
    private Ease moveForwardEase;
    [SerializeField]
    private float moveBackwardAnimDuration;
    [SerializeField]
    private Ease moveBackwardEase;
    [SerializeField]
    private float breakInterval;
    float breakTimer;

    bool isObjectsMoving;
    Tween objectForwardMove;
    Tween objectBackwardMove;
    void Start()
    {
        startPoint = objectTransform;
        objectForwardMove = MoveTween(objectTransform, targetPoint.position, moveForwardAnimDuration, moveForwardEase);
        objectForwardMove.OnComplete(StartMoveBackward);
        objectBackwardMove = MoveTween(objectTransform, startPoint.position, moveBackwardAnimDuration, moveBackwardEase);
        objectBackwardMove.OnComplete(OnMovingEnd);

        ResetTimer();
    }

   
    private void OnDestroy()
    {
        objectTransform.DOKill();
    }

    void Update()
    {
        if (isObjectsMoving)
            return;

        breakTimer -= Time.deltaTime;
        if (breakTimer <= 0)
        {
            StartMoveForward();
        }
    }
 

    Tween MoveTween(Transform objectTransform, Vector3 targetPoint, float animTime, Ease ease)
    {
        Tween moveTween = objectTransform.DOMove(targetPoint, animTime);
        moveTween.SetEase(ease);
        return moveTween;
    }
    void StartMoveForward()
    {
        isObjectsMoving = true;
        objectForwardMove.Play();

    }
    void StartMoveBackward()
    {
        objectBackwardMove.Rewind();
        objectBackwardMove.Play();
    }
    void OnMovingEnd()
    {
        isObjectsMoving = false;
        objectForwardMove.Rewind();
        ResetTimer();
    }
    void ResetTimer()
    {
        breakTimer = breakInterval;
    }

}
