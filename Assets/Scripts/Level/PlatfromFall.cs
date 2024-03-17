using DG.Tweening;
using UnityEngine;

public class PlatfromFall : MonoBehaviour
{
    [SerializeField]
    private float fallAnimTime;
    [SerializeField]
    private Ease ease;

    private float startYPos = 0;
    private float endYPos = -8f;

    void Start()
    {
        startYPos = transform.localPosition.y;
    }

    
    Tween PlatformFallTween(float Ypos)
    {
        Tween fallTween = transform.DOLocalMoveY(Ypos, fallAnimTime);
        fallTween.SetAutoKill();
        fallTween.SetEase(ease);
        return fallTween;
    }

    void MoveDownAnimation()
    {
        Tween moveDown = PlatformFallTween(endYPos);
        moveDown.OnComplete(MoveUpAnimation);
        moveDown.Play();
    }
    void MoveUpAnimation()
    {
        Tween moveUp = PlatformFallTween(startYPos);
        moveUp.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {           
            MoveDownAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}
