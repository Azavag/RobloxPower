using DG.Tweening;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [Header("ShakeAnimation")]
    [SerializeField]
    static private float shakeDuration = 0.4f;
    [SerializeField]
    static private Vector3 shakeVector = new Vector3(10,0,0);


    public static Tween ShakeAnimation(Transform objectTransform)
    {
        return objectTransform.DOShakePosition(shakeDuration, shakeVector, randomness:0);
    }
   
}
