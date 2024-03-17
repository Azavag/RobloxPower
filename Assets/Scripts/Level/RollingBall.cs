using DG.Tweening;
using UnityEngine;

public class RollingBall : MonoBehaviour
{
    [SerializeField]
    private Transform ballTransform;   
    [SerializeField]
    private Transform endPosition;
    [SerializeField]
    private float moveAnimTime;
    [SerializeField]
    private float rotateAnimTime;

    Tween moveBall;
    Tween rotationBall;

    public bool IsReadyToRoll;
    void Start()
    {
        DeactivateBall();
        rotationBall = RotationTween();
        moveBall = MovingTween();
        moveBall.OnComplete(DeactivateBall);
    }

    private void OnDisable()
    {
        ballTransform.DOKill();
    }

    public void StartRolling()
    {      
        ActivateBall();
        moveBall.Play();
        rotationBall.Play();
    }

    Tween RotationTween()
    {
        Tween rotationTween = ballTransform.DORotate(new Vector3(-360, 0, 0),
            rotateAnimTime, RotateMode.FastBeyond360);
        rotationTween.SetLoops(-1, LoopType.Restart);
        rotationTween.SetEase(Ease.Linear);
        return rotationTween;
    }
    Tween MovingTween()
    {
        Tween moveTween = ballTransform.DOMove(endPosition.position, moveAnimTime);
        moveTween.SetEase(Ease.Linear);
        return moveTween;
    }
    void DeactivateBall()
    {
        ballTransform.GetComponent<MeshRenderer>().enabled = false;
        ballTransform.GetComponent<SphereCollider>().enabled = false;
        ballTransform.DORewind();
        IsReadyToRoll = true;
    }
    void ActivateBall()
    {
        IsReadyToRoll = false;
        ballTransform.GetComponent<MeshRenderer>().enabled = true;
        ballTransform.GetComponent<SphereCollider>().enabled = true;
    }

    
}
