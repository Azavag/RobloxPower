using DG.Tweening;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Header("Объект")]
    [SerializeField]
    private Transform objectTransform;

    [Header("Анимация")]
    [SerializeField]
    private Vector3 targetPoint;
    private Vector3 startPoint;
    [SerializeField]
    private float moveForwardAnimTime;
    [SerializeField]
    private Ease moveForwardEase;
    [SerializeField]
    private float moveBackwardAnimTime;
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
        startPoint = objectTransform.localPosition;
        objectForwardMove = MoveTween(objectTransform, targetPoint, moveForwardAnimTime, moveForwardEase);
        objectForwardMove.OnComplete(StartMoveBackward);
        objectBackwardMove = MoveTween(objectTransform, startPoint, moveBackwardAnimTime, moveBackwardEase);
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
        if(breakTimer <= 0)
        {
            StartMoveForward();
        }
    }
    Tween MoveTween(Transform objectTransform, Vector3 point ,float animTime, Ease ease)
    {
        Tween moveTween = objectTransform.DOLocalMove(point, animTime);
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
