using DG.Tweening;
using UnityEngine;

public class PunchbagAnimation : MonoBehaviour
{

    [SerializeField]
    private Transform objectTransform;
    [Header("ScaleAnimaton")]
    [SerializeField]
    private float targetScaleSize;
    [SerializeField]
    private float scaleDuration;
    [SerializeField]
    private Ease scaleEase;
    [Header("ShakeAnimaton")]
    [SerializeField]
    private Vector3 targetShakeVector;
    [SerializeField]
    private float shakeDuration;
    [SerializeField]
    private float shakeRandomness;
    [SerializeField]
    private Ease shakeEase;
    void Start()
    {
        SetScaleAnimation();
        //SetShakeScaleTween();
    }

    private void OnDestroy()
    {
        objectTransform.DOKill();
    }

    private void Update()
    {

    }
    public void PlayHitAnimation()
    {
        objectTransform.DORewind();
        objectTransform.DOPlay();            
    }
   
    void SetScaleAnimation()
    {     
        Vector3 smallScaleVector = objectTransform.transform.localScale * targetScaleSize;
        objectTransform.DOScale(smallScaleVector, scaleDuration)
          .SetEase(scaleEase)
          .SetLoops(2, LoopType.Yoyo);
    }


}
