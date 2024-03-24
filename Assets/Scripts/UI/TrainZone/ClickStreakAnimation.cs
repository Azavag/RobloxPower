using DG.Tweening;
using UnityEngine;
using TMPro;



public class ClickStreakAnimation : MonoBehaviour
{
    [SerializeField]
    private Transform streakText;

    [Header("Animation Settings")]
    [Header("Rotation Animation")]
    Sequence shakeSequence;
    [SerializeField]
    private float rotationDuration;
    [SerializeField]
    private Ease rotationEase;
    [Header("Scale Animation")]
    [SerializeField]
    private float scaleDuration;
    [SerializeField]
    private float TextTargetScale;
    [SerializeField]
    private Ease scaleEase;

    private int lastMultiplierStreak;

    Tween scaleTween;
    Tween rotateAroundTween;

    void Start()
    {
        CreateSwapTweens(streakText, TextTargetScale);      
        CreateRotationTweenSeq(streakText);
        ShakeStreakText(true);
    }

    private void OnDestroy()
    {
        shakeSequence.Kill();
        rotateAroundTween.Kill();
        scaleTween.Kill();

    }
    public void ActivateStreakText(int multiplierValue)
    {
        if (lastMultiplierStreak == multiplierValue)
            return;
        lastMultiplierStreak = multiplierValue;

        streakText.GetComponent<TextMeshProUGUI>().text = $"x{multiplierValue}";
        PlaySwatTweens();
    }
    void PlaySwatTweens()
    {
        ShakeStreakText(false);
        scaleTween.Restart();
        rotateAroundTween.Restart();
        rotateAroundTween.OnComplete(() => { ShakeStreakText(true); });
    }
    void ShakeStreakText(bool state)
    {
        if (state)
            shakeSequence.Play();
        else shakeSequence.Pause();
    }
  
    void CreateRotationTweenSeq(Transform objectTransform)
    {
        shakeSequence = DOTween.Sequence();

        shakeSequence.Append(objectTransform.DORotate(new Vector3(0, 0, 25), rotationDuration));
        shakeSequence.Append(objectTransform.DORotate(new Vector3(0, 0, 0), rotationDuration));
        shakeSequence.Append(objectTransform.DORotate(new Vector3(0, 0, -25), rotationDuration));
        shakeSequence.Append(objectTransform.DORotate(new Vector3(0, 0, 0), rotationDuration));
        shakeSequence.SetLoops(-1, LoopType.Restart)
            .SetEase(rotationEase);
    }

    void CreateSwapTweens(Transform objectTransform, float targetScale)
    {
        scaleTween = objectTransform.DOScale(targetScale, scaleDuration/2)
            .SetEase(scaleEase)
            .SetLoops(2, LoopType.Yoyo);
        rotateAroundTween = streakText.DORotate(new Vector3(0, 0, 720), scaleDuration, RotateMode.FastBeyond360)
           .SetEase(scaleEase);
    }

}
