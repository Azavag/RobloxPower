using DG.Tweening;
using UnityEngine;

public class RollSphereAnimation : MonoBehaviour
{
    [SerializeField]
    private float rollAnimDuration;
    [SerializeField]
    private Transform sphereTransform;
    Tween rotateTween;
  
    public void StartRoll()
    {
        rotateTween = sphereTransform.DOLocalRotate(
         new Vector3(0, 0, -360), rollAnimDuration, RotateMode.FastBeyond360);
        rotateTween.Play();
    }

    private void OnDestroy()
    {       
        rotateTween.Pause();
        rotateTween.Kill();
    }
}
